using System.Windows;
using System.Windows.Media;
using VectorGraphicsEditor.Helpers;

namespace VectorGraphicsEditor.Figures
{
    class Ellipse : Figure
    {
        public Ellipse(System.Windows.Media.Pen pen, Brush brush) : base(pen, brush) { }

        public override void Draw(DrawingContext drawingContext)
        {
            var point1 = Transformations.GoToGlobal(points[0]);
            var point2 = Transformations.GoToGlobal(points[1]);

            var size = Vector.Divide(Point.Subtract(point1, point2), 2.0);

            var center = Point.Add(point2, size);

            drawingContext.DrawEllipse(this.brush, this.pen, Transformations.GoToGlobal(center), size.X, size.Y);
        }
    }
}
