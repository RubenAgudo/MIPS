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
        private VideoActions videoActions;
        private GraphicActions graphicActions;

        public MainWindow()
        {
            InitializeComponent();
            
            this.DataContext = this;
            paneNumber = 0;
            videoActions = new VideoActions();
            graphicActions = new GraphicActions();
        }


        private void anadirVideo(object sender, RoutedEventArgs e)
        {
            try
            {
                UC_VideoPlayer video = new UC_VideoPlayer();
                videoActions.addVideo(video);
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
                graphicActions.addLast(dataVisualizer);
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
            videoActions.sync();
            graphicActions.sync();
        }
    }
}
