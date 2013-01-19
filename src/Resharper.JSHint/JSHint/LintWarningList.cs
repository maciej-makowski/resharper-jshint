using System;
using System.Collections.Generic;
using System.Linq;

namespace JSHintForResharper.JSHint
{
	public class LintWarningList : List<LintWarning>
	{
		public LintWarningList()
		{
		}

		public LintWarningList(IEnumerable<LintWarning> initial)
			: base(initial)
		{
		}

		public static LintWarningList ParseAll(string output)
		{
			return new LintWarningList(
				SplitWarnings(output)
				.Select(LintWarning.Parse)
				.Where(NotNull)
			);
		}

		private static IEnumerable<string> SplitWarnings(string output)
		{
			return output.Replace("\r", "").Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
		}

		private static bool NotNull(LintWarning warning)
		{
			return warning != null;
		}

		public override string ToString()
		{
			return String.Join(Environment.NewLine, this.Select(warning => warning.ToString()).ToArray());
		}
	}
}
