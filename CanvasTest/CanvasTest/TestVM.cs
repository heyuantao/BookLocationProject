using Prism.Commands;
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
        ICommand click;
        public TestVM()
        {
            
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
            Random rnd = new Random();
            Canvas canvas = this.canvas;
            Rectangle rect = new Rectangle();
            rect.Stroke = new SolidColorBrush(Colors.Black);
            rect.Fill = new SolidColorBrush(Colors.Black);
            rect.Width = 30;
            rect.Height = 30;
            Canvas.SetLeft(rect, rnd.Next(1,1000));
            Canvas.SetTop(rect, rnd.Next(1, 1000));
            canvas.Children.Add(rect);

            String showString = "Width:" + canvas.ActualWidth + " " + "Heigh:" + canvas.ActualHeight;
            MessageBox.Show(showString);
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
