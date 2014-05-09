using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Threading;
using TFG.src.classes;
using TFG.src.exceptions;
using TFG.src.ui.userControls;
using TFG.src.ViewModels;
using Xceed.Wpf.AvalonDock.Layout;

namespace TFG
{
    /// <summary>
    /// Lógica de interacción para MainWindow2.xaml
    /// </summary>
    public partial class MainWindow : Window, TFG.src.interfaces.IContainer
    {
        private HashSet<string> loaded;
		private XMLLoader xmlLoader;
		private UC_VideoContainer videoContainer;
		private DispatcherTimer timer;

        public MainWindow()
        {
            InitializeComponent();
            
            this.DataContext = this;
			loaded = new HashSet<string>();
			timer = new DispatcherTimer();
			timer.Interval = new TimeSpan(0, 0, 0, 0, 1000);
			timer.Tick += timer_Tick;
			timer.Start();
			
        }

        /// <summary>
        /// Method that adds to the GraphicsContainer a LayoutAnchorable that contains a Data visualizer user control
        /// </summary>
        /// <param name="objectToAdd"></param>
        public void addToAnchorablePane(UserControl objectToAdd, string Title)
        {
			
			if (mainPanel != null)
			{
				LayoutAnchorable doc = new LayoutAnchorable();
				doc.Hiding += doc_Hiding;
				doc.CanHide = true;
				doc.CanClose = true;
				doc.Title = Title;
				doc.Content = objectToAdd;
				mainPanel.Children.Add(doc);

			}
			else
			{
				throw new NotImplementedException();
			}
            
        }

		private void doc_Hiding(object sender, CancelEventArgs e)
		{
 			LayoutAnchorable doc = (LayoutAnchorable)sender;
			if (sender is UC_ChartContainer)
			{
				UC_ChartContainer content = (UC_ChartContainer)doc.Content;
				GraphicActions.getMyGraphicActions().remove(content);
				loaded.Remove(content.Observation);
			}
			else
			{
				VideoActions.getMyVideoActions().clear();
			}
			
		}

        private void mnitExit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

		private void mnitLoadXML_Click(object sender, RoutedEventArgs e)
		{
			mainPanel.Children.Clear();
			GraphicActions.getMyGraphicActions().clear();
			videoContainer = null;

			
			try
			{
				string pathToXML = XMLLoader.openXML();
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
				MessageBoxResult msg = MessageBox.Show("Error validating XML");
			}
			catch (FileNotSelectedException ex)
			{
				Console.WriteLine(ex.StackTrace);
				MessageBoxResult msg = MessageBox.Show("Select a XML file");
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
			
			if (anItem == null)
			{
				return;
			}

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
				UC_ChartContainer newContainer = null;
				string observacion = viewModel.Observation;
				
				if (createNewObservationContainer)
				{
					createNewObservationContainer = false;
					newContainer = new UC_ChartContainer(observacion);
					GraphicActions.getMyGraphicActions().addObservationContainer(newContainer);
					addToAnchorablePane(newContainer, newContainer.Observation);
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

		private void mnitAddVideo_Click(object sender, RoutedEventArgs e)
		{
			if (videoContainer == null)
			{
				videoContainer = new UC_VideoContainer();
				addToAnchorablePane(videoContainer, "Videos");
			}
			loadVideos();
		}

		private void timer_Tick(object sender, EventArgs e)
		{
			if (videoContainer != null)
			{
				GraphicActions.getMyGraphicActions().updateProgressBar(VideoActions.getMyVideoActions().getVideoProgress());
			}
		}

		private void mnitSaveRange_Click(object sender, RoutedEventArgs e)
		{
			XMLExport.getMyXMLExport().saveData();
		}

		private void loadVideos()
		{
			try
			{
				string[] paths = VideoActions.openFile();

				foreach (string path in paths)
				{
					UC_VideoPlayer video = new UC_VideoPlayer(path);
					VideoActions.getMyVideoActions().addVideo(video);
					videoContainer.addToAnchorablePane(video, video.VideoName);
				}

			}
			catch (NotImplementedException ex)
			{
				System.Console.WriteLine(ex.StackTrace);

			}
			catch (FileNotSelectedException ex2)
			{
				Console.WriteLine(ex2.StackTrace);
			}
		}

    }
}
