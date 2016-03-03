using Infrastructure;
using Microsoft.Practices.Unity;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using UI.Models;

namespace UI.Services
{
    //此服务注入容器
    public class DrawMapService
    {
        IUnityContainer container;
        IEventAggregator eventAggregator;

        CanvasDrawer oneShelfDrawer;
        CanvasDrawer libraryShelfDrawer;

        List<ShelfShape> oneShelfBoxList;  //书架正视图中的几层的矩形图
        ContourShape oneShelfBoxContour;     //书架正视图的外部轮廓

        //List<ShelfShape> shelfMapShelfList; //一个书库俯视图中所有书柜的列表
        //DoorShape shelfMapDoor;             //一个书库门的位置
        //ContourShape shelfMapContour;       //一个书库的外部轮廓
        //RouteShape shelfMapRoute;           //取书的路径列表

        public DrawMapService(IUnityContainer container)
        {
            this.container = container;
            this.eventAggregator = container.Resolve<IEventAggregator>();
            //初始化其他变量
            this.oneShelfDrawer = new CanvasDrawer();
            this.libraryShelfDrawer = new CanvasDrawer();
            //初始化各种图形参数,先初始化一个值，不管是否有意义，以免在后续引用中出错。
            this.oneShelfBoxList = new List<ShelfShape>();
            this.oneShelfBoxContour = new ContourShape(new List<Point>());
            //this.shelfMapShelfList = new List<ShelfShape>();
            //this.shelfMapDoor = new DoorShape(new Point(0,0),new Point(0,0));
            //this.shelfMapContour = new ContourShape(new List<Point>());
            //this.shelfMapRoute = new RouteShape(new List<Point>());
            initVariableValue();
        }

        private void initVariableValue()
        { //定义一个五层的书架
            double leftMargin = 20; double topMargin = 30;
            ShelfShape layer5 = new ShelfShape(new Point(leftMargin, topMargin + 0), new Point(leftMargin + 100, topMargin + 20));
            ShelfShape layer4 = new ShelfShape(new Point(leftMargin, topMargin + 20), new Point(leftMargin + 100, topMargin + 40));
            ShelfShape layer3 = new ShelfShape(new Point(leftMargin, topMargin + 40), new Point(leftMargin + 100, topMargin + 60));
            ShelfShape layer2 = new ShelfShape(new Point(leftMargin, topMargin + 60), new Point(leftMargin + 100, topMargin + 80));
            ShelfShape layer1 = new ShelfShape(new Point(leftMargin, topMargin + 80), new Point(leftMargin + 100, topMargin + 100));

            this.oneShelfBoxList.Add(layer5);
            this.oneShelfBoxList.Add(layer4);
            this.oneShelfBoxList.Add(layer3);
            this.oneShelfBoxList.Add(layer2);
            this.oneShelfBoxList.Add(layer1);

            List<Point> contourPointList = new List<Point>();
            Point a1 = new Point(leftMargin + 0, topMargin + 0);
            Point a2 = new Point(leftMargin + 100, topMargin + 0);
            Point a3 = new Point(leftMargin + 100, topMargin + 100);
            Point a4 = new Point(leftMargin + 0, topMargin + 100);
            contourPointList.Add(a1);
            contourPointList.Add(a2);
            contourPointList.Add(a3);
            contourPointList.Add(a4);

            this.oneShelfBoxContour = new ContourShape(contourPointList);
            //新的代码，用于形成背景图片，开始
            BitmapImage bitmapImage = new BitmapImage();
            Uri uri = new Uri("pack://application:,,,/Resource/images/shelf.png", UriKind.RelativeOrAbsolute);
            bitmapImage.BeginInit();
            bitmapImage.UriSource = uri;
            bitmapImage.EndInit();

            Image backgroundImage = new Image();
            backgroundImage.Source = bitmapImage;
            this.OneShelfMap.Children.Add(backgroundImage);
            //新的代码，用于形成背景图片，结束
        }

        //这两个GET 与SET用于返回绘制的两个地图，一个书架的正视图，一个是书库的俯视图
        public Canvas OneShelfMap
        {
            get {
                return this.oneShelfDrawer.CurrentCanvas;                 
            }
        }
        public Canvas LibraryShelfMap
        {
            get { return this.libraryShelfDrawer.CurrentCanvas; }
        }


        //初始化单个书架画布的各项尺寸参数
        public void initOneShapMap(float canvasWidth, float canvasHeight, float mapWidth, float mapHeight)
        {
            this.oneShelfDrawer.initCanvas(canvasWidth,canvasHeight,mapWidth,mapHeight);
            this.oneShelfDrawer.CurrentCanvas.Background = System.Windows.Media.Brushes.LightSeaGreen;    
        }
        //重新初始化单个书架画布的各项尺寸参数
        public void reinitOneShapMap()
        {
            this.oneShelfDrawer.reinitCanvas();
            this.oneShelfDrawer.CurrentCanvas.Background = System.Windows.Media.Brushes.LightSeaGreen;
        }
        //给上层提供的访问函数
        //该函数能够画出一个书架的正视图，默认为5层。也就是画出几层的书架
        public void drawOneShapeMapBackground()
        {//开始在画布上画图
            foreach(ShelfShape shapeItem in this.oneShelfBoxList){
                this.oneShelfDrawer.drawShelf(shapeItem.topLeft, shapeItem.bottomRight);
            }
            //开始画轮廓
            this.oneShelfDrawer.drawContour(this.oneShelfBoxContour.pointList);
            //在图片四周绘制边框
            //this.oneShelfDrawer.drawShelf(new Point(0, 0), new Point(this.oneShelfDrawer.canvasWidth,this.oneShelfDrawer.canvasHeight));
        }
        //该函数能够在一个已有的书架正视图上画出被选中的书架层，这个层小于书架的总层数
        public void drawSelectedLayerOneShapeMap(String locationString)
        {            
            try
            {
                int layer = this.bookLocationStringToLayerOfShelf(locationString);
                ShelfShape oneshape = this.oneShelfBoxList[layer];
                this.oneShelfDrawer.drawSelectedShelf(oneshape.topLeft, oneshape.bottomRight);
            }
            catch (Exception){
                //是否需要处理异常 
            }            
        }


        //初始化书库俯视图画布的各项尺寸参数
        public void initLibraryShelfMap(float canvasWidth, float canvasHeight, float mapWidth, float mapHeight)
        {
            this.libraryShelfDrawer.initCanvas(canvasWidth, canvasHeight, mapWidth, mapHeight);
            this.libraryShelfDrawer.CurrentCanvas.Background = System.Windows.Media.Brushes.LightSeaGreen;
        }
        //重新初始化书库俯视图画布的各项尺寸参数
        public void reinitLibraryShelfMap()
        {
            this.libraryShelfDrawer.reinitCanvas();
            this.libraryShelfDrawer.CurrentCanvas.Background = System.Windows.Media.Brushes.LightSeaGreen;
        }
        //该函数能够画出一个书库的背景图片，包含书架，轮廓和门，参数为该图书馆名称，参数即为Map表中的location字段
        public void drawLibraryShelfMapBackgroundByLibraryName(String libraryName)
        {
            String libraryNameInTable = this.bookLocationStringToLibraryName(libraryName);
            //获取图书位置信息数据库的引用
            IBookLocationService bookLocationService = this.container.Resolve<IBookLocationService>();

            try
            {
                //获取某个书库中所有的书架
                List<String> shelfPositionStringList = bookLocationService.getItemPositionStringListByLocationAndType(libraryNameInTable, "SHELF");
                //从数据库中读到的字符串列表中解析出所有的书架地理位置信息，并加入到this.shelfMapShelfList中
                List<ShelfShape>  shelfMapShelfList = new List<ShelfShape>();
                foreach (String stringItem in shelfPositionStringList)
                {//stringItem is a string ,such as "13000,1500,1500,1000",startpoint x,y,and width,height
                    char[] separator = { '，', ',' };
                    String[] pointDescInString = stringItem.Split(separator);
                    if (pointDescInString.Length != 4) { 
                        continue; 
                    } //如果不为4个数字，则意味着可能出现了解析或者存储错误
                    double[] pointDescInDouble = new double[] { 
                    Convert.ToDouble(pointDescInString[0]), 
                    Convert.ToDouble(pointDescInString[1]), 
                    Convert.ToDouble(pointDescInString[2]), 
                    Convert.ToDouble(pointDescInString[3]) 
                    };
                    Point leftTop = new Point(pointDescInDouble[0], pointDescInDouble[1]);
                    Point rightBottom = new Point(pointDescInDouble[0] + pointDescInDouble[2], pointDescInDouble[1] + pointDescInDouble[3]);
                    //把书架的位置信息放入结构体的变量中shelfMapShelfList
                    shelfMapShelfList.Add(new ShelfShape(leftTop, rightBottom));
                }
                //开始绘制俯视图中的书架
                foreach (ShelfShape oneShelf in shelfMapShelfList)
                {
                    this.libraryShelfDrawer.drawShelf(oneShelf.topLeft, oneShelf.bottomRight);
                }
            }
            catch (Exception)
            {
                this.eventAggregator.GetEvent<DatabaseEvent>().Publish("DrawMapService:从数据库中获取书架信息出错！");
                //如果发生错误那发布事件
            }


            //获得某个书库的轮廓
            try
            {
                List<String> shelfContourPostionStringList = bookLocationService.getItemPositionStringListByLocationAndType(libraryNameInTable, "CONTOUR");
                String shelfContourPostionString = "";
                ContourShape shelfMapContour ; //轮廓信息存放的地点
                //如果获得的轮廓个数小于零，则说明数据出现了问题.其实一个书库的轮廓只能有一个
                if (shelfContourPostionStringList.Count > 0)
                {
                    shelfContourPostionString = shelfContourPostionStringList[0];
                    char[] separator = { '，', ',' };
                    String[] pointDescInString = shelfContourPostionString.Split(separator);
                    //轮廓点的个数只能是偶数个,如果解析和读取数据的时候出现任何错误，则花图书轮廓的程序终止
                    if ((pointDescInString.Length % 2 == 0) && (pointDescInString.Length >= 2))
                    {
                        List<Double> pointXList = new List<Double>();
                        List<Double> pointYList = new List<Double>();
                        for (int i = 0; i < pointDescInString.Length; i = i + 2)
                        {
                            pointXList.Add(Convert.ToDouble(pointDescInString[i]));
                            pointYList.Add(Convert.ToDouble(pointDescInString[i + 1]));
                        }
                        if (pointXList.Count == pointYList.Count)
                        {
                            List<Point> contourPointList = new List<Point>();
                            for (int i = 0; i < pointXList.Count(); i++)
                            {
                                Point onePoint = new Point(pointXList[i], pointYList[i]);
                                contourPointList.Add(onePoint);
                            }
                            shelfMapContour = new ContourShape(contourPointList);
                            this.libraryShelfDrawer.drawContour(shelfMapContour.pointList);
                        }
                    }

                }
            }catch(Exception)
            {
                this.eventAggregator.GetEvent<DatabaseEvent>().Publish("DrawMapService:从数据库中获取书库轮廓信息出错！");
                //如果发生错误那发布事件
            }
            


            //设置某个书库大门的位置
            try
            {
                List<String> doorPostionStringList = bookLocationService.getItemPositionStringListByLocationAndType(libraryNameInTable, "DOOR");
                DoorShape shelfMapDoor;//入口信息存放的位置
                if (doorPostionStringList.Count > 0)
                {
                    String doorPostionString = doorPostionStringList[0];
                    char[] separator = { '，', ',' };
                    String[] pointDescInString = doorPostionString.Split(separator);
                    if (pointDescInString.Length == 4)
                    {
                        double[] pointDescInDouble = new double[] { 
                        Convert.ToDouble(pointDescInString[0]), 
                        Convert.ToDouble(pointDescInString[1]), 
                        Convert.ToDouble(pointDescInString[2]), 
                        Convert.ToDouble(pointDescInString[3]) 
                        };
                        Point leftTop = new Point(pointDescInDouble[0], pointDescInDouble[1]);
                        Point rightBottom = new Point(pointDescInDouble[0] + pointDescInDouble[2], pointDescInDouble[1] + pointDescInDouble[3]);
                        //把书库大门的位置信息放入结构体的变量中shelfMapShelfList
                        shelfMapDoor = new DoorShape(leftTop, rightBottom);
                        //开始绘制入口
                        this.libraryShelfDrawer.drawDoor(shelfMapDoor.topLeft, shelfMapDoor.bottomRight);
                    }
                }
            }
            catch (Exception)
            {
                this.eventAggregator.GetEvent<DatabaseEvent>().Publish("DrawMapService:从数据库中获取书库大门信息出错！");
                //如果发生错误那发布事件
            }

                        
        }
        //该函数能够画出目标书架在书库中的位置
        public void drawSelectedShelfLibraryShelfMapByLibraryName(String shelfRfid)
        {
            //获取图书位置信息数据库的引用
            IBookLocationService bookLocationService = this.container.Resolve<IBookLocationService>();
            String shelfLocationString = bookLocationService.getItemPositionStringByShelfRfid(shelfRfid);
            //把位置字符解析为具体的图形
            char[] separator = { '，', ',' };
            String[] pointDescInString = shelfLocationString.Split(separator);
            if (pointDescInString.Length == 4)
            {
                double[] pointDescInDouble = new double[] { 
                        Convert.ToDouble(pointDescInString[0]), 
                        Convert.ToDouble(pointDescInString[1]), 
                        Convert.ToDouble(pointDescInString[2]), 
                        Convert.ToDouble(pointDescInString[3]) 
                };
                Point leftTop = new Point(pointDescInDouble[0], pointDescInDouble[1]);
                Point rightBottom = new Point(pointDescInDouble[0] + pointDescInDouble[2], pointDescInDouble[1] + pointDescInDouble[3]);
                //把书库大门的位置信息放入结构体的变量中shelfMapShelfList
                //在地图上画出选中的书库
                this.libraryShelfDrawer.drawSelectedShelf(leftTop, rightBottom);
            }
        }
        //该函数能够画出取书的路线，也就是从入口到目的书架的折线图，参数为Map表中的rfidOfShelf字段
        public void drawRouteOnLibraryShelfMap(String shelfRfid)
        {
            this.eventAggregator.GetEvent<DatabaseEvent>().Publish("DrawMapService:此功能未实现！");

        }
        

        //从UI上显示的图书的中文地址信息中提取出楼层的信息，并转换为整数,如果找到了就返回该值，否则就返回-1
        //输入的字符串为 "图书馆W区6层22行3列A面书架第4层"
        private int bookLocationStringToLayerOfShelf(String bookLocationString)
        {
            try
            {
                int layer;
                String patternString = "第[0-9]+层$";
                String match = Regex.Match(bookLocationString, patternString).Value;
                match = match.Replace("第", "").Replace("层", "");
                layer = Convert.ToInt32(match);
                layer = 4 - (layer - 1);
                return layer;
            }
            catch (Exception)
            {
                return -1;
            }
            
        }
        //从UI上显示的图书的中文地址信息中提取出图书馆的名称信息，返回值是字符串，否则返回空字符串
        //输入的字符串为 "图书馆W区6层22行3列A面书架第4层" 返回值为6W ,如果没找到就返回空字符串
        private string bookLocationStringToLibraryName(String bookLocationString)
        {
            try
            {
                String selectionChar = ""; //"W" or "E"
                String floorChar = "";//1,2,3,4,..5 is a number

                String patternStringForselectionChar = "^图书馆[A-Za-z]+区";
                String matchForSelection = Regex.Match(bookLocationString, patternStringForselectionChar).Value;
                selectionChar = matchForSelection.Replace("图书馆", "").Replace("区", "");
                selectionChar = selectionChar.Replace(" ", "");

                String patternStringForfloorChar = "区[0-9]层";
                String matchForFloor = Regex.Match(bookLocationString, patternStringForfloorChar).Value;
                floorChar = matchForFloor.Replace("区", "").Replace("层", "");
                floorChar = floorChar.Replace(" ", "");

                if (String.IsNullOrEmpty(selectionChar) || String.IsNullOrEmpty(floorChar))
                {
                    return "";
                }
                return floorChar+selectionChar.ToUpper();
            }
            catch (Exception)
            {
                return "";
            }            
        }
    }
    

}
