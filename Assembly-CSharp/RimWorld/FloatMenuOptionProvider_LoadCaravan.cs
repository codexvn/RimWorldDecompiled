using System;
using System.Collections.Generic;
using RimWorld.Planet;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x0200330E RID: 13070
	public class FloatMenuOptionProvider_LoadCaravan : FloatMenuOptionProvider
	{
		// Token: 0x17002FC0 RID: 12224
		// (get) Token: 0x0601261B RID: 75291 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected override bool Drafted
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17002FC1 RID: 12225
		// (get) Token: 0x0601261C RID: 75292 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected override bool Undrafted
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17002FC2 RID: 12226
		// (get) Token: 0x0601261D RID: 75293 RVA: 0x00002501 File Offset: 0x00000701
		protected override bool Multiselect
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0601261E RID: 75294 RVA: 0x0056A9FB File Offset: 0x00568BFB
		protected override bool AppliesInt(FloatMenuContext context)
		{
			return context.FirstSelectedPawn.IsFormingCaravan();
		}

		// Token: 0x0601261F RID: 75295 RVA: 0x0056AA08 File Offset: 0x00568C08
		public override IEnumerable<FloatMenuOption> GetOptionsFor(Thing clickedThing, FloatMenuContext context)
		{
			FloatMenuOptionProvider_LoadCaravan.<>c__DisplayClass7_0 CS$<>8__locals1 = new FloatMenuOptionProvider_LoadCaravan.<>c__DisplayClass7_0();
			CS$<>8__locals1.clickedThing = clickedThing;
			CS$<>8__locals1.context = context;
			if (CS$<>8__locals1.clickedThing.def.category != ThingCategory.Item)
			{
				yield break;
			}
			if (!CS$<>8__locals1.clickedThing.def.EverHaulable || !CS$<>8__locals1.clickedThing.def.canLoadIntoCaravan)
			{
				yield break;
			}
			CS$<>8__locals1.packTarget = (GiveToPackAnimalUtility.UsablePackAnimalWithTheMostFreeSpace(CS$<>8__locals1.context.FirstSelectedPawn) ?? CS$<>8__locals1.context.FirstSelectedPawn);
			CS$<>8__locals1.jobDef = ((CS$<>8__locals1.packTarget == CS$<>8__locals1.context.FirstSelectedPawn) ? JobDefOf.TakeInventory : JobDefOf.GiveToPackAnimal);
			if (!CS$<>8__locals1.context.FirstSelectedPawn.CanReach(CS$<>8__locals1.clickedThing, PathEndMode.ClosestTouch, Danger.Deadly, false, false, TraverseMode.ByPawn))
			{
				yield return new FloatMenuOption("CannotLoadIntoCaravan".Translate(CS$<>8__locals1.clickedThing.Label, CS$<>8__locals1.clickedThing) + ": " + "NoPath".Translate().CapitalizeFirst(), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
			}
			else if (MassUtility.WillBeOverEncumberedAfterPickingUp(CS$<>8__locals1.packTarget, CS$<>8__locals1.clickedThing, 1))
			{
				yield return new FloatMenuOption("CannotLoadIntoCaravan".Translate(CS$<>8__locals1.clickedThing.Label, CS$<>8__locals1.clickedThing) + ": " + "TooHeavy".Translate(), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
			}
			else
			{
				FloatMenuOptionProvider_LoadCaravan.<>c__DisplayClass7_1 CS$<>8__locals2 = new FloatMenuOptionProvider_LoadCaravan.<>c__DisplayClass7_1();
				CS$<>8__locals2.CS$<>8__locals1 = CS$<>8__locals1;
				LordJob_FormAndSendCaravan lordJob = (LordJob_FormAndSendCaravan)CS$<>8__locals2.CS$<>8__locals1.context.FirstSelectedPawn.GetLord().LordJob;
				CS$<>8__locals2.capacityLeft = CaravanFormingUtility.CapacityLeft(lordJob);
				if (CS$<>8__locals2.CS$<>8__locals1.clickedThing.stackCount == 1)
				{
					float capacityLeft = CS$<>8__locals2.capacityLeft - CS$<>8__locals2.CS$<>8__locals1.clickedThing.GetStatValue(StatDefOf.Mass, true, -1);
					yield return FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption(CaravanFormingUtility.AppendOverweightInfo("LoadIntoCaravan".Translate(CS$<>8__locals2.CS$<>8__locals1.clickedThing.Label, CS$<>8__locals2.CS$<>8__locals1.clickedThing), capacityLeft), delegate()
					{
						CS$<>8__locals2.CS$<>8__locals1.clickedThing.SetForbidden(false, false);
						Job job = JobMaker.MakeJob(CS$<>8__locals2.CS$<>8__locals1.jobDef, CS$<>8__locals2.CS$<>8__locals1.clickedThing);
						job.count = 1;
						job.checkEncumbrance = (CS$<>8__locals2.CS$<>8__locals1.packTarget == CS$<>8__locals2.CS$<>8__locals1.context.FirstSelectedPawn);
						CS$<>8__locals2.CS$<>8__locals1.context.FirstSelectedPawn.jobs.TryTakeOrderedJob(job, new JobTag?(JobTag.Misc), false);
					}, MenuOptionPriority.High, null, null, 0f, null, null, true, 0), CS$<>8__locals2.CS$<>8__locals1.context.FirstSelectedPawn, CS$<>8__locals2.CS$<>8__locals1.clickedThing, "ReservedBy", null);
				}
				else
				{
					if (MassUtility.WillBeOverEncumberedAfterPickingUp(CS$<>8__locals2.CS$<>8__locals1.packTarget, CS$<>8__locals2.CS$<>8__locals1.clickedThing, CS$<>8__locals2.CS$<>8__locals1.clickedThing.stackCount))
					{
						yield return new FloatMenuOption("CannotLoadIntoCaravanAll".Translate(CS$<>8__locals2.CS$<>8__locals1.clickedThing.Label, CS$<>8__locals2.CS$<>8__locals1.clickedThing) + ": " + "TooHeavy".Translate(), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
					}
					else
					{
						float capacityLeft2 = CS$<>8__locals2.capacityLeft - (float)CS$<>8__locals2.CS$<>8__locals1.clickedThing.stackCount * CS$<>8__locals2.CS$<>8__locals1.clickedThing.GetStatValue(StatDefOf.Mass, true, -1);
						yield return FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption(CaravanFormingUtility.AppendOverweightInfo("LoadIntoCaravanAll".Translate(CS$<>8__locals2.CS$<>8__locals1.clickedThing.Label, CS$<>8__locals2.CS$<>8__locals1.clickedThing), capacityLeft2), delegate()
						{
							CS$<>8__locals2.CS$<>8__locals1.clickedThing.SetForbidden(false, false);
							Job job = JobMaker.MakeJob(CS$<>8__locals2.CS$<>8__locals1.jobDef, CS$<>8__locals2.CS$<>8__locals1.clickedThing);
							job.count = CS$<>8__locals2.CS$<>8__locals1.clickedThing.stackCount;
							job.checkEncumbrance = (CS$<>8__locals2.CS$<>8__locals1.packTarget == CS$<>8__locals2.CS$<>8__locals1.context.FirstSelectedPawn);
							CS$<>8__locals2.CS$<>8__locals1.context.FirstSelectedPawn.jobs.TryTakeOrderedJob(job, new JobTag?(JobTag.Misc), false);
						}, MenuOptionPriority.High, null, null, 0f, null, null, true, 0), CS$<>8__locals2.CS$<>8__locals1.context.FirstSelectedPawn, CS$<>8__locals2.CS$<>8__locals1.clickedThing, "ReservedBy", null);
					}
					yield return FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption("LoadIntoCaravanSome".Translate(CS$<>8__locals2.CS$<>8__locals1.clickedThing.LabelNoCount, CS$<>8__locals2.CS$<>8__locals1.clickedThing), delegate()
					{
						int num = Mathf.Min(MassUtility.CountToPickUpUntilOverEncumbered(CS$<>8__locals2.CS$<>8__locals1.packTarget, CS$<>8__locals2.CS$<>8__locals1.clickedThing), CS$<>8__locals2.CS$<>8__locals1.clickedThing.stackCount);
						Func<int, string> textGetter;
						if ((textGetter = CS$<>8__locals2.<>9__3) == null)
						{
							textGetter = (CS$<>8__locals2.<>9__3 = delegate(int val)
							{
								float capacityLeft3 = CS$<>8__locals2.capacityLeft - (float)val * CS$<>8__locals2.CS$<>8__locals1.clickedThing.GetStatValue(StatDefOf.Mass, true, -1);
								return CaravanFormingUtility.AppendOverweightInfo("LoadIntoCaravanCount".Translate(CS$<>8__locals2.CS$<>8__locals1.clickedThing.LabelNoCount, CS$<>8__locals2.CS$<>8__locals1.clickedThing).Formatted(val), capacityLeft3);
							});
						}
						int from = 1;
						int to = num;
						Action<int> confirmAction;
						if ((confirmAction = CS$<>8__locals2.CS$<>8__locals1.<>9__4) == null)
						{
							confirmAction = (CS$<>8__locals2.CS$<>8__locals1.<>9__4 = delegate(int count)
							{
								CS$<>8__locals2.CS$<>8__locals1.clickedThing.SetForbidden(false, false);
								Job job = JobMaker.MakeJob(CS$<>8__locals2.CS$<>8__locals1.jobDef, CS$<>8__locals2.CS$<>8__locals1.clickedThing);
								job.count = count;
								job.checkEncumbrance = (CS$<>8__locals2.CS$<>8__locals1.packTarget == CS$<>8__locals2.CS$<>8__locals1.context.FirstSelectedPawn);
								CS$<>8__locals2.CS$<>8__locals1.context.FirstSelectedPawn.jobs.TryTakeOrderedJob(job, new JobTag?(JobTag.Misc), false);
							});
						}
						Dialog_Slider window = new Dialog_Slider(textGetter, from, to, confirmAction, int.MinValue, 1f);
						Find.WindowStack.Add(window);
					}, MenuOptionPriority.High, null, null, 0f, null, null, true, 0), CS$<>8__locals2.CS$<>8__locals1.context.FirstSelectedPawn, CS$<>8__locals2.CS$<>8__locals1.clickedThing, "ReservedBy", null);
				}
				CS$<>8__locals2 = null;
			}
			yield break;
		}
	}
}
