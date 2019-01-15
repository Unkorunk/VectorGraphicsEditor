using System;
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

        public override void Draw(DrawingContext drawingContext)
        {
            var point1 = Transformations.GoToGlobal(points[0]);
            var point2 = Transformations.GoToGlobal(points[1]);

            var size = Point.Subtract(point2, point1);

            drawingContext.DrawRectangle(this.brush, Selected ? new System.Windows.Media.Pen(Brushes.Blue, this.pen.Thickness) : this.pen, new Rect(point1, size));
        }
    }
}
