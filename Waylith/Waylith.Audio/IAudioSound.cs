using System;

namespace Waylith.Audio;

public interface IAudioSound
{
	long LengthFrames { get; }

	int Channels { get; }

	int Frequency { get; }

	bool IsStream { get; }

	bool InstancePerVoice { get; }

	bool TryReadPcm(Span<float> samples, long offsetFrames);

	bool TryWritePcm(ReadOnlySpan<float> samples, long offsetFrames);

	void Release();
}
