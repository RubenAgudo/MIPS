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

        private int containerNumber;
        private LinkedList<UC_VideoContainer> videoContainers;
        private GraphicActions graphicActions;

        public MainWindow()
        {
            InitializeComponent();
            
            this.DataContext = this;
            containerNumber = 0;
            videoContainers = new LinkedList<UC_VideoContainer>() ;
            graphicActions = new GraphicActions();
        }


        private void mnitAddVideoContainer_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                UC_VideoContainer videoContainer = new UC_VideoContainer();
                addToAnchorablePane(videoContainer);

            }
            catch(NotImplementedException ex)
            {
                System.Console.WriteLine(ex.StackTrace);
                
            }
                

        }

        private void mnitAddGraphicContainer_Click(object sender, RoutedEventArgs e)
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
                doc.Title = "Container " + ++containerNumber;
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
            foreach (UC_VideoContainer vc in videoContainers)
            {
                vc.sync(new TimeSpan());
            }
            
            graphicActions.sync();
        }
    }
}
