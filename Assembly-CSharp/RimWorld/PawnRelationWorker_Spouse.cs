using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200242F RID: 9263
	public class PawnRelationWorker_Spouse : PawnRelationWorker
	{
		// Token: 0x0600C875 RID: 51317 RVA: 0x003A81E8 File Offset: 0x003A63E8
		public override float GenerationChance(Pawn generated, Pawn other, PawnGenerationRequest request)
		{
			return LovePartnerRelationUtility.LovePartnerRelationGenerationChance(generated, other, request, false) * base.BaseGenerationChanceFactor(generated, other, request);
		}

		// Token: 0x0600C876 RID: 51318 RVA: 0x003A8D07 File Offset: 0x003A6F07
		public override void CreateRelation(Pawn generated, Pawn other, ref PawnGenerationRequest request)
		{
			generated.relations.AddDirectRelation(PawnRelationDefOf.Spouse, other);
			LovePartnerRelationUtility.TryToShareChildrenForGeneratedLovePartner(generated, other, request, 1f);
			SpouseRelationUtility.ResolveNameForSpouseOnGeneration(ref request, generated);
		}
	}
}
