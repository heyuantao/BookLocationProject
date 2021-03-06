﻿using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace CanvasTest
{
    public class TestVM
    {
        Canvas canvas;
        DrawMap drawMap;
        ICommand click;
        public TestVM()
        {
            this.drawMap = new DrawMap(800,600,1600,1200);
            this.canvas = this.drawMap.CurrentCanvas;
        }
        public ICommand Click
        {
            get
            {
                if (this.click == null)
                {
                    this.click = new DelegateCommand(onClickExecute);
                }
                return this.click;
            }
        }

        private void onClickExecute()
        {
            //Point a1 = new Point(30, 30);
            //Point a2 = new Point(200, 200);
            //this.drawMap.drawOneRectangle(a1, a2);
            //String showString = "Width:" + canvas.ActualWidth + " " + "Heigh:" + canvas.ActualHeight;
            //MessageBox.Show(showString);

            Point a5 = new Point(30, 30);
            Point a6 = new Point(900, 30);
            Point a7 = new Point(30, 900);
            List<Point> pointList = new List<Point>();
            pointList.Add(a5); pointList.Add(a6); pointList.Add(a7);
            this.drawMap.drawRoute(pointList);


        }
        public Canvas MyCanvas
        {
            get
            {
                if (this.canvas == null)
                {
                    this.canvas = new Canvas();
                    //this.canvas.Height = 400;
                    //this.canvas.Width = 300;
                    this.canvas.Background = new SolidColorBrush(Colors.Blue);
                }
                return this.canvas;
            }
        }
    }
}
