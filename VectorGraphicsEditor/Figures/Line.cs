using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

namespace VectorGraphicsEditor.Figures
{
    class Line : Figure
    {
        public Line(System.Windows.Media.Pen pen)
        {
            points = new List<Point>();
            this.pen = pen;
        }

        public override void Draw(DrawingContext drawingContext)
        {
            drawingContext.DrawLine(this.pen, points[0], points[1]);
        }
    }
}
