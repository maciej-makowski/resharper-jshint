using System;
using System.IO;
using JSLintForResharper.JSLint;
using NUnit.Framework;

namespace JSLintForResharper.Tests.JSLint
{
	[TestFixture]
	public class ScriptStorageTests
	{
		private readonly string expectedPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Resharper.JSLint");

		[TestFixtureSetUp]
		public void TestFixtureSetUp()
		{
			if (Directory.Exists(expectedPath))
				Directory.Delete(expectedPath, true);
		}

		[Test]
		public void Code_ReturnsCompleteJSLintCode()
		{
			Assert.IsFalse(String.IsNullOrEmpty(ScriptStorage.Code));
		}

		[Test]
		public void Paths_ReturnsPathsAtAppData()
		{
			Assert.AreEqual(expectedPath, ScriptStorage.ScriptsPath);
			Assert.AreEqual(Path.Combine(expectedPath, "jslint.js"), ScriptStorage.LintPath);
		}

		[Test]
		public void Initialize_CreatesDirectoryAndScriptFile()
		{
			ScriptStorage.Initialize();
			Assert.IsTrue(Directory.Exists(expectedPath));
			Assert.IsTrue(File.Exists(Path.Combine(expectedPath, "jslint.js")));
		}
	}
}
