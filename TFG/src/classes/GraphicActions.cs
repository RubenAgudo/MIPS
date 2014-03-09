using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows;
using TFG.src.ui.userControls;
using TFG.src.interfaces;

namespace TFG.src.classes
{
    public class GraphicActions
    {

        LinkedList<UC_DataVisualizer> data;

        public GraphicActions()
        {
            data = new LinkedList<UC_DataVisualizer>();
        }


        public void addLast(UC_DataVisualizer dataVisualizer)
        {
            this.data.AddLast(dataVisualizer);
        }


		internal void update(double p)
		{
			foreach (UC_DataVisualizer datav in data)
			{
				datav.update(p);
			}
		}
	}
}
