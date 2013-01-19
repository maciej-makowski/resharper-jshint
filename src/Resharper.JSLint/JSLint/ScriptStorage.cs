using System;
using System.IO;

namespace JSLintForResharper.JSLint
{
	public class ScriptStorage
	{
		public static readonly string ScriptsPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Resharper.JSLint");
		public static readonly string LintPath = Path.Combine(ScriptsPath, "jslint.js");
		public static readonly string Code;

		static ScriptStorage()
		{
			using (var stream = typeof (ScriptStorage).Assembly.GetManifestResourceStream("JSLintForResharper.JSLint.Resources.jslint.js"))
			{
				if (stream == null) throw new NullReferenceException("jslint.js is not a resource");
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
