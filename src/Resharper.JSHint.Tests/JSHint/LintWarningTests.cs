using System.Collections.Generic;
using System.Linq;
using JSHintForResharper.JSHint;
using NUnit.Framework;

namespace JSHintForResharper.Tests.JSHint
{
	[TestFixture]
	public class LintWarningTests
	{
		[Test]
		public void Parse_ReturnsInstance()
		{
			const string line = "1	1	'one' was used before it was defined.	one";
			var result = LintWarning.Parse(line);
			Assert.AreEqual(1, result.Line);
			Assert.AreEqual(1, result.Character);
			Assert.AreEqual("'one' was used before it was defined.", result.Message);
			Assert.AreEqual("one", result.Evidence);
		}

		[Test]
		[TestCase("")]
		[TestCase("		invalid	")]
		public void Parse_WhenInvalid_ReturnsNull(string input)
		{
			Assert.IsNull(LintWarning.Parse(input));
		}

		[Test]
		public void ParseAll_ReturnsListOfInstances()
		{
			string output =
				@"1	1	'one' was used before it was defined.	one
1	1	Expected an assignment or function call and instead saw an expression.	one
1	4	Expected ';' and instead saw 'two'.	one
2	1	'two' was used before it was defined.	two
2	1	Expected an assignment or function call and instead saw an expression.	two
2	4	Expected ';' and instead saw '(end)'.	two
".Replace("\r\n", "\n");

			var parsed = LintWarningList.ParseAll(output);
			Assert.IsNotNull(parsed);
			Assert.IsInstanceOf<IEnumerable<LintWarning>>(parsed);
			Assert.AreEqual(6, parsed.Count());
		}
	}
}
