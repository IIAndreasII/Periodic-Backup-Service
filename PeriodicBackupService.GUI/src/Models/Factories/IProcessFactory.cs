using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUI.Models.Factories
{
	public interface IProcessFactory
	{
		IProcessModel Create(params object[] data);
	}
}