using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Reflection.Emit;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;
using System.Windows.Media;
using VectorGraphicsEditor.Figures;
using VectorGraphicsEditor.Tools;
using VectorGraphicsEditor.Helpers;
using Brush = System.Windows.Media.Brush;
using Brushes = System.Windows.Media.Brushes;
using Color = VectorGraphicsEditor.GUI.Color;
using Delegate = System.Delegate;
using Pen = System.Windows.Media.Pen;
using PenTool = VectorGraphicsEditor.Tools.PenTool;
using Point = System.Windows.Point;
using Size = System.Windows.Size;

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
            new Hand(),
            new SelectTool()
        };

        private Tool toolNow = tools[0];
        private int toolPrev = 0;

        private bool nowFirstColorSelect = true;

        private void AddColorOnPanel(StackPanel panel, Brush brush)
        {
            var color = new Color { Fill = brush, Width = 20, Height = 20 };
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
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");

            InitializeComponent();

            GlobalVars.Settings.Add("RadiusX", new List<Delegate>());
            GlobalVars.Settings.Add("RadiusY", new List<Delegate>());
            GlobalVars.Settings.Add("Thickness", new List<Delegate>());
            GlobalVars.Settings.Add("TypeLine", new List<Delegate>());
            GlobalVars.Settings.Add("TypeBrush", new List<Delegate>());

            foreach (var field in typeof(Figures.Figure.TLine).GetFields(BindingFlags.Public | BindingFlags.Static))
            {
                cbTypeLine.Items.Add(new TextBlock()
                {
                    Text = field.Name
                });
            }

            foreach (var field in typeof(Figures.Figure.TBrush).GetFields(BindingFlags.Public | BindingFlags.Static))
            {
                cbTypeBrush.Items.Add(new TextBlock()
                {
                    Text = field.Name
                });
            }

            Brush[] colors =
            {
                Brushes.Black, Brushes.Gray, Brushes.Brown, Brushes.Red, Brushes.OrangeRed, Brushes.Yellow, Brushes.Green,
                Brushes.CornflowerBlue, Brushes.Blue, Brushes.DarkViolet, Brushes.White, Brushes.WhiteSmoke, Brushes.Brown,
                Brushes.Pink, Brushes.Orange, Brushes.SandyBrown, Brushes.LightGreen, Brushes.SkyBlue, Brushes.LightSteelBlue,
                Brushes.Violet
            };

            for (int i = 0; i < colors.Length; i++)
                AddColorOnPanel(i < colors.Length / 2 ? FirstColorPanel : SecondColorPanel, colors[i]);

            string[] namesBtn =
            {
                "Pen", "Line", "Ellipse", "Rectangle", "RoundRect", "Star", "Loupe", "Hand", "SelectTool"
            };

            for (var i = 0; i < namesBtn.Length; i++)
                AddToolOnPanel(i < tools.Length / 2 ? FirstButtonPanel : SecondButtonPanel, tools[i], namesBtn[i]);

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

            GlobalVars.LoadData();
            planeHost.Update();
        }

        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            var mousePosition = e.GetPosition(Canvas);

            if (!(toolNow is Hand))
                mousePosition = Transformations.GoToLocal(mousePosition);

            if (!(toolNow is Hand) && !(toolNow is Loupe) && !(toolNow is SelectTool))
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
                GlobalVars.ColorBrush = (((Color) sender).Fill as SolidColorBrush).Color;
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

        private void TbThickness_GotFocus(object sender, RoutedEventArgs e)
        {
            if (tbThickness.Text == "Thickness")
            {
                tbThickness.Foreground = Brushes.Black;
                tbThickness.Text = "";
            }
        }

        private void TbThickness_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(tbThickness.Text))
            {
                tbThickness.Foreground = Brushes.Gray;
                tbThickness.Text = "Thickness";
            }
        }

        private void TbRadiusX_GotFocus(object sender, RoutedEventArgs e)
        {
            if (tbRadiusX.Text == "Radius X")
            {
                tbRadiusX.Foreground = Brushes.Black;
                tbRadiusX.Text = "";
            }
        }

        private void TbRadiusX_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(tbRadiusX.Text))
            {
                tbRadiusX.Foreground = Brushes.Gray;
                tbRadiusX.Text = "Radius X"; 
            }
        }

        private void TbRadiusY_GotFocus(object sender, RoutedEventArgs e)
        {
            if (tbRadiusY.Text == "Radius Y")
            {
                tbRadiusY.Foreground = Brushes.Black;
                tbRadiusY.Text = "";
            }
        }

        private void TbRadiusY_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(tbRadiusY.Text))
            {
                tbRadiusY.Foreground = Brushes.Gray;
                tbRadiusY.Text = "Radius Y";
            }
        }


        private void TbThickness_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!GlobalVars.Settings.ContainsKey("Thickness") || !double.TryParse(tbThickness.Text, out var result) || Math.Abs(result) < double.Epsilon) return;

            foreach (var del in GlobalVars.Settings["Thickness"])
            {
                del.DynamicInvoke(result);
            }

            planeHost.Update();
        }

        private void TbRadiusX_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!GlobalVars.Settings.ContainsKey("RadiusX") || !double.TryParse(tbRadiusX.Text, out var result) || Math.Abs(result) < double.Epsilon) return;

            foreach (var prop in GlobalVars.Settings["RadiusX"])
                prop.DynamicInvoke(result);

            planeHost.Update();
        }

        private void TbRadiusY_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!GlobalVars.Settings.ContainsKey("RadiusY") || !double.TryParse(tbRadiusY.Text, out var result) || Math.Abs(result) < double.Epsilon) return;

            foreach (var prop in GlobalVars.Settings["RadiusY"])
                prop.DynamicInvoke(result);

            planeHost.Update();
        }

        private void CbTypeLine_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!GlobalVars.Settings.ContainsKey("TypeLine")) return;

            foreach (var prop in GlobalVars.Settings["TypeLine"])
            {
                var field = typeof(Figures.Figure.TLine).GetField((cbTypeLine.SelectedItem as TextBlock).Text);
                prop.DynamicInvoke(field.GetValue(null));
            }
            planeHost.Update();
        }

        private void CbTypeBrush_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!GlobalVars.Settings.ContainsKey("TypeBrush")) return;

            foreach (var prop in GlobalVars.Settings["TypeBrush"])
            {
                var field = typeof(Figures.Figure.TBrush).GetField((cbTypeBrush.SelectedItem as TextBlock).Text);
                prop.DynamicInvoke(field.GetValue(null));
            }
            planeHost.Update();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            File.WriteAllText("data.xml", GlobalVars.SaveData());
            GlobalVars.SaveSVG("image.html");
        }

        private void Canvas_OnPreviewKeyUp(object sender, KeyEventArgs e)
        {
            
        }


        private List<Figure> copyFigures = new List<Figure>();
        private void MainWindow_OnKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.X)
            {
                copyFigures.Clear();
                for (int i = 0; i < GlobalVars.Figures.Count; i++)
                {
                    if (GlobalVars.Figures[i].Selected)
                    {
                        copyFigures.Add(GlobalVars.Figures[i]);
                        GlobalVars.Figures.RemoveAt(i);
                        i--;
                    }
                }
                planeHost.Update();
            } else if (e.Key == Key.C)
            {
                copyFigures.Clear();
                for (int i = 0; i < GlobalVars.Figures.Count; i++)
                {
                    if (GlobalVars.Figures[i].Selected)
                    {
                        copyFigures.Add(GlobalVars.Figures[i]);
                    }
                }
            }
            else if (e.Key == Key.V)
            {
                var center = new Point(0, 0);
                int length = 0;

                foreach (var figure in copyFigures)
                {
                    for (int i = 0; i < figure.points.Count; i++)
                        center = center + new Vector(figure.points[i].X, figure.points[i].Y);
                    length += figure.points.Count;
                }
                center = new Point(center.X / length, center.Y / length);

                var mousePosition = Mouse.GetPosition(Canvas);

                foreach (var figure in copyFigures)
                {
                    dynamic newFigure = figure.Clone();

                    for (int i = 0; i < figure.points.Count; i++)
                        newFigure.points[i] += (mousePosition - center);

                    GlobalVars.Figures.Add(newFigure);
                }

                planeHost.Update();
            }
        }
    }
}
