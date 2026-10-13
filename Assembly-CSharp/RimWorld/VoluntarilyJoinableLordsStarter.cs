using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02002199 RID: 8601
	public class VoluntarilyJoinableLordsStarter : IExposable
	{
		// Token: 0x0600BBDB RID: 48091 RVA: 0x00366C45 File Offset: 0x00364E45
		public VoluntarilyJoinableLordsStarter(Map map)
		{
			this.map = map;
		}

		// Token: 0x0600BBDC RID: 48092 RVA: 0x00366C5F File Offset: 0x00364E5F
		public bool TryStartMarriageCeremony(Pawn firstFiance, Pawn secondFiance)
		{
			if (!GatheringDefOf.MarriageCeremony.CanExecute(firstFiance.Map, firstFiance, true))
			{
				return false;
			}
			GatheringDefOf.MarriageCeremony.Worker.TryExecute(firstFiance.Map, firstFiance);
			this.lastLordStartTick = Find.TickManager.TicksGame;
			return true;
		}

		// Token: 0x0600BBDD RID: 48093 RVA: 0x00366CA0 File Offset: 0x00364EA0
		public bool TryStartRandomGathering(bool forceStart = false)
		{
			VoluntarilyJoinableLordsStarter.tmpGatherings.Clear();
			foreach (GatheringDef gatheringDef in DefDatabase<GatheringDef>.AllDefsListForReading)
			{
				if (gatheringDef.IsRandomSelectable && gatheringDef.CanExecute(this.map, null, forceStart))
				{
					VoluntarilyJoinableLordsStarter.tmpGatherings.Add(gatheringDef);
				}
			}
			GatheringDef gatheringDef2;
			return VoluntarilyJoinableLordsStarter.tmpGatherings.TryRandomElementByWeight((GatheringDef def) => def.randomSelectionWeight, out gatheringDef2) && this.TryStartGathering(gatheringDef2);
		}

		// Token: 0x0600BBDE RID: 48094 RVA: 0x00366D50 File Offset: 0x00364F50
		public bool TryStartGathering(GatheringDef gatheringDef)
		{
			if (!gatheringDef.Worker.TryExecute(this.map, null))
			{
				return false;
			}
			this.lastLordStartTick = Find.TickManager.TicksGame;
			this.startRandomGatheringASAP = false;
			return true;
		}

		// Token: 0x0600BBDF RID: 48095 RVA: 0x00366D80 File Offset: 0x00364F80
		public void VoluntarilyJoinableLordsStarterTick()
		{
			this.Tick_TryStartRandomGathering();
		}

		// Token: 0x0600BBE0 RID: 48096 RVA: 0x00366D88 File Offset: 0x00364F88
		public void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.lastLordStartTick, "lastLordStartTick", 0, false);
			Scribe_Values.Look<bool>(ref this.startRandomGatheringASAP, "startPartyASAP", false, false);
		}

		// Token: 0x0600BBE1 RID: 48097 RVA: 0x00366DB0 File Offset: 0x00364FB0
		private void Tick_TryStartRandomGathering()
		{
			if (!this.map.IsPlayerHome)
			{
				return;
			}
			if (Find.TickManager.TicksGame % 5000 == 0)
			{
				if (Rand.MTBEventOccurs(40f, 60000f, 5000f))
				{
					this.startRandomGatheringASAP = true;
				}
				if (this.startRandomGatheringASAP && Find.TickManager.TicksGame - this.lastLordStartTick >= 600000)
				{
					this.TryStartRandomGathering(false);
				}
			}
		}

		// Token: 0x0400815E RID: 33118
		private Map map;

		// Token: 0x0400815F RID: 33119
		private int lastLordStartTick = -999999;

		// Token: 0x04008160 RID: 33120
		private bool startRandomGatheringASAP;

		// Token: 0x04008161 RID: 33121
		private const int CheckStartGatheringIntervalTicks = 5000;

		// Token: 0x04008162 RID: 33122
		private const float StartGatheringMTBDays = 40f;

		// Token: 0x04008163 RID: 33123
		private static List<GatheringDef> tmpGatherings = new List<GatheringDef>();
	}
}
