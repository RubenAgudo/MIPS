using System.Collections.Generic;
using System.Xml.Linq;
using TFG.src.ui.userControls;
using System;

namespace TFG.src.classes
{
	/// <summary>
	/// Author: Ruben Agudo Santos
	/// Singleton class that manages all the operations that are common for the "Graphics" class,
	/// that is, the UC_ChartContainers and UC_DataVisualizer.
	/// </summary>
    public class GraphicActions
    {
		private static object myLock = new object();
		private Dictionary<string, UC_ChartContainer> chartContainers;
		private static GraphicActions myGraphicActions;
		private static int PROCESSOR_COUNT;

        private GraphicActions()
        {
			chartContainers = new Dictionary<string, UC_ChartContainer>();
			PROCESSOR_COUNT = Environment.ProcessorCount;
        }

		/// <summary>
		/// Gets the unique instance of this class
		/// </summary>
		/// <returns>The GraphicsActions instance</returns>
		public static GraphicActions getMyGraphicActions()
		{
			if (myGraphicActions == null)
			{
				lock (myLock)
				{
					if (myGraphicActions == null)
					{
						myGraphicActions = new GraphicActions();
					}
				}
			}

			return myGraphicActions;
		}

		/// <summary>
		/// Updates the progress bar of every loaded datavisualizer.
		/// The progress is synced with the progress of the video being the reference video.
		/// </summary>
		/// <param name="p"></param>
		internal void updateProgressBar(double p)
		{
			foreach (string key in chartContainers.Keys)
			{
				UC_ChartContainer aChartContainer;
				chartContainers.TryGetValue(key, out aChartContainer);

				aChartContainer.updateVideoProgress(p);

				
			}
			
		}

		/// <summary>
		/// Clears ALL data, I.E. it removes it completely.
		/// </summary>
		internal void clear()
		{
			chartContainers.Clear();
		}

		/// <summary>
		/// Returns if a given observation is currently loaded.
		/// </summary>
		/// <param name="observation">The observation to check if exists</param>
		/// <returns>True if exists, false otherwise</returns>
		internal bool exists(string observation)
		{
			return chartContainers.ContainsKey(observation);
		}

		/// <summary>
		/// Adds an UC_ChartContainer to the loaded chart containers
		/// </summary>
		/// <param name="container">The UC_ChartContainer to be loaded</param>
		internal void addObservationContainer(UC_ChartContainer container)
		{
			chartContainers.Add(container.Observation, container);
		}

		/// <summary>
		/// Removes an UC_ChartContainer from the loaded chart containers
		/// </summary>
		/// <param name="content"></param>
		internal void remove(UC_ChartContainer content)
		{
			chartContainers.Remove(content.Observation);
		}

		/// <summary>
		/// Adds a UC_DataVisualizer to the the given observation
		/// </summary>
		/// <param name="observacion">The observation to which the UC_DataVisualizer will be added</param>
		/// <param name="dataVisualizer">The UC_DataVisualizer to be added</param>
		internal void addToContainer(string observacion, UC_DataVisualizer dataVisualizer)
		{
			UC_ChartContainer theTarget;
			chartContainers.TryGetValue(observacion, out theTarget);
			theTarget.addToAnchorablePane(dataVisualizer, dataVisualizer.Property);				
		}

		/// <summary>
		/// Updates all the UC_DataVisualizers with the given range selection
		/// </summary>
		/// <param name="rangeToSyncWith">The range to be synced with</param>
		internal void updateSelections(double[] rangeToSyncWith)
		{

			UC_ChartContainer aChartContainer;
			foreach (string key in chartContainers.Keys)
			{
				chartContainers.TryGetValue(key, out aChartContainer);
				aChartContainer.updateSelections(rangeToSyncWith);
			}
			
		}

		/// <summary>
		/// Return the data currently loaded into an XML tree, that represents an interval between
		/// start and end
		/// </summary>
		/// <param name="start">The start of the interval</param>
		/// <param name="end">The end of the interval</param>
		/// <returns>The root XElement containing all the children</returns>
		internal XElement getDataForXML(double start, double end)
		{
			XElement intervalo = new XElement("interval");
			XAttribute intervalStart = new XAttribute("start", start);
			XAttribute intervalEnd = new XAttribute("end", end);

			//para cada observacion
			foreach (string key in chartContainers.Keys)
			{
				//creamos la observacion y su atributo con el nombre
				XElement observation = new XElement("observation");
				XAttribute nombreObservacion = new XAttribute("name", key);

				UC_ChartContainer aChartContainer;
				if (chartContainers.TryGetValue(key, out aChartContainer))
				{
					aChartContainer.getXMLDataOfObservationBetween(observation, start, end);
				}
				
				//añadimos a la observacion su atributo y añadimos la observacion al nodo raiz
				observation.Add(nombreObservacion);
				intervalo.Add(observation);
			}
			intervalo.Add(intervalStart);
			intervalo.Add(intervalEnd);
			return intervalo;
		}

		/// <summary>
		/// Returns the selected range
		/// </summary>
		/// <returns>The first number of the array is the start of the range and the second the end</returns>
		internal double[] getSelectedRange()
		{

			UC_ChartContainer aChartContainer;

			foreach (string key in chartContainers.Keys)
			{
				
				if (chartContainers.TryGetValue(key, out aChartContainer))
				{
					double[] selection = aChartContainer.getRangeSelection();
					if (selection[0] != selection[1])
					{
						return selection;
					}
					//keep trying maybe the chart has no properties
				}
				else
				{
					return new double[] { 0d, 0d };
				}
			}
			return new double[] { 0d, 0d };
		}
	}
}
