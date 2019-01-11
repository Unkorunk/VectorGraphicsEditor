using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using VectorGraphicsEditor.Figures;
using VectorGraphicsEditor.Helpers;


namespace VectorGraphicsEditor.Tools
{
    class SelectTool : Tool
    {
        private Figure saveFigure = null;

        public override void MouseDown(Point mousePosition)
        {
            base.MouseDown(mousePosition);

            GlobalVars.Figures.Add(new Rectangle(GlobalVars.Pen.Clone(), Brushes.Transparent));
            GlobalVars.Figures[GlobalVars.Figures.Count - 1].AddPoint(mousePosition);
            GlobalVars.Figures[GlobalVars.Figures.Count - 1].AddPoint(mousePosition);
        }

        private (double k, double l) CalcLine(Point p1, Point p2)
        {
            if (Math.Abs(p2.X - p1.X) < double.Epsilon) return (1.0, 0.0);

            return ((p2.Y - p1.Y) / (p2.X - p1.X), (p2.X * p1.Y - p1.X * p2.Y) / (p2.X - p1.X));
        }

        public override void MouseUp(Point mousePosition)
        {
            base.MouseUp(mousePosition);

            var point0 = GlobalVars.Figures[GlobalVars.Figures.Count - 1].GetPoint(0);
            var point1 = GlobalVars.Figures[GlobalVars.Figures.Count - 1].GetPoint(1);

            var size = point1 - point0;

            size.X = Math.Abs(size.X);
            size.Y = Math.Abs(size.Y);

            point0.X = Math.Min(point0.X, point1.X);
            point0.Y = Math.Min(point0.Y, point1.Y);

            GlobalVars.Figures.RemoveAt(GlobalVars.Figures.Count - 1);

            foreach (var figure in GlobalVars.Figures)
            {
                if (figure is Ellipse ellipse)
                {
                    ellipse.Selected = false;

                    var (k, l) = CalcLine(point0, Point.Add(point0, new Vector(0.0, size.Y)));

                    var a = ellipse.RadiusY * ellipse.RadiusY;
                    var b = -(2 * ellipse.X * ellipse.RadiusY * ellipse.RadiusY +
                                k * ellipse.RadiusX * ellipse.RadiusX);
                    var c = ellipse.X * ellipse.X * ellipse.RadiusY * ellipse.RadiusY + l *
                               (ellipse.RadiusX * ellipse.RadiusX - ellipse.Y * ellipse.RadiusX * ellipse.RadiusX -
                                ellipse.RadiusX * ellipse.RadiusX * ellipse.RadiusY * ellipse.RadiusY);

                    var d = b * b - 4 * a * c;

                    if (a == 0 && b == 0 && c != 0 || d < 0) continue;

                    ellipse.Selected = true;
                }
            }
        }

        public override void MouseLeave()
        {
            if (isDown)
            {
                saveFigure = GlobalVars.Figures[GlobalVars.Figures.Count - 1];
                GlobalVars.Figures.RemoveAt(GlobalVars.Figures.Count - 1);
            }
        }

        public override void MouseEnter()
        {
            base.MouseEnter();

            if (isDown && saveFigure != null)
                GlobalVars.Figures.Add(saveFigure);
        }

        public override void MouseMove(Point mousePosition)
        {
            base.MouseMove(mousePosition);

            if (isDown)
            {
                GlobalVars.Figures[GlobalVars.Figures.Count - 1].SetPoint(0, mousePosition);
            }
        }
    }
}
