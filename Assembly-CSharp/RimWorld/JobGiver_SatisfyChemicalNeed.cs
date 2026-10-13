using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020013EC RID: 5100
	public class JobGiver_SatisfyChemicalNeed : ThinkNode_JobGiver
	{
		// Token: 0x06007C73 RID: 31859 RVA: 0x00257A34 File Offset: 0x00255C34
		public override float GetPriority(Pawn pawn)
		{
			if (pawn.needs.AllNeeds.Any((Need x) => this.ShouldSatisfy(x)))
			{
				return 9.25f;
			}
			return 0f;
		}

		// Token: 0x06007C74 RID: 31860 RVA: 0x00257A60 File Offset: 0x00255C60
		protected override Job TryGiveJob(Pawn pawn)
		{
			JobGiver_SatisfyChemicalNeed.tmpChemicalNeeds.Clear();
			List<Need> allNeeds = pawn.needs.AllNeeds;
			for (int i = 0; i < allNeeds.Count; i++)
			{
				if (this.ShouldSatisfy(allNeeds[i]))
				{
					JobGiver_SatisfyChemicalNeed.tmpChemicalNeeds.Add((Need_Chemical)allNeeds[i]);
				}
			}
			if (!JobGiver_SatisfyChemicalNeed.tmpChemicalNeeds.Any<Need_Chemical>())
			{
				return null;
			}
			JobGiver_SatisfyChemicalNeed.tmpChemicalNeeds.SortBy((Need_Chemical x) => x.CurLevel);
			int j = 0;
			while (j < JobGiver_SatisfyChemicalNeed.tmpChemicalNeeds.Count)
			{
				Thing thing = this.FindDrugFor(pawn, JobGiver_SatisfyChemicalNeed.tmpChemicalNeeds[j]);
				if (thing != null)
				{
					JobGiver_SatisfyChemicalNeed.tmpChemicalNeeds.Clear();
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
			JobGiver_SatisfyChemicalNeed.tmpChemicalNeeds.Clear();
			return null;
		}

		// Token: 0x06007C75 RID: 31861 RVA: 0x00257B7C File Offset: 0x00255D7C
		private bool ShouldSatisfy(Need need)
		{
			Need_Chemical need_Chemical = need as Need_Chemical;
			return need_Chemical != null && need_Chemical.CurCategory <= DrugDesireCategory.Desire;
		}

		// Token: 0x06007C76 RID: 31862 RVA: 0x00257BA0 File Offset: 0x00255DA0
		private Thing FindDrugFor(Pawn pawn, Need_Chemical need)
		{
			Hediff_Addiction addictionHediff = need.AddictionHediff;
			if (addictionHediff == null)
			{
				return null;
			}
			ThingOwner<Thing> innerContainer = pawn.inventory.innerContainer;
			for (int i = 0; i < innerContainer.Count; i++)
			{
				if (JobGiver_SatisfyChemicalNeed.DrugValidator(pawn, addictionHediff, innerContainer[i]))
				{
					return innerContainer[i];
				}
			}
			Thing thing = GenClosest.ClosestThingReachable(pawn.Position, pawn.Map, ThingRequest.ForGroup(ThingRequestGroup.Drug), PathEndMode.ClosestTouch, TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false, false, false, true), 9999f, (Thing x) => JobGiver_SatisfyChemicalNeed.DrugValidator(pawn, addictionHediff, x), null, 0, -1, false, RegionType.Set_Passable, false, false);
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
						if (JobGiver_SatisfyChemicalNeed.DrugValidator(pawn, addictionHediff, thing2) && !pawn2.IsForbidden(pawn) && pawn.CanReach(pawn2, PathEndMode.OnCell, Danger.Some, false, false, TraverseMode.ByPawn))
						{
							return thing2;
						}
					}
				}
			}
			return null;
		}

		// Token: 0x06007C77 RID: 31863 RVA: 0x00257D68 File Offset: 0x00255F68
		private static bool DrugValidator(Pawn pawn, Hediff_Addiction addiction, Thing drug)
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
			if (((compDrug != null) ? compDrug.Props.chemical : null) == null)
			{
				return false;
			}
			if (compDrug.Props.chemical.addictionHediff != addiction.def)
			{
				return false;
			}
			Pawn_DrugPolicyTracker drugs = pawn.drugs;
			DrugPolicy drugPolicy = (drugs != null) ? drugs.CurrentPolicy : null;
			return drugPolicy == null || drugPolicy[drug.def].allowedForAddiction || pawn.story == null || pawn.story.traits.DegreeOfTrait(TraitDefOf.DrugDesire) > 0 || (pawn.InMentalState && pawn.MentalStateDef.ignoreDrugPolicy);
		}

		// Token: 0x040053EF RID: 21487
		private static List<Need_Chemical> tmpChemicalNeeds = new List<Need_Chemical>();
	}
}
