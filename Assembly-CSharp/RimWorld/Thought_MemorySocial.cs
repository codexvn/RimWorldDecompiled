using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02002546 RID: 9542
	public class Thought_MemorySocial : Thought_Memory, ISocialThought
	{
		// Token: 0x17002028 RID: 8232
		// (get) Token: 0x0600D0CB RID: 53451 RVA: 0x003D62B7 File Offset: 0x003D44B7
		public override bool ShouldDiscard
		{
			get
			{
				return this.otherPawn == null || this.opinionOffset == 0f || base.ShouldDiscard;
			}
		}

		// Token: 0x17002029 RID: 8233
		// (get) Token: 0x0600D0CC RID: 53452 RVA: 0x003D62D6 File Offset: 0x003D44D6
		public override bool VisibleInNeedsTab
		{
			get
			{
				return base.VisibleInNeedsTab && this.MoodOffset() != 0f;
			}
		}

		// Token: 0x1700202A RID: 8234
		// (get) Token: 0x0600D0CD RID: 53453 RVA: 0x003D62F2 File Offset: 0x003D44F2
		private float AgePct
		{
			get
			{
				return (float)this.age / (float)this.DurationTicks;
			}
		}

		// Token: 0x1700202B RID: 8235
		// (get) Token: 0x0600D0CE RID: 53454 RVA: 0x003D6303 File Offset: 0x003D4503
		private float AgeFactor
		{
			get
			{
				return Mathf.InverseLerp(1f, this.def.lerpOpinionToZeroAfterDurationPct, this.AgePct);
			}
		}

		// Token: 0x0600D0CF RID: 53455 RVA: 0x003D6320 File Offset: 0x003D4520
		public virtual float OpinionOffset()
		{
			if (ThoughtUtility.ThoughtNullified(this.pawn, this.def))
			{
				return 0f;
			}
			if (this.ShouldDiscard)
			{
				return 0f;
			}
			return this.opinionOffset * this.AgeFactor;
		}

		// Token: 0x0600D0D0 RID: 53456 RVA: 0x003D6356 File Offset: 0x003D4556
		public Pawn OtherPawn()
		{
			return this.otherPawn;
		}

		// Token: 0x0600D0D1 RID: 53457 RVA: 0x003D635E File Offset: 0x003D455E
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.opinionOffset, "opinionOffset", 0f, false);
		}

		// Token: 0x0600D0D2 RID: 53458 RVA: 0x003D637C File Offset: 0x003D457C
		public override void Init()
		{
			base.Init();
			this.opinionOffset = base.CurStage.baseOpinionOffset;
		}

		// Token: 0x0600D0D3 RID: 53459 RVA: 0x003D6395 File Offset: 0x003D4595
		public override bool TryMergeWithExistingMemory(out bool showBubble)
		{
			showBubble = false;
			return false;
		}

		// Token: 0x0600D0D4 RID: 53460 RVA: 0x003D639C File Offset: 0x003D459C
		public override bool GroupsWith(Thought other)
		{
			Thought_MemorySocial thought_MemorySocial = other as Thought_MemorySocial;
			return thought_MemorySocial != null && base.GroupsWith(other) && this.otherPawn == thought_MemorySocial.otherPawn;
		}

		// Token: 0x04008DE8 RID: 36328
		public float opinionOffset;
	}
}
