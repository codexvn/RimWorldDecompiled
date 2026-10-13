using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020015BF RID: 5567
	public class WorkGiver_ExtractSkull : WorkGiver_Scanner
	{
		// Token: 0x17001421 RID: 5153
		// (get) Token: 0x06008393 RID: 33683 RVA: 0x000028E7 File Offset: 0x00000AE7
		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.OnCell;
			}
		}

		// Token: 0x06008394 RID: 33684 RVA: 0x00277C11 File Offset: 0x00275E11
		public static bool CanExtractSkull(Ideo ideo)
		{
			return ideo.classicMode || ideo.HasPrecept(PreceptDefOf.Skullspike_Desired) || (ModsConfig.AnomalyActive && ResearchProjectDefOf.AdvancedPsychicRituals.IsFinished);
		}

		// Token: 0x06008395 RID: 33685 RVA: 0x00277C40 File Offset: 0x00275E40
		public static bool CanPlayerExtractSkull()
		{
			if (Find.IdeoManager.classicMode)
			{
				return true;
			}
			if (WorkGiver_ExtractSkull.CanExtractSkull(Faction.OfPlayer.ideos.PrimaryIdeo))
			{
				return true;
			}
			using (List<Ideo>.Enumerator enumerator = Faction.OfPlayer.ideos.IdeosMinorListForReading.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (WorkGiver_ExtractSkull.CanExtractSkull(enumerator.Current))
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06008396 RID: 33686 RVA: 0x00277CC8 File Offset: 0x00275EC8
		public override IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn)
		{
			foreach (Designation designation in pawn.Map.designationManager.SpawnedDesignationsOfDef(DesignationDefOf.ExtractSkull))
			{
				yield return designation.target.Thing;
			}
			IEnumerator<Designation> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x06008397 RID: 33687 RVA: 0x00277CD8 File Offset: 0x00275ED8
		public override bool ShouldSkip(Pawn pawn, bool forced = false)
		{
			return !pawn.Map.designationManager.AnySpawnedDesignationOfDef(DesignationDefOf.ExtractSkull);
		}

		// Token: 0x06008398 RID: 33688 RVA: 0x00277CF4 File Offset: 0x00275EF4
		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			Corpse corpse = t as Corpse;
			if (corpse == null || corpse.Destroyed)
			{
				return false;
			}
			if (corpse.Map.designationManager.DesignationOn(t, DesignationDefOf.ExtractSkull) == null)
			{
				return false;
			}
			if (!corpse.InnerPawn.health.hediffSet.HasHead)
			{
				return false;
			}
			if (!pawn.CanReserve(t, 1, -1, null, forced))
			{
				return false;
			}
			if (ModsConfig.IdeologyActive && (pawn.Ideo == null || !WorkGiver_ExtractSkull.CanExtractSkull(pawn.Ideo)))
			{
				JobFailReason.Is("CannotExtractSkull".Translate(), null);
				return false;
			}
			return true;
		}

		// Token: 0x06008399 RID: 33689 RVA: 0x00277D90 File Offset: 0x00275F90
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			Job job = JobMaker.MakeJob(JobDefOf.ExtractSkull, t);
			job.count = 1;
			return job;
		}
	}
}
