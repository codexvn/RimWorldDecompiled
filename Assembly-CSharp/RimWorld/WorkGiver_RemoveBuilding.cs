using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200156E RID: 5486
	public abstract class WorkGiver_RemoveBuilding : WorkGiver_Scanner
	{
		// Token: 0x170013E6 RID: 5094
		// (get) Token: 0x06008224 RID: 33316
		protected abstract DesignationDef Designation { get; }

		// Token: 0x170013E7 RID: 5095
		// (get) Token: 0x06008225 RID: 33317
		protected abstract JobDef RemoveBuildingJob { get; }

		// Token: 0x170013E8 RID: 5096
		// (get) Token: 0x06008226 RID: 33318 RVA: 0x000E96BB File Offset: 0x000E78BB
		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.Touch;
			}
		}

		// Token: 0x06008227 RID: 33319 RVA: 0x0027264B File Offset: 0x0027084B
		public override IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn)
		{
			foreach (Designation designation in pawn.Map.designationManager.SpawnedDesignationsOfDef(this.Designation))
			{
				yield return designation.target.Thing;
			}
			IEnumerator<Designation> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x06008228 RID: 33320 RVA: 0x00272662 File Offset: 0x00270862
		public override bool ShouldSkip(Pawn pawn, bool forced = false)
		{
			return !pawn.Map.designationManager.AnySpawnedDesignationOfDef(this.Designation);
		}

		// Token: 0x06008229 RID: 33321 RVA: 0x00272680 File Offset: 0x00270880
		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			if (!pawn.CanReserve(t, 1, -1, null, forced))
			{
				return false;
			}
			if (pawn.Map.designationManager.DesignationOn(t, this.Designation) == null)
			{
				return false;
			}
			CompExplosive compExplosive;
			if (t.TryGetComp(out compExplosive) && compExplosive.wickStarted)
			{
				JobFailReason.Is("AboutToExplode".Translate(), null);
				return false;
			}
			return true;
		}

		// Token: 0x0600822A RID: 33322 RVA: 0x002726E6 File Offset: 0x002708E6
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			return JobMaker.MakeJob(this.RemoveBuildingJob, t);
		}
	}
}
