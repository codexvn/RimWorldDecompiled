using System.Collections.Generic;
using UnityEngine.Networking;

namespace UnityStubs.Internal;

internal sealed class WebTransferRequest
{
	public string Url;

	public string Method;

	public List<KeyValuePair<string, string>> Headers;

	public byte[] Body;

	public string BodyContentType;

	public int TimeoutSeconds;

	public CertificateHandler CertificateHandler;
}
