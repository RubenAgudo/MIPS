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
using TFG.src.ViewModels;
using Xceed.Wpf.AvalonDock;
using Xceed.Wpf.AvalonDock.Layout;

namespace TFG.src.ui.userControls
{
    /// <summary>
    /// Lógica de interacción para GraphicsContainer.xaml
    /// </summary>
    public partial class UC_ChartContainer : UserControl
    {
        private int paneNumber;

        public UC_ChartContainer()
        {
            InitializeComponent();

            paneNumber = 0;
        }

        /// <summary>
        /// Method that adds to the GraphicsContainer a LayoutAnchorable that contains a Data visualizer user control
        /// </summary>
        /// <param name="objectToAdd"></param>
        private void addToAnchorablePane(UserControl objectToAdd)
        {

            if (mainPanelChartContainer != null)
            {
				LayoutAnchorable doc = new LayoutAnchorable();
				doc.CanHide = false;
				doc.CanClose = true;
				doc.Title = "Pane " + ++paneNumber;
				doc.Content = objectToAdd;
                mainPanelChartContainer.Children.Add(doc);

            }
            else
            {
                throw new NotImplementedException();
            }
        }

		/// <summary>
		/// Method that updates the chart progress indicator.
		/// </summary>
		/// <param name="p">The new position of the indicator</param>
		internal void Update(double p)
		{
			GraphicActions.getMyGraphicActions().update(p);
		}

		private void mnitAddContinousChart_Click(object sender, RoutedEventArgs e)
		{
			UC_DataVisualizer dataVisualizer = new UC_DataVisualizer(AbstractDataVisualizerViewModel.CONTINOUS);
			GraphicActions.getMyGraphicActions().addLast(dataVisualizer);
			addToAnchorablePane(dataVisualizer);
		}

		private void mnitAddDiscreteChart_Click(object sender, RoutedEventArgs e)
		{
			
			UC_DataVisualizer dataVisualizer = new UC_DataVisualizer(AbstractDataVisualizerViewModel.DISCRETE);
			GraphicActions.getMyGraphicActions().addLast(dataVisualizer);
			addToAnchorablePane(dataVisualizer);
		}

		private void mnitLoadXML_Click(object sender, RoutedEventArgs e)
		{
			TreeViewItem newChild = new TreeViewItem();
			newChild.Header = paneNumber++;
			//observationsAndProperties.Items.Add(newChild);
		}
	}
}
