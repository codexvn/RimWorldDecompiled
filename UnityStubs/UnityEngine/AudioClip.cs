using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using Fmod5Sharp;
using Fmod5Sharp.FmodTypes;
using UnityEngine.Audio;
using UnityStubs;
using UnityStubs.Internal;
using Waylith.Assets;
using Waylith.Audio;
using Waylith.Rendering;

namespace UnityEngine;

public class AudioClip : AudioResource
{
	internal sealed class Opened : IPrefetchedAsset
	{
		public required IAudioSound Sound { get; init; }

		public required bool Streamed { get; init; }

		public bool Packed { get; init; }

		public bool BakedMono { get; init; }

		public bool ForceToMono { get; init; }

		public long Bytes { get; init; }

		public void Release()
		{
			Sound.Release();
		}
	}

	private IAudioSound sound;

	private bool streamed;

	private List<(IAudioSound sound, AudioSource holder)> streamInstances;

	private bool bakedMono;

	private readonly bool packed;

	private readonly string sourcePath;

	private float normalizeGain;

	public bool isFinite
	{
		get
		{
			StubWarner.LogStub("isFinite", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\UnityStubs\\AudioClip.cs", 10);
			return false;
		}
	}

	public bool isRealtime
	{
		get
		{
			StubWarner.LogStub("isRealtime", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\UnityStubs\\AudioClip.cs", 11);
			return false;
		}
	}

	public float length => (float)sound.LengthFrames / (float)Math.Max(1, sound.Frequency);

	public int channels => sound.Channels;

	public int frequency => sound.Frequency;

	public int samples => (int)sound.LengthFrames;

	internal bool ForceToMono { get; private set; }

	internal float NormalizeGain
	{
		get
		{
			if (!ForceToMono || bakedMono || sourcePath == null)
			{
				return 1f;
			}
			if (normalizeGain == 0f)
			{
				normalizeGain = AudioImportGains.GetMonoNormalizeGain(sourcePath);
			}
			return normalizeGain;
		}
	}

	internal IAudioSound Sound => sound;

	public AudioClip(IAudioSound sound)
	{
		this.sound = sound;
	}

	private AudioClip(Opened opened, string sourcePath)
		: this(opened.Sound)
	{
		this.sourcePath = sourcePath;
		bakedMono = opened.BakedMono;
		packed = opened.Packed;
		ForceToMono = opened.ForceToMono;
		streamed = opened.Streamed;
	}

	internal static AudioClip Load(string path, bool importedByUnity)
	{
		return new AudioClip(Open(path, importedByUnity), path);
	}

	internal static IPrefetchedAsset OpenForPrefetch(string path)
	{
		return Open(path, ImportDefaults.IsOfficial(path));
	}

	internal static Opened Open(string path, bool importedByUnity)
	{
		bool flag;
		if (AudioBackendAccess.Backend.TryGetDecodedSize(path, out var pcm16Bytes))
		{
			flag = (importedByUnity ? (pcm16Bytes >= AudioBackendAccess.Backend.StreamingThresholdBytes) : (pcm16Bytes > AudioBackendAccess.Backend.ModStreamingThresholdBytes));
		}
		else
		{
			AudioBackend.TryGetSourceLength(path, out var num, out var transcoded);
			flag = (importedByUnity ? ((transcoded ? (num * 10) : num) >= AudioBackendAccess.Backend.StreamingThresholdBytes) : (num > AudioBackendAccess.Backend.ModStreamingThresholdBytes));
		}
		bool flag2 = ContentPack.HasFile(path);
		if (importedByUnity && !flag2 && !flag && AudioImportGains.TryReadAsImportedMono(path, out var pcm, out var num2))
		{
			IAudioSound audioSound = AudioBackendAccess.Backend.LoadSoundFromPcm16(pcm, 1, num2);
			return new Opened
			{
				Sound = audioSound,
				Streamed = false,
				BakedMono = true,
				ForceToMono = true,
				Bytes = pcm.Length
			};
		}
		IAudioSound audioSound2 = AudioBackendAccess.Backend.LoadSound(path, flag);
		return new Opened
		{
			Sound = audioSound2,
			Streamed = flag,
			Packed = flag2,
			BakedMono = (flag2 && importedByUnity),
			ForceToMono = importedByUnity,
			Bytes = (flag ? 0 : (audioSound2.LengthFrames * audioSound2.Channels * 2))
		};
	}

	internal void KeepStereo()
	{
		ForceToMono = false;
		if (bakedMono && !packed)
		{
			sound.Release();
			sound = AudioBackendAccess.Backend.LoadSound(sourcePath, streaming: false);
			bakedMono = false;
		}
	}

	internal IAudioSound AcquireSound(AudioSource owner)
	{
		if (!streamed || !sound.InstancePerVoice)
		{
			return sound;
		}
		if (streamInstances == null)
		{
			int num = 1;
			List<(IAudioSound, AudioSource)> list = new List<(IAudioSound, AudioSource)>(num);
			CollectionsMarshal.SetCount(list, num);
			CollectionsMarshal.AsSpan(list)[0] = (sound, null);
			streamInstances = list;
		}
		for (int i = 0; i < streamInstances.Count; i++)
		{
			var (audioSound, audioSource) = streamInstances[i];
			if (!(audioSource != null) || !(audioSource != owner) || !audioSource.HoldsVoiceOf(audioSound))
			{
				streamInstances[i] = (audioSound, owner);
				return audioSound;
			}
		}
		IAudioSound audioSound2 = AudioBackendAccess.Backend.LoadSound(sourcePath, streaming: true);
		streamInstances.Add((audioSound2, owner));
		return audioSound2;
	}

	public static AudioClip LoadFromFile(string path)
	{
		if (AssetPrefetcher.TryTakeAudio(Path.GetFullPath(path), out IPrefetchedAsset asset))
		{
			return new AudioClip((Opened)asset, path);
		}
		return Load(path, ImportDefaults.IsOfficial(path));
	}

	public static AudioClip LoadFromMemory(ReadOnlySpan<byte> data, string name)
	{
		try
		{
			if (!AudioBackendAccess.Backend.CanDecodeFsb && data.Length > 4 && data.Slice(0, 4).SequenceEqual("FSB5"u8))
			{
				FmodSoundBank fmodSoundBank = FsbLoader.LoadFsbFromByteArray(data.ToArray());
				if (fmodSoundBank.Samples.Count == 0)
				{
					throw new InvalidDataException("the sound bank holds no samples");
				}
				if (!fmodSoundBank.Samples[0].RebuildAsStandardFileFormat(out byte[] data2, out string _))
				{
					throw new NotSupportedException($"sound bank codec {fmodSoundBank.Header.AudioType} cannot be rebuilt");
				}
				return new AudioClip(AudioBackendAccess.Backend.LoadSoundFromMemory(data2, streaming: false))
				{
					name = name
				};
			}
			return new AudioClip(AudioBackendAccess.Backend.LoadSoundFromMemory(data, streaming: false))
			{
				name = name
			};
		}
		catch (Exception ex)
		{
			Debug.LogWarning($"AudioClip '{name}' could not be decoded by the {AudioBackendAccess.Backend.Name} backend and plays as silence: {ex.Message}");
			return Create(name, 4800, 1, 48000, stream: false);
		}
	}

	public static AudioClip Create(string name, int lengthSamples, int channels, int frequency, bool stream)
	{
		if (name == null)
		{
			throw new NullReferenceException();
		}
		if (lengthSamples <= 0)
		{
			throw new ArgumentException("Length of created clip must be larger than 0");
		}
		if (channels <= 0)
		{
			throw new ArgumentException("Number of channels in created clip must be greater than 0");
		}
		if (frequency <= 0)
		{
			throw new ArgumentException("Frequency in created clip must be greater than 0");
		}
		byte[] array = new byte[lengthSamples * channels * 2];
		return new AudioClip(AudioBackendAccess.Backend.LoadSoundFromPcm16(array, channels, frequency))
		{
			name = name
		};
	}

	public bool GetData(float[] data, int offsetSamples)
	{
		if (data == null || data.Length == 0)
		{
			return true;
		}
		if (sound.TryReadPcm(data, offsetSamples))
		{
			return true;
		}
		Debug.LogError("AudioClip.GetData failed; AudioClip " + base.name + " contains no data");
		return false;
	}

	public bool SetData(float[] data, int offsetSamples)
	{
		if (offsetSamples < 0 || offsetSamples >= samples)
		{
			throw new ArgumentException("AudioClip.SetData failed; invalid offsetSamples");
		}
		if (data == null || data.Length == 0)
		{
			throw new ArgumentException("AudioClip.SetData failed; invalid data");
		}
		if (sound.TryWritePcm(data, offsetSamples))
		{
			return true;
		}
		Debug.LogError("AudioClip.SetData failed; AudioClip " + base.name + " contains no data");
		return false;
	}
}
