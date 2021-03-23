namespace GUI.Models.Factories
{
	public interface IProcessFactory
	{
		IProcessModel Create(params object[] data);
	}
}