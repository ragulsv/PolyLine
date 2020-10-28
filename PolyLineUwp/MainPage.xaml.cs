using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;
using Windows.UI.Xaml.Navigation;
using Windows.UI;
using Windows.UI.Input;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace PolyLineUwp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        PathGeometry geometry = new PathGeometry();
        PathFigure figure = new PathFigure();
        PolyLineSegment polyLineSegment = new PolyLineSegment();
        PolyLineShape polyLineShape = new PolyLineShape();
        PointCollection pointsCollection = new PointCollection();

        public MainPage()
        {
            this.InitializeComponent(); 
            Random r = new Random();
            for (int i = 0; i < 3000; i++)
            {
                var a = r.Next(100, 200);
                var b = r.Next(100, 200);
                Point pnt = new Point(a, b);
                polyLineSegment.Points.Add(pnt);
                pointsCollection.Add(pnt);
            }

            figure.StartPoint = pointsCollection[0];
            figure.Segments.Add(polyLineSegment);
            geometry.Figures.Add(figure);
            polyLineShape.PolyLinePath = new Windows.UI.Xaml.Shapes.Path();
            polyLineShape.PolyLinePath.Data = geometry;
            polyLineShape.PolyLinePath.Stroke = new SolidColorBrush(Colors.Red);
            polyLineShape.PolyLinePath.StrokeThickness = 3;

            contentControl.Content = polyLineShape.PolyLinePath;
        }

        private void Page_PointerMoved(object sender, PointerRoutedEventArgs e)
        {
            if (pointerPressed)
            {
                
                var point = e.GetCurrentPoint(this);
                var diff = new Point(pressPointer.X - point.Position.X, pressPointer.Y - point.Position.Y);
                var movePointCollection = new PointCollection();
                for (int i = 0; i < pointsCollection.Count; i++)
                {
                    var point1 = new Point(pointsCollection[i].X - diff.X, pointsCollection[i].Y - diff.Y);
                    movePointCollection.Add(point1);
                }
                (polyLineShape.PolyLinePath.Data as PathGeometry).Figures[0].StartPoint = movePointCollection[0];
                ((polyLineShape.PolyLinePath.Data as PathGeometry).Figures[0].Segments[0] as PolyLineSegment).Points = movePointCollection;
                pointsCollection.Clear();
                pointsCollection = movePointCollection;
            }
        }
        Point pressPointer;
        bool pointerPressed = false;
        private void Page_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            pointerPressed = true;
            pressPointer = e.GetCurrentPoint(this).Position;
        }

        private void Page_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            pressPointer = new Point(0, 0);
            pointerPressed = false;
        }
    }

    public class PolyLineShape : Control
    {
        public Windows.UI.Xaml.Shapes.Path PolyLinePath
        {
            get;
            set;
        }
    }
}
