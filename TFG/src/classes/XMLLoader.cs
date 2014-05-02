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
	public class XMLLoader
	{
		private XElement xml;

		public XMLLoader(string pathToXml)
		{
			xml = XElement.Load(pathToXml);
		}

		/// <summary>
		/// Method that validates an XML with a given XSD
		/// </summary>
		/// <param name="pathToXsd"></param>
		/// <returns>A boolean indicating if the given XML complies with the XSD</returns>
		private bool Validate(string pathToXsd)
		{
			XmlSchemaSet schemas = new XmlSchemaSet();
			schemas.Add("", XmlReader.Create(new StreamReader(pathToXsd)));

			XDocument xml = XDocument.Load(this.xml.CreateReader());

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

		private void process(IEnumerable<XElement> propiedades, 
			LinkedList<AbstractDataVisualizerViewModel> viewModels)
		{

			foreach (XElement prop in propiedades)
			{
				IEnumerable<XElement> data =
					from da in prop.Descendants("instant")
					orderby (double)da.Attribute("ins") ascending
					select da;

				bool EsContinuo = isContinousProperty(prop);

				Dictionary<string, double> labels = new Dictionary<string, double>(data.Count());
				ICollection<DataPoint> pointCollection = createPoints(EsContinuo, data, labels);

				List<DataPoint> points = new List<DataPoint>(pointCollection);

				string nombreProp = prop.Attribute("name").Value;
				string observation = prop.Parent.Attribute("name").Value;
				AbstractDataVisualizerViewModel avm = createViewModel(points, labels, EsContinuo, nombreProp, observation);
				viewModels.AddLast(avm);
			}
			
		}

		private bool isContinousProperty(XElement prop)
		{
			return int.Parse(prop.Attribute("type").Value) == AbstractDataVisualizerViewModel.CONTINOUS;
		}

		private AbstractDataVisualizerViewModel createViewModel(List<DataPoint> points,
			Dictionary<string, double> labels, bool EsContinuo, string title, string observation)
		{
			AbstractDataVisualizerViewModel avm = null;
			if (EsContinuo)
			{
				avm = new ContinousDataVisualizerViewModel(points, title, observation);
			}
			else
			{
				avm = new DiscreteDataVisualizerViewModel(points, labels.Keys.ToList(), title, observation);
			}
			return avm;
		}

		private ICollection<DataPoint> createPoints(bool EsContinuo, IEnumerable<XElement> data, 
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


		public static string openXML()
		{
			string filename = null;
			// Configure open file dialog box
			OpenFileDialog dlg = new OpenFileDialog();
			//dlg.FileName = "Document"; // Default file name
			dlg.Multiselect = false;
			dlg.DefaultExt = ".xml"; // Default file extension
			// Filter files by extension
			dlg.Filter = "XML files|*.xml";

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

		internal List<string> getObservations()
		{
			List<string> listObservations = null;
			if (Validate("schemas/ObservationModelData.xsd"))
			{				
				IEnumerable<XElement> observations = 
					from obs in xml.Descendants("observation")
					select obs;

				listObservations = addElements(observations);
			}
			else
			{
				throw new FileFormatException();
			}
			return listObservations;
		}

		private List<string> addElements(IEnumerable<XElement> elements)
		{
			List<string> listObservations = new List<string>(elements.Count());

			foreach (XElement observation in elements)
			{
				listObservations.Add(observation.Attribute("name").Value);
			}
			return listObservations;
		}

		internal List<string> getPropertiesOf(string observation)
		{
			List<string> listProperties = null;
			if (Validate("schemas/ObservationModelData.xsd"))
			{
				IEnumerable<XElement> properties =
					from prop in xml.Descendants("property")
					where (string)prop.Parent.Attribute("name") == observation
					select prop;

				listProperties = addElements(properties);
			}
			return listProperties;
		}


		private IEnumerable<XElement> getData(XElement xml)
		{
			return from prop in xml.Descendants("property")
				   select prop;
		}

		private IEnumerable<XElement> getData(string observacion)
		{
			return from prop in xml.Descendants("property")
				   where (string)prop.Parent.Attribute("name") == observacion
				   select prop;
		}

		private IEnumerable<XElement> getData(string propiedad, string observacion)
		{
			return from prop in xml.Descendants("property")
				   where (string)prop.Parent.Attribute("name") == observacion && (string)prop.Attribute("name") == propiedad
				   select prop;
		}


		///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//Metodos dignos de refactorizar
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

		internal LinkedList<AbstractDataVisualizerViewModel> LoadXMLData(string observacion)
		{
			LinkedList<AbstractDataVisualizerViewModel> viewModels = new LinkedList<AbstractDataVisualizerViewModel>();
			
			if (Validate("schemas/ObservationModelData.xsd"))
			{

				IEnumerable<XElement> propiedades = getData(observacion);
				process(propiedades, viewModels);

			}
			else
			{
				throw new FileFormatException();
			}
			return viewModels;
		}

		internal LinkedList<AbstractDataVisualizerViewModel> LoadXMLData(string propiedad, string observacion)
		{
			LinkedList<AbstractDataVisualizerViewModel> viewModels = new LinkedList<AbstractDataVisualizerViewModel>();
			
			if (Validate("schemas/ObservationModelData.xsd"))
			{
				IEnumerable<XElement> propiedades = getData(propiedad, observacion);
				process(propiedades, viewModels);

			}
			else
			{
				throw new FileFormatException();
			}
			return viewModels;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="pathToXml"></param>
		/// <returns></returns>
		public LinkedList<AbstractDataVisualizerViewModel> LoadXMLData()
		{
			LinkedList<AbstractDataVisualizerViewModel> viewModels = new LinkedList<AbstractDataVisualizerViewModel>();

			if (Validate("schemas/ObservationModelData.xsd"))
			{

				IEnumerable<XElement> propiedades = getData(xml);
				process(propiedades, viewModels);

			}
			else
			{
				throw new FileFormatException();
			}
			return viewModels;
		}

	}
}	
