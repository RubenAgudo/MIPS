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
	abstract public class DataVisualizerViewModel
	{
		public const int DISCRETE = 0;
		public const int CONTINOUS = 1;

		public string Title
		{
			get;
			protected set;
		}

		public IList<IDataPoint> Points
		{
			get;
			protected set;

		}

		public PlotModel Model
		{
			get;
			protected set;
		}

		abstract protected void loadData();
		abstract protected PlotModel createModel();
	}
}
