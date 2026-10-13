using System;
using UnityEngine;

namespace Verse.Sound
{
	// Token: 0x02000E3E RID: 3646
	public static class AudioSourceMaker
	{
		// Token: 0x06005BF3 RID: 23539 RVA: 0x001D6518 File Offset: 0x001D4718
		public static AudioSource NewAudioSourceOn(GameObject go)
		{
			if (go.GetComponent<AudioSource>() != null)
			{
				Log.Warning("Adding audio source on " + ((go != null) ? go.ToString() : null) + " that already has one.");
				return go.GetComponent<AudioSource>();
			}
			AudioSource audioSource = go.AddComponent<AudioSource>();
			audioSource.rolloffMode = AudioRolloffMode.Linear;
			audioSource.dopplerLevel = 0f;
			audioSource.playOnAwake = false;
			return audioSource;
		}

		// Token: 0x0400429C RID: 17052
		private const AudioRolloffMode WorldRolloffMode = AudioRolloffMode.Linear;
	}
}
