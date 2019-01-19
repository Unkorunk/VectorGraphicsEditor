using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using VectorGraphicsEditor.Helpers;

namespace VectorGraphicsEditor.Figures
{
    [Serializable]
    public class MyPen : Figure
    {
        public MyPen(Pen pen) : base(pen) { }

        public MyPen()
        {

        }

        public override string GetSVG()
        {
            var svg_points = string.Empty;

            for (var i = 0; i < points.Count - 1; i++)
                svg_points += $"{points[i].X},{points[i].Y} ";
            svg_points += $"{points[points.Count - 1].X},{points[points.Count - 1].Y}";
            
            var stroke = ((SolidColorBrush)this.pen.Brush).Color.ToString().Remove(1, 2);

            return $"<polyline points=\"{svg_points}\" style=\"fill:none;stroke:{stroke};stroke-width:{Thickness:F}\" />";
        }

        public override object Clone()
        {
            return new MyPen(this.pen)
            {
                points = new List<Point>(points),
                _typeBrush = this._typeBrush,
                _typeLine = this._typeLine,
                Thickness = this.Thickness,
                colorBrush = this.colorBrush
            };
        }

        public override void Draw(DrawingContext drawingContext)
        {
            var point1 = new Point(double.MaxValue, double.MaxValue);
            var point2 = new Point(double.MinValue, double.MinValue);

            for (int i = 0; i < points.Count - 1; i++)
            {
                var t1 = Transformations.GoToGlobal(points[i + 0]);
                var t2 = Transformations.GoToGlobal(points[i + 1]);

                point1.X = Math.Min(Math.Min(t1.X, point1.X), t2.X);
                point1.Y = Math.Min(Math.Min(t1.Y, point1.Y), t2.Y);

                point2.X = Math.Max(Math.Max(t1.X, point2.X), t2.X);
                point2.Y = Math.Max(Math.Max(t1.Y, point2.Y), t2.Y);

                drawingContext.DrawLine(this.pen, t1, t2);
            }

            if (Selected)
            {
                drawingContext.DrawRectangle(Brushes.Transparent, new Pen(Brushes.Blue, 2.0), new Rect(point1, point2));
            }
        }
    }
}
