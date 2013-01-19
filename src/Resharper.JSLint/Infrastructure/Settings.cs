using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JSLintForResharper.Infrastructure
{
	public static class Settings
	{
		private static Dictionary<string, object> values;

		public static Dictionary<string, object> Values
		{
			get
			{
				EnsureSettings();
				return values;
			}
			set { values = value; }
		}

		public static T Get<T>(string key)
		{
			if (Values.ContainsKey(key) && Values[key] is T)
				return (T)Values[key];
			return default(T);
		}

		private static void EnsureSettings()
		{
			if (values != null) return;
			var repo = new SettingsRepository();
			values = repo.Load();

			if (!values.ContainsKey("indent"))
				values.Add("indent", 4);
		}
	}
}
