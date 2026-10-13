using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000F8A RID: 3978
	public class MentalState_BabyGiggle : MentalState_BabyFit
	{
		// Token: 0x0600622A RID: 25130 RVA: 0x001F028C File Offset: 0x001EE48C
		protected override void AuraEffect(Thing source, Pawn hearer)
		{
			Pawn pawn = source as Pawn;
			if (pawn != null && hearer.needs.mood != null)
			{
				if (hearer == pawn.GetMother() || hearer == pawn.GetFather())
				{
					hearer.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.MyGigglingBaby, pawn, null);
				}
				else
				{
					hearer.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.GigglingBaby, pawn, null);
				}
				hearer.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.BabyGiggledSocial, pawn, null);
			}
		}
	}
}
