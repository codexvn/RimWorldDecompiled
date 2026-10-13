using System;
using System.IO;
using System.Numerics;
using Waylith.Assets;

namespace Waylith.Audio;

public abstract class AudioBackend
{
	public const long StreamingFileSizeThresholdBytes = 1048576L;

	public const long TranscodedSizeFactor = 10L;

	public static AudioBackend? Current { get; set; }

	public abstract string Name { get; }

	public virtual bool CanDecodeFsb => false;

	public virtual long StreamingThresholdBytes => 1048576L;

	public virtual long ModStreamingThresholdBytes => 307200L;

	public abstract bool IsInitialized { get; }

	public abstract int SampleRate { get; }

	public abstract int OutputChannels { get; }

	public abstract int DriverChannels { get; }

	public abstract int DspBufferFrames { get; }

	public abstract double DspTime { get; }

	public abstract int PlayingVoices { get; }

	public virtual bool TryGetDecodedSize(string path, out long pcm16Bytes)
	{
		pcm16Bytes = 0L;
		return false;
	}

	public abstract void Initialize(bool noAudio);

	public abstract void Shutdown();

	public abstract void Update();

	public abstract void SetMasterVolume(float volume);

	public abstract void SetOutputPaused(bool paused);

	public abstract void SetListener(Vector3 position, Vector3 forward, Vector3 up);

	public abstract IAudioGroup CreateGroup(string name, IAudioGroup? parent);

	public abstract IAudioSound LoadSound(string path, bool streaming);

	public abstract IAudioSound LoadSoundFromMemory(ReadOnlySpan<byte> data, bool streaming);

	public abstract IAudioSound LoadSoundFromPcm16(ReadOnlySpan<byte> pcm, int channels, int frequency);

	public abstract IAudioSound CreateStreamSound(int channels, int frequency, long lengthFrames, IAudioStreamSource source);

	public abstract IAudioVoice? Play(IAudioSound sound, IAudioGroup? group, bool paused, bool autoRelease);

	public static bool ShouldStream(string path)
	{
		if (TryGetSourceLength(path, out var length, out var transcoded))
		{
			return (transcoded ? (length * 10) : length) >= 1048576;
		}
		return false;
	}

	public static bool TryGetSourceLength(string path, out long length, out bool transcoded)
	{
		transcoded = false;
		if (ContentPack.TryGetFileLength(path, out length))
		{
			ReadOnlySpan<char> extension = Path.GetExtension(path.AsSpan());
			transcoded = MemoryExtensions.Equals(extension, ".ogg".AsSpan(), StringComparison.OrdinalIgnoreCase) || MemoryExtensions.Equals(extension, ".mp3".AsSpan(), StringComparison.OrdinalIgnoreCase);
			return true;
		}
		try
		{
			FileInfo fileInfo = new FileInfo(path);
			length = fileInfo.Length;
			return fileInfo.Exists;
		}
		catch
		{
			length = 0L;
			return false;
		}
	}
}
