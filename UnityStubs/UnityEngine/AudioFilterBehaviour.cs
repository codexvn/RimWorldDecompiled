using UnityStubs.Internal;
using Waylith.Audio;

namespace UnityEngine;

public abstract class AudioFilterBehaviour : Behaviour
{
	private IAudioVoice _voice;

	private bool _warned;

	internal abstract bool Apply(IAudioVoice voice, bool enabled);

	internal void AttachToVoice(IAudioVoice voice)
	{
		DetachFromVoice();
		if (base.enabled && voice != null)
		{
			_voice = voice;
			Push();
		}
	}

	internal void DetachFromVoice()
	{
		if (_voice != null)
		{
			Apply(_voice, enabled: false);
			_voice = null;
		}
	}

	protected void Push()
	{
		if (_voice != null && !Apply(_voice, enabled: true) && !_warned)
		{
			_warned = true;
			StubWarner.LogStub(GetType().Name, "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\UnityStubs\\AudioFilterBehaviour.cs", 44);
		}
	}

	private void OnDisable()
	{
		DetachFromVoice();
	}

	private void OnDestroy()
	{
		DetachFromVoice();
	}
}
