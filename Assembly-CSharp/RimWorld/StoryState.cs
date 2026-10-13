using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001E83 RID: 7811
	public class StoryState : IExposable
	{
		// Token: 0x17001A7D RID: 6781
		// (get) Token: 0x0600AD6C RID: 44396 RVA: 0x00318040 File Offset: 0x00316240
		public IIncidentTarget Target
		{
			get
			{
				return this.target;
			}
		}

		// Token: 0x17001A7E RID: 6782
		// (get) Token: 0x0600AD6D RID: 44397 RVA: 0x00318048 File Offset: 0x00316248
		public List<QuestScriptDef> RecentRandomQuests
		{
			get
			{
				return this.recentRandomQuests;
			}
		}

		// Token: 0x17001A7F RID: 6783
		// (get) Token: 0x0600AD6E RID: 44398 RVA: 0x00318050 File Offset: 0x00316250
		public List<QuestScriptDef> RecentRandomDecrees
		{
			get
			{
				return this.recentRandomDecrees;
			}
		}

		// Token: 0x17001A80 RID: 6784
		// (get) Token: 0x0600AD6F RID: 44399 RVA: 0x00318058 File Offset: 0x00316258
		public List<IncidentDef> RecentRandomIncidents
		{
			get
			{
				return this.recentRandomIncidents;
			}
		}

		// Token: 0x17001A81 RID: 6785
		// (get) Token: 0x0600AD70 RID: 44400 RVA: 0x00318060 File Offset: 0x00316260
		public int LastRoyalFavorQuestTick
		{
			get
			{
				return this.lastRoyalFavorQuestTick;
			}
		}

		// Token: 0x17001A82 RID: 6786
		// (get) Token: 0x0600AD71 RID: 44401 RVA: 0x00318068 File Offset: 0x00316268
		public int LastThreatBigTick
		{
			get
			{
				if (this.lastThreatBigTick > Find.TickManager.TicksGame + 1000)
				{
					Log.Error(string.Concat(new string[]
					{
						"Latest big threat queue time was ",
						this.lastThreatBigTick.ToString(),
						" at tick ",
						Find.TickManager.TicksGame.ToString(),
						". This is too far in the future. Resetting."
					}));
					this.lastThreatBigTick = Find.TickManager.TicksGame - 1;
				}
				return this.lastThreatBigTick;
			}
		}

		// Token: 0x0600AD72 RID: 44402 RVA: 0x003180F0 File Offset: 0x003162F0
		public StoryState(IIncidentTarget target)
		{
			this.target = target;
		}

		// Token: 0x0600AD73 RID: 44403 RVA: 0x00318150 File Offset: 0x00316350
		public void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.lastThreatBigTick, "lastThreatBigTick", 0, true);
			Scribe_Values.Look<int>(ref this.lastRoyalFavorQuestTick, "lastRoyalFavorQuestTick", 0, false);
			Scribe_References.Look<Faction>(ref this.lastRaidFaction, "lastRaidFaction", false);
			Scribe_Collections.Look<QuestScriptDef>(ref this.recentRandomQuests, "recentRandomQuests", LookMode.Def, Array.Empty<object>());
			Scribe_Collections.Look<QuestScriptDef>(ref this.recentRandomDecrees, "recentRandomDecrees", LookMode.Def, Array.Empty<object>());
			Scribe_Collections.Look<IncidentDef>(ref this.recentRandomIncidents, "recentRandomIncidents", LookMode.Def, Array.Empty<object>());
			Scribe_Collections.Look<int, int>(ref this.colonistCountTicks, "colonistCountTicks", LookMode.Value, LookMode.Value);
			Scribe_Collections.Look<IncidentDef, int>(ref this.lastFireTicks, "lastFireTicks", LookMode.Def, LookMode.Value);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				if (this.recentRandomQuests == null)
				{
					this.recentRandomQuests = new List<QuestScriptDef>();
				}
				if (this.recentRandomDecrees == null)
				{
					this.recentRandomDecrees = new List<QuestScriptDef>();
				}
				if (this.recentRandomIncidents == null)
				{
					this.recentRandomIncidents = new List<IncidentDef>();
				}
				if (this.colonistCountTicks == null)
				{
					this.colonistCountTicks = new Dictionary<int, int>();
				}
				this.RecordPopulationIncrease();
			}
		}

		// Token: 0x0600AD74 RID: 44404 RVA: 0x00318254 File Offset: 0x00316454
		public void Notify_IncidentFired(FiringIncident fi)
		{
			if (fi.parms.forced || fi.parms.target != this.target)
			{
				return;
			}
			int ticksGame = Find.TickManager.TicksGame;
			if (fi.def.category == IncidentCategoryDefOf.ThreatBig)
			{
				this.lastThreatBigTick = ticksGame;
				Find.StoryWatcher.statsRecord.numThreatBigs++;
			}
			this.lastFireTicks[fi.def] = ticksGame;
			if (fi.def != IncidentDefOf.GiveQuest_Random)
			{
				if (!fi.def.ShouldIgnoreRecentWeighting)
				{
					this.RecordRandomIncidentFired(fi.def);
				}
				return;
			}
			if (fi.parms.questScriptDef == null)
			{
				Log.Error("Tried to record random quest fired with null questScript. parms=" + fi.parms.ToString());
				return;
			}
			this.RecordRandomQuestFired(fi.parms.questScriptDef);
		}

		// Token: 0x0600AD75 RID: 44405 RVA: 0x00318330 File Offset: 0x00316530
		public void RecordRandomQuestFired(QuestScriptDef questScript)
		{
			this.recentRandomQuests.Insert(0, questScript);
			while (this.recentRandomQuests.Count > 5)
			{
				this.recentRandomQuests.RemoveAt(this.recentRandomQuests.Count - 1);
			}
			if (questScript.canGiveRoyalFavor)
			{
				this.lastRoyalFavorQuestTick = Find.TickManager.TicksGame;
			}
		}

		// Token: 0x0600AD76 RID: 44406 RVA: 0x0031838A File Offset: 0x0031658A
		public void RecordDecreeFired(QuestScriptDef questScript)
		{
			this.recentRandomDecrees.Insert(0, questScript);
			while (this.recentRandomDecrees.Count > 5)
			{
				this.recentRandomDecrees.RemoveAt(this.recentRandomDecrees.Count - 1);
			}
		}

		// Token: 0x0600AD77 RID: 44407 RVA: 0x003183C1 File Offset: 0x003165C1
		private void RecordRandomIncidentFired(IncidentDef incident)
		{
			this.recentRandomIncidents.Insert(0, incident);
			while (this.recentRandomIncidents.Count > 5)
			{
				this.recentRandomIncidents.RemoveAt(this.recentRandomIncidents.Count - 1);
			}
		}

		// Token: 0x0600AD78 RID: 44408 RVA: 0x003183F8 File Offset: 0x003165F8
		public void RecordPopulationIncrease()
		{
			int count = PawnsFinder.AllMapsCaravansAndTravellingTransporters_Alive_FreeColonists.Count;
			if (!this.colonistCountTicks.ContainsKey(count))
			{
				this.colonistCountTicks.Add(count, Find.TickManager.TicksGame);
			}
		}

		// Token: 0x0600AD79 RID: 44409 RVA: 0x00318434 File Offset: 0x00316634
		public int GetTicksFromColonistCount(int count)
		{
			if (!this.colonistCountTicks.ContainsKey(count))
			{
				this.colonistCountTicks.Add(count, Find.TickManager.TicksGame);
			}
			return this.colonistCountTicks[count];
		}

		// Token: 0x0600AD7A RID: 44410 RVA: 0x00318468 File Offset: 0x00316668
		public void CopyTo(StoryState other)
		{
			other.lastThreatBigTick = this.lastThreatBigTick;
			other.lastFireTicks.Clear();
			foreach (KeyValuePair<IncidentDef, int> keyValuePair in this.lastFireTicks)
			{
				other.lastFireTicks.Add(keyValuePair.Key, keyValuePair.Value);
			}
			other.RecentRandomQuests.Clear();
			other.RecentRandomQuests.AddRange(this.RecentRandomQuests);
			other.RecentRandomDecrees.Clear();
			other.RecentRandomDecrees.AddRange(this.RecentRandomDecrees);
			other.RecentRandomIncidents.Clear();
			other.RecentRandomIncidents.AddRange(this.RecentRandomIncidents);
			other.lastRoyalFavorQuestTick = this.lastRoyalFavorQuestTick;
			other.colonistCountTicks.Clear();
			foreach (KeyValuePair<int, int> keyValuePair2 in this.colonistCountTicks)
			{
				other.colonistCountTicks.Add(keyValuePair2.Key, keyValuePair2.Value);
			}
			other.lastRaidFaction = this.lastRaidFaction;
		}

		// Token: 0x04007753 RID: 30547
		private IIncidentTarget target;

		// Token: 0x04007754 RID: 30548
		private int lastThreatBigTick = -1;

		// Token: 0x04007755 RID: 30549
		private Dictionary<int, int> colonistCountTicks = new Dictionary<int, int>();

		// Token: 0x04007756 RID: 30550
		public Dictionary<IncidentDef, int> lastFireTicks = new Dictionary<IncidentDef, int>();

		// Token: 0x04007757 RID: 30551
		public Faction lastRaidFaction;

		// Token: 0x04007758 RID: 30552
		private int lastRoyalFavorQuestTick = -1;

		// Token: 0x04007759 RID: 30553
		private List<QuestScriptDef> recentRandomQuests = new List<QuestScriptDef>();

		// Token: 0x0400775A RID: 30554
		private List<QuestScriptDef> recentRandomDecrees = new List<QuestScriptDef>();

		// Token: 0x0400775B RID: 30555
		private List<IncidentDef> recentRandomIncidents = new List<IncidentDef>();

		// Token: 0x0400775C RID: 30556
		private const int RecentRandomQuestsMaxStorage = 5;
	}
}
