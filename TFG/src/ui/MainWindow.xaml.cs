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

namespace TFG
{
    /// <summary>
    /// Lógica de interacción para MainWindow2.xaml
    /// </summary>
    public partial class MainWindow2 : Window
    {
        private int docNumber;
        public MainWindow2()
        {
            docNumber = 0;
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

        private void anadirDocumento(object sender, RoutedEventArgs e)
        {
            var documentPane = dockingManager.Layout.Descendents().OfType<LayoutDocumentPane>().FirstOrDefault();
            
            if (documentPane != null)
            {
                docNumber++;
                LayoutDocument layoutDocument = new LayoutDocument { Title = "New Document " + docNumber };

                //*********Here you could add whatever you want***********
                layoutDocument.Content = new StackPanel();

                //Add the new LayoutDocument to the existing array
                documentPane.Children.Add(layoutDocument);
            }
        }

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
           
            
        }

    }
}
