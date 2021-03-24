namespace PeriodicBackupService.GUI.Core.Models.Factories
{
	public interface IProcessFactory
	{
		IProcessModel Create(params object[] data);
	}
}