using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OxyPlot;

namespace TFG.src.interfaces
{
	/// <summary>
	/// Methods and fields that all the dataVisualizer view models should have in order to function properly.
	/// </summary>
	interface IDataVisualizerVM
	{
		void loadData();
		String Title{get; set;}
		PlotModel createModel();
		IList<DataPoint> Points { get; set; }
		PlotModel Model { get; set; }
	}
}
