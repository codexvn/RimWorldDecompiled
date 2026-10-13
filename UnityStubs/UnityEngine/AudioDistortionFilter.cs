using Waylith.Audio;

namespace UnityEngine;

public class AudioDistortionFilter : AudioFilterBehaviour
{
	private float _distortionLevel = 0.5f;

	public float distortionLevel
	{
		get
		{
			return _distortionLevel;
		}
		set
		{
			_distortionLevel = value;
			Push();
		}
	}

	internal override bool Apply(IAudioVoice voice, bool enabled)
	{
		return voice.SetDistortion(enabled, Mathf.Clamp01(_distortionLevel));
	}
}
