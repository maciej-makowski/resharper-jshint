using System;
using JSHintForResharper.Resharper.Highlighting;
using JetBrains.Application;
using JetBrains.Application.Progress;
using JetBrains.DocumentManagers;
using JetBrains.DocumentModel;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Feature.Services.Bulbs;
using JetBrains.ReSharper.Intentions.Extensibility;
using JetBrains.ReSharper.Intentions.Extensibility.Menu;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp.CodeStyle;
using JetBrains.ReSharper.Psi.ExtensionsAPI.Tree;
using JetBrains.ReSharper.Psi.JavaScript.LanguageImpl;
using JetBrains.ReSharper.Psi.JavaScript.Services;
using JetBrains.ReSharper.Psi.JavaScript.Tree;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.TextControl;
using JetBrains.Util;

namespace JSHintForResharper.Resharper.QuickFixes
{
	[QuickFix]
	public class UseStrictQuickFix : IQuickFix
	{
		private readonly LintHighlightingBase highlighting;

		public UseStrictQuickFix(LintHighlightingBase highlighting)
		{
			this.highlighting = highlighting;
		}

		public void CreateBulbItems(BulbMenu menu, Severity severity)
		{
			var item = new UseStrictBulbItem(highlighting.Expression);
			item.CreateBulbItems(menu);
		}

		public bool IsAvailable(IUserDataHolder cache)
		{
			return highlighting.IsValid() && highlighting.Message.StartsWith("Missing 'use strict'");
		}
	}

	public class UseStrictBulbItem : BulbItemImpl
	{
		private readonly ITreeNode expression;

		public UseStrictBulbItem(ITreeNode expression)
		{
			this.expression = expression;
		}

		protected override Action<ITextControl> ExecutePsiTransaction(ISolution solution, IProgressIndicator progress)
		{
			if (!expression.IsValid())
				return null;

			IFile containingFile = expression.GetContainingFile();

			JavaScriptElementFactory elementFactory = JavaScriptElementFactory.GetInstance(expression.GetPsiModule(), JavaScriptLanguage.Instance);

			IJavaScriptTreeNode newExpression = null;
			Func<JavaScriptElementFactory, IJavaScriptTreeNode> addMethod = AddUseStrict;
			expression.GetPsiServices().PsiManager.DoTransaction(
			  () =>
			  {
				  newExpression = DoTransaction(solution, elementFactory, addMethod);
			  }, GetType().Name);
				
			if (newExpression != null)
			{
				IRangeMarker marker = newExpression.GetDocumentRange().CreateRangeMarker(solution.GetComponent<DocumentManager>());
				containingFile.OptimizeImportsAndRefs(marker, false, true, NullProgressIndicator.Instance);
			}
			return null;
		}

		private static IJavaScriptTreeNode DoTransaction(ISolution solution, JavaScriptElementFactory elementFactory,
		                                                 Func<JavaScriptElementFactory, IJavaScriptTreeNode> addMethod)
		{
			IJavaScriptTreeNode newExpression;
			using (solution.GetComponent<IShellLocks>().UsingWriteLock())
			{
				newExpression = addMethod(elementFactory);
			}
			return newExpression;
		}

		private IJavaScriptTreeNode AddUseStrict(JavaScriptElementFactory elementFactory)
		{
			var useStrict = elementFactory.CreateStatement("\"use strict\";");
			var statement = expression;
			while (!(statement is IJavaScriptStatement) && statement.Parent != null)
				statement = statement.Parent;
			var newExpression = ModificationUtil.AddChildBefore(statement, useStrict);
			return newExpression;
		}

		public override string Text
		{
			get { return "Add use strict"; }
		}
	}
}
