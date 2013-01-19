using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;

namespace JSHintForResharper.JSHint
{
	public class ScriptRunner : IDisposable
	{
		private const int Timeout = 5000;
		private StringBuilder errorOuput = new StringBuilder();
		private Process process;
		private AutoResetEvent errorWaitHandle = new AutoResetEvent(false);

		public static LintWarningList Evaluate(string input, string options)
		{
			using (var runner = new ScriptRunner())
			{
			    var saneOptions = SanitizeOptions(options);
				var output = runner.Run(input, saneOptions);
				return LintWarningList.ParseAll(output);
			}
		}


		private ScriptRunner()
		{
		}

        private static string SanitizeOptions(string options)
        {
            return options.Replace("\r\n", "")
                          .Replace(" ", "")
                          .Replace("\t", "")
                          .Replace("\"", "'");
        }

		private string Run(string input, string options)
		{
			Start(options);
			WriteInput(input);
			BeginReading();
			WaitForExit();
			return errorOuput.ToString();
		}

		private void Start(string optionsString)
		{
			var pi = CreateProcessStart(optionsString);
			process = Process.Start(pi);
			process.ErrorDataReceived += AppendErrorData;
		}

		private void WriteInput(string input)
		{
			process.StandardInput.Write(input);
			process.StandardInput.Flush();
			process.StandardInput.Close();
		}

		private void BeginReading()
		{
			process.BeginErrorReadLine();
		}

		private void AppendErrorData(object sender, DataReceivedEventArgs e)
		{
			if (e.Data == null)
				errorWaitHandle.Set();
			else
				errorOuput.AppendLine(e.Data);
		}

		private void WaitForExit()
		{
			errorWaitHandle.WaitOne(Timeout);
			process.WaitForExit(Timeout);
		}

		private static ProcessStartInfo CreateProcessStart(string optionsString)
		{
			return new ProcessStartInfo("cscript.exe", "/nologo \"" + ScriptStorage.LintPath + "\" " + optionsString)
			{
			    CreateNoWindow = true,
			    UseShellExecute = false,
			    RedirectStandardOutput = false,
			    RedirectStandardError = true,
			    RedirectStandardInput = true,
				ErrorDialog = true
			};
		}

		public void Dispose()
		{
			if (process != null)
				process.Dispose();
			if (errorWaitHandle != null)
				((IDisposable)errorWaitHandle).Dispose();
		}
	}
}