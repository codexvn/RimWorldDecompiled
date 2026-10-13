using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001EA7 RID: 7847
	public class ComplexThreatWorker_RaidTerminal : ComplexThreatWorker
	{
		// Token: 0x0600AE13 RID: 44563 RVA: 0x0031A3CC File Offset: 0x003185CC
		protected override bool CanResolveInt(ComplexResolveParams parms)
		{
			Faction faction;
			IntVec3 intVec;
			return base.CanResolveInt(parms) && this.TryFindRandomEnemyFaction(parms, out faction) && ComplexUtility.TryFindRandomSpawnCell(ThingDefOf.AncientEnemyTerminal, parms.room, parms.map, out intVec, 1, null);
		}

		// Token: 0x0600AE14 RID: 44564 RVA: 0x0031A414 File Offset: 0x00318614
		protected override void ResolveInt(ComplexResolveParams parms, ref float threatPointsUsed, List<Thing> outSpawnedThings)
		{
			IntVec3 loc;
			ComplexUtility.TryFindRandomSpawnCell(ThingDefOf.AncientEnemyTerminal, parms.room, parms.map, out loc, 1, null);
			Thing thing = GenSpawn.Spawn(ThingDefOf.AncientEnemyTerminal, loc, parms.map, WipeMode.Vanish);
			Faction faction;
			this.TryFindRandomEnemyFaction(parms, out faction);
			float num = Mathf.Max(parms.points, faction.def.MinPointsToGeneratePawnGroup(PawnGroupKindDefOf.Combat, null) * 1.05f);
			IncidentParms incidentParms = new IncidentParms
			{
				forced = true,
				target = parms.map,
				points = num,
				faction = faction,
				raidArrivalMode = PawnsArrivalModeDefOf.EdgeWalkIn,
				raidStrategy = RaidStrategyDefOf.ImmediateAttack
			};
			CompHackable compHackable = thing.TryGetComp<CompHackable>();
			string text = compHackable.hackingCompletedSignal;
			if (text.NullOrEmpty())
			{
				text = "RaidSignal" + Find.UniqueIDsManager.GetNextSignalTagID().ToString();
				compHackable.hackingCompletedSignal = text;
			}
			SignalAction_Incident signalAction_Incident = (SignalAction_Incident)ThingMaker.MakeThing(ThingDefOf.SignalAction_Incident, null);
			signalAction_Incident.incident = IncidentDefOf.RaidEnemy;
			signalAction_Incident.incidentParms = incidentParms;
			signalAction_Incident.signalTag = text;
			GenSpawn.Spawn(signalAction_Incident, parms.room.rects[0].CenterCell, parms.map, WipeMode.Vanish);
			threatPointsUsed += num;
		}

		// Token: 0x0600AE15 RID: 44565 RVA: 0x0031A559 File Offset: 0x00318759
		private bool TryFindRandomEnemyFaction(ComplexResolveParams parms, out Faction faction)
		{
			if (parms.hostileFaction != null && parms.hostileFaction.def.humanlikeFaction)
			{
				faction = parms.hostileFaction;
				return true;
			}
			faction = Find.FactionManager.RandomRaidableEnemyFaction(false, false, false, TechLevel.Undefined);
			return faction != null;
		}

		// Token: 0x040077C5 RID: 30661
		private const string SignalPrefix = "RaidSignal";
	}
}
