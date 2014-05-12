using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using TFG.Properties;

namespace TFG.src.classes
{
	public class XMLExport : XMLValidation
	{
		private SaveFileDialog dlg;
		private static XMLExport myXMLExport;
		private LinkedList<XElement> intervalsXML;
		private LinkedList<double[]> intervals;

		private XMLExport() {
			dlg = new SaveFileDialog();
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

		public void createInterval(double start, double end, bool saveToDisk)
		{
			if (overlaps(start, end)) { return; }

			XElement anInterval = GraphicActions.getMyGraphicActions().getDataForXML(start, end);
			intervalsXML.AddLast(anInterval);
			intervals.AddLast(new double[] { start, end });
			if (saveToDisk)
			{
				saveData();
			}
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

				if(start < storedStart &&
					end > storedStart)
				{
					return true;
				}

				if(start > storedStart &&
					end < storedEnd)
				{
					return true;
				}

				if(start < storedEnd &&
					end > storedEnd)
				{
					return true;
				}
				
				if(start < storedStart &&
					end > storedEnd)
				{
					return true;
				}
			}
			return false;
		}

		private bool saveData()
		{
			dlg.FileName = "Step " + System.DateTime.Today.Ticks; // Default file name
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

				XElement step = new XElement("paso");
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
	}
}
