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
        CanvasDrawer oneShelfDrawer;
        CanvasDrawer shelfMapDrawer;
        //List<ShelfShape> 

        public DrawMapService()
        {
            this.oneShelfDrawer = new CanvasDrawer();
            this.shelfMapDrawer = new CanvasDrawer();
        }
    }
    

}
