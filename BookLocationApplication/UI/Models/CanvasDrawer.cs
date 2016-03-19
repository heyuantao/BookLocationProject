using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace UI.Models
{
    public class ShelfShape
    {
        public Point topLeft, bottomRight;
        public ShelfShape(Point topLeft, Point bottomRight)
        {
            this.topLeft = topLeft; this.bottomRight = bottomRight;
        }
    } //该形状用于描述图书馆中的一个书柜，也就是俯视图中的一个书柜
    //同时该形状还用于描述在正视图中的书架和书架中一格的这种结构。因为这些形状都是矩形
    public class DoorShape
    {
        public Point topLeft, bottomRight;
        public DoorShape(Point topLeft, Point bottomRight)
        {
            this.topLeft = topLeft; this.bottomRight = bottomRight;
        }
    }  //该形状用于描述图书馆中的一个门，也就是俯视图中的门
    public class ContourShape{
        public List<Point> pointList;
        public ContourShape(List<Point> pointList){
            this.pointList = pointList;
        }
    } //该形状用于描述图书馆外围轮廓的多边形，也就是一个俯视图
    public class RouteShape
    {
        public List<Point> pointList;
        public RouteShape(List<Point> pointList)
        {
            this.pointList = pointList;
        }
    } //该形状用于描述图书馆从入口到某一个书柜的路线，也是一个俯视图

    //此服务不注入容器
    public class CanvasDrawer
    {
        Canvas currentCanvas;
        public float canvasWidth, canvasHeight;
        public float mapWidth, mapHeight;
        public float widthRatio, heightRatio;
        public CanvasDrawer()
        {
            this.currentCanvas = new Canvas();
        }
        public void initCanvas(float canvasWidth, float canvasHeight, float mapWidth, float mapHeight)
        {   //头两个参数是画布的大小，以像素为单位。后两个参数是图的尺寸，实际单位可以是米，也可以是厘米

            //虽然四个参数的单位不同，但不影响计算
            //初始化地图的大小和画布的大小，并且计算出缩放的比例，在画图中使用
            //如果用户调整窗口改变了高度和宽度就要重新绘制

            this.canvasHeight = canvasHeight; this.canvasWidth = canvasWidth;
            this.mapHeight = mapHeight; this.mapWidth = mapWidth;
            this.heightRatio = this.canvasHeight / this.mapHeight;
            this.widthRatio = this.canvasWidth / this.mapWidth;

            this.currentCanvas.Width = canvasWidth;
            this.currentCanvas.Height = canvasHeight;

            //在画布周围画上黑色边框
            List<Point> pointList = new List<Point>();
            pointList.Add(new Point(0, 0));
            pointList.Add(new Point(this.mapWidth, 0));
            pointList.Add(new Point(this.mapWidth, this.mapHeight));
            pointList.Add(new Point(0, this.mapHeight));
            this.drawContour(pointList);
            //this.currentCanvas.Background = new SolidColorBrush(Colors.Black);
            //this.currentCanvas.Background = System.Windows.Media.Brushes.Red;
        }
        public void reinitCanvas()
        {   
            //重新绘制
            this.heightRatio = this.canvasHeight / this.mapHeight;
            this.widthRatio = this.canvasWidth / this.mapWidth;

            this.currentCanvas = new Canvas();
            this.currentCanvas.Width = canvasWidth;
            this.currentCanvas.Height = canvasHeight;

            //在画布周围画上黑色边框
            List<Point> pointList = new List<Point>();
            pointList.Add(new Point(0,0));
            pointList.Add(new Point(this.mapWidth, 0));
            pointList.Add(new Point(this.mapWidth, this.mapHeight));
            pointList.Add(new Point(0, this.mapHeight));             
            this.drawContour(pointList);
            //this.currentCanvas.Background = new SolidColorBrush(Colors.Black);
            //this.currentCanvas.Background = System.Windows.Media.Brushes.Red;
        }
        /***
        public void drawBackGround()
        {//画出背景的，例如书架，入口，以及轮廓}
        ***/
        public void drawRoute(List<Point> pointList)
        {
            //画出取书的路线,现在还没有画上开始和结束的标记
            if (pointList.Count() < 2) { return; }
            Point firstPoint = pointList[0];
            for (int i = 1; i < pointList.Count(); i++)
            {
                Line oneLine = drawOneLine(firstPoint, pointList[i]);
                this.currentCanvas.Children.Add(oneLine);

                firstPoint = pointList[i];
            }
        }
        public void drawDoor(Point leftTop, Point rightBottom)
        {
            Rectangle rect = this.drawOneRectangle(leftTop, rightBottom);
            rect.Fill = System.Windows.Media.Brushes.Blue;
            this.currentCanvas.Children.Add(rect);
        }
        /***
        public void drawSelectedShelf(Point leftTop, Point rightBottom) //画被选中的书架，俯视图或者正视图
        {
            Rectangle rect = this.drawOneRectangle(leftTop, rightBottom);
            rect.Opacity = 0.5;
            rect.Fill = System.Windows.Media.Brushes.Red;
            rect.StrokeThickness = 0;
            this.currentCanvas.Children.Add(rect);
        }
         * **/
        public void drawSelectedRectangle(Point leftTop, Point rightBottom) //画被被选定的矩形区域
        {
            Rectangle rect = this.drawOneRectangle(leftTop, rightBottom);
            rect.Opacity = 0.5;
            rect.Fill = System.Windows.Media.Brushes.Red;
            rect.StrokeThickness = 0;
            this.currentCanvas.Children.Add(rect);
        }
        //这段代码基本要被废弃，用于绘制一些矩形边框
        public void drawShelf(Point leftTop, Point rightBottom) 
        {
            Rectangle rect = this.drawOneRectangle(leftTop, rightBottom);
            rect.Stroke = System.Windows.Media.Brushes.Red;
            this.currentCanvas.Children.Add(rect);
        }
        //该方法用于在画布上绘制一个图形，两个点用于表示图像的填充区域，image是原始的图片，angle是角度，只有{0,90,180,270}三个角度可选
        /***
        public void drawImage(Point leftTop, Point rightBottom, BitmapImage bitmapImage)
        {
            double left = leftTop.X;
            double top = leftTop.Y;
            double width = Math.Abs(leftTop.X - rightBottom.X);
            double height = Math.Abs(leftTop.Y - rightBottom.Y);

            double leftWithRatio = left * this.widthRatio;
            double topWithRatio = top * this.heightRatio;
            double widthWithRatio = width * this.widthRatio;
            double heightWithRatio = height * this.heightRatio;

            Image shelfImage = new Image();
            //RotateTransform rotateTransform = new RotateTransform(angle);
           
            //DropShadowBitmapEffect bitmapEffect = new DropShadowBitmapEffect();
            //Color myShadowColor = new Color();
            //myShadowColor.ScA = 1;
            //myShadowColor.ScB = 0;
            //myShadowColor.ScG = 0;
            //myShadowColor.ScR = 0;
            //bitmapEffect.Color = myShadowColor;
            //bitmapEffect.Direction = 320;
            //bitmapEffect.ShadowDepth = 10;
            //bitmapEffect.Softness = 0.1;
            //bitmapEffect.Opacity = 0.1;
            shelfImage.Source = bitmapImage;
            shelfImage.Stretch = Stretch.Uniform;
            //shelfImage.BitmapEffect = bitmapEffect;
            //shelfImage.RenderTransform = rotateTransform;
            //shelfImage.Width = widthWithRatio;shelfImage.Height = heightWithRatio;
            shelfImage.Width = heightWithRatio; shelfImage.Height = widthWithRatio;            

            Canvas.SetTop(shelfImage,topWithRatio);//adjust the postion
            Canvas.SetLeft(shelfImage, leftWithRatio);
            Canvas.SetZIndex(shelfImage, -10);
            this.currentCanvas.Children.Add(shelfImage);
        }
        ***/
        public void drawImage(Point leftTop, Point rightBottom, BitmapImage bitmapImage, 
            Transform rotateTransform, BitmapEffect bitmapEffect)
        {
            double left = leftTop.X;
            double top = leftTop.Y;
            double width = Math.Abs(leftTop.X - rightBottom.X);
            double height = Math.Abs(leftTop.Y - rightBottom.Y);
            double leftWithRatio = left * this.widthRatio;
            double topWithRatio = top * this.heightRatio;
            double widthWithRatio = width * this.widthRatio;
            double heightWithRatio = height * this.heightRatio;

            Image shelfImage = new Image();
            shelfImage.Source = bitmapImage;
            shelfImage.Stretch = Stretch.Fill;
            shelfImage.RenderTransform = rotateTransform;
            shelfImage.BitmapEffect = bitmapEffect;
            shelfImage.Width = widthWithRatio-3;
            shelfImage.Height = heightWithRatio;
            //shelfImage.Width = heightWithRatio; shelfImage.Height = widthWithRatio;

            Canvas.SetTop(shelfImage, topWithRatio);//adjust the postion
            Canvas.SetLeft(shelfImage, leftWithRatio);
            Canvas.SetZIndex(shelfImage, -10);
            this.currentCanvas.Children.Add(shelfImage);
        }
        public void drawFloor(BitmapImage floorTileBitmapImage)
        {
            double floorTileHeight = floorTileBitmapImage.Height;
            double floorTileWidth = floorTileBitmapImage.Width;
            double libraryHeight = this.canvasHeight;
            double libraryWidth = this.canvasWidth;
            int widthCount = (int)(libraryWidth / floorTileWidth) + 1;
            int heightCount = (int)(libraryHeight / floorTileHeight) + 1;

            Image floorTileImage = new Image();
            floorTileImage.Source = floorTileBitmapImage;
            floorTileImage.Height = floorTileHeight * this.heightRatio*20;
            floorTileImage.Width = floorTileWidth * this.widthRatio*20;
            floorTileImage.Opacity = 0.3;

            Canvas.SetTop(floorTileImage, 0);//adjust the postion
            Canvas.SetLeft(floorTileImage, 0);
            Canvas.SetZIndex(floorTileImage, -1);
            this.currentCanvas.Children.Add(floorTileImage);
        }
        public void drawContour(List<Point> pointList) //画轮廓（图书馆，书架）
        {
            Polygon poly = this.drawOnePolygon(pointList);
            //poly.Fill = System.Windows.Media.Brushes.Black;
            poly.Stroke = System.Windows.Media.Brushes.Aqua;
            poly.StrokeThickness = 2;
            this.currentCanvas.Children.Add(poly);
        }
        public Canvas CurrentCanvas  //用户返回当前的的画布
        {
            get { return this.currentCanvas; }
        }
        //public void drawRectangle(float left,float top,float width,float height,) //在画布上画出一个矩形
        //基本形状的绘制函数
        private Rectangle drawOneRectangle(Point leftTop, Point rightBottom)
        {
            //在画布上画出一个矩形
            double left = leftTop.X;
            double top = leftTop.Y;
            double width = Math.Abs(leftTop.X - rightBottom.X);
            double height = Math.Abs(leftTop.Y - rightBottom.Y);

            Rectangle rect = new Rectangle();
            double leftWithRatio = left * this.widthRatio;
            double topWithRatio = top * this.heightRatio;
            double widthWithRatio = width * this.widthRatio;
            double heightWithRatio = height * this.heightRatio;

            rect.Width = widthWithRatio;
            rect.Height = heightWithRatio;
            //设置样式
            //rect.Stroke = new SolidColorBrush(Colors.Black);
            rect.Stroke = System.Windows.Media.Brushes.Black;  //默认颜色黑色
            //rect.Fill = new SolidColorBrush(Colors.Black);
            //System.Windows.Media.Brushes.LightSeaGreen;
            Canvas.SetLeft(rect, leftWithRatio);
            Canvas.SetTop(rect, topWithRatio);
            //this.canvas.Children.Add(rect);
            return rect;  //将画出的多边形返回
        }
        private Polygon drawOnePolygon(List<Point> pointList)
        {
            //绘制图书馆围墙，也就是边界
            Polygon polygon = new Polygon();
            //polygon.Stroke = new SolidColorBrush(Colors.Black);
            polygon.Stroke = System.Windows.Media.Brushes.Black; //默认颜色黑色
            //polygon.Fill = new SolidColorBrush(Colors.Black);
            //polygon.StrokeThickness = 2;//设置样式
            //polygon.HorizontalAlignment = HorizontalAlignment.Left;
            //polygon.VerticalAlignment = VerticalAlignment.Center;

            PointCollection pointCollectionWithRatio = new PointCollection();
            foreach (Point pointItem in pointList)
            {
                Point pointWithRatio = new Point(pointItem.X * this.widthRatio, pointItem.Y * this.heightRatio);
                pointCollectionWithRatio.Add(pointWithRatio);
            }
            polygon.Points = pointCollectionWithRatio;
            return polygon; //将画出的多边形返回
            //this.canvas.Children.Add(polygon);
        }
        private Line drawOneLine(Point first, Point second)
        {
            Line line = new Line();
            line.X1 = first.X; line.Y1 = first.Y;
            line.X2 = second.X; line.Y2 = second.Y;
            line.Stroke = System.Windows.Media.Brushes.Black; //默认颜色黑色
            return line;
        }
    }

}
