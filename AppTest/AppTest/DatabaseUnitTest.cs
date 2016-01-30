using Infrastructure;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppTest
{
    class DatabaseUnitTest
    {
        public static void testAllDatabase(IUnityContainer container)
        {
            testBookInformationDatabase(container);

            Console.ReadKey();
        }
        public static void displayList(List<String> stringList)
        {
            foreach (String item in stringList)
            {
                Console.Write(item+" ");
            }
            Console.WriteLine("");
        }
        public static void testBookInformationDatabase(IUnityContainer container)
        {
            IBookInformationService bookInformationService = container.Resolve<IBookInformationService>();
            bookInformationService.ServerIp = "DESKTOP-PSQP38H";
            bookInformationService.ServerUsername = "sa";
            bookInformationService.ServerPassword = "19831122";

            List<String> bookRfidList = new List<string>();
            bookRfidList.Add("200007857");
            bookRfidList.Add("200019381");
            bookRfidList.Add("200020316");
            bookRfidList.Add("300010248");
            Console.WriteLine("Book RFID List:");
            displayList(bookRfidList);

            List<String> bookAccessCodeList = bookInformationService.getBookAccessCodeListByRfidList(bookRfidList);
            Console.WriteLine("Book Access Code List:");
            displayList(bookAccessCodeList);

            List<String> bookNameList = bookInformationService.getBookNameListByRfidList(bookRfidList);
            Console.WriteLine("Book Name List:");
            displayList(bookNameList);
        }
    }
}
