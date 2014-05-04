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

namespace TFG.src.ui.userControls
{
	/// <summary>
	/// Lógica de interacción para UC_ObservationContainer.xaml
	/// </summary>
	public partial class UC_ObservationContainer : UserControl
	{
		private UC_VideoContainer videoContainer;
		private UC_ChartContainer chartContainer;

		public string Observation { get; private set; }

		public UC_ObservationContainer(string observation)
		{
			InitializeComponent();
			Observation = observation;
			//videoContainer = new UC_VideoContainer();
			//chartContainer = new UC_ChartContainer();
			
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

		internal void addToAnchorablePane(UserControl objectToAdd, string title)
		{
			if (mainObservationContainer != null)
			{
				
				LayoutAnchorable doc = new LayoutAnchorable();
				doc.CanHide = false;
				doc.CanClose = true;
				doc.Title = title;
				doc.Content = objectToAdd;

				if(objectToAdd is UC_DataVisualizer) {
					if(chartContainer == null) {
						chartContainer = new UC_ChartContainer();
						addToAnchorablePane(chartContainer, "Gráficos");
					}

					chartContainer.addToAnchorablePane((UC_DataVisualizer) objectToAdd, title);
					return;
				}
				else if(objectToAdd is UC_VideoPlayer)
				{
					videoContainer.addToAnchorablePane(objectToAdd);
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
