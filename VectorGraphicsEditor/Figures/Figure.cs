using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

namespace VectorGraphicsEditor.Figures
{
    public class Figure
    {
        public List<Point> points;
        public System.Windows.Media.Pen pen = new System.Windows.Media.Pen(Brushes.Black, 1.0);
        public Brush brush = new SolidColorBrush(Colors.Transparent);

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

        public virtual void Draw(DrawingContext drawingContext)
        {

        }
    }
}
