using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Verse.Sound
{
	// Token: 0x02000E44 RID: 3652
	public class SubSustainer
	{
		// Token: 0x17000EC8 RID: 3784
		// (get) Token: 0x06005BFE RID: 23550 RVA: 0x001D67EF File Offset: 0x001D49EF
		public SoundInfo Info
		{
			get
			{
				return this.parent.info;
			}
		}

		// Token: 0x17000EC9 RID: 3785
		// (get) Token: 0x06005BFF RID: 23551 RVA: 0x001D67FC File Offset: 0x001D49FC
		public SoundParams ExternalParams
		{
			get
			{
				return this.parent.externalParams;
			}
		}

		// Token: 0x06005C00 RID: 23552 RVA: 0x001D680C File Offset: 0x001D4A0C
		public SubSustainer(Sustainer parent, SubSoundDef subSoundDef)
		{
			this.parent = parent;
			this.subDef = subSoundDef;
			LongEventHandler.ExecuteWhenFinished(delegate
			{
				this.creationFrame = Time.frameCount;
				this.creationRealTime = Time.realtimeSinceStartup;
				if (Current.ProgramState == ProgramState.Playing)
				{
					this.creationTick = Find.TickManager.TicksGame;
				}
				if (this.subDef.startDelayRange.TrueMax < 0.001f)
				{
					this.StartSample(this.subDef.randomStartPoint ? Rand.Value : 0f);
					return;
				}
				this.nextSampleStartTime = Time.realtimeSinceStartup + this.subDef.startDelayRange.RandomInRange;
			});
		}

		// Token: 0x06005C01 RID: 23553 RVA: 0x001D6864 File Offset: 0x001D4A64
		private void StartSample(float startPct = 0f)
		{
			ResolvedGrain resolvedGrain = this.subDef.RandomizedResolvedGrain();
			if (resolvedGrain == null)
			{
				string[] array = new string[5];
				array[0] = "SubSustainer for ";
				int num = 1;
				SubSoundDef subSoundDef = this.subDef;
				array[num] = ((subSoundDef != null) ? subSoundDef.ToString() : null);
				array[2] = " of ";
				int num2 = 3;
				SoundDef def = this.parent.def;
				array[num2] = ((def != null) ? def.ToString() : null);
				array[4] = " could not resolve any grains.";
				Log.Error(string.Concat(array));
				this.parent.End();
				return;
			}
			float num3;
			if (this.subDef.sustainLoop)
			{
				num3 = this.subDef.sustainLoopDurationRange.RandomInRange;
			}
			else
			{
				num3 = resolvedGrain.duration;
			}
			float num4 = resolvedGrain.duration * startPct;
			float num5 = 1f;
			if (this.subDef.sustainIntervalFactorByAggregateSize != null)
			{
				SimpleCurve sustainIntervalFactorByAggregateSize = this.subDef.sustainIntervalFactorByAggregateSize;
				SoundParams externalParams = this.ExternalParams;
				float? num6;
				if (externalParams == null)
				{
					num6 = null;
				}
				else
				{
					SoundSizeAggregator sizeAggregator = externalParams.sizeAggregator;
					num6 = ((sizeAggregator != null) ? new float?(sizeAggregator.AggregateSize) : null);
				}
				num5 = sustainIntervalFactorByAggregateSize.Evaluate(num6 ?? 1f);
			}
			float num7 = Time.realtimeSinceStartup + num3 - num4;
			this.nextSampleStartTime = num7 + this.subDef.sustainIntervalRange.RandomInRange * num5;
			if (this.nextSampleStartTime < Time.realtimeSinceStartup + 0.01f)
			{
				this.nextSampleStartTime = Time.realtimeSinceStartup + 0.01f;
			}
			if (resolvedGrain is ResolvedGrain_Silence)
			{
				return;
			}
			SampleSustainer sampleSustainer = SampleSustainer.TryMakeAndPlay(this, ((ResolvedGrain_Clip)resolvedGrain).clip, num7, num4);
			this.subDef.Notify_GrainPlayed(resolvedGrain);
			if (sampleSustainer == null)
			{
				return;
			}
			if (this.subDef.sustainSkipFirstAttack && Time.frameCount == this.creationFrame)
			{
				sampleSustainer.resolvedSkipAttack = true;
			}
			this.samples.Add(sampleSustainer);
		}

		// Token: 0x06005C02 RID: 23554 RVA: 0x001D6A30 File Offset: 0x001D4C30
		public void SubSustainerUpdate()
		{
			for (int i = this.samples.Count - 1; i >= 0; i--)
			{
				if (Time.realtimeSinceStartup > this.samples[i].scheduledEndTime)
				{
					this.EndSample(this.samples[i]);
				}
			}
			if (Time.realtimeSinceStartup > this.nextSampleStartTime)
			{
				this.StartSample(0f);
			}
			for (int j = 0; j < this.samples.Count; j++)
			{
				this.samples[j].Update();
			}
		}

		// Token: 0x06005C03 RID: 23555 RVA: 0x001D6ABE File Offset: 0x001D4CBE
		private void EndSample(SampleSustainer samp)
		{
			this.samples.Remove(samp);
			samp.SampleCleanup();
		}

		// Token: 0x06005C04 RID: 23556 RVA: 0x001D6AD3 File Offset: 0x001D4CD3
		public virtual void Cleanup()
		{
			while (this.samples.Count > 0)
			{
				this.EndSample(this.samples[0]);
			}
		}

		// Token: 0x06005C05 RID: 23557 RVA: 0x001D6AF7 File Offset: 0x001D4CF7
		public override string ToString()
		{
			return this.subDef.name + "_" + this.creationFrame.ToString();
		}

		// Token: 0x06005C06 RID: 23558 RVA: 0x001D6B19 File Offset: 0x001D4D19
		public override int GetHashCode()
		{
			return Gen.HashCombine<SubSoundDef>(this.creationRealTime.GetHashCode(), this.subDef);
		}

		// Token: 0x06005C07 RID: 23559 RVA: 0x001D6B34 File Offset: 0x001D4D34
		public string SamplesDebugString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (SampleSustainer sampleSustainer in this.samples)
			{
				stringBuilder.AppendLine(sampleSustainer.ToString());
			}
			return stringBuilder.ToString();
		}

		// Token: 0x040042AB RID: 17067
		public Sustainer parent;

		// Token: 0x040042AC RID: 17068
		public SubSoundDef subDef;

		// Token: 0x040042AD RID: 17069
		private List<SampleSustainer> samples = new List<SampleSustainer>();

		// Token: 0x040042AE RID: 17070
		private float nextSampleStartTime;

		// Token: 0x040042AF RID: 17071
		public int creationFrame = -1;

		// Token: 0x040042B0 RID: 17072
		public int creationTick = -1;

		// Token: 0x040042B1 RID: 17073
		public float creationRealTime = -1f;

		// Token: 0x040042B2 RID: 17074
		private const float MinSampleStartInterval = 0.01f;
	}
}
