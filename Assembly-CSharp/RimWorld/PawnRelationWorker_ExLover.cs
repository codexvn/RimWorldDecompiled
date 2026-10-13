using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200241D RID: 9245
	public class PawnRelationWorker_ExLover : PawnRelationWorker
	{
		// Token: 0x0600C841 RID: 51265 RVA: 0x003A7E96 File Offset: 0x003A6096
		public override float GenerationChance(Pawn generated, Pawn other, PawnGenerationRequest request)
		{
			return LovePartnerRelationUtility.LovePartnerRelationGenerationChance(generated, other, request, true) * base.BaseGenerationChanceFactor(generated, other, request);
		}

		// Token: 0x0600C842 RID: 51266 RVA: 0x003A7EAB File Offset: 0x003A60AB
		public override void CreateRelation(Pawn generated, Pawn other, ref PawnGenerationRequest request)
		{
			generated.relations.AddDirectRelation(PawnRelationDefOf.ExLover, other);
			LovePartnerRelationUtility.TryToShareChildrenForGeneratedLovePartner(generated, other, request, 0.35f);
		}
	}
}
