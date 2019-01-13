using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

namespace VectorGraphicsEditor.Figures
{
    public class Figure
    {
        protected List<Point> points;
        public System.Windows.Media.Pen pen = new System.Windows.Media.Pen(Brushes.Black, 1.0);
        public Brush brush = new SolidColorBrush(Colors.Transparent);
        public bool Selected = false;

        public Figure()
        {
            points = new List<Point>();
        }

        public Figure(System.Windows.Media.Pen pen, Brush brush)
        {
            points = new List<Point>();
            this.pen = pen;
            this.brush = brush;
        }

        public Figure(System.Windows.Media.Pen pen)
        {
            points = new List<Point>();
            this.pen = pen;
        }

        public void AddPoint(Point point)
        {
            points.Add(point);
        }

        public int CountPoint()
        {
            return points.Count;
        }

        public Point GetPoint(int index)
        {
            return points[index];
        }

        public void SetPoint(int index, Point value)
        {
            points[index] = value;
        }

        public virtual void Draw(DrawingContext drawingContext)
        {

        }
    }
}
