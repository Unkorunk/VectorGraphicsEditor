using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Controls;
using VectorGraphicsEditor.Tools;
using VectorGraphicsEditor.Helpers;
using Color = VectorGraphicsEditor.GUI.Color;
using PenTool = VectorGraphicsEditor.Tools.PenTool;

namespace VectorGraphicsEditor
{
    public partial class MainWindow : Window
    {
        private readonly PlaneHost planeHost;

        private static Tool[] tools =
        {
            new PenTool(),
            new LineTool(),
            new EllipseTool(),
            new RectangleTool(),
            new RoundRectTool(),
            new StarTool(),
            new Loupe(),
            new Hand()
        };

        private Tool toolNow = tools[0];
        private int toolPrev = 0;

        private bool nowFirstColorSelect = true;

        private void AddColorOnPanel(StackPanel panel, Brush brush)
        {
            var color = new Button { Background = brush, Width = 20, Height = 20 };
            color.MouseDown += SelectColor;
            panel.Children.Add(color);
        }

        private void AddToolOnPanel(Panel panel, Tool tag, string name)
        {
            var button = new Button
            {
                Content = FindResource(name),
                Tag = tag,
                VerticalAlignment = VerticalAlignment.Stretch,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                Width = 35,
                Height = 35
            };

            button.Click += ButtonChangeTool;

            panel.Children.Add(button);
        }

        public MainWindow()
        {
            InitializeComponent();

            Brush[] colors =
            {
                Brushes.Black, Brushes.Gray, Brushes.Brown, Brushes.Red, Brushes.OrangeRed, Brushes.Yellow, Brushes.Green,
                Brushes.CornflowerBlue, Brushes.Blue, Brushes.DarkViolet, Brushes.White, Brushes.WhiteSmoke, Brushes.Brown,
                Brushes.Pink, Brushes.Orange, Brushes.SandyBrown, Brushes.LightGreen, Brushes.SkyBlue, Brushes.LightSteelBlue,
                Brushes.Violet
            };

            for (int i = 0; i < colors.Length; i++)
                AddColorOnPanel(i < 10 ? FirstColorPanel : SecondColorPanel, colors[i]);

            string[] namesBtn =
            {
                "Pen", "Line", "Ellipse", "Rectangle", "RoundRect", "Star", "Loupe", "Hand"
            };

            for (var i = 0; i < namesBtn.Length; i++)
                AddToolOnPanel(ButtonPanel, tools[i], namesBtn[i]);

            planeHost = new PlaneHost
            {
                VerticalAlignment = VerticalAlignment.Stretch,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                Margin = new Thickness(0, 0, 0, 0)
            };

            Canvas.Children.Add(planeHost);

            GlobalVars.LabelScaleZoom = this.LabelScaleZoom;
            GlobalVars.LabelScaleZoom.Content = Transformations.ScaleZoom;

            GlobalVars.ScrollBarX = this.ScrollBarX;
            GlobalVars.ScrollBarY = this.ScrollBarY;

            Loaded += (sender, args) =>
            {
                GlobalVars.SizeCanvas = new Size(Canvas.ActualWidth, Canvas.ActualHeight);
                ScrollBarX.Maximum = Canvas.ActualWidth / 100;
                ScrollBarY.Maximum = Canvas.ActualHeight / 100;
            };
            SizeChanged += (sender, args) => { GlobalVars.SizeCanvas = new Size(Canvas.ActualWidth, Canvas.ActualHeight); };
        }

        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            var mousePosition = e.GetPosition(Canvas);

            if (!(toolNow is Hand))
                mousePosition = Transformations.GoToLocal(mousePosition);

            if (!(toolNow is Hand) && !(toolNow is Loupe)/* && toolNow.IsDown*/)
            {
                ScrollBarX.Minimum = Math.Min(ScrollBarX.Minimum, mousePosition.X / 100);
                ScrollBarX.Maximum = Math.Max(ScrollBarX.Maximum, mousePosition.X / 100);

                ScrollBarY.Minimum = Math.Min(ScrollBarY.Minimum, mousePosition.Y / 100);
                ScrollBarY.Maximum = Math.Max(ScrollBarY.Maximum, mousePosition.Y / 100);
            }

            toolNow.MouseMove(mousePosition);

            planeHost.Update();
        }

        private void Canvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var mousePosition = e.GetPosition(Canvas);

            if (e.ChangedButton == MouseButton.Middle && e.ButtonState == MouseButtonState.Pressed)
            {
                for (int i = 0; i < tools.Length; i++)
                {
                    if (tools[i] == toolNow)
                    {
                        toolPrev = i;
                        break;
                    }
                }

                toolNow = tools[7];
                toolNow.MouseDown(mousePosition);
            }

            if (!(toolNow is Hand))
                mousePosition = Transformations.GoToLocal(mousePosition);

            if (e.ChangedButton == MouseButton.Left && e.ButtonState == MouseButtonState.Pressed)
                toolNow.MouseDown(mousePosition);

            planeHost.Update();
        }

        private void Canvas_MouseUp(object sender, MouseButtonEventArgs e)
        {
            var mousePosition = e.GetPosition(Canvas);

            if (e.ChangedButton == MouseButton.Middle && e.ButtonState == MouseButtonState.Released)
            {
                toolNow.MouseUp(mousePosition);
                toolNow = tools[toolPrev];
            }

            if (!(toolNow is Hand))
                mousePosition = Transformations.GoToLocal(mousePosition);

            if (e.ChangedButton == MouseButton.Left && e.ButtonState == MouseButtonState.Released)
            {
                toolNow.MouseUp(mousePosition);
            }

            planeHost.Update();
        }

        private void ButtonChangeTool(object sender, RoutedEventArgs e)
        {
            toolNow = (Tool)((Button) sender).Tag;
        }

        private void Canvas_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (toolNow == tools[7] && toolNow.IsDown) return; // ???

            Transformations.ScaleZoom += e.Delta / 200.0;
            planeHost.Update();
        }

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
                GlobalVars.Pen = new Pen(((Color) sender).Fill, 1.0);
                FirstColor.Fill = ((Color) sender).Fill;
            }
            else
            {
                GlobalVars.Brush = ((Color) sender).Fill;
                SecondColor.Fill = ((Color) sender).Fill;
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

        private void ScrollBarX_Scroll(object sender, System.Windows.Controls.Primitives.ScrollEventArgs e)
        {
            Transformations.OffsetPos = new Vector(-ScrollBarX.Value * 100, Transformations.OffsetPos.Y);
            planeHost.Update();
        }

        private void ScrollBarY_Scroll(object sender, System.Windows.Controls.Primitives.ScrollEventArgs e)
        {
            Transformations.OffsetPos = new Vector(Transformations.OffsetPos.X, -ScrollBarY.Value * 100);
            planeHost.Update();
        }

        private void ButtonReset_Click(object sender, RoutedEventArgs e)
        {
            Transformations.ScaleZoom = 1.0;
            Transformations.OffsetPos = new Vector(0, 0);
            planeHost.Update();
        }
    }
}
