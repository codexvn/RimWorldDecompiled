using System;
using System.Collections.Generic;
using System.Linq;

namespace Verse.Sound
{
	// Token: 0x02000E32 RID: 3634
	public class SampleOneShotManager
	{
		// Token: 0x17000EB9 RID: 3769
		// (get) Token: 0x06005BB6 RID: 23478 RVA: 0x001D555E File Offset: 0x001D375E
		public IEnumerable<SampleOneShot> PlayingOneShots
		{
			get
			{
				return this.samples;
			}
		}

		// Token: 0x06005BB7 RID: 23479 RVA: 0x001D5568 File Offset: 0x001D3768
		private float CameraDistanceSquaredOf(SoundInfo info)
		{
			return (float)(Find.CameraDriver.MapPosition - info.Maker.Cell).LengthHorizontalSquared;
		}

		// Token: 0x06005BB8 RID: 23480 RVA: 0x001D559C File Offset: 0x001D379C
		private float ImportanceOf(SampleOneShot sample)
		{
			return this.ImportanceOf(sample.subDef.parentDef, sample.info, sample.AgeRealTime);
		}

		// Token: 0x06005BB9 RID: 23481 RVA: 0x001D55BC File Offset: 0x001D37BC
		private float ImportanceOf(SoundDef def, SoundInfo info, float ageRealTime)
		{
			if (def.priorityMode == VoicePriorityMode.PrioritizeNearest)
			{
				return 1f / (this.CameraDistanceSquaredOf(info) + 1f);
			}
			if (def.priorityMode == VoicePriorityMode.PrioritizeNewest)
			{
				return 1f / (ageRealTime + 1f);
			}
			if (def.priorityMode == VoicePriorityMode.PrioritizeExisting)
			{
				return ageRealTime;
			}
			throw new NotImplementedException();
		}

		// Token: 0x06005BBA RID: 23482 RVA: 0x001D560C File Offset: 0x001D380C
		public bool CanAddPlayingOneShot(SoundDef def, SoundInfo info)
		{
			return SoundDefHelper.CorrectContextNow(def, info.Maker.Map) && (from s in this.samples
			where s.subDef.parentDef == def && s.AgeRealTime < 0.05f
			select s).Count<SampleOneShot>() < def.MaxSimultaneousSamples;
		}

		// Token: 0x06005BBB RID: 23483 RVA: 0x001D5670 File Offset: 0x001D3870
		public void TryAddPlayingOneShot(SampleOneShot newSample)
		{
			if ((from s in this.samples
			where s.subDef.IsSameOrHasSameTag(newSample.subDef)
			select s).Count<SampleOneShot>() >= newSample.subDef.parentDef.maxVoices)
			{
				SampleOneShot sampleOneShot = this.LeastImportantOf(newSample.subDef);
				sampleOneShot.source.Stop();
				this.samples.Remove(sampleOneShot);
			}
			this.samples.Add(newSample);
		}

		// Token: 0x06005BBC RID: 23484 RVA: 0x001D56F8 File Offset: 0x001D38F8
		private SampleOneShot LeastImportantOf(SubSoundDef def)
		{
			SampleOneShot sampleOneShot = null;
			for (int i = 0; i < this.samples.Count; i++)
			{
				SampleOneShot sampleOneShot2 = this.samples[i];
				if (sampleOneShot2.subDef.IsSameOrHasSameTag(def) && (sampleOneShot == null || this.ImportanceOf(sampleOneShot2) < this.ImportanceOf(sampleOneShot)))
				{
					sampleOneShot = sampleOneShot2;
				}
			}
			return sampleOneShot;
		}

		// Token: 0x06005BBD RID: 23485 RVA: 0x001D5750 File Offset: 0x001D3950
		private SampleOneShot LeastImportantOf(SoundDef def)
		{
			SampleOneShot sampleOneShot = null;
			for (int i = 0; i < this.samples.Count; i++)
			{
				SampleOneShot sampleOneShot2 = this.samples[i];
				if (sampleOneShot2.subDef.parentDef == def && (sampleOneShot == null || this.ImportanceOf(sampleOneShot2) < this.ImportanceOf(sampleOneShot)))
				{
					sampleOneShot = sampleOneShot2;
				}
			}
			return sampleOneShot;
		}

		// Token: 0x06005BBE RID: 23486 RVA: 0x001D57A8 File Offset: 0x001D39A8
		public void SampleOneShotManagerUpdate()
		{
			for (int i = 0; i < this.samples.Count; i++)
			{
				this.samples[i].Update();
			}
			this.cleanupList.Clear();
			for (int j = 0; j < this.samples.Count; j++)
			{
				SampleOneShot sampleOneShot = this.samples[j];
				if (sampleOneShot.source == null || (!sampleOneShot.source.isPlaying && (!sampleOneShot.subDef.tempoAffectedByGameSpeed || !Find.TickManager.Paused)) || !SoundDefHelper.CorrectContextNow(sampleOneShot.subDef.parentDef, sampleOneShot.Map))
				{
					if (sampleOneShot.source != null && sampleOneShot.source.isPlaying)
					{
						sampleOneShot.source.Stop();
					}
					sampleOneShot.SampleCleanup();
					this.cleanupList.Add(sampleOneShot);
				}
			}
			if (this.cleanupList.Count > 0)
			{
				this.samples.RemoveAll((SampleOneShot s) => this.cleanupList.Contains(s));
			}
		}

		// Token: 0x04004281 RID: 17025
		private List<SampleOneShot> samples = new List<SampleOneShot>();

		// Token: 0x04004282 RID: 17026
		private List<SampleOneShot> cleanupList = new List<SampleOneShot>();
	}
}
