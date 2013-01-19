using System.Windows.Forms;
using JSHintForResharper.Infrastructure;
using JSHintForResharper.UI.Controllers;
using JSHintForResharper.UI.Views;
using JetBrains.ActionManagement;
using JetBrains.Application.DataContext;

namespace JSHintForResharper.Resharper.Actions
{
	[ActionHandler("Resharper.JSHint.Settings")]
	public class SettingsAction : IActionHandler
	{
		public bool Update(IDataContext context, ActionPresentation presentation, DelegateUpdate nextUpdate)
		{
			return true;
		}

		public void Execute(IDataContext context, DelegateExecute nextExecute)
		{
			using (var form = new SettingsForm())
			{
				var repository = new SettingsRepository();
				var presenter = new SettingsController(repository, form);
				presenter.Initialize();
				if (form.ShowDialog() == DialogResult.OK)
					Settings.Values = presenter.Settings;
			}
		}
	}
}
