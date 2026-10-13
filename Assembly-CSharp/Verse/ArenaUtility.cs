using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;
using Verse.AI.Group;

namespace Verse
{
	// Token: 0x02000C95 RID: 3221
	public static class ArenaUtility
	{
		// Token: 0x060052DC RID: 21212 RVA: 0x001AB4EF File Offset: 0x001A96EF
		public static bool ValidateArenaCapability()
		{
			if (Find.World.info.planetCoverage < 0.299f)
			{
				Log.Error("Planet coverage must be 30%+ to ensure a representative mix of biomes.");
				return false;
			}
			return true;
		}

		// Token: 0x060052DD RID: 21213 RVA: 0x001AB514 File Offset: 0x001A9714
		public static void BeginArenaFight(List<PawnKindDef> lhs, List<PawnKindDef> rhs, Action<ArenaUtility.ArenaResult> callback)
		{
			MapParent mapParent = (MapParent)WorldObjectMaker.MakeWorldObject(WorldObjectDefOf.Debug_Arena);
			mapParent.Tile = TileFinder.RandomSettlementTileFor(Faction.OfPlayer, true, (PlanetTile tile) => lhs.Concat(rhs).Any((PawnKindDef pawnkind) => Find.World.tileTemperatures.SeasonAndOutdoorTemperatureAcceptableFor(tile, pawnkind.race)));
			mapParent.SetFaction(Faction.OfPlayer);
			Find.WorldObjects.Add(mapParent);
			Map orGenerateMap = GetOrGenerateMapUtility.GetOrGenerateMap(mapParent.Tile, new IntVec3(50, 1, 50), null, null, false);
			IntVec3 spot;
			IntVec3 spot2;
			MultipleCaravansCellFinder.FindStartingCellsFor2Groups(orGenerateMap, out spot, out spot2);
			List<Pawn> lhs2 = ArenaUtility.SpawnPawnSet(orGenerateMap, lhs, spot, Faction.OfAncients);
			List<Pawn> rhs2 = ArenaUtility.SpawnPawnSet(orGenerateMap, rhs, spot2, Faction.OfAncientsHostile);
			DebugArena component = mapParent.GetComponent<DebugArena>();
			component.lhs = lhs2;
			component.rhs = rhs2;
			component.callback = callback;
		}

		// Token: 0x060052DE RID: 21214 RVA: 0x001AB5E0 File Offset: 0x001A97E0
		public static List<Pawn> SpawnPawnSet(Map map, List<PawnKindDef> kinds, IntVec3 spot, Faction faction)
		{
			List<Pawn> list = new List<Pawn>();
			for (int i = 0; i < kinds.Count; i++)
			{
				Pawn pawn = PawnGenerator.GeneratePawn(kinds[i], faction, null);
				IntVec3 loc = CellFinder.RandomClosewalkCellNear(spot, map, 12, null);
				GenSpawn.Spawn(pawn, loc, map, Rot4.Random, WipeMode.Vanish, false, false);
				list.Add(pawn);
			}
			LordMaker.MakeNewLord(faction, new LordJob_DefendPoint(map.Center, null, null, false, true), map, list);
			return list;
		}

		// Token: 0x060052DF RID: 21215 RVA: 0x001AB66C File Offset: 0x001A986C
		private static bool ArenaFightQueue(List<PawnKindDef> lhs, List<PawnKindDef> rhs, Action<ArenaUtility.ArenaResult> callback, ArenaUtility.ArenaSetState state)
		{
			if (!ArenaUtility.ValidateArenaCapability())
			{
				return false;
			}
			if (state.live < 15)
			{
				ArenaUtility.BeginArenaFight(lhs, rhs, delegate(ArenaUtility.ArenaResult result)
				{
					state.live--;
					callback(result);
				});
				state.live++;
				return true;
			}
			return false;
		}

		// Token: 0x060052E0 RID: 21216 RVA: 0x001AB6D0 File Offset: 0x001A98D0
		public static void BeginArenaFightSet(int count, List<PawnKindDef> lhs, List<PawnKindDef> rhs, Action<ArenaUtility.ArenaResult> callback, Action report)
		{
			if (!ArenaUtility.ValidateArenaCapability())
			{
				return;
			}
			int remaining = count;
			ArenaUtility.ArenaSetState state = new ArenaUtility.ArenaSetState();
			Action<ArenaUtility.ArenaResult> <>9__1;
			Func<bool> <>9__0;
			for (int i = 0; i < count; i++)
			{
				GameComponent_DebugTools component = Current.Game.GetComponent<GameComponent_DebugTools>();
				Func<bool> callback2;
				if ((callback2 = <>9__0) == null)
				{
					callback2 = (<>9__0 = delegate()
					{
						List<PawnKindDef> lhs2 = lhs;
						List<PawnKindDef> rhs2 = rhs;
						Action<ArenaUtility.ArenaResult> callback3;
						if ((callback3 = <>9__1) == null)
						{
							callback3 = (<>9__1 = delegate(ArenaUtility.ArenaResult result)
							{
								callback(result);
								int remaining;
								remaining--;
								remaining = remaining;
								if (remaining % 10 == 0)
								{
									report();
								}
							});
						}
						return ArenaUtility.ArenaFightQueue(lhs2, rhs2, callback3, state);
					});
				}
				component.AddPerFrameCallback(callback2);
			}
		}

		// Token: 0x060052E1 RID: 21217 RVA: 0x001AB754 File Offset: 0x001A9954
		public static void PerformBattleRoyale(IEnumerable<PawnKindDef> kindsEnumerable)
		{
			if (!ArenaUtility.ValidateArenaCapability())
			{
				return;
			}
			List<PawnKindDef> kinds = kindsEnumerable.ToList<PawnKindDef>();
			Dictionary<PawnKindDef, float> ratings = new Dictionary<PawnKindDef, float>();
			foreach (PawnKindDef pawnKindDef in kinds)
			{
				ratings[pawnKindDef] = EloUtility.CalculateRating(pawnKindDef.combatPower, 1500f, 60f);
			}
			int currentFights = 0;
			int completeFights = 0;
			Current.Game.GetComponent<GameComponent_DebugTools>().AddPerFrameCallback(delegate
			{
				int currentFights;
				if (currentFights >= 15)
				{
					return false;
				}
				PawnKindDef lhsDef = kinds.RandomElement<PawnKindDef>();
				PawnKindDef rhsDef = kinds.RandomElement<PawnKindDef>();
				float num = EloUtility.CalculateExpectation(ratings[lhsDef], ratings[rhsDef]);
				float num2 = 1f - num;
				float num3 = num;
				float num4 = Mathf.Min(num2, num3);
				num2 /= num4;
				num3 /= num4;
				float num5 = Mathf.Max(num2, num3);
				if (num5 > 40f)
				{
					return false;
				}
				float num6 = 40f / num5;
				float num7 = (float)Math.Exp((double)Rand.Range(0f, (float)Math.Log((double)num6)));
				num2 *= num7;
				num3 *= num7;
				List<PawnKindDef> lhs = Enumerable.Repeat<PawnKindDef>(lhsDef, GenMath.RoundRandom(num2)).ToList<PawnKindDef>();
				List<PawnKindDef> rhs = Enumerable.Repeat<PawnKindDef>(rhsDef, GenMath.RoundRandom(num3)).ToList<PawnKindDef>();
				currentFights++;
				currentFights = currentFights;
				ArenaUtility.BeginArenaFight(lhs, rhs, delegate(ArenaUtility.ArenaResult result)
				{
					int num8 = currentFights - 1;
					currentFights = num8;
					num8 = completeFights + 1;
					completeFights = num8;
					if (result.winner != ArenaUtility.ArenaResult.Winner.Other)
					{
						float value = ratings[lhsDef];
						float value2 = ratings[rhsDef];
						float kfactor = 8f * Mathf.Pow(0.5f, Time.realtimeSinceStartup / 900f);
						EloUtility.Update(ref value, ref value2, 0.5f, (float)((result.winner == ArenaUtility.ArenaResult.Winner.Lhs) ? 1 : 0), kfactor);
						ratings[lhsDef] = value;
						ratings[rhsDef] = value2;
						Log.Message(string.Format("Scores after {0} trials:\n\n{1}", completeFights, (from v in ratings
						orderby v.Value
						select string.Format("  {0}: {1}->{2} (rating {2})", new object[]
						{
							v.Key.label,
							v.Key.combatPower,
							EloUtility.CalculateLinearScore(v.Value, 1500f, 60f).ToString("F0"),
							v.Value.ToString("F0")
						})).ToLineList(null, false)));
					}
				});
				return false;
			});
		}

		// Token: 0x04003B1E RID: 15134
		private const int liveSimultaneous = 15;

		// Token: 0x02000C96 RID: 3222
		public struct ArenaResult
		{
			// Token: 0x04003B1F RID: 15135
			public ArenaUtility.ArenaResult.Winner winner;

			// Token: 0x04003B20 RID: 15136
			public int tickDuration;

			// Token: 0x02000C97 RID: 3223
			public enum Winner
			{
				// Token: 0x04003B22 RID: 15138
				Other,
				// Token: 0x04003B23 RID: 15139
				Lhs,
				// Token: 0x04003B24 RID: 15140
				Rhs
			}
		}

		// Token: 0x02000C98 RID: 3224
		private class ArenaSetState
		{
			// Token: 0x04003B25 RID: 15141
			public int live;
		}
	}
}
