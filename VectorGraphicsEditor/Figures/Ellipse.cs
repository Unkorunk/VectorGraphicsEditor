using System.Windows;
using System.Windows.Media;
using VectorGraphicsEditor.Helpers;

namespace VectorGraphicsEditor.Figures
{
    class Ellipse : Figure
    {
        public double X, Y, RadiusX, RadiusY;

        public Ellipse(System.Windows.Media.Pen pen, Brush brush) : base(pen, brush) { }

        public override void Draw(DrawingContext drawingContext)
        {
            var point1 = Transformations.GoToGlobal(points[0]);
            var point2 = Transformations.GoToGlobal(points[1]);

            var size = Point.Subtract(point1, point2);

            RadiusX = size.X / 2;
            RadiusY = size.Y / 2;

            var center = new Point(point1.X - size.X / 2, point1.Y - size.Y / 2);

            X = center.X;
            Y = center.Y;

            drawingContext.DrawEllipse(Selected ? Brushes.Blue : this.brush, this.pen, center, RadiusX, RadiusY);
            
        }
    }
}
