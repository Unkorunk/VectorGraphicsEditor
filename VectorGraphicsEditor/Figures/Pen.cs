using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

namespace VectorGraphicsEditor.Figures
{
    class Pen : Figure
    {
        public Pen(System.Windows.Media.Pen pen)
        {
            points = new List<Point>();
            this.pen = pen;
        }

        public override void Draw(DrawingContext drawingContext)
        {
            for (int i = 0; i < points.Count - 1; i++)
                drawingContext.DrawLine(this.pen, points[i + 0], points[i + 1]);
        }
    }
}
