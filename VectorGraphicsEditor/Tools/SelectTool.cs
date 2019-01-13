using System;
using System.Windows;
using System.Windows.Media;
using VectorGraphicsEditor.Figures;

// TODO: Add check mouse in shape

namespace VectorGraphicsEditor.Tools
{
    class SelectTool : Tool
    {
        private Figure saveFigure = null;

        public override void MouseDown(Point mousePosition)
        {
            base.MouseDown(mousePosition);

            GlobalVars.Figures.Add(new Rectangle(GlobalVars.Pen.Clone(), Brushes.Transparent));
            GlobalVars.Figures[GlobalVars.Figures.Count - 1].AddPoint(mousePosition);
            GlobalVars.Figures[GlobalVars.Figures.Count - 1].AddPoint(mousePosition);
        }

        double area(Point a, Point b, Point c)
        {
            return (b.X - a.X) * (c.Y - a.Y) - (b.Y - a.Y) * (c.X - a.X);
        }
        
        void swap(ref double a, ref double b)
        {
            var t = a;
            a = b;
            b = t;
        }

        bool intersect_1(double a, double b, double c, double d)
        {
            if (a > b)
                swap(ref a, ref b);
            if (c > d)
                swap(ref c, ref d);
            return Math.Max(a, c) <= Math.Min(b, d);
        }

        private bool Intersaction(Point a, Point b, Point c, Point d)
        {
            return intersect_1(a.X, b.X, c.X, d.X)
                   && intersect_1(a.Y, b.Y, c.Y, d.Y)
                   && area(a, b, c) * area(a, b, d) <= 0
                   && area(c, d, a) * area(c, d, b) <= 0;
        }

        // TODO: Fix
        private bool Intersaction(Ellipse ellipse, Point point0, Point point1, bool sideLeftRight)
        {
            double a, b, c;

            if (!sideLeftRight)
            {
                a = ellipse.RadiusY * ellipse.RadiusY;

                b = -2 * ellipse.X * ellipse.RadiusY * ellipse.RadiusY;

                c = ellipse.X * ellipse.X * ellipse.RadiusY * ellipse.RadiusY +
                    ellipse.RadiusX * ellipse.RadiusX * (point0.Y - ellipse.Y) * (point0.Y - ellipse.Y) -
                    ellipse.RadiusX * ellipse.RadiusX * ellipse.RadiusY * ellipse.RadiusY;
            }
            else
            {
                ellipse.Y *= -1;
                point0.Y *= -1;
                point1.Y *= -1;

                a = ellipse.RadiusX * ellipse.RadiusX;

                b = -2 * ellipse.Y * ellipse.RadiusX * ellipse.RadiusX;

                c = ellipse.Y * ellipse.Y * ellipse.RadiusX * ellipse.RadiusX +
                    ellipse.RadiusY * ellipse.RadiusY * (point0.X - ellipse.X) * (point0.X - ellipse.X) -
                    ellipse.RadiusX * ellipse.RadiusX * ellipse.RadiusY * ellipse.RadiusY;

            }
        

            var d = b * b - 4 * a * c;

            Point iPoint0 = new Point(-1, -1);
            Point iPoint1 = new Point(-1, -1);

            if (a == 0)
            {
                if (b == 0)
                {
                    if (c == 0)
                    {
                        if (sideLeftRight)
                        {
                            ellipse.Y *= -1;
                            point0.Y *= -1;
                            point1.Y *= -1;
                        }

                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    iPoint0.X = iPoint0.Y = -c / b;
                }
            }
            else
            {
                iPoint0.X = iPoint0.Y = (-b - Math.Sqrt(d)) / (2 * a);
                iPoint1.X = iPoint1.Y = (-b + Math.Sqrt(d)) / (2 * a);
            }


            if (point0.X <= iPoint0.X && point1.X >= iPoint0.X ||
                point0.X <= iPoint1.X && point1.X >= iPoint1.X ||

                point0.Y <= iPoint0.Y && point1.Y >= iPoint0.Y ||
                point0.Y <= iPoint1.Y && point1.Y >= iPoint1.Y ||

                -point0.Y <= -iPoint0.Y && -point1.Y >= -iPoint0.Y ||
                -point0.Y <= -iPoint1.Y && -point1.Y >= -iPoint1.Y ||

                ((point0.X - ellipse.X) * (point0.X - ellipse.X) / (ellipse.RadiusX * ellipse.RadiusX)) +
                ((point0.Y - ellipse.Y) * (point0.Y - ellipse.Y)) / (ellipse.RadiusY * ellipse.RadiusY) <= 1
            )
            {
                if (sideLeftRight)
                {
                    ellipse.Y *= -1;
                    point0.Y *= -1;
                    point1.Y *= -1;
                }

                return true;
            }

            if (sideLeftRight)
            {
                ellipse.Y *= -1;
                point0.Y *= -1;
                point1.Y *= -1;
            }

            return false;
        }

        public override void MouseUp(Point mousePosition)
        {
            base.MouseUp(mousePosition);

            var point0 = GlobalVars.Figures[GlobalVars.Figures.Count - 1].GetPoint(0);
            var point1 = GlobalVars.Figures[GlobalVars.Figures.Count - 1].GetPoint(1);

            var size = point1 - point0;

            size.X = Math.Abs(size.X);
            size.Y = Math.Abs(size.Y);

            point0.X = Math.Min(point0.X, point1.X);
            point0.Y = Math.Min(point0.Y, point1.Y);

            GlobalVars.Figures.RemoveAt(GlobalVars.Figures.Count - 1);

            // TODO: Maybe move intersaction to shapes classes?
            for (int i = 0; i < GlobalVars.Figures.Count; i++)
            {
                var figure = GlobalVars.Figures[i];

                figure.Selected = false;

                if (figure is Ellipse ellipse)
                {
                    if (point0.X <= ellipse.X && point0.Y <= ellipse.Y && point0.X + size.X >= ellipse.X &&
                        point0.Y + size.Y >= ellipse.Y)
                    {
                        ellipse.Selected = true;
                    }
                    else
                    {
                        ellipse.Selected = Intersaction(ellipse, point0, new Point(point0.X, point0.Y + size.Y), true) ||
                                           Intersaction(ellipse, new Point(point0.X + size.X, point0.Y), new Point(point0.X + size.X, point0.Y + size.Y), true) ||
                                           Intersaction(ellipse, point0, new Point(point0.X + size.X, point0.Y), false) ||
                                           Intersaction(ellipse, new Point(point0.X, point0.Y + size.Y), new Point(point0.X + size.X, point0.Y + size.Y), false);
                    }
                }
                else if (figure is Line line)
                {
                    if (point0.X <= line.GetPoint(0).X && point0.Y <= line.GetPoint(0).Y && point0.X + size.X >= line.GetPoint(0).X &&
                        point0.Y + size.Y >= line.GetPoint(0).Y || point0.X <= line.GetPoint(1).X && point0.Y <= line.GetPoint(1).Y &&
                        point0.X + size.X >= line.GetPoint(1).X && point0.Y + size.Y >= line.GetPoint(1).Y)
                    {
                        line.Selected = true;
                    }
                    else
                    {
                        line.Selected = Intersaction(line.GetPoint(0), line.GetPoint(1), point0, new Point(point0.X, point0.Y + size.Y)) ||
                                        Intersaction(line.GetPoint(0), line.GetPoint(1), new Point(point0.X + size.X, point0.Y), new Point(point0.X + size.X, point0.Y + size.Y)) ||
                                        Intersaction(line.GetPoint(0), line.GetPoint(1), point0, new Point(point0.X + size.X, point0.Y)) ||
                                        Intersaction(line.GetPoint(0), line.GetPoint(1), new Point(point0.X, point0.Y + size.Y), new Point(point0.X + size.X, point0.Y + size.Y));
                    }
                }
                else if (figure is Figures.Pen pen)
                {
                    for (int j = 0; j < figure.CountPoint() - 1 && !pen.Selected; j++)
                    {
                        if (point0.X <= pen.GetPoint(j).X && point0.Y <= pen.GetPoint(j).Y &&
                            point0.X + size.X >= pen.GetPoint(j).X &&
                            point0.Y + size.Y >= pen.GetPoint(j).Y || point0.X <= pen.GetPoint(j + 1).X &&
                            point0.Y <= pen.GetPoint(j + 1).Y &&
                            point0.X + size.X >= pen.GetPoint(j + 1).X && point0.Y + size.Y >= pen.GetPoint(j + 1).Y)
                        {
                            pen.Selected = true;
                        }
                        else
                        {
                            pen.Selected = Intersaction(pen.GetPoint(j), pen.GetPoint(j + 1), point0, new Point(point0.X, point0.Y + size.Y)) ||
                                            Intersaction(pen.GetPoint(j), pen.GetPoint(j + 1), new Point(point0.X + size.X, point0.Y), new Point(point0.X + size.X, point0.Y + size.Y)) ||
                                            Intersaction(pen.GetPoint(j), pen.GetPoint(j + 1), point0, new Point(point0.X + size.X, point0.Y)) ||
                                            Intersaction(pen.GetPoint(j), pen.GetPoint(j + 1), new Point(point0.X, point0.Y + size.Y), new Point(point0.X + size.X, point0.Y + size.Y));
                        }
                    }
                }
                else if (figure is Rectangle rectangle)
                {
                    var startPoint = rectangle.GetPoint(0);
                    var vector = rectangle.GetPoint(1) - startPoint;

                    if (point0.X <= startPoint.X + vector.X / 2 && point0.Y <= startPoint.Y + vector.Y / 2 && point0.X + size.X >= startPoint.X + vector.X / 2 &&
                        point0.Y + size.Y >= startPoint.Y + vector.Y / 2)
                    {
                        rectangle.Selected = true;
                    }
                    else
                    {

                        rectangle.Selected = Intersaction(startPoint, startPoint + new Vector(vector.X, 0.0), point0, new Point(point0.X, point0.Y + size.Y)) ||
                                         Intersaction(startPoint, startPoint + new Vector(vector.X, 0.0), new Point(point0.X + size.X, point0.Y), new Point(point0.X + size.X, point0.Y + size.Y)) ||
                                         Intersaction(startPoint + new Vector(0.0, vector.Y), startPoint + vector, point0, new Point(point0.X, point0.Y + size.Y)) || 
                                         Intersaction(startPoint + new Vector(0.0, vector.Y), startPoint + vector, new Point(point0.X + size.X, point0.Y), new Point(point0.X + size.X, point0.Y + size.Y)) || 
                                         Intersaction(startPoint, startPoint + new Vector(0.0, vector.Y), point0, new Point(point0.X + size.X, point0.Y)) ||
                                         Intersaction(startPoint, startPoint + new Vector(0.0, vector.Y), new Point(point0.X, point0.Y + size.Y), new Point(point0.X + size.X, point0.Y + size.Y)) ||
                                         Intersaction(startPoint + new Vector(vector.X, 0.0), startPoint + vector, point0, new Point(point0.X + size.X, point0.Y)) || 
                                         Intersaction(startPoint + new Vector(vector.X, 0.0), startPoint + vector, new Point(point0.X, point0.Y + size.Y), new Point(point0.X + size.X, point0.Y + size.Y));
                    }
                }
                else if (figure is RoundRect roundRect)
                {
                    var startPoint = roundRect.GetPoint(0);
                    var vector = roundRect.GetPoint(1) - startPoint;

                    if (point0.X <= startPoint.X + vector.X / 2 && point0.Y <= startPoint.Y + vector.Y / 2 && point0.X + size.X >= startPoint.X + vector.X / 2 &&
                        point0.Y + size.Y >= startPoint.Y + vector.Y / 2)
                    {
                        roundRect.Selected = true;
                    }
                    else
                    { 
                        roundRect.Selected = Intersaction(startPoint + new Vector(roundRect.RadiusX, 0.0), startPoint + new Vector(vector.X - roundRect.RadiusX, 0.0), point0, new Point(point0.X, point0.Y + size.Y)) ||
                                         Intersaction(startPoint + new Vector(roundRect.RadiusX, 0.0), startPoint + new Vector(vector.X - roundRect.RadiusX, 0.0), new Point(point0.X + size.X, point0.Y), new Point(point0.X + size.X, point0.Y + size.Y)) ||
                                         Intersaction(startPoint + new Vector(roundRect.RadiusX, vector.Y), startPoint + vector - new Vector(roundRect.RadiusX, 0.0), point0, new Point(point0.X, point0.Y + size.Y)) ||
                                         Intersaction(startPoint + new Vector(roundRect.RadiusX, vector.Y), startPoint + vector - new Vector(roundRect.RadiusX, 0.0), new Point(point0.X + size.X, point0.Y), new Point(point0.X + size.X, point0.Y + size.Y)) ||
                                         Intersaction(startPoint + new Vector(0.0, roundRect.RadiusY), startPoint + new Vector(0.0, vector.Y - roundRect.RadiusY), point0, new Point(point0.X + size.X, point0.Y)) ||
                                         Intersaction(startPoint + new Vector(0.0, roundRect.RadiusY), startPoint + new Vector(0.0, vector.Y - roundRect.RadiusY), new Point(point0.X, point0.Y + size.Y), new Point(point0.X + size.X, point0.Y + size.Y)) ||
                                         Intersaction(startPoint + new Vector(vector.X, roundRect.RadiusY), startPoint + vector - new Vector(0.0, roundRect.RadiusY), point0, new Point(point0.X + size.X, point0.Y)) ||
                                         Intersaction(startPoint + new Vector(vector.X, roundRect.RadiusY), startPoint + vector - new Vector(0.0, roundRect.RadiusY), new Point(point0.X, point0.Y + size.Y), new Point(point0.X + size.X, point0.Y + size.Y));
                        
                        if (!roundRect.Selected)
                        {
                            var ellipse1 = new Ellipse(null, null)
                            {
                                RadiusX = roundRect.RadiusX, RadiusY = roundRect.RadiusY,
                                X = startPoint.X + roundRect.RadiusX, Y = startPoint.Y + roundRect.RadiusY
                            };
                            var ellipse2 = new Ellipse(null, null)
                            {
                                RadiusX = roundRect.RadiusX, RadiusY = roundRect.RadiusY,
                                X = startPoint.X + vector.X - roundRect.RadiusX, Y = startPoint.Y + roundRect.RadiusY
                            };
                            var ellipse3 = new Ellipse(null, null)
                            {
                                RadiusX = roundRect.RadiusX, RadiusY = roundRect.RadiusY,
                                X = startPoint.X + roundRect.RadiusX, Y = startPoint.Y + vector.Y - roundRect.RadiusY
                            };
                            var ellipse4 = new Ellipse(null, null)
                            {
                                RadiusX = roundRect.RadiusX, RadiusY = roundRect.RadiusY,
                                X = startPoint.X + vector.X - roundRect.RadiusX,
                                Y = startPoint.Y + vector.Y - roundRect.RadiusY
                            };

                            roundRect.Selected =
                                         Intersaction(ellipse1, point0, new Point(point0.X, point0.Y + size.Y), true) ||
                                         Intersaction(ellipse1, new Point(point0.X + size.X, point0.Y), new Point(point0.X + size.X, point0.Y + size.Y), true) ||
                                         Intersaction(ellipse1, point0, new Point(point0.X + size.X, point0.Y), false) ||
                                         Intersaction(ellipse1, new Point(point0.X, point0.Y + size.Y), new Point(point0.X + size.X, point0.Y + size.Y), false) ||
                                         
                                         Intersaction(ellipse2, point0, new Point(point0.X, point0.Y + size.Y), true) ||
                                         Intersaction(ellipse2, new Point(point0.X + size.X, point0.Y), new Point(point0.X + size.X, point0.Y + size.Y), true) ||
                                         Intersaction(ellipse2, point0, new Point(point0.X + size.X, point0.Y), false) ||
                                         Intersaction(ellipse2, new Point(point0.X, point0.Y + size.Y), new Point(point0.X + size.X, point0.Y + size.Y), false) ||

                                         Intersaction(ellipse3, point0, new Point(point0.X, point0.Y + size.Y), true) ||
                                         Intersaction(ellipse3, new Point(point0.X + size.X, point0.Y), new Point(point0.X + size.X, point0.Y + size.Y), true) ||
                                         Intersaction(ellipse3, point0, new Point(point0.X + size.X, point0.Y), false) ||
                                         Intersaction(ellipse3, new Point(point0.X, point0.Y + size.Y), new Point(point0.X + size.X, point0.Y + size.Y), false) ||

                                         Intersaction(ellipse4, point0, new Point(point0.X, point0.Y + size.Y), true) ||
                                         Intersaction(ellipse4, new Point(point0.X + size.X, point0.Y), new Point(point0.X + size.X, point0.Y + size.Y), true) ||
                                         Intersaction(ellipse4, point0, new Point(point0.X + size.X, point0.Y), false) ||
                                         Intersaction(ellipse4, new Point(point0.X, point0.Y + size.Y), new Point(point0.X + size.X, point0.Y + size.Y), false);
                        }
                    }
                }
                else if (figure is Star star)
                {
                    for (int j = 0; j < star.forSelectTool.Count - 1 && !star.Selected; j++)
                    {
                        if (point0.X <= star.forSelectTool[j].X && point0.Y <= star.forSelectTool[j].Y &&
                            point0.X + size.X >= star.forSelectTool[j].X &&
                            point0.Y + size.Y >= star.forSelectTool[j].Y || point0.X <= star.forSelectTool[j + 1].X &&
                            point0.Y <= star.forSelectTool[j + 1].Y &&
                            point0.X + size.X >= star.forSelectTool[j + 1].X && point0.Y + size.Y >= star.forSelectTool[j + 1].Y)
                        {
                            star.Selected = true;
                        }
                        else
                        {
                            star.Selected = Intersaction(star.forSelectTool[j], star.forSelectTool[j + 1], point0, new Point(point0.X, point0.Y + size.Y)) ||
                                           Intersaction(star.forSelectTool[j], star.forSelectTool[j + 1], new Point(point0.X + size.X, point0.Y), new Point(point0.X + size.X, point0.Y + size.Y)) ||
                                           Intersaction(star.forSelectTool[j], star.forSelectTool[j + 1], point0, new Point(point0.X + size.X, point0.Y)) ||
                                           Intersaction(star.forSelectTool[j], star.forSelectTool[j + 1], new Point(point0.X, point0.Y + size.Y), new Point(point0.X + size.X, point0.Y + size.Y));
                        }
                    }
                }
            }
        }

        public override void MouseLeave()
        {
            if (isDown)
            {
                saveFigure = GlobalVars.Figures[GlobalVars.Figures.Count - 1];
                GlobalVars.Figures.RemoveAt(GlobalVars.Figures.Count - 1);
            }
        }

        public override void MouseEnter()
        {
            base.MouseEnter();

            if (isDown && saveFigure != null)
                GlobalVars.Figures.Add(saveFigure);
        }

        public override void MouseMove(Point mousePosition)
        {
            base.MouseMove(mousePosition);

            if (isDown)
            {
                GlobalVars.Figures[GlobalVars.Figures.Count - 1].SetPoint(0, mousePosition);
            }
        }
    }
}
