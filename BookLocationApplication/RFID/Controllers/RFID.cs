using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReaderComSDK;
using System.Threading;
using System.Runtime.InteropServices;

namespace RFID
{
    class OpenRFIDDeviceException : Exception { }  //自定义异常类
    class RFIDDevice
    {
        private byte defaultAddress = 0xff;
        private byte defaultRFPower = 20;
        private int delayWhenReadNothing = 200;
        private string serialInterface = "";//COM1
        private int serialSpeed = 115200;
        private const int maxTagsAtOneRead = 10;
        //UserSetting currentUserSetting = new UserSetting();  //全局变量，保存了串口的信息
        private ReaderComSDK.comsdk comsdk = new ReaderComSDK.comsdk();
        public RFIDDevice()
        {
            //currentUserSetting.loadSetting();
            //serialInterface = currentUserSetting.serialPort;
        }
        /***
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
        }
         * **/
        public void openSerialPort(string serialInterface, int serialSpeed)  //逐渐使用这个函数淘汰原来不带参数的openSerialPort函数
        {
            int status = 1;
            this.serialInterface = serialInterface;
            this.serialSpeed = serialSpeed;
            status = comsdk.com_Connect(this.serialInterface, this.serialSpeed);
            while (status != 0)
            {  //当打开串口失败时，抛出异常，上层程序处理该异常
                throw new OpenRFIDDeviceException();
            }
        }
        public Dictionary<String, String> readTags()
        {
            int status = 1;
            //List<string> tagList = new List<string>();
            Dictionary<String, String> tagList = new Dictionary<String, String>();
            //不管读到的是图书还是层架标签，都存储在这里，key为解码后的RFID号，value为类型，比如book或者shelf。
            tagstruct[] tagArray = new tagstruct[maxTagsAtOneRead];
            comsdk.clear_ID_Buff(defaultAddress);
            status = comsdk.gen2_Multi_Identify(defaultAddress, out tagArray);
            //status my return 1,mean read error of nothing to read ,can distrinct it ,i dont know
            /***
            while (status == 1)
            {
                Thread.Sleep(delayWhenReadNothing);
                status = comsdk.gen2_Multi_Identify(defaultAddress, out tagArray);
            }***/
            //上面注释掉的部分是原来的程序，原有的程序只有当读到数据时才返回结果，现在改为不管有没有读到数据都返回
            int readCount=1;
            while ((status == 1)&&(readCount<3)) //一定不能一直读，否则串口会无反应，流量太大了
            {
                //Thread.Sleep(5);//不能睡眠，以免导致冲突
                readCount = readCount + 1; 
                status = comsdk.gen2_Multi_Identify(defaultAddress, out tagArray);
            }
            if (status == 1)
            {
                return tagList;
            }
            //现在的程序改为一直读3次，如果什么都没有读到就返回空的Directory

            for (int i = 0; i < tagArray.Length; i++)
            {
                //this code should examine
                //string tagDataString=rawDataToString(tagArray[i].TagData);
                byte[] userData = new byte[16];  //解析后，不管是层架标签还是图书，最长为16位
                String decodeData = "";
                //读取的RFID标签为12个比特的数据，书的标签和层架标签解析的起始位不同
                //Array.Copy(tagArray[i].TagData, 4, userData, 0, 8);    //for book only
                //Array.Copy(tagArray[i].TagData, 2, userData, 0, 10);   //for shief only 
                if (isBookRFID(tagArray[i].TagData))
                {
                    //Console.WriteLine("book");
                    Array.Copy(tagArray[i].TagData, 4, userData, 0, 8);
                    decodeData = DecodedOfBookRfid(userData, 8);
                    tagList.Add(decodeData, "book");
                }
                if (isShelfRFID(tagArray[i].TagData))
                {
                    //Console.WriteLine("shelf");
                    Array.Copy(tagArray[i].TagData, 2, userData, 0, 10);   //for shief only 
                    decodeData = DecodedOfShelfRfid(userData, 10);
                    tagList.Add(decodeData, "shelf");
                }
            }
            return tagList;

        }
        /***
        public void closeSerialPort()
        {
            comsdk.com_DisConnect(serialInterface);
        }***/
        public void closeSerialPort(string serial)  //用这个函数淘汰原先不带参数的版本 closeSerialPort()
        {
            comsdk.com_DisConnect(serial);
        }
        private string isBoookOrShelf(byte[] rawData, int len)
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
            if (bookCodeLength == 9)
            {
                return "book";
            }
            //if (shelfCodeLength > 0 && shelfCodeLength <= len)
            if (shelfCodeLength == 9)
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
        public Boolean isBookRFID(byte[] rawData)
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
        //[DllImport("D:\\files\\文档\\Visual Studio 2013\\Projects\\BLMandLibrary\\EncodeAndDecode\\Debug\\EncodeAndDecode.dll", CallingConvention = CallingConvention.Cdecl)]
        [DllImport(".\\EncodeAndDecode.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern int decodeBookRfid(byte[] msg, int len, StringBuilder outMsg);
        static string DecodedOfBookRfid(byte[] code, int codeLen)
        {  //code is  byte  array,codeLen is length of code array
            int decodedLength = 0;
            StringBuilder decodedString = new StringBuilder(15);
            decodedLength = decodeBookRfid(code, codeLen, decodedString);
            return decodedString.ToString();
        }
        //[DllImport("D:\\files\\文档\\Visual Studio 2013\\Projects\\BLMandLibrary\\EncodeAndDecode\\Debug\\EncodeAndDecode.dll", CallingConvention = CallingConvention.Cdecl)]
        //[DllImport("C:\\Users\\he_yu\\Documents\\Visual Studio 2013\\Projects\\BLMandLibrary\\EncodeAndDecode\\Debug\\EncodeAndDecode.dll", CallingConvention = CallingConvention.Cdecl)]
        [DllImport(".\\EncodeAndDecode.dll", CallingConvention = CallingConvention.Cdecl)]
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
