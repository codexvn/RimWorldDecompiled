using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020032C4 RID: 12996
	public class FloatMenuOptionProvider_CarryDeathrestingToCasket : FloatMenuOptionProvider
	{
		// Token: 0x17002F44 RID: 12100
		// (get) Token: 0x060124B7 RID: 74935 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected override bool Drafted
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17002F45 RID: 12101
		// (get) Token: 0x060124B8 RID: 74936 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected override bool Undrafted
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17002F46 RID: 12102
		// (get) Token: 0x060124B9 RID: 74937 RVA: 0x00002501 File Offset: 0x00000701
		protected override bool Multiselect
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17002F47 RID: 12103
		// (get) Token: 0x060124BA RID: 74938 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected override bool RequiresManipulation
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060124BB RID: 74939 RVA: 0x00563089 File Offset: 0x00561289
		protected override bool AppliesInt(FloatMenuContext context)
		{
			return ModsConfig.BiotechActive;
		}

		// Token: 0x060124BC RID: 74940 RVA: 0x00563A9C File Offset: 0x00561C9C
		protected override FloatMenuOption GetSingleOptionFor(Pawn clickedPawn, FloatMenuContext context)
		{
			if (!clickedPawn.Deathresting || clickedPawn.InBed())
			{
				return null;
			}
			if (!clickedPawn.IsColonist && !clickedPawn.IsPrisonerOfColony)
			{
				return null;
			}
			if (!context.FirstSelectedPawn.CanReach(clickedPawn, PathEndMode.ClosestTouch, Danger.Deadly, false, false, TraverseMode.ByPawn))
			{
				return new FloatMenuOption("CannotCarry".Translate(clickedPawn) + ": " + "NoPath".Translate().CapitalizeFirst(), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
			}
			Thing bestBedOrCasket = GenClosest.ClosestThingReachable(clickedPawn.PositionHeld, context.FirstSelectedPawn.Map, ThingRequest.ForDef(ThingDefOf.DeathrestCasket), PathEndMode.ClosestTouch, TraverseParms.For(context.FirstSelectedPawn, Danger.Deadly, TraverseMode.ByPawn, false, false, false, true), 9999f, (Thing casket) => casket.Faction == Faction.OfPlayer && RestUtility.IsValidBedFor(casket, clickedPawn, context.FirstSelectedPawn, true, false, false, clickedPawn.GuestStatus), null, 0, -1, false, RegionType.Set_Passable, false, false);
			if (bestBedOrCasket == null)
			{
				bestBedOrCasket = RestUtility.FindBedFor(clickedPawn, context.FirstSelectedPawn, false, false, null);
			}
			if (bestBedOrCasket == null)
			{
				return new FloatMenuOption("CannotCarry".Translate(clickedPawn) + ": " + "NoCasketOrBed".Translate(), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
			}
			return FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption("CarryToSpecificThing".Translate(bestBedOrCasket), delegate()
			{
				Job job = JobMaker.MakeJob(JobDefOf.DeliverToBed, clickedPawn, bestBedOrCasket);
				job.count = 1;
				context.FirstSelectedPawn.jobs.TryTakeOrderedJob(job, new JobTag?(JobTag.Misc), false);
			}, MenuOptionPriority.RescueOrCapture, null, clickedPawn, 0f, null, null, true, 0), context.FirstSelectedPawn, clickedPawn, "ReservedBy", null);
		}
	}
}
