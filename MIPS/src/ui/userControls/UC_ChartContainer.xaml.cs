using OxyPlot;
using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Linq;
using MIPS.src.classes;
using MIPS.src.interfaces;
using MIPS.src.ViewModels;
using Xceed.Wpf.AvalonDock.Layout;
using System.Xml.Linq;

namespace MIPS.src.ui.userControls
{
    /// <summary>
    /// Lógica de interacción para GraphicsContainer.xaml
    /// </summary>
    public partial class UC_ChartContainer : UserControl, IContainer
    {
		private SortedSet<UC_DataVisualizer> datavisualizers;

		public string Observation { get; private set; }

        public UC_ChartContainer(string observation)
        {
            InitializeComponent();
			datavisualizers = new SortedSet<UC_DataVisualizer>();
			Observation = observation;
        }

        /// <summary>
        /// Method that adds to the GraphicsContainer a LayoutAnchorable that contains a Data visualizer user control
        /// </summary>
        /// <param name="objectToAdd"></param>
        public void addToAnchorablePane(UserControl objectToAdd, string Title)
        {

            if (mainPanelChartContainer != null)
            {
				UC_DataVisualizer datav = (UC_DataVisualizer)objectToAdd;
				if(datavisualizers.Add(datav))
				{
					datav.PropertyChanged += datav_PropertyChanged;
					LayoutAnchorable doc = new LayoutAnchorable();
					doc.Hiding += doc_Hiding;
					doc.CanHide = true;
					doc.CanClose = true;
					doc.Title = Title;
					doc.Content = datav;
					mainPanelChartContainer.Children.Add(doc);
				}
            }
            else
            {
                throw new NotImplementedException();
            }
        }

		/// <summary>
		/// Event handler that update the selection of the other charts
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void datav_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			if (e.PropertyName == "RangeSelection")
			{
				UC_DataVisualizer modifiedChart = (UC_DataVisualizer)sender;
				GraphicActions.getMyGraphicActions().updateSelections(modifiedChart.getRangeSelection());
			}
		}

		private void doc_Hiding(object sender, System.ComponentModel.CancelEventArgs e)
		{
			LayoutAnchorable doc = (LayoutAnchorable)sender;
			UC_DataVisualizer content = (UC_DataVisualizer)doc.Content;
			datavisualizers.Remove(content);
		}

		/// <summary>
		/// Updates all it's UC_Datavisualizers with the given range
		/// </summary>
		/// <param name="selectedRange">The range used to sync</param>
		internal void updateSelections(double[] selectedRange)
		{
			foreach(UC_DataVisualizer datav in datavisualizers) {
				datav.updateRangeSelection(selectedRange);
			}
		}

		internal void updateVideoProgress(double p)
		{
			foreach (UC_DataVisualizer datav in datavisualizers)
			{
				datav.update(p);
			}
		}

		/// <summary>
		/// Gets the range selection for this UC_ChartContainer
		/// </summary>
		/// <returns>The range selection, [0.0, 0.0] if nothing is selected or empty</returns>
		internal double[] getRangeSelection()
		{
			if (datavisualizers.Count > 0)
			{
				UC_DataVisualizer datav = datavisualizers.First();
				double[] selection = datav.getRangeSelection();
				if (selection[0] != selection[1])
				{
					return selection;
				}
				return new double[] { 0d, 0d };
			}
			return new double[] { 0d, 0d };
		}

		/// <summary>
		/// Gets the xml data for the given observation between the given interval
		/// </summary>
		/// <param name="observation">The observation to which will data be added</param>
		/// <param name="start">The start of the interval</param>
		/// <param name="end">The end of the interval</param>
		internal void getXMLDataOfObservationBetween(XElement observation, double start, double end)
		{
			//para cada propiedad de la observacion
			foreach (UC_DataVisualizer datav in datavisualizers)
			{

				//creamos una propiedad con los atributos tipo y nombrepropiedad
				XElement property = new XElement("property");
				XAttribute tipo = new XAttribute("type", datav.PropertyType);
				XAttribute nombrePropiedad = new XAttribute("name", datav.Property);

				//obtenemos todos los datapoints que esten en el rango seleccionado
				IEnumerable<DataPoint> list =
					from li in datav.Points
					where li.X >= start && li.X <= end
					select li;

				//Para cada datapoint
				foreach (DataPoint datapoint in list)
				{


					//creamos el elemento y obtenemos su valor Y
					XAttribute datap = null;
					switch (datav.PropertyType)
					{
						case AbstractDataVisualizerViewModel.CONTINOUS:
							datap = new XAttribute("value", datapoint.Y);
							break;
						case AbstractDataVisualizerViewModel.DISCRETE:
							datap = new XAttribute("value", (datapoint.Y >= 0) ? datav.Labels[(int)datapoint.Y] : "no value");
							break;
					}

					XAttribute ins = new XAttribute("ins", datapoint.X);

					//creamos el elemento instante
					XElement instante = new XElement("instant");

					instante.Add(datap);
					instante.Add(ins);

					//añadimos a la propiedad todos los instantes
					property.Add(instante);
				}

				//añadimos a la propiedad sus atributos
				property.Add(tipo);
				property.Add(nombrePropiedad);

				//añadimos a la observacion la propiedad
				observation.Add(property);

			}
		}
	}
}
