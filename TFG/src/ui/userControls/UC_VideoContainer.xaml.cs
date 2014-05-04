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
using TFG.src.interfaces;
using TFG.src.exceptions;

namespace TFG.src.ui.userControls
{
    /// <summary>
    /// Lógica de interacción para UC_VideoContainer.xaml
    /// </summary>
    public partial class UC_VideoContainer : UserControl, IContainer
    {
        private int paneNumber;

		public double Progress
		{
			get
			{
				return VideoActions.getMyVideoActions().getLongestVideoProgress();
			}
		}

        public UC_VideoContainer()
        {
            InitializeComponent();

            paneNumber = 0;
        }

        private void mnitAddVideoPane_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string[] paths = VideoActions.openFile();

                foreach (string path in paths)
                {
                    UC_VideoPlayer video = new UC_VideoPlayer(path);
                    VideoActions.getMyVideoActions().addVideo(video);
                    addToAnchorablePane(video, "Video " + ++paneNumber);
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



        #region PlayBackControlEvents
        /// <summary>
        /// Plays the media if no video is opened. Resumes the media if video is opened
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void OnMouseDownPlayMedia(object sender, RoutedEventArgs args)
        {

            //if (!videoOpened)
            //{
            //    abrirVideo(sender, args);
            //    videoOpened = true;
            //    setStatusBarInfo();
            //}
            //play();
			VideoActions.getMyVideoActions().play();
        }

        // Pause the media. 
        private void OnMouseDownPauseMedia(object sender, RoutedEventArgs args)
        {
			VideoActions.getMyVideoActions().pause();
        }

        // Stop the media. 
        private void OnMouseDownStopMedia(object sender, RoutedEventArgs args)
        {
			VideoActions.getMyVideoActions().stop();
        }

        private void AdvanceFrame_Click(object sender, RoutedEventArgs e)
        {
			VideoActions.getMyVideoActions().advanceFrame();
        }

        #endregion


		public void addToAnchorablePane(UserControl objectToAdd, string Title)
		{
			if (mainPanelVideoContainer != null)
			{
				LayoutAnchorable doc = new LayoutAnchorable();
				doc.CanHide = false;
				doc.CanClose = true;
				doc.Title = Title;
				doc.Content = objectToAdd;
				mainPanelVideoContainer.Children.Add(doc);

			}
			else
			{
				throw new NotImplementedException();
			}
		}
	}
}
