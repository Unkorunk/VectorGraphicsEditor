using System.Windows;
using System.Windows.Media;
using VectorGraphicsEditor.Figures;

namespace VectorGraphicsEditor.Tools
{
    class Loupe : Tool
    {
        private Rectangle rect;
        private double ratio, widthRect, heightRect;

        public Loupe()
        {
            rect = new Rectangle(GlobalVars.BlackPen, Brushes.Transparent);
            rect.points.Add(new Point());
            rect.points.Add(new Point());
        }

        private void RecalRatio(Point mousePosition)
        {
            ratio = GlobalVars.sizeCanvas.Height / GlobalVars.sizeCanvas.Width;
            widthRect = 200;
            heightRect = widthRect * ratio;

            rect.points[0] = new Point(mousePosition.X - widthRect / 2, mousePosition.Y - heightRect / 2);
            rect.points[1] = new Point(mousePosition.X + widthRect / 2, mousePosition.Y + heightRect / 2);
        }

        public override void MouseDown(Point mousePosition)
        {
            GlobalVars.Shearing(Point.Subtract(new Point(), mousePosition));
            GlobalVars.Zooming(GlobalVars.sizeCanvas.Width / widthRect);
            GlobalVars.Shearing(new Vector(GlobalVars.sizeCanvas.Width / 2.0, GlobalVars.sizeCanvas.Height / 2.0));

            RecalRatio(mousePosition);
        }

        public override void Enable()
        {
            base.Enable();
            GlobalVars.Figures.Add(rect);
        }

        public override void Disable()
        {
            base.Disable();
            GlobalVars.Figures.Remove(rect);
        }

        public override void MouseLeave()
        {
            base.MouseLeave();
            rect.pen = GlobalVars.TransparentPen;
        }

        public override void MouseEnter()
        {
            base.MouseEnter();
            rect.pen = GlobalVars.BlackPen;
        }

        public override void MouseMove(Point mousePosition)
        {
            base.MouseMove(mousePosition);

            RecalRatio(mousePosition);
        }
    }
}
