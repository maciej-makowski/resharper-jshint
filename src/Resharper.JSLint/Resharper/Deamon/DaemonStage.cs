using System.Collections.Generic;
using JetBrains.Application.Settings;
using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.JavaScript.LanguageImpl;
using JetBrains.ReSharper.Psi.Tree;

namespace JSLintForResharper.Resharper.Deamon
{
	[DaemonStage(StagesBefore = new[] {typeof (LanguageSpecificDaemonStage)})]
	internal class DaemonStage : IDaemonStage
	{
		public ErrorStripeRequest NeedsErrorStripe(IPsiSourceFile sourceFile, IContextBoundSettingsStore settingsStore)
		{
			return ErrorStripeRequest.STRIPE_AND_ERRORS;
		}

		public IEnumerable<IDaemonStageProcess> CreateProcess(IDaemonProcess process, IContextBoundSettingsStore settings, DaemonProcessKind processKind)
		{
			IFile psiFile = process.SourceFile.GetNonInjectedPsiFile(JavaScriptLanguage.Instance);
			if (psiFile == null)
				yield return null;

			yield return new DaemonStageProcess(process);
		}
	}
}
