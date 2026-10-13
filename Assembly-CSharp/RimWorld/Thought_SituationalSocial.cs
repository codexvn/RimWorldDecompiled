using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200254A RID: 9546
	public class Thought_SituationalSocial : Thought_Situational, ISocialThought
	{
		// Token: 0x17002032 RID: 8242
		// (get) Token: 0x0600D0E9 RID: 53481 RVA: 0x003D6818 File Offset: 0x003D4A18
		public override bool VisibleInNeedsTab
		{
			get
			{
				return base.VisibleInNeedsTab && this.MoodOffset() != 0f;
			}
		}

		// Token: 0x17002033 RID: 8243
		// (get) Token: 0x0600D0EA RID: 53482 RVA: 0x003D6834 File Offset: 0x003D4A34
		public override string LabelCap
		{
			get
			{
				if (!this.reason.NullOrEmpty())
				{
					TaggedString taggedString = base.CurStage.label.Formatted(this.reason.Named("REASON"), this.pawn.Named("PAWN"), this.otherPawn.Named("OTHERPAWN")).CapitalizeFirst();
					if (this.def.Worker != null)
					{
						taggedString = this.def.Worker.PostProcessLabel(this.pawn, taggedString);
					}
					return taggedString;
				}
				if (this.def.Worker == null)
				{
					return base.CurStage.LabelCap.Formatted(this.pawn.Named("PAWN"), this.otherPawn.Named("OTHERPAWN")).CapitalizeFirst();
				}
				return base.LabelCap;
			}
		}

		// Token: 0x17002034 RID: 8244
		// (get) Token: 0x0600D0EB RID: 53483 RVA: 0x003D6920 File Offset: 0x003D4B20
		public override string LabelCapSocial
		{
			get
			{
				if (base.CurStage.labelSocial != null)
				{
					return base.CurStage.LabelSocialCap.Formatted(this.pawn.Named("PAWN"), this.otherPawn.Named("OTHERPAWN"), this.reason.Named("REASON"));
				}
				return base.LabelCapSocial;
			}
		}

		// Token: 0x0600D0EC RID: 53484 RVA: 0x003D6986 File Offset: 0x003D4B86
		public Pawn OtherPawn()
		{
			return this.otherPawn;
		}

		// Token: 0x0600D0ED RID: 53485 RVA: 0x003D6990 File Offset: 0x003D4B90
		public virtual float OpinionOffset()
		{
			if (ThoughtUtility.ThoughtNullified(this.pawn, this.def))
			{
				return 0f;
			}
			float num = base.CurStage.baseOpinionOffset;
			if (this.def.effectMultiplyingStat != null)
			{
				num *= this.pawn.GetStatValue(this.def.effectMultiplyingStat, true, -1) * this.otherPawn.GetStatValue(this.def.effectMultiplyingStat, true, -1);
			}
			return num;
		}

		// Token: 0x0600D0EE RID: 53486 RVA: 0x003D6A04 File Offset: 0x003D4C04
		public override bool GroupsWith(Thought other)
		{
			Thought_SituationalSocial thought_SituationalSocial = other as Thought_SituationalSocial;
			return thought_SituationalSocial != null && base.GroupsWith(other) && this.otherPawn == thought_SituationalSocial.otherPawn;
		}

		// Token: 0x0600D0EF RID: 53487 RVA: 0x003D6A36 File Offset: 0x003D4C36
		protected override ThoughtState CurrentStateInternal()
		{
			return this.def.Worker.CurrentSocialState(this.pawn, this.otherPawn);
		}

		// Token: 0x04008DED RID: 36333
		public Pawn otherPawn;
	}
}
