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

            var size = Point.Subtract(point1, point2);

            var center = new Point(point1.X - size.X / 2, point1.Y - size.Y / 2);

            drawingContext.DrawEllipse(this.brush, this.pen, center, size.X / 2, size.Y / 2);
            
        }
    }
}
