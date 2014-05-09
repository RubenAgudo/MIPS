using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Linq;

namespace TFG.src.classes
{
	public class XMLExport
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

		public void saveData()
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

				XElement contacts =
					new XElement("Contacts",
						new XElement("Contact",
							new XElement("Name", "Patrick Hines"),
							new XElement("Phone", "206-555-0144",
								new XAttribute("Type", "Home")),
							new XElement("phone", "425-555-0145",
								new XAttribute("Type", "Work")),
							new XElement("Address",
								new XElement("Street1", "123 Main St"),
								new XElement("City", "Mercer Island"),
								new XElement("State", "WA"),
								new XElement("Postal", "68042")
							)
						)
					);

				sw.WriteLine(contacts.ToString());
				
				sw.Close();

			}
		}
	}
}
