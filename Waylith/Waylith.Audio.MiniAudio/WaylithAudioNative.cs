using System;
using System.CodeDom.Compiler;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;

namespace Waylith.Audio.MiniAudio;

internal static class WaylithAudioNative
{
	private const string Library = "waylithaudio";

	[DllImport("waylithaudio", ExactSpelling = true)]
	[LibraryImport("waylithaudio")]
	public static extern int wa_init(uint sampleRate, uint channels, int nullBackend);

	[DllImport("waylithaudio", ExactSpelling = true)]
	[LibraryImport("waylithaudio")]
	public static extern void wa_shutdown();

	[DllImport("waylithaudio", ExactSpelling = true)]
	[LibraryImport("waylithaudio")]
	public static extern int wa_last_error();

	[DllImport("waylithaudio", ExactSpelling = true)]
	[LibraryImport("waylithaudio")]
	public static extern uint wa_sample_rate();

	[DllImport("waylithaudio", ExactSpelling = true)]
	[LibraryImport("waylithaudio")]
	public static extern uint wa_channels();

	[DllImport("waylithaudio", ExactSpelling = true)]
	[LibraryImport("waylithaudio")]
	public static extern ulong wa_time_frames();

	[DllImport("waylithaudio", ExactSpelling = true)]
	[LibraryImport("waylithaudio")]
	public static extern uint wa_period_frames();

	[DllImport("waylithaudio", ExactSpelling = true)]
	[LibraryImport("waylithaudio")]
	public static extern void wa_set_master_volume(float volume);

	[DllImport("waylithaudio", ExactSpelling = true)]
	[LibraryImport("waylithaudio")]
	public static extern void wa_set_master_paused(int paused);

	[DllImport("waylithaudio", ExactSpelling = true)]
	[LibraryImport("waylithaudio")]
	public static extern void wa_set_listener(float px, float py, float pz, float fx, float fy, float fz, float ux, float uy, float uz);

	[DllImport("waylithaudio", ExactSpelling = true)]
	[LibraryImport("waylithaudio")]
	public static extern void wa_update();

	[DllImport("waylithaudio", ExactSpelling = true)]
	[LibraryImport("waylithaudio")]
	public static extern int wa_playing_count();

	[DllImport("waylithaudio", ExactSpelling = true)]
	[LibraryImport("waylithaudio")]
	public unsafe static extern ulong wa_sound_load_memory(void* data, nuint length);

	[DllImport("waylithaudio", ExactSpelling = true)]
	[LibraryImport("waylithaudio")]
	public unsafe static extern ulong wa_sound_load_memory_stream(void* data, nuint length, int copy);

	[DllImport("waylithaudio", ExactSpelling = true)]
	[LibraryImport("waylithaudio")]
	public unsafe static extern ulong wa_sound_load_pcm(void* data, ulong frames, uint channels, uint rate, int format);

	[LibraryImport("waylithaudio", StringMarshalling = StringMarshalling.Utf8)]
	[GeneratedCode("Microsoft.Interop.LibraryImportGenerator", "42.42.42.42")]
	[SkipLocalsInit]
	public unsafe static ulong wa_sound_load_file_stream(string path)
	{
		ulong num = 0uL;
		Utf8StringMarshaller.ManagedToUnmanagedIn managedToUnmanagedIn = default(Utf8StringMarshaller.ManagedToUnmanagedIn);
		try
		{
			Span<byte> buffer = stackalloc byte[Utf8StringMarshaller.ManagedToUnmanagedIn.BufferSize];
			managedToUnmanagedIn.FromManaged(path, buffer);
			return __PInvoke(managedToUnmanagedIn.ToUnmanaged());
		}
		finally
		{
			managedToUnmanagedIn.Free();
		}
		[DllImport("waylithaudio", EntryPoint = "wa_sound_load_file_stream", ExactSpelling = true)]
		static extern unsafe ulong __PInvoke(byte* __path_native);
	}

	[LibraryImport("waylithaudio")]
	[GeneratedCode("Microsoft.Interop.LibraryImportGenerator", "42.42.42.42")]
	[SkipLocalsInit]
	public unsafe static int wa_probe_memory(void* data, nuint length, out ulong frames, out uint channels, out uint rate)
	{
		frames = 0uL;
		channels = 0u;
		rate = 0u;
		int result;
		fixed (uint* _rate_native = &rate)
		{
			fixed (uint* _channels_native = &channels)
			{
				fixed (ulong* _frames_native = &frames)
				{
					result = __PInvoke(data, length, _frames_native, _channels_native, _rate_native);
				}
			}
		}
		return result;
		[DllImport("waylithaudio", EntryPoint = "wa_probe_memory", ExactSpelling = true)]
		static extern unsafe int __PInvoke(void* __data_native, nuint __length_native, ulong* __frames_native, uint* __channels_native, uint* __rate_native);
	}

	[LibraryImport("waylithaudio", StringMarshalling = StringMarshalling.Utf8)]
	[GeneratedCode("Microsoft.Interop.LibraryImportGenerator", "42.42.42.42")]
	[SkipLocalsInit]
	public unsafe static int wa_probe_file(string path, out ulong frames, out uint channels, out uint rate)
	{
		frames = 0uL;
		channels = 0u;
		rate = 0u;
		int num = 0;
		Utf8StringMarshaller.ManagedToUnmanagedIn managedToUnmanagedIn = default(Utf8StringMarshaller.ManagedToUnmanagedIn);
		try
		{
			Span<byte> buffer = stackalloc byte[Utf8StringMarshaller.ManagedToUnmanagedIn.BufferSize];
			managedToUnmanagedIn.FromManaged(path, buffer);
			fixed (uint* _rate_native = &rate)
			{
				fixed (uint* _channels_native = &channels)
				{
					fixed (ulong* _frames_native = &frames)
					{
						return __PInvoke(managedToUnmanagedIn.ToUnmanaged(), _frames_native, _channels_native, _rate_native);
					}
				}
			}
		}
		finally
		{
			managedToUnmanagedIn.Free();
		}
		[DllImport("waylithaudio", EntryPoint = "wa_probe_file", ExactSpelling = true)]
		static extern unsafe int __PInvoke(byte* __path_native, ulong* __frames_native, uint* __channels_native, uint* __rate_native);
	}

	[DllImport("waylithaudio", ExactSpelling = true)]
	[LibraryImport("waylithaudio")]
	public unsafe static extern void wa_set_allocator(delegate* unmanaged<nuint, void*> alloc, delegate* unmanaged<void*, nuint, void*> realloc, delegate* unmanaged<void*, void> free);

	[DllImport("waylithaudio", ExactSpelling = true)]
	[LibraryImport("waylithaudio")]
	public unsafe static extern ulong wa_sound_create_user(uint channels, uint rate, ulong lengthFrames, delegate* unmanaged<void*, float*, ulong, int> read, delegate* unmanaged<void*, ulong, int> seek, void* user);

	[DllImport("waylithaudio", ExactSpelling = true)]
	[LibraryImport("waylithaudio")]
	public static extern void wa_sound_release(ulong sound);

	[DllImport("waylithaudio", ExactSpelling = true)]
	[LibraryImport("waylithaudio")]
	public static extern ulong wa_sound_length_frames(ulong sound);

	[DllImport("waylithaudio", ExactSpelling = true)]
	[LibraryImport("waylithaudio")]
	public static extern uint wa_sound_channels(ulong sound);

	[DllImport("waylithaudio", ExactSpelling = true)]
	[LibraryImport("waylithaudio")]
	public static extern uint wa_sound_rate(ulong sound);

	[DllImport("waylithaudio", ExactSpelling = true)]
	[LibraryImport("waylithaudio")]
	public static extern int wa_sound_kind(ulong sound);

	[DllImport("waylithaudio", ExactSpelling = true)]
	[LibraryImport("waylithaudio")]
	public unsafe static extern short* wa_sound_pcm(ulong sound);

	[DllImport("waylithaudio", ExactSpelling = true)]
	[LibraryImport("waylithaudio")]
	public static extern ulong wa_group_create(ulong parent);

	[DllImport("waylithaudio", ExactSpelling = true)]
	[LibraryImport("waylithaudio")]
	public static extern void wa_group_release(ulong group);

	[DllImport("waylithaudio", ExactSpelling = true)]
	[LibraryImport("waylithaudio")]
	public static extern void wa_group_set_volume(ulong group, float volume);

	[DllImport("waylithaudio", ExactSpelling = true)]
	[LibraryImport("waylithaudio")]
	public static extern void wa_group_set_mute(ulong group, int mute);

	[DllImport("waylithaudio", ExactSpelling = true)]
	[LibraryImport("waylithaudio")]
	public static extern void wa_group_set_pitch(ulong group, float pitch);

	[DllImport("waylithaudio", ExactSpelling = true)]
	[LibraryImport("waylithaudio")]
	public static extern void wa_group_set_paused(ulong group, int paused);

	[DllImport("waylithaudio", ExactSpelling = true)]
	[LibraryImport("waylithaudio")]
	public static extern ulong wa_voice_play(ulong sound, ulong group, int startPaused, int autoRelease);

	[DllImport("waylithaudio", ExactSpelling = true)]
	[LibraryImport("waylithaudio")]
	public static extern void wa_voice_stop(ulong voice);

	[DllImport("waylithaudio", ExactSpelling = true)]
	[LibraryImport("waylithaudio")]
	public static extern int wa_voice_is_playing(ulong voice);

	[DllImport("waylithaudio", ExactSpelling = true)]
	[LibraryImport("waylithaudio")]
	public static extern void wa_voice_set_paused(ulong voice, int paused);

	[DllImport("waylithaudio", ExactSpelling = true)]
	[LibraryImport("waylithaudio")]
	public static extern void wa_voice_set_volume(ulong voice, float volume);

	[DllImport("waylithaudio", ExactSpelling = true)]
	[LibraryImport("waylithaudio")]
	public static extern void wa_voice_set_mute(ulong voice, int mute);

	[DllImport("waylithaudio", ExactSpelling = true)]
	[LibraryImport("waylithaudio")]
	public static extern void wa_voice_set_pitch(ulong voice, float pitch);

	[DllImport("waylithaudio", ExactSpelling = true)]
	[LibraryImport("waylithaudio")]
	public static extern void wa_voice_set_priority(ulong voice, int priority);

	[DllImport("waylithaudio", ExactSpelling = true)]
	[LibraryImport("waylithaudio")]
	public static extern void wa_voice_set_loop(ulong voice, int loop);

	[DllImport("waylithaudio", ExactSpelling = true)]
	[LibraryImport("waylithaudio")]
	public static extern void wa_voice_set_pan(ulong voice, float pan);

	[DllImport("waylithaudio", ExactSpelling = true)]
	[LibraryImport("waylithaudio")]
	public unsafe static extern void wa_voice_set_mix_matrix(ulong voice, float* matrix, uint outChannels, uint inChannels);

	[DllImport("waylithaudio", ExactSpelling = true)]
	[LibraryImport("waylithaudio")]
	public static extern void wa_voice_set_group(ulong voice, ulong group);

	[DllImport("waylithaudio", ExactSpelling = true)]
	[LibraryImport("waylithaudio")]
	public static extern ulong wa_voice_get_position_frames(ulong voice);

	[DllImport("waylithaudio", ExactSpelling = true)]
	[LibraryImport("waylithaudio")]
	public static extern int wa_voice_set_position_frames(ulong voice, ulong frame);

	[DllImport("waylithaudio", ExactSpelling = true)]
	[LibraryImport("waylithaudio")]
	public static extern void wa_voice_set_start_time(ulong voice, ulong absoluteFrames);

	[DllImport("waylithaudio", ExactSpelling = true)]
	[LibraryImport("waylithaudio")]
	public static extern void wa_voice_set_3d(ulong voice, int enabled, float minDistance, float maxDistance, int rolloff);

	[DllImport("waylithaudio", ExactSpelling = true)]
	[LibraryImport("waylithaudio")]
	public static extern void wa_voice_set_3d_position(ulong voice, float x, float y, float z);

	[DllImport("waylithaudio", ExactSpelling = true)]
	[LibraryImport("waylithaudio")]
	public static extern void wa_voice_set_doppler(ulong voice, float factor);

	[DllImport("waylithaudio", ExactSpelling = true)]
	[LibraryImport("waylithaudio")]
	public static extern int wa_voice_set_lowpass(ulong voice, int enabled, float cutoffHz, float q);

	[DllImport("waylithaudio", ExactSpelling = true)]
	[LibraryImport("waylithaudio")]
	public static extern int wa_voice_set_highpass(ulong voice, int enabled, float cutoffHz, float q);

	[DllImport("waylithaudio", ExactSpelling = true)]
	[LibraryImport("waylithaudio")]
	public static extern int wa_voice_set_echo(ulong voice, int enabled, float delayMs, float feedback, float wet, float dry);

	[DllImport("waylithaudio", ExactSpelling = true)]
	[LibraryImport("waylithaudio")]
	public static extern int wa_voice_set_reverb(ulong voice, int enabled, float roomsize, float damp, float wet, float dry, float width, float highCutHz);

	[DllImport("waylithaudio", ExactSpelling = true)]
	[LibraryImport("waylithaudio")]
	public static extern int wa_voice_tap_enable(ulong voice, uint capacityFrames);

	[DllImport("waylithaudio", ExactSpelling = true)]
	[LibraryImport("waylithaudio")]
	public unsafe static extern int wa_voice_tap_read(ulong voice, float* output, uint frames, uint channel);
}
