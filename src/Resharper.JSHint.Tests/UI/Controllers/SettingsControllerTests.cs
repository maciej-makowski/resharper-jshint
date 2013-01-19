using System;
using System.Collections.Generic;
using JSHintForResharper.Infrastructure;
using JSHintForResharper.UI;
using JSHintForResharper.UI.Controllers;
using JSHintForResharper.UI.Views;
using NUnit.Framework;
using Rhino.Mocks;

namespace JSHintForResharper.Tests.UI.Controllers
{
	[TestFixture]
	public class SettingsControllerTests
	{
		private const string StupidKey = "stupid";
		private ISettingsRepository repository;
		private ISettingsView view;
		private SettingsController controller;

		[SetUp]
		public void SetUp()
		{
			repository = MockRepository.GenerateMock<ISettingsRepository>();
			view = MockRepository.GenerateMock<ISettingsView>();
			controller = new SettingsController(repository, view);
		}

		[Test]
		public void Initialize_SetsSettingsOnView()
		{
			repository.Stub(r => r.Load()).Return(new Dictionary<string, object> {{StupidKey, (bool?) true}, {"indent", (int?) 4}});
			view.Expect(v => v.SetSetting(StupidKey, (bool?) true));
			view.Expect(v => v.SetSetting("indent", (int?) 4));

			controller.Initialize();

			view.VerifyAllExpectations();
		}

		[Test]
		public void View_SettingsChanged_PreviouslyNull_AddsSetting()
		{
			controller.Settings = new Dictionary<string, object>();
			view.Raise(v => v.SettingChanged += null, view, new SettingChangedEventArgs(StupidKey, (bool?)true));
			Assert.AreEqual((bool?)true, controller.Settings[StupidKey]);
		}

		[Test]
		public void View_SettingsChanged_PreviouslyNullToNull_DoesNothing()
		{
			controller.Settings = new Dictionary<string, object>();
			view.Raise(v => v.SettingChanged += null, view, new SettingChangedEventArgs(StupidKey, (bool?)null));
			Assert.AreEqual(0, controller.Settings.Count);
		}

		[Test]
		public void View_SettingsChanged_Existing_ModifiesSetting()
		{
			controller.Settings = new Dictionary<string, object>{{StupidKey, (bool?)false}};
			view.Raise(v => v.SettingChanged += null, view, new SettingChangedEventArgs(StupidKey, (bool?)true));
			Assert.AreEqual((bool?)true, controller.Settings[StupidKey]);
		}

		[Test]
		public void View_SettingsChanged_ToNull_RemovesSetting()
		{
			controller.Settings = new Dictionary<string, object>{{StupidKey, (bool?)false}};
			view.Raise(v => v.SettingChanged += null, view, new SettingChangedEventArgs(StupidKey, (bool?)null));
			Assert.IsFalse(controller.Settings.ContainsKey(StupidKey));
		}

		[Test]
		public void View_OkClicked_SavesSettings()
		{
			controller.Settings = new Dictionary<string, object>();
			repository.Expect(r => r.Save(controller.Settings));
			view.Raise(v => v.OKClicked += null, view, EventArgs.Empty);
			repository.VerifyAllExpectations();
		}
	}

}
