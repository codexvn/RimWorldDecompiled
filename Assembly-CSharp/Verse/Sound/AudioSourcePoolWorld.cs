using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse.Sound
{
	// Token: 0x02000E41 RID: 3649
	public class AudioSourcePoolWorld
	{
		// Token: 0x06005BF8 RID: 23544 RVA: 0x001D66BC File Offset: 0x001D48BC
		public AudioSourcePoolWorld()
		{
			GameObject gameObject = new GameObject("OneShotSourcesWorldContainer");
			gameObject.transform.position = Vector3.zero;
			for (int i = 0; i < 32; i++)
			{
				GameObject gameObject2 = new GameObject("OneShotSource_" + i.ToString());
				gameObject2.transform.parent = gameObject.transform;
				gameObject2.transform.localPosition = Vector3.zero;
				this.sourcesWorld.Add(AudioSourceMaker.NewAudioSourceOn(gameObject2));
			}
		}

		// Token: 0x06005BF9 RID: 23545 RVA: 0x001D674C File Offset: 0x001D494C
		public AudioSource GetSourceWorld()
		{
			foreach (AudioSource audioSource in this.sourcesWorld)
			{
				if (!audioSource.isPlaying)
				{
					SoundFilterUtility.DisableAllFiltersOn(audioSource);
					return audioSource;
				}
			}
			return null;
		}

		// Token: 0x040042A2 RID: 17058
		private List<AudioSource> sourcesWorld = new List<AudioSource>();

		// Token: 0x040042A3 RID: 17059
		private const int NumSourcesWorld = 32;
	}
}
