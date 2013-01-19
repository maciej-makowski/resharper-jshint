using System;

namespace JSLintForResharper.Utils
{
	public class CoordinateToOffsetTranslator
	{
		private const char CR = '\r';
		private const char LF = '\n';
		private const char TAB = '\t';

		private readonly string input;
		private readonly int tabSize;
		private readonly int removeLines;

		private int offsetLine;
		private int offsetColumn;
		private int offset;
		private int line;
		private int column;
		private char current;

		public CoordinateToOffsetTranslator(string input)
		{
			this.input = input;
			tabSize = 1;
		}

		public CoordinateToOffsetTranslator(string input, int tabSize, int removeLines)
		{
			this.input = input;
			this.tabSize = tabSize;
			this.removeLines = removeLines;
		}

		public int GetOffset(int lineNumber, int columnNumber)
		{
			Initialize(lineNumber, columnNumber);

			if (IsAtCoords())
				return 0;

			for (offset = 0; offset < input.Length; offset++)
			{
				MoveNext();

				if (ShouldIgnore())
					continue;

				IncreaseCoords();

				if (IsAtCoords())
					return GetOffset();
			}

			throw new InvalidCoordinateException();
		}

		private void Initialize(int lineNumber, int columnNumber)
		{
			line = lineNumber;
			column = columnNumber;
			offsetLine = 1 + removeLines;
			offsetColumn = 1;
		}

		private bool IsAtCoords()
		{
			return offsetLine == line && offsetColumn == column;
		}

		private void MoveNext()
		{
			current = input[offset];
		}

		private int GetOffset()
		{
			return offset + 1;
		}

		private void IncreaseCoords()
		{
			if (IsNewLine())
				IncreaseLine();
			else
				IncreaseColumn();
		}

		private void IncreaseColumn()
		{
			if (IsTab())
				offsetColumn += tabSize;
			else
				offsetColumn += 1;
		}

		private void IncreaseLine()
		{
			offsetLine++;
			offsetColumn = 1;
		}

		private bool ShouldIgnore()
		{
			return current == CR;
		}

		private bool IsNewLine()
		{
			return current == LF;
		}

		private bool IsTab()
		{
			return current == TAB;
		}
	}

	public class InvalidCoordinateException : Exception
	{
	}
}