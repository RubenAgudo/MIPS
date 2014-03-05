using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Resources;

namespace TFG.res
{
		

	public class Localizer 
	{

		ResourceManager manager;

		static Localizer localizer = new Localizer();

		private Localizer() {

			manager = new ResourceManager("MyApp.UserMessages", this.GetType().Assembly);

		}

		public static string GetString(StringId id) {

			string ret = localizer.manager.GetString(id.ToString());

			if(ret == null) 

				throw new Exception(string.Format("The localized string for {0} is not found", id));

			return ret;

		}

		public enum StringId {

			MsgGreeting

		//			...  // other IDs

		}
	}

}
