using JetBrains.ReSharper.Daemon.JavaScript;
using NUnit.Framework;

namespace JSHintForResharper.Tests.Resharper.IssuesTests
{
	[TestFixture]
	public class IssuesTests : JavaScriptHighlightingTestBase
	{
		[Test]
		[TestCase("mingled_directive_ignored.htm")]
		[TestCase("mingled_directive_hacked.htm")]
		public void Mingled_JsLintDirective_IsIgnored(string testFile)
		{
			DoTestFiles(testFile);
		}
	}
}
