using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02003483 RID: 13443
	public class PawnColumnWorker_ManhunterOnDamageChance : PawnColumnWorker_Text
	{
		// Token: 0x06012E7A RID: 77434 RVA: 0x00599508 File Offset: 0x00597708
		protected override string GetTextFor(Pawn pawn)
		{
			float manhunterOnDamageChance = PawnUtility.GetManhunterOnDamageChance(pawn, null, -1f);
			if (manhunterOnDamageChance == 0f)
			{
				return "-";
			}
			return manhunterOnDamageChance.ToStringPercent();
		}

		// Token: 0x06012E7B RID: 77435 RVA: 0x00599536 File Offset: 0x00597736
		protected override string GetTip(Pawn pawn)
		{
			return PawnUtility.GetManhunterOnDamageChanceExplanation(pawn.def, pawn);
		}

		// Token: 0x06012E7C RID: 77436 RVA: 0x00599544 File Offset: 0x00597744
		public override int Compare(Pawn a, Pawn b)
		{
			return PawnUtility.GetManhunterOnDamageChance(a, null, -1f).CompareTo(PawnUtility.GetManhunterOnDamageChance(b, null, -1f));
		}
	}
}
