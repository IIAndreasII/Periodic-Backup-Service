namespace PeriodicBackupService.GUI.Core.Services
{
	public interface IIOService
	{
		string GetPath(string defaultPath = "");
	}
}