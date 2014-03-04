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
using TFG.src.interfaces;
using System.Windows.Threading;


namespace TFG.src.ui.userControls
{
    /// <summary>
    /// Lógica de interacción para UC_VideoPlayer.xaml
    /// </summary>
    public partial class UC_VideoPlayer : UserControl, ISynchronizable
    {

        private bool isDragging;
        private DispatcherTimer timer;
        private Uri uriPath;

        public UC_VideoPlayer(string path)
        {
            InitializeComponent();
            this.uriPath = new Uri(path);
            myMediaElement.Source = uriPath;
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(1000);
            timer.Tick += new EventHandler(timer_Tick);
            isDragging = false;
        }
        

        #region PlayBackControl
        internal void pause()
        {
            myMediaElement.Pause();
            timer.Stop();
        }

        internal void play()
        {
            myMediaElement.Play();
            timer.Start();
            
        }
        internal void stop()
        {
            myMediaElement.Stop();
            timer.Stop();
        }
        #endregion

        /// <summary>
        /// Set the video to the specified position. It's used for syncing all the videos.
        /// </summary>
        /// <param name="position"></param>
        public void sync(TimeSpan position)
        {            
            myMediaElement.Position = position;
            
        }


        /// <summary>
        /// Gets or sets position.
        /// Set: Sets the Position of the video to the given TimeSpan. 
        /// Get: Gets the TimeSpan of the video representing it's position
        /// </summary>
        public TimeSpan Position
        {

            get
            {
                return myMediaElement.Position;
            }
            set
            {
                myMediaElement.Position = value;
            }
        }

        private void myMediaElement_MediaEnded(object sender, RoutedEventArgs e)
        {
            stop();
        }

        private void myMediaElement_MediaOpened(object sender, RoutedEventArgs e)
        {
            timelineSlider.Maximum = myMediaElement.NaturalDuration.TimeSpan.TotalSeconds;


            //Binding myBinding = new Binding("Position");
            //myBinding.Source = myMediaElement;
            //timelineSlider.SetBinding(Slider.ToolTipProperty, myBinding);
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
            play();
        }

        // Pause the media. 
        private void OnMouseDownPauseMedia(object sender, RoutedEventArgs args)
        {
            pause();
        }

        // Stop the media. 
        private void OnMouseDownStopMedia(object sender, RoutedEventArgs args)
        {
            stop();
        }

        private void AdvanceFrame_Click(object sender, RoutedEventArgs e)
        {
            advanceFrame();
        }

        internal void advanceFrame()
        {
            pause();
            TimeSpan ts = Position;
            ts = ts.Add(TimeSpan.FromSeconds(Properties.Settings.Default.SecondsToAdvance));
            Position = ts;
        }

        #endregion

        private void timelineSlider_DragStarted(object sender, System.Windows.Controls.Primitives.DragStartedEventArgs e)
        {
            isDragging = true;
        }

        private void timelineSlider_DragCompleted(object sender, System.Windows.Controls.Primitives.DragCompletedEventArgs e)
        {
            isDragging = false;
            Position = TimeSpan.FromSeconds(timelineSlider.Value);
        }

        private void timer_Tick(object sender, EventArgs e)
        {

            if (!isDragging)
            {
                timelineSlider.Value = Position.TotalSeconds;
                
            }
            timelineSlider.SelectionEnd = Position.TotalSeconds;
        }

    }
}
