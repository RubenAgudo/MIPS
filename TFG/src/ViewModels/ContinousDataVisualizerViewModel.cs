using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using OxyPlot.Annotations;
using TFG.src.interfaces;

namespace TFG.src.ViewModels
{
	public class ContinousDataVisualizerViewModel : AbstractDataVisualizerViewModel
	{

		public ContinousDataVisualizerViewModel(List<DataPoint> points, string title)
		{
			Title = title;
			Model = createModel(points);
		}

		private PlotModel createModel(List<DataPoint> points)
		{
			var plotModel1 = new PlotModel();
			var functionSeries1 = new FunctionSeries();
			functionSeries1.Points.AddRange(points);
			plotModel1.Series.Add(functionSeries1);
			plotModel1.Title = Title;
			Points = functionSeries1.Points;
			return plotModel1;
		}
	}
}
