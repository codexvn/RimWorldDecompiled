using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002F2B RID: 12075
	public class CompTargetEffect_Berserk : CompTargetEffect
	{
		// Token: 0x060110C6 RID: 69830 RVA: 0x004E6738 File Offset: 0x004E4938
		public override void DoEffectOn(Pawn user, Thing target)
		{
			Pawn pawn = (Pawn)target;
			if (pawn.Dead)
			{
				return;
			}
			pawn.mindState.mentalStateHandler.TryStartMentalState(MentalStateDefOf.Berserk, null, false, true, false, null, false, false, false);
			Find.BattleLog.Add(new BattleLogEntry_ItemUsed(user, target, this.parent.def, RulePackDefOf.Event_ItemUsed));
		}
	}
}
