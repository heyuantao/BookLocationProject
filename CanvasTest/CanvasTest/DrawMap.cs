using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace CanvasTest
{
    public class DrawMap
    {
        Canvas canvas;
        float canvasWidth, canvasHeight;
        float mapWidth, mapHeight;
        float widthRatio, heightRatio;
        public DrawMap(float canvasWidth, float canvasHeight,float mapWidth,float mapHeight)
        {   //头两个参数是画布的大小，以像素为单位。后两个参数是图的尺寸，实际单位可以是米，也可以是厘米
            //虽然四个参数的单位不同，但不影响计算
            //初始化地图的大小和画布的大小，并且计算出缩放的比例，在画图中使用
            //如果用户调整窗口改变了高度和宽度就要重新绘制
            this.canvas = new Canvas();
            this.canvasHeight = canvasHeight; this.canvasWidth = canvasWidth;
            this.mapHeight = mapHeight; this.mapWidth = mapWidth;
            this.heightRatio = this.canvasHeight / this.mapHeight;
            this.widthRatio = this.canvasWidth / this.mapWidth;

            this.canvas.Background = new SolidColorBrush(Colors.Red);
        }
        public void drawBackGround()
        {
            //画出背景的，例如书架，入口，以及轮廓
        }
        public void drawRoute()
        {
            //画出取书的路线
        }
        public Canvas getCanvas()
        {
            return this.canvas;
        }
        //public void drawRectangle(float left,float top,float width,float height,) //在画布上画出一个矩形
        public void drawOneRectangle(Point leftTop,Point rightBottom)
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

            rect.Width=widthWithRatio;
            rect.Height=heightWithRatio;
            //设置样式
            //rect.Stroke = new SolidColorBrush(Colors.Black);
            rect.Fill = System.Windows.Media.Brushes.Black;
            //rect.Fill = new SolidColorBrush(Colors.Black);
            //System.Windows.Media.Brushes.LightSeaGreen;
            Canvas.SetLeft(rect, leftWithRatio);
            Canvas.SetTop(rect, topWithRatio);
            this.canvas.Children.Add(rect);
        }
        public void drawOnePolygon(List<Point> pointList)
        {
            //绘制图书馆围墙，也就是边界
            Polygon polygon = new Polygon();
            //polygon.Stroke = new SolidColorBrush(Colors.Black);
            polygon.Stroke = System.Windows.Media.Brushes.Black;
            //polygon.Fill = new SolidColorBrush(Colors.Black);
            polygon.StrokeThickness = 2;//设置样式
            //polygon.HorizontalAlignment = HorizontalAlignment.Left;
            //polygon.VerticalAlignment = VerticalAlignment.Center;

            PointCollection pointCollectionWithRatio = new PointCollection();
            foreach (Point pointItem in pointList)
            {
                Point pointWithRatio = new Point(pointItem.X*this.widthRatio,pointItem.Y*this.heightRatio);
                pointCollectionWithRatio.Add(pointWithRatio);
            }
            polygon.Points = pointCollectionWithRatio;
            this.canvas.Children.Add(polygon);
        }

    }
}
