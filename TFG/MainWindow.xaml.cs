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

namespace TFG
{
    /// <summary>
    /// Lógica de interacción para MainWindow2.xaml
    /// </summary>
    public partial class MainWindow2 : Window
    {
        public MainWindow2()
        {
            InitializeComponent();
        }

      
        // Play the media. 
        void OnMouseDownPlayMedia(object sender, RoutedEventArgs args)
        {

            // The Play method will begin the media if it is not currently active or  
            // resume media if it is paused. This has no effect if the media is 
            // already running.
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

    }
}
