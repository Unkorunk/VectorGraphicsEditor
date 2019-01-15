using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using VectorGraphicsEditor.Helpers;

namespace VectorGraphicsEditor.Figures
{
    [Serializable]
    public class Star : Figure
    {
        public List<Point> _patternList = new List<Point>();

        public List<Point> forSelectTool = new List<Point>();

        public Star(System.Windows.Media.Pen pen, Color colorBrush) : base(pen, colorBrush) { }

        public Star()
        {

        }

        public override void Draw(DrawingContext drawingContext)
        {
            forSelectTool.Clear();

            var point1 = Transformations.GoToGlobal(points[0]);
            var point2 = Transformations.GoToGlobal(points[1]);

            var size = Vector.Divide(Point.Subtract(point1, point2), 2.0);

            int[] con_ind = { 0, 3, 2, 5, 4, 7, 6, 9, 8, 1 };

            forSelectTool.Add(new Point(_patternList[con_ind[0]].X * size.X + point2.X, _patternList[con_ind[0]].Y * size.Y + point2.Y));

            PathFigure pathFigure = new PathFigure(forSelectTool[0], new List<PathSegment>(), true );

            for (int i = 1; i < con_ind.Length; i++)
            {
                var firstPoint = new Point(_patternList[con_ind[i]].X * size.X + point2.X, _patternList[con_ind[i]].Y * size.Y + point2.Y);
                forSelectTool.Add(firstPoint);
                pathFigure.Segments.Add(new LineSegment(firstPoint, true));
            }

            forSelectTool.Add(new Point(_patternList[con_ind[0]].X * size.X + point2.X, _patternList[con_ind[0]].Y * size.Y + point2.Y));

            var myGeometry = new PathGeometry();
            myGeometry.Figures.Add(pathFigure);

            drawingContext.DrawGeometry(this.brush, Selected ? new System.Windows.Media.Pen(Brushes.Blue, this.pen.Thickness) : this.pen, myGeometry);
        }
    }
}
