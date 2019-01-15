using System;
using System.Windows.Media;
using VectorGraphicsEditor.Helpers;

namespace VectorGraphicsEditor.Figures
{
    [Serializable]
    public class MyPen : Figure
    {
        public MyPen(System.Windows.Media.Pen pen) : base(pen) { }

        public MyPen()
        {

        }

        public override void Draw(DrawingContext drawingContext)
        {
            for (int i = 0; i < points.Count - 1; i++)
                drawingContext.DrawLine(Selected ? new System.Windows.Media.Pen(Brushes.Blue, this.pen.Thickness) : this.pen, Transformations.GoToGlobal(points[i + 0]), Transformations.GoToGlobal(points[i + 1]));
        }
    }
}
