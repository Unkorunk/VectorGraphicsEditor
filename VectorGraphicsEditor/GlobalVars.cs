using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using VectorGraphicsEditor.Figures;
using Label = System.Windows.Controls.Label;

namespace VectorGraphicsEditor
{
    public static class GlobalVars
    {
        public static readonly System.Windows.Media.Pen BlackPen = new System.Windows.Media.Pen(Brushes.Black, 1.0);
        public static readonly System.Windows.Media.Pen TransparentPen = new System.Windows.Media.Pen(Brushes.Transparent, 1.0);

        public static List<Figure> Figures = new List<Figure>();
        public static System.Windows.Media.Pen Pen = new System.Windows.Media.Pen(Brushes.Black, 1.0);
        public static Brush Brush = new SolidColorBrush(Colors.Transparent);

        public static Size SizeCanvas;

        public static ScrollBar ScrollBarX;
        public static ScrollBar ScrollBarY;

        public static Label LabelScaleZoom;
    }
}
