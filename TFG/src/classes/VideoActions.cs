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
		private double startReference;
		private UC_VideoPlayer referenceVideo;

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
			video.PropertyChanged += video_PropertyChanged;
            videos.AddLast(video);
        }

		void video_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			if (e.PropertyName == "StartHere")
			{
				startReference = ((UC_VideoPlayer)sender).StartHere;
				referenceVideo = (UC_VideoPlayer)sender;
			}
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

		internal double getVideoProgress()
		{
			if (referenceVideo != null)
			{
				double value = referenceVideo.Position.TotalSeconds - startReference;
				return value;
			}
			else
			{
				return 0d;
			}
			
		}

		internal void clear()
		{
			videos.Clear();
		}
	}
}
