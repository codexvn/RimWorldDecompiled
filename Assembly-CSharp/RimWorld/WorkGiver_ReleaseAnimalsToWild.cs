using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200154D RID: 5453
	public class WorkGiver_ReleaseAnimalsToWild : WorkGiver_Scanner
	{
		// Token: 0x06008174 RID: 33140 RVA: 0x0026FB7E File Offset: 0x0026DD7E
		public override IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn)
		{
			foreach (Designation designation in pawn.Map.designationManager.SpawnedDesignationsOfDef(DesignationDefOf.ReleaseAnimalToWild))
			{
				yield return designation.target.Thing;
			}
			IEnumerator<Designation> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x170013C6 RID: 5062
		// (get) Token: 0x06008175 RID: 33141 RVA: 0x000028E7 File Offset: 0x00000AE7
		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.OnCell;
			}
		}

		// Token: 0x06008176 RID: 33142 RVA: 0x0026FB8E File Offset: 0x0026DD8E
		public override bool ShouldSkip(Pawn pawn, bool forced = false)
		{
			return !pawn.Map.designationManager.AnySpawnedDesignationOfDef(DesignationDefOf.ReleaseAnimalToWild);
		}

		// Token: 0x06008177 RID: 33143 RVA: 0x0026FBA8 File Offset: 0x0026DDA8
		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			Pawn pawn2 = t as Pawn;
			if (pawn2 == null || !pawn2.IsAnimal)
			{
				return false;
			}
			if (pawn.Map.designationManager.DesignationOn(t, DesignationDefOf.ReleaseAnimalToWild) == null)
			{
				return false;
			}
			if (pawn.Faction != t.Faction)
			{
				return false;
			}
			if (pawn2.InAggroMentalState || pawn2.Dead)
			{
				return false;
			}
			if (!pawn.CanReserve(t, 1, -1, null, forced))
			{
				return false;
			}
			IntVec3 intVec;
			if (!JobDriver_ReleaseAnimalToWild.TryFindClosestOutsideCell(t.Position, t.Map, TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false, false, false, true), pawn, out intVec))
			{
				JobFailReason.Is("NoReachableOutsideCell".Translate(), null);
				return false;
			}
			return true;
		}

		// Token: 0x06008178 RID: 33144 RVA: 0x0026FC53 File Offset: 0x0026DE53
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			Job job = JobMaker.MakeJob(JobDefOf.ReleaseAnimalToWild, t);
			job.count = 1;
			return job;
		}
	}
}
