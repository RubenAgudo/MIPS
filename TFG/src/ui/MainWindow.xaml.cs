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
using System.Windows.Shapes;
using Xceed.Wpf.AvalonDock;
using Xceed.Wpf.AvalonDock.Layout;
using TFG.src.classes;
using TFG.src.ui.userControls;
using System.ComponentModel;
using Xceed.Wpf.AvalonDock.Themes;
using System.Windows.Threading;

namespace TFG
{
    /// <summary>
    /// Lógica de interacción para MainWindow2.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private UC_VideoContainer videoContainer;
        private UC_ChartContainer chartContainer;
		private DispatcherTimer timer;

        public MainWindow()
        {
            InitializeComponent();
            
            this.DataContext = this;
            videoContainer = null;
            chartContainer = null;
			timer = new DispatcherTimer();
			timer.Interval = new TimeSpan(0,0,0,0,1000);
			timer.Tick += timer_Tick;
			timer.Start();
			XMLLoader.getXMLLoader();
        }

		private void timer_Tick(object sender, EventArgs e)
		{
			if (chartContainer != null && videoContainer != null)
			{

				chartContainer.Update(videoContainer.Progress);
			}
		}


        private void mnitAddVideoContainer_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (videoContainer == null)
                {
                    videoContainer = new UC_VideoContainer();
                    addToAnchorablePane(videoContainer);
                }

            }
            catch (NotImplementedException ex)
            {
                System.Console.WriteLine(ex.StackTrace);

            }


        }

        private void mnitAddChartContainer_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (chartContainer == null)
                {
                    chartContainer = new UC_ChartContainer();
                    addToAnchorablePane(chartContainer);
                }
            }
            catch (NotImplementedException ex)
            {
                System.Console.WriteLine(ex.StackTrace);
            }
        }

        

         /// <summary>
         /// This method add a UserControl to an AnchorablePane
         /// </summary>
         /// <param name="objectToAdd">The UserControl you want to add</param>
         /// <exception cref="NotImplementedException"></exception>
        private void addToAnchorablePane(UserControl objectToAdd)
        {
            if (mainPanel != null)
            {
                LayoutDocument doc = new LayoutDocument();
				if (objectToAdd is UC_ChartContainer)
				{
					doc.Title = "Chart container";
				}
				else if (objectToAdd is UC_VideoContainer)
				{
					doc.Title = "Video container";
				}
                
                
                doc.CanClose = false;
                doc.Content = objectToAdd;
				
				mainPanel.Children.Add(doc);

            }
            else
            {
                throw new NotImplementedException();
            }
        }

        private void mnitExit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
