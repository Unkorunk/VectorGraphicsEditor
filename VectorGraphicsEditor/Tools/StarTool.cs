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

            GlobalVars.Figures.Add(new Star(GlobalVars.pen.Clone()));
            GlobalVars.Figures.Last().points.Add(mousePosition);
            GlobalVars.Figures.Last().points.Add(mousePosition);

            double angle = -33.0;
            int count = 5;
            for (int i = 0; i < count; i++)
                (GlobalVars.Figures.Last() as Star)._patternList.Add(new Point(Math.Cos(angle + 2 * Math.PI * i / count), Math.Sin(angle + 2 * Math.PI * i / count)));
        }

        public override void MouseMove(Point mousePosition)
        {
            if (isDown)
                GlobalVars.Figures.Last().points[0] = mousePosition;
        }
    }
}
