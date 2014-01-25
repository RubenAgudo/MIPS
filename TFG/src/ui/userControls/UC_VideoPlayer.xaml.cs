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

        //private VideoFileReader reader;
        private DispatcherTimer timer;
        private bool videoOpened;
        private Uri uriPath;

        public UC_VideoPlayer()
        {
            videoOpened = false;
            InitializeComponent();
        }

        public UC_VideoPlayer(string path)
        {
            InitializeComponent();
            this.uriPath = new Uri(path);
            myMediaElement.Source = uriPath;
            
        }
        

        #region PlayBackControl
        internal void pause()
        {
            myMediaElement.Pause();
        }

        internal void play()
        {
            myMediaElement.Play();
        }
        internal void stop()
        {
            myMediaElement.Stop();
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

    }
}
