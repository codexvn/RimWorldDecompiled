using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02003B77 RID: 15223
	public class QuestNode_Root_MysteriousCargoRevenantSpine : QuestNode_Root_MysteriousCargo
	{
		// Token: 0x1700336E RID: 13166
		// (get) Token: 0x06014AF8 RID: 84728 RVA: 0x0062F406 File Offset: 0x0062D606
		protected override bool RequiresPawn { get; }

		// Token: 0x06014AF9 RID: 84729 RVA: 0x0062F40E File Offset: 0x0062D60E
		protected override Thing GenerateThing(Pawn _)
		{
			return ThingMaker.MakeThing(ThingDefOf.RevenantSpine, null);
		}
	}
}
