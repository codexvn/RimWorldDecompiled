using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02003B78 RID: 15224
	public class QuestNode_Root_MysteriousCargoUnnaturalCorpse : QuestNode_Root_MysteriousCargo
	{
		// Token: 0x06014AFB RID: 84731 RVA: 0x0062F41B File Offset: 0x0062D61B
		protected override Thing GenerateThing(Pawn pawn)
		{
			return AnomalyUtility.MakeUnnaturalCorpse(pawn);
		}

		// Token: 0x06014AFC RID: 84732 RVA: 0x0062F423 File Offset: 0x0062D623
		protected override bool ValidatePawn(Pawn pawn)
		{
			return base.ValidatePawn(pawn) && pawn.RaceProps.unnaturalCorpseDef != null && !Find.Anomaly.PawnHasUnnaturalCorpse(pawn);
		}

		// Token: 0x06014AFD RID: 84733 RVA: 0x0062F450 File Offset: 0x0062D650
		protected override void AddPostDroppedQuestParts(Pawn pawn, Thing thing, Quest quest)
		{
			quest.PawnDestroyed(pawn, null, delegate
			{
				quest.LinkUnnaturalCorpse(pawn, thing as UnnaturalCorpse, null);
			}, null, null, null, QuestPart.SignalListenMode.OngoingOnly);
		}
	}
}
