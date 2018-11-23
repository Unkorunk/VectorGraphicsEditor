using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

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
                GlobalVars.Shearing(Point.Subtract(mousePosition, _prevMousePoint));
                _prevMousePoint = mousePosition;
            }
        }
    }
}
