using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using VectorGraphicsEditor.Helpers;

namespace VectorGraphicsEditor.Figures
{
    [Serializable]
    public class RoundRect : Figure
    {
        public double RadiusX = 10, RadiusY = 10;

        public RoundRect(System.Windows.Media.Pen pen, Color colorBrush) : base(pen, colorBrush) { }

        public RoundRect()
        {
        }

        public override string GetSVG()
        {
            var point1 = points[0];

            var size = Point.Subtract(points[1], point1);

            var fill = ((SolidColorBrush)this.brush).Color.ToString().Remove(1, 2);
            var stroke = ((SolidColorBrush)this.pen.Brush).Color.ToString().Remove(1, 2);
            var alpha = ((SolidColorBrush)this.brush).Color.A / 255.0;

            return $"<rect x=\"{point1.X:F}\" y=\"{point1.Y:F}\" rx=\"{RadiusX:F}\" ry =\"{RadiusY:F}\" width=\"{size.X:F}\" height=\"{size.Y:F}\" fill-opacity=\"{alpha:F}\" style=\"fill:{fill};stroke:{stroke};stroke-width:{Thickness:F}\" />";
        }

        public override object Clone()
        {
            return new RoundRect(this.pen, ((SolidColorBrush)this.brush).Color)
            {
                points = new List<Point>(points),
                _typeBrush = this._typeBrush,
                _typeLine = this._typeLine,
                Thickness = this.Thickness,
                colorBrush = this.colorBrush,
                RadiusX = this.RadiusX,
                RadiusY = this.RadiusY
            };
        }

        public override void Draw(DrawingContext drawingContext)
        {
            var point1 = Transformations.GoToGlobal(points[0]);
            var point2 = Transformations.GoToGlobal(points[1]);

            var size = Point.Subtract(point1, point2);

            if (Selected)
            {
                drawingContext.DrawRectangle(Brushes.Transparent, new Pen(Brushes.Blue, 2.0), new Rect(point1, point2));
            }

            drawingContext.DrawRoundedRectangle(this.brush, this.pen, new Rect(Transformations.GoToGlobal(points[1]), size), RadiusX, RadiusY);
        }
    }
}
