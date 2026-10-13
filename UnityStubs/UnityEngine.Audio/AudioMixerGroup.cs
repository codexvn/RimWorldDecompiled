using UnityEngine.Internal;
using Waylith.Audio;

namespace UnityEngine.Audio;

public class AudioMixerGroup : Object, ISubAssetNotDuplicatable
{
	public AudioMixer audioMixer => AudioMixer;

	internal AudioMixer AudioMixer { get; set; }

	internal IAudioGroup Group { get; set; }

	internal string Path { get; set; }

	internal AudioMixerGroup(AudioMixer audioMixer, IAudioGroup group, string path)
	{
		AudioMixer = audioMixer;
		Group = group;
		Path = path;
		base.name = path;
	}

	public AudioMixerGroup()
	{
		Path = string.Empty;
	}
}
