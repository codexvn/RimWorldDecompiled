using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x0200258D RID: 9613
	public class FactionManager : IExposable
	{
		// Token: 0x1700206D RID: 8301
		// (get) Token: 0x0600D293 RID: 53907 RVA: 0x003E07A7 File Offset: 0x003DE9A7
		public List<Faction> AllFactionsListForReading
		{
			get
			{
				return this.allFactions;
			}
		}

		// Token: 0x1700206E RID: 8302
		// (get) Token: 0x0600D294 RID: 53908 RVA: 0x003E07A7 File Offset: 0x003DE9A7
		public IEnumerable<Faction> AllFactions
		{
			get
			{
				return this.allFactions;
			}
		}

		// Token: 0x1700206F RID: 8303
		// (get) Token: 0x0600D295 RID: 53909 RVA: 0x003E07AF File Offset: 0x003DE9AF
		public IEnumerable<Faction> AllFactionsVisible
		{
			get
			{
				return from fa in this.allFactions
				where !fa.Hidden
				select fa;
			}
		}

		// Token: 0x17002070 RID: 8304
		// (get) Token: 0x0600D296 RID: 53910 RVA: 0x003E07DB File Offset: 0x003DE9DB
		public IEnumerable<Faction> AllFactionsVisibleInViewOrder
		{
			get
			{
				return FactionManager.GetInViewOrder(this.AllFactionsVisible);
			}
		}

		// Token: 0x17002071 RID: 8305
		// (get) Token: 0x0600D297 RID: 53911 RVA: 0x003E07E8 File Offset: 0x003DE9E8
		public IEnumerable<Faction> AllFactionsInViewOrder
		{
			get
			{
				return FactionManager.GetInViewOrder(this.AllFactions);
			}
		}

		// Token: 0x17002072 RID: 8306
		// (get) Token: 0x0600D298 RID: 53912 RVA: 0x003E07F5 File Offset: 0x003DE9F5
		public Faction OfPlayer
		{
			get
			{
				return this.ofPlayer;
			}
		}

		// Token: 0x17002073 RID: 8307
		// (get) Token: 0x0600D299 RID: 53913 RVA: 0x003E07FD File Offset: 0x003DE9FD
		public Faction OfMechanoids
		{
			get
			{
				return this.ofMechanoids;
			}
		}

		// Token: 0x17002074 RID: 8308
		// (get) Token: 0x0600D29A RID: 53914 RVA: 0x003E0805 File Offset: 0x003DEA05
		public Faction OfInsects
		{
			get
			{
				return this.ofInsects;
			}
		}

		// Token: 0x17002075 RID: 8309
		// (get) Token: 0x0600D29B RID: 53915 RVA: 0x003E080D File Offset: 0x003DEA0D
		public Faction OfAncients
		{
			get
			{
				return this.ofAncients;
			}
		}

		// Token: 0x17002076 RID: 8310
		// (get) Token: 0x0600D29C RID: 53916 RVA: 0x003E0815 File Offset: 0x003DEA15
		public Faction OfAncientsHostile
		{
			get
			{
				return this.ofAncientsHostile;
			}
		}

		// Token: 0x17002077 RID: 8311
		// (get) Token: 0x0600D29D RID: 53917 RVA: 0x003E081D File Offset: 0x003DEA1D
		public Faction OfEmpire
		{
			get
			{
				return this.empire;
			}
		}

		// Token: 0x17002078 RID: 8312
		// (get) Token: 0x0600D29E RID: 53918 RVA: 0x003E0825 File Offset: 0x003DEA25
		public Faction OfPirates
		{
			get
			{
				return this.ofPirates;
			}
		}

		// Token: 0x17002079 RID: 8313
		// (get) Token: 0x0600D29F RID: 53919 RVA: 0x003E082D File Offset: 0x003DEA2D
		public Faction OfHoraxCult
		{
			get
			{
				return this.ofHoraxCult;
			}
		}

		// Token: 0x1700207A RID: 8314
		// (get) Token: 0x0600D2A0 RID: 53920 RVA: 0x003E0835 File Offset: 0x003DEA35
		public Faction OfEntities
		{
			get
			{
				return this.ofEntities;
			}
		}

		// Token: 0x1700207B RID: 8315
		// (get) Token: 0x0600D2A1 RID: 53921 RVA: 0x003E083D File Offset: 0x003DEA3D
		public Faction OfTradersGuild
		{
			get
			{
				return this.ofTradersGuild;
			}
		}

		// Token: 0x1700207C RID: 8316
		// (get) Token: 0x0600D2A2 RID: 53922 RVA: 0x003E0845 File Offset: 0x003DEA45
		public Faction OfSalvagers
		{
			get
			{
				return this.ofSalvagers;
			}
		}

		// Token: 0x0600D2A3 RID: 53923 RVA: 0x003E0850 File Offset: 0x003DEA50
		public void ExposeData()
		{
			Scribe_Collections.Look<Faction>(ref this.allFactions, "allFactions", LookMode.Deep, Array.Empty<object>());
			Scribe_Collections.Look<Faction>(ref this.toRemove, "toRemove", LookMode.Reference, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				BackCompatibility.FactionManagerPostLoadInit();
				if (this.toRemove == null)
				{
					this.toRemove = new List<Faction>();
				}
			}
			if (Scribe.mode == LoadSaveMode.LoadingVars || Scribe.mode == LoadSaveMode.ResolvingCrossRefs || Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				if (this.allFactions.RemoveAll((Faction x) => x == null || x.def == null) != 0)
				{
					Log.Error("Some factions were null after loading.");
				}
				this.RecacheFactions();
			}
		}

		// Token: 0x0600D2A4 RID: 53924 RVA: 0x003E0900 File Offset: 0x003DEB00
		public void Add(Faction faction)
		{
			if (this.allFactions.Contains(faction))
			{
				return;
			}
			this.allFactions.Add(faction);
			this.RecacheFactions();
			foreach (Map map in Find.Maps)
			{
				map.events.Notify_FactionAdded(faction);
			}
		}

		// Token: 0x0600D2A5 RID: 53925 RVA: 0x003E0978 File Offset: 0x003DEB78
		private void Remove(Faction faction)
		{
			if (!faction.temporary)
			{
				Log.Error("Attempting to remove " + faction.Name + " which is not a temporary faction, only temporary factions can be removed");
				return;
			}
			if (!this.allFactions.Contains(faction))
			{
				return;
			}
			faction.RemoveAllRelations();
			this.allFactions.Remove(faction);
			List<Pawn> allMapsWorldAndTemporary_AliveOrDead = PawnsFinder.AllMapsWorldAndTemporary_AliveOrDead;
			for (int i = 0; i < allMapsWorldAndTemporary_AliveOrDead.Count; i++)
			{
				if (allMapsWorldAndTemporary_AliveOrDead[i].Faction == faction)
				{
					allMapsWorldAndTemporary_AliveOrDead[i].SetFaction(null, null);
				}
			}
			for (int j = 0; j < Find.Maps.Count; j++)
			{
				Find.Maps[j].pawnDestinationReservationManager.Notify_FactionRemoved(faction);
				Find.Maps[j].listerBuildings.Notify_FactionRemoved(faction);
			}
			Find.LetterStack.Notify_FactionRemoved(faction);
			Find.PlayLog.Notify_FactionRemoved(faction);
			this.RecacheFactions();
			Find.QuestManager.Notify_FactionRemoved(faction);
			Find.IdeoManager.Notify_FactionRemoved(faction);
			Find.TaleManager.Notify_FactionRemoved(faction);
			foreach (Map map in Find.Maps)
			{
				map.events.Notify_FactionRemoved(faction);
			}
		}

		// Token: 0x0600D2A6 RID: 53926 RVA: 0x003E0AC4 File Offset: 0x003DECC4
		public void FactionManagerTick()
		{
			this.goodwillSituationManager.GoodwillManagerTick();
			SettlementProximityGoodwillUtility.CheckSettlementProximityGoodwillChange();
			for (int i = 0; i < this.allFactions.Count; i++)
			{
				this.allFactions[i].FactionTick();
			}
			for (int j = this.toRemove.Count - 1; j >= 0; j--)
			{
				Faction faction = this.toRemove[j];
				this.toRemove.Remove(faction);
				this.Remove(faction);
			}
		}

		// Token: 0x0600D2A7 RID: 53927 RVA: 0x003E0B44 File Offset: 0x003DED44
		public Faction FirstFactionOfDef(FactionDef facDef)
		{
			for (int i = 0; i < this.allFactions.Count; i++)
			{
				if (this.allFactions[i].def == facDef)
				{
					return this.allFactions[i];
				}
			}
			return null;
		}

		// Token: 0x0600D2A8 RID: 53928 RVA: 0x003E0B8C File Offset: 0x003DED8C
		public bool TryGetRandomNonColonyHumanlikeFaction(out Faction faction, bool tryMedievalOrBetter, bool allowDefeated = false, TechLevel minTechLevel = TechLevel.Undefined, TechLevel maxTechLevel = TechLevel.Undefined, bool allowTemporary = false, bool requireHostile = false)
		{
			return (from x in this.AllFactions
			where !x.IsPlayer && !x.Hidden && x.def.humanlikeFaction && (allowDefeated || !x.defeated) && (allowTemporary || !x.temporary) && (minTechLevel == TechLevel.Undefined || x.def.techLevel >= minTechLevel) && (maxTechLevel == TechLevel.Undefined || x.def.techLevel <= maxTechLevel) && (!requireHostile || x.HostileTo(Faction.OfPlayer))
			select x).TryRandomElementByWeight(delegate(Faction x)
			{
				if (tryMedievalOrBetter && x.def.techLevel < TechLevel.Medieval)
				{
					return 0.1f;
				}
				return 1f;
			}, out faction);
		}

		// Token: 0x0600D2A9 RID: 53929 RVA: 0x003E0BF6 File Offset: 0x003DEDF6
		public IEnumerable<Faction> GetFactions(bool allowHidden = false, bool allowDefeated = false, bool allowNonHumanlike = true, TechLevel minTechLevel = TechLevel.Undefined, bool allowTemporary = false)
		{
			int num;
			for (int i = 0; i < this.allFactions.Count; i = num + 1)
			{
				Faction faction = this.allFactions[i];
				if (!faction.IsPlayer && (allowHidden || !faction.Hidden) && (allowTemporary || !faction.temporary) && (allowDefeated || !faction.defeated) && (allowNonHumanlike || faction.def.humanlikeFaction) && (minTechLevel == TechLevel.Undefined || faction.def.techLevel >= minTechLevel))
				{
					yield return faction;
				}
				num = i;
			}
			yield break;
		}

		// Token: 0x0600D2AA RID: 53930 RVA: 0x003E0C2C File Offset: 0x003DEE2C
		public Faction RandomEnemyFaction(bool allowHidden = false, bool allowDefeated = false, bool allowNonHumanlike = true, TechLevel minTechLevel = TechLevel.Undefined)
		{
			Faction result;
			if ((from x in this.GetFactions(allowHidden, allowDefeated, allowNonHumanlike, minTechLevel, false)
			where x.HostileTo(Faction.OfPlayer)
			select x).TryRandomElement(out result))
			{
				return result;
			}
			return null;
		}

		// Token: 0x0600D2AB RID: 53931 RVA: 0x003E0C78 File Offset: 0x003DEE78
		public Faction RandomRaidableEnemyFaction(bool allowHidden = false, bool allowDefeated = false, bool allowNonHumanlike = true, TechLevel minTechLevel = TechLevel.Undefined)
		{
			Faction result;
			if ((from x in this.GetFactions(allowHidden, allowDefeated, allowNonHumanlike, minTechLevel, false)
			where x.HostileTo(Faction.OfPlayer) && !x.def.pawnGroupMakers.NullOrEmpty<PawnGroupMaker>()
			select x).TryRandomElement(out result))
			{
				return result;
			}
			return null;
		}

		// Token: 0x0600D2AC RID: 53932 RVA: 0x003E0CC4 File Offset: 0x003DEEC4
		public Faction RandomNonHostileFaction(bool allowHidden = false, bool allowDefeated = false, bool allowNonHumanlike = true, TechLevel minTechLevel = TechLevel.Undefined)
		{
			Faction result;
			if ((from x in this.GetFactions(allowHidden, allowDefeated, allowNonHumanlike, minTechLevel, false)
			where !x.HostileTo(Faction.OfPlayer)
			select x).TryRandomElement(out result))
			{
				return result;
			}
			return null;
		}

		// Token: 0x0600D2AD RID: 53933 RVA: 0x003E0D10 File Offset: 0x003DEF10
		public Faction RandomAlliedFaction(bool allowHidden = false, bool allowDefeated = false, bool allowNonHumanlike = true, TechLevel minTechLevel = TechLevel.Undefined)
		{
			Faction result;
			if ((from x in this.GetFactions(allowHidden, allowDefeated, allowNonHumanlike, minTechLevel, false)
			where x.PlayerRelationKind == FactionRelationKind.Ally
			select x).TryRandomElement(out result))
			{
				return result;
			}
			return null;
		}

		// Token: 0x0600D2AE RID: 53934 RVA: 0x003E0D5C File Offset: 0x003DEF5C
		public Faction RandomRoyalFaction(bool allowHidden = false, bool allowDefeated = false, bool allowNonHumanlike = true, TechLevel minTechLevel = TechLevel.Undefined)
		{
			Faction result;
			if ((from x in this.GetFactions(allowHidden, allowDefeated, allowNonHumanlike, minTechLevel, false)
			where x.def.HasRoyalTitles
			select x).TryRandomElement(out result))
			{
				return result;
			}
			return null;
		}

		// Token: 0x0600D2AF RID: 53935 RVA: 0x003E0DA8 File Offset: 0x003DEFA8
		public void LogKidnappedPawns()
		{
			Log.Message("Kidnapped pawns:");
			for (int i = 0; i < this.allFactions.Count; i++)
			{
				this.allFactions[i].kidnapped.LogKidnappedPawns();
			}
		}

		// Token: 0x0600D2B0 RID: 53936 RVA: 0x003E0DEC File Offset: 0x003DEFEC
		public void LogAllFactions()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (Faction faction in this.allFactions)
			{
				stringBuilder.AppendLine(string.Format("name: {0}, temporary: {1}, can be deleted?: {2}", faction.Name, faction.temporary, this.FactionCanBeRemoved(faction)));
			}
			stringBuilder.AppendLine(string.Format("{0} factions found.", this.allFactions.Count));
			Log.Message(stringBuilder.ToString());
		}

		// Token: 0x0600D2B1 RID: 53937 RVA: 0x003E0E98 File Offset: 0x003DF098
		public void LogFactionsToRemove()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (Faction faction in this.toRemove)
			{
				stringBuilder.AppendLine(string.Format("name: {0}, temporary: {1}, can be deleted?: {2}", faction.Name, faction.temporary, this.FactionCanBeRemoved(faction)));
			}
			stringBuilder.AppendLine(string.Format("{0} factions found.", this.toRemove.Count));
			Log.Message(stringBuilder.ToString());
		}

		// Token: 0x0600D2B2 RID: 53938 RVA: 0x003E0F44 File Offset: 0x003DF144
		public void LogFactionsOnPawns()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (IGrouping<Faction, Pawn> grouping in from p in Find.WorldPawns.AllPawnsAliveOrDead
			group p by p.Faction)
			{
				if (grouping.Key == null)
				{
					stringBuilder.AppendLine(string.Format("no faction: {0} pawns found.", grouping.Count<Pawn>()));
				}
				else
				{
					stringBuilder.AppendLine(string.Format("{0}: {1} pawns found.", grouping.Key, grouping.Count<Pawn>()));
				}
			}
			Log.Message(stringBuilder.ToString());
		}

		// Token: 0x0600D2B3 RID: 53939 RVA: 0x003E100C File Offset: 0x003DF20C
		public static IEnumerable<Faction> GetInViewOrder(IEnumerable<Faction> factions)
		{
			return from x in factions
			orderby x.defeated, x.def.listOrderPriority descending
			select x;
		}

		// Token: 0x0600D2B4 RID: 53940 RVA: 0x003E1064 File Offset: 0x003DF264
		private void RecacheFactions()
		{
			this.ofPlayer = null;
			for (int i = 0; i < this.allFactions.Count; i++)
			{
				if (this.allFactions[i].IsPlayer)
				{
					this.ofPlayer = this.allFactions[i];
					break;
				}
			}
			this.ofMechanoids = this.FirstFactionOfDef(FactionDefOf.Mechanoid);
			this.ofInsects = this.FirstFactionOfDef(FactionDefOf.Insect);
			this.ofAncients = this.FirstFactionOfDef(FactionDefOf.Ancients);
			this.ofAncientsHostile = this.FirstFactionOfDef(FactionDefOf.AncientsHostile);
			this.empire = this.FirstFactionOfDef(FactionDefOf.Empire);
			this.ofPirates = this.FirstFactionOfDef(FactionDefOf.Pirate);
			this.ofHoraxCult = this.FirstFactionOfDef(FactionDefOf.HoraxCult);
			this.ofEntities = this.FirstFactionOfDef(FactionDefOf.Entities);
			this.ofTradersGuild = this.FirstFactionOfDef(FactionDefOf.TradersGuild);
			this.ofSalvagers = this.FirstFactionOfDef(FactionDefOf.Salvagers);
		}

		// Token: 0x0600D2B5 RID: 53941 RVA: 0x003E1160 File Offset: 0x003DF360
		public void Notify_QuestCleanedUp(Quest quest)
		{
			for (int i = this.allFactions.Count - 1; i >= 0; i--)
			{
				Faction faction = this.allFactions[i];
				if (this.FactionCanBeRemoved(faction))
				{
					this.QueueForRemoval(faction);
				}
			}
		}

		// Token: 0x0600D2B6 RID: 53942 RVA: 0x003E11A2 File Offset: 0x003DF3A2
		public void Notify_PawnKilled(Pawn pawn)
		{
			this.TryQueuePawnFactionForRemoval(pawn);
		}

		// Token: 0x0600D2B7 RID: 53943 RVA: 0x003E11A2 File Offset: 0x003DF3A2
		public void Notify_PawnLeftMap(Pawn pawn)
		{
			this.TryQueuePawnFactionForRemoval(pawn);
		}

		// Token: 0x0600D2B8 RID: 53944 RVA: 0x003E11AB File Offset: 0x003DF3AB
		public void Notify_PawnLeftFaction(Faction oldFaction)
		{
			if (this.FactionCanBeRemoved(oldFaction))
			{
				this.QueueForRemoval(oldFaction);
			}
		}

		// Token: 0x0600D2B9 RID: 53945 RVA: 0x003E11BD File Offset: 0x003DF3BD
		public void Notify_WorldObjectDestroyed(WorldObject worldObject)
		{
			if (worldObject.Faction == null)
			{
				return;
			}
			if (this.FactionCanBeRemoved(worldObject.Faction))
			{
				this.QueueForRemoval(worldObject.Faction);
			}
		}

		// Token: 0x0600D2BA RID: 53946 RVA: 0x003E11E4 File Offset: 0x003DF3E4
		private void TryQueuePawnFactionForRemoval(Pawn pawn)
		{
			if (pawn.Faction != null && this.FactionCanBeRemoved(pawn.Faction))
			{
				this.QueueForRemoval(pawn.Faction);
			}
			Faction extraHomeFaction = pawn.GetExtraHomeFaction(null);
			if (extraHomeFaction != null && this.FactionCanBeRemoved(extraHomeFaction))
			{
				this.QueueForRemoval(extraHomeFaction);
			}
			Faction extraMiniFaction = pawn.GetExtraMiniFaction(null);
			if (extraMiniFaction != null && this.FactionCanBeRemoved(extraMiniFaction))
			{
				this.QueueForRemoval(extraMiniFaction);
			}
			if (pawn.SlaveFaction != null && this.FactionCanBeRemoved(pawn.SlaveFaction))
			{
				this.QueueForRemoval(pawn.SlaveFaction);
			}
		}

		// Token: 0x0600D2BB RID: 53947 RVA: 0x003E126B File Offset: 0x003DF46B
		private void QueueForRemoval(Faction faction)
		{
			if (!faction.temporary)
			{
				Log.Error("Cannot queue faction " + faction.Name + " for removal, only temporary factions can be removed");
				return;
			}
			if (!this.toRemove.Contains(faction))
			{
				this.toRemove.Add(faction);
			}
		}

		// Token: 0x0600D2BC RID: 53948 RVA: 0x003E12AC File Offset: 0x003DF4AC
		private bool FactionCanBeRemoved(Faction faction)
		{
			FactionManager.<>c__DisplayClass71_0 CS$<>8__locals1;
			CS$<>8__locals1.faction = faction;
			if (!CS$<>8__locals1.faction.temporary || this.toRemove.Contains(CS$<>8__locals1.faction) || Find.QuestManager.IsReservedByAnyQuest(CS$<>8__locals1.faction))
			{
				return false;
			}
			if (!FactionManager.<FactionCanBeRemoved>g__CheckPawns|71_0(PawnsFinder.AllMaps_Spawned, ref CS$<>8__locals1))
			{
				return false;
			}
			if (!FactionManager.<FactionCanBeRemoved>g__CheckPawns|71_0(PawnsFinder.AllCaravansAndTravellingTransporters_Alive, ref CS$<>8__locals1))
			{
				return false;
			}
			using (List<WorldObject>.Enumerator enumerator = Find.WorldObjects.AllWorldObjects.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.Faction == CS$<>8__locals1.faction)
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x0600D2BE RID: 53950 RVA: 0x003E1398 File Offset: 0x003DF598
		[CompilerGenerated]
		internal static bool <FactionCanBeRemoved>g__CheckPawns|71_0(IReadOnlyList<Pawn> pawns, ref FactionManager.<>c__DisplayClass71_0 A_1)
		{
			for (int i = 0; i < pawns.Count; i++)
			{
				Pawn pawn = pawns[i];
				if (!pawn.Dead && ((pawn.Faction != null && pawn.Faction == A_1.faction) || A_1.faction == pawn.GetExtraHomeFaction(null) || A_1.faction == pawn.GetExtraMiniFaction(null) || A_1.faction == pawn.SlaveFaction || (A_1.faction.leader == pawn && pawn.Faction != Faction.OfPlayer)))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x04008EFF RID: 36607
		private List<Faction> allFactions = new List<Faction>();

		// Token: 0x04008F00 RID: 36608
		private List<Faction> toRemove = new List<Faction>();

		// Token: 0x04008F01 RID: 36609
		public GoodwillSituationManager goodwillSituationManager = new GoodwillSituationManager();

		// Token: 0x04008F02 RID: 36610
		private Faction ofPlayer;

		// Token: 0x04008F03 RID: 36611
		private Faction ofMechanoids;

		// Token: 0x04008F04 RID: 36612
		private Faction ofInsects;

		// Token: 0x04008F05 RID: 36613
		private Faction ofAncients;

		// Token: 0x04008F06 RID: 36614
		private Faction ofAncientsHostile;

		// Token: 0x04008F07 RID: 36615
		private Faction empire;

		// Token: 0x04008F08 RID: 36616
		private Faction ofPirates;

		// Token: 0x04008F09 RID: 36617
		private Faction ofHoraxCult;

		// Token: 0x04008F0A RID: 36618
		private Faction ofEntities;

		// Token: 0x04008F0B RID: 36619
		private Faction ofTradersGuild;

		// Token: 0x04008F0C RID: 36620
		private Faction ofSalvagers;
	}
}
