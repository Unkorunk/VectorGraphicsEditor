using System;
using System.Collections.Generic;
using System.Globalization;
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

        public override object Clone()
        {
            Star figure = new Star(this.pen, ((SolidColorBrush)this.brush).Color);

            figure.points = new List<Point>(points);
            figure._patternList = new List<Point>(_patternList);
            figure._typeBrush = this._typeBrush;
            figure._typeLine = this._typeLine;
            figure.Thickness = this.Thickness;
            figure.colorBrush = this.colorBrush;

            return figure;
        }

        public override string GetSVG()
        {
            var point1 = points[0];
            var point2 = points[1];

            var size = Vector.Divide(Point.Subtract(point1, point2), 2.0);

            int[] con_ind = { 0, 3, 2, 5, 4, 7, 6, 9, 8, 1 };

            var point = new Point(_patternList[con_ind[0]].X * size.X + point2.X,
                _patternList[con_ind[0]].Y * size.Y + point2.Y);

            var svg_points = $"{point.X:F},{point.Y:F}";

            for (int i = 1; i < con_ind.Length; i++)
            {
                point = new Point(_patternList[con_ind[i]].X * size.X + point2.X, _patternList[con_ind[i]].Y * size.Y + point2.Y);

                svg_points += $" {point.X:F},{point.Y:F}";
            }

            var fill = ((SolidColorBrush) this.brush).Color.ToString().Remove(1, 2);
            var stroke = ((SolidColorBrush)this.pen.Brush).Color.ToString().Remove(1, 2);
            var alpha = ((SolidColorBrush)this.brush).Color.A / 255.0;

            return $"<polygon points=\"{svg_points}\" fill-opacity=\"{alpha:F}\" style=\"fill:{fill};stroke:{stroke};stroke-width:{Thickness:F}\" />";
        }

        public override void Draw(DrawingContext drawingContext)
        {
            forSelectTool.Clear();

            var point1 = Transformations.GoToGlobal(points[0]);
            var point2 = Transformations.GoToGlobal(points[1]);

            var size = Vector.Divide(Point.Subtract(point1, point2), 2.0);

            int[] con_ind = { 0, 3, 2, 5, 4, 7, 6, 9, 8, 1 };

            var point3 = new Point(double.MaxValue, double.MaxValue);
            var point4 = new Point(double.MinValue, double.MinValue);

            forSelectTool.Add(new Point(_patternList[con_ind[0]].X * size.X + point2.X, _patternList[con_ind[0]].Y * size.Y + point2.Y));

            point3.X = Math.Min(forSelectTool[forSelectTool.Count- 1].X, point3.X);
            point3.Y = Math.Min(forSelectTool[forSelectTool.Count - 1].Y, point3.Y);

            point4.X = Math.Max(forSelectTool[forSelectTool.Count - 1].X, point4.X);
            point4.Y = Math.Max(forSelectTool[forSelectTool.Count - 1].Y, point4.Y);

            PathFigure pathFigure = new PathFigure(forSelectTool[0], new List<PathSegment>(), true );

            for (int i = 1; i < con_ind.Length; i++)
            {
                var firstPoint = new Point(_patternList[con_ind[i]].X * size.X + point2.X, _patternList[con_ind[i]].Y * size.Y + point2.Y);
                forSelectTool.Add(firstPoint);

                point3.X = Math.Min(forSelectTool[forSelectTool.Count - 1].X, point3.X);
                point3.Y = Math.Min(forSelectTool[forSelectTool.Count - 1].Y, point3.Y);

                point4.X = Math.Max(forSelectTool[forSelectTool.Count - 1].X, point4.X);
                point4.Y = Math.Max(forSelectTool[forSelectTool.Count - 1].Y, point4.Y);

                pathFigure.Segments.Add(new LineSegment(firstPoint, true));
            }

            forSelectTool.Add(new Point(_patternList[con_ind[0]].X * size.X + point2.X, _patternList[con_ind[0]].Y * size.Y + point2.Y));

            var myGeometry = new PathGeometry();
            myGeometry.Figures.Add(pathFigure);

            if (Selected)
            {
                drawingContext.DrawRectangle(Brushes.Transparent, new Pen(Brushes.Blue, 2.0), new Rect(point3, point4));
            }

            drawingContext.DrawGeometry(this.brush, this.pen, myGeometry);
        }
    }
}
