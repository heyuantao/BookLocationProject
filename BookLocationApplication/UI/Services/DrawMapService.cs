using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using UI.Models;

namespace UI.Services
{
    //此服务注入容器
    public class DrawMapService
    {
        IUnityContainer container;

        CanvasDrawer oneShelfDrawer;
        CanvasDrawer libraryShelfDrawer;

        List<ShelfShape> oneShelfBoxList;  //书架正视图中的几层的矩形图
        ContourShape oneShelfBoxContour;     //书架正视图的外部轮廓

        List<ShelfShape> shelfMapShelfList; //一个书库俯视图中所有书柜的列表
        DoorShape shelfMapDoor;             //一个书库门的位置
        ContourShape shelfMapContour;       //一个书库的外部轮廓
        RouteShape shelfMapRoute;           //取书的路径列表

        public DrawMapService(IUnityContainer container)
        {
            this.container = container;
            //初始化其他变量
            this.oneShelfDrawer = new CanvasDrawer();
            this.libraryShelfDrawer = new CanvasDrawer();
        }
        public Canvas OneShelfMap
        {
            get { return this.oneShelfDrawer.CurrentCanvas; }
        }
        public Canvas LibraryShelfMap
        {
            get { return this.libraryShelfDrawer.CurrentCanvas; }
        }
        //初始化单个书架画布的各项尺寸参数
        public void initOneShapMap(float canvasWidth, float canvasHeight, float mapWidth, float mapHeight)
        {
            this.oneShelfDrawer.initCanvas(canvasWidth,canvasHeight,mapWidth,mapHeight);
                
        }

        //给上层提供的访问函数
        //该函数能够画出一个书架的正视图，参数为层数。也就是画出几层的书架
        public void drawOneShapeMapBackground(int layers)
        {//定义一个五层的书架
            double leftMargin = 20; double topMargin = 10;
            ShelfShape layer5 = new ShelfShape(new Point(leftMargin, topMargin+0), new Point(leftMargin+100,   topMargin+20));
            ShelfShape layer4 = new ShelfShape(new Point(leftMargin, topMargin+20), new Point(leftMargin + 100, topMargin + 40));
            ShelfShape layer3 = new ShelfShape(new Point(leftMargin, topMargin+40), new Point(leftMargin + 100, topMargin + 60));
            ShelfShape layer2 = new ShelfShape(new Point(leftMargin, topMargin+60), new Point(leftMargin + 100, topMargin + 80));
            ShelfShape layer1 = new ShelfShape(new Point(leftMargin, topMargin+80), new Point(leftMargin + 100, topMargin + 100));

            this.oneShelfBoxList.Add(layer5);
            this.oneShelfBoxList.Add(layer4);
            this.oneShelfBoxList.Add(layer3);
            this.oneShelfBoxList.Add(layer2);
            this.oneShelfBoxList.Add(layer1);
            //开始在画布上画图
            foreach(ShelfShape shapeItem in this.oneShelfBoxList){
                this.oneShelfDrawer.drawShelf(shapeItem.topLeft, shapeItem.bottomRight);
            }

        }
        //该函数能够在一个已有的书架正视图上画出被选中的书架层，这个层小于书架的总层数
        public void drawSelectedLayerOneShapeMap(int layer)
        {

        }
        //该函数能够画出一个书库的背景图片，包含书架，轮廓和门，参数为该图书馆名称，参数即为Map表中的location字段
        public void LibraryShelfMapBackgroundByLibraryName(String libraryname)
        {

        }
        //该函数能够画出取书的路线，也就是从入口到目的书架的折线图，参数为Map表中的rfidOfShelf字段
        public void drawRouteOnLibraryShelfMap(String shelfRfid)
        {

        }
    }
    

}
