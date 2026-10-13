using System;
using System.Collections.Generic;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02001ACD RID: 6861
	public abstract class RaidStrategyWorker
	{
		// Token: 0x06009992 RID: 39314 RVA: 0x002CB904 File Offset: 0x002C9B04
		public float SelectionWeightForFaction(Map map, Faction faction, float basePoints)
		{
			if (faction != null && this.def.selectionWeightCurvesPerFaction != null)
			{
				List<FactionCurve> selectionWeightCurvesPerFaction = this.def.selectionWeightCurvesPerFaction;
				for (int i = 0; i < selectionWeightCurvesPerFaction.Count; i++)
				{
					if (selectionWeightCurvesPerFaction[i].faction == faction.def)
					{
						return selectionWeightCurvesPerFaction[i].Evaluate(basePoints);
					}
				}
			}
			return this.SelectionWeight(map, basePoints);
		}

		// Token: 0x06009993 RID: 39315 RVA: 0x002CB968 File Offset: 0x002C9B68
		public virtual float SelectionWeight(Map map, float basePoints)
		{
			return this.def.selectionWeightPerPointsCurve.Evaluate(basePoints);
		}

		// Token: 0x06009994 RID: 39316
		protected abstract LordJob MakeLordJob(IncidentParms parms, Map map, List<Pawn> pawns, int raidSeed);

		// Token: 0x06009995 RID: 39317 RVA: 0x002CB97C File Offset: 0x002C9B7C
		public virtual void MakeLords(IncidentParms parms, List<Pawn> pawns)
		{
			Map map = (Map)parms.target;
			List<List<Pawn>> list = IncidentParmsUtility.SplitIntoGroups(pawns, parms.pawnGroups);
			int @int = Rand.Int;
			for (int i = 0; i < list.Count; i++)
			{
				List<Pawn> list2 = list[i];
				Lord lord = LordMaker.MakeNewLord(parms.faction, this.MakeLordJob(parms, map, list2, @int), map, list2);
				lord.inSignalLeave = parms.inSignalEnd;
				QuestUtility.AddQuestTag(lord, parms.questTag);
				if (DebugViewSettings.drawStealDebug && parms.faction.HostileTo(Faction.OfPlayer))
				{
					Log.Message(string.Concat(new string[]
					{
						"Market value threshold to start stealing (raiders=",
						lord.ownedPawns.Count.ToString(),
						"): ",
						StealAIUtility.StartStealingMarketValueThreshold(lord).ToString(),
						" (colony wealth=",
						map.wealthWatcher.WealthTotal.ToString(),
						")"
					}));
				}
			}
		}

		// Token: 0x06009996 RID: 39318 RVA: 0x002CBA8C File Offset: 0x002C9C8C
		public virtual bool CanUseWith(IncidentParms parms, PawnGroupKindDef groupKind)
		{
			if (parms.faction != null && parms.faction.def.disallowedRaidStrategies.NotNullAndContains(this.def))
			{
				return false;
			}
			if (this.SelectionWeightForFaction(parms.target as Map, parms.faction, parms.points) <= 0f)
			{
				return false;
			}
			if (groupKind != null && parms.points < this.MinimumPoints(parms.faction, groupKind))
			{
				return false;
			}
			Map map = parms.target as Map;
			if (map != null)
			{
				using (IEnumerator<TileMutatorDef> enumerator = map.TileInfo.Mutators.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (enumerator.Current.blacklistedRaidStrategies.NotNullAndContains(this.def))
						{
							return false;
						}
					}
				}
				if (!map.Tile.Valid)
				{
					return true;
				}
				if (!this.def.layerWhitelist.NullOrEmpty<PlanetLayerDef>() && !this.def.layerWhitelist.Contains(map.Tile.LayerDef))
				{
					return false;
				}
				if (!this.def.layerBlacklist.NullOrEmpty<PlanetLayerDef>() && this.def.layerBlacklist.Contains(map.Tile.LayerDef))
				{
					return false;
				}
				return true;
			}
			return true;
		}

		// Token: 0x06009997 RID: 39319 RVA: 0x002CBBE4 File Offset: 0x002C9DE4
		public virtual float MinimumPoints(Faction faction, PawnGroupKindDef groupKind)
		{
			return faction.def.MinPointsToGeneratePawnGroup(groupKind, null);
		}

		// Token: 0x06009998 RID: 39320 RVA: 0x00006AAC File Offset: 0x00004CAC
		public virtual float MinMaxAllowedPawnGenOptionCost(Faction faction, PawnGroupKindDef groupKind)
		{
			return 0f;
		}

		// Token: 0x06009999 RID: 39321 RVA: 0x002CBBF4 File Offset: 0x002C9DF4
		public virtual bool CanUsePawnGenOption(float pointsTotal, PawnGenOption g, List<PawnGenOptionWithXenotype> chosenGroups, Faction faction = null)
		{
			if (faction != null && faction.def.humanlikeFaction && g.kind.RaceProps.Animal && chosenGroups != null)
			{
				if (!chosenGroups.Any((PawnGenOptionWithXenotype x) => x.Option.kind.RaceProps.Humanlike))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600999A RID: 39322 RVA: 0x000028E7 File Offset: 0x00000AE7
		public virtual bool CanUsePawn(float pointsTotal, Pawn p, List<Pawn> otherPawns)
		{
			return true;
		}

		// Token: 0x0600999B RID: 39323 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void TryGenerateThreats(IncidentParms parms)
		{
		}

		// Token: 0x0600999C RID: 39324 RVA: 0x002CBC54 File Offset: 0x002C9E54
		public virtual List<Pawn> SpawnThreats(IncidentParms parms)
		{
			if (parms.pawnKind != null)
			{
				List<Pawn> list = new List<Pawn>();
				for (int i = 0; i < parms.pawnCount; i++)
				{
					PawnKindDef pawnKind = parms.pawnKind;
					Faction faction = parms.faction;
					PawnGenerationContext context = PawnGenerationContext.NonPlayer;
					float biocodeWeaponsChance = parms.biocodeWeaponsChance;
					float biocodeApparelChance = parms.biocodeApparelChance;
					bool pawnsCanBringFood = this.def.pawnsCanBringFood;
					Pawn pawn = PawnGenerator.GeneratePawn(new PawnGenerationRequest(pawnKind, faction, context, null, false, false, false, true, true, 1f, false, true, false, pawnsCanBringFood, true, false, false, false, false, biocodeWeaponsChance, biocodeApparelChance, null, 1f, null, null, null, null, null, null, null, null, null, null, null, null, false, false, false, false, null, null, null, null, null, 0f, DevelopmentalStage.Adult, null, null, null, false, false, false, -1, 0, false)
					{
						BiocodeApparelChance = 1f
					});
					if (pawn != null)
					{
						list.Add(pawn);
					}
				}
				if (list.Any<Pawn>())
				{
					parms.raidArrivalMode.Worker.Arrive(list, parms);
					return list;
				}
			}
			return null;
		}

		// Token: 0x0400691A RID: 26906
		public RaidStrategyDef def;
	}
}
