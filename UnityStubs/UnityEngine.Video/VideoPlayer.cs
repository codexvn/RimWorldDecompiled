using System;
using System.IO;
using System.Runtime.CompilerServices;
using UnityStubs.Internal;
using Waylith.VideoDecode;

namespace UnityEngine.Video;

public class VideoPlayer : Behaviour
{
	public delegate void EventHandler(VideoPlayer source);

	public delegate void ErrorEventHandler(VideoPlayer source, string message);

	public delegate void FrameReadyEventHandler(VideoPlayer source, long frameIdx);

	private readonly Waylith.VideoDecode.VideoPlayer _player = new Waylith.VideoDecode.VideoPlayer();

	private string _url;

	private VideoClip _clip;

	private VideoSource _source;

	private VideoRenderMode _renderMode;

	private VideoAspectRatio _aspectRatio = VideoAspectRatio.FitHorizontally;

	private RenderTexture _targetTexture;

	private RenderTexture _ownTexture;

	private float _targetCameraAlpha = 1f;

	private bool _playOnAwake = true;

	private bool _waitForFirstFrame = true;

	private bool _sendFrameReadyEvents;

	private AudioSource _audioSource;

	private float _directVolume = 1f;

	private bool _directMute;

	private bool _audioTrackEnabled = true;

	[CompilerGenerated]
	private VideoAudioOutputMode _003CaudioOutputMode_003Ek__BackingField = VideoAudioOutputMode.Direct;

	public ushort audioTrackCount => _player.HasAudio ? ((ushort)1) : ((ushort)0);

	public uint height => (uint)_player.Height;

	public double length => _player.Length;

	public float playbackSpeed
	{
		get
		{
			return _player.PlaybackSpeed;
		}
		set
		{
			_player.PlaybackSpeed = value;
		}
	}

	public double time
	{
		get
		{
			return _player.Time;
		}
		set
		{
			_player.Seek(value);
		}
	}

	public uint width => (uint)_player.Width;

	public VideoRenderMode renderMode
	{
		set
		{
			_renderMode = value;
		}
	}

	public bool sendFrameReadyEvents
	{
		set
		{
			_sendFrameReadyEvents = value;
		}
	}

	public VideoSource source
	{
		set
		{
			_source = value;
		}
	}

	public VideoAspectRatio aspectRatio
	{
		get
		{
			return _aspectRatio;
		}
		set
		{
			_aspectRatio = value;
		}
	}

	public bool isLooping
	{
		get
		{
			return _player.Looping;
		}
		set
		{
			_player.Looping = value;
		}
	}

	public bool playOnAwake
	{
		get
		{
			return _playOnAwake;
		}
		set
		{
			_playOnAwake = value;
		}
	}

	public bool skipOnDrop
	{
		get
		{
			return _player.SkipOnDrop;
		}
		set
		{
			_player.SkipOnDrop = value;
		}
	}

	public bool waitForFirstFrame
	{
		get
		{
			return _waitForFirstFrame;
		}
		set
		{
			_waitForFirstFrame = value;
		}
	}

	public bool isPlaying => _player.IsPlaying;

	public bool isPrepared => _player.IsPrepared;

	public bool isPaused => _player.IsPaused;

	public long frame
	{
		get
		{
			return _player.Frame;
		}
		set
		{
			_player.SeekFrame(value);
		}
	}

	public ulong frameCount => (ulong)_player.FrameCount;

	public float frameRate => (float)_player.FrameRate;

	public float targetCameraAlpha
	{
		get
		{
			return _targetCameraAlpha;
		}
		set
		{
			_targetCameraAlpha = value;
		}
	}

	public RenderTexture targetTexture
	{
		get
		{
			return _targetTexture;
		}
		set
		{
			_targetTexture = value;
		}
	}

	public VideoAudioOutputMode audioOutputMode
	{
		[CompilerGenerated]
		get
		{
			return _003CaudioOutputMode_003Ek__BackingField;
		}
		set
		{
			if (value == VideoAudioOutputMode.APIOnly)
			{
				StubWarner.LogStub("audioOutputMode", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\UnityStubs\\VideoPlayer.cs", 120);
			}
			_003CaudioOutputMode_003Ek__BackingField = value;
			ApplyAudioSettings();
		}
	}

	public Texture texture
	{
		get
		{
			if (UsesTargetTexture)
			{
				return _targetTexture;
			}
			if (!_player.IsPrepared)
			{
				return null;
			}
			return _ownTexture;
		}
	}

	public string url
	{
		get
		{
			return _url;
		}
		set
		{
			_url = value;
			_source = VideoSource.Url;
			SourceChanged();
		}
	}

	public VideoClip clip
	{
		get
		{
			return _clip;
		}
		set
		{
			_clip = value;
			_source = VideoSource.VideoClip;
			SourceChanged();
		}
	}

	private bool UsesTargetTexture
	{
		get
		{
			if (_renderMode == VideoRenderMode.RenderTexture)
			{
				return _targetTexture != null;
			}
			return false;
		}
	}

	public event ErrorEventHandler errorReceived;

	public event FrameReadyEventHandler frameReady;

	public event EventHandler loopPointReached;

	public event EventHandler prepareCompleted;

	public VideoPlayer()
	{
		_player.TargetProvider = ResolveTarget;
		_player.Prepared += delegate
		{
			this.prepareCompleted?.Invoke(this);
		};
		_player.LoopPointReached += delegate
		{
			this.loopPointReached?.Invoke(this);
		};
		_player.ErrorReceived += delegate(string message)
		{
			this.errorReceived?.Invoke(this, "VideoPlayer " + message);
		};
		_player.FrameReady += delegate(long index)
		{
			if (_sendFrameReadyEvents)
			{
				this.frameReady?.Invoke(this, index);
			}
		};
	}

	public void EnableAudioTrack(ushort trackIndex, bool enabled)
	{
		if (trackIndex == 0)
		{
			_audioTrackEnabled = enabled;
		}
		else
		{
			StubWarner.LogStub("EnableAudioTrack", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\UnityStubs\\VideoPlayer.cs", 49);
		}
	}

	public bool GetDirectAudioMute(ushort trackIndex)
	{
		if (trackIndex == 0)
		{
			return _directMute;
		}
		return false;
	}

	public float GetDirectAudioVolume(ushort trackIndex)
	{
		if (trackIndex != 0)
		{
			return 0f;
		}
		return _directVolume;
	}

	public void Pause()
	{
		_player.Pause();
	}

	public void Play()
	{
		_player.Play(Time.realtimeSinceStartupAsDouble);
	}

	public void Prepare()
	{
		_player.Prepare();
	}

	public void SetDirectAudioMute(ushort trackIndex, bool mute)
	{
		if (trackIndex == 0)
		{
			_directMute = mute;
		}
		else
		{
			StubWarner.LogStub("SetDirectAudioMute", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\UnityStubs\\VideoPlayer.cs", 71);
		}
	}

	public void Stop()
	{
		_player.Stop();
	}

	public void SetTargetAudioSource(ushort trackIndex, AudioSource source)
	{
		if (trackIndex != 0)
		{
			StubWarner.LogStub("SetTargetAudioSource", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\UnityStubs\\VideoPlayer.cs", 79);
			return;
		}
		if (_audioSource != null && _audioSource.VideoFeed == _player)
		{
			_audioSource.VideoFeed = null;
		}
		_audioSource = source;
		if (source != null)
		{
			source.VideoFeed = _player;
		}
	}

	public void SetDirectAudioVolume(ushort trackIndex, float volume)
	{
		if (trackIndex == 0)
		{
			_directVolume = Mathf.Clamp01(volume);
		}
		else
		{
			StubWarner.LogStub("SetDirectAudioVolume", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\UnityStubs\\VideoPlayer.cs", 107);
		}
	}

	private void SourceChanged()
	{
		VideoMedia media = null;
		if (_source == VideoSource.Url && !string.IsNullOrEmpty(_url))
		{
			string path = (_url.StartsWith("file://", StringComparison.OrdinalIgnoreCase) ? new Uri(_url).LocalPath : _url);
			media = new VideoMedia
			{
				Path = path,
				Label = Path.GetFileName(path)
			};
		}
		else if (_source == VideoSource.VideoClip && _clip != null)
		{
			media = new VideoMedia
			{
				Data = _clip.Data,
				Label = _clip.name
			};
		}
		ApplyAudioSettings();
		_player.Media = media;
		StartIfPlayOnAwake();
	}

	private void StartIfPlayOnAwake()
	{
		if (_playOnAwake && _player.HasMedia && base.isActiveAndEnabled)
		{
			Play();
		}
	}

	private VideoTarget ResolveTarget()
	{
		if (UsesTargetTexture)
		{
			_targetTexture.Create();
			return new VideoTarget(_targetTexture.GpuTexture, _targetTexture.width, _targetTexture.height, (VideoAspect)_aspectRatio);
		}
		if (_ownTexture != null && (_ownTexture.width != _player.Width || _ownTexture.height != _player.Height))
		{
			_ownTexture.Release();
			_ownTexture = null;
		}
		if (_ownTexture == null)
		{
			_ownTexture = new RenderTexture(_player.Width, _player.Height, 0, RenderTextureFormat.ARGB32)
			{
				name = "VideoPlayer"
			};
			_ownTexture.Create();
		}
		return new VideoTarget(_ownTexture.GpuTexture, _ownTexture.width, _ownTexture.height, VideoAspect.Stretch);
	}

	private void Awake()
	{
		StartIfPlayOnAwake();
	}

	private void ApplyAudioSettings()
	{
		VideoAudioOutputMode videoAudioOutputMode = audioOutputMode;
		AudioSource audioSource = ((videoAudioOutputMode == VideoAudioOutputMode.AudioSource && _audioSource != null) ? _audioSource : null);
		_player.AudioEnabled = _audioTrackEnabled && (videoAudioOutputMode == VideoAudioOutputMode.Direct || audioSource != null);
		_player.AudioVolume = ((audioSource != null) ? audioSource.OutputVolume : _directVolume);
		_player.AudioMuted = ((audioSource != null) ? audioSource.Muted : _directMute);
		_player.AudioGroup = audioSource?.OutputGroup;
	}

	private void Update()
	{
		ApplyAudioSettings();
		_player.Update(Time.realtimeSinceStartupAsDouble);
	}

	private void OnDestroy()
	{
		SetTargetAudioSource(0, null);
		_player.Dispose();
		if (_ownTexture != null)
		{
			_ownTexture.Release();
			_ownTexture = null;
		}
	}
}
