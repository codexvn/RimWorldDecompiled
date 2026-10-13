using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020015F3 RID: 5619
	public class WorkGiver_RemovePaintBuilding : WorkGiver_Scanner
	{
		// Token: 0x17001454 RID: 5204
		// (get) Token: 0x060084A1 RID: 33953 RVA: 0x000E96BB File Offset: 0x000E78BB
		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.Touch;
			}
		}

		// Token: 0x060084A2 RID: 33954 RVA: 0x0027BCD7 File Offset: 0x00279ED7
		public override bool ShouldSkip(Pawn pawn, bool forced = false)
		{
			return !pawn.Map.designationManager.AnySpawnedDesignationOfDef(DesignationDefOf.RemovePaintBuilding);
		}

		// Token: 0x060084A3 RID: 33955 RVA: 0x0027BCF1 File Offset: 0x00279EF1
		public override IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn)
		{
			foreach (Designation designation in pawn.Map.designationManager.SpawnedDesignationsOfDef(DesignationDefOf.RemovePaintBuilding))
			{
				yield return designation.target.Thing;
			}
			IEnumerator<Designation> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x060084A4 RID: 33956 RVA: 0x0027BD04 File Offset: 0x00279F04
		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			return t.def.building != null && t.def.building.paintable && pawn.Map.designationManager.DesignationOn(t, DesignationDefOf.RemovePaintBuilding) != null && pawn.Map.designationManager.DesignationOn(t, DesignationDefOf.Deconstruct) == null && !t.IsBurning() && pawn.CanReserve(t, 1, -1, null, forced);
		}

		// Token: 0x060084A5 RID: 33957 RVA: 0x0027BD88 File Offset: 0x00279F88
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			Job job = JobMaker.MakeJob(JobDefOf.RemovePaintBuilding);
			job.AddQueuedTarget(TargetIndex.A, t);
			for (int i = 0; i < 100; i++)
			{
				IntVec3 c = t.Position + GenRadial.RadialPattern[i];
				if (c.InBounds(t.Map) && !c.Fogged(t.Map) && pawn.CanReach(c, PathEndMode.Touch, Danger.Deadly, false, false, TraverseMode.ByPawn))
				{
					List<Thing> thingList = c.GetThingList(t.Map);
					for (int j = 0; j < thingList.Count; j++)
					{
						Thing thing = thingList[j];
						if (!job.targetQueueA.Contains(thing) && thing != t && this.HasJobOnThing(pawn, thing, forced))
						{
							job.AddQueuedTarget(TargetIndex.A, thing);
						}
					}
					if (job.GetTargetQueue(TargetIndex.A).Count >= 10)
					{
						break;
					}
				}
			}
			if (job.targetQueueA != null && job.targetQueueA.Count >= 5)
			{
				job.targetQueueA.SortBy((LocalTargetInfo targ) => targ.Cell.DistanceToSquared(pawn.Position));
			}
			return job;
		}
	}
}
