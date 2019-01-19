using System;
using System.Collections.Generic;
using System.Windows;
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

        public override string GetSVG()
        {
            var point1 = points[0];
            var point2 = points[1];

            var stroke = ((SolidColorBrush)this.pen.Brush).Color.ToString().Remove(1, 2);

            return $"<line x1=\"{point1.X:F}\" y1=\"{point1.Y:F}\" x2=\"{point2.X:F}\" y2=\"{point2.Y:F}\" style=\"stroke:{stroke};stroke-width:{Thickness:F}\" />";
        }

        public override void Draw(DrawingContext drawingContext)
        {
            if (Selected)
            {
                drawingContext.DrawRectangle(Brushes.Transparent, new Pen(Brushes.Blue, 2.0), new Rect(Transformations.GoToGlobal(points[0]), Transformations.GoToGlobal(points[1])));
            }

            drawingContext.DrawLine(this.pen, Transformations.GoToGlobal(points[0]), Transformations.GoToGlobal(points[1]));
        }

        public override object Clone()
        {
            Line figure = new Line(this.pen);

            figure.points = new List<Point>(points);
            figure._typeBrush = this._typeBrush;
            figure._typeLine = this._typeLine;
            figure.Thickness = this.Thickness;
            figure.colorBrush = this.colorBrush;

            return figure;
        }
    }
}
