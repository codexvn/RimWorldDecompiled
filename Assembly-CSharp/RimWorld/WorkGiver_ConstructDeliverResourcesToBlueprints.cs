using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001565 RID: 5477
	public class WorkGiver_ConstructDeliverResourcesToBlueprints : WorkGiver_ConstructDeliverResources
	{
		// Token: 0x170013D8 RID: 5080
		// (get) Token: 0x060081EC RID: 33260 RVA: 0x00271B82 File Offset: 0x0026FD82
		public override ThingRequest PotentialWorkThingRequest
		{
			get
			{
				return ThingRequest.ForGroup(ThingRequestGroup.Blueprint);
			}
		}

		// Token: 0x060081ED RID: 33261 RVA: 0x00271B8C File Offset: 0x0026FD8C
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			if (t.Faction != pawn.Faction)
			{
				return null;
			}
			Blueprint blueprint = t as Blueprint;
			if (blueprint != null)
			{
				ThingDef thingDef = blueprint.def.entityDefToBuild as ThingDef;
				if (thingDef == null || thingDef.plant == null)
				{
					if (!GenConstruct.CanTouchTargetFromValidCell(blueprint, pawn))
					{
						return null;
					}
					if (GenConstruct.FirstBlockingThing(blueprint, pawn) != null)
					{
						return GenConstruct.HandleBlockingThingJob(blueprint, pawn, forced);
					}
					if (!GenConstruct.CanConstruct(blueprint, pawn, this.def.workType, forced, JobDefOf.HaulToContainer))
					{
						return null;
					}
					if (this.def.workType != WorkTypeDefOf.Construction && WorkGiver_ConstructDeliverResources.ShouldRemoveExistingFloorFirst(pawn, blueprint))
					{
						return null;
					}
					Job job = base.RemoveExistingFloorJob(pawn, blueprint);
					if (job != null)
					{
						return job;
					}
					Job job2 = base.ResourceDeliverJobFor(pawn, blueprint, true, forced);
					if (job2 != null)
					{
						return job2;
					}
					if (this.def.workType != WorkTypeDefOf.Hauling)
					{
						Job job3 = this.NoCostFrameMakeJobFor(blueprint);
						if (job3 != null)
						{
							return job3;
						}
					}
					return null;
				}
			}
			return null;
		}

		// Token: 0x060081EE RID: 33262 RVA: 0x00271C6A File Offset: 0x0026FE6A
		private Job NoCostFrameMakeJobFor(IConstructible c)
		{
			if (c is Blueprint_Install)
			{
				return null;
			}
			if (c is Blueprint && c.TotalMaterialCost().Count == 0)
			{
				Job job = JobMaker.MakeJob(JobDefOf.PlaceNoCostFrame);
				job.targetA = (Thing)c;
				return job;
			}
			return null;
		}
	}
}
