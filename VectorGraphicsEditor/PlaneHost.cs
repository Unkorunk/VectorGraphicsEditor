using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace VectorGraphicsEditor
{
    public class PlaneHost : FrameworkElement
    {
        private VisualCollection visualCollection;
        protected override Visual GetVisualChild(int index)
        {
            return visualCollection[index];
        }
        protected override int VisualChildrenCount => visualCollection.Count;

        public PlaneHost()
        {
            visualCollection = new VisualCollection(this);
        }

        public void Update()
        {
            visualCollection.Clear();

            DrawingVisual drawingVisual = new DrawingVisual();
            DrawingContext drawingContext = drawingVisual.RenderOpen();

            foreach (var figure in GlobalVars.Figures)
                figure.Draw(drawingContext);

            drawingContext.Close();
            visualCollection.Add(drawingVisual);
        }
    }
}
