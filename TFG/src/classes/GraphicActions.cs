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
		private static GraphicActions myGraphicActions;

        private GraphicActions()
        {
            data = new Dictionary<string, LinkedList<UC_DataVisualizer>>();
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
			LinkedList<UC_DataVisualizer> charts;
			if (data.TryGetValue(dataVisualizer.Observation, out charts))
			{
				charts.AddLast(dataVisualizer);
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
	}
}
