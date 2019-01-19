using System;
using System.Windows;
using VectorGraphicsEditor.Figures;

namespace VectorGraphicsEditor.Tools
{
    class EllipseTool : Tool
    {
        public override void MouseDown(Point mousePosition)
        {
            base.MouseDown(mousePosition);

            GlobalVars.Figures.Add(new Ellipse(GlobalVars.Pen.Clone(), GlobalVars.ColorBrush));
            GlobalVars.Figures[GlobalVars.Figures.Count - 1].AddPoint(mousePosition);
            GlobalVars.Figures[GlobalVars.Figures.Count - 1].AddPoint(mousePosition);
        }

        public override void MouseUp(Point mousePosition)
        {
            base.MouseUp(mousePosition);

            var a = GlobalVars.Figures[GlobalVars.Figures.Count - 1].GetPoint(0);
            var b = GlobalVars.Figures[GlobalVars.Figures.Count - 1].GetPoint(1);

            GlobalVars.Figures[GlobalVars.Figures.Count - 1].SetPoint(0, new Point(Math.Min(a.X, b.X), Math.Min(a.Y, b.Y)));
            GlobalVars.Figures[GlobalVars.Figures.Count - 1].SetPoint(1, new Point(Math.Max(a.X, b.X), Math.Max(a.Y, b.Y)));

            a = GlobalVars.Figures[GlobalVars.Figures.Count - 1].GetPoint(0);
            b = GlobalVars.Figures[GlobalVars.Figures.Count - 1].GetPoint(1);

            var size = Point.Subtract(b, a);
            GlobalVars.Figures[GlobalVars.Figures.Count - 1].SetPoint(1, a + size);
        }

        public override void MouseMove(Point mousePosition)
        {
            if (isDown)
                GlobalVars.Figures[GlobalVars.Figures.Count - 1].SetPoint(0, mousePosition);
        }
    }
}
