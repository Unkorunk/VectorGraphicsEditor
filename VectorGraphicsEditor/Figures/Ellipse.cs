using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

namespace VectorGraphicsEditor.Figures
{
    class Ellipse : Figure
    {
        public Ellipse(System.Windows.Media.Pen pen, Brush brush)
        {
            points = new List<Point>();
            this.pen = pen;
            this.brush = brush;
        }

        public override void Draw(DrawingContext drawingContext)
        {
            var size = Vector.Divide(Point.Subtract(points[0], points[1]), 2.0);
            var center = Point.Add(points[1], size);

            drawingContext.DrawEllipse(this.brush, this.pen, center, size.X, size.Y);
        }
    }
}
