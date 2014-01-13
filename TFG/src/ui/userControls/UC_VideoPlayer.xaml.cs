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
using TFG.src.classes;
using System.Windows.Threading;

namespace TFG.src.ui.userControls
{
    /// <summary>
    /// Lógica de interacción para UC_VideoPlayer.xaml
    /// </summary>
    public partial class UC_VideoPlayer : UserControl
    {
        private DispatcherTimer timer;
        private int seconds;
        public UC_VideoPlayer()
        {
            InitializeComponent();
            seconds = 0;
        }

        #region PlayBackControl
        // Play the media. 
        void OnMouseDownPlayMedia(object sender, RoutedEventArgs args)
        {

            // The Play method will begin the media if it is not currently active or  
            // resume media if it is paused. This has no effect if the media is 
            // already running.
            if (!myMediaElement.HasVideo)
            {
                abrirVideo(sender, args);
            }
            myMediaElement.Volume = 0;
            myMediaElement.Play();

        }

        // Pause the media. 
        void OnMouseDownPauseMedia(object sender, RoutedEventArgs args)
        {

            // The Pause method pauses the media if it is currently running. 
            // The Play method can be used to resume.
            myMediaElement.Pause();

        }

        // Stop the media. 
        void OnMouseDownStopMedia(object sender, RoutedEventArgs args)
        {

            // The Stop method stops and resets the media to be played from 
            // the beginning.
            myMediaElement.Stop();

        }

        // When the media playback is finished. Stop() the media to seek to media start. 
        private void Element_MediaEnded(object sender, EventArgs e)
        {
            myMediaElement.Stop();
        }
        #endregion

        private void abrirVideo(object sender, RoutedEventArgs e)
        {
            try
            {
                Uri path = new Uri(VideoActions.openFile());
                myMediaElement.Source = path;

            }
            catch (ArgumentNullException exc)
            {
                Console.WriteLine(exc.StackTrace);

            }
            catch (UriFormatException exc1)
            {
                Console.WriteLine(exc1.StackTrace);
            }

        }

        private void SecondPerSecond_Click(object sender, RoutedEventArgs e)
        {
            if (timer == null)
            {
                timer = new DispatcherTimer();
                timer.Interval = new TimeSpan(1000);
                timer.Tick += timer_Tick;
            }

            if (SecondPerSecond.IsChecked == true)
            {
                timer.Start();
            }
            else
            {
                timer.Stop();
            }
        }

        void timer_Tick(object sender, EventArgs e)
        {
            seconds += 1;
            myMediaElement.Position = new TimeSpan(0,0,seconds);
        }

    }
}
