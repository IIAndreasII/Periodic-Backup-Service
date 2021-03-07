﻿using System;

namespace ValheimServerBackupper
{
	class Program
	{
		static void Main(string[] args)
		{
			App app = new App(new ArgumentParser(args));
			Console.CancelKeyPress += app.OnTimedEvent;
			Console.WriteLine("\nPress CTRL+C to exit...");
			while (true) ;
		}		
	}
}
