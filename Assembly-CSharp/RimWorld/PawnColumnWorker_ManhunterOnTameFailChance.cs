using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02003484 RID: 13444
	public class PawnColumnWorker_ManhunterOnTameFailChance : PawnColumnWorker_Text
	{
		// Token: 0x06012E7E RID: 77438 RVA: 0x00599574 File Offset: 0x00597774
		protected override string GetTextFor(Pawn pawn)
		{
			float manhunterOnTameFailChance = PawnUtility.GetManhunterOnTameFailChance(pawn);
			if (manhunterOnTameFailChance == 0f)
			{
				return "-";
			}
			return manhunterOnTameFailChance.ToStringPercent();
		}

		// Token: 0x06012E7F RID: 77439 RVA: 0x0059959C File Offset: 0x0059779C
		protected override string GetTip(Pawn pawn)
		{
			return PawnUtility.GetManhunterOnTameFailChanceExplanation(pawn.def, pawn);
		}

		// Token: 0x06012E80 RID: 77440 RVA: 0x005995AC File Offset: 0x005977AC
		public override int Compare(Pawn a, Pawn b)
		{
			return PawnUtility.GetManhunterOnTameFailChance(a).CompareTo(PawnUtility.GetManhunterOnTameFailChance(b));
		}
	}
}
