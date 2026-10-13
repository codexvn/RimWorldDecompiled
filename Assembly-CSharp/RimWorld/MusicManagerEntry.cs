using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02002901 RID: 10497
	public class MusicManagerEntry
	{
		// Token: 0x170022D5 RID: 8917
		// (get) Token: 0x0600E587 RID: 58759 RVA: 0x0042F773 File Offset: 0x0042D973
		private float CurVolume
		{
			get
			{
				return Prefs.VolumeMusic * SongDefOf.EntrySong.volume;
			}
		}

		// Token: 0x170022D6 RID: 8918
		// (get) Token: 0x0600E588 RID: 58760 RVA: 0x0042F785 File Offset: 0x0042D985
		public float CurSanitizedVolume
		{
			get
			{
				return AudioSourceUtility.GetSanitizedVolume(this.CurVolume, "MusicManagerEntry");
			}
		}

		// Token: 0x0600E589 RID: 58761 RVA: 0x0042F798 File Offset: 0x0042D998
		public void MusicManagerEntryUpdate()
		{
			if (this.audioSource == null || !this.audioSource.isPlaying)
			{
				this.StartPlaying();
			}
			float curSanitizedVolume = this.CurSanitizedVolume;
			if (Time.frameCount > this.silentTillFrame)
			{
				this.silenceMultiplier = Mathf.Clamp01(this.silenceMultiplier + 1.75f * Time.deltaTime);
			}
			else if (Time.frameCount <= this.silentTillFrame)
			{
				this.silenceMultiplier = Mathf.Clamp01(this.silenceMultiplier - 1.75f * Time.deltaTime);
			}
			this.audioSource.volume = curSanitizedVolume * this.silenceMultiplier;
		}

		// Token: 0x0600E58A RID: 58762 RVA: 0x0042F838 File Offset: 0x0042DA38
		private void StartPlaying()
		{
			if (this.audioSource != null && !this.audioSource.isPlaying)
			{
				this.audioSource.Play();
				return;
			}
			if (GameObject.Find("MusicAudioSourceDummy") != null)
			{
				Log.Error("MusicManagerEntry did StartPlaying but there is already a music source GameObject.");
				return;
			}
			this.audioSource = new GameObject("MusicAudioSourceDummy")
			{
				transform = 
				{
					parent = Camera.main.transform
				}
			}.AddComponent<AudioSource>();
			this.audioSource.bypassEffects = true;
			this.audioSource.bypassListenerEffects = true;
			this.audioSource.bypassReverbZones = true;
			this.audioSource.priority = 0;
			this.audioSource.clip = SongDefOf.EntrySong.clip;
			this.audioSource.volume = this.CurSanitizedVolume;
			this.audioSource.loop = true;
			this.audioSource.spatialBlend = 0f;
			this.audioSource.Play();
		}

		// Token: 0x0600E58B RID: 58763 RVA: 0x0042F932 File Offset: 0x0042DB32
		public void MaintainSilence()
		{
			this.silentTillFrame = Time.frameCount + 1;
		}

		// Token: 0x0400998B RID: 39307
		private AudioSource audioSource;

		// Token: 0x0400998C RID: 39308
		private int silentTillFrame = -1;

		// Token: 0x0400998D RID: 39309
		private float silenceMultiplier = 1f;

		// Token: 0x0400998E RID: 39310
		private const string SourceGameObjectName = "MusicAudioSourceDummy";

		// Token: 0x0400998F RID: 39311
		private const float SilenceMultiplierChangePerSecond = 1.75f;
	}
}
