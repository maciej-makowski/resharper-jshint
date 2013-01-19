using System;

namespace JSHintForResharper.UI.Views
{
	public interface ISettingsView
	{
		event EventHandler<SettingChangedEventArgs> SettingChanged;
		event EventHandler OKClicked;

		void SetSetting(string key, object value);
	}
}