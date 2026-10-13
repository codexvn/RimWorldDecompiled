using UnityStubs.Internal;

namespace UnityEngine;

public sealed class AudioSettings
{
	public delegate void AudioConfigurationChangeHandler(bool deviceWasChanged);

	public static double dspTime
	{
		get
		{
			if (!AudioBackendAccess.Ready)
			{
				return 0.0;
			}
			return AudioBackendAccess.Backend.DspTime;
		}
	}

	public static int outputSampleRate
	{
		get
		{
			if (!AudioBackendAccess.Ready)
			{
				return 48000;
			}
			return AudioBackendAccess.Backend.SampleRate;
		}
	}

	public static AudioSpeakerMode speakerMode
	{
		get
		{
			return GetConfiguration().speakerMode;
		}
		set
		{
			AudioConfiguration configuration = GetConfiguration();
			configuration.speakerMode = value;
			Reset(configuration);
		}
	}

	public static AudioSpeakerMode driverCapabilities
	{
		get
		{
			if (!AudioBackendAccess.Ready)
			{
				return AudioSpeakerMode.Stereo;
			}
			return FromChannels(AudioBackendAccess.Backend.DriverChannels);
		}
	}

	public static event AudioConfigurationChangeHandler OnAudioConfigurationChanged;

	public static AudioConfiguration GetConfiguration()
	{
		AudioConfiguration audioConfiguration = default(AudioConfiguration);
		audioConfiguration.speakerMode = AudioSpeakerMode.Stereo;
		audioConfiguration.sampleRate = 48000;
		audioConfiguration.dspBufferSize = 1024;
		audioConfiguration.numRealVoices = 32;
		audioConfiguration.numVirtualVoices = 512;
		AudioConfiguration result = audioConfiguration;
		if (!AudioBackendAccess.Ready)
		{
			return result;
		}
		result.sampleRate = AudioBackendAccess.Backend.SampleRate;
		result.speakerMode = FromChannels(AudioBackendAccess.Backend.OutputChannels);
		result.dspBufferSize = AudioBackendAccess.Backend.DspBufferFrames;
		return result;
	}

	public static bool Reset(AudioConfiguration config)
	{
		if (!AudioBackendAccess.Ready)
		{
			return false;
		}
		AudioConfiguration configuration = GetConfiguration();
		if (config.speakerMode != configuration.speakerMode || (config.sampleRate > 0 && config.sampleRate != configuration.sampleRate) || (config.dspBufferSize > 0 && config.dspBufferSize != configuration.dspBufferSize))
		{
			StubWarner.LogStub("Reset", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\UnityStubs\\AudioSettings.cs", 45);
			return false;
		}
		AudioSettings.OnAudioConfigurationChanged?.Invoke(deviceWasChanged: false);
		return true;
	}

	private static AudioSpeakerMode FromChannels(int channels)
	{
		if (channels < 8)
		{
			return channels switch
			{
				1 => AudioSpeakerMode.Mono, 
				4 => AudioSpeakerMode.Quad, 
				5 => AudioSpeakerMode.Surround, 
				6 => AudioSpeakerMode.Mode5point1, 
				_ => AudioSpeakerMode.Stereo, 
			};
		}
		return AudioSpeakerMode.Mode7point1;
	}

	public static void GetDSPBufferSize(out int bufferLength, out int numBuffers)
	{
		bufferLength = (AudioBackendAccess.Ready ? AudioBackendAccess.Backend.DspBufferFrames : 0);
		numBuffers = (AudioBackendAccess.Ready ? 4 : 0);
	}
}
