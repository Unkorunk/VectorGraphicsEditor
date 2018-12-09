using System.Windows.Media;
using VectorGraphicsEditor.Helpers;

namespace VectorGraphicsEditor.Figures
{
    class Pen : Figure
    {
        public Pen(System.Windows.Media.Pen pen) : base(pen) { }

        public override void Draw(DrawingContext drawingContext)
        {
            for (int i = 0; i < points.Count - 1; i++)
                drawingContext.DrawLine(this.pen, Transformations.GoToGlobal(points[i + 0]), Transformations.GoToGlobal(points[i + 1]));
        }
    }
}
