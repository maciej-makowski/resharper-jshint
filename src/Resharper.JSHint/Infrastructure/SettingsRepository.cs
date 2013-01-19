using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace JSHintForResharper.Infrastructure
{
	public interface ISettingsRepository
	{
		void Save(Dictionary<string, object> settings);
		Dictionary<string, object> Load();
	}

	public class SettingsRepository : ISettingsRepository
	{
		private static readonly string expectedPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Resharper.JSHint");
		private readonly string path = Path.Combine(expectedPath, "Settings.xml");
		private static readonly XmlSerializer serializer = new XmlSerializer(typeof(List<Setting>));
		
		public void Save(Dictionary<string, object> settings)
		{
			var settingsList = settings.Select(kvp => new Setting{Key = kvp.Key, Value = kvp.Value}).ToList();
			using (var file = File.Create(path)) 
			{
				serializer.Serialize(file, settingsList);
			}
		}

		public Dictionary<string, object> Load()
		{
			if (File.Exists(path))
			{
				using (var file = File.OpenRead(path))
				{
					var keyValuePairs = (List<Setting>) serializer.Deserialize(file);
					return keyValuePairs.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
				}
			}
			return new Dictionary<string, object>();
		}

		public class Setting
		{
			[XmlAttribute]
			public string Key { get; set; }
			public object Value { get; set; }
		}
	}
}