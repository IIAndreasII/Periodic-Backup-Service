namespace PeriodicBackupService.Services
{
	public interface IIOService
	{
		string GetPath(string defaultPath = "");
	}
}