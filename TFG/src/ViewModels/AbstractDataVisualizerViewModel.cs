using OxyPlot;
using System.Collections.Generic;

namespace TFG.src.ViewModels
{
	abstract public class AbstractDataVisualizerViewModel
	{
		public const int DISCRETE = 0;
		public const int CONTINOUS = 1;

		public string Title
		{
			get;
			protected set;
		}

		public IList<DataPoint> Points
		{
			get;
			protected set;

		}

		public PlotModel Model
		{
			get;
			protected set;
		}

		public string Observation { get; protected set; }

		public string Property { get; protected set; }

		protected AbstractDataVisualizerViewModel(string property, string observation)
		{
			Title = property;
			Property = property;
			Observation = observation;
		}
	}
}
