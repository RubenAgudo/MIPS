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
using System.Windows.Navigation;
using System.Windows.Shapes;
using TFG.src.interfaces;
using TFG.src.classes;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using OxyPlot.Annotations;
using TFG.src.ViewModels;


namespace TFG.src.ui.userControls
{

    /// <summary>
    /// Lógica de interacción para UC_DataVisualizer.xaml
    /// </summary>
    public partial class UC_DataVisualizer : UserControl, ISynchronizable
    {

        RectangleAnnotation rectangleAnnotation1;
        bool mouseDown;

        public UC_DataVisualizer()
        {
            InitializeComponent();

            oxyplot.Model = new DataVisualizerViewModel().Model;
            
            rectangleAnnotation1 = new RectangleAnnotation();
            rectangleAnnotation1.Fill = OxyColor.FromArgb(120, 135, 206, 235);
            rectangleAnnotation1.MinimumX = 0;
            rectangleAnnotation1.MaximumX = 0;

            oxyplot.Model.Annotations.Add(rectangleAnnotation1);
            oxyplot.Model.MouseDown += Model_MouseDown;
            oxyplot.Model.MouseMove += Model_MouseMove;
            oxyplot.Model.MouseUp += Model_MouseUp;
            mouseDown = false;
            
        }

        public void sync(TimeSpan position)
        {
            throw new NotImplementedException();
        }

        private void Model_MouseUp(object sender, OxyMouseEventArgs e)
        {
            Console.WriteLine(oxyplot.Model.Width);
            Console.WriteLine(oxyplot.Width);
            if (e.ChangedButton == OxyMouseButton.Left) 
            { 
                Console.WriteLine("Start = " + rectangleAnnotation1.MinimumX);
                Console.WriteLine("End = " + rectangleAnnotation1.MaximumX);
                oxyplot.Model.Annotations.RemoveAt(0);
                oxyplot.Model.Annotations.Add(rectangleAnnotation1);
                mouseDown = false;
            }
        }

        private void Model_MouseMove(object sender, OxyMouseEventArgs e)
        {
            if (mouseDown)
            {
                rectangleAnnotation1.MaximumX = calculatePosition(e.Position.X);

                Console.WriteLine(calculatePosition(e.Position.X));
                oxyplot.RefreshPlot(true);
            }
        }

        private void Model_MouseDown(object sender, OxyMouseEventArgs e)
        {
            if (e.ChangedButton == OxyMouseButton.Left)
            {
                rectangleAnnotation1.MinimumX = calculatePosition(e.Position.X);
                Console.WriteLine(calculatePosition(e.Position.X));
                mouseDown = true;
            }
            
        }

        private double calculatePosition(double xPosition)
        {
            //Console.WriteLine(oxyplot.Model.PlotArea.Width);
            return xPosition * 50.2 / oxyplot.Model.PlotArea.Width;
        }
        
    }
}
