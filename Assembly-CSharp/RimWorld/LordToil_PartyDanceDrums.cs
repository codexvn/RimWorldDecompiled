using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200173D RID: 5949
	public class LordToil_PartyDanceDrums : LordToil_Ritual
	{
		// Token: 0x17001588 RID: 5512
		// (get) Token: 0x06008BD9 RID: 35801 RVA: 0x0029A153 File Offset: 0x00298353
		public new LordToilData_PartyDanceDrums Data
		{
			get
			{
				return (LordToilData_PartyDanceDrums)this.data;
			}
		}

		// Token: 0x06008BDA RID: 35802 RVA: 0x0029A160 File Offset: 0x00298360
		public LordToil_PartyDanceDrums(IntVec3 spot, LordJob_Ritual ritual, RitualStage stage, Pawn organizer) : base(spot, ritual, stage, organizer)
		{
			this.data = new LordToilData_PartyDanceDrums();
		}

		// Token: 0x06008BDB RID: 35803 RVA: 0x0029A178 File Offset: 0x00298378
		public override void UpdateAllDuties()
		{
			this.reservedThings.Clear();
			for (int i = this.lord.ownedPawns.Count - 1; i >= 0; i--)
			{
				Pawn pawn = this.lord.ownedPawns[i];
				pawn.mindState.duty = this.DutyForPawn(pawn);
				pawn.jobs.EndCurrentJob(JobCondition.InterruptForced, true, true);
			}
		}

		// Token: 0x06008BDC RID: 35804 RVA: 0x0029A1E0 File Offset: 0x002983E0
		private PawnDuty DutyForPawn(Pawn pawn)
		{
			DutyDef def = this.stage.defaultDuty;
			IntVec3 spot = this.spot;
			LocalTargetInfo focusSecond = LocalTargetInfo.Invalid;
			int num = this.ritual.assignments.Participants.IndexOf(pawn);
			if (num != -1 && (num % 2 == 0 || this.ritual.assignments.Participants.Count == 0))
			{
				Building_MusicalInstrument building_MusicalInstrument = (from m in pawn.Map.listerBuildings.AllBuildingsColonistOfClass<Building_MusicalInstrument>()
				where GatheringsUtility.InGatheringArea(m.InteractionCell, this.spot, pawn.Map) && GatheringWorker_Concert.InstrumentAccessible(m, pawn)
				select m).RandomElementWithFallback(null);
				if (building_MusicalInstrument != null && building_MusicalInstrument.Spawned)
				{
					def = DutyDefOf.PlayTargetInstrument;
					focusSecond = building_MusicalInstrument;
					this.ritual.usedThings.Add(building_MusicalInstrument);
					this.reservedThings.Add(building_MusicalInstrument);
					this.Data.playedInstruments.SetOrAdd(pawn, building_MusicalInstrument);
				}
			}
			return new PawnDuty(def, spot, focusSecond, (LocalTargetInfo)this.ritual.selectedTarget, -1f);
		}

		// Token: 0x06008BDD RID: 35805 RVA: 0x0029A308 File Offset: 0x00298508
		public override void Notify_BuildingDespawnedOnMap(Building b)
		{
			if (b.def == ThingDefOf.Drum && this.Data.playedInstruments.ContainsValue(b))
			{
				Pawn key = this.Data.playedInstruments.First((KeyValuePair<Pawn, Building> kv) => kv.Value == b).Key;
				key.mindState.duty = this.DutyForPawn(key);
				key.jobs.EndCurrentJob(JobCondition.InterruptForced, true, true);
			}
		}
	}
}
