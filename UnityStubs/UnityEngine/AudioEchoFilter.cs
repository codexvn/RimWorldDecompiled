using Waylith.Audio;

namespace UnityEngine;

public class AudioEchoFilter : AudioFilterBehaviour
{
	private float _delay = 500f;

	private float _decayRatio = 0.5f;

	private float _wetMix = 1f;

	private float _dryMix = 1f;

	public float delay
	{
		get
		{
			return _delay;
		}
		set
		{
			_delay = value;
			Push();
		}
	}

	public float decayRatio
	{
		get
		{
			return _decayRatio;
		}
		set
		{
			_decayRatio = value;
			Push();
		}
	}

	public float wetMix
	{
		get
		{
			return _wetMix;
		}
		set
		{
			_wetMix = value;
			Push();
		}
	}

	public float dryMix
	{
		get
		{
			return _dryMix;
		}
		set
		{
			_dryMix = value;
			Push();
		}
	}

	internal override bool Apply(IAudioVoice voice, bool enabled)
	{
		return voice.SetEcho(enabled, Mathf.Clamp(_delay, 10f, 5000f), Mathf.Clamp01(_decayRatio), _wetMix, _dryMix);
	}
}
