using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02002310 RID: 8976
	public class Recipe_RemoveBodyPart_Cut : Recipe_RemoveBodyPart
	{
		// Token: 0x17001DCF RID: 7631
		// (get) Token: 0x0600C2F1 RID: 49905 RVA: 0x00002501 File Offset: 0x00000701
		protected override bool SpawnPartsWhenRemoved
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600C2F2 RID: 49906 RVA: 0x0038875C File Offset: 0x0038695C
		public override IEnumerable<BodyPartRecord> GetPartsToApplyOn(Pawn pawn, RecipeDef recipe)
		{
			return MedicalRecipesUtility.GetFixedPartsToApplyOn(recipe, pawn, (BodyPartRecord record) => !pawn.health.hediffSet.PartIsMissing(record));
		}

		// Token: 0x0600C2F3 RID: 49907 RVA: 0x0038878E File Offset: 0x0038698E
		public override void ApplyThoughts(Pawn pawn, Pawn billDoer)
		{
			if (pawn.Dead)
			{
				ThoughtUtility.GiveThoughtsForPawnExecuted(pawn, billDoer, PawnExecutionKind.GenericBrutal);
			}
		}

		// Token: 0x0600C2F4 RID: 49908 RVA: 0x003887A0 File Offset: 0x003869A0
		public override string GetLabelWhenUsedOn(Pawn pawn, BodyPartRecord part)
		{
			return "Cut".Translate();
		}
	}
}
