﻿using OxyPlot;
using OxyPlot.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Controls;
using MIPS.src.ViewModels;


namespace MIPS.src.ui.userControls
{

    /// <summary>
    /// Lógica de interacción para UC_DataVisualizer.xaml
    /// </summary>
	public partial class UC_DataVisualizer : UserControl, 
		INotifyPropertyChanged, IEquatable<UC_DataVisualizer>, IComparable<UC_DataVisualizer>
    {
		private RectangleAnnotation Progress;
		private AbstractDataVisualizerViewModel viewModel;
		private double startx;
		public event PropertyChangedEventHandler PropertyChanged;

		public string Observation { get { return viewModel.Observation; } }
		public string Property { get { return viewModel.Property; } }

		/// <summary>
		/// Gets the type of the viewModel associated to this DataVisualizer
		/// </summary>
		public int PropertyType
		{
			get
			{
				return viewModel.Type;
			}
		}

		public System.Collections.Generic.IEnumerable<DataPoint> Points {
			get
			{
				return viewModel.Points;
			}
		}

		public List<string> Labels
		{
			get
			{
				if (viewModel is DiscreteDataVisualizerViewModel)
				{
					return ((DiscreteDataVisualizerViewModel)viewModel).Labels;
				}
				return null;
			}
		}

		private RectangleAnnotation RangeSelection { get; set; }

		public UC_DataVisualizer(AbstractDataVisualizerViewModel viewModel, double start, double end)
		{
			InitializeComponent();
			this.viewModel = viewModel;
			oxyplot.Model = this.viewModel.Model;
			initialize(start, end);

			
		}

		private void NotifyPropertyChanged(string info)
		{
			if (PropertyChanged != null)
			{
				PropertyChanged(this, new PropertyChangedEventArgs(info));
			}
		}

		private void initialize(double start, double end)
		{
			startx = double.NaN;
			RangeSelection = new RectangleAnnotation();
			RangeSelection.Fill = OxyColor.FromArgb(120, 135, 206, 235);
			RangeSelection.MinimumX = start;
			RangeSelection.MaximumX = end;

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
		/// Method fired when the click is released
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
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
			
			if (!double.IsNaN(startx))
			{
				var x = RangeSelection.InverseTransform(e.Position).X;
				RangeSelection.MinimumX = Math.Min(x, startx);
				RangeSelection.MaximumX = Math.Max(x, startx);
				NotifyPropertyChanged("RangeSelection");
				oxyplot.Model.Subtitle = string.Format("{0:0.00} to {1:0.00}", RangeSelection.MinimumX, RangeSelection.MaximumX);
				oxyplot.InvalidatePlot();
				e.Handled = true;
			}
        }

        private void Model_MouseDown(object sender, OxyMouseDownEventArgs e)
        {
			if (e.ChangedButton == OxyMouseButton.Left)
			{
				startx = RangeSelection.InverseTransform(e.Position).X;
				RangeSelection.MinimumX = startx;
				RangeSelection.MaximumX = startx;
				oxyplot.InvalidatePlot();
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
			oxyplot.InvalidatePlot();

		}

		internal double[] getRangeSelection()
		{
			return new double[] { RangeSelection.MinimumX, RangeSelection.MaximumX };
		}

		internal void updateRangeSelection(double[] range)
		{
			RangeSelection.MaximumX = range[1];
			RangeSelection.MinimumX = range[0];
			oxyplot.Model.Subtitle = string.Format("{0:0.00} to {1:0.00}", RangeSelection.MinimumX, RangeSelection.MaximumX);
			oxyplot.InvalidatePlot();
		}


		public bool Equals(UC_DataVisualizer other)
		{
			//dos datavisualizers son iguales si tienen la misma propiedad y observacion
			return this.Property == other.Property && this.Observation == other.Observation;
		}

		public int CompareTo(UC_DataVisualizer other)
		{
			return Property.CompareTo(other.Property);
		}
	}
}
