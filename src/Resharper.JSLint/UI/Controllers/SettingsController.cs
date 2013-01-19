using System;
using System.Collections.Generic;
using JSLintForResharper.Infrastructure;
using JSLintForResharper.UI.Views;

namespace JSLintForResharper.UI.Controllers
{

	public class SettingsController
	{
		private readonly ISettingsRepository repository;
		private readonly ISettingsView view;

		public Dictionary<string, object> Settings;

		public SettingsController(ISettingsRepository repository, ISettingsView view)
		{
			this.repository = repository;
			this.view = view;

			view.SettingChanged += ViewOnSettingChanged;
			view.OKClicked += ViewOnOKClicked;
		}

		public void Initialize()
		{
			Settings = repository.Load();
			foreach (var pair in Settings)
				view.SetSetting(pair.Key, pair.Value);
		}

		private void ViewOnSettingChanged(object sender, SettingChangedEventArgs e)
		{
			if (Settings.ContainsKey(e.Name))
				Settings.Remove(e.Name);
			if (e.Value != null)
				Settings.Add(e.Name, e.Value);
		}

		private void ViewOnOKClicked(object sender, EventArgs eventArgs)
		{
			repository.Save(Settings);
		}
	}
}
