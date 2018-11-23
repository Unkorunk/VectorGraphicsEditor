using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace VectorGraphicsEditor
{
    class Tool
    {
        protected bool isDown = false;

        public Tool()
        {

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
