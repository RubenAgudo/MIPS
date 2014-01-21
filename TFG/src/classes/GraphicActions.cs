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

        private static PointCollection drawHistogram(int[] data)
        {
            int max = data.Max();

            PointCollection points = new PointCollection();
            // first point (lower-left corner)
            points.Add(new Point(0, max));
            // middle points
            for (int i = 0; i < data.Length; i++)
            {
                points.Add(new Point(i, max - data[i]));
            }
            // last point (lower-right corner)
            points.Add(new Point(data.Length - 1, max));

            return points;
        }

        

        public static PointCollection getRandomNumbers()
        {
            Random r1 = new Random();
            int[] data = new int[10];

            for (int x = 0; x < data.Length; x++)
            {

                data[x] = r1.Next(10);

            }
            return drawHistogram(data);
            
        }

        public void sync()
        {
            foreach (UC_DataVisualizer datav in data)
            {
                datav.sync(new TimeSpan());
            }
        }

        public void addLast(UC_DataVisualizer dataVisualizer)
        {
            this.data.AddLast(dataVisualizer);
        }

    }
}
