using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002428 RID: 9256
	public class PawnRelationWorker_Lover : PawnRelationWorker
	{
		// Token: 0x0600C85C RID: 51292 RVA: 0x003A81E8 File Offset: 0x003A63E8
		public override float GenerationChance(Pawn generated, Pawn other, PawnGenerationRequest request)
		{
			return LovePartnerRelationUtility.LovePartnerRelationGenerationChance(generated, other, request, false) * base.BaseGenerationChanceFactor(generated, other, request);
		}

		// Token: 0x0600C85D RID: 51293 RVA: 0x003A81FD File Offset: 0x003A63FD
		public override void CreateRelation(Pawn generated, Pawn other, ref PawnGenerationRequest request)
		{
			generated.relations.AddDirectRelation(PawnRelationDefOf.Lover, other);
			LovePartnerRelationUtility.TryToShareChildrenForGeneratedLovePartner(generated, other, request, 0.35f);
		}
	}
}
