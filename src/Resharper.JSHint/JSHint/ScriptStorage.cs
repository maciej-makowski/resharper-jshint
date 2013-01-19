using System;
using System.IO;

namespace JSHintForResharper.JSHint
{
	public class ScriptStorage
	{
	    public static readonly string LinterFile = "jshint.js";
		public static readonly string ScriptsPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Resharper.JSHint");
		public static readonly string LintPath = Path.Combine(ScriptsPath, LinterFile);
		public static readonly string Code;

		static ScriptStorage()
		{
            using (var stream = typeof(ScriptStorage).Assembly.GetManifestResourceStream("JSHintForResharper.JSHint.Resources." + LinterFile))
			{
				if (stream == null) throw new NullReferenceException(LinterFile + " is not a resource");
				using (var reader = new StreamReader(stream))
				{
					Code = reader.ReadToEnd();
				}

				Initialize();
			}
		}

		public static void Initialize()
		{
			EnsureDirectory();
			WriteFile();
		}

		private static void EnsureDirectory()
		{
			if (!Directory.Exists(ScriptsPath))
				Directory.CreateDirectory(ScriptsPath);
		}

		private static void WriteFile()
		{
			using (var stream = File.Open(LintPath, FileMode.Create, FileAccess.Write, FileShare.None))
			{
				var writer = new StreamWriter(stream);
				writer.Write(Code);
				writer.Flush();
			}
		}
	}
}
