using Microsoft.Win32;
using System;
using System.IO;
using System.Xml.Linq;
using TFG.Properties;

namespace TFG.src.classes
{
	public class XMLExport : XMLValidation
	{

		private SaveFileDialog dlg;
		private static XMLExport myXMLExport;

		private XMLExport() {
			dlg = new SaveFileDialog();
		}



		public static XMLExport getMyXMLExport()
		{
			if (myXMLExport == null)
			{
				myXMLExport = new XMLExport();
			}
			return myXMLExport;
		}

		public bool saveData(double start, double end)
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

				XElement paso = GraphicActions.getMyGraphicActions().getDataForXML(filename, start, end);
				sw.WriteLine(paso.ToString());
				sw.Close();
				return Validate(filename, Settings.Default.pathToXsdSave);
				
			}
			return false;
		}
	}
}
