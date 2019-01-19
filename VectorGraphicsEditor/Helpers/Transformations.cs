using System;
using System.Windows;

namespace VectorGraphicsEditor.Helpers
{
    public static class Transformations
    {
        private static double scaleZoom = 1.0;
        private static Vector offsetPos = new Vector(0.0, 0.0);

        public static Vector OffsetPos
        {
            set
            {
                offsetPos = value;

                GlobalVars.ScrollBarX.Minimum = Math.Min(GlobalVars.ScrollBarX.Minimum, -offsetPos.X / 100);
                GlobalVars.ScrollBarX.Maximum = Math.Max(GlobalVars.ScrollBarX.Maximum, -offsetPos.X / 100);

                GlobalVars.ScrollBarY.Minimum = Math.Min(GlobalVars.ScrollBarY.Minimum, -offsetPos.Y / 100);
                GlobalVars.ScrollBarY.Maximum = Math.Max(GlobalVars.ScrollBarY.Maximum, -offsetPos.Y / 100);

                GlobalVars.ScrollBarX.Value = -offsetPos.X / 100;
                GlobalVars.ScrollBarY.Value = -offsetPos.Y / 100;
            } 
            get => offsetPos;
        }
        public static double ScaleZoom
        {
            set
            {
                if (value <= 0) return;

                scaleZoom = value;

                GlobalVars.LabelScaleZoom.Content = scaleZoom;
            }
            get => scaleZoom;
        }

        public static Point GoToGlobal(Point point)
        {
            return new Point((point.X + offsetPos.X) * ScaleZoom, (point.Y + OffsetPos.Y) * ScaleZoom);
        }

        public static Point GoToLocal(Point point)
        {
            return new Point(point.X / ScaleZoom - OffsetPos.X,  point.Y / ScaleZoom - OffsetPos.Y);
        }
    }
}
