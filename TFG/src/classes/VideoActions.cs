using Microsoft.Win32;
using System;
using System.Collections.Generic;
using TFG.src.exceptions;
using TFG.src.ui.userControls;

namespace TFG.src.classes
{
	/// <summary>
	/// Author: Ruben Agudo Santos
	/// Singleton class that manages all the operations that are common for the "Video" class,
	/// that is, the UC_VideoPlayers. It also keeps track of the video being the reference
	/// to the synchronization
	/// </summary>
    public class VideoActions
    {
        private SortedSet<UC_VideoPlayer> videos;
		private static VideoActions myVideoActions;
		private double startReference;
		private UC_VideoPlayer referenceVideo;

        private VideoActions() 
        {
            videos = new SortedSet<UC_VideoPlayer>();
        }

		/// <summary>
		/// Gets the unique instance of this class
		/// </summary>
		/// <returns>The VideoActions instance</returns>
		public static VideoActions getMyVideoActions()
		{
			if (myVideoActions == null)
			{
				myVideoActions = new VideoActions();
			}

			return myVideoActions;
		}

		/// <summary>
		/// This method shows an Open File Dialog to open videos.
		/// It can open .mp4 and .avi videos. It supports multiple selection
		/// </summary>
		/// <returns>The path to the selected files</returns>
		/// <exception cref="FileNotSelectedException">Thrown if no file was selected</exception>
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

		/// <summary>
		/// Adds the UC_VideoPlayer to the loaded videos
		/// </summary>
		/// <param name="video">The UC_VideoPlayer to be added</param>
		public bool addVideo(UC_VideoPlayer video)
		{
			video.PropertyChanged += video_PropertyChanged;
			return videos.Add(video);
		}

		/// <summary>
		/// Event that handles where the video should begin and which video has to be taken as
		/// reference, this event is called whenever the user clicks on the Start here button in any
		/// UC_VideoPlayer
		/// </summary>
		/// <param name="sender">The UC_VideoPlayer that generated this event</param>
		/// <param name="e"></param>
		private void video_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			if (e.PropertyName == "StartHere")
			{
				startReference = ((UC_VideoPlayer)sender).StartHere;
				referenceVideo = (UC_VideoPlayer)sender;
			}
		}

		/// <summary>
		/// Play every video loaded
		/// </summary>
        internal void play()
        {
            foreach (UC_VideoPlayer actualVideo in videos)
            {
                actualVideo.play();
            }
        }

		/// <summary>
		/// Pauses every video loaded
		/// </summary>
        internal void pause()
        {
            foreach (UC_VideoPlayer actualVideo in videos)
            {
                actualVideo.pause();
            }
        }

		/// <summary>
		/// Stops every video loaded
		/// </summary>
        internal void stop()
        {
            foreach (UC_VideoPlayer actualVideo in videos)
            {
                actualVideo.stop();
            }
        }

		/// <summary>
		/// Advances every video the specified frames in the Settings.
		/// </summary>
        internal void advanceFrame()
        {
            foreach (UC_VideoPlayer actualVideo in videos)
            {
                actualVideo.advanceFrame();                
            }
        }

		/// <summary>
		/// Gets the progress of the reference video.
		/// </summary>
		/// <returns>The actual progress minus the start if a video is taken as a reference,
		/// or zero otherwise</returns>
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

		internal bool removeVideo(UC_VideoPlayer video)
		{
			return videos.Remove(video);
		}
	}
}
