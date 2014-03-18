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
    public class DiscreteDataVisualizerViewModel: AbstractDataVisualizerViewModel
    {

        public DiscreteDataVisualizerViewModel()
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

			var categoryAxis2 = new CategoryAxis(AxisPosition.Left); ;
			categoryAxis2.AxislineStyle = LineStyle.Solid;
			categoryAxis2.MinorStep = 1;
			categoryAxis2.TickStyle = TickStyle.None;
			categoryAxis2.Labels.Add("Jul");
			categoryAxis2.Labels.Add("Aug");
			categoryAxis2.Labels.Add("Sep");
			categoryAxis2.Labels.Add("Oct");
			categoryAxis2.Labels.Add("Nov");
			categoryAxis2.Labels.Add("Dec");

			plotModel1.Axes.Add(categoryAxis2);

			var categoryAxis1 = new CategoryAxis(AxisPosition.Bottom);
			categoryAxis1.AxislineStyle = LineStyle.Solid;
			categoryAxis1.MinorStep = 1;
			categoryAxis1.TickStyle = TickStyle.None;
			categoryAxis1.Labels.Add("Jan");
			categoryAxis1.Labels.Add("Feb");
			categoryAxis1.Labels.Add("Mar");
			categoryAxis1.Labels.Add("Apr");
			categoryAxis1.Labels.Add("May");
			categoryAxis1.Labels.Add("Jun");

			plotModel1.Axes.Add(categoryAxis1);

			var functionSeries1 = new FunctionSeries();
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
