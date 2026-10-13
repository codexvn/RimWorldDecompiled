using System;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020014D4 RID: 5332
	public class JobGiver_TakeForInventoryStock : ThinkNode_JobGiver
	{
		// Token: 0x06007F76 RID: 32630 RVA: 0x00265DE4 File Offset: 0x00263FE4
		protected override Job TryGiveJob(Pawn pawn)
		{
			if (Find.TickManager.TicksGame < pawn.mindState.nextInventoryStockTick)
			{
				return null;
			}
			if (!pawn.inventoryStock.AnyThingsRequiredNow())
			{
				return null;
			}
			if (pawn.inventory.UnloadEverything)
			{
				return null;
			}
			foreach (InventoryStockEntry inventoryStockEntry in pawn.inventoryStock.stockEntries.Values)
			{
				if (pawn.inventory.Count(inventoryStockEntry.thingDef) < inventoryStockEntry.count)
				{
					Thing thing = this.FindThingFor(pawn, inventoryStockEntry.thingDef);
					if (thing != null)
					{
						Job job = JobMaker.MakeJob(JobDefOf.TakeCountToInventory, thing);
						int b = inventoryStockEntry.count - pawn.inventory.Count(thing.def);
						job.count = Mathf.Min(thing.stackCount, b);
						return job;
					}
				}
			}
			pawn.mindState.nextInventoryStockTick = Find.TickManager.TicksGame + Rand.Range(6000, 9000);
			return null;
		}

		// Token: 0x06007F77 RID: 32631 RVA: 0x00265F04 File Offset: 0x00264104
		private Thing FindThingFor(Pawn pawn, ThingDef thingDef)
		{
			return GenClosest.ClosestThingReachable(pawn.Position, pawn.Map, ThingRequest.ForDef(thingDef), PathEndMode.ClosestTouch, TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false, false, false, true), 9999f, (Thing x) => this.ThingValidator(pawn, x), null, 0, -1, false, RegionType.Set_Passable, false, false);
		}

		// Token: 0x06007F78 RID: 32632 RVA: 0x00265F71 File Offset: 0x00264171
		private bool ThingValidator(Pawn pawn, Thing thing)
		{
			return !thing.IsForbidden(pawn) && pawn.CanReserve(thing, 10, -1, null, false);
		}

		// Token: 0x0400554D RID: 21837
		private const int InventoryStockCheckIntervalMin = 6000;

		// Token: 0x0400554E RID: 21838
		private const int InventoryStockCheckIntervalMax = 9000;
	}
}
