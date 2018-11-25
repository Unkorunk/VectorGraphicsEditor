using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace VectorGraphicsEditor
{
    class Tool
    {
        protected bool isDown = false;
        protected bool _enabled = false;

        public bool IsEnabled
        {
            set => _enabled = value;
            get => _enabled;
        }

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

        public virtual void Enable()
        {
            _enabled = true;
        }

        public virtual void Disable()
        {
            _enabled = false;
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
