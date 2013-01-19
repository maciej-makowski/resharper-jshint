using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using JSLintForResharper.JSLint;
using NUnit.Framework;

namespace JSLintForResharper.Tests.JSLint
{
	[TestFixture]
	public class ScriptRunnerTests
	{
		[Test]
		public void Evaluate_ReturnsLintWarnings()
		{
			var expectedWarnings = LintWarningList.ParseAll(
@"1	1	'one' was used before it was defined.	one
1	1	Expected an assignment or function call and instead saw an expression.	one
1	4	Expected ';' and instead saw 'two'.	one
2	1	'two' was used before it was defined.	two
2	1	Expected an assignment or function call and instead saw an expression.	two
2	4	Expected ';' and instead saw '(end)'.	two
"
			);

			const string input = "one\ntwo";
			const string options = "{'indent':'1'}";
			var output = ScriptRunner.Evaluate(input, options);
			Assert.That(expectedWarnings.SequenceEqual(output), expectedWarnings + "---" + output);
		}

		[Test]
		public void Run_DoesntFreezeOnLargeScript()
		{
			var currentDirectory = Environment.CurrentDirectory;
			var scriptPath = currentDirectory + @"\..\..\..\test\data\daemon\javascript\jquery-1.7.2.js";
			var script = File.ReadAllText(scriptPath);
			const string options = "{'indent':'4'}";
			var output = ScriptRunner.Evaluate(script, options);
			Assert.That(output, Is.Not.Empty);
			Console.WriteLine(output);
		}
	}
}
