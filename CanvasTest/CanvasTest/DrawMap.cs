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
        {
            //初始化地图的大小和画布的大小，并且计算出缩放的比例，在画图中使用
            this.canvas = new Canvas();
            this.canvasHeight = canvasHeight; this.canvasWidth = canvasWidth;
            this.mapHeight = mapHeight; this.mapWidth = mapWidth;
            this.heightRatio = this.canvasHeight / this.mapHeight;
            this.widthRatio = this.canvasWidth / this.mapWidth;
        }
        public void drawBackGround()
        {
            //画出背景的，例如书架，入口，以及轮廓
        }
        public void drawRoute()
        {
            //画出取书的路线
        }
        Canvas getCanvas()
        {
            return this.canvas;
        }
        public void drawRectangle(float left,float top,float width,float height,) //在画布上画出一个矩形
        {
            Rectangle rect = new Rectangle();
            float leftWithRatio = left * this.widthRatio;
            float topWithRatio = top * this.heightRatio;
            float widthWithRatio = width * this.widthRatio;
            float heightWithRatio = height * this.heightRatio;
            rect.Width=widthRatio;
            rect.Height=heightWithRatio;
            rect.Stroke = new SolidColorBrush(Colors.Black);
            rect.Fill = new SolidColorBrush(Colors.Black);
            Canvas.SetLeft(rect, leftWithRatio);
            Canvas.SetTop(rect, topWithRatio);
            this.canvas.Children.Add(rect);
        }
        public void drawPolygon()
        {
            //绘制图书馆围墙，也就是边界
            Polygon polygon = new Polygon();
            polygon.Stroke = System.Windows.Media.Brushes.Black;
            polygon.Fill = System.Windows.Media.Brushes.LightSeaGreen;
            polygon.StrokeThickness = 2;
            polygon.HorizontalAlignment = HorizontalAlignment.Left;
            polygon.VerticalAlignment = VerticalAlignment.Center;
            Point Point1 = new Point(1, 50);
            Point Point2 = new Point(10,80);
            Point Point3 = new System.Windows.Point(50,50);
            PointCollection myPointCollection = new PointCollection();
            myPointCollection.Add(Point1);
            myPointCollection.Add(Point2);
            myPointCollection.Add(Point3);
            myPolygon.Points = myPointCollection;
            myGrid.Children.Add(myPolygon);
        }

    }
}
