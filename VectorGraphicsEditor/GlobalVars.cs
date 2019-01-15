using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Xml.Serialization;
using VectorGraphicsEditor.Figures;
using Label = System.Windows.Controls.Label;

namespace VectorGraphicsEditor
{
    public static class GlobalVars
    {
        public static readonly System.Windows.Media.Pen BlackPen = new System.Windows.Media.Pen(Brushes.Black, 1.0);
        public static readonly System.Windows.Media.Pen TransparentPen = new System.Windows.Media.Pen(Brushes.Transparent, 1.0);

        public static List<Figure> Figures = new List<Figure>();

        public static string SaveData()
        {
            var xmlSerializer = new XmlSerializer(typeof(List<Figure>));
            var stringWriter = new StringWriter();
            xmlSerializer.Serialize(stringWriter, Figures);

            return stringWriter.ToString();
        }

        public static void LoadData()
        {
            if (!File.Exists("data.xml")) return;

            var xmlSerializer = new XmlSerializer(typeof(List<Figure>));
            var stringReader = new StringReader(File.ReadAllText("data.xml"));
            Figures = (List<Figure>) xmlSerializer.Deserialize(stringReader);
        }

        public static System.Windows.Media.Pen Pen = new System.Windows.Media.Pen(Brushes.Black, 1.0);
        public static Color ColorBrush = Colors.Transparent;

        public static Size SizeCanvas;

        public static ScrollBar ScrollBarX;
        public static ScrollBar ScrollBarY;

        public static Label LabelScaleZoom;

        public static Dictionary<string, List<Delegate>> Settings = new Dictionary<string, List<Delegate>>();

    }
}
