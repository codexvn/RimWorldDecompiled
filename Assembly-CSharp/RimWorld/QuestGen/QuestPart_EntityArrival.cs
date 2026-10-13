using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Utility;
using UnityEngine;
using Verse;
using Verse.AI.Group;

namespace RimWorld.QuestGen
{
	// Token: 0x02003BB0 RID: 15280
	public class QuestPart_EntityArrival : QuestPart
	{
		// Token: 0x06014BFB RID: 84987 RVA: 0x002D92DC File Offset: 0x002D74DC
		public QuestPart_EntityArrival()
		{
		}

		// Token: 0x06014BFC RID: 84988 RVA: 0x0063769B File Offset: 0x0063589B
		public QuestPart_EntityArrival(string inSignal, Map map, Building_VoidMonolith monolith, int voidAwakeningBeganTick, List<CellRect> structureRects)
		{
			this.inSignal = inSignal;
			this.map = map;
			this.monolith = monolith;
			this.voidAwakeningBeganTick = voidAwakeningBeganTick;
			this.structureRects = structureRects;
		}

		// Token: 0x06014BFD RID: 84989 RVA: 0x006376C8 File Offset: 0x006358C8
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Map>(ref this.map, "map", false);
			Scribe_Collections.Look<CellRect>(ref this.structureRects, "structureRects", LookMode.Value, Array.Empty<object>());
			Scribe_References.Look<Building_VoidMonolith>(ref this.monolith, "monolith", false);
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_Values.Look<int>(ref this.voidAwakeningBeganTick, "voidAwakeningBeganTick", 0, false);
		}

		// Token: 0x06014BFE RID: 84990 RVA: 0x00637738 File Offset: 0x00635938
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			if (!signal.tag.StartsWith(this.inSignal))
			{
				return;
			}
			VoidAwakeningUtility.WaveType waveType;
			float num;
			VoidAwakeningUtility.DecodeWaveType(signal.tag, out waveType, out num);
			if (waveType == VoidAwakeningUtility.WaveType.Twisted)
			{
				waveType = QuestPart_EntityArrival.TwistedWaveOptions.RandomElement<VoidAwakeningUtility.WaveType>();
			}
			float points = StorytellerUtility.DefaultThreatPointsNow(this.map) * num;
			string str = "";
			string str2 = "";
			LookTargets lookTargets = null;
			switch (waveType)
			{
			case VoidAwakeningUtility.WaveType.Fleshbeast:
				lookTargets = this.SpawnFleshbeastBurrows(points, this.structureRects);
				str = "VoidAwakeningFleshbeastBurrowLabel".Translate();
				str2 = "VoidAwakeningFleshbeastBurrowText".Translate();
				break;
			case VoidAwakeningUtility.WaveType.NoctolSightstealer:
				this.FireSightstealerNoctolAssault(points);
				str = "VoidAwakeningEntityArrivalLabel".Translate();
				str2 = "VoidAwakeningEntityArrivalText".Translate();
				break;
			case VoidAwakeningUtility.WaveType.DevourerGorehulk:
				lookTargets = this.FireDevourerGorehulkAssault(points);
				str = "VoidAwakeningEntityArrivalLabel".Translate();
				str2 = "VoidAwakeningEntityArrivalText".Translate();
				break;
			case VoidAwakeningUtility.WaveType.ShamblerGorehulk:
				lookTargets = this.FireShamblerGorehulkAssault(points);
				str = "VoidAwakeningEntityArrivalLabel".Translate();
				str2 = "VoidAwakeningEntityArrivalText".Translate();
				break;
			}
			if (!str.NullOrEmpty())
			{
				Find.LetterStack.ReceiveLetter(str, str2, LetterDefOf.ThreatBig, lookTargets, null, null, null, null, 0, true);
			}
		}

		// Token: 0x06014BFF RID: 84991 RVA: 0x006378A8 File Offset: 0x00635AA8
		private List<Thing> SpawnFleshbeastBurrows(float points, List<CellRect> structureRects)
		{
			List<Thing> list = new List<Thing>();
			LargeBuildingSpawnParms parms = QuestNode_Root_VoidAwakening.StructureSpawnParms.ForThing(ThingDefOf.PitBurrow);
			LargeBuildingSpawnParms parms2 = QuestNode_Root_VoidAwakening.StructureSpawnParms.ForThing(ThingDefOf.PitBurrow);
			Predicate<IntVec3> extraValidator = delegate(IntVec3 cell)
			{
				foreach (CellRect cellRect in structureRects)
				{
					if (cell.InHorDistOf(cellRect.CenterCell, 50f))
					{
						return false;
					}
				}
				return true;
			};
			List<CellRect> list2 = new List<CellRect>();
			list2.AddRange(structureRects);
			int num = 0;
			IntVec3 cell2;
			while (num < 6 && (LargeBuildingCellFinder.TryFindCell(out cell2, this.map, parms, list2, extraValidator, false) || LargeBuildingCellFinder.TryFindCell(out cell2, this.map, parms2, list2, null, false)))
			{
				Thing thing = FleshbeastUtility.SpawnFleshbeastsFromPitBurrowEmergence(cell2, this.map, points / 6f, QuestPart_EntityArrival.PitBurrowEmergenceDelayRangeTicks, QuestPart_EntityArrival.FleshbeastSpawnDelayTicks, true);
				list.Add(thing);
				list2.Add(thing.OccupiedRect());
				num++;
			}
			return list;
		}

		// Token: 0x06014C00 RID: 84992 RVA: 0x00637984 File Offset: 0x00635B84
		private List<Pawn> FireSightstealerNoctolAssault(float points)
		{
			float points2 = Mathf.Max(points, Faction.OfEntities.def.MinPointsToGeneratePawnGroup(PawnGroupKindDefOf.SightstealersNoctols, null) * 1.05f);
			IncidentParms incidentParms = new IncidentParms
			{
				target = this.map,
				raidArrivalMode = PawnsArrivalModeDefOf.EdgeWalkIn,
				sendLetter = false,
				faction = Faction.OfEntities
			};
			if (!incidentParms.raidArrivalMode.Worker.TryResolveRaidSpawnCenter(incidentParms))
			{
				return null;
			}
			List<Pawn> list = PawnGroupMakerUtility.GeneratePawns(new PawnGroupMakerParms
			{
				groupKind = PawnGroupKindDefOf.SightstealersNoctols,
				points = points2,
				faction = Faction.OfEntities
			}, true).ToList<Pawn>();
			PawnsArrivalModeDefOf.EdgeWalkInDistributedGroups.Worker.Arrive(list, incidentParms);
			LordMaker.MakeNewLord(Faction.OfEntities, new LordJob_AssaultColony(incidentParms.faction, false, false, false, false, false, false, false), this.map, list);
			return list.ToList<Pawn>();
		}

		// Token: 0x06014C01 RID: 84993 RVA: 0x00637A60 File Offset: 0x00635C60
		private List<Pawn> FireShamblerGorehulkAssault(float points)
		{
			float points2 = Mathf.Max(points / 2f, Faction.OfEntities.def.MinPointsToGeneratePawnGroup(PawnGroupKindDefOf.Shamblers, null) * 1.05f);
			float points3 = Mathf.Max(points / 2f, Faction.OfEntities.def.MinPointsToGeneratePawnGroup(PawnGroupKindDefOf.Gorehulks, null) * 1.05f);
			IncidentParms incidentParms = new IncidentParms
			{
				target = this.map,
				raidArrivalMode = PawnsArrivalModeDefOf.EdgeWalkIn,
				sendLetter = false,
				faction = Faction.OfEntities
			};
			if (!incidentParms.raidArrivalMode.Worker.TryResolveRaidSpawnCenter(incidentParms))
			{
				return null;
			}
			List<Pawn> list = PawnGroupMakerUtility.GeneratePawns(new PawnGroupMakerParms
			{
				groupKind = PawnGroupKindDefOf.Shamblers,
				points = points2,
				faction = Faction.OfEntities
			}, true).ToList<Pawn>();
			PawnsArrivalModeDefOf.EdgeWalkInDistributedGroups.Worker.Arrive(list, incidentParms);
			LordMaker.MakeNewLord(Faction.OfEntities, new LordJob_ShamblerAssault(), this.map, list);
			List<Pawn> list2 = PawnGroupMakerUtility.GeneratePawns(new PawnGroupMakerParms
			{
				groupKind = PawnGroupKindDefOf.Gorehulks,
				points = points3,
				faction = Faction.OfEntities
			}, true).ToList<Pawn>();
			PawnsArrivalModeDefOf.EdgeWalkInDistributedGroups.Worker.Arrive(list2, incidentParms);
			LordMaker.MakeNewLord(Faction.OfEntities, new LordJob_GorehulkAssault(), this.map, list2);
			return list.Concat(list2).ToList<Pawn>();
		}

		// Token: 0x06014C02 RID: 84994 RVA: 0x00637BC0 File Offset: 0x00635DC0
		private List<Pawn> FireDevourerGorehulkAssault(float points)
		{
			float points2 = Mathf.Max(points / 2f, Faction.OfEntities.def.MinPointsToGeneratePawnGroup(PawnGroupKindDefOf.Devourers, null) * 1.05f);
			float points3 = Mathf.Max(points / 2f, Faction.OfEntities.def.MinPointsToGeneratePawnGroup(PawnGroupKindDefOf.Gorehulks, null) * 1.05f);
			IncidentParms incidentParms = new IncidentParms
			{
				target = this.map,
				raidArrivalMode = PawnsArrivalModeDefOf.EdgeWalkIn,
				sendLetter = false,
				faction = Faction.OfEntities
			};
			if (!incidentParms.raidArrivalMode.Worker.TryResolveRaidSpawnCenter(incidentParms))
			{
				return null;
			}
			List<Pawn> list = PawnGroupMakerUtility.GeneratePawns(new PawnGroupMakerParms
			{
				groupKind = PawnGroupKindDefOf.Devourers,
				points = points2,
				faction = Faction.OfEntities
			}, true).ToList<Pawn>();
			PawnsArrivalModeDefOf.EdgeWalkInDistributedGroups.Worker.Arrive(list, incidentParms);
			LordMaker.MakeNewLord(Faction.OfEntities, new LordJob_DevourerAssault(), this.map, list);
			List<Pawn> list2 = PawnGroupMakerUtility.GeneratePawns(new PawnGroupMakerParms
			{
				groupKind = PawnGroupKindDefOf.Gorehulks,
				points = points3,
				faction = Faction.OfEntities
			}, true).ToList<Pawn>();
			PawnsArrivalModeDefOf.EdgeWalkInDistributedGroups.Worker.Arrive(list2, incidentParms);
			LordMaker.MakeNewLord(Faction.OfEntities, new LordJob_GorehulkAssault(), this.map, list2);
			return list.Concat(list2).ToList<Pawn>();
		}

		// Token: 0x06014C03 RID: 84995 RVA: 0x00637D1D File Offset: 0x00635F1D
		public override void Cleanup()
		{
			base.Cleanup();
			this.map = null;
			this.monolith = null;
			this.structureRects.Clear();
		}

		// Token: 0x0400E701 RID: 59137
		private const float BurrowMinDistFromStructure = 50f;

		// Token: 0x0400E702 RID: 59138
		private const int BurrowCount = 6;

		// Token: 0x0400E703 RID: 59139
		private static readonly IntRange FleshbeastSpawnDelayTicks = new IntRange(180, 180);

		// Token: 0x0400E704 RID: 59140
		private static readonly IntRange PitBurrowEmergenceDelayRangeTicks = new IntRange(300, 1200);

		// Token: 0x0400E705 RID: 59141
		private static readonly List<VoidAwakeningUtility.WaveType> TwistedWaveOptions = new List<VoidAwakeningUtility.WaveType>
		{
			VoidAwakeningUtility.WaveType.DevourerGorehulk,
			VoidAwakeningUtility.WaveType.ShamblerGorehulk,
			VoidAwakeningUtility.WaveType.NoctolSightstealer
		};

		// Token: 0x0400E706 RID: 59142
		private string inSignal;

		// Token: 0x0400E707 RID: 59143
		private Map map;

		// Token: 0x0400E708 RID: 59144
		private List<CellRect> structureRects;

		// Token: 0x0400E709 RID: 59145
		private Building_VoidMonolith monolith;

		// Token: 0x0400E70A RID: 59146
		private int voidAwakeningBeganTick;
	}
}
