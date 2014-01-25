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

namespace TFG.src.ui.userControls
{
    /// <summary>
    /// Lógica de interacción para UC_VideoContainer.xaml
    /// </summary>
    public partial class UC_VideoContainer : UserControl
    {
        private int paneNumber;
        private VideoActions videoActions;

        public UC_VideoContainer()
        {
            paneNumber = 0;
            videoActions = new VideoActions();
            
            UC_VideoPlayer video = new UC_VideoPlayer();
            videoActions.addVideo(video);


            InitializeComponent();

            

        }

        private void mnitAddVideoPane_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string path = VideoActions.openFile();
                UC_VideoPlayer video = new UC_VideoPlayer(path);
                videoActions.addVideo(video);
                addToAnchorablePane(video);

            }
            catch (NotImplementedException ex)
            {
                System.Console.WriteLine(ex.StackTrace);

            }

        }

        private void addToAnchorablePane(UserControl objectToAdd)
        {
            LayoutAnchorablePane pane = videoContainer.Layout.Descendents().
                 OfType<LayoutAnchorablePane>().FirstOrDefault();

            if (pane != null)
            {
                LayoutAnchorable doc = new LayoutAnchorable();
                doc.Title = "Pane " + ++paneNumber;
                doc.Content = objectToAdd;
                pane.Children.Add(doc);

            }
            else
            {
                throw new NotImplementedException();
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
            videoActions.play();
        }

        // Pause the media. 
        private void OnMouseDownPauseMedia(object sender, RoutedEventArgs args)
        {
            videoActions.pause();
        }

        // Stop the media. 
        private void OnMouseDownStopMedia(object sender, RoutedEventArgs args)
        {
            videoActions.stop();
        }

        #endregion

        private void setStatusBarInfo()
        {
            //txblDuration.Text = myMediaElement.NaturalDuration.ToString();
            //txblFramesSkipped.Text = AdvancedSettings.Default.SecondsToAdvance.ToString();
            //txblSpeed.Text = myMediaElement.SpeedRatio.ToString();

        }

        private void AdvanceFrame_Click(object sender, RoutedEventArgs e)
        {
            videoActions.advanceFrame();
        }

    }
}
