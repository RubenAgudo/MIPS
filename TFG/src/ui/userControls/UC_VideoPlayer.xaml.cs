using System;
using System.Drawing;
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
using TFG.src.interfaces;
using System.Windows.Threading;
using AForge.Video;
using AForge.Video.FFMPEG;
using AForge;
using System.IO;
using System.Drawing.Imaging;

namespace TFG.src.ui.userControls
{
    /// <summary>
    /// Lógica de interacción para UC_VideoPlayer.xaml
    /// </summary>
    public partial class UC_VideoPlayer : UserControl, ISynchronizable
    {

        private VideoFileReader reader;
        private DispatcherTimer timer;
        
        private int seconds;

        public UC_VideoPlayer()
        {
            InitializeComponent();
            seconds = 0;
            reader = new VideoFileReader();
            
        }

        #region PlayBackControlEvents
        // Play the media. 
        private void OnMouseDownPlayMedia(object sender, RoutedEventArgs args)
        {

            // The Play method will begin the media if it is not currently active or  
            // resume media if it is paused. This has no effect if the media is 
            // already running.
            if (!reader.IsOpen)
            {
                abrirVideo(sender, args);
                activarTemporizador();
            }
            else
            {
                timer.Start();
            }
            //myMediaElement.Volume = 0;
            //myMediaElement.Play();
            
           

        }

        // Pause the media. 
        private void OnMouseDownPauseMedia(object sender, RoutedEventArgs args)
        {

            // The Pause method pauses the media if it is currently running. 
            // The Play method can be used to resume.
            //myMediaElement.Pause();
            timer.Stop();

        }

        // Stop the media. 
        private void OnMouseDownStopMedia(object sender, RoutedEventArgs args)
        {

            // The Stop method stops and resets the media to be played from 
            // the beginning.
           // myMediaElement.Stop();
            timer.Stop();

        }

        // When the media playback is finished. Stop() the media to seek to media start. 
        private void Element_MediaEnded(object sender, EventArgs e)
        {
            timer.Stop();
        }
        #endregion

        #region PlayBackControl
        public void pause()
        {
            timer.Stop();
            //myMediaElement.Pause();
        }

        public void play()
        {
            timer.Start();
            //myMediaElement.Play();
        }
        #endregion

        private void abrirVideo(object sender, RoutedEventArgs e)
        {
            try
            {
                string path = VideoActions.openFile();
                Uri uriPath = new Uri(path);
                //myMediaElement.Source = uriPath;
                Console.WriteLine(path);
                reader.Open(path);

            }
            catch (ArgumentNullException exc)
            {
                Console.WriteLine(exc.StackTrace);

            }
            catch (UriFormatException exc1)
            {
                Console.WriteLine(exc1.StackTrace);
            }
            catch (VideoException exc2)
            {
                Console.WriteLine(exc2.StackTrace);
            }

        }

        private void activarTemporizador()
        {
            if (timer == null)
            {
                timer = new DispatcherTimer();
                timer.Interval = new TimeSpan(1000);
                timer.Tick += timer_Tick;
                timer.Start();
            }

        }

        private void timer_Tick(object sender, EventArgs e)
        {
            MemoryStream memory = new MemoryStream();
            Bitmap bitmap = reader.ReadVideoFrame();
            bitmap.Save(memory, ImageFormat.Png);
            memory.Position = 0;
            BitmapImage bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.StreamSource = memory;
            bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
            bitmapImage.EndInit();
            image1.Source = bitmapImage;
        }

        public void sync(TimeSpan position)
        {
            //myMediaElement.Position = position;
            
        }


        public TimeSpan position()
        {
            return new TimeSpan();
        }

        private void AdvanceFrame_Click(object sender, RoutedEventArgs e)
        {
            //myMediaElement.Pause();
            //TimeSpan ts = myMediaElement.Position;
            //ts = ts.Add(TimeSpan.FromSeconds(10));
            //myMediaElement.Position = ts;
            //myMediaElement.Play();
            //myMediaElement.Pause();
            
        }

        

    }
}
