using System;
using System.Windows;
using VectorGraphicsEditor.Figures;

namespace VectorGraphicsEditor.Tools
{
    class StarTool : Tool
    {
        public override void MouseDown(Point mousePosition)
        {
            base.MouseDown(mousePosition);

            GlobalVars.Figures.Add(new Star(GlobalVars.Pen.Clone(), GlobalVars.Brush.Clone()));
            GlobalVars.Figures[GlobalVars.Figures.Count - 1].AddPoint(mousePosition);
            GlobalVars.Figures[GlobalVars.Figures.Count - 1].AddPoint(mousePosition);

            double angle = -33.0;
            int count = 5;
            for (int i = 0; i < count; i++)
            {
                ((Star) GlobalVars.Figures[GlobalVars.Figures.Count - 1])._patternList.Add(
                    new Point(Math.Cos(angle + 2 * Math.PI * i / count), Math.Sin(angle + 2 * Math.PI * i / count)));
                ((Star) GlobalVars.Figures[GlobalVars.Figures.Count - 1])._patternList.Add(
                    new Point(0.5 * Math.Cos(angle + 33 + 180 + 2 * Math.PI * i / count), 0.5 * Math.Sin(angle + 33 + 180 + 2 * Math.PI * i / count)));
            }


        }

        public override void MouseMove(Point mousePosition)
        {
            if (isDown)
                GlobalVars.Figures[GlobalVars.Figures.Count - 1].SetPoint(0, mousePosition);
        }
    }
}
