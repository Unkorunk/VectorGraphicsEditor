using System.Windows;
using VectorGraphicsEditor.Figures;

namespace VectorGraphicsEditor.Tools
{
    class LineTool : Tool
    {
        public override void MouseDown(Point mousePosition)
        {
            base.MouseDown(mousePosition);

            GlobalVars.Figures.Add(new Line(GlobalVars.Pen.Clone()));
            GlobalVars.Figures[GlobalVars.Figures.Count - 1].AddPoint(mousePosition);
            GlobalVars.Figures[GlobalVars.Figures.Count - 1].AddPoint(mousePosition);
        }

        public override void MouseMove(Point mousePosition)
        {
            if (isDown)
                GlobalVars.Figures[GlobalVars.Figures.Count - 1].SetPoint(1, mousePosition);
        }
    }
}
