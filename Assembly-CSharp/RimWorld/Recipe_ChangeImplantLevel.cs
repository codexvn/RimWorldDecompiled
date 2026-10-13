using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020022F8 RID: 8952
	public class Recipe_ChangeImplantLevel : Recipe_Surgery
	{
		// Token: 0x0600C29E RID: 49822 RVA: 0x003871CC File Offset: 0x003853CC
		private bool Operable(Hediff target, RecipeDef recipe)
		{
			int hediffLevelOffset = recipe.hediffLevelOffset;
			if (hediffLevelOffset == 0)
			{
				return false;
			}
			Hediff_Level hediff_Level = target as Hediff_Level;
			if (hediff_Level == null)
			{
				return false;
			}
			int level = hediff_Level.level;
			if (hediff_Level.def != recipe.changesHediffLevel)
			{
				return false;
			}
			if (hediffLevelOffset <= 0)
			{
				return level > 0;
			}
			return (float)level < hediff_Level.def.maxSeverity;
		}

		// Token: 0x0600C29F RID: 49823 RVA: 0x00387220 File Offset: 0x00385420
		public override IEnumerable<BodyPartRecord> GetPartsToApplyOn(Pawn pawn, RecipeDef recipe)
		{
			return MedicalRecipesUtility.GetFixedPartsToApplyOn(recipe, pawn, (BodyPartRecord record) => pawn.health.hediffSet.hediffs.Any((Hediff x) => x.Part == record && this.Operable(x, recipe)));
		}

		// Token: 0x0600C2A0 RID: 49824 RVA: 0x00387268 File Offset: 0x00385468
		public override void ApplyOnPawn(Pawn pawn, BodyPartRecord part, Pawn billDoer, List<Thing> ingredients, Bill bill)
		{
			if (billDoer != null)
			{
				TaleRecorder.RecordTale(TaleDefOf.DidSurgery, new object[]
				{
					billDoer,
					pawn
				});
			}
			Hediff_Level hediff_Level = (Hediff_Level)pawn.health.hediffSet.hediffs.FirstOrDefault((Hediff h) => this.Operable(h, this.recipe) && h.Part == part);
			if (hediff_Level != null)
			{
				if (this.IsViolationOnPawn(pawn, part, Faction.OfPlayer))
				{
					base.ReportViolation(pawn, billDoer, pawn.HomeFaction, -70, null);
				}
				hediff_Level.ChangeLevel(this.recipe.hediffLevelOffset);
			}
		}
	}
}
