﻿using System;

namespace PeriodicBackupService.GUI.Core.IO
{
	[Serializable]
	class BackupProcessSerializable
	{
		public string Name { get; set; }
		public string SourcePath { get; set; }
		public string TargetPath { get; set; }
		public double Interval { get; set; }
		public int MaxNbrBackups { get; set; }
		public bool UseCompression { get; set; }
		public string IntervalUnit { get; set; }
	}
}