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
using TFG.src.ui.userControls;
using System.ComponentModel;
using Xceed.Wpf.AvalonDock.Themes;

namespace TFG
{
    /// <summary>
    /// Lógica de interacción para MainWindow2.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private int paneNumber;
        LinkedList<UC_VideoPlayer> videoPanes;

        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = this;
            paneNumber = 0;
            videoPanes = new LinkedList<UC_VideoPlayer>();
        }


        private void anadirVideo(object sender, RoutedEventArgs e)
        {
            try
            {
                UC_VideoPlayer video = new UC_VideoPlayer();
                videoPanes.AddLast(video);
                addToAnchorablePane(video);

            }
            catch(NotImplementedException ex)
            {
                System.Console.WriteLine(ex.StackTrace);
                
            }
                

        }

        private void mnitAnadirPanelGrafico_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                UC_DataVisualizer dataVisualizer = new UC_DataVisualizer();
                addToAnchorablePane(dataVisualizer);
            }
            catch(NotImplementedException ex)
            {
                System.Console.WriteLine(ex.StackTrace);
            }
        }

        

         /// <summary>
         /// This method add a UserControl to an AnchorablePane
         /// </summary>
         /// <param name="objectToAdd">The UserControl you want to add</param>
         /// <exception cref="NotImplementedException"></exception>
        private void addToAnchorablePane(UserControl objectToAdd)
        {
            LayoutAnchorablePane pane = dockingManager.Layout.Descendents().
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

        /// <summary>
        /// This method syncs all the video panes in fact reset all. All the videos must have the same lenght.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mnitSync_Click(object sender, RoutedEventArgs e)
        {
            bool first = true;
            UC_VideoPlayer baseVideo = null;
            //UC_VideoPlayer video;
            foreach (UC_VideoPlayer actualVideo in videoPanes)
            {
               
                if(first)
                {
                    baseVideo = actualVideo;
                    first = false;
                } 
                
                actualVideo.pause();
                actualVideo.sync(baseVideo.position());
                //baseVideo = actualVideo;
                 
            }

            foreach (UC_VideoPlayer actualVideo in videoPanes)
            {
                actualVideo.play();
            }
        }
    }
}
