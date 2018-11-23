using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace VectorGraphicsEditor.Tools
{
    class Loupe : Tool
    {
        public override void MouseDown(Point mousePosition)
        {
            Point to = new Point(GlobalVars.sizeCanvas.Width / 2.0, GlobalVars.sizeCanvas.Height / 2.0);
            GlobalVars.Shearing(Point.Subtract(to, mousePosition));
            GlobalVars.Zooming(2.0);
        }
    }
}
