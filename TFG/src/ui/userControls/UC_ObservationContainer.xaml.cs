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

namespace TFG.src.ui.userControls
{
	/// <summary>
	/// Lógica de interacción para UC_ObservationContainer.xaml
	/// </summary>
	public partial class UC_ObservationContainer : UserControl
	{
		private UC_VideoContainer videoContainer;
		private UC_ChartContainer chartContainer;

		public UC_ObservationContainer()
		{
			InitializeComponent();
		}
	}
}
