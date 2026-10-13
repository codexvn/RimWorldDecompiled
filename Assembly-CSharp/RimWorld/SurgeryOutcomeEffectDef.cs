using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020037BD RID: 14269
	public class SurgeryOutcomeEffectDef : Def
	{
		// Token: 0x06013C5F RID: 80991 RVA: 0x005DE51C File Offset: 0x005DC71C
		public float GetQuality(RecipeDef recipe, Pawn surgeon, Pawn patient, List<Thing> ingredients, BodyPartRecord part, Bill bill)
		{
			float result = 1f;
			if (!this.comps.NullOrEmpty<SurgeryOutcomeComp>())
			{
				for (int i = 0; i < this.comps.Count; i++)
				{
					if (this.comps[i].Affects(recipe, surgeon, patient, part))
					{
						this.comps[i].AffectQuality(recipe, surgeon, patient, ingredients, part, bill, ref result);
					}
				}
			}
			return result;
		}

		// Token: 0x06013C60 RID: 80992 RVA: 0x005DE588 File Offset: 0x005DC788
		public SurgeryOutcome GetOutcome(RecipeDef recipe, Pawn surgeon, Pawn patient, List<Thing> ingredients, BodyPartRecord part, Bill bill)
		{
			float quality = this.GetQuality(recipe, surgeon, patient, ingredients, part, bill);
			if (!this.comps.NullOrEmpty<SurgeryOutcomeComp>())
			{
				for (int i = 0; i < this.comps.Count; i++)
				{
					if (this.comps[i].Affects(recipe, surgeon, patient, part))
					{
						this.comps[i].PreApply(quality, recipe, surgeon, patient, ingredients, part, bill);
					}
				}
			}
			if (!this.outcomes.NullOrEmpty<SurgeryOutcome>())
			{
				for (int j = 0; j < this.outcomes.Count; j++)
				{
					if (this.outcomes[j].Apply(quality, recipe, surgeon, patient, part))
					{
						return this.outcomes[j];
					}
				}
			}
			return null;
		}

		// Token: 0x0400DAFC RID: 56060
		public List<SurgeryOutcomeComp> comps;

		// Token: 0x0400DAFD RID: 56061
		public List<SurgeryOutcome> outcomes;
	}
}
