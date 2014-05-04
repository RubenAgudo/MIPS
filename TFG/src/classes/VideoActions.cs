using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;
using TFG.src.ui.userControls;
using TFG.src.interfaces;
using TFG.src.exceptions;

namespace TFG.src.classes
{
    public class VideoActions
    {
        private LinkedList<UC_VideoPlayer> videos;
		private static VideoActions myVideoActions;

        private VideoActions() 
        {
            videos = new LinkedList<UC_VideoPlayer>();
        }

		public static VideoActions getMyVideoActions()
		{
			if (myVideoActions == null)
			{
				myVideoActions = new VideoActions();
			}

			return myVideoActions;
		}

        public static string[] openFile()
        {
            string[] filenames = null;
            // Configure open file dialog box
            OpenFileDialog dlg = new OpenFileDialog();
            //dlg.FileName = "Document"; // Default file name
            dlg.Multiselect = true;
            dlg.DefaultExt = ".avi"; // Default file extension
            // Filter files by extension
            dlg.Filter = "AVI Videos|*.avi|MP4 Videos|*.mp4|All supported videos|*.avi; *.mp4"; 

            // Show open file dialog box
            Nullable<bool> result = dlg.ShowDialog();

            // Process open file dialog box results 
            if (result == true)
            {
                // Open document 
                filenames = dlg.FileNames;
            }
            else
            {
                throw new FileNotSelectedException();
            }
            return filenames;
        }

        public void addVideo(UC_VideoPlayer video)
        {
            videos.AddLast(video);
        }

        internal void play()
        {
            foreach (UC_VideoPlayer actualVideo in videos)
            {
                actualVideo.play();
            }
        }

        internal void pause()
        {
            foreach (UC_VideoPlayer actualVideo in videos)
            {
                actualVideo.pause();
            }
        }

        internal void stop()
        {
            foreach (UC_VideoPlayer actualVideo in videos)
            {
                actualVideo.stop();
            }
        }



        internal void advanceFrame()
        {
            foreach (UC_VideoPlayer actualVideo in videos)
            {
                actualVideo.advanceFrame();                
            }
        }

		internal double getLongestVideoProgress()
		{
			if (videos.Count > 0)
			{
				return videos.First.Value.Position.TotalSeconds;
			}
			else
			{
				return 0d;
			}
			
		}
	}
}
