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
        private int paneNumber;

        public UC_ChartContainer()
        {
            InitializeComponent();

            paneNumber = 0;
        }

        /// <summary>
        /// Method that adds to the GraphicsContainer a LayoutAnchorable that contains a Data visualizer user control
        /// </summary>
        /// <param name="objectToAdd"></param>
        private void addToAnchorablePane(UserControl objectToAdd)
        {

            if (mainPanelChartContainer != null)
            {
				LayoutAnchorable doc = new LayoutAnchorable();
				doc.CanHide = false;
				doc.CanClose = true;
				//doc.Title = "Pane " + ++paneNumber;
				doc.Content = objectToAdd;
                mainPanelChartContainer.Children.Add(doc);

            }
            else
            {
                throw new NotImplementedException();
            }
        }

		/// <summary>
		/// Method that updates the chart progress indicator.
		/// </summary>
		/// <param name="p">The new position of the indicator</param>
		internal void Update(double p)
		{
			GraphicActions.getMyGraphicActions().update(p);
		}

		private void mnitAddContinousChart_Click(object sender, RoutedEventArgs e)
		{
			UC_DataVisualizer dataVisualizer = new UC_DataVisualizer(AbstractDataVisualizerViewModel.CONTINOUS);
			GraphicActions.getMyGraphicActions().addLast(dataVisualizer);
			addToAnchorablePane(dataVisualizer);
		}

		private void mnitAddDiscreteChart_Click(object sender, RoutedEventArgs e)
		{
			
			UC_DataVisualizer dataVisualizer = new UC_DataVisualizer(AbstractDataVisualizerViewModel.DISCRETE);
			GraphicActions.getMyGraphicActions().addLast(dataVisualizer);
			addToAnchorablePane(dataVisualizer);
		}

		/// <summary>
		/// Loads a valid XML File an creates the charts
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void mnitLoadXML_Click(object sender, RoutedEventArgs e)
		{
			mainPanelChartContainer.Children.Clear();
			string pathToXML = GraphicActions.openXML();
			try
			{
				LinkedList<AbstractDataVisualizerViewModel> viewModels = XMLLoader.LoadXMLData(pathToXML);
				
				foreach (AbstractDataVisualizerViewModel viewModel in viewModels)
				{
					//comprobamos que no sea de la clase abstracta
					if (viewModel is ContinousDataVisualizerViewModel ||
						viewModel is DiscreteDataVisualizerViewModel)
					{
						UC_DataVisualizer dataVisualizer = new UC_DataVisualizer(viewModel);
						GraphicActions.getMyGraphicActions().addLast(dataVisualizer);
						addToAnchorablePane(dataVisualizer);
					}
				}
			}
			catch (FileFormatException ex)
			{
				Console.WriteLine(ex.StackTrace);
				MessageBoxResult msg = MessageBox.Show("Error validando el XML");
			}

			List<string> observations = XMLLoader.getObservations(pathToXML);

			foreach (string observation in observations)
			{
				List<string> properties = XMLLoader.getPropertiesOf(observation, pathToXML);

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

		private void observationsAndProperties_MouseDoubleClick(object sender, MouseButtonEventArgs e)
		{

			TreeViewItem item = (TreeViewItem) observationsAndProperties.SelectedItem;
			TreeViewItem parent = null;
			if (item.Parent != null)
			{
				parent = (TreeViewItem) item.Parent;
			}

			MessageBoxResult test = MessageBox.Show("Has pinchado en: " + item.Header+ " y el padre es: " + parent.Header);
		}
	}
}
