using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02003343 RID: 13123
	public class FloatMenuOptionProvider_TransferEntity : FloatMenuOptionProvider
	{
		// Token: 0x1700300B RID: 12299
		// (get) Token: 0x06012717 RID: 75543 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected override bool Drafted
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700300C RID: 12300
		// (get) Token: 0x06012718 RID: 75544 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected override bool Undrafted
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700300D RID: 12301
		// (get) Token: 0x06012719 RID: 75545 RVA: 0x00002501 File Offset: 0x00000701
		protected override bool Multiselect
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700300E RID: 12302
		// (get) Token: 0x0601271A RID: 75546 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected override bool RequiresManipulation
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0601271B RID: 75547 RVA: 0x005631A8 File Offset: 0x005613A8
		protected override bool AppliesInt(FloatMenuContext context)
		{
			return ModsConfig.AnomalyActive;
		}

		// Token: 0x0601271C RID: 75548 RVA: 0x0056F837 File Offset: 0x0056DA37
		public override IEnumerable<FloatMenuOption> GetOptionsFor(Thing clickedThing, FloatMenuContext context)
		{
			CompEntityHolder compEntityHolder;
			if (!clickedThing.TryGetComp(out compEntityHolder) || compEntityHolder.HeldPawn == null)
			{
				yield break;
			}
			if (!context.FirstSelectedPawn.CanReserveAndReach(clickedThing, PathEndMode.OnCell, Danger.Deadly, 1, -1, null, true))
			{
				yield break;
			}
			Pawn heldPawn = compEntityHolder.HeldPawn;
			if (GenClosest.ClosestThing_Global_Reachable(context.FirstSelectedPawn.Position, context.FirstSelectedPawn.Map, context.FirstSelectedPawn.Map.listerBuildings.AllBuildingsColonistOfClass<Building_HoldingPlatform>(), PathEndMode.ClosestTouch, TraverseParms.For(context.FirstSelectedPawn, Danger.Some, TraverseMode.ByPawn, false, false, false, true), 9999f, delegate(Thing b)
			{
				Building_HoldingPlatform building_HoldingPlatform = b as Building_HoldingPlatform;
				return building_HoldingPlatform != null && !building_HoldingPlatform.Occupied && context.FirstSelectedPawn.CanReserve(building_HoldingPlatform, 1, -1, null, false);
			}, delegate(Thing t)
			{
				CompEntityHolder compEntityHolder2 = t.TryGetComp<CompEntityHolder>();
				if (compEntityHolder2 != null && compEntityHolder2.ContainmentStrength >= heldPawn.GetStatValue(StatDefOf.MinimumContainmentStrength, true, -1))
				{
					return compEntityHolder2.ContainmentStrength / Mathf.Max(heldPawn.PositionHeld.DistanceTo(t.Position), 1f);
				}
				return 0f;
			}, false) != null)
			{
				yield return FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption("TransferEntity".Translate(heldPawn) + " (" + "ChooseEntityHolder".Translate() + "...)", delegate()
				{
					StudyUtility.TargetHoldingPlatformForEntity(context.FirstSelectedPawn, heldPawn, true, clickedThing);
				}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0), context.FirstSelectedPawn, clickedThing, "ReservedBy", null);
			}
			yield break;
		}
	}
}
