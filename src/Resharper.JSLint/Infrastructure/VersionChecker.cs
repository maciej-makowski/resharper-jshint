using System;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;
using JetBrains.Annotations;

namespace JSLintForResharper.Infrastructure
{
	public class VersionChecker
	{
		[NotNull]
		public static string GetLatestReleaseNumber()
		{
			const string uri = "http://resharperjslint.codeplex.com/project/feeds/rss?ProjectRSSFeed=codeplex%3a%2f%2frelease%2fresharperjslint";
			var latestVersion = "";
			var request = WebRequest.Create(uri);
			request.Timeout = TimeSpan.FromSeconds(5).Milliseconds;
			var responseStream = request.GetResponse().GetResponseStream();
			if (responseStream == null) return latestVersion;
			var doc = XDocument.Load(XmlReader.Create(responseStream));
			var item = doc.Descendants("item").OrderByDescending(i => DateTime.Parse((string) i.Element("pubDate"))).First();
			var title = (string) item.Element("title");
			var versionRegEx = new Regex(@"\d+\.\d+\.\d+\.?\d?");
			var match = versionRegEx.Match(title);
			if (match.Success)
				latestVersion = match.Value;
			return latestVersion;
		}


		public static bool IsOutdated()
		{
			var latest = GetLatestReleaseNumber();
			return IsOutdated(latest);
		}

		public static bool IsOutdated(string releasedVersion)
		{
			var version = typeof(VersionChecker).Assembly.GetName().Version;
			var split = releasedVersion.Split('.');
			if (split.Length < 3) return false;
			int major, minor, build, revision = 0;
			int.TryParse(split[0], out major);
			int.TryParse(split[1], out minor);
			int.TryParse(split[2], out build);
			if (split.Length > 3)
				int.TryParse(split[3], out revision);
			return major > version.Major ||
				   minor > version.Minor ||
				   build > version.Build ||
				   revision > version.Revision;
		}
	}
}
