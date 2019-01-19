using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using VectorGraphicsEditor.Helpers;

namespace VectorGraphicsEditor.Figures
{
    [Serializable]
    public class Rectangle : Figure
    {
        public Rectangle(System.Windows.Media.Pen pen, Color colorBrush) : base(pen, colorBrush) { }

        public Rectangle()
        {
        }

        public override string GetSVG()
        {
            var point1 = points[0];
            var point2 = points[1];

            var size = Point.Subtract(point2, point1);

            var fill = ((SolidColorBrush)this.brush).Color.ToString().Remove(1, 2);
            var stroke = ((SolidColorBrush)this.pen.Brush).Color.ToString().Remove(1, 2);
            var alpha = ((SolidColorBrush)this.brush).Color.A / 255.0;

            var svg = $"<rect x=\"{point1.X:F}\" y=\"{point1.Y:F}\" fill-opacity=\"{alpha:F}\" width=\"{size.X:F}\" height=\"{size.Y:F}\" style=\"fill:{fill};stroke:{stroke};stroke-width:{Thickness:F}\" />";
        
            return svg;
        }

        public override object Clone()
        {
            Rectangle figure = new Rectangle(this.pen, ((SolidColorBrush)this.brush).Color);

            figure.points = new List<Point>(points);
            figure._typeBrush = this._typeBrush;
            figure._typeLine = this._typeLine;
            figure.Thickness = this.Thickness;
            figure.colorBrush = this.colorBrush;

            return figure;
        }

        public override void Draw(DrawingContext drawingContext)
        {
            var point1 = Transformations.GoToGlobal(points[0]);
            var point2 = Transformations.GoToGlobal(points[1]);

            var size = Point.Subtract(point2, point1);

            if (Selected)
            {
                drawingContext.DrawRectangle(Brushes.Transparent, new Pen(Brushes.Blue, 2.0), new Rect(point1, point2));
            }

            drawingContext.DrawRectangle(this.brush, this.pen, new Rect(point1, size));
        }
    }
}
