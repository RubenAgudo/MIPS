using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows;
using TFG.src.ui.userControls;
using TFG.src.interfaces;
using Microsoft.Win32;
using TFG.src.exceptions;

namespace TFG.src.classes
{
    public class GraphicActions
    {

        private Dictionary<string, LinkedList<UC_DataVisualizer>> data;
		private LinkedList<UC_ObservationContainer> observationContainers;
		private static GraphicActions myGraphicActions;

        private GraphicActions()
        {
            data = new Dictionary<string, LinkedList<UC_DataVisualizer>>();
			observationContainers = new LinkedList<UC_ObservationContainer>();
        }

		public static GraphicActions getMyGraphicActions()
		{
			if (myGraphicActions == null)
			{
				myGraphicActions = new GraphicActions();
			}

			return myGraphicActions;
		}

        public void addLast(UC_DataVisualizer dataVisualizer)
        {
			dataVisualizer.PropertyChanged += dataVisualizer_PropertyChanged;
			LinkedList<UC_DataVisualizer> datav;
			if (data.TryGetValue(dataVisualizer.Observation, out datav))
			{
				datav.AddLast(dataVisualizer);
			}
			else
			{
				datav = new LinkedList<UC_DataVisualizer>();
				datav.AddLast(dataVisualizer);
				data.Add(dataVisualizer.Observation, datav);
			}
			
			
        }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void dataVisualizer_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName == "RangeSelection")
			{
				UC_DataVisualizer modifiedChart = (UC_DataVisualizer)sender;
				updateSelections(modifiedChart);
			}
		}


		internal void update(double p)
		{
			foreach (string key in data.Keys)
			{
				LinkedList<UC_DataVisualizer> dataVisualizers;
				data.TryGetValue(key, out dataVisualizers);
				foreach (UC_DataVisualizer datav in dataVisualizers)
				{
					datav.update(p);
				}
			}
			
		}

		/// <summary>
		/// Returns the first range of the given observation
		/// </summary>
		/// <returns></returns>
		internal double[] getRange(string observation)
		{
			double[] result = null;
			LinkedList<UC_DataVisualizer> dataVisualizers;
			if (data.TryGetValue(observation, out dataVisualizers))
			{
				double[] range = dataVisualizers.First().getRangeSelection();
				if (Math.Abs((range[1] - range[0])) > 0d)
				{
					result = range;
				}
			}

			return result;
		}

		internal void remove(UC_DataVisualizer content)
		{
			LinkedList<UC_DataVisualizer> datav;
			if (data.TryGetValue(content.Observation, out datav))
			{
				datav.Remove(content);
			}

		}

		internal void clear()
		{
			data.Clear();
		}

		internal bool exists(string observation)
		{
			return data.ContainsKey(observation);
		}

		internal void addObservationContainer(UC_ObservationContainer container)
		{
			observationContainers.AddLast(container);
		}

		internal void remove(UC_ObservationContainer content)
		{
			observationContainers.Remove(content);
		}

		internal void addToContainer(string observacion, UC_DataVisualizer dataVisualizer)
		{
			IEnumerator<UC_ObservationContainer> iterator = observationContainers.GetEnumerator();
			bool exit = false;
			while (iterator.MoveNext() && !exit)
			{
				UC_ObservationContainer container = iterator.Current;
				if(container.Observation == observacion) {
					container.addToAnchorablePane(dataVisualizer, dataVisualizer.Property);
					exit = true;
				}
			}
		}

		internal void updateSelections(UC_DataVisualizer modifiedChart)
		{

			LinkedList<UC_DataVisualizer> dataVisualizers;
			data.TryGetValue(modifiedChart.Observation, out dataVisualizers);
			foreach (UC_DataVisualizer datav in dataVisualizers)
			{
				datav.updateRangeSelection(modifiedChart.RangeSelection);
			}
		}
	}
}
