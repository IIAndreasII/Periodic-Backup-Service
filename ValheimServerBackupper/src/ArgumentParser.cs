using System;
using System.Collections.Generic;
using System.Linq;

namespace ValheimServerBackupper
{
	class ArgumentParser
	{
		public static readonly string[] ACCEPTED_ARGS = { "-s", "--sourceDir", "-t", "--targetDir", "-ow", "overwrite", "-d" }; 
		
		private readonly Dictionary<string, string> arguments = new Dictionary<string, string>();

		public ArgumentParser(string[] args)
		{
			ParseArgs(args);
		}

		public string GetArgument(string key)
		{
			return arguments.ContainsKey(key) ? arguments[key] : string.Empty;
		}

		private void ParseArgs(string[] args)
		{		
			for (int i = 0; i < args.Length; i++)
			{
				if (ACCEPTED_ARGS.Contains(args[i]))
				{
					string key = args[i].Trim('-')[0].ToString();
					arguments.Add(key, args[++i]);
				}
				else
				{
					throw new ArgumentException($"Argument \"{args[i]}\" not acceptable!");
				}
			}
		}
	}
}
