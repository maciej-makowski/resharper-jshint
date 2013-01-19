using System;
using System.IO;
using JSHintForResharper.JSHint;
using NUnit.Framework;

namespace JSHintForResharper.Tests.JSHint
{
	[TestFixture]
	public class ScriptStorageTests
	{
		private readonly string expectedPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Resharper.JSHint");

		[TestFixtureSetUp]
		public void TestFixtureSetUp()
		{
			if (Directory.Exists(expectedPath))
				Directory.Delete(expectedPath, true);
		}

		[Test]
		public void Code_ReturnsCompleteJSHintCode()
		{
			Assert.IsFalse(String.IsNullOrEmpty(ScriptStorage.Code));
		}

		[Test]
		public void Paths_ReturnsPathsAtAppData()
		{
			Assert.AreEqual(expectedPath, ScriptStorage.ScriptsPath);
			Assert.AreEqual(Path.Combine(expectedPath, ScriptStorage.LinterFile), ScriptStorage.LintPath);
		}

		[Test]
		public void Initialize_CreatesDirectoryAndScriptFile()
		{
			ScriptStorage.Initialize();
			Assert.IsTrue(Directory.Exists(expectedPath));
			Assert.IsTrue(File.Exists(Path.Combine(expectedPath, ScriptStorage.LinterFile)));
		}
	}
}
