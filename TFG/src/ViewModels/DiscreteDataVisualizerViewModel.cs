using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using System.Collections.Generic;

namespace TFG.src.ViewModels
{
    public class DiscreteDataVisualizerViewModel: AbstractDataVisualizerViewModel
    {

		public DiscreteDataVisualizerViewModel(List<DataPoint> points, 
			List<string> labels, string property, string observation, int propType) : base(property, observation, propType)
		{
			Model = createModel(points, labels);
		}

		private PlotModel createModel(List<DataPoint> points, List<string> labels)
		{
			var plotModel1 = new PlotModel();
			var functionSeries1 = new StairStepSeries();
			var categoryAxis2 = new CategoryAxis();

			categoryAxis2.Position = AxisPosition.Left;
			categoryAxis2.AxislineStyle = LineStyle.Solid;
			categoryAxis2.MinorStep = 1;
			categoryAxis2.TickStyle = TickStyle.None;
			categoryAxis2.Labels.AddRange(labels);
			plotModel1.Axes.Add(categoryAxis2);
			functionSeries1.Points.AddRange(points);
			plotModel1.Series.Add(functionSeries1);
			plotModel1.Title = Title;
			Points = functionSeries1.Points;
			return plotModel1;
		}

	}
}
