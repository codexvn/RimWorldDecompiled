using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002545 RID: 9541
	public class Thought_MemoryRoyalTitle : Thought_Memory
	{
		// Token: 0x17002026 RID: 8230
		// (get) Token: 0x0600D0C6 RID: 53446 RVA: 0x003D61E8 File Offset: 0x003D43E8
		public override string LabelCap
		{
			get
			{
				return base.CurStage.label.Formatted(this.titleDef.GetLabelCapFor(this.pawn).Named("TITLE"), this.pawn.Named("PAWN"));
			}
		}

		// Token: 0x17002027 RID: 8231
		// (get) Token: 0x0600D0C7 RID: 53447 RVA: 0x003D6238 File Offset: 0x003D4438
		public override string Description
		{
			get
			{
				return base.CurStage.description.Formatted(this.titleDef.GetLabelCapFor(this.pawn).Named("TITLE"), this.pawn.Named("PAWN"));
			}
		}

		// Token: 0x0600D0C8 RID: 53448 RVA: 0x003D6285 File Offset: 0x003D4485
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<RoyalTitleDef>(ref this.titleDef, "titleDef");
		}

		// Token: 0x0600D0C9 RID: 53449 RVA: 0x003D629D File Offset: 0x003D449D
		public override void CopyFrom(Thought_Memory m)
		{
			this.titleDef = ((Thought_MemoryRoyalTitle)m).titleDef;
			base.CopyFrom(m);
		}

		// Token: 0x04008DE7 RID: 36327
		public RoyalTitleDef titleDef;
	}
}
