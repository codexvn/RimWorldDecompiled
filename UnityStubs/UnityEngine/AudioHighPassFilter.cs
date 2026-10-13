using Waylith.Audio;

namespace UnityEngine;

public class AudioHighPassFilter : AudioFilterBehaviour
{
	private float _cutoffFrequency = 5000f;

	private float _highpassResonanceQ = 1f;

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

	public float highpassResonanceQ
	{
		get
		{
			return _highpassResonanceQ;
		}
		set
		{
			_highpassResonanceQ = value;
			Push();
		}
	}

	internal override bool Apply(IAudioVoice voice, bool enabled)
	{
		return voice.SetHighPass(enabled, Mathf.Clamp(_cutoffFrequency, 10f, 22000f), Mathf.Clamp(_highpassResonanceQ, 1f, 10f));
	}
}
