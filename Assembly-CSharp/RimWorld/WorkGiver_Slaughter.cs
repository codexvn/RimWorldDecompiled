using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001550 RID: 5456
	public class WorkGiver_Slaughter : WorkGiver_Scanner
	{
		// Token: 0x06008186 RID: 33158 RVA: 0x0026FDF6 File Offset: 0x0026DFF6
		public override IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn)
		{
			foreach (Designation designation in pawn.Map.designationManager.SpawnedDesignationsOfDef(DesignationDefOf.Slaughter))
			{
				yield return designation.target.Thing;
			}
			IEnumerator<Designation> enumerator = null;
			foreach (Pawn pawn2 in pawn.Map.autoSlaughterManager.AnimalsToSlaughter)
			{
				yield return pawn2;
			}
			List<Pawn>.Enumerator enumerator2 = default(List<Pawn>.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x170013CA RID: 5066
		// (get) Token: 0x06008187 RID: 33159 RVA: 0x000028E7 File Offset: 0x00000AE7
		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.OnCell;
			}
		}

		// Token: 0x06008188 RID: 33160 RVA: 0x0026FE06 File Offset: 0x0026E006
		public override bool ShouldSkip(Pawn pawn, bool forced = false)
		{
			return !pawn.Map.designationManager.AnySpawnedDesignationOfDef(DesignationDefOf.Slaughter) && pawn.Map.autoSlaughterManager.AnimalsToSlaughter.Count == 0;
		}

		// Token: 0x06008189 RID: 33161 RVA: 0x0026FE3C File Offset: 0x0026E03C
		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			Pawn pawn2 = t as Pawn;
			if (pawn2 == null || !pawn2.IsAnimal)
			{
				return false;
			}
			if (!pawn2.ShouldBeSlaughtered())
			{
				return false;
			}
			if (pawn.Faction != t.Faction)
			{
				return false;
			}
			if (pawn2.InAggroMentalState)
			{
				return false;
			}
			if (!pawn.CanReserve(t, 1, -1, null, forced))
			{
				return false;
			}
			if (pawn.WorkTagIsDisabled(WorkTags.Violent))
			{
				JobFailReason.Is("IsIncapableOfViolenceShort".Translate(pawn), null);
				return false;
			}
			return (!ModsConfig.IdeologyActive || new HistoryEvent(HistoryEventDefOf.SlaughteredAnimal, pawn.Named(HistoryEventArgsNames.Doer)).Notify_PawnAboutToDo_Job()) && (!HistoryEventUtility.IsKillingInnocentAnimal(pawn, pawn2) || new HistoryEvent(HistoryEventDefOf.KilledInnocentAnimal, pawn.Named(HistoryEventArgsNames.Doer)).Notify_PawnAboutToDo_Job()) && (pawn.Ideo == null || !pawn.Ideo.IsVeneratedAnimal(pawn2) || new HistoryEvent(HistoryEventDefOf.SlaughteredVeneratedAnimal, pawn.Named(HistoryEventArgsNames.Doer)).Notify_PawnAboutToDo_Job());
		}

		// Token: 0x0600818A RID: 33162 RVA: 0x0026FF3C File Offset: 0x0026E13C
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			return JobMaker.MakeJob(JobDefOf.Slaughter, t);
		}
	}
}
