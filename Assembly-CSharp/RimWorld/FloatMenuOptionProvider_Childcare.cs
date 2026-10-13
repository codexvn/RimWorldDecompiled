using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020032DC RID: 13020
	public class FloatMenuOptionProvider_Childcare : FloatMenuOptionProvider
	{
		// Token: 0x17002F67 RID: 12135
		// (get) Token: 0x06012521 RID: 75041 RVA: 0x00002501 File Offset: 0x00000701
		protected override bool Drafted
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17002F68 RID: 12136
		// (get) Token: 0x06012522 RID: 75042 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected override bool Undrafted
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17002F69 RID: 12137
		// (get) Token: 0x06012523 RID: 75043 RVA: 0x00002501 File Offset: 0x00000701
		protected override bool Multiselect
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17002F6A RID: 12138
		// (get) Token: 0x06012524 RID: 75044 RVA: 0x000028E7 File Offset: 0x00000AE7
		public override bool CanTargetDespawned
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06012525 RID: 75045 RVA: 0x00563089 File Offset: 0x00561289
		protected override bool AppliesInt(FloatMenuContext context)
		{
			return ModsConfig.BiotechActive;
		}

		// Token: 0x06012526 RID: 75046 RVA: 0x00565E8A File Offset: 0x0056408A
		public override IEnumerable<FloatMenuOption> GetOptionsFor(Pawn clickedPawn, FloatMenuContext context)
		{
			FloatMenuOptionProvider_Childcare.<>c__DisplayClass9_0 CS$<>8__locals1 = new FloatMenuOptionProvider_Childcare.<>c__DisplayClass9_0();
			CS$<>8__locals1.context = context;
			ChildcareUtility.BreastfeedFailReason? breastfeedFailReason;
			if (!ChildcareUtility.CanSuckle(clickedPawn, out breastfeedFailReason))
			{
				yield break;
			}
			Job job;
			if (ChildcareUtility.CanBreastfeed(CS$<>8__locals1.context.FirstSelectedPawn, out breastfeedFailReason))
			{
				if (!ChildcareUtility.HasBreastfeedCompatibleFactions(CS$<>8__locals1.context.FirstSelectedPawn, clickedPawn))
				{
					yield break;
				}
				ChildcareUtility.BreastfeedFailReason? breastfeedFailReason2;
				if (!ChildcareUtility.CanMomAutoBreastfeedBabyNow(CS$<>8__locals1.context.FirstSelectedPawn, clickedPawn, true, out breastfeedFailReason2))
				{
					TaggedString taggedString = "BabyCareBreastfeedUnable".Translate(clickedPawn.Named("BABY")) + ": " + breastfeedFailReason2.Value.Translate(CS$<>8__locals1.context.FirstSelectedPawn, CS$<>8__locals1.context.FirstSelectedPawn, clickedPawn).CapitalizeFirst();
					yield return new FloatMenuOption(taggedString, null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
				}
				else
				{
					Job job = ChildcareUtility.MakeBreastfeedJob(clickedPawn, null);
					if (CS$<>8__locals1.context.FirstSelectedPawn.jobs.curJob != null && CS$<>8__locals1.context.FirstSelectedPawn.jobs.curJob.JobIsSameAs(CS$<>8__locals1.context.FirstSelectedPawn, job))
					{
						yield return new FloatMenuOption("AlreadyBreastfeeding".Translate(clickedPawn.Named("BABY")), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
					}
					else
					{
						yield return FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption("BabyCareBreastfeed".Translate(clickedPawn.Named("BABY")), delegate()
						{
							CS$<>8__locals1.context.FirstSelectedPawn.jobs.TryTakeOrderedJob(job, new JobTag?(JobTag.Misc), false);
						}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0), CS$<>8__locals1.context.FirstSelectedPawn, clickedPawn, "ReservedBy", null);
					}
				}
			}
			if (CaravanFormingUtility.IsFormingCaravanOrDownedPawnToBeTakenByCaravan(clickedPawn))
			{
				yield break;
			}
			ChildcareUtility.BreastfeedFailReason? breastfeedFailReason3;
			if (!ChildcareUtility.CanHaulBabyNow(CS$<>8__locals1.context.FirstSelectedPawn, clickedPawn, false, out breastfeedFailReason3))
			{
				Pawn pawn;
				if (CS$<>8__locals1.context.FirstSelectedPawn.MapHeld.reservationManager.TryGetReserver(clickedPawn, CS$<>8__locals1.context.FirstSelectedPawn.Faction, out pawn))
				{
					yield return new FloatMenuOption(string.Format("{0}: {1} {2}", "CannotCarryToSafePlace".Translate(), clickedPawn.LabelShort, "ReservedBy".Translate(pawn.LabelShort, pawn).Resolve().StripTags()), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
				}
				else
				{
					if (breastfeedFailReason3 != null)
					{
						breastfeedFailReason = breastfeedFailReason3;
						ChildcareUtility.BreastfeedFailReason breastfeedFailReason4 = ChildcareUtility.BreastfeedFailReason.HaulerCannotReachBaby;
						if (breastfeedFailReason.GetValueOrDefault() == breastfeedFailReason4 & breastfeedFailReason != null)
						{
							yield return new FloatMenuOption(string.Format("{0}: {1}", "CannotCarryToSafePlace".Translate(), "NoPath".Translate().CapitalizeFirst()), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
							goto IL_470;
						}
					}
					yield return new FloatMenuOption(string.Format("{0}: {1}", "CannotCarryToSafePlace".Translate(), "Incapable".Translate().CapitalizeFirst()), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
				}
				IL_470:
				yield break;
			}
			LocalTargetInfo targetB = ChildcareUtility.SafePlaceForBaby(clickedPawn, CS$<>8__locals1.context.FirstSelectedPawn, false);
			if (!targetB.IsValid)
			{
				yield break;
			}
			Building_Bed building_Bed = targetB.Thing as Building_Bed;
			if (building_Bed != null)
			{
				if (clickedPawn.CurrentBed() == building_Bed)
				{
					yield break;
				}
			}
			else if (clickedPawn.Spawned && clickedPawn.Position == targetB.Cell)
			{
				yield return new FloatMenuOption(string.Format("{0}: {1}", "CannotCarryToSafePlace".Translate(), "NoBetterSafePlace".Translate().CapitalizeFirst()), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
				yield break;
			}
			job = JobMaker.MakeJob(JobDefOf.BringBabyToSafety, clickedPawn, targetB);
			if (CS$<>8__locals1.context.FirstSelectedPawn.jobs.curJob != null && CS$<>8__locals1.context.FirstSelectedPawn.jobs.curJob.JobIsSameAs(CS$<>8__locals1.context.FirstSelectedPawn, job))
			{
				yield return new FloatMenuOption("AlreadyCarryingToSafePlace".Translate(clickedPawn.Named("BABY")), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
			}
			else
			{
				yield return FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption("CarryToSafePlace".Translate(clickedPawn.Named("BABY")), delegate()
				{
					job.count = 1;
					CS$<>8__locals1.context.FirstSelectedPawn.jobs.TryTakeOrderedJob(job, new JobTag?(JobTag.Misc), false);
				}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0), CS$<>8__locals1.context.FirstSelectedPawn, clickedPawn, "ReservedBy", null);
			}
			yield break;
		}
	}
}
