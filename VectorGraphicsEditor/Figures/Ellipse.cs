using System;
using System.Windows;
using System.Windows.Media;
using System.Collections.Generic;
using VectorGraphicsEditor.Helpers;

namespace VectorGraphicsEditor.Figures
{
    [Serializable]
    public class Ellipse : Figure
    {
        public double X, Y;

        public double RadiusX
        {
            get => Math.Abs( Transformations.GoToGlobal((Point)(points[0] - points[1])).X / 2 );
            set
            {
                var centerX = points[0].X - (points[0] - points[1]).X / 2;
                points[0] = new Point(centerX - value, points[0].Y);
                points[1] = new Point(points[0].X + 2 * value, points[1].Y);
            }
        }
        public double RadiusY
        {
            get => Math.Abs( Transformations.GoToGlobal((Point)(points[0] - points[1])).Y / 2 );
            set
            {
                var centerY = points[0].Y - (points[0] - points[1]).Y / 2;
                points[0] = new Point(points[0].X, centerY - value);
                points[1] = new Point(points[1].X, points[0].Y + 2 * value);
            }
        }

        public Ellipse() { }

        public Ellipse(Pen pen, Color colorBrush) : base(pen, colorBrush) { }

        public override string GetSVG()
        {
            var point1 = points[0];
            var point2 = points[1];

            var size = Vector.Divide(Point.Subtract(point2, point1), 2.0);

            var center = new Point(point1.X + size.X, point1.Y + size.Y);

            var fill = ((SolidColorBrush) this.brush).Color.ToString().Remove(1, 2);
            var stroke = ((SolidColorBrush) this.pen.Brush).Color.ToString().Remove(1, 2);
            var alpha = ((SolidColorBrush)this.brush).Color.A / 255.0;

            return $"<ellipse cx=\"{center.X:F}\" cy=\"{center.Y:F}\" fill-opacity=\"{alpha:F}\" rx=\"{size.X:F}\" ry=\"{size.Y:F}\" style=\"fill:{fill};stroke:{stroke};stroke-width:{Thickness:F}\" />";
        }

        public override void Draw(DrawingContext drawingContext)
        {
            var point1 = Transformations.GoToGlobal(points[0]);
            var point2 = Transformations.GoToGlobal(points[1]);

            var size = Point.Subtract(point1, point2);

            var center = new Point(point1.X - size.X / 2, point1.Y - size.Y / 2);

            X = center.X;
            Y = center.Y;

            if (Selected)
            {
                drawingContext.DrawRectangle(Brushes.Transparent, new Pen(Brushes.Blue, 2.0), new Rect(point1, point2));
            }

            drawingContext.DrawEllipse(this.brush, this.pen, center, size.X / 2, size.Y / 2);
        }

        public override object Clone()
        {
            return new Ellipse(this.pen, ((SolidColorBrush)this.brush).Color)
            {
                points = new List<Point>(points),
                _typeBrush = this._typeBrush,
                _typeLine = this._typeLine,
                Thickness = this.Thickness,
                colorBrush = this.colorBrush,
                X = this.X,
                Y = this.Y,
                RadiusX = this.RadiusX,
                RadiusY = this.RadiusY
            };
        }
    }
}
