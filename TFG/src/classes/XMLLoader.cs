using Microsoft.Win32;
using OxyPlot;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;
using TFG.Properties;
using TFG.src.exceptions;
using TFG.src.ViewModels;

namespace TFG.src.classes
{
	/// <summary>
	/// Author: Ruben Agudo Santos
	/// Singleton class that manages all the operations that are common for the "Graphics" class,
	/// that is, the UC_ChartContainers and UC_DataVisualizer.
	/// </summary>
	public class XMLLoader
	{
		private XElement xml;

		/// <summary>
		/// Constructor of the XMLLoader class
		/// </summary>
		/// <param name="pathToXml">The path to the XML we want to load</param>
		/// <exception cref="FileFormatException">if the XML does not validate this exception is thrown</exception>
		public XMLLoader(string pathToXml)
		{
			if (Validate(pathToXml, Settings.Default.pathToXsd))
			{
				xml = XElement.Load(pathToXml);
			}
			else
			{
				throw new FileFormatException();
			}
		}

		/// <summary>
		/// Method that validates an XML with a given XSD
		/// </summary>
		/// <param name="pathToXml"></param>
		/// <param name="pathToXsd"></param>
		/// <returns>A boolean indicating if the given XML complies with the XSD</returns>
		private bool Validate(string pathToXml, string pathToXsd)
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
		/// Method that creates the needed AbstractDataVisualizerViewModels with the given properties.
		/// </summary>
		/// <param name="propiedades">The properties we want to convert in AbstractDataVisualizerViewModels</param>
		/// <returns>A LinkedList containing the AbstractVisualizerViewModels</returns>
		private LinkedList<AbstractDataVisualizerViewModel> process(IEnumerable<XElement> propiedades)
		{
			LinkedList<AbstractDataVisualizerViewModel> viewModels = new LinkedList<AbstractDataVisualizerViewModel>();
			foreach (XElement prop in propiedades)
			{
				IEnumerable<XElement> data =
					from da in prop.Descendants("instant")
					orderby (double)da.Attribute("ins") ascending
					select da;

				int propType = int.Parse(prop.Attribute("type").Value);

				Dictionary<string, double> labels = new Dictionary<string, double>(data.Count());
				ICollection<DataPoint> pointCollection = createPoints(propType, data, labels);

				List<DataPoint> points = new List<DataPoint>(pointCollection);

				string nombreProp = prop.Attribute("name").Value;
				string observation = prop.Parent.Attribute("name").Value;
				AbstractDataVisualizerViewModel avm = createViewModel(points, labels, propType, nombreProp, observation);
				viewModels.AddLast(avm);
			}

			return viewModels;
			
		}

		/// <summary>
		/// Creates a ContinousDataVisualizerViewModel or a DiscreteDataVisualizerViewModel
		/// </summary>
		/// <param name="points">The points to be added to the DataVisualizerViewModel</param>
		/// <param name="labels">The labels in case it's a Discrete property</param>
		/// <param name="EsContinuo">The value indicating if the property is continous</param>
		/// <param name="title">The property name to set as the title of the chart</param>
		/// <param name="observation">The observation name</param>
		/// <returns>An AbstractDataVisualizerViewModel that must be downcasted to use it correctly</returns>
		private AbstractDataVisualizerViewModel createViewModel(List<DataPoint> points,
			Dictionary<string, double> labels, int propType, string title, string observation)
		{
			AbstractDataVisualizerViewModel avm = null;

			switch (propType)
			{
				case AbstractDataVisualizerViewModel.CONTINOUS:
					avm = new ContinousDataVisualizerViewModel(points, title, observation, propType);
					break;
				case AbstractDataVisualizerViewModel.DISCRETE:
					avm = new DiscreteDataVisualizerViewModel(points, labels.Keys.ToList(), title, observation, propType);
					break;
			}
			return avm;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="EsContinuo"></param>
		/// <param name="data"></param>
		/// <param name="labels"></param>
		/// <returns></returns>
		private ICollection<DataPoint> createPoints(int propType, IEnumerable<XElement> data, 
			Dictionary<string, double> labels)
		{
			ICollection<DataPoint> pointCollection = new LinkedList<DataPoint>();
			int instantLength = getInstantLength();
			double lastX = double.MinValue;
			foreach (XElement instant in data)
			{
				double x = Double.Parse(instant.Attribute("ins").Value);
				double y = 0;

				//si la diferencia entre el ultimo x, y el x actual es mayor que instantLength
				//metemos un NaN para que el grafico sepa que ha de romperse.
				//Y lo metemos en x-instantLength porque sabemos que ahi no va a haber ningun punto
				if (Math.Abs(x - lastX) > instantLength)
				{
					pointCollection.Add(new DataPoint(x-instantLength, double.NaN));
				}


				if (propType != AbstractDataVisualizerViewModel.CONTINOUS)
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

				//actualizamos el ultimo valor conocido
				lastX = x;
			}
			return pointCollection;
		}

		private int getInstantLength()
		{
			XAttribute instantLength = xml.Attribute("instantLength");
			return int.Parse(instantLength.Value);
		}

		/// <summary>
		/// This method shows an Open File Dialog to open XML files.
		/// It can open XML files, it only supports single selection
		/// </summary>
		/// <returns>The path to the selected file</returns>
		/// <exception cref="FileNotSelectedException">Thrown if no file was selected</exception>
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

		/// <summary>
		/// Get all the observations of the loaded XML file that was specified in construction time.
		/// </summary>
		/// <returns></returns>
		internal List<string> getObservations()
		{
			List<string> listObservations = null;
			
			IEnumerable<XElement> observations = 
				from obs in xml.Descendants("observation")
				select obs;

			listObservations = addElements(observations);
			
			return listObservations;
		}

		/// <summary>
		/// Adds the specified elements to a list. To work properly the specified XElements must
		/// contain the attribute "name"
		/// </summary>
		/// <param name="elements">The XElements we want to convert into a list</param>
		/// <returns>a List of strings containning the names of the XElements</returns>
		private List<string> addElements(IEnumerable<XElement> elements)
		{
			List<string> listObservations = new List<string>(elements.Count());

			foreach (XElement observation in elements)
			{
				listObservations.Add(observation.Attribute("name").Value);
			}
			return listObservations;
		}

		/// <summary>
		/// Gets the properties of the given observation
		/// </summary>
		/// <param name="observation">The observation of which we want to know its properties</param>
		/// <returns>A list containing the name of the properties of the given observation</returns>
		internal List<string> getPropertiesOf(string observation)
		{
			List<string> listProperties = null;

			IEnumerable<XElement> properties =
				from prop in xml.Descendants("property")
				where (string)prop.Parent.Attribute("name") == observation
				select prop;

			listProperties = addElements(properties);
			return listProperties;
		}

		/// <summary>
		/// Gets an IEnumerable that contains the data relative to the given observation
		/// </summary>
		/// <param name="observacion">The observation </param>
		/// <returns>An IEnumerable that contains the data of the given observation</returns>
		private IEnumerable<XElement> getData(string observacion)
		{
			return from prop in xml.Descendants("property")
				   where (string)prop.Parent.Attribute("name") == observacion
				   select prop;
		}

		/// <summary>
		/// Gets an IEnumerable that contains the data relative to the given property and observation
		/// </summary>
		/// <param name="propiedad">The property of which we want to get the data</param>
		/// <param name="observacion">The observation that contains the property</param>
		/// <returns>An IEnumerable that contains the data of the given property and observation</returns>
		private IEnumerable<XElement> getData(string propiedad, string observacion)
		{
			return from prop in xml.Descendants("property")
				   where (string)prop.Parent.Attribute("name") == observacion && (string)prop.Attribute("name") == propiedad
				   select prop;
		}


		///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//Metodos dignos de refactorizar
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

		/// <summary>
		/// Loads all the properties belonging to the given observation
		/// </summary>
		/// <param name="observacion">The observation we want to load</param>
		/// <returns>A LinkedList with the viewModels corresponding to that observation</returns>
		internal LinkedList<AbstractDataVisualizerViewModel> LoadXMLData(string observacion)
		{
			IEnumerable<XElement> propiedades = getData(observacion);
			LinkedList<AbstractDataVisualizerViewModel> viewModels = process(propiedades);
			return viewModels;
		}

		/// <summary>
		/// Loads the data that corresponds to a given property and observation.
		/// </summary>
		/// <param name="propiedad">The property we want to load</param>
		/// <param name="observacion">The observation that contains the property.</param>
		/// <returns>A LinkedList with the viewModels corresponding to that observation and property</returns>
		internal LinkedList<AbstractDataVisualizerViewModel> LoadXMLData(string propiedad, string observacion)
		{
			IEnumerable<XElement> propiedades = getData(propiedad, observacion);
			LinkedList<AbstractDataVisualizerViewModel> viewModels = process(propiedades);
			return viewModels;
		}

	}
}	
