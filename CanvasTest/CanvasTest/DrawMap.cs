using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace CanvasTest
{
    public class DrawMap
    {
        Canvas canvas;
        float canvasWidth, canvasHeight;
        float mapWidth, mapHeight;
        float widthRatio, heightRatio;
        //初始化地图的大小和画布的大小，并且计算出缩放的比例，在画图中使用
        public DrawMap(float canvasWidth, float canvasHeight,float mapWidth,float mapHeight)
        {
            this.canvas = new Canvas();
            this.canvasHeight = canvasHeight; this.canvasWidth = canvasWidth;
            this.mapHeight = mapHeight; this.mapWidth = mapWidth;
            this.heightRatio = this.canvasHeight / this.mapHeight;
            this.widthRatio = this.canvasWidth / this.mapWidth;
        }

    }
}
