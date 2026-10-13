using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02002023 RID: 8227
	public class GenStep_Labyrinth : GenStep
	{
		// Token: 0x17001B8D RID: 7053
		// (get) Token: 0x0600B4FD RID: 46333 RVA: 0x0033F599 File Offset: 0x0033D799
		public override int SeedPart
		{
			get
			{
				return 8767466;
			}
		}

		// Token: 0x0600B4FE RID: 46334 RVA: 0x0033F5A0 File Offset: 0x0033D7A0
		public override void Generate(Map map, GenStepParams parms)
		{
			if (!ModLister.CheckAnomaly("Labyrinth"))
			{
				return;
			}
			TerrainGrid terrainGrid = map.terrainGrid;
			foreach (IntVec3 c in map.AllCells)
			{
				terrainGrid.SetTerrain(c, TerrainDefOf.GraySurface);
			}
			CellRect rect = map.BoundsRect(0);
			GenStep_Labyrinth.FillEdges(rect, map);
			StructureGenParams parms2 = new StructureGenParams
			{
				size = rect.ContractedBy(2).Size
			};
			LayoutWorker worker = LayoutDefOf.Labyrinth.Worker;
			int num = 10;
			do
			{
				this.structureSketch = worker.GenerateStructureSketch(parms2);
			}
			while (!this.structureSketch.structureLayout.HasRoomWithDef(LayoutRoomDefOf.LabyrinthObelisk) && num-- > 0);
			if (num == 0)
			{
				Log.ErrorOnce("Failed to generate labyrinth, guard exceeded. Check layout worker for errors placing minimum rooms", 9868797);
				return;
			}
			worker.Spawn(this.structureSketch, map, new IntVec3(2, 0, 2), null, null, false, false, null);
			map.layoutStructureSketches.Add(this.structureSketch);
			LabyrinthMapComponent component = map.GetComponent<LabyrinthMapComponent>();
			LayoutRoom firstRoomOfDef = this.structureSketch.structureLayout.GetFirstRoomOfDef(LayoutRoomDefOf.LabyrinthObelisk);
			List<LayoutRoom> spawnableRooms = this.GetSpawnableRooms(firstRoomOfDef);
			component.SetSpawnRooms(spawnableRooms);
			MapGenerator.PlayerStartSpot = IntVec3.Zero;
			map.fogGrid.SetAllFogged();
		}

		// Token: 0x0600B4FF RID: 46335 RVA: 0x0033F704 File Offset: 0x0033D904
		private List<LayoutRoom> GetSpawnableRooms(LayoutRoom obelisk)
		{
			List<LayoutRoom> list = new List<LayoutRoom>();
			list.AddRange(this.structureSketch.structureLayout.Rooms);
			list.Remove(obelisk);
			foreach (ValueTuple<LayoutRoom, CellRect, CellRect> valueTuple in this.structureSketch.structureLayout.GetLogicalRoomConnections(obelisk))
			{
				LayoutRoom item = valueTuple.Item1;
				if (list.Contains(item))
				{
					list.Remove(item);
					foreach (ValueTuple<LayoutRoom, CellRect, CellRect> valueTuple2 in this.structureSketch.structureLayout.GetLogicalRoomConnections(item))
					{
						LayoutRoom item2 = valueTuple2.Item1;
						if (list.Contains(item2))
						{
							list.Remove(item2);
						}
					}
				}
			}
			for (int i = list.Count - 1; i >= 0; i--)
			{
				using (List<LayoutRoomDef>.Enumerator enumerator3 = list[i].defs.GetEnumerator())
				{
					while (enumerator3.MoveNext())
					{
						if (!enumerator3.Current.isValidPlayerSpawnRoom)
						{
							list.RemoveAt(i);
							break;
						}
					}
				}
			}
			if (list.Empty<LayoutRoom>())
			{
				list.Clear();
				list.AddRange(this.structureSketch.structureLayout.Rooms);
				list.Remove(obelisk);
			}
			return list;
		}

		// Token: 0x0600B500 RID: 46336 RVA: 0x0033F880 File Offset: 0x0033DA80
		private static void FillEdges(CellRect rect, Map map)
		{
			for (int i = 0; i < rect.Width; i++)
			{
				for (int j = 0; j < 2; j++)
				{
					GenStep_Labyrinth.SpawnWall(new IntVec3(i, 0, j), map);
					GenStep_Labyrinth.SpawnWall(new IntVec3(i, 0, rect.Height - j - 1), map);
				}
			}
			for (int k = 2; k < rect.Height - 2; k++)
			{
				for (int l = 0; l < 2; l++)
				{
					GenStep_Labyrinth.SpawnWall(new IntVec3(l, 0, k), map);
					GenStep_Labyrinth.SpawnWall(new IntVec3(rect.Width - l - 1, 0, k), map);
				}
			}
		}

		// Token: 0x0600B501 RID: 46337 RVA: 0x0033F917 File Offset: 0x0033DB17
		private static void SpawnWall(IntVec3 pos, Map map)
		{
			GenSpawn.Spawn(ThingMaker.MakeThing(ThingDefOf.GrayWall, ThingDefOf.LabyrinthMatter), pos, map, WipeMode.Vanish);
		}

		// Token: 0x04007C2C RID: 31788
		private LayoutStructureSketch structureSketch;

		// Token: 0x04007C2D RID: 31789
		private const int Border = 2;
	}
}
