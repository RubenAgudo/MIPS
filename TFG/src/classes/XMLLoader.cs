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
using OxyPlot;
using TFG.src.exceptions;
using Microsoft.Win32;
using TFG.src.ViewModels;

namespace TFG.src.classes
{
	public static class XMLLoader
	{
		//private static XMLLoader myXMLLoader;

		//public static XMLLoader getXMLLoader()
		//{
		//	if (myXMLLoader == null)
		//	{
		//		myXMLLoader = new XMLLoader();
		//	}

		//	return myXMLLoader;
		//}


		/// <summary>
		/// Method that validates an XML with a given XSD
		/// </summary>
		/// <param name="pathToXml"></param>
		/// <param name="pathToXsd"></param>
		/// <returns>A boolean indicating if the given XML complies with the XSD</returns>
		public static bool Validate(string pathToXml, string pathToXsd)
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
			return !errors;

		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="pathToXml"></param>
		/// <returns></returns>
		public static LinkedList<AbstractDataVisualizerViewModel> LoadXMLData(string pathToXml)
		{
			LinkedList<AbstractDataVisualizerViewModel> viewModels = new LinkedList<AbstractDataVisualizerViewModel>();

			if (Validate(pathToXml, "schemas/ObservationModelData.xsd"))
			{
				
				XElement xml = XElement.Load(pathToXml);

				IEnumerable<XElement> propiedadesContinuas = getData(xml, AbstractDataVisualizerViewModel.CONTINOUS);
				process(propiedadesContinuas, true, viewModels);

				IEnumerable<XElement> propiedadesDiscretas = getData(xml, AbstractDataVisualizerViewModel.DISCRETE);
				process(propiedadesDiscretas, false, viewModels);

			}
			else
			{
				throw new FileFormatException();
			}
			return viewModels;
		}


		private static void process(IEnumerable<XElement> propiedades, bool EsContinuo, 
			LinkedList<AbstractDataVisualizerViewModel> viewModels)
		{

			foreach (XElement prop in propiedades)
			{
				IEnumerable<XElement> data =
					from da in prop.Descendants("instant")
					orderby (double)da.Attribute("ins") ascending
					select da;


				Dictionary<string, double> labels = new Dictionary<string, double>(data.Count());
				ICollection<DataPoint> pointCollection = createPoints(EsContinuo, data, labels);

				List<DataPoint> points = new List<DataPoint>(pointCollection);

				AbstractDataVisualizerViewModel avm = createViewModel(EsContinuo, points, labels);
				viewModels.AddLast(avm);
			}
			
		}

		private static AbstractDataVisualizerViewModel createViewModel(bool EsContinuo, 
			List<DataPoint> points, Dictionary<string, double> labels)
		{
			AbstractDataVisualizerViewModel avm = null;
			if (EsContinuo)
			{
				avm = new ContinousDataVisualizerViewModel(points);
			}
			else
			{
				avm = new DiscreteDataVisualizerViewModel(points, labels.Keys.ToList());
			}
			return avm;
		}

		private static ICollection<DataPoint> createPoints(bool EsContinuo, IEnumerable<XElement> data, 
			Dictionary<string, double> labels)
		{
			ICollection<DataPoint> pointCollection = new LinkedList<DataPoint>();
			
			foreach (XElement instant in data)
			{
				double x = Double.Parse(instant.Attribute("ins").Value);
				double y = 0;
				if (!EsContinuo)
				{

					bool success = labels.TryGetValue(instant.Attribute("value").Value, out y);
					if (!success)
					{
						y = labels.Count;
						labels.Add(instant.Attribute("value").Value, labels.Count);
					}
				}
				else
				{
					y = Double.Parse(instant.Attribute("value").Value);
				}

				pointCollection.Add(new DataPoint(x, y));
			}
			return pointCollection;
		}

		private static IEnumerable<XElement> getData(XElement xml, int dataType)
		{
			return 	from prop in xml.Descendants("property")
					where (int)prop.Attribute("type") == dataType
					select prop;
		}

		public static string openFile()
		{
			string filename = null;
			// Configure open file dialog box
			OpenFileDialog dlg = new OpenFileDialog();
			//dlg.FileName = "Document"; // Default file name
			dlg.Multiselect = false;
			dlg.DefaultExt = ".xml"; // Default file extension
			// Filter files by extension
			dlg.Filter = "XML Files|*.xml";

			// Show open file dialog box
			Nullable<bool> result = dlg.ShowDialog();

			// Process open file dialog box results 
			if (result == true)
			{
				// Open document 
				filename = dlg.FileName;
			}
			else
			{
				throw new FileNotSelectedException();
			}
			return filename;
		}

		internal static List<string> getObservations(string pathToXML)
		{
			List<string> listObservations = null;
			if (Validate(pathToXML, "schemas/ObservationModelData.xsd"))
			{
				XElement xml = XElement.Load(pathToXML);
				
				IEnumerable<XElement> observations = 
					from obs in xml.Descendants("observation")
					select obs;

				listObservations = addElements(observations);
			}
			return listObservations;
		}

		private static List<string> addElements(IEnumerable<XElement> elements)
		{
			List<string> listObservations = new List<string>(elements.Count());

			foreach (XElement observation in elements)
			{
				listObservations.Add(observation.Attribute("name").Value);
			}
			return listObservations;
		}

		internal static List<string> getPropertiesOf(string observation, string pathToXML)
		{
			List<string> listProperties = null;
			if (Validate(pathToXML, "schemas/ObservationModelData.xsd"))
			{
				XElement xml = XElement.Load(pathToXML);

				IEnumerable<XElement> properties =
					from prop in xml.Descendants("property")
					where (string)prop.Parent.Attribute("name") == observation
					select prop;

				listProperties = addElements(properties);
			}
			return listProperties;
		}

	}
}	
