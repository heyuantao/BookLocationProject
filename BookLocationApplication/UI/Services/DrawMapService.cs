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

        //给上层提供的访问函数
        //该函数能够画出一个书架的正视图，参数为层数。也就是画出几层的书架
        public void drawOneShapeMapBackground(int layers)
        {

        }
        //该函数能够在一个已有的书架正视图上画出被选中的书架层
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
