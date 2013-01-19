using System.Windows.Forms;
using JetBrains.ActionManagement;
using JetBrains.Application.DataContext;

namespace JSLintForResharper.Resharper.Actions
{
  [ActionHandler("Resharper.JSLint.About")]
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
        "JSLint for Resharper " + 
		GetType().Assembly.GetName().Version +
		"\nLars-Erik Aabech\n\nAdds JSLint validation to resharper highlighting",
        "About JSLint for Resharper",
        MessageBoxButtons.OK,
        MessageBoxIcon.Information);
    }
  }
}
