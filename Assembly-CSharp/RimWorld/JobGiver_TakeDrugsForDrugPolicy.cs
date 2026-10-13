using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020013EF RID: 5103
	public class JobGiver_TakeDrugsForDrugPolicy : ThinkNode_JobGiver
	{
		// Token: 0x06007C80 RID: 31872 RVA: 0x00257E90 File Offset: 0x00256090
		public override float GetPriority(Pawn pawn)
		{
			Pawn_DrugPolicyTracker drugs = pawn.drugs;
			DrugPolicy drugPolicy = (drugs != null) ? drugs.CurrentPolicy : null;
			if (drugPolicy == null)
			{
				return 0f;
			}
			for (int i = 0; i < drugPolicy.Count; i++)
			{
				if (pawn.drugs.ShouldTryToTakeScheduledNow(drugPolicy[i].drug))
				{
					return 7.5f;
				}
			}
			return 0f;
		}

		// Token: 0x06007C81 RID: 31873 RVA: 0x00257EF0 File Offset: 0x002560F0
		protected override Job TryGiveJob(Pawn pawn)
		{
			DrugPolicy currentPolicy = pawn.drugs.CurrentPolicy;
			if (currentPolicy == null)
			{
				return null;
			}
			for (int i = 0; i < currentPolicy.Count; i++)
			{
				if (pawn.drugs.ShouldTryToTakeScheduledNow(currentPolicy[i].drug))
				{
					Thing thing = this.FindDrugFor(pawn, currentPolicy[i].drug);
					if (thing != null)
					{
						Pawn_InventoryTracker pawn_InventoryTracker = thing.ParentHolder as Pawn_InventoryTracker;
						Pawn pawn2 = (pawn_InventoryTracker != null) ? pawn_InventoryTracker.pawn : null;
						if (pawn2 != null && pawn2 != pawn)
						{
							Job job = JobMaker.MakeJob(JobDefOf.TakeFromOtherInventory, thing, pawn2);
							job.count = 1;
							return job;
						}
						return DrugAIUtility.IngestAndTakeToInventoryJob(thing, pawn, 1);
					}
				}
			}
			return null;
		}

		// Token: 0x06007C82 RID: 31874 RVA: 0x00257F9C File Offset: 0x0025619C
		private Thing FindDrugFor(Pawn pawn, ThingDef drugDef)
		{
			ThingOwner<Thing> innerContainer = pawn.inventory.innerContainer;
			for (int i = 0; i < innerContainer.Count; i++)
			{
				if (innerContainer[i].def == drugDef && this.DrugValidator(pawn, innerContainer[i]))
				{
					return innerContainer[i];
				}
			}
			Thing thing = GenClosest.ClosestThingReachable(pawn.Position, pawn.Map, ThingRequest.ForDef(drugDef), PathEndMode.ClosestTouch, TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false, false, false, true), 9999f, (Thing x) => this.DrugValidator(pawn, x), null, 0, -1, false, RegionType.Set_Passable, false, false);
			if (thing != null)
			{
				return thing;
			}
			if (pawn.IsColonist && pawn.Map != null)
			{
				foreach (Pawn pawn2 in pawn.Map.mapPawns.SpawnedColonyAnimals)
				{
					foreach (Thing thing2 in pawn2.inventory.innerContainer)
					{
						if (thing2.def == drugDef && this.DrugValidator(pawn, thing2) && !pawn2.IsForbidden(pawn) && pawn.CanReach(pawn2, PathEndMode.OnCell, Danger.Some, false, false, TraverseMode.ByPawn))
						{
							return thing2;
						}
					}
				}
			}
			return null;
		}

		// Token: 0x06007C83 RID: 31875 RVA: 0x00258164 File Offset: 0x00256364
		private bool DrugValidator(Pawn pawn, Thing drug)
		{
			if (!drug.def.IsDrug)
			{
				return false;
			}
			if (drug.Spawned)
			{
				if (drug.IsForbidden(pawn))
				{
					return false;
				}
				if (!pawn.CanReserve(drug, 10, 1, null, false))
				{
					return false;
				}
				if (!drug.IsSociallyProper(pawn))
				{
					return false;
				}
			}
			return true;
		}
	}
}
