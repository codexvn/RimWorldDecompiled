using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02001D97 RID: 7575
	public abstract class IncidentWorker_EntitySwarm : IncidentWorker
	{
		// Token: 0x17001A12 RID: 6674
		// (get) Token: 0x0600A974 RID: 43380 RVA: 0x00307F2E File Offset: 0x0030612E
		protected virtual FloatRange SwarmSizeVariance { get; } = new FloatRange(0.7f, 1.3f);

		// Token: 0x17001A13 RID: 6675
		// (get) Token: 0x0600A975 RID: 43381
		protected abstract PawnGroupKindDef GroupKindDef { get; }

		// Token: 0x0600A976 RID: 43382 RVA: 0x00009C2B File Offset: 0x00007E2B
		protected virtual float TransformPoints(float points)
		{
			return points;
		}

		// Token: 0x0600A977 RID: 43383 RVA: 0x00307F36 File Offset: 0x00306136
		protected override bool CanFireNowSub(IncidentParms parms)
		{
			return ModsConfig.AnomalyActive && parms.points >= Faction.OfEntities.def.MinPointsToGeneratePawnGroup(this.GroupKindDef, null) && base.CanFireNowSub(parms);
		}

		// Token: 0x0600A978 RID: 43384 RVA: 0x00307F68 File Offset: 0x00306168
		protected override bool TryExecuteWorker(IncidentParms parms)
		{
			if (!ModsConfig.AnomalyActive)
			{
				return false;
			}
			Map map = (Map)parms.target;
			int num = 0;
			IntVec3 invalid = IntVec3.Invalid;
			IntVec3 invalid2 = IntVec3.Invalid;
			bool flag = false;
			while (!flag && num < 5)
			{
				RCellFinder.TryFindRandomPawnEntryCell(out invalid, map, CellFinder.EdgeRoadChance_Hostile, false, null);
				flag = RCellFinder.TryFindTravelDestFrom(invalid, map, out invalid2);
				num++;
			}
			if (!flag)
			{
				return false;
			}
			parms.spawnCenter = invalid;
			List<Pawn> list = this.GenerateEntities(parms, this.TransformPoints(parms.points));
			if (list.NullOrEmpty<Pawn>())
			{
				return false;
			}
			if (AnomalyIncidentUtility.IncidentShardChance(parms.points))
			{
				AnomalyIncidentUtility.PawnShardOnDeath(list.RandomElement<Pawn>());
			}
			IntVec3 spawnCenter = parms.spawnCenter;
			if (!spawnCenter.IsValid && !RCellFinder.TryFindRandomPawnEntryCell(out spawnCenter, map, CellFinder.EdgeRoadChance_Hostile, false, null))
			{
				return false;
			}
			Rot4 rot = Rot4.FromAngleFlat((map.Center - spawnCenter).AngleFlat);
			foreach (Pawn pawn in list)
			{
				IntVec3 loc = CellFinder.RandomClosewalkCellNear(spawnCenter, map, 10, null);
				GenSpawn.Spawn(pawn, loc, map, rot, WipeMode.Vanish, false, false);
				QuestUtility.AddQuestTag(pawn, parms.questTag);
			}
			this.SendLetter(parms, list);
			Find.TickManager.slower.SignalForceNormalSpeedShort();
			LordMaker.MakeNewLord(parms.faction, this.GenerateLordJob(invalid, invalid2), map, list);
			return true;
		}

		// Token: 0x0600A979 RID: 43385
		protected abstract LordJob GenerateLordJob(IntVec3 entry, IntVec3 dest);

		// Token: 0x0600A97A RID: 43386 RVA: 0x003080E0 File Offset: 0x003062E0
		protected virtual void SendLetter(IncidentParms parms, List<Pawn> entities)
		{
			base.SendStandardLetter(parms, entities, Array.Empty<NamedArgument>());
		}

		// Token: 0x0600A97B RID: 43387 RVA: 0x003080F4 File Offset: 0x003062F4
		protected virtual List<Pawn> GenerateEntities(IncidentParms parms, float points)
		{
			Map map = (Map)parms.target;
			PawnGroupMakerParms pawnGroupMakerParms = new PawnGroupMakerParms
			{
				groupKind = this.GroupKindDef,
				tile = map.Tile,
				faction = Faction.OfEntities,
				points = points * this.SwarmSizeVariance.RandomInRange
			};
			pawnGroupMakerParms.points = Mathf.Max(pawnGroupMakerParms.points, Faction.OfEntities.def.MinPointsToGeneratePawnGroup(pawnGroupMakerParms.groupKind, null) * 1.05f);
			return PawnGroupMakerUtility.GeneratePawns(pawnGroupMakerParms, true).ToList<Pawn>();
		}
	}
}
