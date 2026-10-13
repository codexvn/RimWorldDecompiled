using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002F3C RID: 12092
	public class CompTargetEffect_MutateIntoFleshbeast : CompTargetEffect
	{
		// Token: 0x060110E8 RID: 69864 RVA: 0x004E6B74 File Offset: 0x004E4D74
		public override void DoEffectOn(Pawn user, Thing target)
		{
			Pawn pawn = target as Pawn;
			if (pawn != null)
			{
				FleshbeastUtility.MeatSplatter(CompTargetEffect_MutateIntoFleshbeast.BloodFilth.RandomInRange, pawn.PositionHeld, pawn.MapHeld, FleshbeastUtility.MeatExplosionSize.Large);
				FleshbeastUtility.SpawnFleshbeastFromPawn(pawn, false, true, Array.Empty<PawnKindDef>());
			}
		}

		// Token: 0x0400B129 RID: 45353
		private static readonly IntRange BloodFilth = new IntRange(3, 4);
	}
}
