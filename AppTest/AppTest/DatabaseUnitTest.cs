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
            //testBookInformationDatabase(container);
            testShelfPositonDatabase(container);
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
        public static void testShelfPositonDatabase(IUnityContainer container)
        {
            IBookLocationService locationService = container.Resolve<IBookLocationService>();
            locationService.ServerIp = "DESKTOP-PSQP38H";
            locationService.ServerUsername = "sa";
            locationService.ServerPassword = "19831122";

            List<String> shelfRfidList = new List<string>();
            shelfRfidList.Add("6W22A0304");
            shelfRfidList.Add("6W22A0305");
            shelfRfidList.Add("3W01A0103");
            shelfRfidList.Add("3W01A0503");
            Console.WriteLine("Shelf Rfid List:");
            displayList(shelfRfidList);

            List<String> shelfNameList = new List<String>();
            foreach(String item in shelfRfidList){
                String name=locationService.getShelfNameByShelfRfid(item);
                shelfNameList.Add(name);
            }
            Console.WriteLine("Shelf Name List:");
            displayList(shelfNameList);

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
