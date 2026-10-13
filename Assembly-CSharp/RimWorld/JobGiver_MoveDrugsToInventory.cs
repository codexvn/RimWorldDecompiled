using System;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020013F1 RID: 5105
	public class JobGiver_MoveDrugsToInventory : ThinkNode_JobGiver
	{
		// Token: 0x06007C87 RID: 31879 RVA: 0x002581C8 File Offset: 0x002563C8
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
				if (pawn.drugs.AllowedToTakeToInventory(drugPolicy[i].drug))
				{
					return 7.5f;
				}
			}
			return 0f;
		}

		// Token: 0x06007C88 RID: 31880 RVA: 0x00258228 File Offset: 0x00256428
		protected override Job TryGiveJob(Pawn pawn)
		{
			Pawn_DrugPolicyTracker drugs = pawn.drugs;
			DrugPolicy drugPolicy = (drugs != null) ? drugs.CurrentPolicy : null;
			if (drugPolicy == null)
			{
				return null;
			}
			for (int i = 0; i < drugPolicy.Count; i++)
			{
				if (pawn.drugs.AllowedToTakeToInventory(drugPolicy[i].drug))
				{
					int num = drugPolicy[i].takeToInventory - pawn.inventory.innerContainer.TotalStackCountOfDef(drugPolicy[i].drug);
					Thing thing = this.FindDrugFor(pawn, drugPolicy[i].drug, num);
					if (thing != null)
					{
						Job job = JobMaker.MakeJob(JobDefOf.TakeInventory, thing);
						job.count = Mathf.Min(num, thing.stackCount);
						return job;
					}
				}
			}
			return null;
		}

		// Token: 0x06007C89 RID: 31881 RVA: 0x002582E4 File Offset: 0x002564E4
		private Thing FindDrugFor(Pawn pawn, ThingDef drugDef, int desired)
		{
			return GenClosest.ClosestThingReachable(pawn.Position, pawn.Map, ThingRequest.ForDef(drugDef), PathEndMode.ClosestTouch, TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false, false, false, true), 9999f, (Thing x) => this.DrugValidator(pawn, x, desired), null, 0, -1, false, RegionType.Set_Passable, false, false);
		}

		// Token: 0x06007C8A RID: 31882 RVA: 0x00258358 File Offset: 0x00256558
		private bool DrugValidator(Pawn pawn, Thing drug, int desired)
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
				if (!pawn.CanReserve(drug, 10, Mathf.Min(drug.stackCount, desired), null, false))
				{
					return false;
				}
			}
			return true;
		}
	}
}
