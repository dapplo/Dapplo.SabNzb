using System.IO;
using Dapplo.HttpExtensions.Support;

namespace Dapplo.SabNzb.Entities
{
	[HttpRequest]
	public class NzbUpload
    {
		[HttpPart(HttpParts.RequestMultipartName, Order = 0)]
		public string OutputPartName { get; } = "output";

		[HttpPart(HttpParts.RequestContent, Order = 0)]
		public string Output { get; } = "json";

		[HttpPart(HttpParts.RequestMultipartName, Order = 1)]
		public string ModePartName { get; } = "mode";

		[HttpPart(HttpParts.RequestContent, Order = 1)]
		public string Mode { get; } = "addfile";

		[HttpPart(HttpParts.RequestMultipartName, Order = 2)]
		public string NzbNamePartName { get; } = "nzbname";

		[HttpPart(HttpParts.RequestContent, Order = 2)]
		public string NzbName { get; set; }

		[HttpPart(HttpParts.RequestMultipartName, Order = 3)]
		public string NzbFilePartName { get; } = "nzbfile";

		[HttpPart(HttpParts.RequestContentType, Order = 3)]
		public string NzbContentType { get; } = "application/x-nzb";

		[HttpPart(HttpParts.RequestMultipartFilename, Order = 3)]
		public string NzbFileName { get; set; }

		[HttpPart(HttpParts.RequestContent, Order = 3)]

		public Stream NzbContent { get; set; }
		[HttpPart(HttpParts.RequestMultipartName, Order = 4)]
		public string ApiKeyPartName { get; } = "apikey";

		[HttpPart(HttpParts.RequestContent, Order = 4)]
		public string ApiKey { get; set; }

	}
}
