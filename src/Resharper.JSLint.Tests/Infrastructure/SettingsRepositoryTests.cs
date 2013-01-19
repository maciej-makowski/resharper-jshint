using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using JSLintForResharper.Infrastructure;
using NUnit.Framework;

namespace JSLintForResharper.Tests.Infrastructure
{
	[TestFixture]
	public class SettingsRepositoryTests
	{
		private static readonly string expectedPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Resharper.JSLint");
		private readonly string expectedFilePath = Path.Combine(expectedPath, "Settings.xml");
		private SettingsRepository repository;
		private Dictionary<string, object> settings;

		[SetUp]
		public void SetUp()
		{
			File.Delete(expectedFilePath);
			repository = new SettingsRepository();
			settings = new Dictionary<string, object>
				{
					{ "stupid", (bool?)true },
					{ "indent", (int?)4 } 
				};
		}

		[Test]
		public void Save_WritesXmlFile()
		{
			repository.Save(settings);
			Assert.That(File.Exists(expectedFilePath));
		}

		[Test]
		public void Load_GetsContent()
		{
			var settingsList = settings.Select(kvp => new SettingsRepository.Setting { Key = kvp.Key, Value = kvp.Value }).ToList();
			using (var file = File.Create(expectedFilePath))
			{
				new XmlSerializer(settingsList.GetType()).Serialize(file, settingsList);
			}
			var loadedSettings = repository.Load();
			Assert.That(loadedSettings, Is.EquivalentTo(settings));
		}

		[Test]
		public void Load_WhenNoFile_ReturnsEmptyDictionary()
		{
			var loadedSettings = repository.Load();
			Assert.That(loadedSettings.Count, Is.EqualTo(0));
		}
	}
}
