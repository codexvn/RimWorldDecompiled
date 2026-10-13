using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02002303 RID: 8963
	public class Recipe_InstallArtificialBodyPart : Recipe_Surgery
	{
		// Token: 0x0600C2C5 RID: 49861 RVA: 0x00387BB4 File Offset: 0x00385DB4
		public override IEnumerable<BodyPartRecord> GetPartsToApplyOn(Pawn pawn, RecipeDef recipe)
		{
			Func<Hediff, bool> <>9__2;
			return MedicalRecipesUtility.GetFixedPartsToApplyOn(recipe, pawn, delegate(BodyPartRecord record)
			{
				IEnumerable<Hediff> enumerable = from x in pawn.health.hediffSet.hediffs
				where x.Part == record
				select x;
				if (typeof(Hediff_AddedPart).IsAssignableFrom(recipe.addsHediff.hediffClass))
				{
					if (enumerable.Count<Hediff>() == 1 && enumerable.First<Hediff>().def == recipe.addsHediff)
					{
						return false;
					}
				}
				else
				{
					IEnumerable<Hediff> source = enumerable;
					Func<Hediff, bool> predicate;
					if ((predicate = <>9__2) == null)
					{
						predicate = (<>9__2 = ((Hediff hd) => hd.def == recipe.addsHediff));
					}
					if (source.Any(predicate))
					{
						return false;
					}
				}
				return (record.parent == null || pawn.health.hediffSet.GetNotMissingParts(BodyPartHeight.Undefined, BodyPartDepth.Undefined, null, null).Contains(record.parent)) && (!pawn.health.hediffSet.PartOrAnyAncestorHasDirectlyAddedParts(record) || pawn.health.hediffSet.HasDirectlyAddedPartFor(record));
			});
		}

		// Token: 0x0600C2C6 RID: 49862 RVA: 0x00387BF4 File Offset: 0x00385DF4
		public override void ApplyOnPawn(Pawn pawn, BodyPartRecord part, Pawn billDoer, List<Thing> ingredients, Bill bill)
		{
			bool flag = MedicalRecipesUtility.IsClean(pawn, part);
			bool flag2 = !PawnGenerator.IsBeingGenerated(pawn) && this.IsViolationOnPawn(pawn, part, Faction.OfPlayer);
			Hediff hediff = null;
			if (billDoer != null)
			{
				if (base.CheckSurgeryFail(billDoer, pawn, ingredients, part, bill))
				{
					return;
				}
				TaleRecorder.RecordTale(TaleDefOf.DidSurgery, new object[]
				{
					billDoer,
					pawn
				});
				hediff = pawn.health.hediffSet.GetDirectlyAddedPartFor(part);
				if (part != null)
				{
					MedicalRecipesUtility.RestorePartAndSpawnAllPreviousParts(pawn, part, billDoer.Position, billDoer.Map);
				}
				if (flag && flag2 && part.def.spawnThingOnRemoved != null)
				{
					ThoughtUtility.GiveThoughtsForPawnOrganHarvested(pawn, billDoer);
				}
				if (flag2)
				{
					base.ReportViolation(pawn, billDoer, pawn.HomeFaction, -70, null);
				}
				if (ModsConfig.IdeologyActive)
				{
					Find.HistoryEventsManager.RecordEvent(new HistoryEvent(HistoryEventDefOf.InstalledProsthetic, billDoer.Named(HistoryEventArgsNames.Doer)), true);
				}
			}
			else if (pawn.Map != null)
			{
				if (part != null)
				{
					MedicalRecipesUtility.RestorePartAndSpawnAllPreviousParts(pawn, part, pawn.Position, pawn.Map);
				}
			}
			else if (part != null)
			{
				pawn.health.RestorePart(part, null, true);
			}
			pawn.health.AddHediff(this.recipe.addsHediff, part, null, null);
			if (hediff != null)
			{
				hediff.Notify_SurgicallyReplaced(billDoer);
			}
		}

		// Token: 0x0600C2C7 RID: 49863 RVA: 0x00387D30 File Offset: 0x00385F30
		public override bool IsViolationOnPawn(Pawn pawn, BodyPartRecord part, Faction billDoerFaction)
		{
			return ((pawn.Faction != billDoerFaction && pawn.Faction != null) || pawn.IsQuestLodger()) && (this.recipe.addsHediff.addedPartProps == null || !this.recipe.addsHediff.addedPartProps.betterThanNatural) && HealthUtility.PartRemovalIntent(pawn, part) == BodyPartRemovalIntent.Harvest;
		}
	}
}
