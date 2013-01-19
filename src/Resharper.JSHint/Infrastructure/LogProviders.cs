using System;
using System.Diagnostics;
using System.IO;

namespace JSHintForResharper.Infrastructure
{
	public abstract class LogProvider
	{
		private static LogProvider instance;
		public static LogProvider Instance
		{
			get
			{
				return instance ?? 
					(instance = 
						#if DEBUG
						new DebugLogProvider()
						#else
						new FileLogProvider()
						#endif
					);
			}
			set { instance = value; }
		}

		public abstract void Log(string line);
	}

	public class FileLogProvider : LogProvider
	{
		private readonly string path;

		public FileLogProvider()
		{
			path = Path.Combine(
				Path.Combine(
					Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)
					, "Resharper.JSHint"
				)
				, "Log.txt"
			);
		}

		public override void Log(string line)
		{
			try
			{
				using (var writer = new StreamWriter(File.Open(path, FileMode.Append, FileAccess.Write, FileShare.ReadWrite)))
				{
					writer.WriteLine(line);
					writer.Flush();
				}
			}
			catch
			{
				
			}
		}
	}

	public class DebugLogProvider : LogProvider
	{
		public override void Log(string line)
		{
			Debug.WriteLine(line);
		}
	}
}
