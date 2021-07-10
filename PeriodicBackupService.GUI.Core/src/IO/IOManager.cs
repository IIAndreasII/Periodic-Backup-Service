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
			foreach (var bps in items.Select(it => new BackupProcessSerializable
			{
				SourcePath = it.SourcePath,
				TargetPath = it.TargetPath,
				UseCompression = it.UseCompression,
				Interval = it.Interval,
				IntervalUnit = it.IntervalUnit,
				MaxNbrBackups = it.MaxNbrBackups,
				Name = it.Name
			}))
			{
				itemList.Add(bps);
			}
		}

		public void Write(string filepath)
		{
			try
			{
				using var file = File.Create(filepath);
				using var streamWriter = new StreamWriter(file);
				streamWriter.Write(Serialized());
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
					true.ToString(), string.Empty)).ToList();
			return thing;
		}

		public string Serialized()
		{
			return JsonConvert.SerializeObject(itemList);
		}
	}
}