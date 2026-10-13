using Waylith.Audio;

namespace UnityEngine;

public class AudioChorusFilter : AudioFilterBehaviour
{
	private float _dryMix = 0.5f;

	private float _wetMix1 = 0.5f;

	private float _wetMix2 = 0.5f;

	private float _wetMix3 = 0.5f;

	private float _delay = 40f;

	private float _rate = 0.8f;

	private float _depth = 0.03f;

	private float _feedback;

	public float dryMix
	{
		get
		{
			return _dryMix;
		}
		set
		{
			_dryMix = value;
		}
	}

	public float wetMix1
	{
		get
		{
			return _wetMix1;
		}
		set
		{
			_wetMix1 = value;
			Push();
		}
	}

	public float wetMix2
	{
		get
		{
			return _wetMix2;
		}
		set
		{
			_wetMix2 = value;
		}
	}

	public float wetMix3
	{
		get
		{
			return _wetMix3;
		}
		set
		{
			_wetMix3 = value;
		}
	}

	public float delay
	{
		get
		{
			return _delay;
		}
		set
		{
			_delay = value;
		}
	}

	public float rate
	{
		get
		{
			return _rate;
		}
		set
		{
			_rate = value;
			Push();
		}
	}

	public float depth
	{
		get
		{
			return _depth;
		}
		set
		{
			_depth = value;
			Push();
		}
	}

	public float feedback
	{
		get
		{
			return _feedback;
		}
		set
		{
			_feedback = value;
		}
	}

	internal override bool Apply(IAudioVoice voice, bool enabled)
	{
		return voice.SetChorus(enabled, Mathf.Clamp(_rate, 0f, 20f), Mathf.Clamp01(_depth), Mathf.Clamp01(_wetMix1));
	}
}
