using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JSLintForResharper.UI
{
	public class SettingChangedEventArgs : EventArgs
	{
		public string Name { get; private set; }
		public object Value { get; private set; }

		public SettingChangedEventArgs(string name, object value)
		{
			Name = name;
			Value = value;
		}
	}
}
