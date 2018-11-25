using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using VectorGraphicsEditor.Tools;
using VectorGraphicsEditor.Windows;
using Color = VectorGraphicsEditor.GUI.Color;
using PenTool = VectorGraphicsEditor.Tools.PenTool;

namespace VectorGraphicsEditor
{
    public partial class MainWindow : Window
    {
        private readonly PlaneHost planeHost;

        private static readonly PenTool penTool = new PenTool();
        private static readonly LineTool lineTool = new LineTool();
        private static readonly EllipseTool ellipseTool = new EllipseTool();
        private static readonly RectangleTool rectangleTool = new RectangleTool();
        private static readonly RoundRectTool roundRectTool = new RoundRectTool();
        private static readonly StarTool starTool = new StarTool();
        private static readonly Loupe loupe = new Loupe();
        private static readonly Hand hand = new Hand();

        private Tool toolNow = penTool;

#if DEBUG
        private Window1 debugWindow;
#endif

        public MainWindow()
        {
            InitializeComponent();

#if DEBUG
            debugWindow = new Window1();
            debugWindow.Show();
#endif

            planeHost = new PlaneHost
            {
                VerticalAlignment = VerticalAlignment.Stretch,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                Margin = new Thickness(0, 0, 0, 0)
            };

            Canvas.Children.Add(planeHost);

            GlobalVars.sizeCanvas = new Size(792, 499);
        }

        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            var mousePosition = e.GetPosition(Canvas);

            toolNow.MouseMove(mousePosition);

            planeHost.Update();
        }

        private void Canvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var mousePosition = e.GetPosition(Canvas);

            if (e.LeftButton == MouseButtonState.Pressed)
                toolNow.MouseDown(mousePosition);

            planeHost.Update();
        }

        private void Canvas_MouseUp(object sender, MouseButtonEventArgs e)
        {
            var mousePosition = e.GetPosition(Canvas);

            if (e.LeftButton == MouseButtonState.Released)
                toolNow.MouseUp(mousePosition);

            planeHost.Update();
        }

        private readonly Dictionary<string, Tool> transform = new Dictionary<string, Tool>()
        {
            { "Pen", penTool },
            { "Line", lineTool },
            { "Ellipse", ellipseTool },
            { "Rectangle", rectangleTool },
            { "RoundRect", roundRectTool },
            { "Star", starTool },
            { "Loupe",  loupe },
            { "Hand", hand }
        };

        private void ButtonChangeTool(object sender, RoutedEventArgs e)
        {
            toolNow.Disable();
            toolNow = transform[(sender as Button).Content.ToString()];
            toolNow.Enable();
        }

#if DEBUG
        private string formatList(List<Point> points)
        {
            string msg = string.Empty;

            for (int i = 0; i < points.Count; i++)
                msg += points[i].X + " " + points[i].Y + (i != points.Count - 1 ? Environment.NewLine : "");

            return msg;
        }
#endif

        private void Canvas_MouseWheel(object sender, MouseWheelEventArgs e)
        {
#if DEBUG
            string pattern = "ZOOM:\r\n\tOld: {0}\r\n\tNew: {1}\r\n\tDelta: {2}";
            double oldZoom = GlobalVars.scaleZoom;
            double deltaZoom = e.Delta / 200.0;
#endif

            GlobalVars.Zooming(e.Delta / 200.0);
            planeHost.Update();

#if DEBUG
            if (GlobalVars.Figures.Count > 0)
                debugWindow.AppendText(string.Format(pattern, oldZoom, GlobalVars.scaleZoom, deltaZoom));
#endif
        }

        private void MainWindow_OnClosing(object sender, CancelEventArgs e)
        {
#if DEBUG
            debugWindow.Close();
#endif
        }

        private bool nowFirstColorSelect = true;

        private void SelectColorFirst(object sender, MouseButtonEventArgs e)
        {
            GridFirstColor.Background = Brushes.CadetBlue;
            GridSecondColor.Background = Brushes.Transparent;
            nowFirstColorSelect = true;
        }

        private void SelectColorSecond(object sender, MouseButtonEventArgs e)
        {
            GridFirstColor.Background = Brushes.Transparent;
            GridSecondColor.Background = Brushes.CadetBlue;
            nowFirstColorSelect = false;
        }

        private void SelectColor(object sender, MouseButtonEventArgs e)
        {
            if (nowFirstColorSelect)
            {
                GlobalVars.pen = new Pen((sender as Color).Fill, 1.0);
                FirstColor.Fill = (sender as Color).Fill;
            }
            else
            {
                GlobalVars.brush = (sender as Color).Fill;
                SecondColor.Fill = (sender as Color).Fill;
            }

            planeHost.Update();
        }

        private void ButtonClear_Click(object sender, RoutedEventArgs e)
        {
            GlobalVars.Figures.Clear();
            planeHost.Update();
        }

        private void Canvas_OnMouseLeave(object sender, MouseEventArgs e)
        {
            toolNow.MouseLeave();
            planeHost.Update();
        }

        private void Canvas_OnMouseEnter(object sender, MouseEventArgs e)
        {
            toolNow.MouseEnter();
            planeHost.Update();
        }
    }
}
