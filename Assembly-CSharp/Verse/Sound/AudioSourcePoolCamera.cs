using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse.Sound
{
	// Token: 0x02000E40 RID: 3648
	public class AudioSourcePoolCamera
	{
		// Token: 0x06005BF6 RID: 23542 RVA: 0x001D65B4 File Offset: 0x001D47B4
		public AudioSourcePoolCamera()
		{
			this.cameraSourcesContainer = new GameObject("OneShotSourcesCameraContainer");
			this.cameraSourcesContainer.transform.parent = Find.Camera.transform;
			this.cameraSourcesContainer.transform.localPosition = Vector3.zero;
			for (int i = 0; i < 16; i++)
			{
				AudioSource audioSource = AudioSourceMaker.NewAudioSourceOn(new GameObject("OneShotSourceCamera_" + i.ToString())
				{
					transform = 
					{
						parent = this.cameraSourcesContainer.transform,
						localPosition = Vector3.zero
					}
				});
				audioSource.bypassReverbZones = true;
				this.sourcesCamera.Add(audioSource);
			}
		}

		// Token: 0x06005BF7 RID: 23543 RVA: 0x001D6674 File Offset: 0x001D4874
		public AudioSource GetSourceCamera()
		{
			for (int i = 0; i < this.sourcesCamera.Count; i++)
			{
				AudioSource audioSource = this.sourcesCamera[i];
				if (!audioSource.isPlaying)
				{
					audioSource.clip = null;
					SoundFilterUtility.DisableAllFiltersOn(audioSource);
					return audioSource;
				}
			}
			return null;
		}

		// Token: 0x0400429F RID: 17055
		public GameObject cameraSourcesContainer;

		// Token: 0x040042A0 RID: 17056
		private List<AudioSource> sourcesCamera = new List<AudioSource>();

		// Token: 0x040042A1 RID: 17057
		private const int NumSourcesCamera = 16;
	}
}
