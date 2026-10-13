using System;

namespace Waylith.Audio;

public interface IAudioVoice
{
	bool IsPlaying { get; }

	long PositionFrames { get; set; }

	void SetVolume(float volume);

	void SetMuted(bool muted);

	void SetPitch(float pitch);

	void SetPriority(int priority);

	void SetPaused(bool paused);

	void SetLoop(bool loop);

	void SetGroup(IAudioGroup? group);

	void SetPan(float pan);

	void SetMixMatrix(ReadOnlySpan<float> matrix, int outChannels, int inChannels);

	void SetSpatial(bool enabled, float minDistance, float maxDistance, AudioRolloffModel rolloff, float level);

	void SetPosition(float x, float y, float z);

	void SetDoppler(float factor);

	void SetSpread(float degrees);

	void ScheduleStart(double delaySeconds);

	bool SetLowPass(bool enabled, float cutoffHz, float q);

	bool SetHighPass(bool enabled, float cutoffHz, float q);

	bool SetEcho(bool enabled, float delayMs, float feedback, float wetMix, float dryMix);

	bool SetReverb(bool enabled, in ReverbParams parameters);

	bool SetChorus(bool enabled, float rateHz, float depth, float mix);

	bool SetDistortion(bool enabled, float level);

	void EnableOutputTap();

	void ReadOutput(Span<float> samples, int channel);

	void Stop();
}
