using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace UI.Services
{
    public class ShelfShape
    {
        Point topLeft, bottomRight;
        public ShelfShape(Point topLeft, Point bottomRight)
        {
            this.topLeft = topLeft; this.bottomRight = bottomRight;
        }
    }
    public class DoorShape
    {
        Point topLeft, bottomRight;
        public DoorShape(Point topLeft, Point bottomRight)
        {
            this.topLeft = topLeft; this.bottomRight = bottomRight;
        }
    }
    public class DrawMapService
    {
        public DrawMapService()
        {

        }
    }
    //此服务不注入容器
    public class CanvasDrawer
    {
        Canvas currentCanvas;
        float canvasWidth, canvasHeight;
        float mapWidth, mapHeight;
        float widthRatio, heightRatio;
        public CanvasDrawer()
        {
            this.currentCanvas = new Canvas();
        }
        public void initCarvas(float canvasWidth, float canvasHeight, float mapWidth, float mapHeight)
        {   //头两个参数是画布的大小，以像素为单位。后两个参数是图的尺寸，实际单位可以是米，也可以是厘米

            //虽然四个参数的单位不同，但不影响计算
            //初始化地图的大小和画布的大小，并且计算出缩放的比例，在画图中使用
            //如果用户调整窗口改变了高度和宽度就要重新绘制
            
            this.canvasHeight = canvasHeight; this.canvasWidth = canvasWidth;
            this.mapHeight = mapHeight; this.mapWidth = mapWidth;
            this.heightRatio = this.canvasHeight / this.mapHeight;
            this.widthRatio = this.canvasWidth / this.mapWidth;
            this.currentCanvas.Background = new SolidColorBrush(Colors.Black);
            //this.currentCanvas.Background = System.Windows.Media.Brushes.Red;
        }
        public void drawBackGround()
        {
            //画出背景的，例如书架，入口，以及轮廓

        }
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
            rect.Fill = System.Windows.Media.Brushes.Red;
            this.currentCanvas.Children.Add(rect);
        }
        public void drawShelf(Point leftTop, Point rightBottom) //画书架，俯视图或者正视图
        {
            Rectangle rect = this.drawOneRectangle(leftTop, rightBottom);
            rect.Fill = System.Windows.Media.Brushes.LightSeaGreen;
            this.currentCanvas.Children.Add(rect);
        }
        public void drawContour(List<Point> pointList) //画轮廓（图书馆，书架）
        {
            Polygon poly = this.drawOnePolygon(pointList);
            //poly.Fill = System.Windows.Media.Brushes.Black;
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
