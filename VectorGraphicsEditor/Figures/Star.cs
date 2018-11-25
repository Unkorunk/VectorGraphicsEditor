using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

namespace VectorGraphicsEditor.Figures
{
    class Star : Figure
    {
        public List<Point> _patternList = new List<Point>();

        public Star(System.Windows.Media.Pen pen)
        {
            points = new List<Point>();
            this.pen = pen;
        }

        public override void Draw(DrawingContext drawingContext)
        {
            var size = Vector.Divide(Point.Subtract(points[0], points[1]), 2.0);
            int[] con_ind = { 0, 2, 4, 1, 3, 0 };
            for (int i = 0; i < con_ind.Length - 1; i++)
            {
                var firstPoint = new Point(_patternList[con_ind[i]].X * size.X + points[1].X, _patternList[con_ind[i]].Y * size.Y + points[1].Y);
                var secondPoint = new Point(_patternList[con_ind[i + 1]].X * size.X + points[1].X, _patternList[con_ind[i + 1]].Y * size.Y + points[1].Y);
                drawingContext.DrawLine(this.pen, firstPoint, secondPoint);
            }
        }
    }
}
