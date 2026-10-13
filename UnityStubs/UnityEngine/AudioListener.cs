using System.Collections.Generic;
using System.Numerics;
using UnityStubs.Internal;

namespace UnityEngine;

public class AudioListener : AudioBehaviour
{
	private static readonly List<AudioListener> ActiveListeners = new List<AudioListener>();

	private static float _volume = 1f;

	public static float volume
	{
		get
		{
			return _volume;
		}
		set
		{
			_volume = value;
			if (AudioBackendAccess.Ready)
			{
				AudioBackendAccess.Backend.SetMasterVolume(value);
			}
			foreach (AudioSource item in AudioSource.IgnoringListenerVolume)
			{
				item.OnListenerVolumeChanged();
			}
		}
	}

	internal static AudioListener Current
	{
		get
		{
			if (ActiveListeners.Count <= 0)
			{
				return null;
			}
			List<AudioListener> activeListeners = ActiveListeners;
			return activeListeners[activeListeners.Count - 1];
		}
	}

	public void OnEnable()
	{
		if (!ActiveListeners.Contains(this))
		{
			ActiveListeners.Add(this);
		}
	}

	public void OnDisable()
	{
		ActiveListeners.Remove(this);
	}

	public static void SyncToBackend()
	{
		AudioListener current = Current;
		if (!(current == null) && AudioBackendAccess.Ready)
		{
			AudioBackendAccess.Backend.SetListener(ToNumerics(current.transform.position), ToNumerics(current.transform.forward), ToNumerics(current.transform.up));
		}
	}

	private static System.Numerics.Vector3 ToNumerics(Vector3 value)
	{
		return new System.Numerics.Vector3(value.x, value.y, value.z);
	}
}
