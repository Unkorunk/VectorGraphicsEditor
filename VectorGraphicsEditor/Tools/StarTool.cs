using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using VectorGraphicsEditor.Figures;

namespace VectorGraphicsEditor.Tools
{
    class StarTool : Tool
    {
        public override void MouseDown(Point mousePosition)
        {
            base.MouseDown(mousePosition);

            GlobalVars.figures.Add(new Star(GlobalVars.pen.Clone()));
            GlobalVars.figures.Last().points.Add(mousePosition);
            GlobalVars.figures.Last().points.Add(mousePosition);

            double angle = -33.0;
            int count = 5;
            for (int i = 0; i < count; i++)
                (GlobalVars.figures.Last() as Star)._patternList.Add(new Point(Math.Cos(angle + 2 * Math.PI * i / count), Math.Sin(angle + 2 * Math.PI * i / count)));
        }

        public override void MouseMove(Point mousePosition)
        {
            if (isDown)
                GlobalVars.figures.Last().points[0] = mousePosition;
        }
    }
}
