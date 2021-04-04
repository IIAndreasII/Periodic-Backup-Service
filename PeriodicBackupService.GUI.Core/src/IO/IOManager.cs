using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using PeriodicBackupService.GUI.Core.Models;
using PeriodicBackupService.GUI.Core.Models.Factories;

namespace PeriodicBackupService.GUI.Core.IO
{
	public class IOManager
	{
		private readonly List<BackupProcessSerializable> itemList = new List<BackupProcessSerializable>();

		public void SetItems(List<IProcessModel> items)
		{
			itemList.Clear();
			foreach (var it in items)
			{
				var bps = new BackupProcessSerializable
				{
					SourcePath = it.SourcePath,
					TargetPath = it.TargetPath,
					UseCompression = it.UseCompression,
					Interval = it.Interval,
					IntervalUnit = it.IntervalUnit,
					MaxNbrBackups = it.MaxNbrBackups,
					Name = it.Name
				};

				itemList.Add(bps);
			}
		}

		public void Write(string filepath)
		{
			try
			{
				using var fs = File.Create(filepath);
				using var sw = new StreamWriter(fs);
				sw.Write(Serialized());
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
				throw;
			}
		}

		public string Read(string filepath)
		{
			try
			{
				using var fs = File.OpenRead(filepath);
				using var sr = new StreamReader(fs);
				return sr.ReadToEnd();
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
				throw;
			}
		}

		public List<IProcessModel> Deserialized(string filepath, IProcessFactory factory)
		{
			var thing = JsonConvert.DeserializeObject<List<BackupProcessSerializable>>(Read(filepath))?.Select(it =>
				factory.Create(it.Name, it.SourcePath, it.TargetPath, it.MaxNbrBackups.ToString(),
					it.Interval.ToString(),
					it.IntervalUnit,
					it.UseCompression.ToString(),
					true.ToString(), "")).ToList();
			return thing;
		}

		public string Serialized()
		{
			return JsonConvert.SerializeObject(itemList);
		}
	}
}