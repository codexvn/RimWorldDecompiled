using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02003D4C RID: 15692
	public static class CaravanExitMapUtility
	{
		// Token: 0x060155F5 RID: 87541 RVA: 0x00663F9C File Offset: 0x0066219C
		public static Caravan ExitMapAndCreateCaravan(IEnumerable<Pawn> pawns, Faction faction, PlanetTile exitFromTile, Direction8Way dir, PlanetTile destinationTile, bool sendMessage = true)
		{
			PlanetTile directionTile = CaravanExitMapUtility.FindRandomStartingTileBasedOnExitDir(exitFromTile, dir);
			return CaravanExitMapUtility.ExitMapAndCreateCaravan(pawns, faction, exitFromTile, directionTile, destinationTile, sendMessage);
		}

		// Token: 0x060155F6 RID: 87542 RVA: 0x00663FC0 File Offset: 0x006621C0
		public static Caravan ExitMapAndCreateCaravan(IEnumerable<Pawn> pawns, Faction faction, PlanetTile exitFromTile, PlanetTile directionTile, PlanetTile destinationTile, bool sendMessage = true)
		{
			if (!GenWorldClosest.TryFindClosestPassableTile(exitFromTile, out exitFromTile))
			{
				Log.Error("Could not find any passable tile for a new caravan.");
				return null;
			}
			if (Find.World.Impassable(directionTile))
			{
				directionTile = exitFromTile;
			}
			CaravanExitMapUtility.tmpPawns.Clear();
			CaravanExitMapUtility.tmpPawns.AddRange(pawns);
			Map map = null;
			for (int i = 0; i < CaravanExitMapUtility.tmpPawns.Count; i++)
			{
				CaravanExitMapUtility.AddCaravanExitTaleIfShould(CaravanExitMapUtility.tmpPawns[i]);
				map = CaravanExitMapUtility.tmpPawns[i].MapHeld;
				if (map != null)
				{
					break;
				}
			}
			Caravan caravan = CaravanMaker.MakeCaravan(CaravanExitMapUtility.tmpPawns, faction, exitFromTile, false);
			Rot4 exitDir = (map != null) ? Find.WorldGrid.GetRotFromTo(exitFromTile, directionTile) : Rot4.Invalid;
			for (int j = 0; j < CaravanExitMapUtility.tmpPawns.Count; j++)
			{
				CaravanExitMapUtility.tmpPawns[j].ExitMap(false, exitDir);
			}
			List<Pawn> pawnsListForReading = caravan.PawnsListForReading;
			for (int k = 0; k < pawnsListForReading.Count; k++)
			{
				if (!pawnsListForReading[k].IsWorldPawn())
				{
					Find.WorldPawns.PassToWorld(pawnsListForReading[k], PawnDiscardDecideMode.Decide);
				}
			}
			if (map != null)
			{
				map.Parent.Notify_CaravanFormed(caravan);
				map.retainedCaravanData.Notify_CaravanFormed(caravan);
			}
			if (!caravan.pather.Moving && caravan.Tile != directionTile)
			{
				caravan.pather.StartPath(directionTile, null, true, true);
				caravan.pather.nextTileCostLeft /= 2f;
				caravan.tweener.ResetTweenedPosToRoot();
			}
			if (destinationTile.Valid)
			{
				List<FloatMenuOption> list = FloatMenuMakerWorld.ChoicesAtFor(destinationTile, caravan);
				if (list.Any((FloatMenuOption x) => !x.Disabled))
				{
					list.First((FloatMenuOption x) => !x.Disabled).action();
				}
				else
				{
					caravan.pather.StartPath(destinationTile, null, true, true);
				}
			}
			if (sendMessage)
			{
				TaggedString taggedString = "MessageFormedCaravan".Translate(caravan.Name).CapitalizeFirst();
				if (caravan.pather.Moving && caravan.pather.ArrivalAction != null)
				{
					taggedString += " " + "MessageFormedCaravan_Orders".Translate() + ": " + caravan.pather.ArrivalAction.Label + ".";
				}
				Messages.Message(taggedString, caravan, MessageTypeDefOf.TaskCompletion, true);
			}
			return caravan;
		}

		// Token: 0x060155F7 RID: 87543 RVA: 0x00664260 File Offset: 0x00662460
		public static void ExitMapAndJoinOrCreateCaravan(Pawn pawn, Rot4 exitDir)
		{
			Caravan caravan = CaravanExitMapUtility.FindCaravanToJoinFor(pawn);
			if (caravan != null)
			{
				CaravanExitMapUtility.AddCaravanExitTaleIfShould(pawn);
				caravan.AddPawn(pawn, true);
				pawn.ExitMap(false, exitDir);
				return;
			}
			if (pawn.IsColonist || pawn.IsColonySubhumanPlayerControlled)
			{
				Map map = pawn.Map;
				PlanetTile directionTile = CaravanExitMapUtility.FindRandomStartingTileBasedOnExitDir(map.Tile, exitDir);
				Caravan caravan2 = CaravanExitMapUtility.ExitMapAndCreateCaravan(Gen.YieldSingle<Pawn>(pawn), pawn.Faction, map.Tile, directionTile, PlanetTile.Invalid, false);
				caravan2.autoJoinable = true;
				bool flag = false;
				IReadOnlyList<Pawn> allPawnsSpawned = map.mapPawns.AllPawnsSpawned;
				for (int i = 0; i < allPawnsSpawned.Count; i++)
				{
					if (CaravanExitMapUtility.FindCaravanToJoinFor(allPawnsSpawned[i]) != null && !allPawnsSpawned[i].Downed && !allPawnsSpawned[i].Drafted)
					{
						if (allPawnsSpawned[i].IsAnimal)
						{
							flag = true;
						}
						RestUtility.WakeUp(allPawnsSpawned[i], true);
						allPawnsSpawned[i].jobs.CheckForJobOverride(0f, true);
					}
				}
				TaggedString taggedString = "MessagePawnLeftMapAndCreatedCaravan".Translate(pawn.LabelShort, pawn).CapitalizeFirst();
				if (flag)
				{
					taggedString += " " + "MessagePawnLeftMapAndCreatedCaravan_AnimalsWantToJoin".Translate();
				}
				Messages.Message(taggedString, caravan2, MessageTypeDefOf.TaskCompletion, true);
				return;
			}
			Log.Error("Pawn " + ((pawn != null) ? pawn.ToString() : null) + " didn't find any caravan to join, and he can't create one.");
		}

		// Token: 0x060155F8 RID: 87544 RVA: 0x006643ED File Offset: 0x006625ED
		public static bool CanExitMapAndJoinOrCreateCaravanNow(Pawn pawn)
		{
			return pawn.Spawned && pawn.Map.exitMapGrid.MapUsesExitGrid && (pawn.IsColonist || pawn.IsColonySubhumanPlayerControlled || CaravanExitMapUtility.FindCaravanToJoinFor(pawn) != null);
		}

		// Token: 0x060155F9 RID: 87545 RVA: 0x00664428 File Offset: 0x00662628
		public static List<PlanetTile> AvailableExitTilesAt(Map map)
		{
			CaravanExitMapUtility.retTiles.Clear();
			PlanetTile currentTileID = map.Tile;
			World world = Find.World;
			WorldGrid grid = world.grid;
			grid.GetTileNeighbors(currentTileID, CaravanExitMapUtility.tmpNeighbors);
			Predicate<IntVec3> <>9__1;
			Predicate<IntVec3> <>9__2;
			for (int i = 0; i < CaravanExitMapUtility.tmpNeighbors.Count; i++)
			{
				PlanetTile planetTile = CaravanExitMapUtility.tmpNeighbors[i];
				if (CaravanExitMapUtility.IsGoodCaravanStartingTile(planetTile))
				{
					Rot4 rot;
					Rot4 rot2;
					CaravanExitMapUtility.GetExitMapEdges(grid, currentTileID, planetTile, out rot, out rot2);
					IntVec3 intVec;
					if (rot != Rot4.Invalid)
					{
						Predicate<IntVec3> validator;
						if ((validator = <>9__1) == null)
						{
							validator = (<>9__1 = ((IntVec3 x) => x.Walkable(map) && !x.Fogged(map)));
						}
						if (CellFinder.TryFindRandomEdgeCellWith(validator, map, rot, CellFinder.EdgeRoadChance_Ignore, out intVec))
						{
							goto IL_10E;
						}
					}
					if (!(rot2 != Rot4.Invalid))
					{
						goto IL_126;
					}
					Predicate<IntVec3> validator2;
					if ((validator2 = <>9__2) == null)
					{
						validator2 = (<>9__2 = ((IntVec3 x) => x.Walkable(map) && !x.Fogged(map)));
					}
					if (!CellFinder.TryFindRandomEdgeCellWith(validator2, map, rot2, CellFinder.EdgeRoadChance_Ignore, out intVec))
					{
						goto IL_126;
					}
					IL_10E:
					if (!CaravanExitMapUtility.retTiles.Contains(planetTile))
					{
						CaravanExitMapUtility.retTiles.Add(planetTile);
					}
				}
				IL_126:;
			}
			CaravanExitMapUtility.retTiles.SortBy((PlanetTile x) => grid.GetHeadingFromTo(currentTileID, x));
			return CaravanExitMapUtility.retTiles;
		}

		// Token: 0x060155FA RID: 87546 RVA: 0x0066458C File Offset: 0x0066278C
		public static void GetExitMapEdges(WorldGrid grid, PlanetTile fromTile, PlanetTile toTile, out Rot4 primary, out Rot4 secondary)
		{
			primary = (secondary = Rot4.Invalid);
			int num = 0;
			float heading = grid.GetHeadingFromTo(fromTile, toTile);
			if (heading >= 292.5f || heading <= 67.5f)
			{
				CaravanExitMapUtility.rotTmp[num++] = Rot4.North;
			}
			else if (heading >= 112.5f && heading <= 247.5f)
			{
				CaravanExitMapUtility.rotTmp[num++] = Rot4.South;
			}
			if (heading >= 22.5f && heading <= 157.5f)
			{
				CaravanExitMapUtility.rotTmp[num++] = Rot4.East;
			}
			else if (heading >= 202.5f && heading <= 337.5f)
			{
				CaravanExitMapUtility.rotTmp[num++] = Rot4.West;
			}
			Array.Sort<Rot4>(CaravanExitMapUtility.rotTmp, (Rot4 r1, Rot4 r2) => Mathf.Abs(r1.AsAngle - heading).CompareTo(Mathf.Abs(r2.AsAngle - heading)));
			if (num >= 1)
			{
				primary = CaravanExitMapUtility.rotTmp[0];
			}
			if (num >= 2)
			{
				secondary = CaravanExitMapUtility.rotTmp[1];
			}
		}

		// Token: 0x060155FB RID: 87547 RVA: 0x006646C4 File Offset: 0x006628C4
		public static PlanetTile RandomBestExitTileFrom(Map map)
		{
			Tile tileInfo = map.TileInfo;
			List<PlanetTile> options = CaravanExitMapUtility.AvailableExitTilesAt(map);
			if (!options.Any<PlanetTile>())
			{
				return PlanetTile.Invalid;
			}
			SurfaceTile surface = tileInfo as SurfaceTile;
			if (surface == null || surface.Roads == null)
			{
				return options.RandomElement<PlanetTile>();
			}
			int bestRoadIndex = -1;
			for (int i = 0; i < surface.Roads.Count; i++)
			{
				if (options.Contains(surface.Roads[i].neighbor) && (bestRoadIndex == -1 || surface.Roads[i].road.priority > surface.Roads[bestRoadIndex].road.priority))
				{
					bestRoadIndex = i;
				}
			}
			if (bestRoadIndex == -1)
			{
				return options.RandomElement<PlanetTile>();
			}
			return (from rl in surface.Roads
			where options.Contains(rl.neighbor) && rl.road == surface.Roads[bestRoadIndex].road
			select rl).RandomElement<SurfaceTile.RoadLink>().neighbor;
		}

		// Token: 0x060155FC RID: 87548 RVA: 0x00664800 File Offset: 0x00662A00
		public static PlanetTile BestExitTileToGoTo(PlanetTile destinationTile, Map from)
		{
			PlanetTile planetTile = PlanetTile.Invalid;
			using (WorldPath worldPath = from.Tile.Layer.Pather.FindPath(from.Tile, destinationTile, null, null))
			{
				if (worldPath.Found && worldPath.NodesLeftCount >= 2)
				{
					List<PlanetTile> nodesReversed = worldPath.NodesReversed;
					planetTile = nodesReversed[nodesReversed.Count - 2];
				}
			}
			if (!planetTile.Valid)
			{
				return CaravanExitMapUtility.RandomBestExitTileFrom(from);
			}
			float num = 0f;
			PlanetTile result = PlanetTile.Invalid;
			List<PlanetTile> list = CaravanExitMapUtility.AvailableExitTilesAt(from);
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i] == planetTile)
				{
					return list[i];
				}
				float num2 = (Find.WorldGrid.GetTileCenter(list[i]) - Find.WorldGrid.GetTileCenter(planetTile)).MagnitudeHorizontalSquared();
				if (!result.Valid || num2 < num)
				{
					result = list[i];
					num = num2;
				}
			}
			return result;
		}

		// Token: 0x060155FD RID: 87549 RVA: 0x00664914 File Offset: 0x00662B14
		private static PlanetTile FindRandomStartingTileBasedOnExitDir(PlanetTile tileID, Rot4 exitDir)
		{
			CaravanExitMapUtility.tileCandidates.Clear();
			World world = Find.World;
			WorldGrid grid = world.grid;
			grid.GetTileNeighbors(tileID, CaravanExitMapUtility.tmpNeighbors);
			for (int i = 0; i < CaravanExitMapUtility.tmpNeighbors.Count; i++)
			{
				PlanetTile planetTile = CaravanExitMapUtility.tmpNeighbors[i];
				if (CaravanExitMapUtility.IsGoodCaravanStartingTile(planetTile) && (!exitDir.IsValid || !(grid.GetRotFromTo(tileID, planetTile) != exitDir)))
				{
					CaravanExitMapUtility.tileCandidates.Add(planetTile);
				}
			}
			PlanetTile result;
			if (CaravanExitMapUtility.tileCandidates.TryRandomElement(out result))
			{
				return result;
			}
			if (CaravanExitMapUtility.tmpNeighbors.Where(delegate(PlanetTile x)
			{
				if (!CaravanExitMapUtility.IsGoodCaravanStartingTile(x))
				{
					return false;
				}
				Rot4 rotFromTo = grid.GetRotFromTo(tileID, x);
				return ((exitDir == Rot4.North || exitDir == Rot4.South) && (rotFromTo == Rot4.East || rotFromTo == Rot4.West)) || ((exitDir == Rot4.East || exitDir == Rot4.West) && (rotFromTo == Rot4.North || rotFromTo == Rot4.South));
			}).TryRandomElement(out result))
			{
				return result;
			}
			if ((from x in CaravanExitMapUtility.tmpNeighbors
			where CaravanExitMapUtility.IsGoodCaravanStartingTile(x)
			select x).TryRandomElement(out result))
			{
				return result;
			}
			return tileID;
		}

		// Token: 0x060155FE RID: 87550 RVA: 0x00664A38 File Offset: 0x00662C38
		private static PlanetTile FindRandomStartingTileBasedOnExitDir(PlanetTile tileID, Direction8Way exitDir)
		{
			CaravanExitMapUtility.tileCandidates.Clear();
			WorldGrid grid = Find.World.grid;
			grid.GetTileNeighbors(tileID, CaravanExitMapUtility.tmpNeighbors);
			for (int i = 0; i < CaravanExitMapUtility.tmpNeighbors.Count; i++)
			{
				PlanetTile planetTile = CaravanExitMapUtility.tmpNeighbors[i];
				if (CaravanExitMapUtility.IsGoodCaravanStartingTile(planetTile) && grid.GetDirection8WayFromTo(tileID, planetTile) == exitDir)
				{
					CaravanExitMapUtility.tileCandidates.Add(planetTile);
				}
			}
			PlanetTile result;
			if (CaravanExitMapUtility.tileCandidates.TryRandomElement(out result))
			{
				return result;
			}
			if (CaravanExitMapUtility.tmpNeighbors.Where(new Func<PlanetTile, bool>(CaravanExitMapUtility.IsGoodCaravanStartingTile)).TryRandomElement(out result))
			{
				return result;
			}
			return tileID;
		}

		// Token: 0x060155FF RID: 87551 RVA: 0x00664AD7 File Offset: 0x00662CD7
		private static bool IsGoodCaravanStartingTile(PlanetTile tile)
		{
			return !Find.World.Impassable(tile);
		}

		// Token: 0x06015600 RID: 87552 RVA: 0x00664AE8 File Offset: 0x00662CE8
		public static Caravan FindCaravanToJoinFor(Pawn pawn)
		{
			if (pawn.Faction != Faction.OfPlayer && pawn.HostFaction != Faction.OfPlayer)
			{
				return null;
			}
			if (!pawn.Spawned || !pawn.CanReachMapEdge())
			{
				return null;
			}
			if (pawn.Map.IsPocketMap)
			{
				return null;
			}
			PlanetTile tile = pawn.Map.Tile;
			Find.WorldGrid.GetTileNeighbors(tile, CaravanExitMapUtility.tmpNeighbors);
			CaravanExitMapUtility.tmpNeighbors.Add(tile);
			List<Caravan> caravans = Find.WorldObjects.Caravans;
			for (int i = 0; i < caravans.Count; i++)
			{
				Caravan caravan = caravans[i];
				if (CaravanExitMapUtility.tmpNeighbors.Contains(caravan.Tile) && caravan.autoJoinable)
				{
					if (pawn.GetMechWorkMode() == MechWorkModeDefOf.Escort)
					{
						if (caravan.PawnsListForReading.Contains(pawn.GetOverseer()))
						{
							return caravan;
						}
					}
					else if (pawn.HostFaction == null)
					{
						if (caravan.Faction == pawn.Faction)
						{
							return caravan;
						}
					}
					else if (caravan.Faction == pawn.HostFaction)
					{
						return caravan;
					}
				}
			}
			return null;
		}

		// Token: 0x06015601 RID: 87553 RVA: 0x00664BE4 File Offset: 0x00662DE4
		public static bool AnyoneTryingToJoinCaravan(Caravan c)
		{
			List<Map> maps = Find.Maps;
			for (int i = 0; i < maps.Count; i++)
			{
				Map map = maps[i];
				if (!map.IsPlayerHome && Find.WorldGrid.IsNeighborOrSame(c.Tile, map.Tile))
				{
					IReadOnlyList<Pawn> allPawnsSpawned = map.mapPawns.AllPawnsSpawned;
					for (int j = 0; j < allPawnsSpawned.Count; j++)
					{
						if (!allPawnsSpawned[j].IsColonistPlayerControlled && !allPawnsSpawned[j].Downed && CaravanExitMapUtility.FindCaravanToJoinFor(allPawnsSpawned[j]) == c)
						{
							return true;
						}
					}
				}
			}
			return false;
		}

		// Token: 0x06015602 RID: 87554 RVA: 0x00664C85 File Offset: 0x00662E85
		public static void OpenSomeoneTryingToJoinCaravanDialog(Caravan c, Action confirmAction)
		{
			Find.WindowStack.Add(Dialog_MessageBox.CreateConfirmation("ConfirmMoveAutoJoinableCaravan".Translate(), confirmAction, false, null, WindowLayer.Dialog));
		}

		// Token: 0x06015603 RID: 87555 RVA: 0x00664CA4 File Offset: 0x00662EA4
		private static void AddCaravanExitTaleIfShould(Pawn pawn)
		{
			if (pawn.Spawned && pawn.IsFreeColonist)
			{
				if (pawn.Map.IsPlayerHome)
				{
					TaleRecorder.RecordTale(TaleDefOf.CaravanFormed, new object[]
					{
						pawn
					});
					return;
				}
				if (GenHostility.AnyHostileActiveThreatToPlayer(pawn.Map, false, false))
				{
					TaleRecorder.RecordTale(TaleDefOf.CaravanFled, new object[]
					{
						pawn
					});
				}
			}
		}

		// Token: 0x0400EDFE RID: 60926
		private static readonly List<PlanetTile> tmpNeighbors = new List<PlanetTile>();

		// Token: 0x0400EDFF RID: 60927
		private static readonly List<Pawn> tmpPawns = new List<Pawn>();

		// Token: 0x0400EE00 RID: 60928
		private static readonly List<PlanetTile> retTiles = new List<PlanetTile>();

		// Token: 0x0400EE01 RID: 60929
		private static readonly Rot4[] rotTmp = new Rot4[2];

		// Token: 0x0400EE02 RID: 60930
		private static readonly List<PlanetTile> tileCandidates = new List<PlanetTile>();
	}
}
