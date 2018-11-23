using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

namespace VectorGraphicsEditor.Figures
{
    class Rectangle : Figure
    {
        public Rectangle(System.Windows.Media.Pen pen, Brush brush)
        {
            points = new List<Point>();
            this.pen = pen;
            this.brush = brush;
        }

        public override void Draw(DrawingContext drawingContext)
        {
            var size = Point.Subtract(points[0], points[1]);

            drawingContext.DrawRectangle(this.brush, this.pen, new Rect(points[1], size));
        }
    }
}
