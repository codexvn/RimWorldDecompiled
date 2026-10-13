using System;
using System.Collections.Generic;
using System.Linq;
using LudeonTK;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001E0A RID: 7690
	public abstract class IncidentWorker_Raid : IncidentWorker_PawnsArrive
	{
		// Token: 0x0600AB2B RID: 43819
		protected abstract bool TryResolveRaidFaction(IncidentParms parms);

		// Token: 0x0600AB2C RID: 43820
		public abstract void ResolveRaidStrategy(IncidentParms parms, PawnGroupKindDef groupKind);

		// Token: 0x0600AB2D RID: 43821
		protected abstract string GetLetterLabel(IncidentParms parms);

		// Token: 0x0600AB2E RID: 43822
		protected abstract string GetLetterText(IncidentParms parms, List<Pawn> pawns);

		// Token: 0x0600AB2F RID: 43823
		protected abstract LetterDef GetLetterDef();

		// Token: 0x0600AB30 RID: 43824
		protected abstract string GetRelatedPawnsInfoLetterText(IncidentParms parms);

		// Token: 0x0600AB31 RID: 43825
		protected abstract void ResolveRaidPoints(IncidentParms parms);

		// Token: 0x0600AB32 RID: 43826 RVA: 0x0030ED0C File Offset: 0x0030CF0C
		public virtual void ResolveRaidArriveMode(IncidentParms parms)
		{
			if (parms.raidArrivalMode != null)
			{
				return;
			}
			if (parms.raidArrivalModeForQuickMilitaryAid)
			{
				if (!(from d in DefDatabase<PawnsArrivalModeDef>.AllDefs
				where d.forQuickMilitaryAid
				select d).Any((PawnsArrivalModeDef d) => d.Worker.GetSelectionWeight(parms) > 0f))
				{
					parms.raidArrivalMode = ((Rand.Value < 0.6f) ? PawnsArrivalModeDefOf.EdgeDrop : PawnsArrivalModeDefOf.CenterDrop);
					return;
				}
			}
			if (parms.raidStrategy == null)
			{
				Log.Error("parms raidStrategy was null but shouldn't be. Defaulting to ImmediateAttack.");
				parms.raidStrategy = RaidStrategyDefOf.ImmediateAttack;
			}
			if (!(from x in parms.raidStrategy.arriveModes
			where x.Worker.CanUseWith(parms)
			select x).TryRandomElementByWeight((PawnsArrivalModeDef x) => x.Worker.GetSelectionWeight(parms), out parms.raidArrivalMode))
			{
				string str = "Could not resolve arrival mode for raid. Defaulting to EdgeWalkIn. parms=";
				IncidentParms parms2 = parms;
				Log.Error(str + ((parms2 != null) ? parms2.ToString() : null));
				parms.raidArrivalMode = PawnsArrivalModeDefOf.EdgeWalkIn;
			}
		}

		// Token: 0x0600AB33 RID: 43827 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void ResolveRaidAgeRestriction(IncidentParms parms)
		{
		}

		// Token: 0x0600AB34 RID: 43828 RVA: 0x000026EA File Offset: 0x000008EA
		protected virtual void GenerateRaidLoot(IncidentParms parms, float raidLootPoints, List<Pawn> pawns)
		{
		}

		// Token: 0x0600AB35 RID: 43829 RVA: 0x0030EE38 File Offset: 0x0030D038
		public bool TryGenerateRaidInfo(IncidentParms parms, out List<Pawn> pawns, bool debugTest = false)
		{
			pawns = null;
			this.ResolveRaidPoints(parms);
			if (!this.TryResolveRaidFaction(parms))
			{
				return false;
			}
			PawnGroupKindDef groupKind = parms.pawnGroupKind ?? PawnGroupKindDefOf.Combat;
			this.ResolveRaidStrategy(parms, groupKind);
			this.ResolveRaidArriveMode(parms);
			this.ResolveRaidAgeRestriction(parms);
			if (!debugTest)
			{
				parms.raidStrategy.Worker.TryGenerateThreats(parms);
			}
			if (!debugTest && !parms.raidArrivalMode.Worker.TryResolveRaidSpawnCenter(parms))
			{
				return false;
			}
			float points = parms.points;
			parms.points = IncidentWorker_Raid.AdjustedRaidPoints(parms.points, parms.raidArrivalMode, parms.raidStrategy, parms.faction, groupKind, parms.target, parms.raidAgeRestriction);
			if (!debugTest)
			{
				pawns = parms.raidStrategy.Worker.SpawnThreats(parms);
			}
			if (pawns == null)
			{
				PawnGroupMakerParms defaultPawnGroupMakerParms = IncidentParmsUtility.GetDefaultPawnGroupMakerParms(groupKind, parms, false);
				pawns = PawnGroupMakerUtility.GeneratePawns(defaultPawnGroupMakerParms, true).ToList<Pawn>();
				if (pawns.Count == 0)
				{
					if (debugTest)
					{
						Log.Error("Got no pawns spawning raid from parms " + ((parms != null) ? parms.ToString() : null));
					}
					return false;
				}
				if (!debugTest)
				{
					parms.raidArrivalMode.Worker.Arrive(pawns, parms);
				}
			}
			parms.pawnCount = pawns.Count;
			this.PostProcessSpawnedPawns(parms, pawns);
			if (debugTest)
			{
				parms.target.StoryState.lastRaidFaction = parms.faction;
			}
			else
			{
				this.GenerateRaidLoot(parms, points, pawns);
			}
			return true;
		}

		// Token: 0x0600AB36 RID: 43830 RVA: 0x000026EA File Offset: 0x000008EA
		protected virtual void PostProcessSpawnedPawns(IncidentParms parms, List<Pawn> pawns)
		{
		}

		// Token: 0x0600AB37 RID: 43831 RVA: 0x0030EF90 File Offset: 0x0030D190
		protected override bool TryExecuteWorker(IncidentParms parms)
		{
			List<Pawn> list;
			if (!this.TryGenerateRaidInfo(parms, out list, false))
			{
				return false;
			}
			TaggedString baseLetterLabel = this.GetLetterLabel(parms);
			TaggedString baseLetterText = this.GetLetterText(parms, list);
			PawnRelationUtility.Notify_PawnsSeenByPlayer_Letter(list, ref baseLetterLabel, ref baseLetterText, this.GetRelatedPawnsInfoLetterText(parms), true, true);
			List<TargetInfo> list2 = new List<TargetInfo>();
			if (parms.pawnGroups != null)
			{
				List<List<Pawn>> list3 = IncidentParmsUtility.SplitIntoGroups(list, parms.pawnGroups);
				List<Pawn> list4 = list3.MaxBy((List<Pawn> x) => x.Count);
				if (list4.Any<Pawn>())
				{
					list2.Add(list4[0]);
				}
				for (int i = 0; i < list3.Count; i++)
				{
					if (list3[i] != list4 && list3[i].Any<Pawn>())
					{
						list2.Add(list3[i][0]);
					}
				}
			}
			else if (list.Any<Pawn>())
			{
				foreach (Pawn t in list)
				{
					list2.Add(t);
				}
			}
			base.SendStandardLetter(baseLetterLabel, baseLetterText, this.GetLetterDef(), parms, list2, Array.Empty<NamedArgument>());
			if (parms.controllerPawn == null || parms.controllerPawn.Faction != Faction.OfPlayer)
			{
				parms.raidStrategy.Worker.MakeLords(parms, list);
			}
			LessonAutoActivator.TeachOpportunity(ConceptDefOf.EquippingWeapons, OpportunityType.Critical);
			if (!PlayerKnowledgeDatabase.IsComplete(ConceptDefOf.ShieldBelts))
			{
				for (int j = 0; j < list.Count; j++)
				{
					Pawn pawn = list[j];
					if (pawn.apparel != null)
					{
						if (pawn.apparel.WornApparel.Any((Apparel ap) => ap.def == ThingDefOf.Apparel_ShieldBelt))
						{
							LessonAutoActivator.TeachOpportunity(ConceptDefOf.ShieldBelts, OpportunityType.Critical);
							break;
						}
					}
				}
			}
			if (DebugSettings.logRaidInfo)
			{
				Log.Message(string.Format("Raid: {0} ({1}) {2} {3} c={4} p={5}", new object[]
				{
					parms.faction.Name,
					parms.faction.def.defName,
					parms.raidArrivalMode.defName,
					parms.raidStrategy.defName,
					parms.spawnCenter,
					parms.points
				}));
			}
			return true;
		}

		// Token: 0x0600AB38 RID: 43832 RVA: 0x0030F218 File Offset: 0x0030D418
		public static float AdjustedRaidPoints(float points, PawnsArrivalModeDef raidArrivalMode, RaidStrategyDef raidStrategy, Faction faction, PawnGroupKindDef groupKind, IIncidentTarget target, RaidAgeRestrictionDef ageRestriction = null)
		{
			if (raidArrivalMode.pointsFactorCurve != null)
			{
				points *= raidArrivalMode.pointsFactorCurve.Evaluate(points);
			}
			if (raidStrategy.pointsFactorCurve != null)
			{
				points *= raidStrategy.pointsFactorCurve.Evaluate(points);
			}
			if (ageRestriction != null)
			{
				points *= ageRestriction.threatPointsFactor;
			}
			if (target.Tile.Valid)
			{
				points *= target.Tile.LayerDef.raidPointsFactor;
			}
			points = Mathf.Max(points, raidStrategy.Worker.MinimumPoints(faction, groupKind) * 1.05f);
			return points;
		}

		// Token: 0x0600AB39 RID: 43833 RVA: 0x0030F2AC File Offset: 0x0030D4AC
		public void DoTable_RaidFactionSampled()
		{
			int ticksGame = Find.TickManager.TicksGame;
			Find.TickManager.DebugSetTicksGame(36000000);
			Faction lastRaidFaction = Find.CurrentMap.StoryState.lastRaidFaction;
			List<TableDataGetter<Faction>> list = new List<TableDataGetter<Faction>>();
			list.Add(new TableDataGetter<Faction>("name", (Faction f) => f.Name + " (" + f.def.defName + ")"));
			foreach (float points in DebugActionsUtility.PointsOptions(false))
			{
				Dictionary<Faction, int> factionCount = new Dictionary<Faction, int>();
				foreach (Faction key in Find.FactionManager.AllFactions)
				{
					factionCount.Add(key, 0);
				}
				for (int i = 0; i < 500; i++)
				{
					IncidentParms incidentParms = new IncidentParms();
					incidentParms.target = Find.CurrentMap;
					incidentParms.points = points;
					if (this.TryResolveRaidFaction(incidentParms))
					{
						Dictionary<Faction, int> factionCount2 = factionCount;
						Faction faction = incidentParms.faction;
						int num = factionCount2[faction];
						factionCount2[faction] = num + 1;
						Find.CurrentMap.StoryState.lastRaidFaction = incidentParms.faction;
					}
				}
				list.Add(new TableDataGetter<Faction>(points.ToString("F0"), (Faction str) => ((float)factionCount[str] / 500f).ToStringPercent()));
			}
			Find.TickManager.DebugSetTicksGame(ticksGame);
			Find.CurrentMap.StoryState.lastRaidFaction = lastRaidFaction;
			DebugTables.MakeTablesDialog<Faction>(Find.FactionManager.AllFactions, list.ToArray());
		}

		// Token: 0x0600AB3A RID: 43834 RVA: 0x0030F49C File Offset: 0x0030D69C
		public void DoTable_RaidStrategySampled(Faction fac)
		{
			int ticksGame = Find.TickManager.TicksGame;
			Find.TickManager.DebugSetTicksGame(36000000);
			FactionRelationKind? factionRelationKind = null;
			int? num = null;
			if (fac != null && !fac.HostileTo(Faction.OfPlayer))
			{
				if (fac.HasGoodwill)
				{
					num = new int?(fac.GoodwillToMakeHostile(Faction.OfPlayer));
					fac.ChangeGoodwill_Debug(Faction.OfPlayer, num.Value);
				}
				else
				{
					factionRelationKind = new FactionRelationKind?(fac.RelationKindWith(Faction.OfPlayer));
					fac.SetRelationDirect(Faction.OfPlayer, FactionRelationKind.Hostile, false, null, null);
				}
			}
			List<TableDataGetter<RaidStrategyDef>> list = new List<TableDataGetter<RaidStrategyDef>>();
			list.Add(new TableDataGetter<RaidStrategyDef>("defName", (RaidStrategyDef d) => d.defName));
			foreach (float num2 in DebugActionsUtility.PointsOptions(false))
			{
				if (fac != null && num2 < fac.def.MinPointsToGeneratePawnGroup(PawnGroupKindDefOf.Combat, null))
				{
					list.Add(new TableDataGetter<RaidStrategyDef>(num2.ToString("F0"), (RaidStrategyDef str) => "0%"));
				}
				else
				{
					Dictionary<RaidStrategyDef, int> strats = new Dictionary<RaidStrategyDef, int>();
					foreach (RaidStrategyDef key in DefDatabase<RaidStrategyDef>.AllDefs)
					{
						strats.Add(key, 0);
					}
					for (int i = 0; i < 500; i++)
					{
						IncidentParms incidentParms = new IncidentParms();
						incidentParms.target = Find.CurrentMap;
						incidentParms.points = num2;
						incidentParms.faction = fac;
						if (this.TryResolveRaidFaction(incidentParms))
						{
							this.ResolveRaidStrategy(incidentParms, PawnGroupKindDefOf.Combat);
							Find.CurrentMap.StoryState.lastRaidFaction = incidentParms.faction;
							if (incidentParms.raidStrategy != null)
							{
								Dictionary<RaidStrategyDef, int> strats2 = strats;
								RaidStrategyDef raidStrategy = incidentParms.raidStrategy;
								int num3 = strats2[raidStrategy];
								strats2[raidStrategy] = num3 + 1;
							}
						}
					}
					list.Add(new TableDataGetter<RaidStrategyDef>(num2.ToString("F0"), (RaidStrategyDef str) => ((float)strats[str] / 500f).ToStringPercent()));
				}
			}
			Find.TickManager.DebugSetTicksGame(ticksGame);
			if (factionRelationKind != null)
			{
				fac.SetRelationDirect(Faction.OfPlayer, factionRelationKind.Value, false, null, null);
			}
			else if (num != null)
			{
				fac.ChangeGoodwill_Debug(Faction.OfPlayer, -num.Value);
			}
			DebugTables.MakeTablesDialog<RaidStrategyDef>(DefDatabase<RaidStrategyDef>.AllDefs, list.ToArray());
		}

		// Token: 0x0600AB3B RID: 43835 RVA: 0x0030F79C File Offset: 0x0030D99C
		public void DoTable_RaidArrivalModeSampled(Faction fac)
		{
			int ticksGame = Find.TickManager.TicksGame;
			Find.TickManager.DebugSetTicksGame(36000000);
			Faction lastRaidFaction = Find.CurrentMap.StoryState.lastRaidFaction;
			FactionRelationKind? factionRelationKind = null;
			int? num = null;
			if (fac != null && !fac.HostileTo(Faction.OfPlayer))
			{
				if (fac.HasGoodwill)
				{
					num = new int?(fac.GoodwillToMakeHostile(Faction.OfPlayer));
					fac.ChangeGoodwill_Debug(Faction.OfPlayer, num.Value);
				}
				else
				{
					factionRelationKind = new FactionRelationKind?(fac.RelationKindWith(Faction.OfPlayer));
					fac.SetRelationDirect(Faction.OfPlayer, FactionRelationKind.Hostile, false, null, null);
				}
			}
			List<TableDataGetter<PawnsArrivalModeDef>> list = new List<TableDataGetter<PawnsArrivalModeDef>>();
			list.Add(new TableDataGetter<PawnsArrivalModeDef>("mode", (PawnsArrivalModeDef f) => f.defName));
			foreach (float num2 in DebugActionsUtility.PointsOptions(false))
			{
				if (num2 < fac.def.MinPointsToGeneratePawnGroup(PawnGroupKindDefOf.Combat, null))
				{
					list.Add(new TableDataGetter<PawnsArrivalModeDef>(num2.ToString("F0"), (PawnsArrivalModeDef str) => "0%"));
				}
				else
				{
					Dictionary<PawnsArrivalModeDef, int> modeCount = new Dictionary<PawnsArrivalModeDef, int>();
					foreach (PawnsArrivalModeDef key in DefDatabase<PawnsArrivalModeDef>.AllDefs)
					{
						modeCount.Add(key, 0);
					}
					for (int i = 0; i < 500; i++)
					{
						IncidentParms incidentParms = new IncidentParms();
						incidentParms.target = Find.CurrentMap;
						incidentParms.points = num2;
						incidentParms.faction = fac;
						if (this.TryResolveRaidFaction(incidentParms))
						{
							Find.CurrentMap.storyState.lastRaidFaction = incidentParms.faction;
							this.ResolveRaidStrategy(incidentParms, PawnGroupKindDefOf.Combat);
							if (incidentParms.raidStrategy != null)
							{
								this.ResolveRaidArriveMode(incidentParms);
								Dictionary<PawnsArrivalModeDef, int> modeCount2 = modeCount;
								PawnsArrivalModeDef raidArrivalMode = incidentParms.raidArrivalMode;
								int num3 = modeCount2[raidArrivalMode];
								modeCount2[raidArrivalMode] = num3 + 1;
							}
						}
					}
					list.Add(new TableDataGetter<PawnsArrivalModeDef>(num2.ToString("F0"), (PawnsArrivalModeDef str) => ((float)modeCount[str] / 500f).ToStringPercent()));
				}
			}
			Find.TickManager.DebugSetTicksGame(ticksGame);
			Find.CurrentMap.storyState.lastRaidFaction = lastRaidFaction;
			if (factionRelationKind != null)
			{
				fac.SetRelationDirect(Faction.OfPlayer, factionRelationKind.Value, false, null, null);
			}
			else if (num != null)
			{
				fac.ChangeGoodwill_Debug(Faction.OfPlayer, -num.Value);
			}
			DebugTables.MakeTablesDialog<PawnsArrivalModeDef>(DefDatabase<PawnsArrivalModeDef>.AllDefs, list.ToArray());
		}
	}
}
