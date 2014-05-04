using System;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Xceed.Wpf.AvalonDock;
using Xceed.Wpf.AvalonDock.Layout;
using TFG.src.classes;
using TFG.src.ui.userControls;
using Xceed.Wpf.AvalonDock.Themes;
using System.Windows.Threading;
using TFG.src.interfaces;

namespace TFG.src.ui.userControls
{
	/// <summary>
	/// Lógica de interacción para UC_ObservationContainer.xaml
	/// </summary>
	public partial class UC_ObservationContainer : UserControl, IContainer
	{
		private UC_VideoContainer videoContainer;
		private UC_ChartContainer chartContainer;
		private DispatcherTimer timer;

		public string Observation { get; private set; }

		public UC_ObservationContainer(string observation)
		{
			InitializeComponent();
			Observation = observation;
			timer = new DispatcherTimer();
			timer.Interval = new TimeSpan(0, 0, 0, 0, 1000);
			timer.Tick += timer_Tick;
			timer.Start();
			
		}

		private void timer_Tick(object sender, EventArgs e)
		{
			if (chartContainer != null && videoContainer != null)
			{
				chartContainer.Update(videoContainer.Progress);
			}
		}

		private void mnitSave_Click(object sender, RoutedEventArgs e)
		{

		}

		private void mnitAddVideo_Click(object sender, RoutedEventArgs e)
		{
			if (videoContainer == null)
			{
				videoContainer = new UC_VideoContainer();
				addToAnchorablePane(videoContainer, "Videos");
			} 
		}

		public void addToAnchorablePane(UserControl objectToAdd, string Title)
		{
			if (mainObservationContainer != null)
			{
				
				LayoutAnchorable doc = new LayoutAnchorable();
				doc.CanHide = false;
				doc.CanClose = true;
				doc.Title = Title;
				doc.Content = objectToAdd;

				if(objectToAdd is UC_DataVisualizer) {
					if(chartContainer == null) {
						chartContainer = new UC_ChartContainer();
						addToAnchorablePane(chartContainer, "Gráficos");
					}

					chartContainer.addToAnchorablePane((UC_DataVisualizer) objectToAdd, Title);
					return;
				}
				else if(objectToAdd is UC_VideoPlayer)
				{
					videoContainer.addToAnchorablePane(objectToAdd, null);
					return;
				}
				
				mainObservationContainer.Children.Add(doc);

			}
			else
			{
				throw new NotImplementedException();
			}
		}
	}
}
