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
using TFG.src.interfaces;
using TFG.src.classes; 

namespace TFG.src.ui.userControls
{
    /// <summary>
    /// Lógica de interacción para UC_DataVisualizer.xaml
    /// </summary>
    public partial class UC_DataVisualizer : UserControl, ISynchronizable
    {
        private Data m_PetData;
        public UC_DataVisualizer()
        {
            InitializeComponent();
            m_PetData = new Data();
            DataContext = m_PetData;

        }

        public void sync(TimeSpan timePosition)
        {
            throw new NotImplementedException();
        }

        public class Data : Dictionary<int, int>
        {
             public Data()
            {
                    Add(1, 7);
                    Add(2, 10);
                    Add(3, 11);
                    Add(4, 6);
                    Add(5, 6);
            }
        }
    }
}
