using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020015F0 RID: 5616
	public class WorkGiver_PaintBuilding : WorkGiver_Scanner
	{
		// Token: 0x17001451 RID: 5201
		// (get) Token: 0x0600848D RID: 33933 RVA: 0x000E96BB File Offset: 0x000E78BB
		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.Touch;
			}
		}

		// Token: 0x0600848E RID: 33934 RVA: 0x0027B6C1 File Offset: 0x002798C1
		public override bool ShouldSkip(Pawn pawn, bool forced = false)
		{
			return !pawn.Map.designationManager.AnySpawnedDesignationOfDef(DesignationDefOf.PaintBuilding);
		}

		// Token: 0x0600848F RID: 33935 RVA: 0x0027B6DB File Offset: 0x002798DB
		public override IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn)
		{
			foreach (Designation designation in pawn.Map.designationManager.SpawnedDesignationsOfDef(DesignationDefOf.PaintBuilding))
			{
				yield return designation.target.Thing;
			}
			IEnumerator<Designation> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x06008490 RID: 33936 RVA: 0x0027B6EC File Offset: 0x002798EC
		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			AcceptanceReport report = this.ShouldPaintThing(pawn, t, forced, true);
			if (!report)
			{
				if (!report.Reason.NullOrEmpty())
				{
					JobFailReason.Is(report.Reason, null);
				}
				return false;
			}
			return true;
		}

		// Token: 0x06008491 RID: 33937 RVA: 0x0027B72C File Offset: 0x0027992C
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			WorkGiver_PaintBuilding.tmpDye.Clear();
			WorkGiver_PaintBuilding.tmpDye = PaintUtility.FindNearbyDyes(pawn, forced);
			int stackCountFromThingList = ThingUtility.GetStackCountFromThingList(WorkGiver_PaintBuilding.tmpDye);
			if (!WorkGiver_PaintBuilding.tmpDye.Any<Thing>())
			{
				return null;
			}
			WorkGiver_PaintBuilding.tmpDye.SortBy((Thing x) => x.Position.DistanceToSquared(t.Position));
			int num = 0;
			Job job = JobMaker.MakeJob(JobDefOf.PaintBuilding);
			job.AddQueuedTarget(TargetIndex.A, t);
			job.AddQueuedTarget(TargetIndex.B, WorkGiver_PaintBuilding.tmpDye[num]);
			job.countQueue = new List<int>
			{
				1
			};
			int num2 = Mathf.Min(10, stackCountFromThingList);
			for (int i = 0; i < 100; i++)
			{
				IntVec3 c = t.Position + GenRadial.RadialPattern[i];
				if (c.InBounds(t.Map) && !c.Fogged(t.Map) && pawn.CanReach(c, PathEndMode.Touch, Danger.Deadly, false, false, TraverseMode.ByPawn))
				{
					List<Thing> thingList = c.GetThingList(t.Map);
					for (int j = 0; j < thingList.Count; j++)
					{
						Thing thing = thingList[j];
						if (thing != t && this.ShouldPaintThing(pawn, thing, forced, false) && !job.targetQueueA.Contains(thing))
						{
							job.AddQueuedTarget(TargetIndex.A, thing);
							List<int> countQueue = job.countQueue;
							int num3 = countQueue[0];
							countQueue[0] = num3 + 1;
							if (job.countQueue[0] >= WorkGiver_PaintBuilding.tmpDye[num].stackCount)
							{
								num++;
								if (num >= WorkGiver_PaintBuilding.tmpDye.Count)
								{
									break;
								}
								job.AddQueuedTarget(TargetIndex.B, WorkGiver_PaintBuilding.tmpDye[num]);
							}
						}
					}
					if (job.GetTargetQueue(TargetIndex.A).Count >= num2 || num >= WorkGiver_PaintBuilding.tmpDye.Count)
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

		// Token: 0x06008492 RID: 33938 RVA: 0x0027B998 File Offset: 0x00279B98
		private AcceptanceReport ShouldPaintThing(Pawn pawn, Thing t, bool forced, bool checkDye)
		{
			if (t.def.building == null || !t.def.building.paintable)
			{
				return false;
			}
			Designation designation = pawn.Map.designationManager.DesignationOn(t, DesignationDefOf.PaintBuilding);
			if (((designation != null) ? designation.colorDef : null) == null)
			{
				return false;
			}
			Building building = t as Building;
			if (building != null && building.PaintColorDef == designation.colorDef)
			{
				return false;
			}
			if (pawn.Map.designationManager.DesignationOn(t, DesignationDefOf.PaintBuilding) == null)
			{
				return false;
			}
			if (pawn.Map.designationManager.DesignationOn(t, DesignationDefOf.Deconstruct) != null)
			{
				return false;
			}
			if (pawn.Map.designationManager.DesignationOn(t, DesignationDefOf.RemovePaintBuilding) != null)
			{
				return false;
			}
			if (t.IsBurning())
			{
				return false;
			}
			if (!pawn.CanReserve(t, 1, -1, null, forced))
			{
				return false;
			}
			if (checkDye)
			{
				List<Thing> list = pawn.Map.listerThings.ThingsOfDef(ThingDefOf.Dye);
				for (int i = 0; i < list.Count; i++)
				{
					if (!list[i].IsForbidden(pawn) && pawn.CanReserveAndReach(list[i], PathEndMode.ClosestTouch, Danger.Deadly, 1, 1, null, forced))
					{
						return true;
					}
				}
				return "NoIngredient".Translate(ThingDefOf.Dye);
			}
			return true;
		}

		// Token: 0x04005728 RID: 22312
		private static List<Thing> tmpDye = new List<Thing>();
	}
}
