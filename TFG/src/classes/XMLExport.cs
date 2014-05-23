using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Xml.Linq;
using TFG.Properties;

namespace TFG.src.classes
{
	public class XMLExport : XMLValidation
	{
		private static XMLExport myXMLExport;
		private LinkedList<XElement> intervalsXML;
		private LinkedList<double[]> intervals;

		private XMLExport() {
			intervalsXML = new LinkedList<XElement>();
			intervals = new LinkedList<double[]>();
		}

		public static XMLExport getMyXMLExport()
		{
			if (myXMLExport == null)
			{
				myXMLExport = new XMLExport();
			}
			return myXMLExport;
		}

		/// <summary>
		/// Creates a new interval.
		/// 
		/// Checks if the given interval overlaps, and saves to the collection of intervals if necessary
		/// </summary>
		/// <param name="start"></param>
		/// <param name="end"></param>
		/// <returns>True if a new interval was created, false otherwise</returns>
		public bool createInterval(double start, double end)
		{
			if (overlaps(start, end)) { return false; }

			XElement anInterval = GraphicActions.getMyGraphicActions().getDataForXML(start, end);
			intervalsXML.AddLast(anInterval);
			intervals.AddLast(new double[] { start, end });
			return true;
		}

		/// <summary>
		/// Checks if the given interval overlaps with the stored ones
		/// </summary>
		/// <param name="start">The start of the interval</param>
		/// <param name="end">The end of the interval</param>
		/// <returns>True if overlaps with any of the stored invertals, false otherwise</returns>
		private bool overlaps(double start, double end)
		{
			foreach (double[] interval in intervals)
			{
				double storedStart = interval[0];
				double storedEnd = interval[1];

				if(start <= storedStart &&
					end >= storedStart)
				{
					return true;
				}

				if(start >= storedStart &&
					end <= storedEnd)
				{
					return true;
				}

				if(start <= storedEnd &&
					end >= storedEnd)
				{
					return true;
				}
				
				if(start <= storedStart &&
					end >= storedEnd)
				{
					return true;
				}
			}
			return false;
		}

		public bool saveData(string rootElement)
		{
			SaveFileDialog dlg = new SaveFileDialog();
			dlg.FileName = rootElement + " " + System.DateTime.Today.Ticks; // Default file name
			dlg.DefaultExt = ".xml"; // Default file extension
			dlg.Filter = "XML documents (.xml)|*.xml"; // Filter files by extension

			// Show save file dialog box
			Nullable<bool> result = dlg.ShowDialog();

			// Process save file dialog box results
			if (result == true)
			{
				// Save document
				string filename = dlg.FileName;
				StreamWriter sw = new StreamWriter(filename);

				Console.WriteLine(intervalsXML.ToString());

				XElement step = new XElement(rootElement);
				XAttribute stepName = new XAttribute("name", dlg.SafeFileName);
				step.Add(stepName);

				foreach (XElement intervalo in intervalsXML)
				{
					step.Add(intervalo);
				}

				sw.Write(step.ToString());
				sw.Close();
				//Despues de guardar, limpiamos los intervalos
				intervalsXML.Clear();
				return Validate(filename, Settings.Default.pathToXsdSave);
				
			}
			return false;
		}

		/// <summary>
		/// Clears the saved data
		/// </summary>
		internal void clear()
		{
			intervals.Clear();
			intervalsXML.Clear();
		}
	}
}
