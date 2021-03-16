using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUI.Services
{
	public interface IWindowService
	{
		void OpenWindow(object context);

		void CloseWindow();
	}
}