using System;
using System.Collections.Generic;
using System.Linq;

namespace PeriodicBackupService.ConsoleApp
{
	internal class ArgumentParser
	{
		public static readonly string[] ACCEPTED_ARGS = {"-s", "-t", "-d", "-c"};

		private readonly Dictionary<string, string> arguments = new Dictionary<string, string>();


		public ArgumentParser(IReadOnlyList<string> args)
		{
			ParseArgs(args);
		}

		public string GetArgument(string key)
		{
			return arguments.ContainsKey(key) ? arguments[key] : string.Empty;
		}

		public bool GetBoolArgument(string key)
		{
			return arguments.ContainsKey(key) && bool.Parse(arguments[key]);
		}

		private void ParseArgs(IReadOnlyList<string> args)
		{
			for (var i = 0; i < args.Count; i++)
			{
				if (ACCEPTED_ARGS.Contains(args[i]))
				{
					var key = args[i].Trim('-')[0].ToString();
					string value;

					if (key == "c")
					{
						value = "true";
						i++;
					}
					else
					{
						value = args[++i];
					}

					arguments.Add(key, value);
				}
				else
				{
					throw new ArgumentException($"Argument \"{args[i]}\" not acceptable!");
				}
			}
		}
	}
}