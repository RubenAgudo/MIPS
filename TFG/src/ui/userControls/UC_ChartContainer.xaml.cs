using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TFG.src.classes;
using TFG.src.ViewModels;
using Xceed.Wpf.AvalonDock;
using Xceed.Wpf.AvalonDock.Layout;

namespace TFG.src.ui.userControls
{
    /// <summary>
    /// Lógica de interacción para GraphicsContainer.xaml
    /// </summary>
    public partial class UC_ChartContainer : UserControl
    {
		private HashSet<string> loaded;
		private XMLLoader xmlLoader;
        public UC_ChartContainer()
        {
            InitializeComponent();
			loaded = new HashSet<string>();
        }

        /// <summary>
        /// Method that adds to the GraphicsContainer a LayoutAnchorable that contains a Data visualizer user control
        /// </summary>
        /// <param name="objectToAdd"></param>
        private void addToAnchorablePane(UC_DataVisualizer objectToAdd, string title)
        {

            if (mainPanelChartContainer != null)
            {
				LayoutAnchorable doc = new LayoutAnchorable();
				doc.Closing += doc_Closing;
				doc.CanHide = false;
				doc.CanClose = true;
				doc.Title = title;
				doc.Content = objectToAdd;
                mainPanelChartContainer.Children.Add(doc);

            }
            else
            {
                throw new NotImplementedException();
            }
        }

		private void doc_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			LayoutAnchorable doc = (LayoutAnchorable)sender;
			UC_DataVisualizer content = (UC_DataVisualizer)doc.Content;
			GraphicActions.getMyGraphicActions().remove(content);
			loaded.Remove(content.getPropertyName());
		}

		/// <summary>
		/// Method that updates the chart progress indicator.
		/// </summary>
		/// <param name="p">The new position of the indicator</param>
		internal void Update(double p)
		{
			GraphicActions.getMyGraphicActions().update(p);
		}

		/// <summary>
		/// Loads a valid XML File an creates the charts
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void mnitLoadXML_Click(object sender, RoutedEventArgs e)
		{
			mainPanelChartContainer.Children.Clear();
			string pathToXML = XMLLoader.openXML();
			try
			{
				xmlLoader = new XMLLoader(pathToXML);
				List<string> observations = xmlLoader.getObservations();
                loaded = new HashSet<string>();
				foreach (string observation in observations)
				{
					List<string> properties = xmlLoader.getPropertiesOf(observation);

					TreeViewItem newObservation = new TreeViewItem();
					newObservation.Header = observation;

					foreach (string property in properties)
					{
						TreeViewItem newProperty = new TreeViewItem();
						newProperty.Header = property;
						newObservation.Items.Add(newProperty);
					}

					observationsAndProperties.Items.Add(newObservation);
				}

			}
			catch (FileFormatException ex)
			{
				Console.WriteLine(ex.StackTrace);
				MessageBoxResult msg = MessageBox.Show("Error validando el XML");
			}

			
			
		}

		private void observationsAndProperties_MouseDoubleClick(object sender, MouseButtonEventArgs e)
		{
			LinkedList<AbstractDataVisualizerViewModel> viewModels;
			TreeViewItem anItem = (TreeViewItem) observationsAndProperties.SelectedItem;
			
			string itemHeader = anItem.Header.ToString(), 
				parentHeader;

			
			if (!anItem.HasItems)
			{
				parentHeader = ((TreeViewItem)anItem.Parent).Header.ToString();
				viewModels = xmlLoader.LoadXMLData(itemHeader, parentHeader);
			}
			else
			{
				viewModels = xmlLoader.LoadXMLData(itemHeader);
			}


			foreach (AbstractDataVisualizerViewModel viewModel in viewModels)
			{
				//comprobamos que no sea de la clase abstracta
				if ((viewModel is ContinousDataVisualizerViewModel ||
					viewModel is DiscreteDataVisualizerViewModel) && !loaded.Contains(viewModel.Title))
				{
					loaded.Add(viewModel.Title);
					UC_DataVisualizer dataVisualizer = new UC_DataVisualizer(viewModel);
					GraphicActions.getMyGraphicActions().addLast(dataVisualizer);
					addToAnchorablePane(dataVisualizer, viewModel.Title);
				}
			}
		}

		private void mnitSave_Click(object sender, RoutedEventArgs e)
		{
			double[] range = GraphicActions.getMyGraphicActions().getRange();
			//dosomething
		}
		
	}
}
