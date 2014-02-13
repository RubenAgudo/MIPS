using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using OxyPlot.Annotations;


namespace TFG.src.ViewModels
{
    public class DataVisualizerViewModel
    {
        RectangleAnnotation rectangleAnnotation1;

        public DataVisualizerViewModel()
        {
            //this.Title = "Example 2";
            //this.Points = new List<IDataPoint>
            //                  {
            //                      new DataPoint(0, 4),
            //                      new DataPoint(10, 13),
            //                      new DataPoint(20, 15),
            //                      new DataPoint(30, 16),
            //                      new DataPoint(40, 12),
            //                      new DataPoint(50, 12)
            //                  };

            Model = Selectrange();
        }

        public string Title {get; private set;}
        public IList<IDataPoint> Points { get; private set; }

        public PlotModel Model { get; set; }

        private PlotModel Selectrange()
        {
            var plotModel1 = new PlotModel();
            plotModel1.Subtitle = "Left click and drag to select a range.";
            plotModel1.Title = "Select range";
            var functionSeries1 = new FunctionSeries();
            functionSeries1.Points.Add(new DataPoint(0, 4));
            functionSeries1.Points.Add(new DataPoint(10, 13));
            functionSeries1.Points.Add(new DataPoint(20, 15));
            functionSeries1.Points.Add(new DataPoint(30, 16));
            functionSeries1.Points.Add(new DataPoint(40, 12));
            functionSeries1.Points.Add(new DataPoint(50, 12));
            plotModel1.Series.Add(functionSeries1);
            //plotModel1.DefaultXAxis.AbsoluteMinimum = 0;
            //plotModel1.DefaultXAxis.AbsoluteMaximum = 50;
            return plotModel1;

        }

       

    }
}
