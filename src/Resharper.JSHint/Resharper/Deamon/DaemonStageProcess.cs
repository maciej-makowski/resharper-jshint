using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using JSHintForResharper.Infrastructure;
using JSHintForResharper.JSHint;
using JSHintForResharper.Resharper.Highlighting;
using JSHintForResharper.Utils;
using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.JavaScript.LanguageImpl;
using JetBrains.ReSharper.Psi.JavaScript.Tree;
using JetBrains.ReSharper.Psi.Tree;

namespace JSHintForResharper.Resharper.Deamon
{
	internal class DaemonStageProcess : IDaemonStageProcess
	{
		private readonly IDaemonProcess daemonProcess;
		private readonly List<HighlightingInfo> highlightings = new List<HighlightingInfo>();
		private IJavaScriptFile mainFile;
		private IJavaScriptTreeNode mainNode;
		private List<HighlightedWarning> warnings;
		private string code;
		private CoordinateToOffsetTranslator offsetTranslator;
		private int tabSize = 4;
		private bool isMixed;
		private int lineOffset;

		public DaemonStageProcess(IDaemonProcess daemonProcess)
		{
			this.daemonProcess = daemonProcess;
		}

		public IDaemonProcess DaemonProcess
		{
			get { return daemonProcess; }
		}

		public void Execute(Action<DaemonStageResult> commiter)
		{
			if (!daemonProcess.FullRehighlightingRequired)
				return;

			var sourceFile = daemonProcess.SourceFile;

			mainFile = sourceFile.GetNonInjectedPsiFile(JavaScriptLanguage.Instance) as IJavaScriptFile;
			highlightings.Clear();

			if (mainFile != null)
			{
				if (sourceFile.PrimaryPsiLanguage is JavaScriptLanguage)
				{
					isMixed = false;
					mainNode = mainFile;
					LintAndAddHighlightings();
				}
				else
				{
					isMixed = true;
					foreach (var subFile in mainFile.Sections)
					{
						mainNode = subFile;
						LintAndAddHighlightings();
					}
				}
			} 
			
			commiter(new DaemonStageResult(highlightings));
		}

        private string SettingsFromRepository()
        {
			var settings = Settings.Values
				.Where(pair => pair.Value != null && !pair.Key.StartsWith("R#_"))
				.Select(pair => "'" + pair.Key + "':'" + pair.Value.ToString().ToLower() + "'")
				.ToArray();
			return "{" + String.Join(",", settings) + "}";
        }

        private string GetSettings()
        {
            var path = Settings.Get<string>("R#_jshint_config");
            if (File.Exists(path))
            {
                return File.ReadAllText(path);
            }
            return SettingsFromRepository();

        }

		private IEnumerable<LintWarning> Lint(string input)
		{
			bool addedWhite = false;
			if (isMixed && !Settings.Values.ContainsKey("white"))
			{
				Settings.Values.Add("white", true);
				addedWhite = true;
			}

		    var options = GetSettings();

			if (isMixed && addedWhite)
				Settings.Values.Remove("white");

			var lintWarnings = ScriptRunner.Evaluate(input, options);
			return lintWarnings;
		}

		private void LintAndAddHighlightings()
		{
			try
			{
				lineOffset = 0;
				var sectionCode = mainNode.GetText();
				code = sectionCode;
				if (isMixed)
					AppendCommentsAndAdjustLineOffset();

				offsetTranslator = new CoordinateToOffsetTranslator(sectionCode, tabSize, lineOffset);
				warnings = Lint(code).Select(CreateHighlightedWarning).ToList();

				foreach (var highlightedWarning in warnings)
				{
					var nodes = mainNode.FindNodesAt(new TreeOffset(highlightedWarning.Offset)).ToArray();

					if (IsWhitespaceWarning(highlightedWarning))
						AddWhitespaceWarning(highlightedWarning, nodes);
					else
						TryAddWarningOnEvidence(highlightedWarning, nodes);

					if (IsNotHightlighted(highlightedWarning, nodes))
						AddNonWhitespaceWarning(highlightedWarning, nodes);
				}
			}
			catch (Exception ex)
			{
				Log(ex);
			}
		}

		private void AppendCommentsAndAdjustLineOffset()
		{
			var comments = mainNode.LeftSiblings().OfType<IJavaScriptCommentNode>()
				.Select(n => n.GetText().Replace("\r", "").Replace("\n", ""))
				.ToArray();
			lineOffset = comments.Length;
			var commentString = String.Join("\n", comments);
			if (lineOffset > 0)
				code = commentString + "\n" + code;
		}

		private void AddNonWhitespaceWarning(HighlightedWarning highlightedWarning, ITreeNode[] nodes)
		{
			var node = nodes.FirstOrDefault(n => !n.IsWhitespaceToken());
			AddHighlighting(highlightedWarning, node);
		}

		private void TryAddWarningOnEvidence(HighlightedWarning highlightedWarning, ITreeNode[] nodes)
		{
			for (var i = 0; i < nodes.Length; i++)
			{
				var node = nodes[i];
				ITreeNode parent = null;
				var evidence = highlightedWarning.Warning.Evidence;
				var nodeText = node.GetText();
				var noMatch = nodeText != evidence;
				if (noMatch && evidence.StartsWith(nodeText))
					parent = node.GetContainingNode<ITreeNode>(n => n.GetText() == evidence);
				if (parent != null)
				{
					node = parent;
					nodeText = node.GetText();
				}
				else
					continue;

				AddHighlighting(highlightedWarning, node);
				break;
			}
		}

		private void AddWhitespaceWarning(HighlightedWarning highlightedWarning, ITreeNode[] nodes)
		{
			var node = nodes.FirstOrDefault(n => n.IsWhitespaceToken());
			if (node != null)
				AddHighlighting(highlightedWarning, node);
		}

		private static bool IsWhitespaceWarning(HighlightedWarning highlightedWarning)
		{
			return highlightedWarning.Warning.Message.ToLower().Contains("space");
		}

		private static bool IsNotHightlighted(HighlightedWarning highlightedWarning, ITreeNode[] nodes)
		{
			return !highlightedWarning.Highlighted && nodes.Any();
		}

		private HighlightedWarning CreateHighlightedWarning(LintWarning warning)
		{
			int offset = GetMessageOffset(warning);
			return new HighlightedWarning(warning, offset);
		}

		private void AddHighlighting(HighlightedWarning warning, ITreeNode node)
		{
			var sameHighlight = highlightings.SingleOrDefault(SameNode(node));
			if (sameHighlight != null)
			{
				((LintHighlightingBase)sameHighlight.Highlighting).AddWarning(warning.Warning);
			}
			else
			{
				var highlightingInfo = new HighlightingInfo(node.GetDocumentRange(), CreateHighlighting(warning, node));
				highlightings.Add(highlightingInfo);
			}
			warning.Highlighted = true;
		}

		private static LintHighlightingBase CreateHighlighting(HighlightedWarning warning, ITreeNode node)
		{
			if (Settings.Get<bool?>("R#_priority") == true)
				return new PrioritizedLintHighlighting(node, warning.Warning);
			return new LintHighlighting(node, warning.Warning);
		}

		private static Func<HighlightingInfo, bool> SameNode(ITreeNode node)
		{
			return h => ((LintHighlightingBase)h.Highlighting).Expression == node;
		}

		private int GetMessageOffset(LintWarning message)
		{
			return offsetTranslator.GetOffset(message.Line, message.Character);
		}

		private void Log(Exception ex)
		{
			Log("", ex);
		}

		private void Log(string extra, Exception ex, params object[] args)
		{
			LogProvider.Instance.Log(String.Format("{0}: {1}, {2}" + extra,
				new object[]{
					DateTime.Now.ToString("yy-MM-dd HH:mm:ss"),
					ex.Message + Environment.NewLine + ex.StackTrace,
					daemonProcess.SourceFile.Name
				}.Union(args).ToArray()
			));
		}
	}

	public class HighlightedWarning
	{
		public LintWarning Warning { get; private set; }
		public int Offset { get; private set; }
		public bool Highlighted { get; set; }

		public HighlightedWarning(LintWarning warning, int offset)
		{
			Warning = warning;
			Offset = offset;
		}
	}
}
