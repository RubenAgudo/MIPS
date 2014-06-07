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
		private XMLLoader xmlLoader;
		private UC_VideoContainer videoContainer;
		private DispatcherTimer timer;

        public MainWindow()
        {
            this.DataContext = this;
            InitializeComponent();
           

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
			if (doc.Content is UC_ChartContainer)
			{
				GraphicActions.getMyGraphicActions().remove((UC_ChartContainer)doc.Content);
			}
			else
			{
				VideoActions.getMyVideoActions().clear();
				videoContainer = null;
			}
			
		}

        private void mnitExit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

		private void mnitLoadXML_Click(object sender, RoutedEventArgs e)
		{
			ClearData();
			observationsAndProperties.Items.Clear();
			try
			{
				string pathToXML = XMLLoader.openXML();
				xmlLoader = new XMLLoader(pathToXML);
				List<string> observations = xmlLoader.getObservations();

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

		private void ClearData()
		{
			mainPanel.Children.Clear();
			GraphicActions.getMyGraphicActions().clear();
			XMLExport.getMyXMLExport().clear();
			videoContainer = null;
		}

		/// <summary>
		/// Añade las propiedades seleccionadas al ChartContainer que corresponda
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void observationsAndProperties_MouseDoubleClick(object sender, MouseButtonEventArgs e)
		{
			LinkedList<AbstractDataVisualizerViewModel> viewModels;
			TreeViewItem anItem = (TreeViewItem)observationsAndProperties.SelectedItem;

			if (anItem == null) { return; }

			bool createNewChartContainer = LoadDataFromXML(anItem, out viewModels);

			foreach (AbstractDataVisualizerViewModel viewModel in viewModels)
			{
				string observacion = viewModel.Observation;

				if (createNewChartContainer)
				{
					createNewChartContainer = false;
					UC_ChartContainer newContainer = new UC_ChartContainer(observacion);
					GraphicActions.getMyGraphicActions().addObservationContainer(newContainer);
					addToAnchorablePane(newContainer, newContainer.Observation);
				}

				//comprobamos que no sea de la clase abstracta
				if (((viewModel is ContinousDataVisualizerViewModel) ||
					(viewModel is DiscreteDataVisualizerViewModel)))
				{
					double[] startEnd = GraphicActions.getMyGraphicActions().getSelectedRange();
					UC_DataVisualizer dataVisualizer = new UC_DataVisualizer(viewModel, startEnd[0], startEnd[1]);
					GraphicActions.getMyGraphicActions().addToContainer(observacion, dataVisualizer);
				}
			}
		}


		/// <summary>
		/// This method loads the data from the xml, and returns if a new container must be created.
		/// It also returns in an out variable the viewModels to add
		/// </summary>
		/// <param name="anItem">the TreeViewItem that has been selected</param>
		/// <param name="viewModels"> The viewModels that are going to be created</param>
		/// <returns></returns>
		private bool LoadDataFromXML(TreeViewItem anItem, out LinkedList<AbstractDataVisualizerViewModel> viewModels)
		{
			string parentHeader;
			string itemHeader = anItem.Header.ToString();
			bool result;
			if (!anItem.HasItems)
			{
				parentHeader = ((TreeViewItem)anItem.Parent).Header.ToString();
				result = !GraphicActions.getMyGraphicActions().exists(parentHeader);
				viewModels = xmlLoader.LoadXMLData(itemHeader, parentHeader);
			}
			else
			{
				result = !GraphicActions.getMyGraphicActions().exists(itemHeader);
				viewModels = xmlLoader.LoadXMLData(itemHeader);
			}
			return result;
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

		private void saveInterval()
		{
			mnitSave.IsEnabled = false;
			BackgroundWorker worker = new BackgroundWorker();
			worker.DoWork += worker_DoWorkSaveInterval;
			worker.RunWorkerCompleted += worker_RunWorkerCompletedSaveInterval;
			worker.RunWorkerAsync();
			
		}

		void worker_RunWorkerCompletedSaveInterval(object sender, RunWorkerCompletedEventArgs e)
		{
			mnitSave.IsEnabled = true;
		}

		private void worker_DoWorkSaveInterval(object sender, DoWorkEventArgs e)
		{
			double[] selectedRange = GraphicActions.getMyGraphicActions().getSelectedRange();
			if (selectedRange[0] == selectedRange[1])
			{
				MessageBoxResult msg = MessageBox.Show("You must select a range", "No range selected",
					MessageBoxButton.OK, MessageBoxImage.Warning);
			}
			else
			{
				if (!XMLExport.getMyXMLExport().createInterval(selectedRange[0], selectedRange[1]))
				{
					MessageBoxResult msg = MessageBox.Show("Selected interval overlaps! Aborting creation of new interval",
						"Interval NOT saved", MessageBoxButton.OK, MessageBoxImage.Warning);
				}
				else
				{
					MessageBoxResult msg = MessageBox.Show("Interval saved succesfully!",
						"Interval saved", MessageBoxButton.OK, MessageBoxImage.Information);
				}
			}
		}

		private void loadVideos()
		{
			try
			{
				string[] paths = VideoActions.openFile();

				foreach (string path in paths)
				{
					UC_VideoPlayer video = new UC_VideoPlayer(path);
					if(VideoActions.getMyVideoActions().addVideo(video)) {
						videoContainer.addToAnchorablePane(video, video.VideoName);
					}
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

		private void mnitSaveStep_Click(object sender, RoutedEventArgs e)
		{
			BackgroundWorker worker = new BackgroundWorker();
			worker.DoWork += new DoWorkEventHandler(
				delegate(object send, DoWorkEventArgs eventArgs)
				{
					bool result = XMLExport.getMyXMLExport().saveData("step");
					eventArgs.Result = result;
				}
			);

			worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(
				delegate(object send, RunWorkerCompletedEventArgs eventArgs)
				{
					MessageBoxResult result = MessageBox.Show(((bool)eventArgs.Result) ? "validated" : "not validated");
				}
			);

			worker.RunWorkerAsync();
		}

		private void mnitSaveSituation_Click(object sender, RoutedEventArgs e)
		{
			BackgroundWorker worker = new BackgroundWorker();
			worker.DoWork += new DoWorkEventHandler(
				delegate(object send, DoWorkEventArgs eventArgs)
				{
					bool result = XMLExport.getMyXMLExport().saveData("situation");
					eventArgs.Result = result;
				}
			);

			worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(
				delegate(object send, RunWorkerCompletedEventArgs eventArgs)
				{
					MessageBoxResult result = MessageBox.Show(((bool)eventArgs.Result) ? "validated" : "not validated");
				}
			);

			worker.RunWorkerAsync();
		}

		private void mnitCreateInterval_Click(object sender, RoutedEventArgs e)
		{
			saveInterval();
		}

    }
}
