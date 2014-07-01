using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace MIPS.src.interfaces
{
	interface IContainer
	{
		void addToAnchorablePane(UserControl objectToAdd, string Title);
	}
}
