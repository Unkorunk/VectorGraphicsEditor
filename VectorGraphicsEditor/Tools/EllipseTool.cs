using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using VectorGraphicsEditor.Figures;

namespace VectorGraphicsEditor.Tools
{
    class EllipseTool : Tool
    {
        public override void MouseDown(Point mousePosition)
        {
            base.MouseDown(mousePosition);

            GlobalVars.Figures.Add(new Ellipse(GlobalVars.pen.Clone(), GlobalVars.brush.Clone()));
            GlobalVars.Figures.Last().points.Add(mousePosition);
            GlobalVars.Figures.Last().points.Add(mousePosition);
        }

        public override void MouseMove(Point mousePosition)
        {
            if (isDown)
                GlobalVars.Figures.Last().points[0] = mousePosition;
        }
    }
}
