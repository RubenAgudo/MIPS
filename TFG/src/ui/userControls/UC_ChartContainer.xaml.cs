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

        private GraphicActions graphicActions;
        private int paneNumber;

        public UC_ChartContainer()
        {
            InitializeComponent();

            graphicActions = new GraphicActions();
            paneNumber = 0;
        }

        /// <summary>
        /// Method that adds to the GraphicsContainer a LayoutAnchorable that contains a Data visualizer user control
        /// </summary>
        /// <param name="objectToAdd"></param>
        private void addToAnchorablePane(UserControl objectToAdd)
        {
            LayoutAnchorablePane pane = chartContainer.Layout.Descendents().
                 OfType<LayoutAnchorablePane>().FirstOrDefault();

            if (pane != null)
            {
                LayoutAnchorable doc = new LayoutAnchorable();
                doc.CanHide = false;
                doc.CanClose = true;
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
		/// Method that updates the chart progress indicator.
		/// </summary>
		/// <param name="p">The new position of the indicator</param>
		internal void Update(double p)
		{
			graphicActions.update(p);
		}

		private void mnitAddContinousChart_Click(object sender, RoutedEventArgs e)
		{
			UC_DataVisualizer dataVisualizer = new UC_DataVisualizer(AbstractDataVisualizerViewModel.CONTINOUS);
			graphicActions.addLast(dataVisualizer);
			addToAnchorablePane(dataVisualizer);
		}

		private void mnitAddDiscreteChart_Click(object sender, RoutedEventArgs e)
		{
			
			UC_DataVisualizer dataVisualizer = new UC_DataVisualizer(AbstractDataVisualizerViewModel.DISCRETE);
			graphicActions.addLast(dataVisualizer);
			addToAnchorablePane(dataVisualizer);
		}
	}
}
