using System;
using System.Collections.Generic;
using System.Linq;

namespace PeriodicBackupService
{
	class ArgumentParser
	{
		public static readonly string[] ACCEPTED_ARGS = { "-s", "-t", "-d", "-c" }; 
		
		private readonly Dictionary<string, string> arguments = new Dictionary<string, string>();


		public ArgumentParser(string[] args)
		{
			ParseArgs(args);
		}

		public string GetArgument(string key)
		{
			return arguments.ContainsKey(key) ? arguments[key] : string.Empty;
		}

		public bool GetBoolArgument(string key)
		{
			if (arguments.ContainsKey(key))
			{
				return bool.Parse(arguments[key]);
			}
			return false;
		}

		private void ParseArgs(string[] args)
		{		
			for (int i = 0; i < args.Length; i++)
			{
				if (ACCEPTED_ARGS.Contains(args[i]))
				{
					string key = args[i].Trim('-')[0].ToString();
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
