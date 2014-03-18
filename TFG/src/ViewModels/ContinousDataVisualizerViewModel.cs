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

		public ContinousDataVisualizerViewModel()
		{
			Model = createModel();
		}

		protected override void loadData()
		{
			throw new NotImplementedException();
		}

		protected override PlotModel createModel()
		{
			var plotModel1 = new PlotModel();
			var functionSeries1 = new StairStepSeries();
			functionSeries1.Points.Add(new DataPoint(0, 4));
			functionSeries1.Points.Add(new DataPoint(1, 13));
			functionSeries1.Points.Add(new DataPoint(2, 15));
			functionSeries1.Points.Add(new DataPoint(3, 16));
			functionSeries1.Points.Add(new DataPoint(4, 12));
			functionSeries1.Points.Add(new DataPoint(5, 12));
			plotModel1.Series.Add(functionSeries1);
			//plotModel1.DefaultXAxis.AbsoluteMinimum = 0;
			//plotModel1.DefaultXAxis.AbsoluteMaximum = 50;
			return plotModel1;
		}
	}
}
