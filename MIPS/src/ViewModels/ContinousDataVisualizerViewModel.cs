using OxyPlot;
using OxyPlot.Series;
using System.Collections.Generic;

namespace MIPS.src.ViewModels
{
	public class ContinousDataVisualizerViewModel : AbstractDataVisualizerViewModel
	{

		public ContinousDataVisualizerViewModel(List<DataPoint> points, 
			string property, string observation, int propType) : base(property, observation, propType)
		{
			Model = createModel(points);
		}

		protected override PlotModel createModel(List<DataPoint> points)
		{
			var plotModel = new PlotModel();
			var functionSeries = new FunctionSeries();
			functionSeries.Points.AddRange(points);
			plotModel.Series.Add(functionSeries);
			plotModel.Title = Title;
			Points = functionSeries.Points;
			return plotModel;
		}
	}
}
