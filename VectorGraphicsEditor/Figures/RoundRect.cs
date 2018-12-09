using System.Windows;
using System.Windows.Media;
using VectorGraphicsEditor.Helpers;

namespace VectorGraphicsEditor.Figures
{
    class RoundRect : Figure
    {
        public RoundRect(System.Windows.Media.Pen pen, Brush brush) : base(pen, brush) { }

        public override void Draw(DrawingContext drawingContext)
        {
            var point1 = Transformations.GoToGlobal(points[0]);
            var point2 = Transformations.GoToGlobal(points[1]);

            var size = Point.Subtract(point1, point2);

            drawingContext.DrawRoundedRectangle(this.brush, this.pen, new Rect(Transformations.GoToGlobal(points[1]), size), 10.0, 10.0);
        }
    }
}
