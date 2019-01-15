using System;
using System.Windows.Media;
using VectorGraphicsEditor.Helpers;

namespace VectorGraphicsEditor.Figures
{
    [Serializable]
    public class Line : Figure
    {
        public Line(System.Windows.Media.Pen pen) : base(pen) { }

        public Line()
        {

        }

        public override void Draw(DrawingContext drawingContext)
        {
            drawingContext.DrawLine(Selected ? new System.Windows.Media.Pen(Brushes.Blue, this.pen.Thickness) : this.pen, Transformations.GoToGlobal(points[0]), Transformations.GoToGlobal(points[1]));
        }
    }
}
