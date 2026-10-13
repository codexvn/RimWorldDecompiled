using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020015FF RID: 5631
	public class WorkGiver_PlantsCut : WorkGiver_Scanner
	{
		// Token: 0x060084DF RID: 34015 RVA: 0x00075162 File Offset: 0x00073362
		public override Danger MaxPathDanger(Pawn pawn)
		{
			return Danger.Deadly;
		}

		// Token: 0x060084E0 RID: 34016 RVA: 0x0027CB1D File Offset: 0x0027AD1D
		public override IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn)
		{
			foreach (Designation designation in pawn.Map.designationManager.designationsByDef[DesignationDefOf.CutPlant])
			{
				yield return designation.target.Thing;
			}
			List<Designation>.Enumerator enumerator = default(List<Designation>.Enumerator);
			foreach (Designation designation2 in pawn.Map.designationManager.designationsByDef[DesignationDefOf.HarvestPlant])
			{
				yield return designation2.target.Thing;
			}
			enumerator = default(List<Designation>.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x060084E1 RID: 34017 RVA: 0x0027CB2D File Offset: 0x0027AD2D
		public override bool ShouldSkip(Pawn pawn, bool forced = false)
		{
			return !pawn.Map.designationManager.AnySpawnedDesignationOfDef(DesignationDefOf.CutPlant) && !pawn.Map.designationManager.AnySpawnedDesignationOfDef(DesignationDefOf.HarvestPlant);
		}

		// Token: 0x1700145D RID: 5213
		// (get) Token: 0x060084E2 RID: 34018 RVA: 0x000E96BB File Offset: 0x000E78BB
		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.Touch;
			}
		}

		// Token: 0x060084E3 RID: 34019 RVA: 0x0027CB60 File Offset: 0x0027AD60
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			if (t.def.category != ThingCategory.Plant)
			{
				return null;
			}
			if (!pawn.CanReserve(t, 1, -1, null, forced))
			{
				return null;
			}
			if (t.IsBurning())
			{
				return null;
			}
			if (!PlantUtility.PawnWillingToCutPlant_Job(t, pawn))
			{
				return null;
			}
			CompPlantPreventCutting compPlantPreventCutting;
			if (!forced && t.TryGetComp(out compPlantPreventCutting) && compPlantPreventCutting.PreventCutting)
			{
				return null;
			}
			foreach (Designation designation in pawn.Map.designationManager.AllDesignationsOn(t))
			{
				if (designation.def == DesignationDefOf.HarvestPlant)
				{
					if (!((Plant)t).HarvestableNow)
					{
						return null;
					}
					return JobMaker.MakeJob(JobDefOf.HarvestDesignated, t);
				}
				else if (designation.def == DesignationDefOf.CutPlant)
				{
					return JobMaker.MakeJob(JobDefOf.CutPlantDesignated, t);
				}
			}
			return null;
		}

		// Token: 0x060084E4 RID: 34020 RVA: 0x0027CC5C File Offset: 0x0027AE5C
		public override string PostProcessedGerund(Job job)
		{
			if (job.def == JobDefOf.HarvestDesignated)
			{
				return "HarvestGerund".Translate();
			}
			if (job.def == JobDefOf.CutPlantDesignated)
			{
				return "CutGerund".Translate();
			}
			return this.def.gerund;
		}
	}
}
