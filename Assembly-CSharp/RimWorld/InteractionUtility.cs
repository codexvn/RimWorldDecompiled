using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001111 RID: 4369
	public static class InteractionUtility
	{
		// Token: 0x06006A8E RID: 27278 RVA: 0x0021A3CC File Offset: 0x002185CC
		public static void OrderInteraction(Pawn pawn, Thing thing, ThingDef optionalItem = null, int optionItemCount = 1)
		{
			Job job;
			if (optionalItem != null)
			{
				List<Thing> list = HaulAIUtility.FindFixedIngredientCount(pawn, optionalItem, optionItemCount);
				if (list.NullOrEmpty<Thing>())
				{
					return;
				}
				job = JobMaker.MakeJob(JobDefOf.InteractThing, thing, list[0]);
				job.targetQueueB = (from i in list.Skip(1)
				select new LocalTargetInfo(i)).ToList<LocalTargetInfo>();
			}
			else
			{
				job = JobMaker.MakeJob(JobDefOf.InteractThing, thing);
			}
			job.count = optionItemCount;
			job.playerForced = true;
			pawn.jobs.TryTakeOrderedJob(job, new JobTag?(JobTag.Misc), false);
		}

		// Token: 0x06006A8F RID: 27279 RVA: 0x0021A478 File Offset: 0x00218678
		public static void OrderInteraction(Pawn pawn, ThingComp comp, ThingDef optionalItem = null, int optionItemCount = 1)
		{
			if (comp.Isnt<CompInteractable>())
			{
				Log.Error("Cannot order an interaction from a comp (" + comp.GetType().Name + ") which does not inherit CompInteractable");
				return;
			}
			Job job;
			if (optionalItem != null)
			{
				List<Thing> list = HaulAIUtility.FindFixedIngredientCount(pawn, optionalItem, optionItemCount);
				if (list.NullOrEmpty<Thing>())
				{
					Log.Error("Failed to find required fixed ingredients for interaction");
					return;
				}
				job = JobMaker.MakeJob(JobDefOf.InteractThing, comp.parent, list[0]);
				job.targetQueueB = (from i in list.Skip(1)
				select new LocalTargetInfo(i)).ToList<LocalTargetInfo>();
			}
			else
			{
				job = JobMaker.MakeJob(JobDefOf.InteractThing, comp.parent);
			}
			job.count = optionItemCount;
			job.playerForced = true;
			job.interactableIndex = comp.parent.AllComps.IndexOf(comp);
			pawn.jobs.TryTakeOrderedJob(job, new JobTag?(JobTag.Misc), false);
		}
	}
}
