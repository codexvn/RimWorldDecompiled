using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LudeonTK;
using RimWorld.Planet;
using UnityEngine;
using Verse;
using Verse.AI.Group;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02002902 RID: 10498
	public class MusicManagerPlay
	{
		// Token: 0x170022D7 RID: 8919
		// (get) Token: 0x0600E58D RID: 58765 RVA: 0x0042F95B File Offset: 0x0042DB5B
		public MusicManagerPlay.MusicManagerState State
		{
			get
			{
				return this.state;
			}
		}

		// Token: 0x170022D8 RID: 8920
		// (get) Token: 0x0600E58E RID: 58766 RVA: 0x0042F963 File Offset: 0x0042DB63
		// (set) Token: 0x0600E58F RID: 58767 RVA: 0x0042F96B File Offset: 0x0042DB6B
		public bool IsPlaying { get; private set; }

		// Token: 0x170022D9 RID: 8921
		// (get) Token: 0x0600E590 RID: 58768 RVA: 0x0042F974 File Offset: 0x0042DB74
		public MusicTransition TriggeredTransition
		{
			get
			{
				return this.triggeredTransition;
			}
		}

		// Token: 0x170022DA RID: 8922
		// (get) Token: 0x0600E591 RID: 58769 RVA: 0x0042F97C File Offset: 0x0042DB7C
		public MusicSequenceWorker MusicSequenceWorker
		{
			get
			{
				return this.sequenceWorker;
			}
		}

		// Token: 0x170022DB RID: 8923
		// (get) Token: 0x0600E592 RID: 58770 RVA: 0x0042F984 File Offset: 0x0042DB84
		public SongDef CurrentSong
		{
			get
			{
				return this.currentSong;
			}
		}

		// Token: 0x170022DC RID: 8924
		// (get) Token: 0x0600E593 RID: 58771 RVA: 0x0042F98C File Offset: 0x0042DB8C
		public float FadeoutPercent
		{
			get
			{
				return 1f - this.fadeoutFactor;
			}
		}

		// Token: 0x170022DD RID: 8925
		// (get) Token: 0x0600E594 RID: 58772 RVA: 0x0042F99A File Offset: 0x0042DB9A
		public float FadeInPercent
		{
			get
			{
				return this.fadeinFactor;
			}
		}

		// Token: 0x170022DE RID: 8926
		// (get) Token: 0x0600E595 RID: 58773 RVA: 0x0042F9A4 File Offset: 0x0042DBA4
		public float PausedVolumeFactor
		{
			get
			{
				MusicSequenceWorker musicSequenceWorker = this.sequenceWorker;
				if (musicSequenceWorker != null && musicSequenceWorker.def.pausedVolumeFactor != null)
				{
					return Mathf.Lerp(this.sequenceWorker.def.pausedVolumeFactor.Value, 1f, this.pausedVolumeFactor);
				}
				return 1f;
			}
		}

		// Token: 0x170022DF RID: 8927
		// (get) Token: 0x0600E596 RID: 58774 RVA: 0x0042F9FA File Offset: 0x0042DBFA
		public float FadeoutDuration
		{
			get
			{
				return this.fadeoutDuration;
			}
		}

		// Token: 0x170022E0 RID: 8928
		// (get) Token: 0x0600E597 RID: 58775 RVA: 0x0042FA02 File Offset: 0x0042DC02
		public float NextSongTimer
		{
			get
			{
				return this.nextSongStartTime - Time.time;
			}
		}

		// Token: 0x170022E1 RID: 8929
		// (get) Token: 0x0600E598 RID: 58776 RVA: 0x0042FA10 File Offset: 0x0042DC10
		private float CurTime
		{
			get
			{
				return Time.time;
			}
		}

		// Token: 0x170022E2 RID: 8930
		// (get) Token: 0x0600E599 RID: 58777 RVA: 0x0042FA17 File Offset: 0x0042DC17
		public float CurSanitizedVolume
		{
			get
			{
				return AudioSourceUtility.GetSanitizedVolume(this.CurVolume, "MusicManagerPlay");
			}
		}

		// Token: 0x170022E3 RID: 8931
		// (get) Token: 0x0600E59A RID: 58778 RVA: 0x0042FA29 File Offset: 0x0042DC29
		// (set) Token: 0x0600E59B RID: 58779 RVA: 0x0042FA31 File Offset: 0x0042DC31
		public bool OverrideDangerMode { get; set; }

		// Token: 0x170022E4 RID: 8932
		// (get) Token: 0x0600E59C RID: 58780 RVA: 0x0042FA3A File Offset: 0x0042DC3A
		public float SongTime
		{
			get
			{
				return this.audioSource.time;
			}
		}

		// Token: 0x170022E5 RID: 8933
		// (get) Token: 0x0600E59D RID: 58781 RVA: 0x0042FA47 File Offset: 0x0042DC47
		public float SongDuration
		{
			get
			{
				if (!(this.audioSource.clip != null))
				{
					return 0f;
				}
				return this.audioSource.clip.length;
			}
		}

		// Token: 0x170022E6 RID: 8934
		// (get) Token: 0x0600E59E RID: 58782 RVA: 0x0042FA74 File Offset: 0x0042DC74
		public bool DangerMusicMode
		{
			get
			{
				if (this.OverrideDangerMode)
				{
					return true;
				}
				if (Find.Scenario.OverrideDangerMusic)
				{
					return true;
				}
				List<Map> maps = Find.Maps;
				for (int i = 0; i < maps.Count; i++)
				{
					if (maps[i].dangerWatcher.DangerRating == StoryDanger.High)
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x170022E7 RID: 8935
		// (get) Token: 0x0600E59F RID: 58783 RVA: 0x0042FAC8 File Offset: 0x0042DCC8
		private float CurVolume
		{
			get
			{
				float num;
				if (!this.ignorePrefsVolumeThisSong)
				{
					MusicSequenceWorker musicSequenceWorker = this.sequenceWorker;
					num = ((((musicSequenceWorker != null) ? musicSequenceWorker.def.overrideVolume : null) * Prefs.VolumeMaster) ?? Prefs.VolumeMusic);
				}
				else
				{
					num = Prefs.VolumeMaster;
				}
				float num2 = num;
				if (this.currentSong == null)
				{
					return num2;
				}
				return this.currentSong.volume * num2 * this.fadeoutFactor * this.fadeinFactor * this.musicOverridenFadeFactor * this.PausedVolumeFactor;
			}
		}

		// Token: 0x0600E5A0 RID: 58784 RVA: 0x0042FB7A File Offset: 0x0042DD7A
		public void ForceFadeoutAndSilenceFor(float time, float fadeoutTime = 10f, bool preventDangerTransition = false)
		{
			this.StopAndFadeoutSequence(fadeoutTime);
			this.nextSongStartTime = this.CurTime + time;
			this.preventDangerTransitionCheck = preventDangerTransition;
		}

		// Token: 0x0600E5A1 RID: 58785 RVA: 0x0042FB98 File Offset: 0x0042DD98
		public void ForceSilenceFor(float time)
		{
			this.Stop();
			this.nextSongStartTime = this.CurTime + time;
		}

		// Token: 0x0600E5A2 RID: 58786 RVA: 0x0042FBB0 File Offset: 0x0042DDB0
		public void MusicUpdate()
		{
			if (!this.gameObjectCreated)
			{
				this.InitializeMusicManager();
			}
			this.UpdateSubtleAmbienceSoundVolumeMultiplier();
			if (this.disabled)
			{
				return;
			}
			TickManager tickManager = Find.TickManager;
			if (tickManager.Paused != this.isPaused)
			{
				this.isPaused = tickManager.Paused;
				this.pauseToggledTime = Time.time;
				this.pausedVolumeFactorAtToggle = this.pausedVolumeFactor;
			}
			float t = Mathf.Clamp01((Time.time - this.pauseToggledTime) / 0.5f);
			this.pausedVolumeFactor = Mathf.Clamp01(Mathf.Lerp(this.pausedVolumeFactorAtToggle, (float)(this.isPaused ? 0 : 1), t));
			if (this.songWasForced)
			{
				this.state = MusicManagerPlay.MusicManagerState.Normal;
				this.fadeoutFactor = 1f;
			}
			if (this.CurTime >= this.songEndTime)
			{
				if (this.loopWhileFading)
				{
					this.PlaySong(this.CurrentSong, false, false);
				}
				else
				{
					this.IsPlaying = false;
				}
			}
			if (this.IsPlaying && this.state == MusicManagerPlay.MusicManagerState.Normal && !this.songWasForced && ((this.DangerMusicMode && !this.currentSong.tense) || (!this.DangerMusicMode && this.currentSong.tense)))
			{
				this.StartFadeout(10f);
			}
			this.audioSource.volume = this.CurSanitizedVolume;
			if (this.state == MusicManagerPlay.MusicManagerState.Sequence)
			{
				this.UpdateSequenceState();
			}
			if (this.state != MusicManagerPlay.MusicManagerState.Fadeout)
			{
				this.CheckTransitionInterval();
			}
			if (this.state == MusicManagerPlay.MusicManagerState.Fadeout)
			{
				this.UpdateFadeout();
			}
			if (this.IsPlaying)
			{
				this.UpdateMusicFadeout();
				return;
			}
			if (!this.preventDangerTransitionCheck && this.DangerMusicMode && this.nextSongStartTime > this.CurTime + MusicManagerPlay.SongIntervalTension.max)
			{
				this.nextSongStartTime = this.CurTime + MusicManagerPlay.SongIntervalTension.RandomInRange;
			}
			if (this.nextSongStartTime < this.CurTime - 5f)
			{
				float num = this.DangerMusicMode ? MusicManagerPlay.SongIntervalTension.RandomInRange : MusicManagerPlay.SongIntervalRelax.RandomInRange;
				this.nextSongStartTime = this.CurTime + num;
			}
			if (this.CurTime >= this.nextSongStartTime)
			{
				this.StartNewSong();
			}
		}

		// Token: 0x0600E5A3 RID: 58787 RVA: 0x0042FDCC File Offset: 0x0042DFCC
		private void CheckTransitionInterval()
		{
			if (!GenTicks.IsTickInterval(60) && this.initalized)
			{
				return;
			}
			this.initalized = true;
			this.CheckTransitions();
		}

		// Token: 0x0600E5A4 RID: 58788 RVA: 0x0042FDF0 File Offset: 0x0042DFF0
		public void CheckTransitions()
		{
			bool flag = this.sequenceWorker != null && this.sequenceWorker.def.useTransitionForLifetime && !this.manuallyTriggered;
			bool flag2 = this.sequenceWorker == null || this.sequenceWorker.CanBeInterrupted();
			MusicTransition musicTransition = null;
			foreach (MusicTransition musicTransition2 in this.transitions)
			{
				if (musicTransition2 == this.triggeredTransition)
				{
					if (musicTransition == null || musicTransition2.def.priority >= musicTransition.def.priority)
					{
						musicTransition = musicTransition2;
					}
				}
				else if ((flag2 || musicTransition2.def.overridesInterruptions) && (this.triggeredTransition == null || this.triggeredTransition.def.priority <= musicTransition2.def.priority) && musicTransition2.IsTransitionSatisfied() && (musicTransition == null || musicTransition2.def.priority > musicTransition.def.priority))
				{
					musicTransition = musicTransition2;
				}
			}
			if (musicTransition != null && musicTransition != this.triggeredTransition)
			{
				this.triggeredTransition = musicTransition;
				this.StartSequence(musicTransition.def.sequence);
				return;
			}
			if (this.triggeredTransition != null && flag && this.sequenceWorker.MinTimeSatisfied() && !this.triggeredTransition.IsTransitionSatisfied())
			{
				this.StopAndFadeoutSequence(10f);
			}
		}

		// Token: 0x0600E5A5 RID: 58789 RVA: 0x0042FF64 File Offset: 0x0042E164
		private void UpdateSequenceState()
		{
			if (this.sequenceWorker.def.nextSequence != null && this.sequenceWorker.ShouldTransition())
			{
				this.StartSequence(this.sequenceWorker.def.nextSequence);
				return;
			}
			if (this.sequenceWorker.ShouldEnd())
			{
				this.StopAndFadeoutSequence(10f);
				return;
			}
			if (!this.IsPlaying)
			{
				if (this.sequenceWorker.ShouldLoop())
				{
					if (!(this.sequenceWorker.def.loopDelayRange != FloatRange.Zero))
					{
						MusicSequenceWorker musicSequenceWorker = this.sequenceWorker;
						int timesLooped = musicSequenceWorker.timesLooped;
						musicSequenceWorker.timesLooped = timesLooped + 1;
						this.PlayNextSongInSequence();
						return;
					}
					if (this.nextSongStartTime < this.CurTime - 5f)
					{
						this.nextSongStartTime = this.CurTime + this.sequenceWorker.def.loopDelayRange.RandomInRange;
						return;
					}
					if (this.CurTime >= this.nextSongStartTime)
					{
						MusicSequenceWorker musicSequenceWorker2 = this.sequenceWorker;
						int timesLooped = musicSequenceWorker2.timesLooped;
						musicSequenceWorker2.timesLooped = timesLooped + 1;
						this.PlayNextSongInSequence();
						return;
					}
				}
				else
				{
					this.Stop();
				}
			}
		}

		// Token: 0x0600E5A6 RID: 58790 RVA: 0x0043007C File Offset: 0x0042E27C
		private void StopAndFadeoutSequence(float duration = 10f)
		{
			if (this.sequenceWorker != null)
			{
				if (this.sequenceWorker.def.overrideFadeout != null)
				{
					duration = this.sequenceWorker.def.overrideFadeout.Value;
				}
				if (this.sequenceWorker.def.loopFadeout)
				{
					this.loopWhileFading = true;
				}
			}
			this.sequenceWorker = null;
			this.triggeredTransition = null;
			this.manuallyTriggered = false;
			this.StartFadeout(duration);
		}

		// Token: 0x0600E5A7 RID: 58791 RVA: 0x004300F4 File Offset: 0x0042E2F4
		public void Stop()
		{
			if (this.audioSource != null)
			{
				this.audioSource.Stop();
			}
			this.sequenceWorker = null;
			this.triggeredTransition = null;
			this.IsPlaying = false;
			this.loopWhileFading = false;
			this.manuallyTriggered = false;
			this.state = MusicManagerPlay.MusicManagerState.Normal;
			float num = this.DangerMusicMode ? MusicManagerPlay.SongIntervalTension.RandomInRange : MusicManagerPlay.SongIntervalRelax.RandomInRange;
			this.nextSongStartTime = this.CurTime + num;
		}

		// Token: 0x0600E5A8 RID: 58792 RVA: 0x00430178 File Offset: 0x0042E378
		private void StartSequence(MusicSequenceDef def)
		{
			float curVolume = this.CurVolume;
			this.sequenceWorker = (MusicSequenceWorker)Activator.CreateInstance(def.workerType);
			this.sequenceWorker.InitializeWorker(def, this);
			if (this.sequenceWorker.ShouldEnd())
			{
				this.Stop();
				return;
			}
			if (this.IsPlaying && this.sequenceWorker.def.fadeoutLastSongDuration > 0f && curVolume > 0f)
			{
				this.StartFadeout(this.sequenceWorker.def.fadeoutLastSongDuration);
				return;
			}
			this.state = MusicManagerPlay.MusicManagerState.Sequence;
			this.PlayNextSongInSequence();
		}

		// Token: 0x0600E5A9 RID: 58793 RVA: 0x00430210 File Offset: 0x0042E410
		private void UpdateMusicFadeout()
		{
			if (this.fadeInStarted != 0f)
			{
				this.fadeinFactor = Mathf.Clamp01(Mathf.InverseLerp(this.fadeInStarted, this.fadeInStarted + this.fadeInDuration, this.CurTime));
			}
			else
			{
				this.fadeinFactor = 1f;
			}
			Map currentMap = Find.CurrentMap;
			if (currentMap != null && !WorldRendererUtility.WorldSelected && !Find.TickManager.Paused)
			{
				float num = 1f;
				Camera camera = Find.Camera;
				List<Thing> list = currentMap.listerThings.ThingsInGroup(ThingRequestGroup.MusicalInstrument);
				for (int i = 0; i < list.Count; i++)
				{
					Building_MusicalInstrument building_MusicalInstrument = (Building_MusicalInstrument)list[i];
					if (building_MusicalInstrument.IsBeingPlayed)
					{
						float fadeFactor = this.GetFadeFactor(building_MusicalInstrument.Position.ToVector3Shifted(), building_MusicalInstrument.SoundRange, camera);
						if (fadeFactor < num)
						{
							num = fadeFactor;
						}
					}
				}
				List<Thing> list2 = currentMap.listerThings.ThingsInGroup(ThingRequestGroup.MusicSource);
				for (int j = 0; j < list2.Count; j++)
				{
					Thing thing = list2[j];
					CompPlaysMusic compPlaysMusic = thing.TryGetComp<CompPlaysMusic>();
					if (compPlaysMusic.Playing)
					{
						float fadeFactor2 = this.GetFadeFactor(thing.Position.ToVector3Shifted(), compPlaysMusic.SoundRange, camera);
						if (fadeFactor2 < num)
						{
							num = fadeFactor2;
						}
					}
				}
				foreach (Lord lord in currentMap.lordManager.lords)
				{
					LordJob_Ritual lordJob_Ritual = lord.LordJob as LordJob_Ritual;
					if (lordJob_Ritual != null && lordJob_Ritual.AmbiencePlaying != null && !lordJob_Ritual.AmbiencePlaying.def.subSounds.NullOrEmpty<SubSoundDef>())
					{
						float fadeFactor3 = this.GetFadeFactor(lordJob_Ritual.selectedTarget.CenterVector3, lordJob_Ritual.AmbiencePlaying.def.subSounds.First<SubSoundDef>().distRange, camera);
						if (fadeFactor3 < num)
						{
							num = fadeFactor3;
						}
					}
				}
				this.musicOverridenFadeFactor = num;
				return;
			}
			this.musicOverridenFadeFactor = 1f;
		}

		// Token: 0x0600E5AA RID: 58794 RVA: 0x00430420 File Offset: 0x0042E620
		private void UpdateFadeout()
		{
			this.fadeoutFactor -= Time.deltaTime / this.fadeoutDuration;
			if (this.fadeoutFactor <= 0f)
			{
				this.IsPlaying = false;
				this.loopWhileFading = false;
				this.audioSource.Stop();
				this.fadeoutFactor = 1f;
				if (this.sequenceWorker != null)
				{
					this.PlayNextSongInSequence();
					this.state = MusicManagerPlay.MusicManagerState.Sequence;
					return;
				}
				this.state = MusicManagerPlay.MusicManagerState.Normal;
			}
		}

		// Token: 0x0600E5AB RID: 58795 RVA: 0x00430494 File Offset: 0x0042E694
		private void StartFadeout(float duration)
		{
			this.state = MusicManagerPlay.MusicManagerState.Fadeout;
			this.fadeoutDuration = duration;
		}

		// Token: 0x0600E5AC RID: 58796 RVA: 0x004304A4 File Offset: 0x0042E6A4
		private float GetFadeFactor(Vector3 pos, FloatRange soundRange, Camera camera)
		{
			Vector3 vector = camera.transform.position - pos;
			vector.y = Mathf.Max(vector.y - 15f, 0f);
			vector.y *= 3.5f;
			return Mathf.Min(Mathf.Max(vector.magnitude - soundRange.min, 0f) / (soundRange.max - soundRange.min), 1f);
		}

		// Token: 0x0600E5AD RID: 58797 RVA: 0x00430520 File Offset: 0x0042E720
		private void InitializeMusicManager()
		{
			this.gameObjectCreated = true;
			this.audioSource = new GameObject("MusicAudioSourceDummy")
			{
				transform = 
				{
					parent = Find.Root.soundRoot.sourcePool.sourcePoolCamera.cameraSourcesContainer.transform
				}
			}.AddComponent<AudioSource>();
			this.audioSource.bypassEffects = true;
			this.audioSource.bypassListenerEffects = true;
			this.audioSource.bypassReverbZones = true;
			this.audioSource.priority = 0;
			this.CacheTransitions();
			if (!this.disabled && !this.IsPlaying)
			{
				this.CheckTransitionInterval();
				if (!this.IsPlaying)
				{
					this.StartNewSong();
				}
			}
		}

		// Token: 0x0600E5AE RID: 58798 RVA: 0x004305D0 File Offset: 0x0042E7D0
		private void CacheTransitions()
		{
			this.transitions.Clear();
			List<MusicTransitionDef> list = (from x in DefDatabase<MusicTransitionDef>.AllDefsListForReading
			orderby x.priority descending
			select x).ToList<MusicTransitionDef>();
			for (int i = 0; i < list.Count; i++)
			{
				MusicTransitionDef musicTransitionDef = list[i];
				MusicTransition musicTransition = (MusicTransition)Activator.CreateInstance(musicTransitionDef.workerType);
				musicTransition.InitializeWorker(musicTransitionDef, this);
				this.transitions.Add(musicTransition);
			}
		}

		// Token: 0x0600E5AF RID: 58799 RVA: 0x00430658 File Offset: 0x0042E858
		private void UpdateSubtleAmbienceSoundVolumeMultiplier()
		{
			if (this.IsPlaying && this.CurSanitizedVolume > 0.001f)
			{
				this.subtleAmbienceSoundVolumeMultiplier -= Time.deltaTime * 0.1f;
			}
			else
			{
				this.subtleAmbienceSoundVolumeMultiplier += Time.deltaTime * 0.1f;
			}
			this.subtleAmbienceSoundVolumeMultiplier = Mathf.Clamp01(this.subtleAmbienceSoundVolumeMultiplier);
		}

		// Token: 0x0600E5B0 RID: 58800 RVA: 0x004306BD File Offset: 0x0042E8BD
		public void StartNewSong()
		{
			this.PlaySong(this.ChooseNextSong(), false, false);
		}

		// Token: 0x0600E5B1 RID: 58801 RVA: 0x004306CD File Offset: 0x0042E8CD
		public void ScheduleNewSong()
		{
			this.nextSongStartTime = this.CurTime + 1f;
		}

		// Token: 0x0600E5B2 RID: 58802 RVA: 0x004306E4 File Offset: 0x0042E8E4
		private void PlayNextSongInSequence()
		{
			if (this.sequenceWorker == null)
			{
				Log.Error("Attempted to select a song with a null sequence");
				return;
			}
			if (this.sequenceWorker.def.fadeInDuration > 0f && this.sequenceWorker.timesLooped == 0)
			{
				this.fadeInDuration = this.sequenceWorker.def.fadeInDuration;
				this.fadeInStarted = this.CurTime;
				this.fadeinFactor = 0f;
			}
			this.PlaySong(this.sequenceWorker.SelectSong(), false, false);
		}

		// Token: 0x0600E5B3 RID: 58803 RVA: 0x00430768 File Offset: 0x0042E968
		private void PlaySong(SongDef song, bool forced = false, bool ignorePrefsVolume = false)
		{
			this.songWasForced = forced;
			this.currentSong = song;
			this.audioSource.clip = song.clip;
			this.audioSource.volume = this.CurSanitizedVolume;
			this.audioSource.spatialBlend = 0f;
			this.ignorePrefsVolumeThisSong = ignorePrefsVolume;
			this.songEndTime = this.CurTime + song.clip.length;
			this.audioSource.Play();
			this.IsPlaying = true;
			this.preventDangerTransitionCheck = false;
			this.recentSongs.Enqueue(song);
		}

		// Token: 0x0600E5B4 RID: 58804 RVA: 0x004307FC File Offset: 0x0042E9FC
		public void ForceTriggerNextSongOrSequence()
		{
			if (this.sequenceWorker == null)
			{
				return;
			}
			if (this.sequenceWorker.def.nextSequence != null)
			{
				this.StartSequence(this.sequenceWorker.def.nextSequence);
				return;
			}
			if (this.sequenceWorker.ShouldLoop())
			{
				MusicSequenceWorker musicSequenceWorker = this.sequenceWorker;
				int timesLooped = musicSequenceWorker.timesLooped;
				musicSequenceWorker.timesLooped = timesLooped + 1;
				this.StartFadeout(10f);
			}
		}

		// Token: 0x0600E5B5 RID: 58805 RVA: 0x00430868 File Offset: 0x0042EA68
		public void ForceTriggerTransition(MusicTransitionDef transition)
		{
			foreach (MusicTransition musicTransition in this.transitions)
			{
				if (musicTransition.def == transition)
				{
					this.manuallyTriggered = true;
					this.triggeredTransition = musicTransition;
					this.StartSequence(musicTransition.def.sequence);
					break;
				}
			}
		}

		// Token: 0x0600E5B6 RID: 58806 RVA: 0x004308E0 File Offset: 0x0042EAE0
		public void ForcePlaySong(SongDef song, bool ignorePrefsVolume)
		{
			this.PlaySong(song, true, ignorePrefsVolume);
		}

		// Token: 0x0600E5B7 RID: 58807 RVA: 0x004308EC File Offset: 0x0042EAEC
		private SongDef ChooseNextSong()
		{
			while (this.recentSongs.Count > 7)
			{
				this.recentSongs.Dequeue();
			}
			List<SongDef> list = DefDatabase<SongDef>.AllDefs.Where(new Func<SongDef, bool>(this.AppropriateNow)).ToList<SongDef>();
			if (list.Empty<SongDef>())
			{
				this.recentSongs.Clear();
				list = DefDatabase<SongDef>.AllDefs.Where(new Func<SongDef, bool>(this.AppropriateNow)).ToList<SongDef>();
			}
			if (list.Empty<SongDef>())
			{
				Log.Error("Could not get any appropriate song. Getting random and logging song selection data.");
				this.SongSelectionData();
				return DefDatabase<SongDef>.GetRandom();
			}
			return list.RandomElementByWeight((SongDef s) => s.commonality);
		}

		// Token: 0x0600E5B8 RID: 58808 RVA: 0x004309A4 File Offset: 0x0042EBA4
		private bool AppropriateNow(SongDef song)
		{
			if (!song.playOnMap)
			{
				return false;
			}
			if (this.DangerMusicMode)
			{
				if (!song.tense)
				{
					return false;
				}
			}
			else if (song.tense)
			{
				return false;
			}
			Map map = Find.AnyPlayerHomeMap ?? Find.CurrentMap;
			if (!song.allowedSeasons.NullOrEmpty<Season>())
			{
				if (map == null)
				{
					return false;
				}
				if (!song.allowedSeasons.Contains(GenLocalDate.Season(map)))
				{
					return false;
				}
			}
			if (song.minRoyalTitle != null && !PawnsFinder.AllMapsCaravansAndTravellingTransporters_Alive_Colonists.Any((Pawn p) => p.royalty != null && p.royalty.AllTitlesForReading.Any<RoyalTitle>() && p.royalty.MostSeniorTitle.def.seniority >= song.minRoyalTitle.seniority && !p.IsQuestLodger()))
			{
				return false;
			}
			if (this.recentSongs.Contains(song))
			{
				return false;
			}
			if (song.allowedTimeOfDay == TimeOfDay.Any)
			{
				return true;
			}
			if (map == null)
			{
				return true;
			}
			if (song.allowedTimeOfDay == TimeOfDay.Night)
			{
				return GenLocalDate.DayPercent(map) < 0.2f || GenLocalDate.DayPercent(map) > 0.7f;
			}
			return GenLocalDate.DayPercent(map) > 0.2f && GenLocalDate.DayPercent(map) < 0.7f;
		}

		// Token: 0x0600E5B9 RID: 58809 RVA: 0x000026EA File Offset: 0x000008EA
		public void OnApplicationFocus()
		{
		}

		// Token: 0x0600E5BA RID: 58810 RVA: 0x00430ACC File Offset: 0x0042ECCC
		public string DebugString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("MusicManagerMap");
			stringBuilder.AppendLine("state: " + this.state.ToString());
			string str = "lastStartedSong: ";
			SongDef songDef = this.currentSong;
			stringBuilder.AppendLine(str + ((songDef != null) ? songDef.ToString() : null));
			stringBuilder.AppendLine("fadeoutFactor: " + this.fadeoutFactor.ToString());
			stringBuilder.AppendLine("nextSongStartTime: " + this.nextSongStartTime.ToString());
			stringBuilder.AppendLine("CurTime: " + this.CurTime.ToString());
			stringBuilder.AppendLine("recentSongs: " + (from s in this.recentSongs
			select s.defName).ToCommaList(true, false));
			stringBuilder.AppendLine("disabled: " + this.disabled.ToString());
			return stringBuilder.ToString();
		}

		// Token: 0x0600E5BB RID: 58811 RVA: 0x00430BEC File Offset: 0x0042EDEC
		[DebugOutput]
		public void SongSelectionData()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("Most recent song: " + ((this.currentSong != null) ? this.currentSong.defName : "None"));
			stringBuilder.AppendLine();
			stringBuilder.AppendLine("Songs appropriate to play now:");
			foreach (SongDef songDef in DefDatabase<SongDef>.AllDefs.Where(new Func<SongDef, bool>(this.AppropriateNow)))
			{
				stringBuilder.AppendLine("   " + songDef.defName);
			}
			stringBuilder.AppendLine();
			stringBuilder.AppendLine("Recently played songs:");
			foreach (SongDef songDef2 in this.recentSongs)
			{
				stringBuilder.AppendLine("   " + songDef2.defName);
			}
			Log.Message(stringBuilder.ToString());
		}

		// Token: 0x04009990 RID: 39312
		private AudioSource audioSource;

		// Token: 0x04009991 RID: 39313
		private MusicManagerPlay.MusicManagerState state;

		// Token: 0x04009992 RID: 39314
		private float fadeoutFactor = 1f;

		// Token: 0x04009993 RID: 39315
		private float fadeinFactor = 1f;

		// Token: 0x04009994 RID: 39316
		private float pausedVolumeFactor = 1f;

		// Token: 0x04009995 RID: 39317
		private float musicOverridenFadeFactor = 1f;

		// Token: 0x04009996 RID: 39318
		private float nextSongStartTime = 12f;

		// Token: 0x04009997 RID: 39319
		private float pauseToggledTime = -9999f;

		// Token: 0x04009998 RID: 39320
		private float pausedVolumeFactorAtToggle;

		// Token: 0x04009999 RID: 39321
		private bool isPaused;

		// Token: 0x0400999A RID: 39322
		public bool disabled;

		// Token: 0x0400999B RID: 39323
		private bool songWasForced;

		// Token: 0x0400999C RID: 39324
		private bool gameObjectCreated;

		// Token: 0x0400999D RID: 39325
		private bool ignorePrefsVolumeThisSong;

		// Token: 0x0400999E RID: 39326
		private bool manuallyTriggered;

		// Token: 0x0400999F RID: 39327
		private bool preventDangerTransitionCheck;

		// Token: 0x040099A0 RID: 39328
		private bool initalized;

		// Token: 0x040099A1 RID: 39329
		public float subtleAmbienceSoundVolumeMultiplier = 1f;

		// Token: 0x040099A2 RID: 39330
		private MusicSequenceWorker sequenceWorker;

		// Token: 0x040099A3 RID: 39331
		private MusicTransition triggeredTransition;

		// Token: 0x040099A4 RID: 39332
		private float fadeoutDuration;

		// Token: 0x040099A5 RID: 39333
		private float fadeInDuration;

		// Token: 0x040099A6 RID: 39334
		private float fadeInStarted;

		// Token: 0x040099A7 RID: 39335
		private bool loopWhileFading;

		// Token: 0x040099A8 RID: 39336
		private SongDef currentSong;

		// Token: 0x040099A9 RID: 39337
		private float songEndTime;

		// Token: 0x040099AA RID: 39338
		private readonly Queue<SongDef> recentSongs = new Queue<SongDef>();

		// Token: 0x040099AB RID: 39339
		private readonly List<MusicTransition> transitions = new List<MusicTransition>();

		// Token: 0x040099AC RID: 39340
		private static readonly FloatRange SongIntervalRelax = new FloatRange(85f, 105f);

		// Token: 0x040099AD RID: 39341
		private static readonly FloatRange SongIntervalTension = new FloatRange(2f, 5f);

		// Token: 0x040099AE RID: 39342
		private const float DefaultFadeout = 10f;

		// Token: 0x040099AF RID: 39343
		private const float PauseVolumeFadeTime = 0.5f;

		// Token: 0x040099B0 RID: 39344
		private const int TransitionUpdatePeriodTicks = 60;

		// Token: 0x02002903 RID: 10499
		public enum MusicManagerState
		{
			// Token: 0x040099B4 RID: 39348
			Normal,
			// Token: 0x040099B5 RID: 39349
			Sequence,
			// Token: 0x040099B6 RID: 39350
			Fadeout
		}
	}
}
