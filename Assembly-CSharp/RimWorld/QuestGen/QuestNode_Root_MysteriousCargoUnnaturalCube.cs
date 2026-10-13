using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02003B75 RID: 15221
	public class QuestNode_Root_MysteriousCargoUnnaturalCube : QuestNode_Root_MysteriousCargo
	{
		// Token: 0x06014AF2 RID: 84722 RVA: 0x0062F351 File Offset: 0x0062D551
		protected override Thing GenerateThing(Pawn pawn)
		{
			return ThingMaker.MakeThing(ThingDefOf.GoldenCube, null);
		}

		// Token: 0x06014AF3 RID: 84723 RVA: 0x0062F360 File Offset: 0x0062D560
		protected override void AddPostDroppedQuestParts(Pawn pawn, Thing thing, Quest quest)
		{
			quest.PawnDestroyed(pawn, null, delegate
			{
				quest.GiveHediff(pawn, HediffDefOf.CubeInterest, null);
			}, null, null, null, QuestPart.SignalListenMode.OngoingOnly);
		}

		// Token: 0x06014AF4 RID: 84724 RVA: 0x0062F3A4 File Offset: 0x0062D5A4
		protected override bool ValidatePawn(Pawn pawn)
		{
			return base.ValidatePawn(pawn) && !pawn.health.hediffSet.HasHediff(HediffDefOf.CubeInterest, false) && !pawn.health.hediffSet.HasHediff(HediffDefOf.CubeComa, false);
		}
	}
}
