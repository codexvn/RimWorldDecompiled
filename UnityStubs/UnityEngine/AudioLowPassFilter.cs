using Waylith.Audio;

namespace UnityEngine;

public class AudioLowPassFilter : AudioFilterBehaviour
{
	private float _cutoffFrequency = 5000f;

	private float _lowpassResonanceQ = 1f;

	public float cutoffFrequency
	{
		get
		{
			return _cutoffFrequency;
		}
		set
		{
			_cutoffFrequency = value;
			Push();
		}
	}

	public float lowpassResonanceQ
	{
		get
		{
			return _lowpassResonanceQ;
		}
		set
		{
			_lowpassResonanceQ = value;
			Push();
		}
	}

	internal override bool Apply(IAudioVoice voice, bool enabled)
	{
		return voice.SetLowPass(enabled, Mathf.Clamp(_cutoffFrequency, 10f, 22000f), Mathf.Clamp(_lowpassResonanceQ, 1f, 10f));
	}
}
