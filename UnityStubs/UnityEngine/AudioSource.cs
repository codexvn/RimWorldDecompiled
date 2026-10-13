using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine.Audio;
using UnityStubs.Internal;
using Waylith.Audio;
using Waylith.Audio.MiniAudio;
using Waylith.VideoDecode;

namespace UnityEngine;

public class AudioSource : AudioBehaviour
{
	private int _priority = 128;

	internal static readonly HashSet<AudioSource> IgnoringListenerVolume = new HashSet<AudioSource>();

	private bool _ignoreListenerVolume;

	private AudioClip _clip;

	private AudioMixerGroup _outputAudioMixerGroup;

	private float _time;

	private float _volume = 1f;

	private float _pitch = 1f;

	private float _panStereo;

	private float _spatialBlend;

	private float _minDistance = 1f;

	private float _maxDistance = 500f;

	private float _dopplerLevel = 1f;

	private float _spread;

	private readonly List<(IAudioVoice voice, IAudioSound sound)> _oneShots = new List<(IAudioVoice, IAudioSound)>();

	private bool _startTimePending;

	private bool _loop;

	private bool _mute;

	private AudioRolloffMode _rolloffMode;

	private IAudioVoice _voice;

	private IAudioSound _playingSound;

	private bool _paused;

	private static bool _spreadWarned;

	internal VideoPlayer VideoFeed;

	public bool bypassEffects { get; set; }

	public bool bypassListenerEffects { get; set; }

	public bool bypassReverbZones { get; set; }

	public bool playOnAwake { get; set; }

	public bool ignoreListenerPause { get; set; }

	public int priority
	{
		get
		{
			return _priority;
		}
		set
		{
			_priority = Math.Clamp(value, 0, 256);
			_voice?.SetPriority(_priority);
		}
	}

	public bool ignoreListenerVolume
	{
		get
		{
			return _ignoreListenerVolume;
		}
		set
		{
			_ignoreListenerVolume = value;
			if (value)
			{
				IgnoringListenerVolume.Add(this);
			}
			else
			{
				IgnoringListenerVolume.Remove(this);
			}
			if (_voice != null)
			{
				ApplyVolume();
			}
		}
	}

	public AudioClip clip
	{
		get
		{
			return _clip;
		}
		set
		{
			_clip = value;
		}
	}

	public bool isPlaying
	{
		get
		{
			if (_oneShots.Count > 0)
			{
				PruneOneShots();
				if (_oneShots.Count > 0)
				{
					return true;
				}
			}
			if (_voice == null || _paused)
			{
				return false;
			}
			if (!_voice.IsPlaying)
			{
				Invalidate();
				return false;
			}
			return true;
		}
	}

	public AudioMixerGroup outputAudioMixerGroup
	{
		get
		{
			return _outputAudioMixerGroup;
		}
		set
		{
			_outputAudioMixerGroup = value;
			if (value != null)
			{
				_voice?.SetGroup(value.Group);
			}
		}
	}

	public float time
	{
		get
		{
			if (_voice != null && _playingSound != null)
			{
				_time = (float)_voice.PositionFrames / (float)Math.Max(1, _playingSound.Frequency);
			}
			return _time;
		}
		set
		{
			_time = Math.Max(0f, value);
			_startTimePending = true;
			if (_voice != null && _playingSound != null)
			{
				_voice.PositionFrames = (long)Math.Round(_time * (float)_playingSound.Frequency);
			}
		}
	}

	public float volume
	{
		get
		{
			return _volume;
		}
		set
		{
			_volume = Mathf.Clamp01(value);
			if (_voice != null)
			{
				ApplyVolume();
			}
		}
	}

	public float spread
	{
		get
		{
			return _spread;
		}
		set
		{
			_spread = Math.Clamp(value, 0f, 360f);
			if (_voice != null && _spatialBlend > 0f)
			{
				_voice.SetSpread(_spread);
			}
		}
	}

	public float dopplerLevel
	{
		set
		{
			_dopplerLevel = value;
			if (_voice != null && _spatialBlend > 0f)
			{
				_voice.SetDoppler(value);
			}
		}
	}

	public bool loop
	{
		set
		{
			_loop = value;
			_voice?.SetLoop(value);
		}
	}

	public float maxDistance
	{
		set
		{
			_maxDistance = value;
			if (_voice != null && _spatialBlend > 0f)
			{
				ApplySpatialMode();
			}
		}
	}

	public float minDistance
	{
		set
		{
			_minDistance = value;
			if (_voice != null && _spatialBlend > 0f)
			{
				ApplySpatialMode();
			}
		}
	}

	public bool mute
	{
		set
		{
			_mute = value;
			_voice?.SetMuted(value);
		}
	}

	public float panStereo
	{
		set
		{
			_panStereo = value;
			if (_voice != null && _spatialBlend <= 0f)
			{
				ApplyPan();
			}
		}
	}

	public float pitch
	{
		get
		{
			return _pitch;
		}
		set
		{
			_pitch = value;
			_voice?.SetPitch(value);
		}
	}

	public AudioRolloffMode rolloffMode
	{
		set
		{
			_rolloffMode = value;
			if (_voice != null && _spatialBlend > 0f)
			{
				ApplySpatialMode();
			}
		}
	}

	internal bool Muted => _mute;

	internal float OutputVolume
	{
		get
		{
			if (!_ignoreListenerVolume || !(AudioListener.volume > 0f))
			{
				return _volume;
			}
			return _volume / AudioListener.volume;
		}
	}

	internal IAudioGroup OutputGroup => _outputAudioMixerGroup?.Group;

	public float spatialBlend
	{
		get
		{
			return _spatialBlend;
		}
		set
		{
			_spatialBlend = value;
			if (_voice != null)
			{
				if (value > 0f)
				{
					ApplySpatialMode();
					ApplySpatialState();
				}
				else
				{
					ApplyNonSpatialMode();
				}
			}
		}
	}

	private AudioRolloffModel Rolloff
	{
		get
		{
			if (_rolloffMode == AudioRolloffMode.Linear)
			{
				return AudioRolloffModel.Linear;
			}
			return AudioRolloffModel.Inverse;
		}
	}

	internal bool HoldsVoiceOf(IAudioSound sound)
	{
		if (_oneShots.Count > 0)
		{
			PruneOneShots();
			foreach (var oneShot in _oneShots)
			{
				if (oneShot.sound == sound)
				{
					return true;
				}
			}
		}
		if (_voice != null && _playingSound == sound)
		{
			return _voice.IsPlaying;
		}
		return false;
	}

	public void Pause()
	{
		if (_voice != null)
		{
			_voice.SetPaused(paused: true);
			_paused = true;
		}
	}

	public void Play()
	{
		Play(-1.0);
	}

	public void PlayDelayed(float delay)
	{
		Play(AudioSettings.dspTime + (double)Math.Max(delay, 0f));
	}

	public void PlayScheduled(double time)
	{
		Play(Math.Max(time, 0.0));
	}

	private void Play(double dspStart)
	{
		AudioClip audioClip = _clip;
		if ((object)audioClip == null)
		{
			return;
		}
		float num = (_startTimePending ? _time : 0f);
		Stop();
		_time = num;
		_startTimePending = num > 0f;
		_playingSound = audioClip.AcquireSound(this);
		_voice = AudioBackendAccess.Backend.Play(_playingSound, _outputAudioMixerGroup?.Group, paused: true, autoRelease: true);
		if (_voice == null)
		{
			_playingSound = null;
			return;
		}
		_paused = false;
		ApplyVolume();
		_voice.SetMuted(_mute);
		_voice.SetPitch(_pitch);
		_voice.SetPriority(_priority);
		_voice.SetLoop(_loop);
		if (_spatialBlend > 0f)
		{
			ApplySpatialMode();
			ApplySpatialState();
		}
		else
		{
			ApplyNonSpatialMode();
		}
		if (_time > 0f)
		{
			_voice.PositionFrames = (long)Math.Round(_time * (float)_playingSound.Frequency);
		}
		AttachFilters();
		if (dspStart >= 0.0)
		{
			_voice.ScheduleStart(dspStart - AudioSettings.dspTime);
		}
		_voice.SetPaused(paused: false);
	}

	public static void PlayClipAtPoint(AudioClip clip, Vector3 position, float volume)
	{
		StubWarner.LogStub("PlayClipAtPoint", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\UnityStubs\\AudioSource.cs", 204);
	}

	public void PlayOneShot(AudioClip clip)
	{
		PlayOneShot(clip, 1f);
	}

	public void PlayOneShot(AudioClip clip, float volumeScale)
	{
		if (clip == null)
		{
			return;
		}
		PruneOneShots();
		IAudioSound audioSound = clip.AcquireSound(this);
		IAudioVoice audioVoice = AudioBackendAccess.Backend.Play(audioSound, _outputAudioMixerGroup?.Group, paused: true, autoRelease: true);
		if (audioVoice != null)
		{
			float num = _volume * volumeScale * clip.NormalizeGain;
			if (_ignoreListenerVolume && AudioListener.volume > 0f)
			{
				num /= AudioListener.volume;
			}
			audioVoice.SetVolume(num);
			audioVoice.SetMuted(_mute);
			audioVoice.SetPitch(_pitch);
			audioVoice.SetPriority(_priority);
			audioVoice.SetLoop(loop: false);
			if (_spatialBlend > 0f)
			{
				audioVoice.SetSpatial(enabled: true, _minDistance, _maxDistance, Rolloff, Math.Clamp(_spatialBlend, 0f, 1f));
				audioVoice.SetPosition(base.transform.position.x, base.transform.position.y, base.transform.position.z);
			}
			audioVoice.SetPaused(paused: false);
			_oneShots.Add((audioVoice, audioSound));
		}
	}

	private void StopOneShots()
	{
		foreach (var oneShot in _oneShots)
		{
			oneShot.voice.Stop();
		}
		_oneShots.Clear();
	}

	private void PruneOneShots()
	{
		for (int num = _oneShots.Count - 1; num >= 0; num--)
		{
			if (!_oneShots[num].voice.IsPlaying)
			{
				_oneShots.RemoveAt(num);
			}
		}
	}

	public void GetOutputData(float[] samples, int channel)
	{
		if (VideoFeed != null)
		{
			VideoFeed.GetAudioOutput(samples, channel);
			return;
		}
		Array.Clear(samples);
		if (_voice != null)
		{
			_voice.EnableOutputTap();
			_voice.ReadOutput(samples, channel);
			float num = (_mute ? 0f : _volume);
			for (int i = 0; i < samples.Length; i++)
			{
				samples[i] *= num;
			}
		}
	}

	public void Stop()
	{
		DetachFilters();
		StopOneShots();
		_voice?.Stop();
		_voice = null;
		_playingSound = null;
		_paused = false;
		_time = 0f;
		_startTimePending = false;
	}

	private void OnDestroy()
	{
		Stop();
		IgnoringListenerVolume.Remove(this);
	}

	public void UnPause()
	{
		if (_voice != null)
		{
			_voice.SetPaused(paused: false);
			_paused = false;
		}
	}

	public void Update()
	{
		if (_voice != null && _spatialBlend > 0f)
		{
			ApplySpatialState();
		}
	}

	private void Invalidate()
	{
		DetachFilters();
		_voice?.Stop();
		_voice = null;
		_playingSound = null;
		_startTimePending = false;
	}

	private void ApplyNonSpatialMode()
	{
		_voice.SetSpatial(enabled: false, _minDistance, _maxDistance, Rolloff, 0f);
		ApplyPan();
	}

	private void ApplyPan()
	{
		AudioClip audioClip = _clip;
		if ((object)audioClip == null)
		{
			return;
		}
		float x = (Math.Clamp(_panStereo, -1f, 1f) + 1f) * ((float)Math.PI / 4f);
		float num = MathF.Cos(x);
		float num2 = MathF.Sin(x);
		switch (audioClip.channels)
		{
		case 1:
		{
			IAudioVoice voice2 = _voice;
			InlineArray2<float> buffer2 = default(InlineArray2<float>);
			global::_003CPrivateImplementationDetails_003E.InlineArrayElementRef<InlineArray2<float>, float>(ref buffer2, 0) = num;
			global::_003CPrivateImplementationDetails_003E.InlineArrayElementRef<InlineArray2<float>, float>(ref buffer2, 1) = num2;
			voice2.SetMixMatrix(global::_003CPrivateImplementationDetails_003E.InlineArrayAsReadOnlySpan<InlineArray2<float>, float>(in buffer2, 2), 2, 1);
			return;
		}
		case 2:
			if (audioClip.ForceToMono)
			{
				IAudioVoice voice = _voice;
				InlineArray4<float> buffer = default(InlineArray4<float>);
				global::_003CPrivateImplementationDetails_003E.InlineArrayElementRef<InlineArray4<float>, float>(ref buffer, 0) = 0.5f * num;
				global::_003CPrivateImplementationDetails_003E.InlineArrayElementRef<InlineArray4<float>, float>(ref buffer, 1) = 0.5f * num;
				global::_003CPrivateImplementationDetails_003E.InlineArrayElementRef<InlineArray4<float>, float>(ref buffer, 2) = 0.5f * num2;
				global::_003CPrivateImplementationDetails_003E.InlineArrayElementRef<InlineArray4<float>, float>(ref buffer, 3) = 0.5f * num2;
				voice.SetMixMatrix(global::_003CPrivateImplementationDetails_003E.InlineArrayAsReadOnlySpan<InlineArray4<float>, float>(in buffer, 4), 2, 2);
				return;
			}
			break;
		}
		_voice.SetPan(_panStereo);
	}

	private void ApplySpatialMode()
	{
		_voice.SetSpatial(enabled: true, _minDistance, _maxDistance, Rolloff, Math.Clamp(_spatialBlend, 0f, 1f));
	}

	private void ApplySpatialState()
	{
		_voice.SetPosition(base.transform.position.x, base.transform.position.y, base.transform.position.z);
		_voice.SetDoppler(_dopplerLevel);
		if (_spread > 0f)
		{
			_voice.SetSpread(_spread);
			if (!_spreadWarned && AudioBackendAccess.Backend is MiniAudioBackend)
			{
				_spreadWarned = true;
				StubWarner.LogStub("ApplySpatialState", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\UnityStubs\\AudioSource.cs", 457);
			}
		}
	}

	internal void OnListenerVolumeChanged()
	{
		if (_voice != null)
		{
			ApplyVolume();
		}
	}

	private void ApplyVolume()
	{
		float num = _volume * (_clip?.NormalizeGain ?? 1f);
		if (_ignoreListenerVolume && AudioListener.volume > 0f)
		{
			num /= AudioListener.volume;
		}
		_voice.SetVolume(num);
	}

	private void AttachFilters()
	{
		AudioFilterBehaviour[] components;
		try
		{
			components = base.gameObject.GetComponents<AudioFilterBehaviour>();
		}
		catch
		{
			return;
		}
		for (int i = 0; i < components.Length; i++)
		{
			components[i].AttachToVoice(_voice);
		}
	}

	private void DetachFilters()
	{
		AudioFilterBehaviour[] components;
		try
		{
			components = base.gameObject.GetComponents<AudioFilterBehaviour>();
		}
		catch
		{
			return;
		}
		for (int i = 0; i < components.Length; i++)
		{
			components[i].DetachFromVoice();
		}
	}
}
