using System.Windows;
using System.Windows.Input;
using VectorGraphicsEditor.Helpers;

namespace VectorGraphicsEditor.Tools
{
    class Hand : Tool
    {
        private Point _prevMousePoint;

        public override void MouseDown(Point mousePosition)
        {
            base.MouseDown(mousePosition);
            _prevMousePoint = mousePosition;
        }

        public override void MouseMove(Point mousePosition)
        {
            if (isDown)
            {
                Transformations.OffsetPos += Vector.Divide(Point.Subtract(mousePosition, _prevMousePoint), Transformations.ScaleZoom);
                _prevMousePoint = mousePosition;
            }
        }
    }
}
