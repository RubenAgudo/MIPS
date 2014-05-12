using OxyPlot;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Linq;
using System.Linq;
using TFG.src.ui.userControls;
using System;
using TFG.src.ViewModels;

namespace TFG.src.classes
{
	/// <summary>
	/// Author: Ruben Agudo Santos
	/// Singleton class that manages all the operations that are common for the "Graphics" class,
	/// that is, the UC_ChartContainers and UC_DataVisualizer.
	/// </summary>
    public class GraphicActions
    {

        private Dictionary<string, LinkedList<UC_DataVisualizer>> data;
		private LinkedList<UC_ChartContainer> chartContainers;
		private static GraphicActions myGraphicActions;

        private GraphicActions()
        {
            data = new Dictionary<string, LinkedList<UC_DataVisualizer>>();
			chartContainers = new LinkedList<UC_ChartContainer>();
        }

		/// <summary>
		/// Gets the unique instance of this class
		/// </summary>
		/// <returns>The GraphicsActions instance</returns>
		public static GraphicActions getMyGraphicActions()
		{
			if (myGraphicActions == null)
			{
				myGraphicActions = new GraphicActions();
			}

			return myGraphicActions;
		}

		/// <summary>
		/// Adds a new UC_DataVisualizer to the end of the data
		/// </summary>
		/// <param name="dataVisualizer"></param>
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
		/// Event handler that update the selection of the other charts
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

		/// <summary>
		/// Updates the progress bar of every loaded datavisualizer.
		/// The progress is synced with the progress of the video being the reference video.
		/// </summary>
		/// <param name="p"></param>
		internal void updateProgressBar(double p)
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
		/// Removes a dataVisualizer from the loaded dataVisualizers
		/// </summary>
		/// <param name="content">The UC_DataVisualizer to be deleted</param>
		internal void remove(UC_DataVisualizer content)
		{
			LinkedList<UC_DataVisualizer> datav;
			if (data.TryGetValue(content.Observation, out datav))
			{
				datav.Remove(content);
			}

		}

		/// <summary>
		/// Clears ALL data, I.E. it removes it completely.
		/// </summary>
		internal void clear()
		{
			data.Clear();
			chartContainers.Clear();
		}

		/// <summary>
		/// Returns if a given observation is currently loaded.
		/// </summary>
		/// <param name="observation">The observation to check if exists</param>
		/// <returns>True if exists, false otherwise</returns>
		internal bool exists(string observation)
		{
			return data.ContainsKey(observation);
		}

		/// <summary>
		/// Adds an UC_ChartContainer to the loaded chart containers
		/// </summary>
		/// <param name="container">The UC_ChartContainer to be loaded</param>
		internal void addObservationContainer(UC_ChartContainer container)
		{
			chartContainers.AddLast(container);
		}

		/// <summary>
		/// Removes an UC_ChartContainer from the loaded chart containers
		/// </summary>
		/// <param name="content"></param>
		internal void remove(UC_ChartContainer content)
		{
			chartContainers.Remove(content);
			data.Remove(content.Observation);
		}

		/// <summary>
		/// Adds a UC_DataVisualizer to the the given observation
		/// </summary>
		/// <param name="observacion">The observation to which the UC_DataVisualizer will be added</param>
		/// <param name="dataVisualizer">The UC_DataVisualizer to be added</param>
		internal void addToContainer(string observacion, UC_DataVisualizer dataVisualizer)
		{
			IEnumerator<UC_ChartContainer> iterator = chartContainers.GetEnumerator();
			bool exit = false;
			while (iterator.MoveNext() && !exit)
			{
				UC_ChartContainer container = iterator.Current;
				if(container.Observation == observacion) {
					container.addToAnchorablePane(dataVisualizer, dataVisualizer.Property);
					exit = true;
				}
			}
		}

		/// <summary>
		/// Updates all the UC_DataVisualizers with the modifiedCharts RangeSelection
		/// </summary>
		/// <param name="modifiedChart">The UC_DataVisualizer that raised the PropertyChanged event</param>
		private void updateSelections(UC_DataVisualizer modifiedChart)
		{

			LinkedList<UC_DataVisualizer> dataVisualizers;
			foreach (string key in data.Keys)
			{
				data.TryGetValue(key, out dataVisualizers);
				foreach (UC_DataVisualizer datav in dataVisualizers)
				{
					datav.updateRangeSelection(modifiedChart.getRangeSelection());
				}
			}
			
		}

		/// <summary>
		/// Return the data currently loaded into an XML tree
		/// </summary>
		/// <returns>The root XElement containing all the children</returns>
		internal XElement getDataForXML(string fileName, double start, double end)
		{
			XElement root = new XElement("paso");

			//para cada observacion
			foreach (string key in data.Keys)
			{
				//creamos la observacion y su atributo con el nombre
				XElement observation = new XElement("observacion");
				XAttribute nombreObservacion = new XAttribute("nombreObservacion", key);
				
				//obtenemos la lista de propiedades de esa observacion
				LinkedList<UC_DataVisualizer> dataVisualizersOfObservation;
				data.TryGetValue(key, out dataVisualizersOfObservation);

				//para cada propiedad de la observacion
				foreach(UC_DataVisualizer datav in dataVisualizersOfObservation) {

					//creamos una propiedad con los atributos tipo y nombrepropiedad
					XElement property = new XElement("propiedad");
					XAttribute tipo = new XAttribute("tipo", datav.PropertyType);
					XAttribute nombrePropiedad = new XAttribute("nombrePropiedad", datav.Property);

					//obtenemos todos los datapoints que esten en el rango seleccionado
					IEnumerable<DataPoint> list =
						from li in datav.Points
						where li.X >= start && li.X <= end
						select li;

					//Para cada datapoint
					foreach (DataPoint datapoint in list) 
					{
						//creamos el elemento y obtenemos su valor Y
						XElement datap = null; 
						switch (datav.PropertyType)
						{
							case AbstractDataVisualizerViewModel.CONTINOUS:
								datap = new XElement("data", datapoint.Y);
								break;
							case AbstractDataVisualizerViewModel.DISCRETE:
								datap = new XElement("data", datav.Labels[(int)datapoint.Y]);
								break;
						} 
						//creamos el atributo instante con el valor X (tiempo) y lo añadimos
						XAttribute instante = new XAttribute("instante", datapoint.X);
						datap.Add(instante);
						
						//añadimos a la propiedad todos los datapoints
						property.Add(datap);
					}
					//añadimos a la propiedad sus atributos
					property.Add(tipo);
					property.Add(nombrePropiedad);

					//añadimos a la observacion la propiedad
					observation.Add(property);

				}
				//añadimos a la observacion su atributo y añadimos la observacion al nodo raiz
				observation.Add(nombreObservacion);
				root.Add(observation);
			}
			//le ponemos al nodo raiz su atributo
			root.Add(new XAttribute("name", fileName));
			
			return root;
		}

		internal double[] getSelectedRange()
		{
			foreach (string key in data.Keys)
			{
				LinkedList<UC_DataVisualizer> datavisualizers;
				if (data.TryGetValue(key, out datavisualizers))
				{
					if (datavisualizers.Count > 0)
					{
						UC_DataVisualizer datav = datavisualizers.First.Value;
						double[] selection = datav.getRangeSelection();
						if (selection[0] != selection[1])
						{
							return selection;
						}
						return null;
					}
					return null;
				}
				return null;
			}
			return null;
		}
	}
}
