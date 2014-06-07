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
			Labels = labels;
			Model = createModel(points);
		}

		public List<string> Labels { get; private set; }

		protected override PlotModel createModel(List<DataPoint> points)
		{
			var plotModel = new PlotModel();
			var functionSeries = new StairStepSeries();
			var categoryAxis = new CategoryAxis();

			categoryAxis.Position = AxisPosition.Left;
			categoryAxis.AxislineStyle = LineStyle.Solid;
			categoryAxis.MinorStep = 1;
			categoryAxis.TickStyle = TickStyle.None;
			categoryAxis.Labels.AddRange(Labels);
			plotModel.Axes.Add(categoryAxis);
			functionSeries.Points.AddRange(points);
			plotModel.Series.Add(functionSeries);
			plotModel.Title = Title;
			Points = functionSeries.Points;
			return plotModel;
		}

	}
}
