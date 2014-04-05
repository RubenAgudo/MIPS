using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Linq;
using System.Data;

namespace TFG.src.classes
{
	public class XMLLoader
	{
		private static XMLLoader myXMLLoader;

		private XMLLoader()
		{
			
		}

		public static XMLLoader getXMLLoader()
		{
			if (myXMLLoader == null)
			{
				myXMLLoader = new XMLLoader();
			}

			return myXMLLoader;
		}


		/// <summary>
		/// Method that validates an XML with a given XSD
		/// </summary>
		/// <param name="pathToXml"></param>
		/// <param name="pathToXsd"></param>
		public void Validate(string pathToXml, string pathToXsd)
		{

			XmlSchemaSet schemas = new XmlSchemaSet();
			schemas.Add("", XmlReader.Create(new StreamReader(pathToXsd)));

			XDocument xml = XDocument.Load(pathToXml);

			Console.WriteLine("Validating xml");
			bool errors = false;
			xml.Validate(schemas, (o, e) =>
			{
				Console.WriteLine("{0}", e.Message);
				errors = true;
			});
			Console.WriteLine("doc1 {0}", errors ? "did not validate" : "validated");


		}

		public void LoadXML(string pathToXml)
		{
			XDocument xml = XDocument.Load(pathToXml);
			foreach (XElement descendant in xml.Descendants())
			{
				
			}
			DataSet dataSet = new DataSet();
			dataSet.ReadXml(@"C:\Books\Books.xml");

		}
	}
}	
