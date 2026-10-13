using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020037CA RID: 14282
	public class SurgeryOutcomeSuccess : SurgeryOutcome
	{
		// Token: 0x06013C82 RID: 81026 RVA: 0x005DE8F7 File Offset: 0x005DCAF7
		public override bool Apply(float quality, RecipeDef recipe, Pawn surgeon, Pawn patient, BodyPartRecord part)
		{
			return Rand.Chance(quality);
		}
	}
}
