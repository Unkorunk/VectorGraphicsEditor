using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using VectorGraphicsEditor.Figures;

namespace VectorGraphicsEditor.Tools
{
    class LineTool : Tool
    {
        public override void MouseDown(Point mousePosition)
        {
            base.MouseDown(mousePosition);

            GlobalVars.figures.Add(new Line(GlobalVars.pen.Clone()));
            GlobalVars.figures.Last().points.Add(mousePosition);
            GlobalVars.figures.Last().points.Add(mousePosition);
        }

        public override void MouseMove(Point mousePosition)
        {
            if (isDown)
                GlobalVars.figures.Last().points[1] = mousePosition;
        }
    }
}
