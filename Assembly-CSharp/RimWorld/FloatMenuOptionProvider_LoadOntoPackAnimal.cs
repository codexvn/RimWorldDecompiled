using System;
using System.Collections.Generic;
using RimWorld.Planet;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02003312 RID: 13074
	public class FloatMenuOptionProvider_LoadOntoPackAnimal : FloatMenuOptionProvider
	{
		// Token: 0x17002FC5 RID: 12229
		// (get) Token: 0x06012630 RID: 75312 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected override bool Drafted
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17002FC6 RID: 12230
		// (get) Token: 0x06012631 RID: 75313 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected override bool Undrafted
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17002FC7 RID: 12231
		// (get) Token: 0x06012632 RID: 75314 RVA: 0x00002501 File Offset: 0x00000701
		protected override bool Multiselect
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06012633 RID: 75315 RVA: 0x0056B2CF File Offset: 0x005694CF
		protected override bool AppliesInt(FloatMenuContext context)
		{
			return !context.FirstSelectedPawn.IsFormingCaravan() && !context.map.IsPlayerHome;
		}

		// Token: 0x06012634 RID: 75316 RVA: 0x0056B2EE File Offset: 0x005694EE
		public override IEnumerable<FloatMenuOption> GetOptionsFor(Thing clickedThing, FloatMenuContext context)
		{
			if (clickedThing.def.category != ThingCategory.Item)
			{
				yield break;
			}
			if (!clickedThing.def.EverHaulable)
			{
				yield break;
			}
			Pawn bestPackAnimal = GiveToPackAnimalUtility.UsablePackAnimalWithTheMostFreeSpace(context.FirstSelectedPawn);
			if (bestPackAnimal == null)
			{
				yield break;
			}
			if (!context.FirstSelectedPawn.CanReach(clickedThing, PathEndMode.ClosestTouch, Danger.Deadly, false, false, TraverseMode.ByPawn))
			{
				yield return new FloatMenuOption("CannotGiveToPackAnimal".Translate(clickedThing.Label, clickedThing) + ": " + "NoPath".Translate().CapitalizeFirst(), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
			}
			else if (MassUtility.WillBeOverEncumberedAfterPickingUp(bestPackAnimal, clickedThing, 1))
			{
				yield return new FloatMenuOption("CannotGiveToPackAnimal".Translate(clickedThing.Label, clickedThing) + ": " + "TooHeavy".Translate(), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
			}
			else if (clickedThing.stackCount == 1)
			{
				yield return FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption("GiveToPackAnimal".Translate(clickedThing.Label, clickedThing), delegate()
				{
					clickedThing.SetForbidden(false, false);
					Job job = JobMaker.MakeJob(JobDefOf.GiveToPackAnimal, clickedThing);
					job.count = 1;
					context.FirstSelectedPawn.jobs.TryTakeOrderedJob(job, new JobTag?(JobTag.Misc), false);
				}, MenuOptionPriority.High, null, null, 0f, null, null, true, 0), context.FirstSelectedPawn, clickedThing, "ReservedBy", null);
			}
			else
			{
				if (MassUtility.WillBeOverEncumberedAfterPickingUp(bestPackAnimal, clickedThing, clickedThing.stackCount))
				{
					yield return new FloatMenuOption("CannotGiveToPackAnimalAll".Translate(clickedThing.Label, clickedThing) + ": " + "TooHeavy".Translate(), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
				}
				else
				{
					yield return FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption("GiveToPackAnimalAll".Translate(clickedThing.Label, clickedThing), delegate()
					{
						clickedThing.SetForbidden(false, false);
						Job job = JobMaker.MakeJob(JobDefOf.GiveToPackAnimal, clickedThing);
						job.count = clickedThing.stackCount;
						context.FirstSelectedPawn.jobs.TryTakeOrderedJob(job, new JobTag?(JobTag.Misc), false);
					}, MenuOptionPriority.High, null, null, 0f, null, null, true, 0), context.FirstSelectedPawn, clickedThing, "ReservedBy", null);
				}
				Action<int> <>9__3;
				yield return FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption("GiveToPackAnimalSome".Translate(clickedThing.LabelNoCount, clickedThing), delegate()
				{
					int num = Mathf.Min(MassUtility.CountToPickUpUntilOverEncumbered(bestPackAnimal, clickedThing), clickedThing.stackCount);
					string text = "GiveToPackAnimalCount".Translate(clickedThing.LabelNoCount, clickedThing);
					int from = 1;
					int to = num;
					Action<int> confirmAction;
					if ((confirmAction = <>9__3) == null)
					{
						confirmAction = (<>9__3 = delegate(int count)
						{
							clickedThing.SetForbidden(false, false);
							Job job = JobMaker.MakeJob(JobDefOf.GiveToPackAnimal, clickedThing);
							job.count = count;
							context.FirstSelectedPawn.jobs.TryTakeOrderedJob(job, new JobTag?(JobTag.Misc), false);
						});
					}
					Dialog_Slider window = new Dialog_Slider(text, from, to, confirmAction, int.MinValue, 1f);
					Find.WindowStack.Add(window);
				}, MenuOptionPriority.High, null, null, 0f, null, null, true, 0), context.FirstSelectedPawn, clickedThing, "ReservedBy", null);
			}
			yield break;
		}
	}
}
