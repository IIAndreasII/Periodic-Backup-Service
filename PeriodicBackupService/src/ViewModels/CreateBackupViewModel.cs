using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeriodicBackupService.ViewModels
{
	public class CreateBackupViewModel : ViewModelBase, IPageViewModel
	{
		public override void Dispose()
		{
		}

		public string Name => "Create backup";
	}
}