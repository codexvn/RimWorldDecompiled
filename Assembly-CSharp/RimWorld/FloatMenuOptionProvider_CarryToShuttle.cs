using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020032DA RID: 13018
	public class FloatMenuOptionProvider_CarryToShuttle : FloatMenuOptionProvider
	{
		// Token: 0x17002F63 RID: 12131
		// (get) Token: 0x06012516 RID: 75030 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected override bool Drafted
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17002F64 RID: 12132
		// (get) Token: 0x06012517 RID: 75031 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected override bool Undrafted
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17002F65 RID: 12133
		// (get) Token: 0x06012518 RID: 75032 RVA: 0x00002501 File Offset: 0x00000701
		protected override bool Multiselect
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17002F66 RID: 12134
		// (get) Token: 0x06012519 RID: 75033 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected override bool RequiresManipulation
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0601251A RID: 75034 RVA: 0x00565C37 File Offset: 0x00563E37
		protected override bool AppliesInt(FloatMenuContext context)
		{
			return ModsConfig.RoyaltyActive;
		}

		// Token: 0x0601251B RID: 75035 RVA: 0x00565C40 File Offset: 0x00563E40
		protected override FloatMenuOption GetSingleOptionFor(Pawn clickedPawn, FloatMenuContext context)
		{
			FloatMenuOptionProvider_CarryToShuttle.<>c__DisplayClass9_0 CS$<>8__locals1 = new FloatMenuOptionProvider_CarryToShuttle.<>c__DisplayClass9_0();
			CS$<>8__locals1.clickedPawn = clickedPawn;
			CS$<>8__locals1.context = context;
			if (!this.CanBeCarriedToShuttle(CS$<>8__locals1.clickedPawn))
			{
				return null;
			}
			if (!CS$<>8__locals1.clickedPawn.Spawned)
			{
				return null;
			}
			if (!CS$<>8__locals1.clickedPawn.CanReserveAndReach(CS$<>8__locals1.clickedPawn, PathEndMode.OnCell, Danger.Deadly, 1, -1, null, true))
			{
				return null;
			}
			CS$<>8__locals1.shuttleThing = GenClosest.ClosestThingReachable(CS$<>8__locals1.clickedPawn.Position, CS$<>8__locals1.clickedPawn.Map, ThingRequest.ForDef(ThingDefOf.Shuttle), PathEndMode.ClosestTouch, TraverseParms.For(CS$<>8__locals1.context.FirstSelectedPawn, Danger.Deadly, TraverseMode.ByPawn, false, false, false, true), 9999f, new Predicate<Thing>(CS$<>8__locals1.<GetSingleOptionFor>g__IsValidShuttle|0), null, 0, -1, false, RegionType.Set_Passable, false, false);
			if (CS$<>8__locals1.shuttleThing == null)
			{
				return null;
			}
			if (CS$<>8__locals1.context.FirstSelectedPawn.WorkTypeIsDisabled(WorkTypeDefOf.Hauling))
			{
				return new FloatMenuOption("CannotLoadIntoShuttle".Translate(CS$<>8__locals1.shuttleThing) + ": " + "Incapable".Translate().CapitalizeFirst(), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
			}
			return FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption("CarryToShuttle".Translate(CS$<>8__locals1.clickedPawn), new Action(CS$<>8__locals1.<GetSingleOptionFor>g__CarryToShuttleAct|1), MenuOptionPriority.Default, null, null, 0f, null, null, true, 0), CS$<>8__locals1.context.FirstSelectedPawn, CS$<>8__locals1.clickedPawn, "ReservedBy", null);
		}

		// Token: 0x0601251C RID: 75036 RVA: 0x00565DC2 File Offset: 0x00563FC2
		private bool CanBeCarriedToShuttle(Pawn pawn)
		{
			if (pawn.IsPrisonerOfColony)
			{
				return pawn.guest.PrisonerIsSecure;
			}
			return pawn.AnimalOrWildMan() || pawn.Downed;
		}
	}
}
