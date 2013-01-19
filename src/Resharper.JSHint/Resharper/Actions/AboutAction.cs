using System.Windows.Forms;
using JetBrains.ActionManagement;
using JetBrains.Application.DataContext;

namespace JSHintForResharper.Resharper.Actions
{
  [ActionHandler("Resharper.JSHint.About")]
  public class AboutAction : IActionHandler
  {
    public bool Update(IDataContext context, ActionPresentation presentation, DelegateUpdate nextUpdate)
    {
      // return true or false to enable/disable this action
      return true;
    }

    public void Execute(IDataContext context, DelegateExecute nextExecute)
    {
      MessageBox.Show(
        "JSHint for Resharper " + 
		GetType().Assembly.GetName().Version +
		"\nMaciej Makowski\n\nAdds JSHint validation to resharper highlighting\nbased on JSLint for resharper by Lars-Erik Aabech",
        "About JSHint for Resharper",
        MessageBoxButtons.OK,
        MessageBoxIcon.Information);
    }
  }
}
