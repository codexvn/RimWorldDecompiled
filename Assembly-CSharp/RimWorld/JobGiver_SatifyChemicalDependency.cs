using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020013E9 RID: 5097
	public class JobGiver_SatifyChemicalDependency : ThinkNode_JobGiver
	{
		// Token: 0x06007C67 RID: 31847 RVA: 0x00257606 File Offset: 0x00255806
		public override float GetPriority(Pawn pawn)
		{
			if (!ModsConfig.BiotechActive)
			{
				return 0f;
			}
			if (pawn.health.hediffSet.hediffs.Any(new Predicate<Hediff>(this.ShouldSatify)))
			{
				return 9.25f;
			}
			return 0f;
		}

		// Token: 0x06007C68 RID: 31848 RVA: 0x00257644 File Offset: 0x00255844
		protected override Job TryGiveJob(Pawn pawn)
		{
			JobGiver_SatifyChemicalDependency.tmpChemicalDependencies.Clear();
			if (!ModsConfig.BiotechActive)
			{
				return null;
			}
			List<Hediff> hediffs = pawn.health.hediffSet.hediffs;
			for (int i = 0; i < hediffs.Count; i++)
			{
				if (this.ShouldSatify(hediffs[i]))
				{
					JobGiver_SatifyChemicalDependency.tmpChemicalDependencies.Add((Hediff_ChemicalDependency)hediffs[i]);
				}
			}
			if (!JobGiver_SatifyChemicalDependency.tmpChemicalDependencies.Any<Hediff_ChemicalDependency>())
			{
				return null;
			}
			JobGiver_SatifyChemicalDependency.tmpChemicalDependencies.SortBy((Hediff_ChemicalDependency x) => -x.Severity);
			int j = 0;
			while (j < JobGiver_SatifyChemicalDependency.tmpChemicalDependencies.Count)
			{
				Thing thing = this.FindDrugFor(pawn, JobGiver_SatifyChemicalDependency.tmpChemicalDependencies[j]);
				if (thing != null)
				{
					JobGiver_SatifyChemicalDependency.tmpChemicalDependencies.Clear();
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
				else
				{
					j++;
				}
			}
			JobGiver_SatifyChemicalDependency.tmpChemicalDependencies.Clear();
			return null;
		}

		// Token: 0x06007C69 RID: 31849 RVA: 0x0025776C File Offset: 0x0025596C
		private bool ShouldSatify(Hediff hediff)
		{
			Hediff_ChemicalDependency hediff_ChemicalDependency = hediff as Hediff_ChemicalDependency;
			return hediff_ChemicalDependency != null && hediff_ChemicalDependency.ShouldSatify;
		}

		// Token: 0x06007C6A RID: 31850 RVA: 0x0025778C File Offset: 0x0025598C
		private Thing FindDrugFor(Pawn pawn, Hediff_ChemicalDependency dependency)
		{
			ThingOwner<Thing> innerContainer = pawn.inventory.innerContainer;
			for (int i = 0; i < innerContainer.Count; i++)
			{
				if (JobGiver_SatifyChemicalDependency.DrugValidator(pawn, dependency, innerContainer[i]))
				{
					return innerContainer[i];
				}
			}
			Thing thing = GenClosest.ClosestThingReachable(pawn.Position, pawn.Map, ThingRequest.ForGroup(ThingRequestGroup.Drug), PathEndMode.ClosestTouch, TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false, false, false, true), 9999f, (Thing x) => JobGiver_SatifyChemicalDependency.DrugValidator(pawn, dependency, x), null, 0, -1, false, RegionType.Set_Passable, false, false);
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
						if (JobGiver_SatifyChemicalDependency.DrugValidator(pawn, dependency, thing2) && !pawn2.IsForbidden(pawn) && pawn.CanReach(pawn2, PathEndMode.OnCell, Danger.Some, false, false, TraverseMode.ByPawn))
						{
							return thing2;
						}
					}
				}
			}
			return null;
		}

		// Token: 0x06007C6B RID: 31851 RVA: 0x00257944 File Offset: 0x00255B44
		private static bool DrugValidator(Pawn pawn, Hediff_ChemicalDependency dependency, Thing drug)
		{
			if (!drug.def.IsDrug)
			{
				return false;
			}
			if (drug.Spawned && (!pawn.CanReserve(drug, 1, -1, null, false) || drug.IsForbidden(pawn) || !drug.IsSociallyProper(pawn) || !drug.IngestibleNow))
			{
				return false;
			}
			CompDrug compDrug = drug.TryGetComp<CompDrug>();
			return compDrug != null && compDrug.Props.chemical != null && compDrug.Props.chemical == dependency.chemical && (pawn.drugs == null || pawn.drugs.CurrentPolicy[drug.def].allowedForAddiction || (pawn.InMentalState && !pawn.MentalStateDef.ignoreDrugPolicy));
		}

		// Token: 0x040053EA RID: 21482
		private static readonly List<Hediff_ChemicalDependency> tmpChemicalDependencies = new List<Hediff_ChemicalDependency>();
	}
}
