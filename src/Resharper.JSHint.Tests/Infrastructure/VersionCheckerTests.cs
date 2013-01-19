using JSHintForResharper.Infrastructure;
using NUnit.Framework;

namespace JSHintForResharper.Tests.Infrastructure
{
	[TestFixture]
	[Ignore]
	public class VersionCheckerTests
	{
		[Test]
		public void GetLatestReleaseNumber_ReturnsLatestVersionFromRssFeed()
		{
			var latestVersion = VersionChecker.GetLatestReleaseNumber();
			Assert.AreEqual("0.0.3", latestVersion);
		}

		[Test]
		public void IsOutdated_ReturnsFalse_ForNow()
		{
			// TODO: This has got to be true in the future :P
			Assert.IsFalse(VersionChecker.IsOutdated());
		}

		[Test]
		[TestCase("0.0.1", false)]
		[TestCase("0.0.2", false)]
		[TestCase("0.0.3", false)]
		[TestCase("0.0.4", false)]
		[TestCase("0.1.0", false)]
		[TestCase("0.1.1", false)]
		[TestCase("0.1.5000", true)]
		[TestCase("0.2.0", true)]
		[TestCase("0.10.0", true)]
		[TestCase("1.0.0", true)]
		public void IsOutdated_ForVersionNumber_ReturnsComparisonToBuildVersion(string releasedVersion, bool isOutdated)
		{
			Assert.AreEqual(isOutdated, VersionChecker.IsOutdated(releasedVersion));
		}
	}
}
