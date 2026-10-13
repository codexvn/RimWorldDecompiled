using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200241F RID: 9247
	public class PawnRelationWorker_Fiance : PawnRelationWorker
	{
		// Token: 0x0600C847 RID: 51271 RVA: 0x003A7EF8 File Offset: 0x003A60F8
		public override float GenerationChance(Pawn generated, Pawn other, PawnGenerationRequest request)
		{
			float num = 1f;
			num *= this.GetOldAgeFactor(generated);
			num *= this.GetOldAgeFactor(other);
			return LovePartnerRelationUtility.LovePartnerRelationGenerationChance(generated, other, request, false) * base.BaseGenerationChanceFactor(generated, other, request) * num;
		}

		// Token: 0x0600C848 RID: 51272 RVA: 0x003A7F34 File Offset: 0x003A6134
		public override void CreateRelation(Pawn generated, Pawn other, ref PawnGenerationRequest request)
		{
			generated.relations.AddDirectRelation(PawnRelationDefOf.Fiance, other);
			LovePartnerRelationUtility.TryToShareChildrenForGeneratedLovePartner(generated, other, request, 0.7f);
		}

		// Token: 0x0600C849 RID: 51273 RVA: 0x003A7F59 File Offset: 0x003A6159
		private float GetOldAgeFactor(Pawn pawn)
		{
			return Mathf.Clamp(GenMath.LerpDouble(50f, 80f, 1f, 0.01f, (float)pawn.ageTracker.AgeBiologicalYears), 0.01f, 1f);
		}

		// Token: 0x0600C84A RID: 51274 RVA: 0x003A7F90 File Offset: 0x003A6190
		public override void OnRelationCreated(Pawn firstPawn, Pawn secondPawn)
		{
			Pawn pawn;
			if (firstPawn.Ideo == secondPawn.Ideo)
			{
				pawn = firstPawn;
			}
			else
			{
				pawn = ((Rand.Value < 0.5f) ? firstPawn : secondPawn);
			}
			firstPawn.relations.nextMarriageNameChange = (secondPawn.relations.nextMarriageNameChange = SpouseRelationUtility.Roll_NameChangeOnMarriage(pawn));
		}
	}
}
