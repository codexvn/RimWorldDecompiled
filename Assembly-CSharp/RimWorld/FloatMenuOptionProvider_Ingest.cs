using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200330A RID: 13066
	public class FloatMenuOptionProvider_Ingest : FloatMenuOptionProvider
	{
		// Token: 0x17002FB9 RID: 12217
		// (get) Token: 0x0601260B RID: 75275 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected override bool Drafted
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17002FBA RID: 12218
		// (get) Token: 0x0601260C RID: 75276 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected override bool Undrafted
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17002FBB RID: 12219
		// (get) Token: 0x0601260D RID: 75277 RVA: 0x00002501 File Offset: 0x00000701
		protected override bool Multiselect
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0601260E RID: 75278 RVA: 0x0056A278 File Offset: 0x00568478
		protected override FloatMenuOption GetSingleOptionFor(Thing clickedThing, FloatMenuContext context)
		{
			if (clickedThing.def.ingestible == null || !clickedThing.def.ingestible.showIngestFloatOption)
			{
				return null;
			}
			if (!clickedThing.IngestibleNow || !context.FirstSelectedPawn.RaceProps.CanEverEat(clickedThing.def))
			{
				return null;
			}
			string text;
			if (clickedThing.def.ingestible.ingestCommandString.NullOrEmpty())
			{
				text = "ConsumeThing".Translate(clickedThing.LabelShort, clickedThing);
			}
			else
			{
				text = clickedThing.def.ingestible.ingestCommandString.Formatted(clickedThing.LabelShort);
			}
			if (!clickedThing.IsSociallyProper(context.FirstSelectedPawn))
			{
				text = text + ": " + "ReservedForPrisoners".Translate().CapitalizeFirst();
			}
			else if (FoodUtility.MoodFromIngesting(context.FirstSelectedPawn, clickedThing, clickedThing.def) < 0f)
			{
				text = string.Format("{0} ({1})", text, "WarningFoodDisliked".Translate());
			}
			if (!clickedThing.def.IsDrug && !context.FirstSelectedPawn.FoodIsSuitable(clickedThing.def))
			{
				return new FloatMenuOption(text + ": " + "FoodNotSuitable".Translate().CapitalizeFirst(), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
			}
			if (clickedThing.def.IsDrug && !context.FirstSelectedPawn.DrugIsSuitable(clickedThing.def))
			{
				return new FloatMenuOption(text + ": " + "DrugNotSuitable".Translate().CapitalizeFirst(), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
			}
			if (ModsConfig.IdeologyActive)
			{
				FloatMenuOption result;
				if (clickedThing.def.IsDrug && !new HistoryEvent(HistoryEventDefOf.IngestedDrug, context.FirstSelectedPawn.Named(HistoryEventArgsNames.Doer)).Notify_PawnAboutToDo(out result, text) && !PawnUtility.CanTakeDrugForDependency(context.FirstSelectedPawn, clickedThing.def))
				{
					return result;
				}
				if (clickedThing.def.IsNonMedicalDrug && !new HistoryEvent(HistoryEventDefOf.IngestedRecreationalDrug, context.FirstSelectedPawn.Named(HistoryEventArgsNames.Doer)).Notify_PawnAboutToDo(out result, text) && !PawnUtility.CanTakeDrugForDependency(context.FirstSelectedPawn, clickedThing.def))
				{
					return result;
				}
				if (clickedThing.def.IsDrug && clickedThing.def.ingestible.drugCategory == DrugCategory.Hard && !new HistoryEvent(HistoryEventDefOf.IngestedHardDrug, context.FirstSelectedPawn.Named(HistoryEventArgsNames.Doer)).Notify_PawnAboutToDo(out result, text))
				{
					return result;
				}
			}
			if (clickedThing.def.IsNonMedicalDrug && !context.FirstSelectedPawn.CanTakeDrug(clickedThing.def))
			{
				return new FloatMenuOption(text + ": " + TraitDefOf.DrugDesire.DataAtDegree(-1).GetLabelCapFor(context.FirstSelectedPawn), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
			}
			if (FoodUtility.InappropriateForTitle(clickedThing.def, context.FirstSelectedPawn, true))
			{
				return new FloatMenuOption(text + ": " + "FoodBelowTitleRequirements".Translate(context.FirstSelectedPawn.royalty.MostSeniorTitle.def.GetLabelFor(context.FirstSelectedPawn).CapitalizeFirst()).CapitalizeFirst(), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
			}
			if (!context.FirstSelectedPawn.CanReach(clickedThing, PathEndMode.OnCell, Danger.Deadly, false, false, TraverseMode.ByPawn))
			{
				return new FloatMenuOption(text + ": " + "NoPath".Translate().CapitalizeFirst(), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
			}
			bool maxAmountToPickup = FoodUtility.GetMaxAmountToPickup(clickedThing, context.FirstSelectedPawn, FoodUtility.WillIngestStackCountOf(context.FirstSelectedPawn, clickedThing.def, FoodUtility.NutritionForEater(context.FirstSelectedPawn, clickedThing))) != 0;
			FloatMenuOption floatMenuOption = FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption(text, delegate()
			{
				int maxAmountToPickup2 = FoodUtility.GetMaxAmountToPickup(clickedThing, context.FirstSelectedPawn, FoodUtility.WillIngestStackCountOf(context.FirstSelectedPawn, clickedThing.def, FoodUtility.NutritionForEater(context.FirstSelectedPawn, clickedThing)));
				if (maxAmountToPickup2 == 0)
				{
					return;
				}
				clickedThing.SetForbidden(false, true);
				Job job = JobMaker.MakeJob(JobDefOf.Ingest, clickedThing);
				job.count = maxAmountToPickup2;
				context.FirstSelectedPawn.jobs.TryTakeOrderedJob(job, new JobTag?(JobTag.Misc), false);
			}, (clickedThing is Corpse) ? MenuOptionPriority.Low : MenuOptionPriority.Default, null, null, 0f, null, null, true, 0), context.FirstSelectedPawn, clickedThing, "ReservedBy", null);
			if (!maxAmountToPickup)
			{
				floatMenuOption.action = null;
			}
			return floatMenuOption;
		}
	}
}
