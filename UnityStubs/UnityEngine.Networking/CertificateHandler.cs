using System;

namespace UnityEngine.Networking;

public class CertificateHandler : IDisposable
{
	public void Dispose()
	{
	}

	protected virtual bool ValidateCertificate(byte[] certificateData)
	{
		return false;
	}

	internal bool Validate(byte[] certificateData)
	{
		return ValidateCertificate(certificateData);
	}
}
