using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;
using TFG.src.ui.userControls;
using TFG.src.interfaces;

namespace TFG.src.classes
{
    public class VideoActions
    {
        private LinkedList<UC_VideoPlayer> videos;

        public VideoActions() 
        {
            videos = new LinkedList<UC_VideoPlayer>();
        }

        public void sync() {

            bool first = true;
            UC_VideoPlayer baseVideo = null;
            
            foreach (UC_VideoPlayer actualVideo in videos)
            {

                if (first)
                {
                    baseVideo = actualVideo;
                    first = false;
                }

                actualVideo.pause();
                actualVideo.sync(baseVideo.position());
                //baseVideo = actualVideo;

            }

            foreach (UC_VideoPlayer actualVideo in videos)
            {
                actualVideo.play();
            }

        }

        public static string openFile()
        {
            string filename = "";
            // Configure open file dialog box
            OpenFileDialog dlg = new OpenFileDialog();
            //dlg.FileName = "Document"; // Default file name
            dlg.DefaultExt = ".avi"; // Default file extension
            // Filter files by extension
            dlg.Filter = "All Video Types|*.avi;*.m4v;*.mp4;*.mpg;*.mp4v"; 

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

        public void addVideo(UC_VideoPlayer video)
        {
            videos.AddLast(video);
        }
    }
}
