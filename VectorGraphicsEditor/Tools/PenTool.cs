using System.Windows;
using VectorGraphicsEditor.Figures;

namespace VectorGraphicsEditor.Tools
{
    class PenTool : Tool
    {
        public override void MouseDown(Point mousePosition)
        {
            base.MouseDown(mousePosition);

            GlobalVars.Figures.Add(new MyPen(GlobalVars.Pen.Clone()));
        }

        public override void MouseMove(Point mousePosition)
        {
            if (isDown)
                GlobalVars.Figures[GlobalVars.Figures.Count - 1].AddPoint(mousePosition);
        }
    }
}
