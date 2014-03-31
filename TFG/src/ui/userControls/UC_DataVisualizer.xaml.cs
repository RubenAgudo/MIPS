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
using System.ComponentModel;


namespace TFG.src.ui.userControls
{

    /// <summary>
    /// Lógica de interacción para UC_DataVisualizer.xaml
    /// </summary>
	public partial class UC_DataVisualizer : UserControl
    {

        private RectangleAnnotation RangeSelection;
		private RectangleAnnotation Progress;
		private double startx;

        public UC_DataVisualizer(int chartType)
        {

			//DataContext = new ContinousDataVisualizerViewModel();
			selectDataContext(chartType);

            InitializeComponent();
			startx = double.NaN;

			RangeSelection = new RectangleAnnotation();
			RangeSelection.Fill = OxyColor.FromArgb(120, 135, 206, 235);
			RangeSelection.MinimumX = 0;
			RangeSelection.MaximumX = 0;

			oxyplot.Model.Annotations.Add(RangeSelection);
			oxyplot.Model.MouseDown += Model_MouseDown;
			oxyplot.Model.MouseMove += Model_MouseMove;
			oxyplot.Model.MouseUp += Model_MouseUp;

			Progress = new RectangleAnnotation();
			Progress.Fill = OxyColor.FromArgb(100, 100, 100, 100);
			Progress.MinimumX = 0;
			Progress.MaximumX = 0;
			oxyplot.Model.Annotations.Add(Progress);
            
        }

		/// <summary>
		/// Method that selects the DataContext based on which button clicked the user
		/// </summary>
		/// <exception cref="NotImplementedException">
		/// if the option is not any of the consts in the AbstractDataVisualizerViewModel
		/// an exception is raised.
		/// </exception>
		/// <param name="chartType">The constant</param>
		private void selectDataContext(int chartType)
		{
			switch (chartType)
			{
				case AbstractDataVisualizerViewModel.CONTINOUS:
					DataContext = new ContinousDataVisualizerViewModel();
					break;
				case AbstractDataVisualizerViewModel.DISCRETE:
					DataContext = new DiscreteDataVisualizerViewModel();
					break;
				default:
					throw new NotImplementedException();
			}
		}


        private void Model_MouseUp(object sender, OxyMouseEventArgs e)
        {
			startx = double.NaN;
        }

		/// <summary>
		/// Updates the range selection only if the changed button is the left button
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
        private void Model_MouseMove(object sender, OxyMouseEventArgs e)
        {
			if (e.ChangedButton == OxyMouseButton.Left && !double.IsNaN(startx))
			{
				var x = RangeSelection.InverseTransform(e.Position).X;
				RangeSelection.MinimumX = Math.Min(x, startx);
				RangeSelection.MaximumX = Math.Max(x, startx);
				RangeSelection.Text = string.Format("{0:0.00}", RangeSelection.MaximumX - RangeSelection.MinimumX);
				oxyplot.Model.Subtitle = string.Format("{0:0.00} to {1:0.00}", RangeSelection.MinimumX, RangeSelection.MaximumX);
				oxyplot.Model.RefreshPlot(true);
				e.Handled = true;
			}
        }

        private void Model_MouseDown(object sender, OxyMouseEventArgs e)
        {
			if (e.ChangedButton == OxyMouseButton.Left)
			{
				startx = RangeSelection.InverseTransform(e.Position).X;
				RangeSelection.MinimumX = startx;
				RangeSelection.MaximumX = startx;
				oxyplot.RefreshPlot(true);
				e.Handled = true;
			}
            
        }

		/// <summary>
		/// Updates the progress of the video annotation.
		/// </summary>
		/// <param name="p">The position based on the video position</param>
		internal void update(double p)
		{
			Progress.MaximumX = p;
			oxyplot.RefreshPlot(true);
		}

		/// <summary>
		/// Saves into the BD the selected range and all the points in the selection
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void mnitSaveRange_Click(object sender, RoutedEventArgs e)
		{
			double start = RangeSelection.MinimumX, 
				end = RangeSelection.MaximumX;
			
			if (end != start) //if a range is selected
			{
								
			}
		}
	}
}
