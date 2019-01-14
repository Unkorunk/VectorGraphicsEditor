using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

namespace VectorGraphicsEditor.Figures
{
    public class Figure
    {
        private Color colorBrush;

        protected List<Point> points;
        public System.Windows.Media.Pen pen = new System.Windows.Media.Pen(Brushes.Black, 1.0);
        public Brush brush = new SolidColorBrush(Colors.Transparent);
        public bool Selected = false;

        public enum TLine
        {
            Solid,
            Dashed,
            Dotted,
            DashDotted
        }

        public enum TBrush
        {
            Solid,
            Transparent,
            VerticalStripes,
            HorizontalStripes
        }

        public double Thickness
        {
            get => this.pen.Thickness;
            set => this.pen.Thickness = value;
        }

        private TLine _typeLine;
        public TLine TypeLine
        {
            get => this._typeLine;
            set
            {
                _typeLine = value;
                switch (_typeLine)
                {
                    case TLine.Solid:
                        this.pen.DashCap = PenLineCap.Flat;
                        this.pen.DashStyle = new DashStyle(new List<double> {1, 0}, 0.0);
                        break;
                    case TLine.Dashed:
                        this.pen.DashCap = PenLineCap.Flat;
                        this.pen.DashStyle = new DashStyle(new List<double> { 1, 1 }, 0.0);
                        break;
                    case TLine.Dotted:
                        this.pen.DashCap = PenLineCap.Round;
                        this.pen.DashStyle = new DashStyle(new List<double> { 1, 2 }, 0.0);
                        break;
                    case TLine.DashDotted:
                        this.pen.DashCap = PenLineCap.Round;
                        this.pen.DashStyle = new DashStyle(new List<double> { 4, 2, 1, 2 }, 0.0);
                        break;
                    default:
                        throw new Exception("Unknown type line");
                }
            }
        }

        private TBrush _typeBrush;
        public TBrush TypeBrush
        {
            get => _typeBrush;
            set
            {
                _typeBrush = value;
                switch (_typeBrush)
                {
                    case TBrush.HorizontalStripes:
                        var myBrush = new DrawingBrush();

                        var backgroundSquare =
                            new GeometryDrawing(
                                Brushes.Transparent,
                                null,
                                new RectangleGeometry(new Rect(0, 0, 100, 100)));

                        var aGeometryGroup = new GeometryGroup();
                        aGeometryGroup.Children.Add(new RectangleGeometry(new Rect(0, 0, 100, 25)));

                        var checkers = new GeometryDrawing(new SolidColorBrush(colorBrush), null, aGeometryGroup);

                        var checkersDrawingGroup = new DrawingGroup();
                        checkersDrawingGroup.Children.Add(backgroundSquare);
                        checkersDrawingGroup.Children.Add(checkers);

                        myBrush.Drawing = checkersDrawingGroup;
                        myBrush.Viewport = new Rect(0, 0, 0.1, 0.1);
                        myBrush.TileMode = TileMode.Tile;

                        this.brush = myBrush;

                        break;
                    case TBrush.Solid:
                        this.brush = new SolidColorBrush(colorBrush);
                        break;
                    case TBrush.Transparent:
                        this.brush = Brushes.Transparent;
                        break;
                    case TBrush.VerticalStripes:

                        myBrush = new DrawingBrush();

                        backgroundSquare =
                            new GeometryDrawing(
                                Brushes.Transparent,
                                null,
                                new RectangleGeometry(new Rect(0, 0, 100, 100)));

                        aGeometryGroup = new GeometryGroup();
                        aGeometryGroup.Children.Add(new RectangleGeometry(new Rect(0, 0, 25, 100)));

                        checkers = new GeometryDrawing(new SolidColorBrush(colorBrush), null, aGeometryGroup);

                        checkersDrawingGroup = new DrawingGroup();
                        checkersDrawingGroup.Children.Add(backgroundSquare);
                        checkersDrawingGroup.Children.Add(checkers);

                        myBrush.Drawing = checkersDrawingGroup;
                        myBrush.Viewport = new Rect(0, 0, 0.1, 0.1);
                        myBrush.TileMode = TileMode.Tile;

                        this.brush = myBrush;

                        break;
                    default:
                        throw new Exception("Unknown type brush");
                }
            }
        }

        public Figure()
        {
            points = new List<Point>();
        }

        public Figure(System.Windows.Media.Pen pen, Color colorBrush)
        {
            points = new List<Point>();
            this.pen = pen;
            this.colorBrush = colorBrush;
            this.brush = new SolidColorBrush(colorBrush);
        }

        public Figure(System.Windows.Media.Pen pen)
        {
            points = new List<Point>();
            this.pen = pen;
        }

        public void AddPoint(Point point)
        {
            points.Add(point);
        }

        public int CountPoint()
        {
            return points.Count;
        }

        public Point GetPoint(int index)
        {
            return points[index];
        }

        public void SetPoint(int index, Point value)
        {
            points[index] = value;
        }

        public virtual void Draw(DrawingContext drawingContext)
        {

        }
    }
}
