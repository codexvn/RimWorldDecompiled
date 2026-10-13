using System;
using System.Collections.Generic;
using Waylith.Audio;

namespace UnityEngine.Audio;

public class AudioMixer : Object
{
	private readonly Dictionary<string, AudioMixerGroup> _groupsByPath = new Dictionary<string, AudioMixerGroup>(StringComparer.OrdinalIgnoreCase);

	private readonly Dictionary<string, float> _floatParameters = new Dictionary<string, float>(StringComparer.Ordinal);

	public AudioMixerGroup[] FindMatchingGroups(string subPath)
	{
		if (string.IsNullOrWhiteSpace(subPath))
		{
			return Array.Empty<AudioMixerGroup>();
		}
		List<AudioMixerGroup> list = new List<AudioMixerGroup>();
		foreach (KeyValuePair<string, AudioMixerGroup> item in _groupsByPath)
		{
			if (item.Key.Equals(subPath, StringComparison.OrdinalIgnoreCase) || item.Key.EndsWith("/" + subPath, StringComparison.OrdinalIgnoreCase) || item.Value.name.Equals(subPath, StringComparison.OrdinalIgnoreCase))
			{
				list.Add(item.Value);
			}
		}
		return list.ToArray();
	}

	public bool SetFloat(string name, float value)
	{
		if (string.IsNullOrEmpty(name))
		{
			return false;
		}
		_floatParameters[name] = value;
		return true;
	}

	internal bool TryGetFloat(string name, out float value)
	{
		return _floatParameters.TryGetValue(name, out value);
	}

	public AudioMixerGroup AddGroup(string path, IAudioGroup group)
	{
		AudioMixerGroup audioMixerGroup = new AudioMixerGroup(this, group, path);
		_groupsByPath[path] = audioMixerGroup;
		return audioMixerGroup;
	}
}
