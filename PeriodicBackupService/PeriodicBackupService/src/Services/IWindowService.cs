namespace PeriodicBackupService.Services
{
	public interface IWindowService
	{
		void OpenWindow(object context);

		void CloseWindow();
	}
}