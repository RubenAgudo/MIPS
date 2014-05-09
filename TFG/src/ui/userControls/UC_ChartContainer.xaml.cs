using System;
using System.Collections.Generic;
using System.Windows.Controls;
using TFG.src.classes;
using TFG.src.interfaces;
using Xceed.Wpf.AvalonDock.Layout;

namespace TFG.src.ui.userControls
{
    /// <summary>
    /// Lógica de interacción para GraphicsContainer.xaml
    /// </summary>
    public partial class UC_ChartContainer : UserControl, IContainer
    {
		private HashSet<string> loaded;

		public string Observation { get; private set; }

        public UC_ChartContainer(string observation)
        {
            InitializeComponent();
			loaded = new HashSet<string>();
			Observation = observation;
        }

        /// <summary>
        /// Method that adds to the GraphicsContainer a LayoutAnchorable that contains a Data visualizer user control
        /// </summary>
        /// <param name="objectToAdd"></param>
        public void addToAnchorablePane(UserControl objectToAdd, string Title)
        {

            if (mainPanelChartContainer != null)
            {
				if(!loaded.Contains(Title))
				{
					loaded.Add(Title);
					LayoutAnchorable doc = new LayoutAnchorable();
					doc.Hiding += doc_Hiding;
					doc.CanHide = true;
					doc.CanClose = true;
					doc.Title = Title;
					doc.Content = objectToAdd;
					mainPanelChartContainer.Children.Add(doc);
				}
            }
            else
            {
                throw new NotImplementedException();
            }
        }

		private void doc_Hiding(object sender, System.ComponentModel.CancelEventArgs e)
		{
			LayoutAnchorable doc = (LayoutAnchorable)sender;
			UC_DataVisualizer content = (UC_DataVisualizer)doc.Content;
			GraphicActions.getMyGraphicActions().remove(content);
			loaded.Remove(content.Property);
		}
		
	}
}
