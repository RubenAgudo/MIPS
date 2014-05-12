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

				XElement paso = GraphicActions.getMyGraphicActions().getDataForXML(filename);

				//XElement contacts =
				//	new XElement("paso",
				//		new XAttribute("name", "paso1"),
				//		new XElement("intervalo",
				//			new XAttribute("inicio", 0.0),
				//			new XAttribute("fin", 10.5),

				//			new XElement("instante",
				//				new XAttribute("instante", 0.0),

				//				new XElement("observacion",
				//					new XAttribute("nombreObservacion", "obs1"),
				//					new XElement("propiedad" ,
				//						new XAttribute("nombrePropiedad", "prop11"),
				//						new XAttribute("tipo", 0),
				//						new XElement("data", 10)
				//					)
				//				),

				//				new XElement("observacion",
				//					new XAttribute("nombreObservacion", "obs2"),
				//					new XElement("propiedad",
				//						new XAttribute("nombrePropiedad", "prop21"),
				//						new XAttribute("tipo", 0),
				//						new XElement("data", 20)
				//					)
				//				)
				//			),

				//			new XElement("instante",
				//				new XAttribute("instante", 1.5),

				//				new XElement("observacion",
				//					new XAttribute("nombreObservacion", "obs1"),
				//					new XElement("propiedad" ,
				//						new XAttribute("nombrePropiedad", "prop11"),
				//						new XAttribute("tipo", 0),
				//						new XElement("data", 15)
				//					)
				//				),

				//				new XElement("observacion",
				//					new XAttribute("nombreObservacion", "obs2"),
				//					new XElement("propiedad",
				//						new XAttribute("nombrePropiedad", "prop21"),
				//						new XAttribute("tipo", 0),
				//						new XElement("data", 25)
				//					)
				//				)
				//			)
				//		)
				//	);

				//XElement contacts2 =
				//	new XElement("Contacts",
				//		new XElement("Contact",
				//			new XElement("Name", "Patrick Hines"),
				//			new XElement("Phone", "206-555-0144",
				//				new XAttribute("Type", "Home")),
				//			new XElement("phone", "425-555-0145",
				//				new XAttribute("Type", "Work")),
				//			new XElement("Address",
				//				new XElement("Street1", "123 Main St"),
				//				new XElement("City", "Mercer Island"),
				//				new XElement("State", "WA"),
				//				new XElement("Postal", "68042")
				//			)
				//		)
				//	);

				sw.WriteLine(paso.ToString());
				
				sw.Close();

			}
		}
	}
}
