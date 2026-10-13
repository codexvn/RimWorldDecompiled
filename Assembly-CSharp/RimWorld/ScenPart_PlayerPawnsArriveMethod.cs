using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.SketchGen;
using Verse;

namespace RimWorld
{
	// Token: 0x020028D9 RID: 10457
	public class ScenPart_PlayerPawnsArriveMethod : ScenPart
	{
		// Token: 0x0600E4A2 RID: 58530 RVA: 0x0042C42A File Offset: 0x0042A62A
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<PlayerPawnsArriveMethod>(ref this.method, "method", PlayerPawnsArriveMethod.Standing, false);
		}

		// Token: 0x0600E4A3 RID: 58531 RVA: 0x0042C444 File Offset: 0x0042A644
		public override void DoEditInterface(Listing_ScenEdit listing)
		{
			if (Widgets.ButtonText(listing.GetScenPartRect(this, ScenPart.RowHeight), this.method.ToStringHuman(), true, true, true, null))
			{
				List<FloatMenuOption> list = new List<FloatMenuOption>();
				foreach (object obj in Enum.GetValues(typeof(PlayerPawnsArriveMethod)))
				{
					PlayerPawnsArriveMethod playerPawnsArriveMethod = (PlayerPawnsArriveMethod)obj;
					if (playerPawnsArriveMethod != PlayerPawnsArriveMethod.Gravship || ModsConfig.OdysseyActive)
					{
						PlayerPawnsArriveMethod localM = playerPawnsArriveMethod;
						list.Add(new FloatMenuOption(localM.ToStringHuman(), delegate()
						{
							this.method = localM;
						}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
					}
				}
				Find.WindowStack.Add(new FloatMenu(list));
			}
		}

		// Token: 0x0600E4A4 RID: 58532 RVA: 0x0042C538 File Offset: 0x0042A738
		public override string Summary(Scenario scen)
		{
			if (this.method == PlayerPawnsArriveMethod.DropPods)
			{
				return "ScenPart_ArriveInDropPods".Translate();
			}
			return null;
		}

		// Token: 0x0600E4A5 RID: 58533 RVA: 0x0042C554 File Offset: 0x0042A754
		public override void Randomize()
		{
			this.method = ((Rand.Value < 0.5f) ? PlayerPawnsArriveMethod.DropPods : PlayerPawnsArriveMethod.Standing);
		}

		// Token: 0x0600E4A6 RID: 58534 RVA: 0x0042C56C File Offset: 0x0042A76C
		public override void GenerateIntoMap(Map map)
		{
			if (Find.GameInitData == null)
			{
				return;
			}
			List<Thing> list = new List<Thing>();
			foreach (ScenPart scenPart in Find.Scenario.AllParts)
			{
				list.AddRange(scenPart.PlayerStartingThings());
			}
			foreach (Pawn key in Find.GameInitData.startingAndOptionalPawns)
			{
				foreach (ThingDefCount t in Find.GameInitData.startingPossessions[key])
				{
					list.Add(StartingPawnUtility.GenerateStartingPossession(t));
				}
			}
			if (this.method == PlayerPawnsArriveMethod.Gravship && ModsConfig.OdysseyActive)
			{
				this.DoGravship(map, list);
				return;
			}
			this.DoDropPods(map, list);
		}

		// Token: 0x0600E4A7 RID: 58535 RVA: 0x0042C688 File Offset: 0x0042A888
		private void DoDropPods(Map map, List<Thing> startingItems)
		{
			List<List<Thing>> list = new List<List<Thing>>();
			foreach (Pawn item in Find.GameInitData.startingAndOptionalPawns)
			{
				list.Add(new List<Thing>
				{
					item
				});
			}
			int num = 0;
			foreach (Thing thing in startingItems)
			{
				if (thing.def.CanHaveFaction)
				{
					thing.SetFactionDirect(Faction.OfPlayer);
				}
				list[num].Add(thing);
				num++;
				if (num >= list.Count)
				{
					num = 0;
				}
			}
			DropPodUtility.DropThingGroupsNear(MapGenerator.PlayerStartSpot, map, list, 110, Find.GameInitData.QuickStarted || this.method != PlayerPawnsArriveMethod.DropPods, true, true, true, false, false, null);
		}

		// Token: 0x0600E4A8 RID: 58536 RVA: 0x0042C798 File Offset: 0x0042A998
		private void DoGravship(Map map, List<Thing> startingItems)
		{
			SketchResolveParams parms = default(SketchResolveParams);
			parms.sketch = new Sketch();
			Sketch sketch = SketchGen.Generate(SketchResolverDefOf.Gravship, parms);
			sketch.Rotate(Rot4.Random);
			HashSet<IntVec3> hashSet = (from c in sketch.OccupiedRect.Cells
			select c - sketch.OccupiedCenter).ToHashSet<IntVec3>();
			List<CellRect> orGenerateVar = MapGenerator.GetOrGenerateVar<List<CellRect>>("UsedRects");
			map.regionAndRoomUpdater.Enabled = true;
			IntVec3 playerStartSpot = MapGenerator.PlayerStartSpot;
			if (!MapGenerator.PlayerStartSpotValid)
			{
				GenStep_ReserveGravshipArea.SetStartSpot(map, hashSet, orGenerateVar);
				playerStartSpot = MapGenerator.PlayerStartSpot;
			}
			GravshipPlacementUtility.ClearAreaForGravship(map, playerStartSpot, hashSet);
			List<Thing> list = new List<Thing>();
			sketch.Spawn(map, playerStartSpot, Faction.OfPlayer, Sketch.SpawnPosType.OccupiedCenter, Sketch.SpawnMode.Normal, true, true, true, list, false, true, null, null, null);
			IntVec3 offset = playerStartSpot - sketch.OccupiedCenter;
			CellRect cellRect = sketch.OccupiedRect.MovedBy(offset);
			orGenerateVar.Add(cellRect);
			Predicate<IntVec3> <>9__1;
			foreach (Pawn pawn in Find.GameInitData.startingAndOptionalPawns)
			{
				IEnumerable<IntVec3> source = cellRect;
				Predicate<IntVec3> predicate;
				if ((predicate = <>9__1) == null)
				{
					predicate = (<>9__1 = delegate(IntVec3 c)
					{
						if (c.Standable(map))
						{
							TerrainDef terrain = c.GetTerrain(map);
							return terrain != null && terrain.IsSubstructure;
						}
						return false;
					});
				}
				IntVec3 center;
				if (!source.TryRandomElement(predicate, out center))
				{
					string str = "Could not find a valid spawn location for pawn ";
					Name name = pawn.Name;
					Log.Error(str + ((name != null) ? name.ToString() : null));
				}
				else
				{
					GenPlace.TryPlaceThing(pawn, center, map, ThingPlaceMode.Near, null, null, null, 1);
				}
			}
			foreach (Thing thing in startingItems)
			{
				if (thing.def.CanHaveFaction)
				{
					thing.SetFactionDirect(Faction.OfPlayer);
				}
				int num = thing.stackCount;
				int num2 = 99;
				while (num > 0 && num2-- > 0)
				{
					Thing t2;
					if ((from t in list
					where t.def == ThingDefOf.Shelf || t.def == ThingDefOf.ShelfSmall
					select t).TryRandomElement(out t2))
					{
						IntVec3 randomCell = t2.OccupiedRect().RandomCell;
						Thing thing2 = thing.SplitOff(Math.Min(thing.def.stackLimit, num));
						num -= thing2.stackCount;
						GenPlace.TryPlaceThing(thing2, randomCell, map, ThingPlaceMode.Near, null, null, null, 1);
					}
				}
			}
			foreach (Thing thing3 in list)
			{
				if (thing3.def == ThingDefOf.Door)
				{
					MapGenerator.rootsToUnfog.AddRange(GenAdj.CellsAdjacentCardinal(thing3));
				}
				CompRefuelable compRefuelable;
				if (thing3.TryGetComp(out compRefuelable))
				{
					compRefuelable.Refuel(compRefuelable.Props.fuelCapacity);
				}
				Building_GravEngine building_GravEngine = thing3 as Building_GravEngine;
				if (building_GravEngine != null)
				{
					building_GravEngine.silentlyActivate = true;
				}
			}
			foreach (IntVec3 c2 in cellRect)
			{
				if (c2.GetTerrain(map) == TerrainDefOf.Substructure)
				{
					map.areaManager.Home[c2] = true;
				}
			}
		}

		// Token: 0x0600E4A9 RID: 58537 RVA: 0x0042CB6C File Offset: 0x0042AD6C
		public override void PostMapGenerate(Map map)
		{
			if (Find.GameInitData == null)
			{
				return;
			}
			if (this.method == PlayerPawnsArriveMethod.DropPods)
			{
				PawnUtility.GiveAllStartingPlayerPawnsThought(ThoughtDefOf.CrashedTogether);
			}
		}

		// Token: 0x0600E4AA RID: 58538 RVA: 0x0042CB89 File Offset: 0x0042AD89
		public override int GetHashCode()
		{
			return base.GetHashCode() ^ this.method.GetHashCode();
		}

		// Token: 0x04009907 RID: 39175
		private PlayerPawnsArriveMethod method;
	}
}
