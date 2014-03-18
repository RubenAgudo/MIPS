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
			categoryAxis2.Labels.Add("Tipo 1");
			categoryAxis2.Labels.Add("Tipo 2");
			categoryAxis2.Labels.Add("Tipo 3");
			categoryAxis2.Labels.Add("Tipo 4");
			categoryAxis2.Labels.Add("Tipo 5");
			categoryAxis2.Labels.Add("Tipo 6");

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

			var functionSeries1 = new StairStepSeries();
			functionSeries1.Points.Add(new DataPoint(0, 1));
			functionSeries1.Points.Add(new DataPoint(1, 0));
			functionSeries1.Points.Add(new DataPoint(2, 2));
			functionSeries1.Points.Add(new DataPoint(3, 5));
			functionSeries1.Points.Add(new DataPoint(4, 3));
			functionSeries1.Points.Add(new DataPoint(5, 4));
			plotModel1.Series.Add(functionSeries1);
			//plotModel1.DefaultXAxis.AbsoluteMinimum = 0;
			//plotModel1.DefaultXAxis.AbsoluteMaximum = 50;
			return plotModel1;
		}



	}
}
