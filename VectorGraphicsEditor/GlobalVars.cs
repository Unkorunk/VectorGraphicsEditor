using System;
using System.Collections.Generic;
using System.IO;
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
        public static List<Figure> Figures = new List<Figure>();
        public static Pen Pen = new Pen(Brushes.Black, 1.0);
        public static Color ColorBrush = Colors.Transparent;

        public static Size SizeCanvas;

        public static ScrollBar ScrollBarX;
        public static ScrollBar ScrollBarY;

        public static Label LabelScaleZoom;

        public static Dictionary<string, List<Delegate>> Settings = new Dictionary<string, List<Delegate>>();

        public static string SaveData()
        {
            var xmlSerializer = new XmlSerializer(typeof(List<Figure>));
            var stringWriter = new StringWriter();
            xmlSerializer.Serialize(stringWriter, Figures);

            return stringWriter.ToString();
        }

        public static void SaveSVG(string path)
        {
            var svg = $"<svg width=\"{SizeCanvas.Width:F}\" height=\"{SizeCanvas.Height:F}\">" + Environment.NewLine;
            foreach (var figure in Figures)
                svg += "    " + figure.GetSVG() + Environment.NewLine;
            string.
            svg += "</svg>";

            File.WriteAllText(path, svg);
        }

        public static void LoadData()
        {
            if (!File.Exists("data.xml")) return;

            var xmlSerializer = new XmlSerializer(typeof(List<Figure>));
            var stringReader = new StringReader(File.ReadAllText("data.xml"));
            Figures = (List<Figure>) xmlSerializer.Deserialize(stringReader);
        }
    }
}
