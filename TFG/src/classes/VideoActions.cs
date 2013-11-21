using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;

namespace TFG.src.classes
{
    public static class VideoActions
    {
        public static string openFile()
        {
            string filename = "";
            // Configure open file dialog box
            OpenFileDialog dlg = new OpenFileDialog();
            //dlg.FileName = "Document"; // Default file name
            dlg.DefaultExt = ".avi"; // Default file extension
            dlg.Filter = "Video Files (.avi)|*.avi"; // Filter files by extension

            // Show open file dialog box
            Nullable<bool> result = dlg.ShowDialog();

            // Process open file dialog box results 
            if (result == true)
            {
                // Open document 
                filename = dlg.FileName;
            }
            return filename;
        }
    }
}
