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
using Microsoft.Win32;
using TFG.src.exceptions;

namespace TFG.src.classes
{
    public class GraphicActions
    {

        private LinkedList<UC_DataVisualizer> data;
		private static GraphicActions myGraphicActions;

        private GraphicActions()
        {
            data = new LinkedList<UC_DataVisualizer>();
        }

		public static GraphicActions getMyGraphicActions()
		{
			if (myGraphicActions == null)
			{
				myGraphicActions = new GraphicActions();
			}

			return myGraphicActions;
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
