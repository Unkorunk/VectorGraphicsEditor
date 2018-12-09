using System.Windows.Media;
using VectorGraphicsEditor.Helpers;

namespace VectorGraphicsEditor.Figures
{
    class Line : Figure
    {
        public Line(System.Windows.Media.Pen pen) : base(pen) { }

        public override void Draw(DrawingContext drawingContext)
        {
            drawingContext.DrawLine(this.pen, Transformations.GoToGlobal(points[0]), Transformations.GoToGlobal(points[1]));
        }
    }
}
