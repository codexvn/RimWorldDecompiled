using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200275E RID: 10078
	public class RitualOutcomeEffectDef : Def
	{
		// Token: 0x170021D1 RID: 8657
		// (get) Token: 0x0600DD06 RID: 56582 RVA: 0x00410A74 File Offset: 0x0040EC74
		public string Description
		{
			get
			{
				if (this.minMoodCached == 3.4028235E+38f && !this.outcomeChances.NullOrEmpty<RitualOutcomePossibility>())
				{
					foreach (RitualOutcomePossibility ritualOutcomePossibility in this.outcomeChances)
					{
						if (ritualOutcomePossibility.memory != null)
						{
							float baseMoodEffect = ritualOutcomePossibility.memory.stages[0].baseMoodEffect;
							if (baseMoodEffect > this.maxMoodCached)
							{
								this.maxMoodCached = baseMoodEffect;
							}
							if (baseMoodEffect < this.minMoodCached)
							{
								this.minMoodCached = baseMoodEffect;
							}
							this.moodDurationCached = ritualOutcomePossibility.memory.durationDays;
						}
					}
				}
				return this.description.Formatted(this.minMoodCached.Named("MINMOOD"), this.maxMoodCached.ToStringWithSign("0.##").Named("MAXMOOD"), this.moodDurationCached.Named("MOODDAYS"));
			}
		}

		// Token: 0x170021D2 RID: 8658
		// (get) Token: 0x0600DD07 RID: 56583 RVA: 0x00410B84 File Offset: 0x0040ED84
		public RitualOutcomePossibility BestOutcome
		{
			get
			{
				if (this.bestOutcomeCached == null)
				{
					this.bestOutcomeCached = this.outcomeChances.MaxBy((RitualOutcomePossibility o) => o.positivityIndex);
				}
				return this.bestOutcomeCached;
			}
		}

		// Token: 0x170021D3 RID: 8659
		// (get) Token: 0x0600DD08 RID: 56584 RVA: 0x00410BC4 File Offset: 0x0040EDC4
		public RitualOutcomePossibility WorstOutcome
		{
			get
			{
				if (this.worstOutcomeCached == null)
				{
					this.worstOutcomeCached = this.outcomeChances.MinBy((RitualOutcomePossibility o) => o.positivityIndex);
				}
				return this.worstOutcomeCached;
			}
		}

		// Token: 0x0600DD09 RID: 56585 RVA: 0x00410C04 File Offset: 0x0040EE04
		public RitualOutcomeEffectWorker GetInstance()
		{
			return (RitualOutcomeEffectWorker)Activator.CreateInstance(this.workerClass, new object[]
			{
				this
			});
		}

		// Token: 0x0600DD0A RID: 56586 RVA: 0x00410C20 File Offset: 0x0040EE20
		public string OutcomeMoodBreakdown(RitualOutcomePossibility outcome)
		{
			if (outcome.memory != null && outcome.memory.stages[0].baseMoodEffect != 0f)
			{
				return "RitualOutcomeExtraDesc_Mood".Translate(outcome.memory.stages[0].baseMoodEffect.ToStringWithSign("0.##"), outcome.memory.durationDays);
			}
			return "";
		}

		// Token: 0x0600DD0B RID: 56587 RVA: 0x00410C9C File Offset: 0x0040EE9C
		public override IEnumerable<string> ConfigErrors()
		{
			foreach (string text in base.ConfigErrors())
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			float num = 0f;
			foreach (RitualOutcomePossibility ritualOutcomePossibility in this.outcomeChances)
			{
				num += ritualOutcomePossibility.chance;
			}
			if (this.outcomeChances.Any<RitualOutcomePossibility>() && Mathf.Abs(num - 1f) > 0.0001f)
			{
				yield return "Sum of outcome chances doesn't add up to 1.0. total=" + num.ToString();
			}
			yield break;
			yield break;
		}

		// Token: 0x04009505 RID: 38149
		public Type workerClass;

		// Token: 0x04009506 RID: 38150
		public float startingQuality;

		// Token: 0x04009507 RID: 38151
		public float minQuality;

		// Token: 0x04009508 RID: 38152
		public float maxQuality = 1f;

		// Token: 0x04009509 RID: 38153
		public bool givesDevelopmentPoints = true;

		// Token: 0x0400950A RID: 38154
		public ThoughtDef memoryDef;

		// Token: 0x0400950B RID: 38155
		public List<RitualOutcomePossibility> outcomeChances = new List<RitualOutcomePossibility>();

		// Token: 0x0400950C RID: 38156
		public SimpleCurve honorFromQuality;

		// Token: 0x0400950D RID: 38157
		[MustTranslate]
		public List<string> extraPredictedOutcomeDescriptions;

		// Token: 0x0400950E RID: 38158
		[MustTranslate]
		public List<string> extraInfoLines;

		// Token: 0x0400950F RID: 38159
		public List<RitualOutcomeEffectDef.ExtraOutcomeChanceDescription> extraOutcomeDescriptions;

		// Token: 0x04009510 RID: 38160
		public EffecterDef effecter;

		// Token: 0x04009511 RID: 38161
		public FleckDef fleckDef;

		// Token: 0x04009512 RID: 38162
		public int flecksPerCell;

		// Token: 0x04009513 RID: 38163
		public FloatRange fleckRotationRange = new FloatRange(0f, 360f);

		// Token: 0x04009514 RID: 38164
		public FloatRange fleckScaleRange = FloatRange.One;

		// Token: 0x04009515 RID: 38165
		public FloatRange fleckVelocityAngle = FloatRange.Zero;

		// Token: 0x04009516 RID: 38166
		public FloatRange fleckVelocitySpeed = FloatRange.Zero;

		// Token: 0x04009517 RID: 38167
		public ThingDef filthDefToSpawn;

		// Token: 0x04009518 RID: 38168
		public IntRange filthCountToSpawn = IntRange.Zero;

		// Token: 0x04009519 RID: 38169
		public List<RitualOutcomeComp> comps;

		// Token: 0x0400951A RID: 38170
		public bool warnOnLowQuality = true;

		// Token: 0x0400951B RID: 38171
		public bool allowAttachableOutcome = true;

		// Token: 0x0400951C RID: 38172
		public bool allowOutcomeWithNoAttendance;

		// Token: 0x0400951D RID: 38173
		private float minMoodCached = float.MaxValue;

		// Token: 0x0400951E RID: 38174
		private float maxMoodCached = float.MinValue;

		// Token: 0x0400951F RID: 38175
		private float moodDurationCached = -1f;

		// Token: 0x04009520 RID: 38176
		private RitualOutcomePossibility bestOutcomeCached;

		// Token: 0x04009521 RID: 38177
		private RitualOutcomePossibility worstOutcomeCached;

		// Token: 0x0200275F RID: 10079
		public class ExtraOutcomeChanceDescription
		{
			// Token: 0x04009522 RID: 38178
			[MustTranslate]
			public string description;

			// Token: 0x04009523 RID: 38179
			public Func<float, float> qualityToValue;
		}
	}
}
