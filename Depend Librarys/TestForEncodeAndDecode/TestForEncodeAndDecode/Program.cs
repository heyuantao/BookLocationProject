using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Threading;

namespace TestForEncodeAndDecode
{
    class Program
    {
        /***
        [DllImport("C:\\Users\\he_yu\\Documents\\Visual Studio 2013\\Projects\\EncodeAndDecode\\Debug\\EncodeAndDecode.dll", CallingConvention = CallingConvention.Cdecl)]
        //static extern string rfidEncode96bit(out string scancode);
        //static extern string rfidDecode96bit(out string scancode);
        static extern int decodeRfid(string msg, int len,StringBuilder outMsg);
         * **/
        static void Main(string[] args)
        {
            RFID rfidUtils = new RFID();
            //List<string> newTagList = new List<string>();
            Dictionary<string, string> newTagList = new Dictionary<string, string>();
            rfidUtils.openSerialPort();
            while(true){
                Thread.Sleep(1000);
                newTagList = rfidUtils.readTags();
                Console.WriteLine("tag len:" + newTagList.Count());
                foreach(KeyValuePair<string,string> pair in newTagList){
                    Console.Write("RFID code:"+pair.Key+"   "+"type:"+pair.Value);
                }
                Console.WriteLine();
            }
            rfidUtils.closeSerialPort();
            //StringBuilder outStr=new StringBuilder(15);
            //string strofbyte = System.Text.Encoding.Default.GetString(str);

            //String showString=RFID.DecodedOfBookRfid(str, 8);

            //Console.WriteLine("output is:" + showString);
            Console.ReadKey();
        }



        //other dll function
        /***
        [DllImport("C:\\Users\\he_yu\\Documents\\Visual Studio 2013\\Projects\\BLMandLibrary\\EncodeAndDecode\\Debug\\EncodeAndDecode.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern int decodeBookRfid(string msg, int len, StringBuilder outMsg);        
        static string DecodedOfBookRfid(byte[] code, int codeLen)
        {  //code is  byte  array,codeLen is length of code array
            int decodedLength = 0;
            String rawData = System.Text.Encoding.Default.GetString(code);
            StringBuilder decodedString = new StringBuilder(15);
            decodedLength = decodeBookRfid(rawData, codeLen, decodedString);
            return decodedString.ToString();
        }
        [DllImport("C:\\Users\\he_yu\\Documents\\Visual Studio 2013\\Projects\\BLMandLibrary\\EncodeAndDecode\\Debug\\EncodeAndDecode.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern int decodeShelfRfid(string msg, int len, StringBuilder outMsg);
        static string DecodedOfShelfRfid(byte[] code, int codeLen)
        {  //code is  byte  array,codeLen is length of code array
            int decodedLength = 0;
            String rawData = System.Text.Encoding.Default.GetString(code);
            StringBuilder decodedString = new StringBuilder(16);
            decodedLength = decodeShelfRfid(rawData, codeLen, decodedString);
            return decodedString.ToString();
        }
         * **/
    }
}
