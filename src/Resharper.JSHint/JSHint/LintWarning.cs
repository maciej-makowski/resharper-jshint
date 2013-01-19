using System;
using System.Collections.Generic;
using System.Linq;

namespace JSHintForResharper.JSHint
{
	public class LintWarning : IEquatable<LintWarning>
	{
		private const string ToStringFormat = "Line {0} Char {1}: {2} ({3})";

		public int Line { get; private set; }
		public int Character { get; private set; }
		public string Message { get; private set; }
		public string Evidence { get; private set; }

		private LintWarning(int line, int character, string message, string evidence)
		{
			Line = line;
			Character = character;
			Message = message;
			Evidence = evidence;
		}

		public static LintWarning Parse(string resultLine)
		{
			var parts = resultLine.Split('\t');
			if (parts.Length != 4)
				return null;
			try
			{
				return new LintWarning(
					Convert.ToInt32(parts[0]), 
					Convert.ToInt32(parts[1]),
					parts[2], 
					parts[3]);
			}
			catch(FormatException)
			{
				return null;
			}
		}

		public override string ToString()
		{
			return String.Format(ToStringFormat, Line, Character, Message, Evidence);
		}

		#region Equality

		public bool Equals(LintWarning other)
		{
			if (ReferenceEquals(null, other)) return false;
			if (ReferenceEquals(this, other)) return true;
			return other.Line == Line && other.Character == Character && Equals(other.Message, Message) && Equals(other.Evidence, Evidence);
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			if (ReferenceEquals(this, obj)) return true;
			if (obj.GetType() != typeof (LintWarning)) return false;
			return Equals((LintWarning) obj);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				int result = Line;
				result = (result*397) ^ Character;
				result = (result*397) ^ (Message != null ? Message.GetHashCode() : 0);
				result = (result*397) ^ (Evidence != null ? Evidence.GetHashCode() : 0);
				return result;
			}
		}

		public static bool operator ==(LintWarning left, LintWarning right)
		{
			return Equals(left, right);
		}

		public static bool operator !=(LintWarning left, LintWarning right)
		{
			return !Equals(left, right);
		}

		#endregion
	}
}