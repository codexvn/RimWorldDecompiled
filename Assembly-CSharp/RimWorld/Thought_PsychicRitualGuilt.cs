using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002548 RID: 9544
	public class Thought_PsychicRitualGuilt : Thought_Memory
	{
		// Token: 0x1700202D RID: 8237
		// (get) Token: 0x0600D0DB RID: 53467 RVA: 0x003D6544 File Offset: 0x003D4744
		public override string LabelCap
		{
			get
			{
				return base.CurStage.label.Formatted(this.ritualDef.label.Named("RITUAL")).CapitalizeFirst();
			}
		}

		// Token: 0x1700202E RID: 8238
		// (get) Token: 0x0600D0DC RID: 53468 RVA: 0x003D6583 File Offset: 0x003D4783
		public override int CurStageIndex
		{
			get
			{
				if (!ModsConfig.IdeologyActive)
				{
					return 0;
				}
				if (this.pawn.Ideo.HasPrecept(PreceptDefOf.PsychicRituals_Disapproved))
				{
					return 1;
				}
				if (this.pawn.Ideo.HasPrecept(PreceptDefOf.PsychicRituals_Abhorrent))
				{
					return 2;
				}
				return 0;
			}
		}

		// Token: 0x0600D0DD RID: 53469 RVA: 0x003D65C1 File Offset: 0x003D47C1
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<PsychicRitualDef>(ref this.ritualDef, "ritualDef");
		}

		// Token: 0x0600D0DE RID: 53470 RVA: 0x003D65DC File Offset: 0x003D47DC
		public override void CopyFrom(Thought_Memory m)
		{
			base.CopyFrom(m);
			Thought_PsychicRitualGuilt thought_PsychicRitualGuilt = m as Thought_PsychicRitualGuilt;
			if (thought_PsychicRitualGuilt != null)
			{
				this.ritualDef = thought_PsychicRitualGuilt.ritualDef;
			}
		}

		// Token: 0x0600D0DF RID: 53471 RVA: 0x003D6608 File Offset: 0x003D4808
		public override bool GroupsWith(Thought other)
		{
			if (base.GroupsWith(other))
			{
				Thought_PsychicRitualGuilt thought_PsychicRitualGuilt = other as Thought_PsychicRitualGuilt;
				if (thought_PsychicRitualGuilt != null)
				{
					return this.ritualDef == thought_PsychicRitualGuilt.ritualDef;
				}
			}
			return false;
		}

		// Token: 0x04008DEA RID: 36330
		public PsychicRitualDef ritualDef;
	}
}
