using System;

namespace Waylith.VideoDecode.H264;

internal sealed class H264Exception : Exception
{
	public H264Exception(string message)
		: base(message)
	{
	}
}
