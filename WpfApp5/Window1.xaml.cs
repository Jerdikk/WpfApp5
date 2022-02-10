using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WpfApp5
{
    /// <summary>
    /// Логика взаимодействия для Window1.xaml
    /// </summary>
    /// 

    public class DrawingCanvas : Panel
    {
        private List<Visual> visuals = new List<Visual>();

        protected override Visual GetVisualChild(int index) => visuals[index];
        protected override int VisualChildrenCount
        {
            get
            {
                return visuals.Count;
            }
        }

        public void AddVisual(Visual visual)
        {
            visuals.Add(visual);

            base.AddVisualChild(visual);
            base.AddLogicalChild(visual);
        }

        public void DeleteVisual(Visual visual)
        {
            visuals.Remove(visual);

            base.RemoveVisualChild(visual);
            base.RemoveLogicalChild(visual);
        }

        public DrawingVisual GetVisual(Point point)
        {
            HitTestResult hitResult = VisualTreeHelper.HitTest(this, point);
            return hitResult.VisualHit as DrawingVisual;
        }

        private List<Visual> hits = new List<Visual>();
        public List<Visual> GetVisuals(Geometry region)
        {
            hits.Clear();
            GeometryHitTestParameters parameters = new GeometryHitTestParameters(region);
            HitTestResultCallback callback = new HitTestResultCallback(this.HitTestCallback);
            VisualTreeHelper.HitTest(this, null, callback, parameters);
            return hits;
        }

        private HitTestResultBehavior HitTestCallback(HitTestResult result)
        {
            GeometryHitTestResult geometryResult = (GeometryHitTestResult)result;
            DrawingVisual visual = result.VisualHit as DrawingVisual;
            if (visual != null &&
                geometryResult.IntersectionDetail == IntersectionDetail.FullyInside)
            {
                hits.Add(visual);
            }
            return HitTestResultBehavior.Continue;
        }

    }

    public partial class Window1 : Window
    {
        public class VisualHost : UIElement
        {
            public Visual Visual { get; set; }

            protected override int VisualChildrenCount
            {
                get { return Visual != null ? 1 : 0; }
            }

            protected override Visual GetVisualChild(int index)
            {
                return Visual;
            }
        }

        public Window1()
        {
            InitializeComponent();

            this.Activated += Window1_Activated;

            DrawTest();
        }

        private void Window1_Activated(object sender, EventArgs e)
        {

            DrawAxis();
            //throw new NotImplementedException();
        }

        void DrawAxis()
        {
            double startx = 20;
            double endy = 20;

            double h = Math.Round( grid1.ActualHeight-20);
            double w = Math.Round( grid1.ActualWidth-10-endy);

            grid1.Children.Clear();

            Path p = new Path();
            string myPathGeom = "M " + startx.ToString() + "," + h.ToString()+ " L " + startx.ToString() + "," + endy.ToString() + " M " + startx.ToString() + "," + h.ToString() + " L " + w.ToString() + "," + h.ToString() ;

            int vertRisk = (int)Math.Round(h / 10);
            int horizRisk = (int)Math.Round(w  / 10);

            for (byte i = 1; i < (horizRisk-1); i++)
            {
                myPathGeom += " M ";
                myPathGeom += (startx+i * 10).ToString() + ","+h.ToString();
                myPathGeom += " L ";
                myPathGeom += (startx+i * 10).ToString() + "," + (h-3).ToString();
            }

            for (byte i = 1; i < (vertRisk-2); i++)
            {
                myPathGeom += " M ";
                myPathGeom += "" + startx.ToString() + "," + (h-i * 10).ToString();
                myPathGeom += " L ";
                myPathGeom += "" + (startx+3).ToString() + "," + (h-i * 10).ToString();
            }

         //   myPathGeom += " M 5,15 L 10,10 L 15,15";
          //  myPathGeom += " M " + (w-5).ToString() + "," + (h-5).ToString() + " L " + w.ToString() + "," + h.ToString() + " L " + (w - 5).ToString() + "," + (h+5).ToString() ;
            myPathGeom += " M 0,0 Z";
            p.Data = Geometry.Parse(myPathGeom);
            p.StrokeThickness = 2;
            p.Stroke = Brushes.Black;

            grid1.Children.Add(p);

        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            DrawAxis();
        }

        void DrawTest()
        {
            drawTest.Children.Clear();

            DrawingVisual drawingVisual = new DrawingVisual();

            // Retrieve the DrawingContext in order to create new drawing content.
            DrawingContext drawingContext = drawingVisual.RenderOpen();

            // Create a rectangle and draw it in the DrawingContext.
            Rect rect = new Rect(new System.Windows.Point(0, 0), new System.Windows.Size(100, 100));
            drawingContext.DrawRectangle(System.Windows.Media.Brushes.Aqua, (System.Windows.Media.Pen)null, rect);

            // Persist the drawing content.
            drawingContext.Close();
            drawTest.Children.Add(new VisualHost { Visual = drawingVisual });

            DrawingCanvas cnvDraw = new DrawingCanvas();

            testDrawVisual.Children.Clear();

            cnvDraw.Children.Clear();

            Point point2 = new Point(10, 80);
            Point point3 = new Point(50, 50);
            StreamGeometry streamGeometry = new StreamGeometry();
            using (StreamGeometryContext geometryContext = streamGeometry.Open())
            {
                geometryContext.BeginFigure(new Point(1, 50), false, false);
                PointCollection points = new PointCollection
                                             {
                                                 point2,
                                                 point3
                                             };
                geometryContext.PolyLineTo(points, true, true);
            }

            DrawingVisual visual = new DrawingVisual();
            using (DrawingContext dc = visual.RenderOpen())
            {
              //  Pen drawingpen = new Pen(Brushes.Black, 3);
              //  dc.DrawLine(drawingpen, new Point(0, 50), new Point(50, 0));
              //  dc.DrawLine(drawingpen, new Point(50, 0), new Point(100, 50));
              //  dc.DrawLine(drawingpen, new Point(0, 50), new Point(100, 50));




                    dc.DrawGeometry(Brushes.LightGreen, new Pen(Brushes.Aqua, 2), streamGeometry);

            }

            cnvDraw.AddVisual(visual);

            testDrawVisual.Children.Add(cnvDraw);

        }



    }
}
