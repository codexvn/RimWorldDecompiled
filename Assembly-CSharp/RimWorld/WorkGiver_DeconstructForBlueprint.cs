using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200156B RID: 5483
	public class WorkGiver_DeconstructForBlueprint : WorkGiver_Scanner
	{
		// Token: 0x170013E1 RID: 5089
		// (get) Token: 0x0600820B RID: 33291 RVA: 0x000E96BB File Offset: 0x000E78BB
		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.Touch;
			}
		}

		// Token: 0x0600820C RID: 33292 RVA: 0x00272050 File Offset: 0x00270250
		public override IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn)
		{
			foreach (Building building in pawn.Map.listerBuildings.allBuildingsColonist)
			{
				if (building.def != null && !pawn.Map.blueprintGrid[building.Position].NullOrEmpty<Blueprint>() && building.DeconstructibleBy(pawn.Faction).Accepted)
				{
					yield return building;
				}
			}
			List<Building>.Enumerator enumerator = default(List<Building>.Enumerator);
			foreach (Building building2 in pawn.Map.listerBuildings.allBuildingsNonColonist)
			{
				if (building2.def != null && !pawn.Map.blueprintGrid[building2.Position].NullOrEmpty<Blueprint>() && building2.DeconstructibleBy(pawn.Faction).Accepted)
				{
					yield return building2;
				}
			}
			enumerator = default(List<Building>.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x0600820D RID: 33293 RVA: 0x00272060 File Offset: 0x00270260
		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			if (!pawn.CanReserve(t, 1, -1, null, forced))
			{
				return false;
			}
			IEnumerable<Blueprint> enumerable = WorkGiver_DeconstructForBlueprint.BuildableBlueprintsAt(pawn, t.Position);
			if (enumerable.EnumerableNullOrEmpty<Blueprint>())
			{
				return false;
			}
			CompExplosive compExplosive;
			if (t.TryGetComp(out compExplosive) && compExplosive.wickStarted)
			{
				JobFailReason.Is("AboutToExplode".Translate(), null);
				return false;
			}
			Building building = t as Building;
			if (building != null)
			{
				AcceptanceReport acceptanceReport = building.DeconstructibleBy(pawn.Faction);
				if (!acceptanceReport.Accepted)
				{
					JobFailReason.Is(acceptanceReport.Reason, null);
					return false;
				}
				if (building.IsForbidden(pawn))
				{
					JobFailReason.Is("ForbiddenLower".Translate(), null);
					return false;
				}
			}
			foreach (Blueprint blueprint in enumerable)
			{
				Blueprint_Install blueprint_Install = blueprint as Blueprint_Install;
				if (blueprint_Install != null && blueprint_Install.ThingToInstall == t)
				{
					return false;
				}
				if (blueprint.IsForbidden(pawn))
				{
					JobFailReason.Is("ForbiddenLower".Translate(), null);
					return false;
				}
				if (GenConstruct.CanReplace(blueprint.EntityToBuild(), t.def, blueprint.Stuff, t.Stuff))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600820E RID: 33294 RVA: 0x002721B4 File Offset: 0x002703B4
		private static IEnumerable<Blueprint> BuildableBlueprintsAt(Pawn pawn, IntVec3 cell)
		{
			List<Blueprint> list = pawn.Map.blueprintGrid[cell];
			if (list.NullOrEmpty<Blueprint>())
			{
				yield break;
			}
			foreach (Blueprint blueprint in list)
			{
				if (blueprint.Faction == pawn.Faction)
				{
					yield return blueprint;
				}
			}
			List<Blueprint>.Enumerator enumerator = default(List<Blueprint>.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x0600820F RID: 33295 RVA: 0x002721CB File Offset: 0x002703CB
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			return JobMaker.MakeJob(JobDefOf.DeconstructForBlueprint, t);
		}
	}
}
