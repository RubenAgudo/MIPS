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

        private int docNumber;

        public MainWindow()
        {
            docNumber = 0;
            InitializeComponent();
            this.DataContext = this;
        }


        private void anadirVideo(object sender, RoutedEventArgs e)
        {
            LayoutAnchorablePane pane = dockingManager.Layout.Descendents().
                OfType<LayoutAnchorablePane>().FirstOrDefault();

            if (pane != null)
            {
                LayoutAnchorable doc = new LayoutAnchorable();
                doc.Title = "VideoTest";

                UserControl Video = new UC_VideoPlayer();
                doc.Content = Video;
                pane.Children.Add(doc);

            }

        }

        private void generarHistograma(object sender, RoutedEventArgs e)
        {
            myHistogram.Points = GraphicActions.getRandomNumbers();
            
        }

        private void anadirPanelVideo(object sender, RoutedEventArgs e)
        {
            LayoutAnchorablePane pane = dockingManager.Layout.Descendents().
                OfType<LayoutAnchorablePane>().FirstOrDefault();

            if (pane != null)
            {
                //TODO do something here
            }
        }

        private void mnitAnadirPanelGrafico_Click(object sender, RoutedEventArgs e)
        {
            LayoutAnchorablePane pane = dockingManager.Layout.Descendents().
                OfType<LayoutAnchorablePane>().FirstOrDefault();

            if (pane != null)
            {
                LayoutAnchorable doc = new LayoutAnchorable();
                doc.Title = "MyGraphPane"; 
                
                Grid grid = new Grid();
                Polyline pL = new Polyline();
                pL.Points = GraphicActions.getRandomNumbers();
                pL.Stroke = Brushes.Black;
                pL.Stretch = Stretch.Fill;
                pL.Name = "myPolyLine";
                grid.Children.Add(pL);
                doc.Content = grid;
                pane.Children.Add(doc);

            }
            
        }

        private void btnPruebas_Click(object sender, RoutedEventArgs e)
        {
            Button b1 = new Button();
            b1.Name = "test";
            
        }


    }
}
