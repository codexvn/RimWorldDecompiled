using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x020025A8 RID: 9640
	public class GravshipAudio
	{
		// Token: 0x0600D33A RID: 54074 RVA: 0x003E3634 File Offset: 0x003E1834
		private SoundDef[] GetRetroThrusterSounds()
		{
			return new SoundDef[]
			{
				SoundDefOf.Gravship_RetroThruster_Oneshot_01,
				SoundDefOf.Gravship_RetroThruster_Oneshot_02,
				SoundDefOf.Gravship_RetroThruster_Oneshot_03
			};
		}

		// Token: 0x0600D33B RID: 54075 RVA: 0x003E3654 File Offset: 0x003E1854
		public void BeginTakeoff()
		{
			this.active = true;
			if (!DebugViewSettings.playGravshipCutscenes)
			{
				return;
			}
			SoundDefOf.Gravship_Engine_Start.PlayOneShotOnCamera(null);
			this.InitialiseSustainers();
			this.engineLoop.FadeIn(1f);
		}

		// Token: 0x0600D33C RID: 54076 RVA: 0x003E3688 File Offset: 0x003E1888
		public void BeginLanding()
		{
			this.active = true;
			if (!DebugViewSettings.playGravshipCutscenes)
			{
				return;
			}
			this.InitialiseSustainers();
			this.landingServos = new GravshipAudio.TriggeredOneOff(SoundDefOf.Gravship_LandingServos);
			this.touchdown = new GravshipAudio.TriggeredOneOff(SoundDefOf.Gravship_Touchdown);
			this.engineStop = new GravshipAudio.TriggeredOneOff(SoundDefOf.Gravship_Engine_Stop);
			this.retroThrusterQueue.Clear();
			foreach (SoundDef def in this.GetRetroThrusterSounds().InRandomOrder(null))
			{
				this.retroThrusterQueue.Enqueue(new GravshipAudio.TriggeredOneOff(def));
			}
			this.engineLoop.FadeIn(0.5f);
			this.antigravLoop.FadeIn(0.5f);
			this.retroThrusterLoop.FadeIn(0.5f);
		}

		// Token: 0x0600D33D RID: 54077 RVA: 0x003E3768 File Offset: 0x003E1968
		public void InitialiseSustainers()
		{
			this.engineLoop = new GravshipAudio.ManagedSustainer(SoundDefOf.Gravship_Engine_Loop);
			this.antigravLoop = new GravshipAudio.ManagedSustainer(SoundDefOf.Gravship_Antigrav_Loop);
			this.retroThrusterLoop = new GravshipAudio.ManagedSustainer(SoundDefOf.Gravship_RetroThruster_Loop);
		}

		// Token: 0x0600D33E RID: 54078 RVA: 0x003E379C File Offset: 0x003E199C
		public void Update(float cutsceneTime, float progress, bool isTakeoff)
		{
			if (!this.active)
			{
				return;
			}
			GravshipAudio.ManagedSustainer managedSustainer = this.engineLoop;
			if (managedSustainer != null)
			{
				managedSustainer.Update();
			}
			GravshipAudio.ManagedSustainer managedSustainer2 = this.antigravLoop;
			if (managedSustainer2 != null)
			{
				managedSustainer2.Update();
			}
			GravshipAudio.ManagedSustainer managedSustainer3 = this.retroThrusterLoop;
			if (managedSustainer3 != null)
			{
				managedSustainer3.Update();
			}
			if (isTakeoff)
			{
				if (cutsceneTime > 1.5f)
				{
					GravshipAudio.ManagedSustainer managedSustainer4 = this.antigravLoop;
					if (managedSustainer4 != null)
					{
						managedSustainer4.FadeIn(0.1f);
					}
				}
			}
			else
			{
				if (progress > 0.15f)
				{
					GravshipAudio.ManagedSustainer managedSustainer5 = this.retroThrusterLoop;
					if (managedSustainer5 != null)
					{
						managedSustainer5.FadeIn(0f);
					}
					if (this.retroThrusterQueue.Count > 0)
					{
						GravshipAudio.TriggeredOneOff triggeredOneOff = this.retroThrusterQueue.Peek();
						if (triggeredOneOff.CurrentTimestamp > 0.5f)
						{
							this.retroThrusterQueue.Dequeue();
						}
						else
						{
							triggeredOneOff.Trigger();
						}
					}
				}
				if (progress > 0.5f)
				{
					GravshipAudio.TriggeredOneOff triggeredOneOff2 = this.landingServos;
					if (triggeredOneOff2 != null)
					{
						triggeredOneOff2.Trigger();
					}
				}
				if (progress > 0.8f)
				{
					GravshipAudio.TriggeredOneOff triggeredOneOff3 = this.touchdown;
					if (triggeredOneOff3 != null)
					{
						triggeredOneOff3.Trigger();
					}
					GravshipAudio.ManagedSustainer managedSustainer6 = this.retroThrusterLoop;
					if (managedSustainer6 != null)
					{
						managedSustainer6.FadeOut(0.5f);
					}
				}
				if (progress > 0.9f)
				{
					GravshipAudio.ManagedSustainer managedSustainer7 = this.engineLoop;
					if (managedSustainer7 != null)
					{
						managedSustainer7.FadeOut(1.2f);
					}
					GravshipAudio.ManagedSustainer managedSustainer8 = this.antigravLoop;
					if (managedSustainer8 != null)
					{
						managedSustainer8.FadeOut(1.2f);
					}
					GravshipAudio.TriggeredOneOff triggeredOneOff4 = this.engineStop;
					if (triggeredOneOff4 != null)
					{
						triggeredOneOff4.Trigger();
					}
				}
			}
			if (!Find.GravshipController.IsGravshipTravelling && !isTakeoff)
			{
				GravshipAudio.ManagedSustainer managedSustainer9 = this.engineLoop;
				if (managedSustainer9 == null || !managedSustainer9.Alive)
				{
					GravshipAudio.ManagedSustainer managedSustainer10 = this.antigravLoop;
					if (managedSustainer10 == null || !managedSustainer10.Alive)
					{
						GravshipAudio.TriggeredOneOff triggeredOneOff5 = this.engineStop;
						if (triggeredOneOff5 == null || triggeredOneOff5.Finished)
						{
							this.active = false;
						}
					}
				}
			}
		}

		// Token: 0x0600D33F RID: 54079 RVA: 0x003E394C File Offset: 0x003E1B4C
		public void EndTakeoff()
		{
			GravshipAudio.ManagedSustainer managedSustainer = this.engineLoop;
			if (managedSustainer != null)
			{
				managedSustainer.Stop();
			}
			GravshipAudio.ManagedSustainer managedSustainer2 = this.antigravLoop;
			if (managedSustainer2 != null)
			{
				managedSustainer2.Stop();
			}
			GravshipAudio.ManagedSustainer managedSustainer3 = this.retroThrusterLoop;
			if (managedSustainer3 != null)
			{
				managedSustainer3.Stop();
			}
			this.engineLoop = null;
			this.antigravLoop = null;
			this.retroThrusterLoop = null;
		}

		// Token: 0x04008F4D RID: 36685
		private bool active;

		// Token: 0x04008F4E RID: 36686
		private GravshipAudio.ManagedSustainer engineLoop;

		// Token: 0x04008F4F RID: 36687
		private GravshipAudio.ManagedSustainer antigravLoop;

		// Token: 0x04008F50 RID: 36688
		private GravshipAudio.ManagedSustainer retroThrusterLoop;

		// Token: 0x04008F51 RID: 36689
		private GravshipAudio.TriggeredOneOff landingServos;

		// Token: 0x04008F52 RID: 36690
		private GravshipAudio.TriggeredOneOff touchdown;

		// Token: 0x04008F53 RID: 36691
		private GravshipAudio.TriggeredOneOff engineStop;

		// Token: 0x04008F54 RID: 36692
		private Queue<GravshipAudio.TriggeredOneOff> retroThrusterQueue = new Queue<GravshipAudio.TriggeredOneOff>();

		// Token: 0x04008F55 RID: 36693
		private const float ProgressThrustersDyingDown = 0.9f;

		// Token: 0x04008F56 RID: 36694
		private const float ProgressDustCloudsVisible = 0.15f;

		// Token: 0x04008F57 RID: 36695
		private const float ProgressTouchdown = 0.8f;

		// Token: 0x04008F58 RID: 36696
		private const float ThrusterSFXInterval = 0.5f;

		// Token: 0x020025A9 RID: 9641
		private class TriggeredOneOff
		{
			// Token: 0x17002081 RID: 8321
			// (get) Token: 0x0600D341 RID: 54081 RVA: 0x003E39B4 File Offset: 0x003E1BB4
			public bool Finished
			{
				get
				{
					return this.played && Time.time > this.playedTime + this.def.Duration.max;
				}
			}

			// Token: 0x17002082 RID: 8322
			// (get) Token: 0x0600D342 RID: 54082 RVA: 0x003E39DE File Offset: 0x003E1BDE
			public float CurrentTimestamp
			{
				get
				{
					if (this.played)
					{
						return Time.time - this.playedTime;
					}
					return 0f;
				}
			}

			// Token: 0x0600D343 RID: 54083 RVA: 0x003E39FA File Offset: 0x003E1BFA
			public TriggeredOneOff(SoundDef def)
			{
				this.def = def;
			}

			// Token: 0x0600D344 RID: 54084 RVA: 0x003E3A09 File Offset: 0x003E1C09
			public void Trigger()
			{
				if (this.played)
				{
					return;
				}
				this.played = true;
				this.playedTime = Time.time;
				this.def.PlayOneShotOnCamera(null);
			}

			// Token: 0x04008F59 RID: 36697
			private SoundDef def;

			// Token: 0x04008F5A RID: 36698
			private bool played;

			// Token: 0x04008F5B RID: 36699
			private float playedTime;
		}

		// Token: 0x020025AA RID: 9642
		private class ManagedSustainer
		{
			// Token: 0x17002083 RID: 8323
			// (get) Token: 0x0600D345 RID: 54085 RVA: 0x003E3A32 File Offset: 0x003E1C32
			public Sustainer Sustainer
			{
				get
				{
					return this.sustainer;
				}
			}

			// Token: 0x17002084 RID: 8324
			// (get) Token: 0x0600D346 RID: 54086 RVA: 0x003E3A3A File Offset: 0x003E1C3A
			public bool Fading
			{
				get
				{
					return this.fading;
				}
			}

			// Token: 0x17002085 RID: 8325
			// (get) Token: 0x0600D347 RID: 54087 RVA: 0x003E3A42 File Offset: 0x003E1C42
			public bool Alive
			{
				get
				{
					return this.alive;
				}
			}

			// Token: 0x0600D348 RID: 54088 RVA: 0x003E3A4A File Offset: 0x003E1C4A
			public ManagedSustainer(SoundDef def)
			{
				this.sustainer = def.TrySpawnSustainer(SoundInfo.OnCamera(MaintenanceType.PerFrame));
				this.sustainer.info.volumeFactor = 0f;
			}

			// Token: 0x0600D349 RID: 54089 RVA: 0x003E3A80 File Offset: 0x003E1C80
			public void Update()
			{
				if (this.fading || this.alive)
				{
					this.sustainer.Maintain();
				}
				if (this.fading)
				{
					this.fadeTime += Time.deltaTime;
					this.sustainer.info.volumeFactor = Mathf.Lerp(this.fadeInitialVolume, this.fadeTarget, this.fadeTime / this.fadeDuration);
					if (this.fadeTime >= this.fadeDuration)
					{
						this.fading = false;
					}
				}
			}

			// Token: 0x0600D34A RID: 54090 RVA: 0x003E3B08 File Offset: 0x003E1D08
			private void Fade(float duration, float volume)
			{
				if (!this.alive)
				{
					return;
				}
				this.fading = true;
				this.fadeInitialVolume = this.sustainer.info.volumeFactor;
				this.fadeTarget = volume;
				this.fadeTime = 0f;
				this.fadeDuration = duration;
			}

			// Token: 0x0600D34B RID: 54091 RVA: 0x003E3B54 File Offset: 0x003E1D54
			public void FadeIn(float duration)
			{
				if (this.hasFadedIn)
				{
					return;
				}
				this.hasFadedIn = true;
				this.Fade(duration, 1f);
			}

			// Token: 0x0600D34C RID: 54092 RVA: 0x003E3B72 File Offset: 0x003E1D72
			public void FadeOut(float duration)
			{
				if (!this.hasFadedIn || this.hasFadedOut)
				{
					return;
				}
				this.hasFadedOut = true;
				this.Fade(duration, 0f);
				this.alive = false;
			}

			// Token: 0x0600D34D RID: 54093 RVA: 0x003E3B9F File Offset: 0x003E1D9F
			public void Stop()
			{
				if (!this.alive)
				{
					return;
				}
				this.alive = false;
				this.sustainer.End();
				this.sustainer = null;
			}

			// Token: 0x04008F5C RID: 36700
			private Sustainer sustainer;

			// Token: 0x04008F5D RID: 36701
			private bool alive = true;

			// Token: 0x04008F5E RID: 36702
			private bool hasFadedIn;

			// Token: 0x04008F5F RID: 36703
			private bool hasFadedOut;

			// Token: 0x04008F60 RID: 36704
			private bool fading;

			// Token: 0x04008F61 RID: 36705
			private float fadeTime;

			// Token: 0x04008F62 RID: 36706
			private float fadeDuration;

			// Token: 0x04008F63 RID: 36707
			private float fadeInitialVolume;

			// Token: 0x04008F64 RID: 36708
			private float fadeTarget;
		}
	}
}
