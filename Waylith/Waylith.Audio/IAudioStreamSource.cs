using System;

namespace Waylith.Audio;

public interface IAudioStreamSource
{
	int Read(Span<float> interleaved);

	bool Seek(long frame);
}
