using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020037BE RID: 14270
	public class SurgeryOutcomeComp
	{
		// Token: 0x06013C62 RID: 80994 RVA: 0x000028E7 File Offset: 0x00000AE7
		public virtual bool Affects(RecipeDef recipe, Pawn surgeon, Pawn patient, BodyPartRecord part)
		{
			return true;
		}

		// Token: 0x06013C63 RID: 80995 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void AffectQuality(RecipeDef recipe, Pawn surgeon, Pawn patient, List<Thing> ingredients, BodyPartRecord part, Bill bill, ref float quality)
		{
		}

		// Token: 0x06013C64 RID: 80996 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void PreApply(float quality, RecipeDef recipe, Pawn surgeon, Pawn patient, List<Thing> ingredients, BodyPartRecord part, Bill bill)
		{
		}
	}
}
