using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;

namespace Verse.Sound
{
	// Token: 0x02000E2F RID: 3631
	public abstract class Sample
	{
		// Token: 0x17000EA4 RID: 3748
		// (get) Token: 0x06005B94 RID: 23444 RVA: 0x001D4BBE File Offset: 0x001D2DBE
		public float AgeRealTime
		{
			get
			{
				return Time.realtimeSinceStartup - this.startRealTime;
			}
		}

		// Token: 0x17000EA5 RID: 3749
		// (get) Token: 0x06005B95 RID: 23445 RVA: 0x001D4BCC File Offset: 0x001D2DCC
		public int AgeTicks
		{
			get
			{
				if (Current.ProgramState == ProgramState.Playing)
				{
					return Find.TickManager.TicksGame - this.startTick;
				}
				return (int)(this.AgeRealTime * 60f);
			}
		}

		// Token: 0x17000EA6 RID: 3750
		// (get) Token: 0x06005B96 RID: 23446
		public abstract float ParentStartRealTime { get; }

		// Token: 0x17000EA7 RID: 3751
		// (get) Token: 0x06005B97 RID: 23447
		public abstract float ParentStartTick { get; }

		// Token: 0x17000EA8 RID: 3752
		// (get) Token: 0x06005B98 RID: 23448
		public abstract float ParentHashCode { get; }

		// Token: 0x17000EA9 RID: 3753
		// (get) Token: 0x06005B99 RID: 23449
		public abstract SoundParams ExternalParams { get; }

		// Token: 0x17000EAA RID: 3754
		// (get) Token: 0x06005B9A RID: 23450
		public abstract SoundInfo Info { get; }

		// Token: 0x17000EAB RID: 3755
		// (get) Token: 0x06005B9B RID: 23451 RVA: 0x001D4BF8 File Offset: 0x001D2DF8
		public Map Map
		{
			get
			{
				return this.Info.Maker.Map;
			}
		}

		// Token: 0x17000EAC RID: 3756
		// (get) Token: 0x06005B9C RID: 23452 RVA: 0x001D4C1B File Offset: 0x001D2E1B
		protected bool TestPlaying
		{
			get
			{
				return this.Info.testPlay;
			}
		}

		// Token: 0x17000EAD RID: 3757
		// (get) Token: 0x06005B9D RID: 23453 RVA: 0x001D4C28 File Offset: 0x001D2E28
		protected float MappedVolumeMultiplier
		{
			get
			{
				float num = 1f;
				foreach (float num2 in this.volumeInMappings.Values)
				{
					num *= num2;
				}
				return num;
			}
		}

		// Token: 0x17000EAE RID: 3758
		// (get) Token: 0x06005B9E RID: 23454 RVA: 0x001D4C84 File Offset: 0x001D2E84
		protected float ContextVolumeMultiplier
		{
			get
			{
				if (Screen_Credits.creditsShowing)
				{
					return 1f - Mathf.Clamp01((Time.realtimeSinceStartup - Screen_Credits.creationRealtime) / 3f);
				}
				if (!this.TestPlaying && !SoundDefHelper.CorrectContextNow(this.subDef.parentDef, this.Map))
				{
					return 0f;
				}
				if (this.subDef.parentDef.context != SoundContext.MapOnly && this.subDef.parentDef.context != SoundContext.WorldOnly)
				{
					return Prefs.VolumeUI;
				}
				if ((this.subDef.parentDef.context == SoundContext.MapOnly || this.subDef.parentDef.context == SoundContext.WorldOnly) && !this.IsAmbient)
				{
					return Prefs.VolumeGame;
				}
				return 1f;
			}
		}

		// Token: 0x17000EAF RID: 3759
		// (get) Token: 0x06005B9F RID: 23455 RVA: 0x001D4D40 File Offset: 0x001D2F40
		protected virtual float Volume
		{
			get
			{
				if (this.subDef.muteWhenPaused && Current.ProgramState == ProgramState.Playing && Find.TickManager.Paused && !this.TestPlaying)
				{
					return 0f;
				}
				return this.resolvedVolume * this.Info.volumeFactor * this.MappedVolumeMultiplier * this.ContextVolumeMultiplier;
			}
		}

		// Token: 0x17000EB0 RID: 3760
		// (get) Token: 0x06005BA0 RID: 23456 RVA: 0x001D4D9C File Offset: 0x001D2F9C
		public float SanitizedVolume
		{
			get
			{
				return AudioSourceUtility.GetSanitizedVolume(this.Volume, this.subDef.parentDef);
			}
		}

		// Token: 0x17000EB1 RID: 3761
		// (get) Token: 0x06005BA1 RID: 23457 RVA: 0x001D4DB4 File Offset: 0x001D2FB4
		public bool IsAmbient
		{
			get
			{
				foreach (SubSoundDef subSoundDef in this.subDef.parentDef.subSounds)
				{
					using (List<SoundParameterMapping>.Enumerator enumerator2 = subSoundDef.paramMappings.GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							if (enumerator2.Current.inParam is SoundParamSource_AmbientVolume)
							{
								return true;
							}
						}
					}
				}
				return false;
			}
		}

		// Token: 0x17000EB2 RID: 3762
		// (get) Token: 0x06005BA2 RID: 23458 RVA: 0x001D4E58 File Offset: 0x001D3058
		protected virtual float Pitch
		{
			get
			{
				float num = this.resolvedPitch * this.Info.pitchFactor;
				if (this.subDef.tempoAffectedByGameSpeed && Current.ProgramState == ProgramState.Playing && !this.TestPlaying && !Find.TickManager.Paused)
				{
					num *= Find.TickManager.TickRateMultiplier;
				}
				return num;
			}
		}

		// Token: 0x17000EB3 RID: 3763
		// (get) Token: 0x06005BA3 RID: 23459 RVA: 0x001D4EAF File Offset: 0x001D30AF
		public float SanitizedPitch
		{
			get
			{
				return AudioSourceUtility.GetSanitizedPitch(this.Pitch, this.subDef.parentDef);
			}
		}

		// Token: 0x06005BA4 RID: 23460 RVA: 0x001D4EC8 File Offset: 0x001D30C8
		public Sample(SubSoundDef def)
		{
			this.subDef = def;
			this.resolvedVolume = def.RandomizedVolume();
			this.resolvedPitch = def.pitchRange.RandomInRange;
			this.startRealTime = Time.realtimeSinceStartup;
			if (Current.ProgramState == ProgramState.Playing)
			{
				this.startTick = Find.TickManager.TicksGame;
			}
			else
			{
				this.startTick = 0;
			}
			foreach (SoundParamTarget_Volume key in (from m in this.subDef.paramMappings
			select m.outParam).OfType<SoundParamTarget_Volume>())
			{
				this.volumeInMappings.Add(key, 0f);
			}
		}

		// Token: 0x06005BA5 RID: 23461 RVA: 0x001D4FB0 File Offset: 0x001D31B0
		public virtual void Update()
		{
			this.source.pitch = this.SanitizedPitch;
			this.ApplyMappedParameters();
			this.source.volume = this.SanitizedVolume;
			if (this.source.volume < 0.001f)
			{
				this.source.mute = true;
			}
			else
			{
				this.source.mute = false;
			}
			if (this.subDef.tempoAffectedByGameSpeed && !this.TestPlaying)
			{
				if (Current.ProgramState == ProgramState.Playing && Find.TickManager.Paused)
				{
					if (this.source.isPlaying)
					{
						this.source.Pause();
						return;
					}
				}
				else if (!this.source.isPlaying)
				{
					this.source.UnPause();
				}
			}
		}

		// Token: 0x06005BA6 RID: 23462 RVA: 0x001D506C File Offset: 0x001D326C
		public void ApplyMappedParameters()
		{
			for (int i = 0; i < this.subDef.paramMappings.Count; i++)
			{
				SoundParameterMapping soundParameterMapping = this.subDef.paramMappings[i];
				if (soundParameterMapping.paramUpdateMode != SoundParamUpdateMode.OncePerSample || !this.mappingsApplied)
				{
					soundParameterMapping.Apply(this);
				}
			}
			this.mappingsApplied = true;
		}

		// Token: 0x06005BA7 RID: 23463 RVA: 0x001D50C5 File Offset: 0x001D32C5
		public void SignalMappedVolume(float value, SoundParamTarget sourceParam)
		{
			this.volumeInMappings[sourceParam] = value;
		}

		// Token: 0x06005BA8 RID: 23464 RVA: 0x001D50D4 File Offset: 0x001D32D4
		public virtual void SampleCleanup()
		{
			for (int i = 0; i < this.subDef.paramMappings.Count; i++)
			{
				SoundParameterMapping soundParameterMapping = this.subDef.paramMappings[i];
				if (soundParameterMapping.curve.HasView)
				{
					soundParameterMapping.curve.View.ClearDebugInputFrom(this);
				}
			}
		}

		// Token: 0x06005BA9 RID: 23465 RVA: 0x001D512C File Offset: 0x001D332C
		public override string ToString()
		{
			return string.Format("Sample_{0} volume={1} pitch={2} at {3}", new object[]
			{
				this.subDef.name,
				this.source.volume,
				this.source.pitch,
				this.source.transform.position.ToIntVec3()
			});
		}

		// Token: 0x06005BAA RID: 23466 RVA: 0x001D519A File Offset: 0x001D339A
		public override int GetHashCode()
		{
			return Gen.HashCombine<SubSoundDef>(this.startRealTime.GetHashCode(), this.subDef);
		}

		// Token: 0x04004272 RID: 17010
		public SubSoundDef subDef;

		// Token: 0x04004273 RID: 17011
		public AudioSource source;

		// Token: 0x04004274 RID: 17012
		public float startRealTime;

		// Token: 0x04004275 RID: 17013
		public int startTick;

		// Token: 0x04004276 RID: 17014
		public float resolvedVolume;

		// Token: 0x04004277 RID: 17015
		public float resolvedPitch;

		// Token: 0x04004278 RID: 17016
		private bool mappingsApplied;

		// Token: 0x04004279 RID: 17017
		private Dictionary<SoundParamTarget, float> volumeInMappings = new Dictionary<SoundParamTarget, float>();
	}
}
