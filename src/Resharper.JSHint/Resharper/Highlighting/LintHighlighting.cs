using System;
using System.Collections.Generic;
using System.Linq;
using JSHintForResharper.JSHint;
using JSHintForResharper.Resharper.Highlighting;
using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Psi.JavaScript.LanguageImpl;
using JetBrains.ReSharper.Psi.Tree;

[assembly: RegisterConfigurableSeverity(LintHighlightingBase.SeverityId,
  null,
  HighlightingGroupIds.CodeSmell,
  "JSHint validation failure",
  "JSHint has found an issue with this code",
  Severity.WARNING,
  false)]

namespace JSHintForResharper.Resharper.Highlighting
{
	public class LintHighlightingBase : IHighlighting
	{
		public const string SeverityId = "JSHint";
		private readonly List<LintWarning> warnings = new List<LintWarning>();

		protected LintHighlightingBase(ITreeNode expression, LintWarning lintWarning)
		{
			this.Expression = expression;
			warnings.Add(lintWarning);
		}

		public ITreeNode Expression { get; private set; }

		public string Message
		{
			get { return String.Join(Environment.NewLine, warnings.Select(w=> w.Message).ToArray()); }
		}

		public string ToolTip
		{
			get { return Message; }
		}

		public string ErrorStripeToolTip
		{
			get { return ToolTip; }
		}

		public int NavigationOffsetPatch
		{
			get { return 0; }
		}

		public bool IsValid()
		{
			return Expression == null || Expression.IsValid();
		}

		public void AddWarning(LintWarning warning)
		{
			warnings.Add(warning);
		}
	}

	[ConfigurableSeverityHighlighting(SeverityId, JavaScriptLanguage.Name, OverlapResolve = OverlapResolveKind.WARNING)]
	public class LintHighlighting : LintHighlightingBase
	{
		public LintHighlighting(ITreeNode expression, LintWarning lintWarning) : base(expression, lintWarning)
		{
		}
	}

	[ConfigurableSeverityHighlighting(SeverityId, JavaScriptLanguage.Name, OverlapResolve = OverlapResolveKind.WARNING, OverloadResolvePriority = int.MaxValue)]
	public class PrioritizedLintHighlighting : LintHighlightingBase
	{
		public PrioritizedLintHighlighting(ITreeNode expression, LintWarning lintWarning)
			: base(expression, lintWarning)
		{
		}
	}
}