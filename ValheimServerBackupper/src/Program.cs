using System;

namespace ValheimServerBackupper
{
	class Program
	{
		static void Main(string[] args)
		{
			Console.CancelKeyPress += App.OnTimedEvent;
			App.Init(new ArgumentParser(args));
			Console.WriteLine("Press CTRL+C to exit...");
			while (true) ;
		}		
	}
}
