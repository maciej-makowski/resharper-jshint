using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace JSLintForResharper.UI.Views
{
	public partial class SettingsForm : Form, ISettingsView
	{
		private static readonly Dictionary<CheckState, bool?> states = new Dictionary<CheckState, bool?>
		{
			{ CheckState.Checked, true },
			{ CheckState.Unchecked, false},
			{ CheckState.Indeterminate, null }
		};
		
		public event EventHandler<SettingChangedEventArgs> SettingChanged;
		public event EventHandler OKClicked;

		private bool inSetSetting = false;

		public SettingsForm()
		{
			InitializeComponent();

			foreach(GroupBox group in Controls.OfType<GroupBox>())
				AssignCheckedStateChanged(group);
		}

		public void SetSetting(string key, object value)
		{
			inSetSetting = true;
			foreach(GroupBox group in Controls.OfType<GroupBox>())
			{
				Control control = @group.Controls.Cast<Control>().FirstOrDefault(c => c.Tag != null && c.Tag.Equals(key));
				if (control != null)
				{
					var checkBox = control as CheckBox;
					if (checkBox != null)
						checkBox.CheckState = GetCheckState(value);

					var textBox = control as TextBox;
					if (textBox != null)
						textBox.Text = value.ToString();

					break;
				}
			}
			inSetSetting = false;
		}

		private void AssignCheckedStateChanged(GroupBox groupBox)
		{
			foreach (CheckBox cb in groupBox.Controls.OfType<CheckBox>())
				cb.CheckStateChanged += CheckStateChanged;
			foreach (TextBox cb in groupBox.Controls.OfType<TextBox>())
				cb.TextChanged += TextSettingChanged;
		}

		private void TextSettingChanged(object sender, EventArgs e)
		{
			if (inSetSetting) return;
			if (SettingChanged != null)
			{
				var textBox = (TextBox) sender;
				SettingChanged(this, new SettingChangedEventArgs((string) textBox.Tag, textBox.Text));
			}
		}

		private void CheckStateChanged(object sender, EventArgs e)
		{
			if (inSetSetting) return;
			if (SettingChanged != null)
			{
				var checkBox = (CheckBox) sender;
				SettingChanged(this, new SettingChangedEventArgs((string)checkBox.Tag, GetSettingValue(checkBox)));
			}
		}

		private static bool? GetSettingValue(CheckBox checkBox)
		{
			return states[checkBox.CheckState];
		}

		private static CheckState GetCheckState(object value)
		{
			return states.Single(kvp => kvp.Value == (bool?) value).Key;
		}

		private void OkButton_Click(object sender, EventArgs e)
		{
			if (OKClicked != null)
				OKClicked(this, EventArgs.Empty);
		}

		private void CancelButton_Click(object sender, EventArgs e)
		{
		}
	}
}
