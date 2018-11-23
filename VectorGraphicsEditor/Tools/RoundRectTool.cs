using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using VectorGraphicsEditor.Figures;

namespace VectorGraphicsEditor.Tools
{
    class RoundRectTool : Tool
    {
        public override void MouseDown(Point mousePosition)
        {
            base.MouseDown(mousePosition);

            GlobalVars.figures.Add(new RoundRect(GlobalVars.pen.Clone(), GlobalVars.brush.Clone()));
            GlobalVars.figures.Last().points.Add(mousePosition);
            GlobalVars.figures.Last().points.Add(mousePosition);
        }

        public override void MouseMove(Point mousePosition)
        {
            if (isDown)
                GlobalVars.figures.Last().points[0] = mousePosition;
        }
    }
}
