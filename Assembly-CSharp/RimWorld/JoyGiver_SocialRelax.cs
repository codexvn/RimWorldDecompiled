using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020014F4 RID: 5364
	public class JoyGiver_SocialRelax : JoyGiver
	{
		// Token: 0x06007FF0 RID: 32752 RVA: 0x00268245 File Offset: 0x00266445
		public override Job TryGiveJob(Pawn pawn)
		{
			return this.TryGiveJobInt(pawn, null);
		}

		// Token: 0x06007FF1 RID: 32753 RVA: 0x00268250 File Offset: 0x00266450
		public override Job TryGiveJobInGatheringArea(Pawn pawn, IntVec3 gatheringSpot, float maxRadius = -1f)
		{
			return this.TryGiveJobInt(pawn, (CompGatherSpot x) => GatheringsUtility.InGatheringArea(x.parent.Position, gatheringSpot, pawn.Map) && (maxRadius < 0f || x.parent.Position.InHorDistOf(gatheringSpot, maxRadius)));
		}

		// Token: 0x06007FF2 RID: 32754 RVA: 0x00268290 File Offset: 0x00266490
		private Job TryGiveJobInt(Pawn pawn, Predicate<CompGatherSpot> gatherSpotValidator)
		{
			if (pawn.Map.gatherSpotLister.activeSpots.Count == 0)
			{
				return null;
			}
			JoyGiver_SocialRelax.workingSpots.Clear();
			for (int i = 0; i < pawn.Map.gatherSpotLister.activeSpots.Count; i++)
			{
				JoyGiver_SocialRelax.workingSpots.Add(pawn.Map.gatherSpotLister.activeSpots[i]);
			}
			CompGatherSpot compGatherSpot;
			while (JoyGiver_SocialRelax.workingSpots.TryRandomElement(out compGatherSpot))
			{
				JoyGiver_SocialRelax.workingSpots.Remove(compGatherSpot);
				if (!compGatherSpot.parent.IsForbidden(pawn) && !compGatherSpot.parent.Fogged() && pawn.CanReach(compGatherSpot.parent, PathEndMode.Touch, Danger.None, false, false, TraverseMode.ByPawn) && compGatherSpot.parent.IsSociallyProper(pawn) && compGatherSpot.parent.IsPoliticallyProper(pawn) && !compGatherSpot.parent.Position.VacuumConcernTo(pawn) && (gatherSpotValidator == null || gatherSpotValidator(compGatherSpot)))
				{
					Job job;
					Thing t2;
					if (compGatherSpot.parent.def.surfaceType == SurfaceType.Eat)
					{
						Thing t;
						if (!JoyGiver_SocialRelax.TryFindChairBesideTable(compGatherSpot.parent, pawn, out t))
						{
							return null;
						}
						job = JobMaker.MakeJob(this.def.jobDef, compGatherSpot.parent, t);
					}
					else if (JoyGiver_SocialRelax.TryFindChairNear(compGatherSpot.parent.Position, pawn, out t2))
					{
						job = JobMaker.MakeJob(this.def.jobDef, compGatherSpot.parent, t2);
					}
					else
					{
						IntVec3 c;
						if (!JoyGiver_SocialRelax.TryFindSitSpotOnGroundNear(compGatherSpot.parent.Position, pawn, out c))
						{
							return null;
						}
						job = JobMaker.MakeJob(this.def.jobDef, compGatherSpot.parent, c);
					}
					Thing thing;
					if (pawn.health.capacities.CapableOf(PawnCapacityDefOf.Manipulation) && JoyGiver_SocialRelax.TryFindIngestibleToNurse(compGatherSpot.parent.Position, pawn, out thing))
					{
						job.targetC = thing;
						job.count = Mathf.Min(thing.stackCount, thing.def.ingestible.defaultNumToIngestAtOnce);
					}
					return job;
				}
			}
			return null;
		}

		// Token: 0x06007FF3 RID: 32755 RVA: 0x002684AC File Offset: 0x002666AC
		private static bool TryFindIngestibleToNurse(IntVec3 center, Pawn ingester, out Thing ingestible)
		{
			JoyGiver_SocialRelax.<>c__DisplayClass8_0 CS$<>8__locals1 = new JoyGiver_SocialRelax.<>c__DisplayClass8_0();
			CS$<>8__locals1.ingester = ingester;
			if (CS$<>8__locals1.ingester.IsTeetotaler())
			{
				ingestible = null;
				return false;
			}
			if (!new HistoryEvent(HistoryEventDefOf.IngestedRecreationalDrug, CS$<>8__locals1.ingester.Named(HistoryEventArgsNames.Doer)).DoerWillingToDo())
			{
				ingestible = null;
				return false;
			}
			if (CS$<>8__locals1.ingester.drugs == null)
			{
				ingestible = null;
				return false;
			}
			JoyGiver_SocialRelax.nurseableDrugs.Clear();
			DrugPolicy currentPolicy = CS$<>8__locals1.ingester.drugs.CurrentPolicy;
			for (int i = 0; i < currentPolicy.Count; i++)
			{
				if (currentPolicy[i].allowedForJoy && currentPolicy[i].drug.ingestible.nurseable)
				{
					JoyGiver_SocialRelax.nurseableDrugs.Add(currentPolicy[i].drug);
				}
			}
			JoyGiver_SocialRelax.nurseableDrugs.Shuffle<ThingDef>();
			for (int j = 0; j < JoyGiver_SocialRelax.nurseableDrugs.Count; j++)
			{
				List<Thing> list = CS$<>8__locals1.ingester.Map.listerThings.ThingsOfDef(JoyGiver_SocialRelax.nurseableDrugs[j]);
				if (list.Count > 0)
				{
					ingestible = GenClosest.ClosestThing_Global_Reachable(center, CS$<>8__locals1.ingester.Map, list, PathEndMode.OnCell, TraverseParms.For(CS$<>8__locals1.ingester, Danger.Deadly, TraverseMode.ByPawn, false, false, false, true), 40f, new Predicate<Thing>(CS$<>8__locals1.<TryFindIngestibleToNurse>g__Validator|0), null, false);
					if (ingestible != null)
					{
						return true;
					}
				}
			}
			ingestible = null;
			return false;
		}

		// Token: 0x06007FF4 RID: 32756 RVA: 0x00268608 File Offset: 0x00266808
		private static bool TryFindChairBesideTable(Thing table, Pawn sitter, out Thing chair)
		{
			for (int i = 0; i < 30; i++)
			{
				Building edifice = table.RandomAdjacentCellCardinal().GetEdifice(table.Map);
				CompAssignableToPawn_Throne compAssignableToPawn_Throne = edifice.TryGetComp<CompAssignableToPawn_Throne>();
				IntVec3 intVec;
				if (edifice != null && edifice.def.building.isSittable && !edifice.Fogged() && !edifice.IsForbidden(sitter) && Toils_Ingest.TryFindFreeSittingSpotOnThing(edifice, sitter, out intVec) && !edifice.Position.VacuumConcernTo(sitter) && (compAssignableToPawn_Throne == null || compAssignableToPawn_Throne.AssignedPawns.Contains(sitter)))
				{
					chair = edifice;
					return true;
				}
			}
			chair = null;
			return false;
		}

		// Token: 0x06007FF5 RID: 32757 RVA: 0x00268694 File Offset: 0x00266894
		private static bool TryFindChairNear(IntVec3 center, Pawn sitter, out Thing chair)
		{
			for (int i = 0; i < JoyGiver_SocialRelax.RadialPatternMiddleOutward.Count; i++)
			{
				Building edifice = (center + JoyGiver_SocialRelax.RadialPatternMiddleOutward[i]).GetEdifice(sitter.Map);
				CompAssignableToPawn_Throne compAssignableToPawn_Throne = edifice.TryGetComp<CompAssignableToPawn_Throne>();
				IntVec3 intVec;
				if (edifice != null && edifice.def.building.isSittable && !edifice.Fogged() && !edifice.IsForbidden(sitter) && !edifice.VacuumConcernTo(sitter) && Toils_Ingest.TryFindFreeSittingSpotOnThing(edifice, sitter, out intVec) && GenSight.LineOfSight(center, edifice.Position, sitter.Map, true, null, 0, 0) && (compAssignableToPawn_Throne == null || compAssignableToPawn_Throne.AssignedPawns.Contains(sitter)))
				{
					chair = edifice;
					return true;
				}
			}
			chair = null;
			return false;
		}

		// Token: 0x06007FF6 RID: 32758 RVA: 0x0026874C File Offset: 0x0026694C
		private static bool TryFindSitSpotOnGroundNear(IntVec3 center, Pawn sitter, out IntVec3 result)
		{
			for (int i = 0; i < 30; i++)
			{
				IntVec3 intVec = center + GenRadial.RadialPattern[Rand.Range(1, JoyGiver_SocialRelax.NumRadiusCells)];
				if (sitter.CanReserveAndReach(intVec, PathEndMode.OnCell, Danger.None, 1, -1, null, false) && intVec.GetEdifice(sitter.Map) == null && !intVec.Fogged(sitter.Map) && !intVec.VacuumConcernTo(sitter) && GenSight.LineOfSight(center, intVec, sitter.Map, true, null, 0, 0))
				{
					result = intVec;
					return true;
				}
			}
			result = IntVec3.Invalid;
			return false;
		}

		// Token: 0x0400558D RID: 21901
		private static readonly List<CompGatherSpot> workingSpots = new List<CompGatherSpot>();

		// Token: 0x0400558E RID: 21902
		private const float GatherRadius = 3.9f;

		// Token: 0x0400558F RID: 21903
		private static readonly int NumRadiusCells = GenRadial.NumCellsInRadius(3.9f);

		// Token: 0x04005590 RID: 21904
		private static readonly List<IntVec3> RadialPatternMiddleOutward = (from c in GenRadial.RadialPattern.Take(JoyGiver_SocialRelax.NumRadiusCells)
		orderby Mathf.Abs((c - IntVec3.Zero).LengthHorizontal - 1.95f)
		select c).ToList<IntVec3>();

		// Token: 0x04005591 RID: 21905
		private static readonly List<ThingDef> nurseableDrugs = new List<ThingDef>();
	}
}
