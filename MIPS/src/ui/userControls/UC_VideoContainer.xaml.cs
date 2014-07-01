using System;
using System.Windows;
using System.Windows.Controls;
using MIPS.src.classes;
using MIPS.src.interfaces;
using Xceed.Wpf.AvalonDock.Layout;

namespace MIPS.src.ui.userControls
{
    /// <summary>
    /// Lógica de interacción para UC_VideoContainer.xaml
    /// </summary>
    public partial class UC_VideoContainer : UserControl, IContainer
    {

        public UC_VideoContainer()
        {
            InitializeComponent();
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
				doc.Hiding += doc_Hiding;
				doc.CanHide = true;
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

		void doc_Hiding(object sender, System.ComponentModel.CancelEventArgs e)
		{
			LayoutAnchorable doc = (LayoutAnchorable)sender;
			UC_VideoPlayer thePlayer = (UC_VideoPlayer)doc.Content;
			bool result = VideoActions.getMyVideoActions().removeVideo(thePlayer);
			Console.WriteLine(result);
		}

	}
}
