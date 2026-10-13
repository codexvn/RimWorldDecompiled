using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020037CB RID: 14283
	public class SurgeryOutcome_Death : SurgeryOutcome
	{
		// Token: 0x06013C84 RID: 81028 RVA: 0x005DE908 File Offset: 0x005DCB08
		public override bool Apply(float quality, RecipeDef recipe, Pawn surgeon, Pawn patient, BodyPartRecord part)
		{
			if (ModsConfig.BiotechActive && patient.genes != null && patient.genes.HasActiveGene(GeneDefOf.Deathless))
			{
				return false;
			}
			if (Rand.Chance(recipe.deathOnFailedSurgeryChance))
			{
				base.ApplyDamage(patient, part);
				if (!patient.Dead)
				{
					patient.Kill(null, null);
				}
				base.SendLetter(surgeon, patient, recipe);
				return true;
			}
			return false;
		}
	}
}
