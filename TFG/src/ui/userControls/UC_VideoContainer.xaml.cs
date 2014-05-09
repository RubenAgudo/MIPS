﻿using System;
using System.Windows;
using System.Windows.Controls;
using TFG.src.classes;
using TFG.src.interfaces;
using Xceed.Wpf.AvalonDock.Layout;

namespace TFG.src.ui.userControls
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
