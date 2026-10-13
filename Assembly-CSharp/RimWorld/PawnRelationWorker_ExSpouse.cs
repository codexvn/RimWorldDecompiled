using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200241E RID: 9246
	public class PawnRelationWorker_ExSpouse : PawnRelationWorker
	{
		// Token: 0x0600C844 RID: 51268 RVA: 0x003A7E96 File Offset: 0x003A6096
		public override float GenerationChance(Pawn generated, Pawn other, PawnGenerationRequest request)
		{
			return LovePartnerRelationUtility.LovePartnerRelationGenerationChance(generated, other, request, true) * base.BaseGenerationChanceFactor(generated, other, request);
		}

		// Token: 0x0600C845 RID: 51269 RVA: 0x003A7ED0 File Offset: 0x003A60D0
		public override void CreateRelation(Pawn generated, Pawn other, ref PawnGenerationRequest request)
		{
			generated.relations.AddDirectRelation(PawnRelationDefOf.ExSpouse, other);
			LovePartnerRelationUtility.TryToShareChildrenForGeneratedLovePartner(generated, other, request, 1f);
		}
	}
}
