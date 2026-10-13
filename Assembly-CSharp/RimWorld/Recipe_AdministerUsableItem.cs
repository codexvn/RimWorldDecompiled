using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020022F5 RID: 8949
	public class Recipe_AdministerUsableItem : Recipe_Surgery
	{
		// Token: 0x0600C291 RID: 49809 RVA: 0x00386F1C File Offset: 0x0038511C
		public override void ApplyOnPawn(Pawn pawn, BodyPartRecord part, Pawn billDoer, List<Thing> ingredients, Bill bill)
		{
			CompUsable compUsable;
			if (!ingredients[0].TryGetComp(out compUsable))
			{
				return;
			}
			AcceptanceReport acceptanceReport = compUsable.CanBeUsedBy(pawn, false, true);
			if (acceptanceReport.Accepted)
			{
				compUsable.UsedBy(pawn);
				return;
			}
			if (!string.IsNullOrEmpty(acceptanceReport.Reason))
			{
				compUsable.SendCannotUseMessage(pawn, acceptanceReport.Reason);
			}
		}

		// Token: 0x0600C292 RID: 49810 RVA: 0x00386F74 File Offset: 0x00385174
		public override bool AvailableOnNow(Thing thing, BodyPartRecord part = null)
		{
			if (!base.AvailableOnNow(thing, part))
			{
				return false;
			}
			Pawn pawn = thing as Pawn;
			Hediff hediff;
			BodyPartRecord bodyPartRecord;
			return pawn != null && HealthUtility.TryGetWorstHealthCondition(pawn, out hediff, out bodyPartRecord, Array.Empty<HediffDef>());
		}

		// Token: 0x0600C293 RID: 49811 RVA: 0x000026EA File Offset: 0x000008EA
		public override void ConsumeIngredient(Thing ingredient, RecipeDef recipe, Map map)
		{
		}
	}
}
