using OxyPlot;
using OxyPlot.Series;
using System.Collections.Generic;

namespace TFG.src.ViewModels
{
	public class ContinousDataVisualizerViewModel : AbstractDataVisualizerViewModel
	{

		public ContinousDataVisualizerViewModel(List<DataPoint> points, 
			string property, string observation, int propType) : base(property, observation, propType)
		{
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
