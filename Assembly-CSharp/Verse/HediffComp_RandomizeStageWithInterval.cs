using System;
using RimWorld;

namespace Verse
{
	// Token: 0x020006F2 RID: 1778
	public class HediffComp_RandomizeStageWithInterval : HediffComp_Randomizer
	{
		// Token: 0x170007B6 RID: 1974
		// (get) Token: 0x06002BC5 RID: 11205 RVA: 0x000E4224 File Offset: 0x000E2424
		public HediffCompProperties_RandomizeStageWithInterval Props
		{
			get
			{
				return (HediffCompProperties_RandomizeStageWithInterval)this.props;
			}
		}

		// Token: 0x06002BC6 RID: 11206 RVA: 0x000E4234 File Offset: 0x000E2434
		public override void Randomize()
		{
			int curStageIndex = this.parent.CurStageIndex;
			this.parent.Severity = this.parent.def.stages.RandomElement<HediffStage>().minSeverity;
			int curStageIndex2 = this.parent.CurStageIndex;
			if (curStageIndex != curStageIndex2 && !this.Props.notifyMessage.NullOrEmpty() && PawnUtility.ShouldSendNotificationAbout(this.parent.pawn))
			{
				Messages.Message(this.Props.notifyMessage.Formatted(this.parent.pawn.Named("PAWN"), this.parent.def.stages[curStageIndex].label, this.parent.def.stages[curStageIndex2].label), this.parent.pawn, MessageTypeDefOf.NeutralEvent, true);
			}
		}
	}
}
