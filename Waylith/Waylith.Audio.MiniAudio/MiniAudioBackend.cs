using System;
using System.IO;
using System.Numerics;
using System.Runtime.InteropServices;
using Waylith.Assets;
using Waylith.Runtime;

namespace Waylith.Audio.MiniAudio;

public sealed class MiniAudioBackend : AudioBackend
{
	private sealed class StreamBox(IAudioStreamSource source, int channels)
	{
		public readonly IAudioStreamSource Source = source;

		public readonly int Channels = channels;
	}

	private sealed class Group(ulong handle) : IAudioGroup
	{
		public readonly ulong Handle = handle;

		public float Volume
		{
			set
			{
				WaylithAudioNative.wa_group_set_volume(handle, value);
			}
		}

		public bool Muted
		{
			set
			{
				WaylithAudioNative.wa_group_set_mute(handle, value ? 1 : 0);
			}
		}

		public bool Paused
		{
			set
			{
				WaylithAudioNative.wa_group_set_paused(handle, value ? 1 : 0);
			}
		}

		public float Pitch
		{
			set
			{
				WaylithAudioNative.wa_group_set_pitch(handle, value);
			}
		}
	}

	private sealed class Sound : IAudioSound
	{
		public readonly ulong Handle;

		public GCHandle StreamHandle;

		public long LengthFrames { get; }

		public int Channels { get; }

		public int Frequency { get; }

		public bool IsStream { get; }

		public long ResidentBytes => (long)WaylithAudioNative.wa_sound_resident_bytes(Handle);

		public bool InstancePerVoice => false;

		public Sound(ulong handle, bool streaming)
		{
			Handle = handle;
			IsStream = streaming;
			LengthFrames = (long)WaylithAudioNative.wa_sound_length_frames(handle);
			Channels = (int)WaylithAudioNative.wa_sound_channels(handle);
			Frequency = (int)WaylithAudioNative.wa_sound_rate(handle);
		}

		public bool TryReadPcm(Span<float> samples, long offsetFrames)
		{
			return Transfer(samples, default(ReadOnlySpan<float>), offsetFrames, isWrite: false);
		}

		public bool TryWritePcm(ReadOnlySpan<float> samples, long offsetFrames)
		{
			return Transfer(default(Span<float>), samples, offsetFrames, isWrite: true);
		}

		private unsafe bool Transfer(Span<float> read, ReadOnlySpan<float> write, long offsetFrames, bool isWrite)
		{
			short* ptr = WaylithAudioNative.wa_sound_pcm(Handle);
			if (ptr == null || LengthFrames == 0L)
			{
				return false;
			}
			long num = LengthFrames * Channels;
			long num2 = offsetFrames * Channels;
			int num3 = (isWrite ? write.Length : read.Length);
			for (int i = 0; i < num3; i++)
			{
				long num4 = (num2 + i) % num;
				if (isWrite)
				{
					ptr[num4] = (short)System.Math.Clamp(MathF.Round(write[i] * 32767f), -32768f, 32767f);
				}
				else
				{
					read[i] = (float)ptr[num4] / 32768f;
				}
			}
			return true;
		}

		public void Release()
		{
			WaylithAudioNative.wa_sound_release(Handle);
			if (StreamHandle.IsAllocated)
			{
				StreamHandle.Free();
			}
		}
	}

	private sealed class Voice : IAudioVoice
	{
		private readonly ulong handle = handle;

		private bool tap;

		public bool IsPlaying => WaylithAudioNative.wa_voice_is_playing(handle) != 0;

		public long PositionFrames
		{
			get
			{
				return (long)WaylithAudioNative.wa_voice_get_position_frames(handle);
			}
			set
			{
				WaylithAudioNative.wa_voice_set_position_frames(handle, (ulong)System.Math.Max(0L, value));
			}
		}

		public Voice(ulong handle, MiniAudioBackend backend)
		{
		}

		public void SetVolume(float volume)
		{
			WaylithAudioNative.wa_voice_set_volume(handle, volume);
		}

		public void SetMuted(bool muted)
		{
			WaylithAudioNative.wa_voice_set_mute(handle, muted ? 1 : 0);
		}

		public void SetPitch(float pitch)
		{
			WaylithAudioNative.wa_voice_set_pitch(handle, pitch);
		}

		public void SetPriority(int priority)
		{
			WaylithAudioNative.wa_voice_set_priority(handle, priority);
		}

		public void SetPaused(bool paused)
		{
			WaylithAudioNative.wa_voice_set_paused(handle, paused ? 1 : 0);
		}

		public void SetLoop(bool loop)
		{
			WaylithAudioNative.wa_voice_set_loop(handle, loop ? 1 : 0);
		}

		public void SetGroup(IAudioGroup? group)
		{
			WaylithAudioNative.wa_voice_set_group(handle, (group is Group group2) ? group2.handle : 0);
		}

		public void SetPan(float pan)
		{
			WaylithAudioNative.wa_voice_set_pan(handle, pan);
		}

		public unsafe void SetMixMatrix(ReadOnlySpan<float> matrix, int outChannels, int inChannels)
		{
			fixed (float* matrix2 = matrix)
			{
				WaylithAudioNative.wa_voice_set_mix_matrix(handle, matrix2, (uint)outChannels, (uint)inChannels);
			}
		}

		public void SetSpatial(bool enabled, float minDistance, float maxDistance, AudioRolloffModel rolloff, float level)
		{
			WaylithAudioNative.wa_voice_set_3d(handle, (enabled && level > 0f) ? 1 : 0, minDistance, maxDistance, (rolloff == AudioRolloffModel.Linear) ? 1 : 0);
		}

		public void SetPosition(float x, float y, float z)
		{
			WaylithAudioNative.wa_voice_set_3d_position(handle, 0f - x, y, z);
		}

		public void SetDoppler(float factor)
		{
			WaylithAudioNative.wa_voice_set_doppler(handle, factor);
		}

		public void SetSpread(float degrees)
		{
		}

		public void ScheduleStart(double delaySeconds)
		{
			if (delaySeconds > 0.0)
			{
				WaylithAudioNative.wa_voice_set_start_time(handle, WaylithAudioNative.wa_time_frames() + (ulong)(delaySeconds * (double)backend.sampleRate));
			}
		}

		private static bool Ok(int result)
		{
			if (result == -3 || result == 0)
			{
				return true;
			}
			return false;
		}

		public bool SetLowPass(bool enabled, float cutoffHz, float q)
		{
			return Ok(WaylithAudioNative.wa_voice_set_lowpass(handle, enabled ? 1 : 0, cutoffHz, q));
		}

		public bool SetHighPass(bool enabled, float cutoffHz, float q)
		{
			return Ok(WaylithAudioNative.wa_voice_set_highpass(handle, enabled ? 1 : 0, cutoffHz, q));
		}

		public bool SetEcho(bool enabled, float delayMs, float feedback, float wetMix, float dryMix)
		{
			return Ok(WaylithAudioNative.wa_voice_set_echo(handle, enabled ? 1 : 0, delayMs, feedback, wetMix, dryMix));
		}

		public bool SetReverb(bool enabled, in ReverbParams p)
		{
			if (!enabled)
			{
				return Ok(WaylithAudioNative.wa_voice_set_reverb(handle, 0, 0f, 0f, 0f, 1f, 1f, 0f));
			}
			float roomsize = System.Math.Clamp((MathF.Pow(0.001f, 0.0317f / System.Math.Max(0.05f, p.decayTimeSeconds)) - 0.7f) / 0.28f, 0f, 0.98f);
			float damp = System.Math.Clamp(1f - p.decayHfRatio, 0f, 1f);
			float wet = MathF.Pow(10f, System.Math.Clamp(p.roomMb + p.reverbLevelMb, -10000f, 2000f) / 2000f) / 3f;
			float dry = MathF.Pow(10f, System.Math.Clamp(p.dryLevelMb, -10000f, 2000f) / 2000f);
			float width = System.Math.Clamp(p.diffusionPercent / 100f, 0f, 1f);
			float num = MathF.Pow(10f, System.Math.Clamp(p.roomHfMb, -10000f, 0f) / 2000f);
			float highCutHz = ((num >= 1f) ? 0f : System.Math.Clamp(System.Math.Clamp(p.hfReferenceHz, 20f, 20000f) * MathF.Sqrt(num / (1f - num)), 20f, 20000f));
			return Ok(WaylithAudioNative.wa_voice_set_reverb(handle, 1, roomsize, damp, wet, dry, width, highCutHz));
		}

		public bool SetChorus(bool enabled, float rateHz, float depth, float mix)
		{
			return false;
		}

		public bool SetDistortion(bool enabled, float level)
		{
			return false;
		}

		public void EnableOutputTap()
		{
			if (!tap)
			{
				tap = WaylithAudioNative.wa_voice_tap_enable(handle, 16384u) == 0;
			}
		}

		public unsafe void ReadOutput(Span<float> samples, int channel)
		{
			if (!tap || channel >= backend.channels)
			{
				samples.Clear();
				return;
			}
			fixed (float* output = samples)
			{
				if (WaylithAudioNative.wa_voice_tap_read(handle, output, (uint)samples.Length, (uint)channel) != 0)
				{
					samples.Clear();
				}
			}
		}

		public void Stop()
		{
			WaylithAudioNative.wa_voice_stop(handle);
		}

		bool IAudioVoice.SetReverb(bool enabled, in ReverbParams parameters)
		{
			return SetReverb(enabled, in parameters);
		}
	}

	private const int TapCapacityFrames = 16384;

	private bool initialized;

	private int sampleRate;

	private int channels;

	public override string Name => "miniaudio";

	public override long StreamingThresholdBytes => 2097152L;

	public override long ModStreamingThresholdBytes => 2097152L;

	public override bool IsInitialized => initialized;

	public override int SampleRate => sampleRate;

	public override int OutputChannels => channels;

	public override int DriverChannels => channels;

	public override int DspBufferFrames
	{
		get
		{
			if (!initialized)
			{
				return 0;
			}
			return (int)WaylithAudioNative.wa_period_frames();
		}
	}

	public override double DspTime
	{
		get
		{
			if (!initialized)
			{
				return 0.0;
			}
			return (double)WaylithAudioNative.wa_time_frames() / (double)sampleRate;
		}
	}

	public override int PlayingVoices
	{
		get
		{
			if (!initialized)
			{
				return 0;
			}
			return WaylithAudioNative.wa_playing_count();
		}
	}

	public unsafe override void Initialize(bool noAudio)
	{
		if (!initialized)
		{
			WaylithAudioNative.wa_set_allocator((delegate* unmanaged<nuint, void*>)(delegate*<nuint, void*>)(&HostAlloc), (delegate* unmanaged<void*, nuint, void*>)(delegate*<void*, nuint, void*>)(&HostRealloc), (delegate* unmanaged<void*, void>)(delegate*<void*, void>)(&HostFree));
			if (WaylithAudioNative.wa_init(48000u, 2u, noAudio ? 1 : 0) != 0)
			{
				throw new InvalidOperationException($"miniaudio failed to initialize ({WaylithAudioNative.wa_last_error()})");
			}
			sampleRate = (int)WaylithAudioNative.wa_sample_rate();
			channels = (int)WaylithAudioNative.wa_channels();
			initialized = true;
		}
	}

	public override void Shutdown()
	{
		if (initialized)
		{
			WaylithAudioNative.wa_shutdown();
			initialized = false;
		}
	}

	public override void Update()
	{
		if (initialized)
		{
			WaylithAudioNative.wa_update();
		}
	}

	public override void SetMasterVolume(float volume)
	{
		if (initialized)
		{
			WaylithAudioNative.wa_set_master_volume(volume);
		}
	}

	public override void SetOutputPaused(bool paused)
	{
		if (initialized)
		{
			WaylithAudioNative.wa_set_master_paused(paused ? 1 : 0);
		}
	}

	public override void SetListener(Vector3 position, Vector3 forward, Vector3 up)
	{
		if (initialized)
		{
			WaylithAudioNative.wa_set_listener(0f - position.X, position.Y, position.Z, 0f - forward.X, forward.Y, forward.Z, 0f - up.X, up.Y, up.Z);
		}
	}

	public override IAudioGroup CreateGroup(string name, IAudioGroup? parent)
	{
		ulong num = WaylithAudioNative.wa_group_create((parent is Group group) ? group.handle : 0);
		if (num == 0L)
		{
			throw new InvalidOperationException("miniaudio could not create group '" + name + "'");
		}
		return new Group(num);
	}

	public unsafe override IAudioSound LoadSound(string path, bool streaming)
	{
		if (ContentPack.TryGetStoredFile(path, out var pointer, out var length))
		{
			return LoadSound((void*)pointer, length, streaming, mappedMemory: true);
		}
		if (ContentPack.TryReadFile(path, out byte[] data))
		{
			return LoadSoundFromMemory(data, streaming);
		}
		if (streaming)
		{
			ulong num = WaylithAudioNative.wa_sound_load_file_stream(path);
			if (num != 0L)
			{
				return new Sound(num, streaming: true);
			}
		}
		return LoadSoundFromMemory(File.ReadAllBytes(path), streaming);
	}

	public unsafe override bool TryGetDecodedSize(string path, out long pcm16Bytes)
	{
		pcm16Bytes = 0L;
		if (!initialized)
		{
			return false;
		}
		int num2;
		ulong frames;
		uint num;
		uint rate;
		byte[] data;
		if (ContentPack.TryGetStoredFile(path, out var pointer, out var length))
		{
			num2 = WaylithAudioNative.wa_probe_memory((void*)pointer, (nuint)length, out frames, out num, out rate);
		}
		else if (ContentPack.TryReadFile(path, out data))
		{
			fixed (byte* data2 = data)
			{
				num2 = WaylithAudioNative.wa_probe_memory(data2, (nuint)data.Length, out frames, out num, out rate);
			}
		}
		else
		{
			if (!File.Exists(path))
			{
				return false;
			}
			num2 = WaylithAudioNative.wa_probe_file(path, out frames, out num, out rate);
		}
		if (num2 != 0 || frames == 0L)
		{
			return false;
		}
		pcm16Bytes = (long)(frames * num * 2);
		return true;
	}

	public unsafe override IAudioSound LoadSoundFromMemory(ReadOnlySpan<byte> data, bool streaming)
	{
		fixed (byte* data2 = data)
		{
			return LoadSound(data2, data.Length, streaming, mappedMemory: false);
		}
	}

	private unsafe IAudioSound LoadSound(void* data, int length, bool streaming, bool mappedMemory)
	{
		ulong num = ((streaming || !IsWav(data, length)) ? WaylithAudioNative.wa_sound_load_memory_stream(data, (nuint)length, (!mappedMemory) ? 1 : 0) : WaylithAudioNative.wa_sound_load_memory(data, (nuint)length));
		if (num == 0L)
		{
			if (OggVorbisDemuxer.TryExtractVorbisStream(new ReadOnlySpan<byte>(data, length), out byte[] vorbis))
			{
				fixed (byte* data2 = vorbis)
				{
					num = WaylithAudioNative.wa_sound_load_memory_stream(data2, (nuint)vorbis.Length, 1);
				}
			}
			if (num == 0L)
			{
				throw new InvalidDataException($"miniaudio could not decode the sound ({WaylithAudioNative.wa_last_error()})");
			}
		}
		return new Sound(num, streaming);
	}

	private unsafe static bool IsWav(void* data, int length)
	{
		if (length < 12)
		{
			return false;
		}
		ReadOnlySpan<byte> readOnlySpan = new ReadOnlySpan<byte>(data, 12);
		if (readOnlySpan.Slice(0, 4).SequenceEqual("RIFF"u8))
		{
			return readOnlySpan.Slice(8).SequenceEqual("WAVE"u8);
		}
		return false;
	}

	public unsafe override IAudioSound LoadSoundFromPcm16(ReadOnlySpan<byte> pcm, int channels, int frequency)
	{
		fixed (byte* data = pcm)
		{
			ulong num = WaylithAudioNative.wa_sound_load_pcm(data, (ulong)(pcm.Length / (channels * 2)), (uint)channels, (uint)frequency, 1);
			if (num == 0L)
			{
				throw new InvalidOperationException("miniaudio could not create the pcm sound");
			}
			return new Sound(num, streaming: false);
		}
	}

	public unsafe override IAudioSound CreateStreamSound(int channels, int frequency, long lengthFrames, IAudioStreamSource source)
	{
		GCHandle gCHandle = GCHandle.Alloc(new StreamBox(source, channels));
		ulong num = WaylithAudioNative.wa_sound_create_user((uint)channels, (uint)frequency, (ulong)lengthFrames, (delegate* unmanaged<void*, float*, ulong, int>)(delegate*<void*, float*, ulong, int>)(&StreamRead), (delegate* unmanaged<void*, ulong, int>)(delegate*<void*, ulong, int>)(&StreamSeek), (void*)GCHandle.ToIntPtr(gCHandle));
		if (num == 0L)
		{
			gCHandle.Free();
			throw new InvalidOperationException("miniaudio could not create the stream sound");
		}
		return new Sound(num, streaming: true)
		{
			StreamHandle = gCHandle
		};
	}

	public override IAudioVoice? Play(IAudioSound sound, IAudioGroup? group, bool paused, bool autoRelease)
	{
		ulong num = WaylithAudioNative.wa_voice_play(((Sound)sound).Handle, (group is Group group2) ? group2.handle : 0, paused ? 1 : 0, autoRelease ? 1 : 0);
		if (num != 0L)
		{
			return new Voice(num, this);
		}
		return null;
	}

	[UnmanagedCallersOnly]
	private unsafe static void* HostAlloc(nuint size)
	{
		return (void*)NativeAllocationRuntime.Allocate((long)size, 16, AllocationKind.AudioKernel);
	}

	[UnmanagedCallersOnly]
	private unsafe static void* HostRealloc(void* p, nuint size)
	{
		if (p == null)
		{
			return (void*)NativeAllocationRuntime.Allocate((long)size, 16, AllocationKind.AudioKernel);
		}
		if (size == 0)
		{
			NativeAllocationRuntime.Free((nint)p, AllocationKind.AudioKernel);
			return null;
		}
		if (!NativeAllocationRuntime.TryGetAllocationSize((nint)p, out var size2))
		{
			return null;
		}
		nint num = NativeAllocationRuntime.Allocate((long)size, 16, AllocationKind.AudioKernel);
		Buffer.MemoryCopy(p, (void*)num, (long)size, System.Math.Min(size2, (long)size));
		NativeAllocationRuntime.Free((nint)p, AllocationKind.AudioKernel);
		return (void*)num;
	}

	[UnmanagedCallersOnly]
	private unsafe static void HostFree(void* p)
	{
		NativeAllocationRuntime.Free((nint)p, AllocationKind.AudioKernel);
	}

	[UnmanagedCallersOnly]
	private unsafe static int StreamRead(void* user, float* output, ulong frames)
	{
		try
		{
			StreamBox streamBox = (StreamBox)GCHandle.FromIntPtr((nint)user).Target;
			return streamBox.source.Read(new Span<float>(output, (int)frames * streamBox.channels));
		}
		catch
		{
			return 0;
		}
	}

	[UnmanagedCallersOnly]
	private unsafe static int StreamSeek(void* user, ulong frame)
	{
		try
		{
			return (!((StreamBox)GCHandle.FromIntPtr((nint)user).Target).source.Seek((long)frame)) ? (-1) : 0;
		}
		catch
		{
			return -1;
		}
	}
}
