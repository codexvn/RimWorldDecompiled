using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020028DD RID: 10461
	public class ScenPart_PursuingMechanoids : ScenPart
	{
		// Token: 0x170022B9 RID: 8889
		// (get) Token: 0x0600E4B4 RID: 58548 RVA: 0x0042CC2C File Offset: 0x0042AE2C
		private Alert_MechThreat AlertCached
		{
			get
			{
				if (this.Disabled)
				{
					return null;
				}
				if (this.cachedAlertMap != Find.CurrentMap)
				{
					this.alertCached = null;
				}
				if (this.alertCached != null)
				{
					return this.alertCached;
				}
				int num;
				if (this.mapWarningTimers.TryGetValue(Find.CurrentMap, out num) && Find.TickManager.TicksGame > num)
				{
					this.alertCached = new Alert_MechThreat
					{
						raidTick = this.mapRaidTimers[Find.CurrentMap]
					};
					this.cachedAlertMap = Find.CurrentMap;
				}
				return this.alertCached;
			}
		}

		// Token: 0x170022BA RID: 8890
		// (get) Token: 0x0600E4B5 RID: 58549 RVA: 0x0042CCB9 File Offset: 0x0042AEB9
		private bool Disabled
		{
			get
			{
				return this.questCompleted || !this.hasGravEngineCached;
			}
		}

		// Token: 0x170022BB RID: 8891
		// (get) Token: 0x0600E4B6 RID: 58550 RVA: 0x0042CCCE File Offset: 0x0042AECE
		public override bool OverrideDangerMusic
		{
			get
			{
				return this.onStartMap;
			}
		}

		// Token: 0x0600E4B7 RID: 58551 RVA: 0x0042CCD8 File Offset: 0x0042AED8
		public override void ExposeData()
		{
			base.ExposeData();
			if (Scribe.mode == LoadSaveMode.Saving)
			{
				foreach (Map map in this.mapWarningTimers.Keys.ToList<Map>())
				{
					if (((map != null) ? map.Parent : null) == null || map.Parent.Destroyed)
					{
						this.mapWarningTimers.Remove(map);
						this.mapRaidTimers.Remove(map);
					}
				}
			}
			Scribe_Values.Look<bool>(ref this.onStartMap, "initialMap", false, false);
			Scribe_Collections.Look<Map, int>(ref this.mapWarningTimers, "mapWarningTimers", LookMode.Reference, LookMode.Value, ref this.tmpWarningKeys, ref this.tmpWarningValues, true, false, false);
			Scribe_Collections.Look<Map, int>(ref this.mapRaidTimers, "mapRaidTimers", LookMode.Reference, LookMode.Value, ref this.tmpRaidKeys, ref this.tmpRaidValues, true, false, false);
			Scribe_Values.Look<bool>(ref this.questCompleted, "questCompleted", false, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				if (this.mapWarningTimers == null)
				{
					this.mapWarningTimers = new Dictionary<Map, int>();
				}
				if (this.mapRaidTimers == null)
				{
					this.mapRaidTimers = new Dictionary<Map, int>();
				}
			}
			this.lastCheckedGravEngineTick = -999999;
		}

		// Token: 0x0600E4B8 RID: 58552 RVA: 0x0042CE10 File Offset: 0x0042B010
		public override void PostWorldGenerate()
		{
			this.onStartMap = true;
			this.mapWarningTimers.Clear();
			this.mapRaidTimers.Clear();
		}

		// Token: 0x0600E4B9 RID: 58553 RVA: 0x0042CE2F File Offset: 0x0042B02F
		public override void PostMapGenerate(Map map)
		{
			if (!map.IsPlayerHome)
			{
				return;
			}
			this.StartTimers(map);
			this.lastCheckedGravEngineTick = -999999;
		}

		// Token: 0x0600E4BA RID: 58554 RVA: 0x0042CE4C File Offset: 0x0042B04C
		public override void MapRemoved(Map map)
		{
			if (this.mapWarningTimers.Remove(map))
			{
				this.mapRaidTimers.Remove(map);
				this.onStartMap = false;
			}
		}

		// Token: 0x0600E4BB RID: 58555 RVA: 0x0042CE70 File Offset: 0x0042B070
		public override void Tick()
		{
			if (Find.TickManager.TicksGame > this.lastCheckedGravEngineTick + 2500)
			{
				this.hasGravEngineCached = GravshipUtility.PlayerHasGravEngine();
				this.lastCheckedGravEngineTick = Find.TickManager.TicksGame;
			}
			if (this.Disabled)
			{
				foreach (Map map in Find.Maps)
				{
					if (this.mapWarningTimers.ContainsKey(map))
					{
						Dictionary<Map, int> dictionary = this.mapWarningTimers;
						Map key = map;
						int num = dictionary[key];
						dictionary[key] = num + 1;
						Dictionary<Map, int> dictionary2 = this.mapRaidTimers;
						key = map;
						num = dictionary2[key];
						dictionary2[key] = num + 1;
					}
				}
				return;
			}
			foreach (Map map2 in this.mapWarningTimers.Keys)
			{
				if (Find.TickManager.TicksGame == this.mapWarningTimers[map2])
				{
					Thing t = map2.listerThings.ThingsOfDef(ThingDefOf.PilotConsole).FirstOrDefault<Thing>();
					Find.LetterStack.ReceiveLetter("LetterLabelMechanoidThreat".Translate(), "LetterTextMechanoidThreat".Translate(), LetterDefOf.ThreatSmall, t, null, null, null, null, 0, true);
				}
			}
			foreach (Map map3 in this.mapRaidTimers.Keys)
			{
				if (Find.TickManager.TicksGame >= this.mapRaidTimers[map3] && (Find.TickManager.TicksGame - this.mapRaidTimers[map3]) % 30000 == 0)
				{
					this.FireRaid(map3);
				}
			}
		}

		// Token: 0x0600E4BC RID: 58556 RVA: 0x0042D060 File Offset: 0x0042B260
		private void StartTimers(Map map)
		{
			if (map.generatorDef == MapGeneratorDefOf.Mechhive)
			{
				return;
			}
			if (this.onStartMap)
			{
				this.mapWarningTimers[map] = Find.TickManager.TicksGame + 2700;
				this.mapRaidTimers[map] = Find.TickManager.TicksGame + 30000;
				return;
			}
			this.mapWarningTimers[map] = Find.TickManager.TicksGame + ScenPart_PursuingMechanoids.WarningDelayRange.RandomInRange;
			this.mapRaidTimers[map] = Find.TickManager.TicksGame + ScenPart_PursuingMechanoids.RaidDelayRange.RandomInRange;
		}

		// Token: 0x0600E4BD RID: 58557 RVA: 0x0042D0FE File Offset: 0x0042B2FE
		public void Notify_QuestCompleted()
		{
			this.questCompleted = true;
		}

		// Token: 0x0600E4BE RID: 58558 RVA: 0x0042D108 File Offset: 0x0042B308
		private void FireRaid(Map map)
		{
			IncidentParms incidentParms = new IncidentParms();
			incidentParms.forced = true;
			incidentParms.target = map;
			incidentParms.points = Mathf.Max(5000f, StorytellerUtility.DefaultThreatPointsNow(map) * 1.5f);
			incidentParms.faction = Faction.OfMechanoids;
			incidentParms.raidArrivalMode = PawnsArrivalModeDefOf.RandomDrop;
			incidentParms.raidStrategy = RaidStrategyDefOf.ImmediateAttack;
			IncidentDefOf.RaidEnemy.Worker.TryExecute(incidentParms);
		}

		// Token: 0x0600E4BF RID: 58559 RVA: 0x0042D177 File Offset: 0x0042B377
		public override IEnumerable<Alert> GetAlerts()
		{
			Map currentMap = Find.CurrentMap;
			if (currentMap == null || !currentMap.IsPlayerHome)
			{
				yield break;
			}
			if (this.AlertCached != null)
			{
				yield return this.AlertCached;
			}
			yield break;
		}

		// Token: 0x0400990F RID: 39183
		private bool onStartMap = true;

		// Token: 0x04009910 RID: 39184
		private Dictionary<Map, int> mapWarningTimers = new Dictionary<Map, int>();

		// Token: 0x04009911 RID: 39185
		private Dictionary<Map, int> mapRaidTimers = new Dictionary<Map, int>();

		// Token: 0x04009912 RID: 39186
		private bool questCompleted;

		// Token: 0x04009913 RID: 39187
		private const int InitialWarningDelay = 2700;

		// Token: 0x04009914 RID: 39188
		private const int InitialRaidDelay = 30000;

		// Token: 0x04009915 RID: 39189
		private static IntRange WarningDelayRange = new IntRange(840000, 960000);

		// Token: 0x04009916 RID: 39190
		private static IntRange RaidDelayRange = new IntRange(1080000, 2100000);

		// Token: 0x04009917 RID: 39191
		private const int MinRaidPoints = 5000;

		// Token: 0x04009918 RID: 39192
		private const float RaidPointMultiplier = 1.5f;

		// Token: 0x04009919 RID: 39193
		private Map cachedAlertMap;

		// Token: 0x0400991A RID: 39194
		private Alert_MechThreat alertCached;

		// Token: 0x0400991B RID: 39195
		private bool hasGravEngineCached = true;

		// Token: 0x0400991C RID: 39196
		private int lastCheckedGravEngineTick = -999999;

		// Token: 0x0400991D RID: 39197
		private const int GravEngineCheckInterval = 2500;

		// Token: 0x0400991E RID: 39198
		private List<Map> tmpWarningKeys;

		// Token: 0x0400991F RID: 39199
		private List<int> tmpWarningValues;

		// Token: 0x04009920 RID: 39200
		private List<Map> tmpRaidKeys;

		// Token: 0x04009921 RID: 39201
		private List<int> tmpRaidValues;
	}
}
