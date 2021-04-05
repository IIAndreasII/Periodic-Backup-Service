using System;

namespace GUI.ConsoleApp
{
	internal class Program
	{
		private static void Main(string[] args)
		{
			var app = new App(new ArgumentParser(args));
			Console.CancelKeyPress += app.OnTimedEvent;
			Console.WriteLine("\nPress CTRL+C to exit...");
			while (true)
			{
			}
		}
	}
}