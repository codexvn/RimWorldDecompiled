using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld.QuestGen;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020024D4 RID: 9428
	[StaticConstructorOnStartup]
	public class Pawn_RoyaltyTracker : IExposable
	{
		// Token: 0x17001F87 RID: 8071
		// (get) Token: 0x0600CDA4 RID: 52644 RVA: 0x003C5114 File Offset: 0x003C3314
		public List<RoyalTitle> AllTitlesForReading
		{
			get
			{
				return this.titles;
			}
		}

		// Token: 0x17001F88 RID: 8072
		// (get) Token: 0x0600CDA5 RID: 52645 RVA: 0x003C511C File Offset: 0x003C331C
		public List<RoyalTitle> AllTitlesInEffectForReading
		{
			get
			{
				if (!this.pawn.IsWildMan() && (!this.pawn.IsMutant || !this.pawn.mutant.Def.disableTitles))
				{
					return this.titles;
				}
				return Pawn_RoyaltyTracker.EmptyTitles;
			}
		}

		// Token: 0x17001F89 RID: 8073
		// (get) Token: 0x0600CDA6 RID: 52646 RVA: 0x003C515C File Offset: 0x003C335C
		public RoyalTitle MostSeniorTitle
		{
			get
			{
				List<RoyalTitle> allTitlesInEffectForReading = this.AllTitlesInEffectForReading;
				int num = -1;
				RoyalTitle royalTitle = null;
				for (int i = 0; i < allTitlesInEffectForReading.Count; i++)
				{
					if (allTitlesInEffectForReading[i].def.seniority > num)
					{
						num = allTitlesInEffectForReading[i].def.seniority;
						royalTitle = allTitlesInEffectForReading[i];
					}
				}
				return royalTitle ?? null;
			}
		}

		// Token: 0x17001F8A RID: 8074
		// (get) Token: 0x0600CDA7 RID: 52647 RVA: 0x003C51B9 File Offset: 0x003C33B9
		public List<Ability> AllAbilitiesForReading
		{
			get
			{
				return this.abilities;
			}
		}

		// Token: 0x17001F8B RID: 8075
		// (get) Token: 0x0600CDA8 RID: 52648 RVA: 0x003C51C1 File Offset: 0x003C33C1
		public IEnumerable<QuestScriptDef> PossibleDecreeQuests
		{
			get
			{
				this.tmpDecreeTags.Clear();
				List<RoyalTitle> allTitlesInEffectForReading = this.AllTitlesInEffectForReading;
				for (int i = 0; i < allTitlesInEffectForReading.Count; i++)
				{
					if (allTitlesInEffectForReading[i].def.decreeTags != null)
					{
						this.tmpDecreeTags.AddRange(allTitlesInEffectForReading[i].def.decreeTags);
					}
				}
				foreach (QuestScriptDef questScriptDef in DefDatabase<QuestScriptDef>.AllDefs)
				{
					if (questScriptDef.decreeTags != null && questScriptDef.decreeTags.Any((string x) => this.tmpDecreeTags.Contains(x)))
					{
						Slate slate = new Slate();
						Slate slate2 = slate;
						string name = "points";
						IIncidentTarget mapHeld = this.pawn.MapHeld;
						slate2.Set<float>(name, StorytellerUtility.DefaultThreatPointsNow(mapHeld ?? Find.World), false);
						slate.Set<Pawn>("asker", this.pawn, false);
						QuestScriptDef questScriptDef2 = questScriptDef;
						Slate slate3 = slate;
						mapHeld = this.pawn.MapHeld;
						if (questScriptDef2.CanRun(slate3, mapHeld ?? Find.World))
						{
							yield return questScriptDef;
						}
					}
				}
				IEnumerator<QuestScriptDef> enumerator = null;
				yield break;
				yield break;
			}
		}

		// Token: 0x17001F8C RID: 8076
		// (get) Token: 0x0600CDA9 RID: 52649 RVA: 0x003C51D4 File Offset: 0x003C33D4
		public bool PermitPointsAvailable
		{
			get
			{
				foreach (Faction faction in Find.FactionManager.AllFactionsVisible)
				{
					if (this.GetPermitPoints(faction) > 0)
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x17001F8D RID: 8077
		// (get) Token: 0x0600CDAA RID: 52650 RVA: 0x003C5230 File Offset: 0x003C3430
		public List<FactionPermit> AllFactionPermits
		{
			get
			{
				return this.factionPermits;
			}
		}

		// Token: 0x0600CDAB RID: 52651 RVA: 0x003C5238 File Offset: 0x003C3438
		public Pawn_RoyaltyTracker(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x0600CDAC RID: 52652 RVA: 0x003C52C4 File Offset: 0x003C34C4
		private int FindFactionTitleIndex(Faction faction, bool createIfNotExisting = false)
		{
			for (int i = 0; i < this.titles.Count; i++)
			{
				if (this.titles[i].faction == faction)
				{
					return i;
				}
			}
			if (createIfNotExisting)
			{
				this.titles.Add(new RoyalTitle
				{
					faction = faction,
					receivedTick = GenTicks.TicksGame,
					conceited = RoyalTitleUtility.ShouldBecomeConceitedOnNewTitle(this.pawn),
					pawn = this.pawn
				});
				return this.titles.Count - 1;
			}
			return -1;
		}

		// Token: 0x0600CDAD RID: 52653 RVA: 0x003C5350 File Offset: 0x003C3550
		public bool HasAnyTitleIn(Faction faction)
		{
			using (List<RoyalTitle>.Enumerator enumerator = this.titles.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.faction == faction)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x0600CDAE RID: 52654 RVA: 0x003C53AC File Offset: 0x003C35AC
		public bool HasTitle(RoyalTitleDef title)
		{
			using (List<RoyalTitle>.Enumerator enumerator = this.titles.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.def == title)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x0600CDAF RID: 52655 RVA: 0x003C5408 File Offset: 0x003C3608
		public List<FactionPermit> PermitsFromFaction(Faction faction)
		{
			Pawn_RoyaltyTracker.tmpPermits.Clear();
			foreach (FactionPermit factionPermit in this.factionPermits)
			{
				if (factionPermit.Faction == faction)
				{
					Pawn_RoyaltyTracker.tmpPermits.Add(factionPermit);
				}
			}
			return Pawn_RoyaltyTracker.tmpPermits;
		}

		// Token: 0x0600CDB0 RID: 52656 RVA: 0x003C5478 File Offset: 0x003C3678
		public void AddPermit(RoyalTitlePermitDef permit, Faction faction)
		{
			if (!ModLister.CheckRoyalty("Permit"))
			{
				return;
			}
			if (this.HasPermit(permit, faction))
			{
				return;
			}
			if (permit.prerequisite != null && this.HasPermit(permit.prerequisite, faction))
			{
				FactionPermit item = this.factionPermits.Find((FactionPermit x) => x.Permit == permit.prerequisite && x.Faction == faction);
				this.factionPermits.Remove(item);
			}
			this.factionPermits.Add(new FactionPermit(faction, this.GetCurrentTitle(faction), permit));
		}

		// Token: 0x0600CDB1 RID: 52657 RVA: 0x003C5530 File Offset: 0x003C3730
		public void RefundPermits(int favorCost, Faction faction)
		{
			if (this.favor[faction] < favorCost)
			{
				Log.Error("Not enough favor to refund permits.");
				return;
			}
			bool flag = false;
			for (int i = this.factionPermits.Count - 1; i >= 0; i--)
			{
				if (this.factionPermits[i].Faction == faction)
				{
					this.factionPermits.RemoveAt(i);
					flag = true;
				}
			}
			if (flag)
			{
				int num = this.GetFavor(faction);
				Dictionary<Faction, int> dictionary = this.favor;
				dictionary[faction] -= favorCost;
				this.OnFavorChanged(faction, num, num - favorCost);
			}
		}

		// Token: 0x0600CDB2 RID: 52658 RVA: 0x003C55C8 File Offset: 0x003C37C8
		public bool HasPermit(RoyalTitlePermitDef permit, Faction faction)
		{
			foreach (FactionPermit factionPermit in this.factionPermits)
			{
				if (factionPermit.Permit == permit && factionPermit.Faction == faction)
				{
					return true;
				}
			}
			RoyalTitleDef currentTitle = this.GetCurrentTitle(faction);
			return currentTitle != null && currentTitle.permits != null && currentTitle.permits.Contains(permit);
		}

		// Token: 0x17001F8E RID: 8078
		// (get) Token: 0x0600CDB3 RID: 52659 RVA: 0x003C5650 File Offset: 0x003C3850
		public bool HasAidPermit
		{
			get
			{
				return this.factionPermits.Any<FactionPermit>();
			}
		}

		// Token: 0x0600CDB4 RID: 52660 RVA: 0x003C5660 File Offset: 0x003C3860
		public RoyalTitleDef MainTitle()
		{
			if (this.titles.Count == 0)
			{
				return null;
			}
			RoyalTitleDef royalTitleDef = null;
			foreach (RoyalTitle royalTitle in this.titles)
			{
				if (royalTitleDef == null || royalTitle.def.seniority > royalTitleDef.seniority)
				{
					royalTitleDef = royalTitle.def;
				}
			}
			return royalTitleDef;
		}

		// Token: 0x0600CDB5 RID: 52661 RVA: 0x003C56DC File Offset: 0x003C38DC
		public int GetFavor(Faction faction)
		{
			int result;
			if (!this.favor.TryGetValue(faction, out result))
			{
				return 0;
			}
			return result;
		}

		// Token: 0x0600CDB6 RID: 52662 RVA: 0x003C56FC File Offset: 0x003C38FC
		public FactionPermit GetPermit(RoyalTitlePermitDef permit, Faction faction)
		{
			foreach (FactionPermit factionPermit in this.factionPermits)
			{
				if (factionPermit.Permit == permit && factionPermit.Faction == faction)
				{
					return factionPermit;
				}
			}
			return null;
		}

		// Token: 0x0600CDB7 RID: 52663 RVA: 0x003C5764 File Offset: 0x003C3964
		public int GetPermitPoints(Faction faction)
		{
			int num = 0;
			RoyalTitleDef royalTitleDef = this.GetCurrentTitle(faction);
			int num2 = 200;
			while (royalTitleDef != null)
			{
				num += royalTitleDef.permitPointsAwarded;
				royalTitleDef = royalTitleDef.GetPreviousTitle_IncludeNonRewardable(faction);
				num2--;
				if (num2 <= 0)
				{
					Log.ErrorOnce("GetPermitPoints exceeded iterations limit.", 1837503);
					break;
				}
			}
			for (int i = 0; i < this.factionPermits.Count; i++)
			{
				if (this.factionPermits[i].Faction == faction)
				{
					RoyalTitlePermitDef royalTitlePermitDef = this.factionPermits[i].Permit;
					num2 = 200;
					while (royalTitlePermitDef != null)
					{
						num -= royalTitlePermitDef.permitPointCost;
						royalTitlePermitDef = royalTitlePermitDef.prerequisite;
						num2--;
						if (num2 <= 0)
						{
							Log.ErrorOnce("GetPermitPoints exceeded iterations limit.", 1837503);
							break;
						}
					}
				}
			}
			return num;
		}

		// Token: 0x0600CDB8 RID: 52664 RVA: 0x003C5828 File Offset: 0x003C3A28
		public void GainFavor(Faction faction, int amount)
		{
			if (!ModLister.CheckRoyalty("Honor"))
			{
				return;
			}
			int oldAmount = this.GetFavor(faction);
			int num;
			if (!this.favor.TryGetValue(faction, out num))
			{
				num = 0;
				this.favor.Add(faction, 0);
			}
			num += amount;
			this.favor[faction] = num;
			if (amount < 0)
			{
				this.TryUpdateTitle(faction, true, null);
			}
			this.OnFavorChanged(faction, oldAmount, num);
		}

		// Token: 0x0600CDB9 RID: 52665 RVA: 0x003C5894 File Offset: 0x003C3A94
		public RoyalTitleDef GetTitleAwardedWhenUpdating(Faction faction, int favor)
		{
			RoyalTitleDef royalTitleDef = this.GetCurrentTitle(faction);
			RoyalTitleDef result = null;
			while (favor > 0 && royalTitleDef.GetNextTitle(faction) != null)
			{
				if (royalTitleDef == null)
				{
					royalTitleDef = faction.def.RoyalTitlesAwardableInSeniorityOrderForReading.First<RoyalTitleDef>();
				}
				else
				{
					royalTitleDef = royalTitleDef.GetNextTitle(faction);
				}
				favor -= royalTitleDef.favorCost;
				if (favor >= 0)
				{
					result = royalTitleDef;
				}
			}
			return result;
		}

		// Token: 0x0600CDBA RID: 52666 RVA: 0x003C58EC File Offset: 0x003C3AEC
		public bool CanUpdateTitleOfAnyFaction(out Faction faction)
		{
			foreach (Faction faction2 in Find.FactionManager.AllFactions)
			{
				if (this.CanUpdateTitle(faction2))
				{
					faction = faction2;
					return true;
				}
			}
			faction = null;
			return false;
		}

		// Token: 0x0600CDBB RID: 52667 RVA: 0x003C594C File Offset: 0x003C3B4C
		public bool CanUpdateTitle(Faction faction)
		{
			RoyalTitleDef currentTitle = this.GetCurrentTitle(faction);
			int num = this.GetFavor(faction);
			RoyalTitleDef nextTitle = currentTitle.GetNextTitle(faction);
			return nextTitle != null && num >= nextTitle.favorCost;
		}

		// Token: 0x0600CDBC RID: 52668 RVA: 0x003C5980 File Offset: 0x003C3B80
		public bool TryUpdateTitle(Faction faction, bool sendLetter = true, RoyalTitleDef updateTo = null)
		{
			RoyalTitleDef currentTitle = this.GetCurrentTitle(faction);
			this.UpdateRoyalTitle(faction, sendLetter, updateTo);
			RoyalTitleDef currentTitle2 = this.GetCurrentTitle(faction);
			if (currentTitle2 != currentTitle)
			{
				this.ApplyRewardsForTitle(faction, currentTitle, currentTitle2, false);
				this.OnPostTitleChanged(faction, currentTitle, currentTitle2);
			}
			return currentTitle2 != currentTitle;
		}

		// Token: 0x0600CDBD RID: 52669 RVA: 0x003C59C8 File Offset: 0x003C3BC8
		public bool TryRemoveFavor(Faction faction, int amount)
		{
			int num = this.GetFavor(faction);
			if (num < amount)
			{
				return false;
			}
			this.SetFavor(faction, num - amount, true);
			return true;
		}

		// Token: 0x0600CDBE RID: 52670 RVA: 0x003C59F0 File Offset: 0x003C3BF0
		public void SetFavor(Faction faction, int amount, bool notifyOnFavorChanged = true)
		{
			if (!ModLister.CheckRoyalty("Honor"))
			{
				return;
			}
			int oldAmount = this.GetFavor(faction);
			if (amount == 0 && this.favor.ContainsKey(faction) && this.FindFactionTitleIndex(faction, false) == -1)
			{
				this.favor.Remove(faction);
			}
			else
			{
				this.favor.SetOrAdd(faction, amount);
			}
			if (notifyOnFavorChanged)
			{
				this.OnFavorChanged(faction, oldAmount, amount);
			}
		}

		// Token: 0x0600CDBF RID: 52671 RVA: 0x003C5A58 File Offset: 0x003C3C58
		private void OnFavorChanged(Faction faction, int oldAmount, int newAmount)
		{
			RoyalTitleDef titleAwardedWhenUpdating = this.GetTitleAwardedWhenUpdating(faction, oldAmount);
			RoyalTitleDef titleAwardedWhenUpdating2 = this.GetTitleAwardedWhenUpdating(faction, newAmount);
			RoyalTitle currentTitleInFaction = this.GetCurrentTitleInFaction(faction);
			RoyalTitleDef royalTitleDef = null;
			if (currentTitleInFaction != null && titleAwardedWhenUpdating != null)
			{
				royalTitleDef = ((currentTitleInFaction.def.seniority >= titleAwardedWhenUpdating.seniority) ? currentTitleInFaction.def : titleAwardedWhenUpdating);
			}
			else if (currentTitleInFaction != null)
			{
				royalTitleDef = currentTitleInFaction.def;
			}
			else if (titleAwardedWhenUpdating != null)
			{
				royalTitleDef = titleAwardedWhenUpdating;
			}
			if (royalTitleDef != titleAwardedWhenUpdating2)
			{
				List<RoyalTitleDef> list = new List<RoyalTitleDef>();
				int previousTitleSeniority = (royalTitleDef == null) ? -1 : royalTitleDef.seniority;
				int newAwardedSeniority = (titleAwardedWhenUpdating2 == null) ? -1 : titleAwardedWhenUpdating2.seniority;
				if (previousTitleSeniority < newAwardedSeniority)
				{
					list.AddRange(from t in faction.def.RoyalTitlesAllInSeniorityOrderForReading
					where t.seniority > previousTitleSeniority && t.seniority <= newAwardedSeniority
					select t);
				}
				else
				{
					list.Add(titleAwardedWhenUpdating2);
				}
				foreach (RoyalTitleDef royalTitleDef2 in list)
				{
					if (royalTitleDef2 != null && this.pawn.Faction != null && this.pawn.Faction.IsPlayer)
					{
						royalTitleDef2.AwardWorker.OnPreAward(this.pawn, faction, royalTitleDef, royalTitleDef2);
					}
					QuestUtility.SendQuestTargetSignals(this.pawn.questTags, "TitleAwardedWhenUpdatingChanged");
					if (royalTitleDef2 != null && this.pawn.Faction != null && this.pawn.Faction.IsPlayer)
					{
						royalTitleDef2.AwardWorker.DoAward(this.pawn, faction, royalTitleDef, royalTitleDef2);
					}
					royalTitleDef = royalTitleDef2;
				}
				if (previousTitleSeniority < newAwardedSeniority)
				{
					RoyalTitleUtility.EndExistingBestowingCeremonyQuest(this.pawn, faction);
					RoyalTitleUtility.GenerateBestowingCeremonyQuest(this.pawn, faction);
				}
			}
		}

		// Token: 0x0600CDC0 RID: 52672 RVA: 0x003C5C28 File Offset: 0x003C3E28
		public RoyalTitleDef GetCurrentTitle(Faction faction)
		{
			RoyalTitle currentTitleInFaction = this.GetCurrentTitleInFaction(faction);
			if (!this.AllTitlesInEffectForReading.Contains(currentTitleInFaction))
			{
				return null;
			}
			if (currentTitleInFaction == null)
			{
				return null;
			}
			return currentTitleInFaction.def;
		}

		// Token: 0x0600CDC1 RID: 52673 RVA: 0x003C5C58 File Offset: 0x003C3E58
		public RoyalTitle GetCurrentTitleInFaction(Faction faction)
		{
			if (faction == null)
			{
				Log.Error("Cannot get current title for null faction.");
			}
			int num = this.FindFactionTitleIndex(faction, false);
			if (num == -1)
			{
				return null;
			}
			return this.titles[num];
		}

		// Token: 0x0600CDC2 RID: 52674 RVA: 0x003C5C90 File Offset: 0x003C3E90
		public void SetTitle(Faction faction, RoyalTitleDef title, bool grantRewards, bool rewardsOnlyForNewestTitle = false, bool sendLetter = true)
		{
			if (!ModLister.CheckRoyalty("Honor"))
			{
				return;
			}
			RoyalTitleDef currentTitle = this.GetCurrentTitle(faction);
			this.OnPreTitleChanged(faction, currentTitle, title, sendLetter);
			if (grantRewards)
			{
				this.ApplyRewardsForTitle(faction, currentTitle, title, rewardsOnlyForNewestTitle);
			}
			int index = this.FindFactionTitleIndex(faction, true);
			if (title != null)
			{
				this.titles[index].def = title;
				this.titles[index].receivedTick = GenTicks.TicksGame;
			}
			else
			{
				this.titles.RemoveAt(index);
			}
			this.SetFavor(faction, 0, true);
			this.OnPostTitleChanged(faction, currentTitle, title);
		}

		// Token: 0x0600CDC3 RID: 52675 RVA: 0x003C5D20 File Offset: 0x003C3F20
		public void ReduceTitle(Faction faction)
		{
			RoyalTitleDef currentTitle = this.GetCurrentTitle(faction);
			if (currentTitle == null || !currentTitle.Awardable)
			{
				return;
			}
			for (int i = this.factionPermits.Count - 1; i >= 0; i--)
			{
				if (this.factionPermits[i].Title == currentTitle)
				{
					this.factionPermits.RemoveAt(i);
				}
			}
			RoyalTitleDef previousTitle = currentTitle.GetPreviousTitle(faction);
			this.OnPreTitleChanged(faction, currentTitle, previousTitle, true);
			this.CleanupThoughts(currentTitle);
			this.CleanupThoughts(previousTitle);
			if (currentTitle.awardThought != null && this.pawn.needs.mood != null)
			{
				Thought_MemoryRoyalTitle thought_MemoryRoyalTitle = (Thought_MemoryRoyalTitle)ThoughtMaker.MakeThought(currentTitle.lostThought);
				thought_MemoryRoyalTitle.titleDef = currentTitle;
				this.pawn.needs.mood.thoughts.memories.TryGainMemory(thought_MemoryRoyalTitle, null);
			}
			int index = this.FindFactionTitleIndex(faction, false);
			if (previousTitle == null)
			{
				this.titles.RemoveAt(index);
			}
			else
			{
				this.titles[index].def = previousTitle;
			}
			this.SetFavor(faction, 0, true);
			this.OnPostTitleChanged(faction, currentTitle, previousTitle);
		}

		// Token: 0x0600CDC4 RID: 52676 RVA: 0x003C5E2D File Offset: 0x003C402D
		public Pawn GetHeir(Faction faction)
		{
			if (this.heirs != null && this.heirs.ContainsKey(faction))
			{
				return this.heirs[faction];
			}
			return null;
		}

		// Token: 0x0600CDC5 RID: 52677 RVA: 0x003C5E53 File Offset: 0x003C4053
		public void SetHeir(Pawn heir, Faction faction)
		{
			if (this.heirs != null)
			{
				this.heirs[faction] = heir;
			}
		}

		// Token: 0x0600CDC6 RID: 52678 RVA: 0x003C5E6C File Offset: 0x003C406C
		public void AssignHeirIfNone(RoyalTitleDef t, Faction faction)
		{
			if (!this.heirs.ContainsKey(faction) && t.Awardable && (Faction.OfEmpire == null || this.pawn.HomeFaction != Faction.OfEmpire))
			{
				this.SetHeir(t.GetInheritanceWorker(faction).FindHeir(faction, this.pawn, t), faction);
			}
		}

		// Token: 0x0600CDC7 RID: 52679 RVA: 0x003C5EC4 File Offset: 0x003C40C4
		public void RoyaltyTrackerTickInterval(int delta)
		{
			Faction faction;
			if (this.pawn.IsHashIntervalTick(37500, delta) && RoyalTitleUtility.ShouldGetBestowingCeremonyQuest(this.pawn, out faction))
			{
				RoyalTitleUtility.GenerateBestowingCeremonyQuest(this.pawn, faction);
			}
			List<RoyalTitle> allTitlesInEffectForReading = this.AllTitlesInEffectForReading;
			for (int i = 0; i < allTitlesInEffectForReading.Count; i++)
			{
				allTitlesInEffectForReading[i].RoyalTitleTick(delta);
			}
			if (!this.pawn.Spawned || this.pawn.RaceProps.Animal)
			{
				return;
			}
			this.factionHeirsToClearTmp.Clear();
			foreach (KeyValuePair<Faction, Pawn> keyValuePair in this.heirs)
			{
				RoyalTitleDef currentTitle = this.GetCurrentTitle(keyValuePair.Key);
				if (currentTitle != null && currentTitle.canBeInherited)
				{
					Pawn value = keyValuePair.Value;
					if (value != null && value.Dead)
					{
						Find.LetterStack.ReceiveLetter("LetterTitleHeirLostLabel".Translate(), "LetterTitleHeirLost".Translate(this.pawn.Named("HOLDER"), value.Named("HEIR"), keyValuePair.Key.Named("FACTION")), LetterDefOf.NegativeEvent, this.pawn, null, null, null, null, 0, true);
						this.factionHeirsToClearTmp.Add(keyValuePair.Key);
					}
				}
			}
			foreach (Faction key in this.factionHeirsToClearTmp)
			{
				this.heirs[key] = null;
			}
			foreach (FactionPermit factionPermit in this.factionPermits)
			{
				if (factionPermit.LastUsedTick > 0 && Find.TickManager.TicksGame == factionPermit.LastUsedTick + factionPermit.Permit.CooldownTicks)
				{
					Messages.Message("MessagePermitCooldownFinished".Translate(this.pawn, factionPermit.Permit.LabelCap), this.pawn, MessageTypeDefOf.PositiveEvent, true);
				}
			}
		}

		// Token: 0x0600CDC8 RID: 52680 RVA: 0x003C6134 File Offset: 0x003C4334
		public void IssueDecree(bool causedByMentalBreak, string mentalBreakReason = null)
		{
			Pawn_RoyaltyTracker.<>c__DisplayClass59_0 CS$<>8__locals1 = new Pawn_RoyaltyTracker.<>c__DisplayClass59_0();
			if (!ModLister.CheckRoyalty("Decree"))
			{
				return;
			}
			Pawn_RoyaltyTracker.<>c__DisplayClass59_0 CS$<>8__locals2 = CS$<>8__locals1;
			IIncidentTarget mapHeld = this.pawn.MapHeld;
			CS$<>8__locals2.target = (mapHeld ?? Find.World);
			QuestScriptDef questScriptDef;
			if (this.PossibleDecreeQuests.TryRandomElementByWeight((QuestScriptDef x) => NaturalRandomQuestChooser.GetNaturalDecreeSelectionWeight(x, CS$<>8__locals1.target.StoryState), out questScriptDef))
			{
				this.lastDecreeTicks = Find.TickManager.TicksGame;
				Slate slate = new Slate();
				slate.Set<float>("points", StorytellerUtility.DefaultThreatPointsNow(CS$<>8__locals1.target), false);
				slate.Set<Pawn>("asker", this.pawn, false);
				Quest quest = QuestUtility.GenerateQuestAndMakeAvailable(questScriptDef, slate);
				CS$<>8__locals1.target.StoryState.RecordDecreeFired(questScriptDef);
				string str;
				if (causedByMentalBreak)
				{
					str = "WildDecree".Translate() + ": " + this.pawn.LabelShortCap;
				}
				else
				{
					str = "LetterLabelRandomDecree".Translate(this.pawn);
				}
				string text;
				if (causedByMentalBreak)
				{
					text = "LetterDecreeMentalBreak".Translate(this.pawn);
				}
				else
				{
					text = "LetterRandomDecree".Translate(this.pawn);
				}
				if (mentalBreakReason != null)
				{
					text = text + "\n\n" + mentalBreakReason;
				}
				text += "\n\n" + "LetterDecree_Quest".Translate(quest.name);
				ChoiceLetter let = LetterMaker.MakeLetter(str, text, IncidentDefOf.GiveQuest_Random.letterDef, LookTargets.Invalid, null, quest, null);
				Find.LetterStack.ReceiveLetter(let, null, 0, true);
			}
		}

		// Token: 0x0600CDC9 RID: 52681 RVA: 0x003C62E8 File Offset: 0x003C44E8
		private void CleanupThoughts(RoyalTitleDef title)
		{
			if (title == null)
			{
				return;
			}
			if (title.awardThought != null && this.pawn.needs != null && this.pawn.needs.mood != null)
			{
				this.pawn.needs.mood.thoughts.memories.RemoveMemoriesOfDef(title.awardThought);
			}
			if (title.lostThought != null && this.pawn.needs != null && this.pawn.needs.mood != null)
			{
				this.pawn.needs.mood.thoughts.memories.RemoveMemoriesOfDef(title.lostThought);
			}
		}

		// Token: 0x0600CDCA RID: 52682 RVA: 0x003C6394 File Offset: 0x003C4594
		public static void MakeLetterTextForTitleChange(Pawn pawn, Faction faction, RoyalTitleDef currentTitle, RoyalTitleDef newTitle, out string headline, out string body)
		{
			if (currentTitle == null || faction.def.RoyalTitlesAwardableInSeniorityOrderForReading.IndexOf(currentTitle) < faction.def.RoyalTitlesAwardableInSeniorityOrderForReading.IndexOf(newTitle))
			{
				headline = "LetterGainedRoyalTitle".Translate(pawn.Named("PAWN"), faction.Named("FACTION"), newTitle.GetLabelCapFor(pawn).Named("TITLE")).Resolve();
			}
			else
			{
				headline = "LetterLostRoyalTitle".Translate(pawn.Named("PAWN"), faction.Named("FACTION"), currentTitle.GetLabelCapFor(pawn).Named("TITLE")).Resolve();
			}
			body = RoyalTitleUtility.BuildDifferenceExplanationText(currentTitle, newTitle, faction, pawn);
			body = body.Resolve().TrimEndNewlines();
		}

		// Token: 0x0600CDCB RID: 52683 RVA: 0x003C6468 File Offset: 0x003C4668
		public static string MakeLetterTextForTitleChange(Pawn pawn, Faction faction, RoyalTitleDef currentTitle, RoyalTitleDef newTitle)
		{
			string str;
			string text;
			Pawn_RoyaltyTracker.MakeLetterTextForTitleChange(pawn, faction, currentTitle, newTitle, out str, out text);
			if (text.Length > 0)
			{
				text = "\n\n" + text;
			}
			return str + text;
		}

		// Token: 0x0600CDCC RID: 52684 RVA: 0x003C64A0 File Offset: 0x003C46A0
		public void ResetPermitsAndPoints(Faction faction, RoyalTitleDef currentTitle)
		{
			if (currentTitle == null)
			{
				return;
			}
			for (int i = faction.def.RoyalTitlesAwardableInSeniorityOrderForReading.IndexOf(currentTitle) + 1 - 1; i >= 0; i--)
			{
				RoyalTitleDef royalTitleDef = faction.def.RoyalTitlesAwardableInSeniorityOrderForReading[i];
				List<FactionPermit> list = this.PermitsFromFaction(faction);
				for (int j = 0; j < list.Count; j++)
				{
					if (list[j].Title == royalTitleDef)
					{
						this.factionPermits.Remove(list[j]);
					}
				}
			}
		}

		// Token: 0x0600CDCD RID: 52685 RVA: 0x003C6520 File Offset: 0x003C4720
		private void OnPreTitleChanged(Faction faction, RoyalTitleDef currentTitle, RoyalTitleDef newTitle, bool sendLetter = true)
		{
			if (newTitle != null)
			{
				this.AssignHeirIfNone(newTitle, faction);
			}
			if (Current.ProgramState == ProgramState.Playing && sendLetter && this.pawn.IsColonist)
			{
				TaggedString label = null;
				if (currentTitle == null || faction.def.RoyalTitlesAwardableInSeniorityOrderForReading.IndexOf(currentTitle) < faction.def.RoyalTitlesAwardableInSeniorityOrderForReading.IndexOf(newTitle))
				{
					label = "LetterLabelGainedRoyalTitle".Translate(this.pawn.Named("PAWN"), newTitle.GetLabelCapFor(this.pawn).Named("TITLE"));
				}
				else
				{
					label = "LetterLabelLostRoyalTitle".Translate(this.pawn.Named("PAWN"), currentTitle.GetLabelCapFor(this.pawn).Named("TITLE"));
				}
				Find.LetterStack.ReceiveLetter(label, Pawn_RoyaltyTracker.MakeLetterTextForTitleChange(this.pawn, faction, currentTitle, newTitle), LetterDefOf.PositiveEvent, this.pawn, null, null, null, null, 0, true);
			}
			if (currentTitle != null)
			{
				for (int i = 0; i < currentTitle.grantedAbilities.Count; i++)
				{
					this.pawn.abilities.RemoveAbility(currentTitle.grantedAbilities[i]);
				}
			}
		}

		// Token: 0x0600CDCE RID: 52686 RVA: 0x003C6654 File Offset: 0x003C4854
		private void OnPostTitleChanged(Faction faction, RoyalTitleDef prevTitle, RoyalTitleDef newTitle)
		{
			this.pawn.Notify_DisabledWorkTypesChanged();
			Pawn_NeedsTracker needs = this.pawn.needs;
			if (needs != null)
			{
				needs.AddOrRemoveNeedsAsAppropriate();
			}
			if (newTitle != null)
			{
				if (newTitle.disabledJoyKinds != null && this.pawn.jobs != null && RoyalTitleUtility.ShouldBecomeConceitedOnNewTitle(this.pawn))
				{
					foreach (JoyKindDef joyKind in newTitle.disabledJoyKinds)
					{
						this.pawn.jobs.Notify_JoyKindDisabled(joyKind);
					}
				}
				this.UpdateAvailableAbilities();
				this.pawn.abilities.Notify_TemporaryAbilitiesChanged();
				this.UpdateHighestTitleAchieved(faction, newTitle);
			}
			QuestUtility.SendQuestTargetSignals(this.pawn.questTags, "TitleChanged", this.pawn.Named("SUBJECT"));
			MeditationFocusTypeAvailabilityCache.ClearFor(this.pawn);
			Pawn_ApparelTracker apparel = this.pawn.apparel;
			if (apparel == null)
			{
				return;
			}
			apparel.Notify_TitleChanged();
		}

		// Token: 0x0600CDCF RID: 52687 RVA: 0x003C6760 File Offset: 0x003C4960
		private void UpdateRoyalTitle(Faction faction, bool sendLetter = true, RoyalTitleDef updateTo = null)
		{
			RoyalTitleDef currentTitle = this.GetCurrentTitle(faction);
			if (currentTitle != null && !currentTitle.Awardable)
			{
				return;
			}
			RoyalTitleDef nextTitle = currentTitle.GetNextTitle(faction);
			if (nextTitle == null)
			{
				return;
			}
			int num = this.GetFavor(faction);
			if (num >= nextTitle.favorCost)
			{
				this.OnPreTitleChanged(faction, currentTitle, nextTitle, sendLetter);
				this.SetFavor(faction, num - nextTitle.favorCost, false);
				int index = this.FindFactionTitleIndex(faction, true);
				this.titles[index].def = nextTitle;
				this.CleanupThoughts(currentTitle);
				this.CleanupThoughts(nextTitle);
				if (nextTitle.awardThought != null && this.pawn.needs != null && this.pawn.needs.mood != null)
				{
					Thought_MemoryRoyalTitle thought_MemoryRoyalTitle = (Thought_MemoryRoyalTitle)ThoughtMaker.MakeThought(nextTitle.awardThought);
					thought_MemoryRoyalTitle.titleDef = nextTitle;
					this.pawn.needs.mood.thoughts.memories.TryGainMemory(thought_MemoryRoyalTitle, null);
				}
				if (nextTitle == updateTo)
				{
					return;
				}
				this.UpdateRoyalTitle(faction, sendLetter, null);
			}
		}

		// Token: 0x0600CDD0 RID: 52688 RVA: 0x003C6858 File Offset: 0x003C4A58
		public List<Thing> ApplyRewardsForTitle(Faction faction, RoyalTitleDef currentTitle, RoyalTitleDef newTitle, bool onlyForNewestTitle = false)
		{
			List<Thing> list = new List<Thing>();
			List<ThingCount> list2 = new List<ThingCount>();
			if (newTitle != null && newTitle.Awardable && this.pawn.IsColonist && this.NewHighestTitle(faction, newTitle))
			{
				int num = ((currentTitle != null) ? faction.def.RoyalTitlesAwardableInSeniorityOrderForReading.IndexOf(currentTitle) : 0) + 1;
				int num2 = faction.def.RoyalTitlesAwardableInSeniorityOrderForReading.IndexOf(newTitle);
				if (onlyForNewestTitle)
				{
					num = num2;
				}
				IntVec3 dropCenter = IntVec3.Invalid;
				Map mapHeld = this.pawn.MapHeld;
				if (mapHeld != null)
				{
					if (mapHeld.IsPlayerHome)
					{
						dropCenter = DropCellFinder.TradeDropSpot(mapHeld);
					}
					else if (!DropCellFinder.TryFindDropSpotNear(this.pawn.Position, mapHeld, out dropCenter, false, false, true, null, true))
					{
						dropCenter = DropCellFinder.RandomDropSpot(mapHeld, true);
					}
				}
				for (int i = num; i <= num2; i++)
				{
					RoyalTitleDef royalTitleDef = faction.def.RoyalTitlesAwardableInSeniorityOrderForReading[i];
					if (royalTitleDef.rewards != null)
					{
						List<Thing> list3 = royalTitleDef.rewards.Select(delegate(ThingDefCountClass r)
						{
							Thing thing = ThingMaker.MakeThing(r.thingDef, null);
							thing.stackCount = r.count;
							return thing;
						}).ToList<Thing>();
						for (int j = 0; j < list3.Count; j++)
						{
							if (list3[j].def == ThingDefOf.PsychicAmplifier)
							{
								Find.History.Notify_PsylinkAvailable();
								break;
							}
						}
						if (this.pawn.Spawned)
						{
							DropPodUtility.DropThingsNear(dropCenter, mapHeld, list3, 110, false, false, false, false, true, null);
						}
						else
						{
							foreach (Thing item in list3)
							{
								this.pawn.inventory.TryAddItemNotForSale(item);
							}
						}
						for (int k = 0; k < list3.Count; k++)
						{
							list2.Add(new ThingCount(list3[k], list3[k].stackCount, false));
						}
						list.AddRange(list3);
					}
				}
				if (list.Count > 0)
				{
					TaggedString text = "LetterRewardsForNewTitle".Translate(this.pawn.Named("PAWN"), faction.Named("FACTION"), newTitle.GetLabelCapFor(this.pawn).Named("TITLE")) + "\n\n" + GenLabel.ThingsLabel(list2, "  - ", true) + "\n\n" + (this.pawn.Spawned ? "LetterRewardsForNewTitleDeliveryBase" : "LetterRewardsForNewTitleDeliveryDirect").Translate(this.pawn.Named("PAWN"));
					Find.LetterStack.ReceiveLetter("LetterLabelRewardsForNewTitle".Translate(), text, LetterDefOf.PositiveEvent, list, null, null, null, null, 0, true);
				}
			}
			return list;
		}

		// Token: 0x0600CDD1 RID: 52689 RVA: 0x003C6B48 File Offset: 0x003C4D48
		private void UpdateHighestTitleAchieved(Faction faction, RoyalTitleDef title)
		{
			if (!this.highestTitles.ContainsKey(faction))
			{
				this.highestTitles.Add(faction, title);
				return;
			}
			if (this.NewHighestTitle(faction, title))
			{
				this.highestTitles[faction] = title;
			}
		}

		// Token: 0x0600CDD2 RID: 52690 RVA: 0x003C6B80 File Offset: 0x003C4D80
		public bool NewHighestTitle(Faction faction, RoyalTitleDef newTitle)
		{
			if (this.highestTitles == null)
			{
				this.highestTitles = new Dictionary<Faction, RoyalTitleDef>();
			}
			if (!this.highestTitles.ContainsKey(faction))
			{
				return true;
			}
			int num = faction.def.RoyalTitlesAwardableInSeniorityOrderForReading.IndexOf(this.highestTitles[faction]);
			return faction.def.RoyalTitlesAwardableInSeniorityOrderForReading.IndexOf(newTitle) > num;
		}

		// Token: 0x0600CDD3 RID: 52691 RVA: 0x003C6BE1 File Offset: 0x003C4DE1
		public IEnumerable<Gizmo> GetGizmos()
		{
			if (this.PermitPointsAvailable && this.pawn.Faction == Faction.OfPlayer)
			{
				Faction faction = null;
				foreach (Faction faction2 in Find.FactionManager.AllFactionsVisibleInViewOrder)
				{
					if (this.GetPermitPoints(faction2) > 0)
					{
						faction = faction2;
						break;
					}
				}
				if (faction == null)
				{
					yield break;
				}
				yield return new Command_Action
				{
					defaultLabel = "ChooseRoyalPermit".Translate(),
					defaultDesc = "ChooseRoyalPermit_Desc".Translate(),
					icon = faction.def.FactionIcon,
					defaultIconColor = faction.Color,
					action = delegate()
					{
						this.OpenPermitWindow();
					},
					Order = -100f
				};
			}
			yield break;
		}

		// Token: 0x0600CDD4 RID: 52692 RVA: 0x003C6BF4 File Offset: 0x003C4DF4
		public void OpenPermitWindow()
		{
			Dialog_InfoCard dialog_InfoCard = new Dialog_InfoCard(this.pawn, null);
			dialog_InfoCard.SetTab(Dialog_InfoCard.InfoCardTab.Permits);
			Find.WindowStack.Add(dialog_InfoCard);
		}

		// Token: 0x0600CDD5 RID: 52693 RVA: 0x003C6C20 File Offset: 0x003C4E20
		public void Notify_PawnKilled()
		{
			if (PawnGenerator.IsBeingGenerated(this.pawn) || this.AllTitlesForReading.Count == 0)
			{
				return;
			}
			if (this.pawn.IsMutant && this.pawn.mutant.Def.disableTitles)
			{
				return;
			}
			bool flag = false;
			StringBuilder stringBuilder = new StringBuilder();
			try
			{
				stringBuilder.AppendLine("LetterTitleInheritance_Base".Translate(this.pawn.Named("PAWN")));
				stringBuilder.AppendLine();
				foreach (RoyalTitle royalTitle in this.AllTitlesForReading)
				{
					if (royalTitle.def.canBeInherited)
					{
						if (this.pawn.IsFreeColonist && !this.pawn.IsQuestLodger())
						{
							flag = true;
						}
						RoyalTitleInheritanceOutcome outcome;
						if (royalTitle.def.TryInherit(this.pawn, royalTitle.faction, out outcome))
						{
							if (outcome.HeirHasTitle && !outcome.heirTitleHigher)
							{
								stringBuilder.AppendLineTagged("LetterTitleInheritance_AsReplacement".Translate(this.pawn.Named("PAWN"), royalTitle.faction.Named("FACTION"), outcome.heir.Named("HEIR"), royalTitle.def.GetLabelFor(this.pawn).Named("TITLE"), outcome.heirCurrentTitle.GetLabelFor(outcome.heir).Named("REPLACEDTITLE")).CapitalizeFirst());
								stringBuilder.AppendLine();
							}
							else if (outcome.heirTitleHigher)
							{
								stringBuilder.AppendLineTagged("LetterTitleInheritance_NoEffectHigherTitle".Translate(this.pawn.Named("PAWN"), royalTitle.faction.Named("FACTION"), outcome.heir.Named("HEIR"), royalTitle.def.GetLabelFor(this.pawn).Named("TITLE"), outcome.heirCurrentTitle.GetLabelFor(outcome.heir).Named("HIGHERTITLE")).CapitalizeFirst());
								stringBuilder.AppendLine();
							}
							else
							{
								stringBuilder.AppendLineTagged("LetterTitleInheritance_WasInherited".Translate(this.pawn.Named("PAWN"), royalTitle.faction.Named("FACTION"), outcome.heir.Named("HEIR"), royalTitle.def.GetLabelFor(this.pawn).Named("TITLE")).CapitalizeFirst());
								stringBuilder.AppendLine();
							}
							if (!outcome.heirTitleHigher)
							{
								RoyalTitle titleLocal = royalTitle;
								Pawn_RoyaltyTracker.tmpInheritedTitles.Add(delegate
								{
									int num = titleLocal.def.favorCost;
									RoyalTitleDef previousTitle_IncludeNonRewardable = titleLocal.def.GetPreviousTitle_IncludeNonRewardable(titleLocal.faction);
									int num2 = 1000;
									while (previousTitle_IncludeNonRewardable != null)
									{
										num += previousTitle_IncludeNonRewardable.favorCost;
										previousTitle_IncludeNonRewardable = previousTitle_IncludeNonRewardable.GetPreviousTitle_IncludeNonRewardable(titleLocal.faction);
										num2--;
										if (num2 <= 0)
										{
											Log.ErrorOnce("Iterations limit exceeded while getting favor for inheritance.", 91727191);
											break;
										}
									}
									outcome.heir.royalty.GainFavor(titleLocal.faction, num);
									titleLocal.wasInherited = true;
								});
								if (outcome.heir.IsFreeColonist && !outcome.heir.IsQuestLodger())
								{
									flag = true;
								}
							}
						}
						else
						{
							stringBuilder.AppendLineTagged("LetterTitleInheritance_NoHeirFound".Translate(this.pawn.Named("PAWN"), royalTitle.def.GetLabelFor(this.pawn).Named("TITLE"), royalTitle.faction.Named("FACTION")).CapitalizeFirst());
						}
					}
				}
				if (stringBuilder.Length > 0 && flag)
				{
					Find.LetterStack.ReceiveLetter("LetterTitleInheritance".Translate(), stringBuilder.ToString().TrimEndNewlines(), LetterDefOf.PositiveEvent, null, 0, true);
				}
				foreach (Action action in Pawn_RoyaltyTracker.tmpInheritedTitles)
				{
					action();
				}
			}
			finally
			{
				Pawn_RoyaltyTracker.tmpInheritedTitles.Clear();
			}
		}

		// Token: 0x0600CDD6 RID: 52694 RVA: 0x003C7080 File Offset: 0x003C5280
		public void Notify_Resurrected()
		{
			foreach (Faction faction in (from t in this.titles
			select t.faction).Distinct<Faction>().ToList<Faction>())
			{
				int index = this.FindFactionTitleIndex(faction, false);
				if (this.titles[index].wasInherited)
				{
					List<FactionPermit> list = this.PermitsFromFaction(faction);
					for (int i = 0; i < list.Count; i++)
					{
						if (list[i].Title == this.titles[index].def)
						{
							this.factionPermits.Remove(list[i]);
						}
					}
					this.SetTitle(faction, null, false, false, false);
				}
			}
			this.UpdateAvailableAbilities();
		}

		// Token: 0x0600CDD7 RID: 52695 RVA: 0x003C7180 File Offset: 0x003C5380
		public Gizmo RoyalAidGizmo()
		{
			Command_Action command_Action = new Command_Action();
			command_Action.defaultLabel = "CommandCallRoyalAid".Translate();
			command_Action.defaultDesc = "CommandCallRoyalAidDesc".Translate();
			command_Action.icon = Pawn_RoyaltyTracker.CommandTex;
			if (Find.Selector.NumSelected > 1)
			{
				Command_Action command_Action2 = command_Action;
				command_Action2.defaultLabel = command_Action2.defaultLabel + " (" + this.pawn.LabelShort + ")";
			}
			if (this.pawn.Spawned && this.pawn.Map.weatherManager.CurWeatherMaxRangeCap >= 0f)
			{
				command_Action.defaultDescPostfix = "\n\n" + ("WeatherMaxRangeCap".Translate() + ": " + this.pawn.Map.weatherManager.curWeather.LabelCap).Colorize(ColoredText.WarningColor);
			}
			if (this.pawn.Downed)
			{
				command_Action.Disable("CommandDisabledUnconscious".TranslateWithBackup("CommandCallRoyalAidUnconscious").Formatted(this.pawn));
			}
			if (this.pawn.IsSlave)
			{
				command_Action.Disable("CommandCallRoyalAidSlave".Translate());
			}
			if (this.pawn.IsQuestLodger())
			{
				command_Action.Disable("CommandCallRoyalAidLodger".Translate());
			}
			Map mapHeld = this.pawn.MapHeld;
			if (mapHeld != null && mapHeld.IsPocketMap && mapHeld.generatorDef.disableCallAid)
			{
				command_Action.Disable("CommandCallRoyalAidInvalidMap".Translate());
			}
			command_Action.action = delegate()
			{
				List<FloatMenuOption> list = new List<FloatMenuOption>();
				foreach (FactionPermit factionPermit in this.factionPermits)
				{
					IEnumerable<FloatMenuOption> royalAidOptions = factionPermit.Permit.Worker.GetRoyalAidOptions(this.pawn.MapHeld, this.pawn, factionPermit.Faction);
					if (royalAidOptions != null)
					{
						list.AddRange(royalAidOptions);
					}
				}
				Find.WindowStack.Add(new FloatMenu(list));
			};
			return command_Action;
		}

		// Token: 0x0600CDD8 RID: 52696 RVA: 0x003C733D File Offset: 0x003C553D
		public bool CanRequireThroneroom()
		{
			return this.pawn.IsFreeColonist && this.allowRoomRequirements && !this.pawn.IsQuestLodger();
		}

		// Token: 0x0600CDD9 RID: 52697 RVA: 0x003C7364 File Offset: 0x003C5564
		public RoyalTitle HighestTitleWithThroneRoomRequirements()
		{
			if (!this.CanRequireThroneroom())
			{
				return null;
			}
			RoyalTitle royalTitle = null;
			List<RoyalTitle> allTitlesInEffectForReading = this.AllTitlesInEffectForReading;
			for (int i = 0; i < allTitlesInEffectForReading.Count; i++)
			{
				if (!allTitlesInEffectForReading[i].def.throneRoomRequirements.EnumerableNullOrEmpty<RoomRequirement>() && (royalTitle == null || allTitlesInEffectForReading[i].def.seniority > royalTitle.def.seniority))
				{
					royalTitle = allTitlesInEffectForReading[i];
				}
			}
			return royalTitle;
		}

		// Token: 0x0600CDDA RID: 52698 RVA: 0x003C73D7 File Offset: 0x003C55D7
		public IEnumerable<string> GetUnmetThroneroomRequirements(bool includeOnGracePeriod = true, bool onlyOnGracePeriod = false)
		{
			Building_Throne assignedThrone = this.pawn.ownership.AssignedThrone;
			if (assignedThrone == null || !assignedThrone.SpawnedOrAnyParentSpawned)
			{
				yield break;
			}
			if (GenTicks.TicksGame - assignedThrone.MapHeld.generationTick < 10)
			{
				yield break;
			}
			RoyalTitle royalTitle = this.HighestTitleWithThroneRoomRequirements();
			if (royalTitle == null)
			{
				yield break;
			}
			Room throneRoom = assignedThrone.GetRoom(RegionType.Set_All);
			if (throneRoom == null)
			{
				yield break;
			}
			bool roomValid = RoomRoleWorker_ThroneRoom.Validate(throneRoom) == null;
			bool gracePeriodActive = royalTitle.RoomRequirementGracePeriodActive(this.pawn);
			RoyalTitleDef prevTitle = royalTitle.def.GetPreviousTitle(royalTitle.faction);
			foreach (RoomRequirement roomRequirement in royalTitle.def.throneRoomRequirements)
			{
				if (!roomValid || !roomRequirement.MetOrDisabled(throneRoom, this.pawn))
				{
					bool flag = (prevTitle != null && !prevTitle.HasSameThroneroomRequirement(roomRequirement)) || MoveColonyUtility.TitleAndRoleRequirementsGracePeriodActive;
					if ((!onlyOnGracePeriod || (flag && gracePeriodActive)) && (!gracePeriodActive || !flag || includeOnGracePeriod))
					{
						yield return roomRequirement.LabelCap(throneRoom);
					}
				}
			}
			List<RoomRequirement>.Enumerator enumerator = default(List<RoomRequirement>.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x0600CDDB RID: 52699 RVA: 0x003C73F8 File Offset: 0x003C55F8
		public bool AnyUnmetThroneroomRequirements()
		{
			if (this.pawn.ownership.AssignedThrone == null)
			{
				return false;
			}
			RoyalTitle royalTitle = this.HighestTitleWithThroneRoomRequirements();
			if (royalTitle == null)
			{
				return false;
			}
			Room room = this.pawn.ownership.AssignedThrone.GetRoom(RegionType.Set_All);
			if (room == null)
			{
				return false;
			}
			if (RoomRoleWorker_ThroneRoom.Validate(room) != null)
			{
				return true;
			}
			using (List<RoomRequirement>.Enumerator enumerator = royalTitle.def.throneRoomRequirements.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (!enumerator.Current.MetOrDisabled(room, this.pawn))
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x0600CDDC RID: 52700 RVA: 0x003C74A4 File Offset: 0x003C56A4
		public bool CanRequireBedroom()
		{
			return this.allowRoomRequirements && !this.pawn.IsPrisoner;
		}

		// Token: 0x0600CDDD RID: 52701 RVA: 0x003C74C0 File Offset: 0x003C56C0
		public RoyalTitle HighestTitleWithBedroomRequirements()
		{
			if (!this.CanRequireBedroom())
			{
				return null;
			}
			RoyalTitle royalTitle = null;
			List<RoyalTitle> allTitlesInEffectForReading = this.AllTitlesInEffectForReading;
			for (int i = 0; i < allTitlesInEffectForReading.Count; i++)
			{
				if (!allTitlesInEffectForReading[i].def.GetBedroomRequirements(this.pawn).EnumerableNullOrEmpty<RoomRequirement>() && (royalTitle == null || allTitlesInEffectForReading[i].def.seniority > royalTitle.def.seniority))
				{
					royalTitle = allTitlesInEffectForReading[i];
				}
			}
			return royalTitle;
		}

		// Token: 0x0600CDDE RID: 52702 RVA: 0x003C753C File Offset: 0x003C573C
		public void UpdateAvailableAbilities()
		{
			this.abilities.RemoveAll(delegate(Ability a)
			{
				for (int k = 0; k < this.AllTitlesInEffectForReading.Count; k++)
				{
					if (this.AllTitlesInEffectForReading[k].def.grantedAbilities.Contains(a.def))
					{
						return false;
					}
				}
				return true;
			});
			for (int i = 0; i < this.AllTitlesInEffectForReading.Count; i++)
			{
				RoyalTitle royalTitle = this.AllTitlesInEffectForReading[i];
				for (int j = 0; j < royalTitle.def.grantedAbilities.Count; j++)
				{
					AbilityDef def = royalTitle.def.grantedAbilities[j];
					if (!this.abilities.Any((Ability a) => a.def == def))
					{
						this.abilities.Add(AbilityUtility.MakeAbility(royalTitle.def.grantedAbilities[j], this.pawn));
					}
				}
			}
		}

		// Token: 0x0600CDDF RID: 52703 RVA: 0x003C7602 File Offset: 0x003C5802
		public IEnumerable<string> GetUnmetBedroomRequirements(bool includeOnGracePeriod = true, bool onlyOnGracePeriod = false)
		{
			RoyalTitle royalTitle = this.HighestTitleWithBedroomRequirements();
			if (royalTitle == null)
			{
				yield break;
			}
			bool gracePeriodActive = royalTitle.RoomRequirementGracePeriodActive(this.pawn);
			RoyalTitleDef prevTitle = royalTitle.def.GetPreviousTitle(royalTitle.faction);
			if (!this.HasPersonalBedroom())
			{
				yield break;
			}
			Room bedroom = this.pawn.ownership.OwnedRoom;
			foreach (RoomRequirement roomRequirement in royalTitle.def.GetBedroomRequirements(this.pawn))
			{
				if (!roomRequirement.MetOrDisabled(bedroom, this.pawn))
				{
					bool flag = (prevTitle != null && !prevTitle.HasSameBedroomRequirement(roomRequirement)) || MoveColonyUtility.TitleAndRoleRequirementsGracePeriodActive;
					if ((!onlyOnGracePeriod || (flag && gracePeriodActive)) && (!gracePeriodActive || !flag || includeOnGracePeriod))
					{
						yield return roomRequirement.LabelCap(bedroom);
					}
				}
			}
			IEnumerator<RoomRequirement> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x0600CDE0 RID: 52704 RVA: 0x003C7620 File Offset: 0x003C5820
		public bool AnyUnmetBedroomRequirements()
		{
			RoyalTitle royalTitle = this.HighestTitleWithBedroomRequirements();
			if (royalTitle == null)
			{
				return false;
			}
			if (!this.HasPersonalBedroom())
			{
				return false;
			}
			Room ownedRoom = this.pawn.ownership.OwnedRoom;
			using (IEnumerator<RoomRequirement> enumerator = royalTitle.def.GetBedroomRequirements(this.pawn).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (!enumerator.Current.MetOrDisabled(ownedRoom, this.pawn))
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x0600CDE1 RID: 52705 RVA: 0x003C76AC File Offset: 0x003C58AC
		public bool HasPersonalBedroom()
		{
			Building_Bed ownedBed = this.pawn.ownership.OwnedBed;
			if (ownedBed == null)
			{
				return false;
			}
			Room ownedRoom = this.pawn.ownership.OwnedRoom;
			if (ownedRoom == null)
			{
				return false;
			}
			foreach (Building_Bed building_Bed in ownedRoom.ContainedBeds)
			{
				if (building_Bed != ownedBed)
				{
					List<Pawn> loveCluster = this.pawn.GetLoveCluster();
					if (building_Bed.OwnersForReading.Any((Pawn p) => p != this.pawn && !p.RaceProps.Animal && !loveCluster.Contains(p)))
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x0600CDE2 RID: 52706 RVA: 0x003C7768 File Offset: 0x003C5968
		public void ExposeData()
		{
			Scribe_Collections.Look<RoyalTitle>(ref this.titles, "titles", LookMode.Deep, Array.Empty<object>());
			Scribe_Collections.Look<Faction, int>(ref this.favor, "favor", LookMode.Reference, LookMode.Value, ref this.tmpFavorFactions, ref this.tmpAmounts, true, false, false);
			Scribe_Values.Look<int>(ref this.lastDecreeTicks, "lastDecreeTicks", -999999, false);
			Scribe_Collections.Look<Faction, RoyalTitleDef>(ref this.highestTitles, "highestTitles", LookMode.Reference, LookMode.Def, ref this.tmpHighestTitleFactions, ref this.tmpTitleDefs, true, false, false);
			Scribe_Collections.Look<Faction, Pawn>(ref this.heirs, "heirs", LookMode.Reference, LookMode.Reference, ref this.tmpHeirFactions, ref this.tmpPawns, true, false, false);
			Scribe_Values.Look<bool>(ref this.allowRoomRequirements, "allowRoomRequirements", true, false);
			Scribe_Values.Look<bool>(ref this.allowApparelRequirements, "allowApparelRequirements", true, false);
			Scribe_Collections.Look<FactionPermit>(ref this.factionPermits, "permits", LookMode.Deep, Array.Empty<object>());
			Scribe_Collections.Look<Ability>(ref this.abilities, "abilities", LookMode.Deep, new object[]
			{
				this.pawn
			});
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				if (this.titles.RemoveAll((RoyalTitle x) => x.def == null) != 0)
				{
					Log.Error("Some RoyalTitles had null defs after loading.");
				}
				foreach (RoyalTitle royalTitle in this.titles)
				{
					royalTitle.pawn = this.pawn;
				}
				if (this.heirs == null)
				{
					this.heirs = new Dictionary<Faction, Pawn>();
				}
				if (this.factionPermits == null)
				{
					this.factionPermits = new List<FactionPermit>();
				}
				if (this.factionPermits.RemoveAll((FactionPermit x) => DefDatabase<RoyalTitlePermitDef>.AllDefs.Any((RoyalTitlePermitDef y) => y.prerequisite == x.Permit && this.HasPermit(y, x.Faction) && this.HasPermit(y.prerequisite, x.Faction))) != 0)
				{
					Log.Error("Removed some null permits.");
				}
				foreach (RoyalTitle royalTitle2 in this.titles)
				{
					this.AssignHeirIfNone(royalTitle2.def, royalTitle2.faction);
				}
				if (this.abilities == null)
				{
					this.abilities = new List<Ability>();
				}
				this.UpdateAvailableAbilities();
			}
			BackCompatibility.PostExposeData(this);
		}

		// Token: 0x04008BFF RID: 35839
		public Pawn pawn;

		// Token: 0x04008C00 RID: 35840
		private List<RoyalTitle> titles = new List<RoyalTitle>();

		// Token: 0x04008C01 RID: 35841
		private Dictionary<Faction, int> favor = new Dictionary<Faction, int>();

		// Token: 0x04008C02 RID: 35842
		private Dictionary<Faction, RoyalTitleDef> highestTitles = new Dictionary<Faction, RoyalTitleDef>();

		// Token: 0x04008C03 RID: 35843
		private Dictionary<Faction, Pawn> heirs = new Dictionary<Faction, Pawn>();

		// Token: 0x04008C04 RID: 35844
		private List<FactionPermit> factionPermits = new List<FactionPermit>();

		// Token: 0x04008C05 RID: 35845
		private List<Ability> abilities = new List<Ability>();

		// Token: 0x04008C06 RID: 35846
		public int lastDecreeTicks = -999999;

		// Token: 0x04008C07 RID: 35847
		public bool allowRoomRequirements = true;

		// Token: 0x04008C08 RID: 35848
		public bool allowApparelRequirements = true;

		// Token: 0x04008C09 RID: 35849
		private static List<RoyalTitle> EmptyTitles = new List<RoyalTitle>();

		// Token: 0x04008C0A RID: 35850
		private const int BestowingCeremonyCheckInterval = 37500;

		// Token: 0x04008C0B RID: 35851
		private List<string> tmpDecreeTags = new List<string>();

		// Token: 0x04008C0C RID: 35852
		private static List<FactionPermit> tmpPermits = new List<FactionPermit>();

		// Token: 0x04008C0D RID: 35853
		private List<Faction> factionHeirsToClearTmp = new List<Faction>();

		// Token: 0x04008C0E RID: 35854
		private static List<Action> tmpInheritedTitles = new List<Action>();

		// Token: 0x04008C0F RID: 35855
		public static readonly Texture2D CommandTex = ContentFinder<Texture2D>.Get("UI/Commands/CallAid", true);

		// Token: 0x04008C10 RID: 35856
		private List<Faction> tmpFavorFactions;

		// Token: 0x04008C11 RID: 35857
		private List<Faction> tmpHighestTitleFactions;

		// Token: 0x04008C12 RID: 35858
		private List<Faction> tmpHeirFactions;

		// Token: 0x04008C13 RID: 35859
		private List<int> tmpAmounts;

		// Token: 0x04008C14 RID: 35860
		private List<Pawn> tmpPawns;

		// Token: 0x04008C15 RID: 35861
		private List<RoyalTitleDef> tmpTitleDefs;
	}
}
