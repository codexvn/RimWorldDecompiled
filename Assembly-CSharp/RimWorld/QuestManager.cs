using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001B7C RID: 7036
	public class QuestManager : IExposable
	{
		// Token: 0x17001828 RID: 6184
		// (get) Token: 0x06009D0C RID: 40204 RVA: 0x002D8A7B File Offset: 0x002D6C7B
		public List<Quest> QuestsListForReading
		{
			get
			{
				return this.allQuests;
			}
		}

		// Token: 0x17001829 RID: 6185
		// (get) Token: 0x06009D0D RID: 40205 RVA: 0x002D8A83 File Offset: 0x002D6C83
		public List<Quest> ActiveQuestsListForReading
		{
			get
			{
				return this.activeQuests;
			}
		}

		// Token: 0x1700182A RID: 6186
		// (get) Token: 0x06009D0E RID: 40206 RVA: 0x002D8A8B File Offset: 0x002D6C8B
		public List<QuestPart_SituationalThought> SituationalThoughtQuestParts
		{
			get
			{
				return this.cachedSituationalThoughtQuestParts;
			}
		}

		// Token: 0x1700182B RID: 6187
		// (get) Token: 0x06009D0F RID: 40207 RVA: 0x002D8A93 File Offset: 0x002D6C93
		public List<QuestPart_ExtraFaction> ExtraFactionQuestParts
		{
			get
			{
				return this.cachedExtraFactionQuestParts;
			}
		}

		// Token: 0x06009D10 RID: 40208 RVA: 0x002D8A9C File Offset: 0x002D6C9C
		public void Add(Quest quest)
		{
			if (quest == null)
			{
				Log.Error("Tried to add a null quest.");
				return;
			}
			if (this.Contains(quest))
			{
				Log.Error("Tried to add the same quest twice: " + quest.ToStringSafe<Quest>());
				return;
			}
			this.activeQuests.Add(quest);
			this.allQuests.Add(quest);
			this.AddToCache(quest);
			Find.SignalManager.RegisterReceiver(quest);
			List<QuestPart> partsListForReading = quest.PartsListForReading;
			for (int i = 0; i < partsListForReading.Count; i++)
			{
				partsListForReading[i].PostQuestAdded();
			}
			quest.PostAdded();
			if (quest.initiallyAccepted)
			{
				quest.Initiate();
			}
		}

		// Token: 0x06009D11 RID: 40209 RVA: 0x002D8B38 File Offset: 0x002D6D38
		public void Remove(Quest quest)
		{
			if (!this.Contains(quest))
			{
				Log.Error("Tried to remove non-existent quest: " + quest.ToStringSafe<Quest>());
				return;
			}
			this.allQuests.Remove(quest);
			this.activeQuests.Remove(quest);
			this.historicalQuests.Remove(quest);
			this.RemoveFromCache(quest);
			Find.SignalManager.DeregisterReceiver(quest);
		}

		// Token: 0x06009D12 RID: 40210 RVA: 0x002D8B9D File Offset: 0x002D6D9D
		public bool Contains(Quest quest)
		{
			return this.allQuests.Contains(quest);
		}

		// Token: 0x06009D13 RID: 40211 RVA: 0x002D8BAC File Offset: 0x002D6DAC
		public void QuestManagerTick()
		{
			for (int i = 0; i < this.historicalQuests.Count; i++)
			{
				this.historicalQuests[i].QuestTick();
			}
			for (int j = 0; j < this.activeQuests.Count; j++)
			{
				this.activeQuests[j].QuestTick();
				if (this.activeQuests[j].Historical)
				{
					this.historicalQuests.Add(this.activeQuests[j]);
					this.activeQuests[j] = null;
				}
			}
			this.activeQuests.RemoveAll((Quest quest) => quest == null);
		}

		// Token: 0x06009D14 RID: 40212 RVA: 0x002D8C6C File Offset: 0x002D6E6C
		public bool IsReservedByAnyQuest(Pawn p)
		{
			int count = this.activeQuests.Count;
			for (int i = 0; i < count; i++)
			{
				if (this.activeQuests[i] != null && this.activeQuests[i].QuestReserves(p))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06009D15 RID: 40213 RVA: 0x002D8CB8 File Offset: 0x002D6EB8
		public bool IsReservedByAnyQuest(Faction f)
		{
			for (int i = 0; i < this.activeQuests.Count; i++)
			{
				if (this.activeQuests[i] != null && this.activeQuests[i].QuestReserves(f))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06009D16 RID: 40214 RVA: 0x002D8D00 File Offset: 0x002D6F00
		public bool IsReservedByAnyQuest(TransportShip ship)
		{
			for (int i = 0; i < this.activeQuests.Count; i++)
			{
				if (this.activeQuests[i] != null && this.activeQuests[i].QuestReserves(ship))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06009D17 RID: 40215 RVA: 0x002D8D48 File Offset: 0x002D6F48
		private void AddToCache(Quest quest)
		{
			this.questsInDisplayOrder.Add(quest);
			this.questsInDisplayOrder.SortBy((Quest x) => x.TicksSinceAppeared);
			for (int i = 0; i < quest.PartsListForReading.Count; i++)
			{
				QuestPart_SituationalThought questPart_SituationalThought = quest.PartsListForReading[i] as QuestPart_SituationalThought;
				if (questPart_SituationalThought != null)
				{
					this.cachedSituationalThoughtQuestParts.Add(questPart_SituationalThought);
				}
				QuestPart_ExtraFaction questPart_ExtraFaction = quest.PartsListForReading[i] as QuestPart_ExtraFaction;
				if (questPart_ExtraFaction != null)
				{
					this.cachedExtraFactionQuestParts.Add(questPart_ExtraFaction);
				}
			}
		}

		// Token: 0x06009D18 RID: 40216 RVA: 0x002D8DE4 File Offset: 0x002D6FE4
		private void RemoveFromCache(Quest quest)
		{
			this.questsInDisplayOrder.Remove(quest);
			for (int i = 0; i < quest.PartsListForReading.Count; i++)
			{
				QuestPart_SituationalThought questPart_SituationalThought = quest.PartsListForReading[i] as QuestPart_SituationalThought;
				if (questPart_SituationalThought != null)
				{
					this.cachedSituationalThoughtQuestParts.Remove(questPart_SituationalThought);
				}
				QuestPart_ExtraFaction questPart_ExtraFaction = quest.PartsListForReading[i] as QuestPart_ExtraFaction;
				if (questPart_ExtraFaction != null)
				{
					this.cachedExtraFactionQuestParts.Remove(questPart_ExtraFaction);
				}
			}
		}

		// Token: 0x06009D19 RID: 40217 RVA: 0x002D8E58 File Offset: 0x002D7058
		public void Notify_PawnDiscarded(Pawn pawn)
		{
			for (int i = 0; i < this.allQuests.Count; i++)
			{
				this.allQuests[i].Notify_PawnDiscarded(pawn);
			}
		}

		// Token: 0x06009D1A RID: 40218 RVA: 0x002D8E90 File Offset: 0x002D7090
		public void ExposeData()
		{
			Scribe_Collections.Look<Quest>(ref this.allQuests, "quests", LookMode.Deep, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				int num = this.allQuests.RemoveAll((Quest x) => x == null);
				if (num != 0)
				{
					Log.Error(num.ToString() + " quest(s) were null after loading.");
				}
				int num2 = this.allQuests.RemoveAll((Quest q) => q.root == null);
				if (num2 != 0)
				{
					Log.Error(num2.ToString() + " quest(s) had null roots after loading.");
				}
				this.cachedExtraFactionQuestParts.Clear();
				this.cachedSituationalThoughtQuestParts.Clear();
				this.questsInDisplayOrder.Clear();
				for (int i = 0; i < this.allQuests.Count; i++)
				{
					this.AddToCache(this.allQuests[i]);
				}
			}
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				for (int j = 0; j < this.allQuests.Count; j++)
				{
					Find.SignalManager.RegisterReceiver(this.allQuests[j]);
				}
				this.activeQuests.Clear();
				this.historicalQuests.Clear();
				for (int k = 0; k < this.allQuests.Count; k++)
				{
					if (this.allQuests[k].Historical)
					{
						this.historicalQuests.Add(this.allQuests[k]);
					}
					else
					{
						this.activeQuests.Add(this.allQuests[k]);
					}
				}
			}
			BackCompatibility.PostExposeData(this);
		}

		// Token: 0x06009D1B RID: 40219 RVA: 0x002D9044 File Offset: 0x002D7244
		public void Notify_ThingsProduced(Pawn worker, List<Thing> things)
		{
			for (int i = 0; i < this.activeQuests.Count; i++)
			{
				if (this.activeQuests[i].State == QuestState.Ongoing)
				{
					this.activeQuests[i].Notify_ThingsProduced(worker, things);
				}
			}
		}

		// Token: 0x06009D1C RID: 40220 RVA: 0x002D9090 File Offset: 0x002D7290
		public void Notify_PlantHarvested(Pawn worker, Thing harvested)
		{
			for (int i = 0; i < this.activeQuests.Count; i++)
			{
				if (this.activeQuests[i].State == QuestState.Ongoing)
				{
					this.activeQuests[i].Notify_PlantHarvested(worker, harvested);
				}
			}
		}

		// Token: 0x06009D1D RID: 40221 RVA: 0x002D90DC File Offset: 0x002D72DC
		public void Notify_PawnKilled(Pawn pawn, DamageInfo? dinfo)
		{
			for (int i = 0; i < this.activeQuests.Count; i++)
			{
				if (this.activeQuests[i].State == QuestState.Ongoing)
				{
					this.activeQuests[i].Notify_PawnKilled(pawn, dinfo);
				}
			}
		}

		// Token: 0x06009D1E RID: 40222 RVA: 0x002D9128 File Offset: 0x002D7328
		public void Notify_PawnBorn(Thing baby, Thing birther, Pawn mother, Pawn father)
		{
			for (int i = 0; i < this.activeQuests.Count; i++)
			{
				if (this.activeQuests[i].State == QuestState.Ongoing)
				{
					this.activeQuests[i].Notify_PawnBorn(baby, birther, mother, father);
				}
			}
		}

		// Token: 0x06009D1F RID: 40223 RVA: 0x002D9178 File Offset: 0x002D7378
		public void Notify_FactionRemoved(Faction faction)
		{
			for (int i = 0; i < this.allQuests.Count; i++)
			{
				this.allQuests[i].Notify_FactionRemoved(faction);
			}
		}

		// Token: 0x04006CEB RID: 27883
		private List<Quest> historicalQuests = new List<Quest>();

		// Token: 0x04006CEC RID: 27884
		private List<Quest> activeQuests = new List<Quest>();

		// Token: 0x04006CED RID: 27885
		private List<Quest> allQuests = new List<Quest>();

		// Token: 0x04006CEE RID: 27886
		public List<Quest> questsInDisplayOrder = new List<Quest>();

		// Token: 0x04006CEF RID: 27887
		private List<QuestPart_SituationalThought> cachedSituationalThoughtQuestParts = new List<QuestPart_SituationalThought>();

		// Token: 0x04006CF0 RID: 27888
		private List<QuestPart_ExtraFaction> cachedExtraFactionQuestParts = new List<QuestPart_ExtraFaction>();
	}
}
