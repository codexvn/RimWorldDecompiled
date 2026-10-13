using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02002312 RID: 8978
	public class Recipe_RemoveBodyPart_CutMany : Recipe_RemoveBodyPart_Cut
	{
		// Token: 0x0600C2F8 RID: 49912 RVA: 0x003887D6 File Offset: 0x003869D6
		public override IEnumerable<BodyPartRecord> GetPartsToApplyOn(Pawn pawn, RecipeDef recipe)
		{
			IEnumerable<BodyPartRecord> source = this.FixedPartsToApplyOn(pawn, recipe);
			if (source.Any<BodyPartRecord>() && recipe.minPartCount <= source.Count<BodyPartRecord>())
			{
				yield return source.First<BodyPartRecord>();
			}
			yield break;
		}

		// Token: 0x0600C2F9 RID: 49913 RVA: 0x003887F4 File Offset: 0x003869F4
		public IEnumerable<BodyPartRecord> FixedPartsToApplyOn(Pawn pawn, RecipeDef recipe)
		{
			return MedicalRecipesUtility.GetFixedPartsToApplyOn(recipe, pawn, (BodyPartRecord record) => !pawn.health.hediffSet.PartIsMissing(record));
		}

		// Token: 0x0600C2FA RID: 49914 RVA: 0x00388828 File Offset: 0x00386A28
		public override void DamagePart(Pawn pawn, BodyPartRecord part)
		{
			foreach (BodyPartRecord hitPart in this.FixedPartsToApplyOn(pawn, this.recipe))
			{
				pawn.TakeDamage(new DamageInfo(DamageDefOf.SurgicalCut, 99999f, 999f, -1f, null, hitPart, null, DamageInfo.SourceCategory.ThingOrUnknown, null, true, true, QualityCategory.Normal, true, false));
			}
		}

		// Token: 0x0600C2FB RID: 49915 RVA: 0x000489F6 File Offset: 0x00046BF6
		public override string GetLabelWhenUsedOn(Pawn pawn, BodyPartRecord part)
		{
			return this.recipe.label;
		}
	}
}
