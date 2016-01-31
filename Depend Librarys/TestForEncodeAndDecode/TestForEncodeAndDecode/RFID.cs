using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Runtime.InteropServices;
using ReaderComSDK;

namespace TestForEncodeAndDecode
{
    class RFID
    {
        public byte defaultAddress = 0xff;
        public byte defaultRFPower;
        public int delayWhenReadNothing = 1000;
        public string serialInterface = "COM3";
        public int serialSpeed = 115200;
        public const int maxTagsAtOneRead = 10;
        public ReaderComSDK.comsdk comsdk = new ReaderComSDK.comsdk();

        public void openSerialPort()
        {
            int status = 1;
            
            status = comsdk.com_Connect(serialInterface, serialSpeed);
            while (status != 0)
            {
                //Thread.Sleep(delayWhenReadNothing);  //可能有问题
                //status = comsdk.com_Connect(serialInterface, serialSpeed);
                throw new Exception("serial open failure !");
            }
            byte defpower=20;
            byte defstand=0;
            //comsdk.set_RF(defaultAddress, defpower, defstand);
            //comsdk.reset_Reader(defaultAddress);
            Thread.Sleep(1000);
            comsdk.get_Rf(defaultAddress,out defpower,out defstand);
            Console.WriteLine("power if:" + defpower + "   stand is:" + defstand);
            //Console.ReadLine();
            
        }
        public Dictionary<string,string> readTags()
        {
            int status = 1;
            //List<string> tagList = new List<string>();
            Dictionary<string, string> tagList = new Dictionary<string, string>(); 
            //不管读到的是图书还是层架标签，都存储在这里，key为解码后的RFID号，value为类型，比如book或者shelf。
            tagstruct[] tagArray = new tagstruct[maxTagsAtOneRead];
            comsdk.clear_ID_Buff(defaultAddress);
            status = comsdk.gen2_Multi_Identify(defaultAddress, out tagArray);
            //status my return 1,mean read error of nothing to read ,can distrinct it ,i dont know
            while (status == 1)
            {
                Thread.Sleep(delayWhenReadNothing);
                status = comsdk.gen2_Multi_Identify(defaultAddress, out tagArray);
            }

            for (int i = 0; i < tagArray.Length; i++)
            {
                //this code should examine
                //string tagDataString=rawDataToString(tagArray[i].TagData);
                byte[] userData = new byte[16];  //解析后，不管是层架标签还是图书，最长为16位
                string decodeData = "";
                //读取的RFID标签为12个比特的数据，书的标签和层架标签解析的起始位不同
                //Array.Copy(tagArray[i].TagData, 4, userData, 0, 8);    //for book only
                //Array.Copy(tagArray[i].TagData, 2, userData, 0, 10);   //for shief only 
                if (isRFIDBook(tagArray[i].TagData))
                {
                    //Console.WriteLine("book");
                    Array.Copy(tagArray[i].TagData, 4, userData, 0, 8);
                    decodeData = DecodedOfBookRfid(userData, 8);
                    tagList.Add(decodeData,"book");
                }
                if(isShelfRFID(tagArray[i].TagData))
                {
                    //Console.WriteLine("shelf");
                    Array.Copy(tagArray[i].TagData, 2, userData, 0, 10);   //for shief only 
                    decodeData = DecodedOfShelfRfid(userData, 10);
                    tagList.Add(decodeData,"shelf");
                }     
            }
            return tagList;

        }
        public void closeSerialPort()
        {
            comsdk.com_DisConnect(serialInterface);
        }
        private string isBoookOrShelf(byte[] rawData,int len)
        { 
            //判断rfid是什么类型的，是图书还是层架标签，分别返回"book"或者"shelf",如果返回"nothing"则意味着错误
            //rawData[] 是原始，未经过处理的rfid标签数据，由于使用的是超高频标签，长度都为12个比特
            //图书的rfid标签数据从第四个比特位开始，而层架标签的数据位从第二个比特开始，分别解析长度，根据解析的长度判断
            //属于那种类型的层架标签，但也可能出现错误的情况 ，这个函数属于可能出错的函数
            byte bitOfBook = rawData[4];
            byte bitOfShelf = rawData[2];
            int bookCodeLength = (UInt16)(bitOfBook & 0x0F);
            int shelfCodeLength = (UInt16)(bitOfShelf);
            //Console.WriteLine("in book len:" + bookCodeLength);
            //Console.WriteLine("in shelf len:" + shelfCodeLength);
            //解码后，发现大量的rfid标签，不管是层架标签还是图书标签，解码后的长度都是9位，因此9只是个经验性的数值
            //注意！！！！，9这个数值可能在某种情况下，即增加编码长度后可能会出错，现在来看还是正确的
            //if (bookCodeLength > 0 && bookCodeLength <= len)
            if(bookCodeLength==9)
            {
                return "book";
            }
            //if (shelfCodeLength > 0 && shelfCodeLength <= len)
            if(shelfCodeLength==9)
            {
                return "shelf";
            }

            return "nothing";
        }
        public bool isShelfRFID(byte[] rawData)
        {
            string bookString = "shelf";
            if (bookString.Equals(isBoookOrShelf(rawData, 16)))  //16为解码后的最长长度
            {
                return true;
            }
            return false;
        }
        public Boolean isRFIDBook(byte[] rawData)
        {
            string bookString = "book";
            if (bookString.Equals(isBoookOrShelf(rawData, 16)))  //16为解码后的最长长度
            {
                return true;
            }
            return false;
        }

        //other dll function
        //[DllImport("C:\\Users\\he_yu\\Documents\\Visual Studio 2013\\Projects\\BLMandLibrary\\EncodeAndDecode\\Debug\\EncodeAndDecode.dll", CallingConvention = CallingConvention.Cdecl)]
        [DllImport("D:\\files\\文档\\Visual Studio 2013\\Projects\\BLMandLibrary\\EncodeAndDecode\\Debug\\EncodeAndDecode.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern int decodeBookRfid(byte[] msg, int len, StringBuilder outMsg);
        static string DecodedOfBookRfid(byte[] code, int codeLen)
        {  //code is  byte  array,codeLen is length of code array
            int decodedLength = 0;
            StringBuilder decodedString = new StringBuilder(15);
            decodedLength = decodeBookRfid(code, codeLen, decodedString);
            return decodedString.ToString();
        }
        [DllImport("D:\\files\\文档\\Visual Studio 2013\\Projects\\BLMandLibrary\\EncodeAndDecode\\Debug\\EncodeAndDecode.dll", CallingConvention = CallingConvention.Cdecl)]
        //[DllImport("C:\\Users\\he_yu\\Documents\\Visual Studio 2013\\Projects\\BLMandLibrary\\EncodeAndDecode\\Debug\\EncodeAndDecode.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern int decodeShelfRfid(byte[] msg, int len, StringBuilder outMsg);
        static string DecodedOfShelfRfid(byte[] code, int codeLen)
        {  //code is  byte  array,codeLen is length of code array
            int decodedLength = 0;
            StringBuilder decodedString = new StringBuilder(16);
            decodedLength = decodeShelfRfid(code, codeLen, decodedString);
            return decodedString.ToString();
        }
    }
}
