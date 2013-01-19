using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using JSHintForResharper.JSHint;
using NUnit.Framework;

namespace JSHintForResharper.Tests.JSHint
{
	[TestFixture]
	public class ScriptRunnerTests
	{
		[Test]
		public void Evaluate_ReturnsLintWarnings()
		{
			var expectedWarnings = LintWarningList.ParseAll(
                    "1\t1\tExpected an assignment or function call and instead saw an expression.\tone\n"+
                    "1\t4\tMissing semicolon.\tone\n"+
                    "2\t1\tExpected an assignment or function call and instead saw an expression.\ttwo\n"+
                    "2\t4\tMissing semicolon.\ttwo"
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

        [Test]
        public void Evaluate_WorksWithJshintConfigFile()
        {
            var currentDirectory = Environment.CurrentDirectory;
            var configPath = currentDirectory + @"\..\..\..\test\data\daemon\javascript\config.json";
            var config = File.ReadAllText(configPath);
            const string input = "one\ntwo";
            var output = ScriptRunner.Evaluate(input, config);
            Assert.That(output, Is.Not.Empty);
            Console.WriteLine(output);
        }
	}
}
