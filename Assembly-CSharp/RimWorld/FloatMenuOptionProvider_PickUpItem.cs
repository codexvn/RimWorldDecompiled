using System;
using System.Collections.Generic;
using RimWorld.Planet;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200331D RID: 13085
	public class FloatMenuOptionProvider_PickUpItem : FloatMenuOptionProvider
	{
		// Token: 0x17002FD6 RID: 12246
		// (get) Token: 0x06012669 RID: 75369 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected override bool Drafted
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17002FD7 RID: 12247
		// (get) Token: 0x0601266A RID: 75370 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected override bool Undrafted
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17002FD8 RID: 12248
		// (get) Token: 0x0601266B RID: 75371 RVA: 0x00002501 File Offset: 0x00000701
		protected override bool Multiselect
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0601266C RID: 75372 RVA: 0x0056C494 File Offset: 0x0056A694
		protected override bool AppliesInt(FloatMenuContext context)
		{
			return !context.FirstSelectedPawn.IsFormingCaravan();
		}

		// Token: 0x0601266D RID: 75373 RVA: 0x0056C4A4 File Offset: 0x0056A6A4
		public override IEnumerable<FloatMenuOption> GetOptionsFor(Thing clickedThing, FloatMenuContext context)
		{
			FloatMenuOptionProvider_PickUpItem.<>c__DisplayClass7_0 CS$<>8__locals1 = new FloatMenuOptionProvider_PickUpItem.<>c__DisplayClass7_0();
			CS$<>8__locals1.clickedThing = clickedThing;
			CS$<>8__locals1.context = context;
			if (CS$<>8__locals1.clickedThing.def.category != ThingCategory.Item)
			{
				yield break;
			}
			if (!CS$<>8__locals1.clickedThing.def.EverHaulable || !PawnUtility.CanPickUp(CS$<>8__locals1.context.FirstSelectedPawn, CS$<>8__locals1.clickedThing.def))
			{
				yield break;
			}
			if (CS$<>8__locals1.context.FirstSelectedPawn.Map.IsPlayerHome && !JobGiver_DropUnusedInventory.ShouldKeepDrugInInventory(CS$<>8__locals1.context.FirstSelectedPawn, CS$<>8__locals1.clickedThing))
			{
				yield break;
			}
			if (!CS$<>8__locals1.context.FirstSelectedPawn.CanReach(CS$<>8__locals1.clickedThing, PathEndMode.ClosestTouch, Danger.Deadly, false, false, TraverseMode.ByPawn))
			{
				yield return new FloatMenuOption("CannotPickUp".Translate(CS$<>8__locals1.clickedThing.Label, CS$<>8__locals1.clickedThing) + ": " + "NoPath".Translate().CapitalizeFirst(), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
			}
			else if (MassUtility.WillBeOverEncumberedAfterPickingUp(CS$<>8__locals1.context.FirstSelectedPawn, CS$<>8__locals1.clickedThing, 1))
			{
				yield return new FloatMenuOption("CannotPickUp".Translate(CS$<>8__locals1.clickedThing.Label, CS$<>8__locals1.clickedThing) + ": " + "TooHeavy".Translate(), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
			}
			else
			{
				FloatMenuOptionProvider_PickUpItem.<>c__DisplayClass7_1 CS$<>8__locals2 = new FloatMenuOptionProvider_PickUpItem.<>c__DisplayClass7_1();
				CS$<>8__locals2.CS$<>8__locals1 = CS$<>8__locals1;
				CS$<>8__locals2.maxAllowedToPickUp = PawnUtility.GetMaxAllowedToPickUp(CS$<>8__locals2.CS$<>8__locals1.context.FirstSelectedPawn, CS$<>8__locals2.CS$<>8__locals1.clickedThing.def);
				if (CS$<>8__locals2.maxAllowedToPickUp == 0)
				{
					yield return new FloatMenuOption("CannotPickUp".Translate(CS$<>8__locals2.CS$<>8__locals1.clickedThing.Label, CS$<>8__locals2.CS$<>8__locals1.clickedThing) + ": " + "MaxPickUpAllowed".Translate(CS$<>8__locals2.CS$<>8__locals1.clickedThing.def.orderedTakeGroup.max, CS$<>8__locals2.CS$<>8__locals1.clickedThing.def.orderedTakeGroup.label), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
				}
				else if (CS$<>8__locals2.CS$<>8__locals1.clickedThing.stackCount == 1 || CS$<>8__locals2.maxAllowedToPickUp == 1)
				{
					yield return FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption("PickUpOne".Translate(CS$<>8__locals2.CS$<>8__locals1.clickedThing.LabelNoCount, CS$<>8__locals2.CS$<>8__locals1.clickedThing), delegate()
					{
						CS$<>8__locals2.CS$<>8__locals1.clickedThing.SetForbidden(false, false);
						Job job = JobMaker.MakeJob(JobDefOf.TakeInventory, CS$<>8__locals2.CS$<>8__locals1.clickedThing);
						job.count = 1;
						job.checkEncumbrance = true;
						job.takeInventoryDelay = 120;
						CS$<>8__locals2.CS$<>8__locals1.context.FirstSelectedPawn.jobs.TryTakeOrderedJob(job, new JobTag?(JobTag.Misc), false);
					}, MenuOptionPriority.High, null, null, 0f, null, null, true, 0), CS$<>8__locals2.CS$<>8__locals1.context.FirstSelectedPawn, CS$<>8__locals2.CS$<>8__locals1.clickedThing, "ReservedBy", null);
				}
				else
				{
					if (CS$<>8__locals2.maxAllowedToPickUp < CS$<>8__locals2.CS$<>8__locals1.clickedThing.stackCount)
					{
						yield return new FloatMenuOption("CannotPickUpAll".Translate(CS$<>8__locals2.CS$<>8__locals1.clickedThing.Label, CS$<>8__locals2.CS$<>8__locals1.clickedThing) + ": " + "MaxPickUpAllowed".Translate(CS$<>8__locals2.CS$<>8__locals1.clickedThing.def.orderedTakeGroup.max, CS$<>8__locals2.CS$<>8__locals1.clickedThing.def.orderedTakeGroup.label), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
					}
					else if (MassUtility.WillBeOverEncumberedAfterPickingUp(CS$<>8__locals2.CS$<>8__locals1.context.FirstSelectedPawn, CS$<>8__locals2.CS$<>8__locals1.clickedThing, CS$<>8__locals2.CS$<>8__locals1.clickedThing.stackCount))
					{
						yield return new FloatMenuOption("CannotPickUpAll".Translate(CS$<>8__locals2.CS$<>8__locals1.clickedThing.Label, CS$<>8__locals2.CS$<>8__locals1.clickedThing) + ": " + "TooHeavy".Translate(), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
					}
					else
					{
						yield return FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption("PickUpAll".Translate(CS$<>8__locals2.CS$<>8__locals1.clickedThing.Label, CS$<>8__locals2.CS$<>8__locals1.clickedThing), delegate()
						{
							CS$<>8__locals2.CS$<>8__locals1.clickedThing.SetForbidden(false, false);
							Job job = JobMaker.MakeJob(JobDefOf.TakeInventory, CS$<>8__locals2.CS$<>8__locals1.clickedThing);
							job.count = CS$<>8__locals2.CS$<>8__locals1.clickedThing.stackCount;
							job.checkEncumbrance = true;
							job.takeInventoryDelay = 120;
							CS$<>8__locals2.CS$<>8__locals1.context.FirstSelectedPawn.jobs.TryTakeOrderedJob(job, new JobTag?(JobTag.Misc), false);
						}, MenuOptionPriority.High, null, null, 0f, null, null, true, 0), CS$<>8__locals2.CS$<>8__locals1.context.FirstSelectedPawn, CS$<>8__locals2.CS$<>8__locals1.clickedThing, "ReservedBy", null);
					}
					yield return FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption("PickUpSome".Translate(CS$<>8__locals2.CS$<>8__locals1.clickedThing.LabelNoCount, CS$<>8__locals2.CS$<>8__locals1.clickedThing), delegate()
					{
						int b = Mathf.Min(MassUtility.CountToPickUpUntilOverEncumbered(CS$<>8__locals2.CS$<>8__locals1.context.FirstSelectedPawn, CS$<>8__locals2.CS$<>8__locals1.clickedThing), CS$<>8__locals2.CS$<>8__locals1.clickedThing.stackCount);
						int num = Mathf.Min(CS$<>8__locals2.maxAllowedToPickUp, b);
						string text = "PickUpCount".Translate(CS$<>8__locals2.CS$<>8__locals1.clickedThing.LabelNoCount, CS$<>8__locals2.CS$<>8__locals1.clickedThing);
						int from = 1;
						int to = num;
						Action<int> confirmAction;
						if ((confirmAction = CS$<>8__locals2.CS$<>8__locals1.<>9__3) == null)
						{
							confirmAction = (CS$<>8__locals2.CS$<>8__locals1.<>9__3 = delegate(int count)
							{
								CS$<>8__locals2.CS$<>8__locals1.clickedThing.SetForbidden(false, false);
								Job job = JobMaker.MakeJob(JobDefOf.TakeInventory, CS$<>8__locals2.CS$<>8__locals1.clickedThing);
								job.count = count;
								job.checkEncumbrance = true;
								job.takeInventoryDelay = 120;
								CS$<>8__locals2.CS$<>8__locals1.context.FirstSelectedPawn.jobs.TryTakeOrderedJob(job, new JobTag?(JobTag.Misc), false);
							});
						}
						Dialog_Slider window = new Dialog_Slider(text, from, to, confirmAction, int.MinValue, 1f);
						Find.WindowStack.Add(window);
					}, MenuOptionPriority.High, null, null, 0f, null, null, true, 0), CS$<>8__locals2.CS$<>8__locals1.context.FirstSelectedPawn, CS$<>8__locals2.CS$<>8__locals1.clickedThing, "ReservedBy", null);
				}
				CS$<>8__locals2 = null;
			}
			yield break;
		}
	}
}
