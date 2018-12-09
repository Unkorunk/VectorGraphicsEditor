using System.Windows;
using System.Windows.Input;

namespace VectorGraphicsEditor
{
    public class Tool
    {
        protected bool isDown = false;

        public bool IsDown => isDown;

        public Tool()
        {

        }

        public virtual void MouseLeave()
        {
            
        }

        public virtual void MouseEnter()
        {
            if (Mouse.LeftButton == MouseButtonState.Released)
                isDown = false;
        }

        public virtual void MouseDown(Point mousePosition)
        {
            isDown = true;
        }

        public virtual void MouseUp(Point mousePosition)
        {
            isDown = false;
        }

        public virtual void MouseMove(Point mousePosition)
        {
            
        }
    }
}
