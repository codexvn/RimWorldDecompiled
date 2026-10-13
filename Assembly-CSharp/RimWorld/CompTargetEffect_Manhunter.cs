using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002F36 RID: 12086
	public class CompTargetEffect_Manhunter : CompTargetEffect
	{
		// Token: 0x060110DC RID: 69852 RVA: 0x004E6A18 File Offset: 0x004E4C18
		public override void DoEffectOn(Pawn user, Thing target)
		{
			Pawn pawn = (Pawn)target;
			if (pawn.Dead)
			{
				return;
			}
			if (!pawn.Awake())
			{
				RestUtility.WakeUp(pawn, true);
			}
			pawn.mindState.mentalStateHandler.TryStartMentalState(MentalStateDefOf.Manhunter, null, false, false, false, null, false, false, false);
		}
	}
}
