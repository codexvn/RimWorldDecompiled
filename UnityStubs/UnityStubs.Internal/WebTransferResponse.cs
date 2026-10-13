using System.Collections.Generic;

namespace UnityStubs.Internal;

internal sealed class WebTransferResponse
{
	public long StatusCode;

	public string StatusLine;

	public Dictionary<string, string> Headers;

	public byte[] Body;

	public string LocalPath;

	public string ConnectionError;
}
