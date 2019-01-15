using System;
using System.Windows;
using System.Windows.Media;
using VectorGraphicsEditor.Figures;
using VectorGraphicsEditor.Helpers;

namespace VectorGraphicsEditor.Tools
{
    class Loupe : Tool
    {
        private Figure saveFigure = null;

        public override void MouseDown(Point mousePosition)
        {
            base.MouseDown(mousePosition);

            GlobalVars.Figures.Add(new Rectangle(new System.Windows.Media.Pen(Brushes.Black, 2.0), Colors.Transparent));
            GlobalVars.Figures[GlobalVars.Figures.Count - 1].AddPoint(mousePosition);
            GlobalVars.Figures[GlobalVars.Figures.Count - 1].AddPoint(mousePosition);
        }

        public override void MouseUp(Point mousePosition)
        {
            base.MouseUp(mousePosition);

            var point0 = GlobalVars.Figures[GlobalVars.Figures.Count - 1].GetPoint(0);
            var point1 = GlobalVars.Figures[GlobalVars.Figures.Count - 1].GetPoint(1);

            var size = point1 - point0;
            size.X = Math.Abs(size.X);
            size.Y = Math.Abs(size.Y);

            var ratioCanvas = GlobalVars.SizeCanvas.Width / GlobalVars.SizeCanvas.Height;
            var ratioRect = size.X / size.Y;

            if (ratioCanvas > ratioRect)
            {
                size.Y = size.X / ratioCanvas;
            }
            else
            {
                size.X = size.Y * ratioCanvas;
            }

            GlobalVars.Figures.RemoveAt(GlobalVars.Figures.Count - 1);

            if (size.X < double.Epsilon || size.Y < double.Epsilon)
            {
                size.X = 50;
                size.Y = size.X / ratioCanvas;

                Transformations.OffsetPos = -new Vector(Math.Min(point1.X, point0.X) - 25, Math.Min(point1.Y, point0.Y) - 25);
                Transformations.ScaleZoom = GlobalVars.SizeCanvas.Height / size.Y;

                return;
            }

            if (GlobalVars.SizeCanvas.Height / size.Y > 500.0)
                return;

            Transformations.OffsetPos = -new Vector(Math.Min(point1.X, point0.X), Math.Min(point1.Y, point0.Y));
            Transformations.ScaleZoom = GlobalVars.SizeCanvas.Height / size.Y;
        }

        public override void MouseLeave()
        {
            if (isDown)
            {
                saveFigure = GlobalVars.Figures[GlobalVars.Figures.Count - 1];
                GlobalVars.Figures.RemoveAt(GlobalVars.Figures.Count - 1);
            }
        }

        public override void MouseEnter()
        {
            base.MouseEnter();

            if (isDown && saveFigure != null)
                GlobalVars.Figures.Add(saveFigure);
        }

        public override void MouseMove(Point mousePosition)
        {
            base.MouseMove(mousePosition);

            if (isDown)
            {
                GlobalVars.Figures[GlobalVars.Figures.Count - 1].SetPoint(0, mousePosition);
            }
        }
    }
}
