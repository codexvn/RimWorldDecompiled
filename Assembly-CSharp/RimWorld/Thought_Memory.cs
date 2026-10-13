using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002541 RID: 9537
	public class Thought_Memory : Thought
	{
		// Token: 0x17002017 RID: 8215
		// (get) Token: 0x0600D0A6 RID: 53414 RVA: 0x003D5B65 File Offset: 0x003D3D65
		public override bool VisibleInNeedsTab
		{
			get
			{
				return base.VisibleInNeedsTab && !this.ShouldDiscard;
			}
		}

		// Token: 0x17002018 RID: 8216
		// (get) Token: 0x0600D0A7 RID: 53415 RVA: 0x003D5B7A File Offset: 0x003D3D7A
		public override int DurationTicks
		{
			get
			{
				if (this.durationTicksOverride < 0)
				{
					return this.def.DurationTicks;
				}
				return this.durationTicksOverride;
			}
		}

		// Token: 0x17002019 RID: 8217
		// (get) Token: 0x0600D0A8 RID: 53416 RVA: 0x003D5B97 File Offset: 0x003D3D97
		public override int CurStageIndex
		{
			get
			{
				return this.forcedStage;
			}
		}

		// Token: 0x1700201A RID: 8218
		// (get) Token: 0x0600D0A9 RID: 53417 RVA: 0x003D5B9F File Offset: 0x003D3D9F
		public virtual bool ShouldDiscard
		{
			get
			{
				return !this.permanent && this.age > this.DurationTicks;
			}
		}

		// Token: 0x1700201B RID: 8219
		// (get) Token: 0x0600D0AA RID: 53418 RVA: 0x003D5BBC File Offset: 0x003D3DBC
		public override string LabelCap
		{
			get
			{
				if (this.cachedLabelCap == null || this.cachedLabelCapForOtherPawn != this.otherPawn || this.cachedLabelCapForStageIndex != this.CurStageIndex)
				{
					if (this.otherPawn != null)
					{
						this.cachedLabelCap = base.CurStage.label.Formatted(this.otherPawn.LabelShort, this.otherPawn).CapitalizeFirst();
						if (this.def.Worker != null)
						{
							this.cachedLabelCap = this.def.Worker.PostProcessLabel(this.pawn, this.cachedLabelCap);
						}
					}
					else
					{
						this.cachedLabelCap = base.LabelCap;
					}
					this.cachedLabelCapForOtherPawn = this.otherPawn;
					this.cachedLabelCapForStageIndex = this.CurStageIndex;
				}
				return this.cachedLabelCap;
			}
		}

		// Token: 0x1700201C RID: 8220
		// (get) Token: 0x0600D0AB RID: 53419 RVA: 0x003D5C94 File Offset: 0x003D3E94
		public override string LabelCapSocial
		{
			get
			{
				string text;
				if (base.CurStage.labelSocial != null)
				{
					text = base.CurStage.LabelSocialCap.Formatted(this.pawn.Named("PAWN"), this.otherPawn.Named("OTHERPAWN"));
				}
				else
				{
					text = base.LabelCapSocial;
				}
				if (this.sourcePrecept != null)
				{
					text += " (" + "Ideo".Translate() + ")";
				}
				return text;
			}
		}

		// Token: 0x1700201D RID: 8221
		// (get) Token: 0x0600D0AC RID: 53420 RVA: 0x000028E7 File Offset: 0x00000AE7
		public virtual bool Save
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600D0AD RID: 53421 RVA: 0x003D5D20 File Offset: 0x003D3F20
		public void SetForcedStage(int stageIndex)
		{
			this.forcedStage = stageIndex;
		}

		// Token: 0x0600D0AE RID: 53422 RVA: 0x003D5D2C File Offset: 0x003D3F2C
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Pawn>(ref this.otherPawn, "otherPawn", true);
			Scribe_Values.Look<float>(ref this.moodPowerFactor, "moodPowerFactor", 1f, false);
			Scribe_Values.Look<int>(ref this.moodOffset, "moodOffset", 0, false);
			Scribe_Values.Look<int>(ref this.age, "age", 0, false);
			Scribe_Values.Look<int>(ref this.forcedStage, "stageIndex", 0, false);
			Scribe_Values.Look<int>(ref this.durationTicksOverride, "durationTicksOverride", -1, false);
			Scribe_Values.Look<bool>(ref this.permanent, "permanent", false, false);
		}

		// Token: 0x0600D0AF RID: 53423 RVA: 0x003D5DC0 File Offset: 0x003D3FC0
		public virtual void ThoughtInterval()
		{
			this.age += 150;
		}

		// Token: 0x0600D0B0 RID: 53424 RVA: 0x003D5DD4 File Offset: 0x003D3FD4
		public void Renew()
		{
			this.age = 0;
		}

		// Token: 0x0600D0B1 RID: 53425 RVA: 0x003D5DE0 File Offset: 0x003D3FE0
		public virtual bool TryMergeWithExistingMemory(out bool showBubble)
		{
			ThoughtHandler thoughts = this.pawn.needs.mood.thoughts;
			if (thoughts.memories.NumMemoriesInGroup(this) >= this.def.stackLimit)
			{
				Thought_Memory thought_Memory = thoughts.memories.OldestMemoryInGroup(this);
				if (thought_Memory != null)
				{
					showBubble = (thought_Memory.age > thought_Memory.DurationTicks / 2);
					thought_Memory.Renew();
					return true;
				}
			}
			showBubble = true;
			return false;
		}

		// Token: 0x0600D0B2 RID: 53426 RVA: 0x003D5E4C File Offset: 0x003D404C
		public override bool GroupsWith(Thought other)
		{
			Thought_Memory thought_Memory = other as Thought_Memory;
			return thought_Memory != null && base.GroupsWith(other) && (this.otherPawn == thought_Memory.otherPawn || this.LabelCap == thought_Memory.LabelCap);
		}

		// Token: 0x0600D0B3 RID: 53427 RVA: 0x003D5E94 File Offset: 0x003D4094
		public override float MoodOffset()
		{
			if (ThoughtUtility.ThoughtNullified(this.pawn, this.def))
			{
				return 0f;
			}
			float num = base.MoodOffset();
			num *= this.moodPowerFactor;
			num += (float)this.moodOffset;
			if (this.def.lerpMoodToZero)
			{
				num *= 1f - (float)this.age / (float)this.DurationTicks;
			}
			return num;
		}

		// Token: 0x0600D0B4 RID: 53428 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void Notify_NewThoughtInGroupAdded(Thought_Memory memory)
		{
		}

		// Token: 0x0600D0B5 RID: 53429 RVA: 0x003D5EFC File Offset: 0x003D40FC
		public override string ToString()
		{
			return string.Concat(new string[]
			{
				"(",
				this.def.defName,
				", moodPowerFactor=",
				this.moodPowerFactor.ToString(),
				", moodOffset=",
				this.moodOffset.ToString(),
				", age=",
				this.age.ToString(),
				")"
			});
		}

		// Token: 0x0600D0B6 RID: 53430 RVA: 0x003D5F78 File Offset: 0x003D4178
		public virtual void CopyFrom(Thought_Memory m)
		{
			this.age = m.age;
			this.pawn = m.pawn;
			this.sourcePrecept = m.sourcePrecept;
			this.moodOffset = m.moodOffset;
			this.otherPawn = m.otherPawn;
			this.durationTicksOverride = m.durationTicksOverride;
			this.moodPowerFactor = m.moodPowerFactor;
			this.SetForcedStage(m.CurStageIndex);
		}

		// Token: 0x04008DD9 RID: 36313
		public float moodPowerFactor = 1f;

		// Token: 0x04008DDA RID: 36314
		public int moodOffset;

		// Token: 0x04008DDB RID: 36315
		public Pawn otherPawn;

		// Token: 0x04008DDC RID: 36316
		public bool permanent;

		// Token: 0x04008DDD RID: 36317
		public int age;

		// Token: 0x04008DDE RID: 36318
		private int forcedStage;

		// Token: 0x04008DDF RID: 36319
		public int durationTicksOverride = -1;

		// Token: 0x04008DE0 RID: 36320
		private string cachedLabelCap;

		// Token: 0x04008DE1 RID: 36321
		private Pawn cachedLabelCapForOtherPawn;

		// Token: 0x04008DE2 RID: 36322
		private int cachedLabelCapForStageIndex = -1;
	}
}
