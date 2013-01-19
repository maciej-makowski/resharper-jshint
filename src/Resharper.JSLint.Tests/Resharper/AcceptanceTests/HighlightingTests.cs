using System.Collections.Generic;
using JSLintForResharper.Infrastructure;
using JSLintForResharper.Resharper.Highlighting;
using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Daemon.JavaScript;
using JetBrains.ReSharper.Psi;
using NUnit.Framework;

namespace JSLintForResharper.Tests.Resharper.AcceptanceTests
{
	[TestFixture]
	public class HighlightingTests : JavaScriptHighlightingTestBase
	{
		protected override bool HighlightingPredicate(IHighlighting highlighting, JetBrains.Application.Settings.IContextBoundSettingsStore settingsstore)
		{
			return highlighting is LintHighlightingBase;
		}

		[Test]
		[TestCase("OneLineWithUndefined.js", true)]
		[TestCase("UndefinedGlobalAndSemi.js", false)]
		[TestCase("Closure.js", false)]
		[TestCase("MingledHtml.htm", true)]
		[TestCase("GlobalsSemiAndComma.js", false)]
		[TestCase("MingledAspx.aspx", true)]
		[TestCase("jquery-1.7.2.js", false)]
		public void TestHighlighting(string file, bool priority = false)
		{
			Settings.Values = new Dictionary<string, object>();
			if (priority)
				Settings.Values = new Dictionary<string, object>{{"R#_priority", true}};
			DoTestFiles(file);
		}
	}
}
