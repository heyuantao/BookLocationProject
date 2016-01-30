using Infrastructure;
using Microsoft.Practices.Unity;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RFID{
    public class RFIDService:IRFIDService
    {
        IUnityContainer container; 
        IEventAggregator eventAggregator;    //the Aggregator for some event,such as scane a tag,stop start scan 
        String hardwareInterface;            //the interface of system ,mybe COM1 COM2...
        String hardwareInterfaceConnectionSpeed; //my be 115200
        RFIDDevice rfidDevice;               //the user custom class
        Boolean backgroundTaskStatus;        //this status indicate the running status of the main task
        Task backgroundTask;                 //reference to the man task
        object rfidLock;
        public RFIDService(IUnityContainer container)
        {
            this.container = container;
            this.eventAggregator = container.Resolve<IEventAggregator>();
            //this.eventAggregator = eventAggregator;
            this.rfidLock = new Object();
            HardwareInterface="";
            HardwareInterfaceConnectionSpeed = "";
            //this.rfidDevice = new RFIDDevice();
            //this.rfidDevice.openSerialPort("COM1",115200); //调试使用
            this.backgroundTask = new Task(mainTask);            
            this.backgroundTaskStatus = false;
            this.backgroundTask.Start();  //让后台开始运行
        }
        public String HardwareInterface { 
            get {return this.hardwareInterface;}
            set {this.hardwareInterface=value;} 
        }
        public String HardwareInterfaceConnectionSpeed{
            get { return this.hardwareInterfaceConnectionSpeed; }
            set { this.hardwareInterfaceConnectionSpeed = value; }
        }
        public void start()
        {
            lock (this.rfidLock)
            {
                if (String.IsNullOrEmpty(HardwareInterface) || HardwareInterfaceConnectionSpeed == "0")
                {//如果发现RFID的串口参数没有配置，则抛出事件
                    eventAggregator.GetEvent<RFIDHardwareEvent>().Publish("串口未设置");
                    return;
                }
                //初始化并打开RFID设备
                if (this.backgroundTaskStatus == false)
                {
                    rfidDevice = new RFIDDevice();
                    try
                    {
                        rfidDevice.openSerialPort(HardwareInterface, int.Parse(HardwareInterfaceConnectionSpeed));
                        //打开串口时可能由于设备设置错误或者未加电导致打开失败
                    }
                    catch (OpenRFIDDeviceException)
                    {
                        eventAggregator.GetEvent<RFIDHardwareEvent>().Publish("串口打开失败");
                        return;
                    }
                    this.backgroundTaskStatus = true;
                }   
            }        
            
        }
        public void stop()
        {
            lock (this.rfidLock)
            {
                this.backgroundTaskStatus = false;
                //Thread.Sleep(510); 不需要等待了，这个地方是最可能出错的地方。
                //设置状态位为false时，后台线程可能在睡眠，所以等线程醒来后就先检查状态位，
                //发现是false的话就不会再读取串口设备，避免了竞争条件。
                if (rfidDevice != null)
                {
                    rfidDevice.closeSerialPort(HardwareInterface);
                }  
            }
          
        }
        private void mainTask()  
        {//这个用于扫描的后台线程永远不会退出，如果状态为为false，他将不做任何工作，仅仅是不断睡眠并等待状态为变为true
         //如果这个状态位是true，就会定期扫描标签，并把标签信息用事件发送出去            
            Dictionary<String, String> TagList = new Dictionary<String, String>();   
            while (true) //扫描主程序，一直会运行
            {
                playSound();
                if (this.backgroundTaskStatus == true)
                {
                    lock(this.rfidLock)
                    {
                        RFIDContent content = new RFIDContent();
                        try
                        {
                            TagList = rfidDevice.readTags();  //该函数会被阻塞，直到读到标签，如果被阻塞在函数内部则等待一段时间并再次读取
                        }
                        catch (Exception e)
                        {
                            eventAggregator.GetEvent<RFIDHardwareEvent>().Publish(e.ToString());
                            //RFIDHardwareEvent 读卡时可能发生错误
                        }
                        foreach (KeyValuePair<String, String> item in TagList)
                        {//从读卡器读到的一批数据，可能有图书的，也可能有书架的，把这些数据进行整理，并准备用event发送出去
                            if (item.Value == "book")
                            {
                                content.bookRfidList.Add(item.Key);
                            }
                            if (item.Value == "shelf")
                            {
                                content.shelfRfidList.Add(item.Key);
                            }
                        }
                        if ((content.bookRfidList.Count() != 0) || (content.shelfRfidList.Count() != 0))
                        {
                            eventAggregator.GetEvent<RFIDNewItemEvent>().Publish(content);
                        }
                        //把读到的内容通过事件发送出去，不管读到的是图书信息还是书架的信息
                        Thread.Sleep(500);
                        //睡眠一定要在所有操作完成后进行，因为醒来后可能状态位已经发生了改变，否则会发生异常
                
                    }
                    
                }
                else
                {                    
                    Thread.Sleep(500);
                    //睡眠一定要在所有操作完成后进行，因为醒来后可能状态位已经发生了改变，否则会发生异常
                }
                lock (this.rfidLock)
                {
                    TagList.Clear();
                }                
            }
        }
        private void playSound()  //仅仅用来测试多线程
        {
            System.Media.SystemSounds.Beep.Play();
        }
    }
}
