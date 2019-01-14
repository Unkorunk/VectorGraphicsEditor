using System.Windows;
using System.Windows.Media;
using VectorGraphicsEditor.Helpers;

namespace VectorGraphicsEditor.Figures
{
    class Ellipse : Figure
    {
        public double X, Y;

        public double RadiusX
        {
            get => (points[0] - points[1]).X / 2;
            set
            {
                var centerX = points[0].X - (points[0] - points[1]).X / 2;
                points[0] = new Point(centerX - value, points[0].Y);
                points[1] = new Point(points[0].X + 2 * value, points[1].Y);
            }
        }

        public double RadiusY
        {
            get => (points[0] - points[1]).Y / 2;
            set
            {
                var centerY = points[0].Y - (points[0] - points[1]).Y / 2;
                points[0] = new Point(points[0].X, centerY - value);
                points[1] = new Point(points[1].X, points[0].Y + 2 * value);
            }
        }

        public Ellipse(System.Windows.Media.Pen pen, Color colorBrush) : base(pen, colorBrush) { }

        public override void Draw(DrawingContext drawingContext)
        {
            var point1 = Transformations.GoToGlobal(points[0]);
            var point2 = Transformations.GoToGlobal(points[1]);

            var size = Point.Subtract(point1, point2);

            var center = new Point(point1.X - size.X / 2, point1.Y - size.Y / 2);

            X = center.X;
            Y = center.Y;

            drawingContext.DrawEllipse(this.brush, Selected ? new System.Windows.Media.Pen(Brushes.Blue, this.pen.Thickness) : this.pen, center, size.X / 2, size.Y / 2);
        }
    }
}
