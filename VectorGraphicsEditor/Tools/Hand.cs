using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

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
                if (_enabled)
                {
                    Mouse.SetCursor(Cursors.Hand);
                }

                GlobalVars.Shearing(Point.Subtract(mousePosition, _prevMousePoint));
                _prevMousePoint = mousePosition;
            }
        }
    }
}
