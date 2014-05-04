﻿using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;
using Xceed.Wpf.AvalonDock;
using Xceed.Wpf.AvalonDock.Layout;
using TFG.src.classes;
using TFG.src.ui.userControls;
using System.ComponentModel;
using Xceed.Wpf.AvalonDock.Themes;
using System.Windows.Threading;
using System.IO;
using TFG.src.ViewModels;

namespace TFG
{
    /// <summary>
    /// Lógica de interacción para MainWindow2.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private HashSet<string> loaded;
		private XMLLoader xmlLoader;
		

        public MainWindow()
        {
            InitializeComponent();
            
            this.DataContext = this;
			loaded = new HashSet<string>();
			
        }

		
        

          /// <summary>
        /// Method that adds to the GraphicsContainer a LayoutAnchorable that contains a Data visualizer user control
        /// </summary>
        /// <param name="objectToAdd"></param>
        private void addToAnchorablePane(UC_ObservationContainer objectToAdd)
        {

            if (mainPanel != null)
            {
				LayoutAnchorable doc = new LayoutAnchorable();
				doc.Closing += doc_Closing;
				doc.CanHide = false;
				doc.CanClose = true;
				doc.Title = objectToAdd.Observation;
				doc.Content = objectToAdd;
                mainPanel.Children.Add(doc);

            }
            else
            {
                throw new NotImplementedException();
            }
        }

		private void doc_Closing(object sender, CancelEventArgs e)
		{
 			LayoutAnchorable doc = (LayoutAnchorable)sender;
			UC_ObservationContainer content = (UC_ObservationContainer)doc.Content;
			GraphicActions.getMyGraphicActions().remove(content);
			loaded.Remove(content.Observation);
		}

        private void mnitExit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

		private void mnitLoadXML_Click(object sender, RoutedEventArgs e)
		{
			mainPanel.Children.Clear();
			string pathToXML = XMLLoader.openXML();
			try
			{
				xmlLoader = new XMLLoader(pathToXML);
				List<string> observations = xmlLoader.getObservations();
                loaded = new HashSet<string>();
				GraphicActions.getMyGraphicActions().clear();
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

		/// <summary>
		/// Añade las propiedades seleccionadas al ObservationContainer que corresponda
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void observationsAndProperties_MouseDoubleClick(object sender, MouseButtonEventArgs e)
		{
			LinkedList<AbstractDataVisualizerViewModel> viewModels;
			TreeViewItem anItem = (TreeViewItem) observationsAndProperties.SelectedItem;
			
			string itemHeader = anItem.Header.ToString(), 
				parentHeader;
			bool createNewObservationContainer;

			if (!anItem.HasItems)
			{
				parentHeader = ((TreeViewItem)anItem.Parent).Header.ToString();
				createNewObservationContainer = !GraphicActions.getMyGraphicActions().exists(parentHeader);
				viewModels = xmlLoader.LoadXMLData(itemHeader, parentHeader);
			}
			else
			{
				createNewObservationContainer = !GraphicActions.getMyGraphicActions().exists(itemHeader);
				viewModels = xmlLoader.LoadXMLData(itemHeader);
			}

			foreach (AbstractDataVisualizerViewModel viewModel in viewModels)
			{
				UC_ObservationContainer newContainer = null;
				string observacion = viewModel.Observation;
				
				if (createNewObservationContainer)
				{
					createNewObservationContainer = false;
					newContainer = new UC_ObservationContainer(observacion);
					GraphicActions.getMyGraphicActions().addObservationContainer(newContainer);
					addToAnchorablePane(newContainer);
				}

				//comprobamos que no sea de la clase abstracta
				if ((viewModel is ContinousDataVisualizerViewModel ||
					viewModel is DiscreteDataVisualizerViewModel) && !loaded.Contains(viewModel.Title))
				{
					UC_DataVisualizer dataVisualizer = new UC_DataVisualizer(viewModel);
					GraphicActions.getMyGraphicActions().addLast(dataVisualizer);
					GraphicActions.getMyGraphicActions().addToContainer(observacion, dataVisualizer);
				}
			}
		}
    }
}
