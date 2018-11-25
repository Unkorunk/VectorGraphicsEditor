using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using VectorGraphicsEditor.Figures;

namespace VectorGraphicsEditor
{
    public static class GlobalVars
    {
        public static readonly System.Windows.Media.Pen BlackPen = new System.Windows.Media.Pen(Brushes.Black, 1.0);
        public static readonly System.Windows.Media.Pen TransparentPen = new System.Windows.Media.Pen(Brushes.Transparent, 1.0);

        public static List<Figure> Figures = new List<Figure>();
        public static System.Windows.Media.Pen pen = new System.Windows.Media.Pen(Brushes.Black, 1.0);
        public static Brush brush = new SolidColorBrush(Colors.Transparent);

        public static Size sizeCanvas;

        public static double scaleZoom = 1.0;
        public static Vector offsetPos = new Vector(0.0, 0.0);

        public static void Zooming(double delta)
        {
            if (delta + scaleZoom <= 0.0)
                delta = 0.0;

            foreach (var figure in Figures)
            {
                for (int j = 0; j < figure.points.Count; j++)
                {
                    figure.points[j] = new Point(
                        figure.points[j].X * (delta + scaleZoom) / scaleZoom,
                        figure.points[j].Y * (delta + scaleZoom) / scaleZoom
                    );
                }
            }

            scaleZoom += delta;
        }

        public static void Shearing(Vector delta)
        {
            foreach (var figure in Figures)
                for (int j = 0; j < figure.points.Count; j++)
                    figure.points[j] = Point.Add(figure.points[j], delta);
            offsetPos += delta;
        }
    }
}
