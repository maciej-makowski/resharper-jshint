using System;
using System.Diagnostics;
using System.Windows.Forms;
using JSLintForResharper.UI;
using JSLintForResharper.UI.Views;

namespace Resharper.JSLint.UITest
{
	static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main()
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			var settingsForm = new SettingsForm();

			settingsForm.SettingChanged += (sender, args) => Debug.WriteLine(args.Name + ": " + (args.Value ?? "default"));

			settingsForm.SetSetting("browser", true);
			settingsForm.SetSetting("indent", 4);
			settingsForm.SetSetting("R#_priority", true);

			Application.Run(settingsForm);
		}
	}
}
