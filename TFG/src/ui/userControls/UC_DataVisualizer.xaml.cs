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
		double startx;

        public UC_DataVisualizer()
        {
            InitializeComponent();

            oxyplot.Model = new DataVisualizerViewModel().Model;

			startx = double.NaN;

            rectangleAnnotation1 = new RectangleAnnotation();
            rectangleAnnotation1.Fill = OxyColor.FromArgb(120, 135, 206, 235);
            rectangleAnnotation1.MinimumX = 0;
            rectangleAnnotation1.MaximumX = 0;

            oxyplot.Model.Annotations.Add(rectangleAnnotation1);
            oxyplot.Model.MouseDown += Model_MouseDown;
            oxyplot.Model.MouseMove += Model_MouseMove;
            oxyplot.Model.MouseUp += Model_MouseUp;
            
			
            
        }

        public void sync(TimeSpan position)
        {
            throw new NotImplementedException();
        }

        private void Model_MouseUp(object sender, OxyMouseEventArgs e)
        {
			startx = double.NaN;
        }

        private void Model_MouseMove(object sender, OxyMouseEventArgs e)
        {
			if (e.ChangedButton == OxyMouseButton.Left && !double.IsNaN(startx))
			{
				var x = rectangleAnnotation1.InverseTransform(e.Position).X;
				rectangleAnnotation1.MinimumX = Math.Min(x, startx);
				rectangleAnnotation1.MaximumX = Math.Max(x, startx);
				rectangleAnnotation1.Text = string.Format("{0:0.00}", rectangleAnnotation1.MaximumX - rectangleAnnotation1.MinimumX);
				oxyplot.Model.Subtitle = string.Format("{0:0.00} to {1:0.00}", rectangleAnnotation1.MinimumX, rectangleAnnotation1.MaximumX);
				oxyplot.Model.RefreshPlot(true);
				e.Handled = true;
			}
        }

        private void Model_MouseDown(object sender, OxyMouseEventArgs e)
        {
			//if (e.ChangedButton == OxyMouseButton.Left)
			//{
			//	rectangleAnnotation1.MinimumX = calculatePosition(e.Position.X);
			//	Console.WriteLine(calculatePosition(e.Position.X));
			//	mouseDown = true;
			//}
			if (e.ChangedButton == OxyMouseButton.Left)
			{
				startx = rectangleAnnotation1.InverseTransform(e.Position).X;
				rectangleAnnotation1.MinimumX = startx;
				rectangleAnnotation1.MaximumX = startx;
				oxyplot.RefreshPlot(true);
				e.Handled = true;
			}
            
        }

        private double calculatePosition(double xPosition)
        {

            double visibleMax = oxyplot.Model.DefaultXAxis.ActualMaximum;
            double visibleMin = oxyplot.Model.DefaultXAxis.ActualMinimum;
            double modelWidth = oxyplot.Model.PlotArea.Width;
            return xPosition * (visibleMax - visibleMin) / modelWidth;
        }
        
    }
}
