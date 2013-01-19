using System;
using JSLintForResharper.Utils;
using NUnit.Framework;

namespace JSLintForResharper.Tests.Utils
{
	[TestFixture]
	public class OffsetTranslatorTests
	{
		private const string Tab = "	";
		private const string ThreeByThreeLf = "   \n   \n   ";
		private const string ThreeByThreeCrLf = "   \r\n   \r\n   ";
		private CoordinateToOffsetTranslator offsetTranslator;
		private string Empty = String.Empty;

		[Test]
		public void GetOffset_WhenEmptyString_L1C1_ReturnsZero()
		{
			const int expectedOffset = 0;
			const int line = 1;
			const int col = 1;
			var offset = GetOffset(Empty, line, col);
			Assert.AreEqual(expectedOffset, offset);
		}

		[Test]
		[ExpectedException(typeof(InvalidCoordinateException))]
		public void GetOffset_WhenEmptyString_L2C1_Throws()
		{
			const int line = 2;
			const int col = 1;
			GetOffset(Empty, line, col);
		}

		[Test]
		[ExpectedException(typeof(InvalidCoordinateException))]
		public void GetOffset_WhenEmptyString_L1C2_Throws()
		{
			const int line = 1;
			const int col = 2;
			GetOffset(Empty, line, col);
		}

		[Test]
		[TestCase(1)]
		[TestCase(4)]
		[TestCase(8)]
		public void GetOffset_WhenTab_ForTabSizeN_L1CNplus1_ReturnsOne(int tabSize)
		{
			const int expectedOffset = 1;
			const int line = 1;
			var col = tabSize + 1;
			CreateTranslator(Tab, tabSize, 0);
			var offset = offsetTranslator.GetOffset(line, col);
			Assert.AreEqual(expectedOffset, offset);
		}

		[Test]
		[TestCase(1)]
		[TestCase(4)]
		[TestCase(8)]
		public void GetOffset_WhenMingledTab_ForTabSizeN_ReturnsAdjustedOffset(int tabSize)
		{
			const string input = "\r\n	var a = 1;\r\n";
			//                          ^
			const int expectedOffset = 3;
			const int line = 2;
			var col = tabSize + 1;
			CreateTranslator(input, tabSize, 0);
			var offset = offsetTranslator.GetOffset(line, col);
			Assert.AreEqual(expectedOffset, offset);
		}

		[Test]
		[TestCase(1,1,0)]
		[TestCase(1,2,1)]
		[TestCase(1,3,2)]
		[TestCase(1,4,3)]
		[TestCase(2,1,5)]
		[TestCase(2,2,6)]
		[TestCase(2,3,7)]
		[TestCase(2,4,8)]
		[TestCase(3,1,10)]
		[TestCase(3,2,11)]
		[TestCase(3,3,12)]
		[TestCase(3,4,13)]
		public void GetOffset_WhenThreeByThreeCrLf_ReturnsOffset(int line, int col, int expectedOffset)
		{
			var offset = GetOffset(ThreeByThreeCrLf, line, col);
			Assert.AreEqual(expectedOffset, offset);
		}

		[Test]
		[TestCase(1,1,0)]
		[TestCase(1,2,1)]
		[TestCase(1,3,2)]
		[TestCase(1,4,3)]
		[TestCase(2,1,4)]
		[TestCase(2,2,5)]
		[TestCase(2,3,6)]
		[TestCase(2,4,7)]
		[TestCase(3,1,8)]
		[TestCase(3,2,9)]
		[TestCase(3,3,10)]
		public void GetOffset_WhenThreeByThreeLf_ReturnsOffset(int line, int col, int expectedOffset)
		{
			var offset = GetOffset(ThreeByThreeLf, line, col);
			Assert.AreEqual(expectedOffset, offset);
		}

		[Test]
		[TestCase(2,1,0)]
		[TestCase(2,2,1)]
		[TestCase(2,3,2)]
		[TestCase(2,4,3)]
		[TestCase(3,1,4)]
		[TestCase(3,2,5)]
		[TestCase(3,3,6)]
		[TestCase(3,4,7)]
		[TestCase(4,1,8)]
		[TestCase(4,2,9)]
		[TestCase(4,3,10)]
		public void GetOffset_WhenThreeByThreeLf_RemoveOneLine_Subtracts1FromLineReturnsOffset(int line, int col, int expectedOffset)
		{
			offsetTranslator = new CoordinateToOffsetTranslator(ThreeByThreeLf, 1, 1);
			var offset = offsetTranslator.GetOffset(line, col);
			Assert.AreEqual(expectedOffset, offset);
		}

		[Test]
		[TestCase(1)]
		[TestCase(2)]
		[TestCase(3)]
		[ExpectedException(typeof(InvalidCoordinateException))]
		public void GetOffset_WhenThreeByThreeLf_C5_Throws(int line)
		{
			const int col = 5;
			GetOffset(ThreeByThreeLf, line, col);
		}

		[Test]
		[ExpectedException(typeof(InvalidCoordinateException))]
		public void GetOffset_WhenThreeByThreeLf_L4_Throws()
		{
			const int line = 4;
			const int col = 1;
			GetOffset(ThreeByThreeLf, line, col);
		}

		private int GetOffset(string input, int line, int col)
		{
			CreateTranslator(input);
			return offsetTranslator.GetOffset(line, col);
		}

		private void CreateTranslator(string input)
		{
			CreateTranslator(input, 1, 0);
		}

		private void CreateTranslator(string input, int tabSize, int removeLines)
		{
			offsetTranslator = new CoordinateToOffsetTranslator(input, tabSize, removeLines);
		}
	}
}
