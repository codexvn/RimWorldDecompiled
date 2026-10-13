using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02002AD9 RID: 10969
	public static class FilthMaker
	{
		// Token: 0x0600F470 RID: 62576 RVA: 0x004707B4 File Offset: 0x0046E9B4
		public static bool CanMakeFilth(IntVec3 cell, Map map, ThingDef filthDef, FilthSourceFlags additionalFlags = FilthSourceFlags.None)
		{
			TerrainGrid terrainGrid = map.terrainGrid;
			foreach (IntVec3 intVec in GenAdj.OccupiedRect(cell, Rot4.North, filthDef.size))
			{
				if (!intVec.InBounds(map))
				{
					return false;
				}
				TerrainDef terrainDef = terrainGrid.FoundationAt(intVec) ?? terrainGrid.TerrainAt(intVec);
				if (!filthDef.filth.ignoreFilthMultiplierStat && (filthDef.filth.placementMask & FilthSourceFlags.Natural) == FilthSourceFlags.None && Rand.Value > terrainDef.GetStatValueAbstract(StatDefOf.FilthMultiplier, null))
				{
					return false;
				}
				FilthSourceFlags filthSourceFlags = filthDef.filth.placementMask | additionalFlags;
				if (terrainDef.filthAcceptanceMask != FilthSourceFlags.None && filthSourceFlags.HasFlag(FilthSourceFlags.Pawn))
				{
					if (intVec.GetRoof(map) != null)
					{
						return true;
					}
					Room room = intVec.GetRoom(map);
					if (room != null && !room.TouchesMapEdge && !room.UsesOutdoorTemperature)
					{
						return true;
					}
				}
				if (!FilthMaker.TerrainAcceptsFilth(terrainDef, filthDef, additionalFlags))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600F471 RID: 62577 RVA: 0x004708EC File Offset: 0x0046EAEC
		public static bool TerrainAcceptsFilth(TerrainDef terrainDef, ThingDef filthDef, FilthSourceFlags additionalFlags = FilthSourceFlags.None)
		{
			if (terrainDef.filthAcceptanceMask == FilthSourceFlags.None)
			{
				return false;
			}
			FilthSourceFlags filthSourceFlags = filthDef.filth.placementMask | additionalFlags;
			return (terrainDef.filthAcceptanceMask & filthSourceFlags) == filthSourceFlags;
		}

		// Token: 0x0600F472 RID: 62578 RVA: 0x0047091C File Offset: 0x0046EB1C
		public static bool TryMakeFilth(IntVec3 c, Map map, ThingDef filthDef, int count = 1, FilthSourceFlags additionalFlags = FilthSourceFlags.None, bool shouldPropagate = true)
		{
			bool flag = false;
			for (int i = 0; i < count; i++)
			{
				Filth filth;
				flag |= FilthMaker.TryMakeFilth(c, map, filthDef, null, shouldPropagate, out filth, additionalFlags);
			}
			return flag;
		}

		// Token: 0x0600F473 RID: 62579 RVA: 0x0047094C File Offset: 0x0046EB4C
		public static bool TryMakeFilth(IntVec3 c, Map map, ThingDef filthDef, out Filth outFilth, int count = 1, FilthSourceFlags additionalFlags = FilthSourceFlags.None, bool shouldPropagate = true)
		{
			outFilth = null;
			bool flag = false;
			for (int i = 0; i < count; i++)
			{
				flag |= FilthMaker.TryMakeFilth(c, map, filthDef, null, shouldPropagate, out outFilth, additionalFlags);
			}
			return flag;
		}

		// Token: 0x0600F474 RID: 62580 RVA: 0x00470980 File Offset: 0x0046EB80
		public static bool TryMakeFilth(IntVec3 c, Map map, ThingDef filthDef, string source, int count = 1, FilthSourceFlags additionalFlags = FilthSourceFlags.None)
		{
			bool flag = false;
			for (int i = 0; i < count; i++)
			{
				Filth filth;
				flag |= FilthMaker.TryMakeFilth(c, map, filthDef, Gen.YieldSingle<string>(source), true, out filth, additionalFlags);
			}
			return flag;
		}

		// Token: 0x0600F475 RID: 62581 RVA: 0x004709B4 File Offset: 0x0046EBB4
		public static bool TryMakeFilth(IntVec3 c, Map map, ThingDef filthDef, IEnumerable<string> sources, FilthSourceFlags additionalFlags = FilthSourceFlags.None)
		{
			Filth filth;
			return FilthMaker.TryMakeFilth(c, map, filthDef, sources, true, out filth, additionalFlags);
		}

		// Token: 0x0600F476 RID: 62582 RVA: 0x004709CF File Offset: 0x0046EBCF
		public static bool TryMakeFilth(IntVec3 c, Map map, ThingDef filthDef, out Filth outFilth, string source, FilthSourceFlags additionalFlags = FilthSourceFlags.None, bool shouldPropagate = true)
		{
			outFilth = null;
			return FilthMaker.TryMakeFilth(c, map, filthDef, Gen.YieldSingle<string>(source), shouldPropagate, out outFilth, additionalFlags);
		}

		// Token: 0x0600F477 RID: 62583 RVA: 0x004709E8 File Offset: 0x0046EBE8
		private static bool TryMakeFilth(IntVec3 c, Map map, ThingDef filthDef, IEnumerable<string> sources, bool shouldPropagate, out Filth outFilth, FilthSourceFlags additionalFlags = FilthSourceFlags.None)
		{
			outFilth = (Filth)c.GetThingList(map).FirstOrDefault((Thing t) => t.def == filthDef);
			if (c.GetTerrain(map).exposesToVacuum)
			{
				return false;
			}
			if (!c.WalkableByAny(map) || (outFilth != null && !outFilth.CanBeThickened))
			{
				if (shouldPropagate)
				{
					List<IntVec3> list = GenAdj.AdjacentCells8WayRandomized();
					for (int i = 0; i < 8; i++)
					{
						IntVec3 c2 = c + list[i];
						if (c2.InBounds(map) && FilthMaker.TryMakeFilth(c2, map, filthDef, sources, false, out outFilth, FilthSourceFlags.None))
						{
							return true;
						}
					}
				}
				if (outFilth != null)
				{
					outFilth.AddSources(sources);
				}
				return false;
			}
			if (outFilth != null)
			{
				outFilth.ThickenFilth();
				outFilth.AddSources(sources);
			}
			else
			{
				if (!FilthMaker.CanMakeFilth(c, map, filthDef, additionalFlags))
				{
					return false;
				}
				outFilth = (Filth)ThingMaker.MakeThing(filthDef, null);
				outFilth.AddSources(sources);
				GenSpawn.Spawn(outFilth, c, map, WipeMode.Vanish);
			}
			FilthMonitor.Notify_FilthSpawned();
			return true;
		}

		// Token: 0x0600F478 RID: 62584 RVA: 0x00470AF8 File Offset: 0x0046ECF8
		public static void RemoveAllFilth(IntVec3 c, Map map)
		{
			FilthMaker.toBeRemoved.Clear();
			List<Thing> thingList = c.GetThingList(map);
			for (int i = 0; i < thingList.Count; i++)
			{
				Filth filth = thingList[i] as Filth;
				if (filth != null)
				{
					FilthMaker.toBeRemoved.Add(filth);
				}
			}
			for (int j = 0; j < FilthMaker.toBeRemoved.Count; j++)
			{
				FilthMaker.toBeRemoved[j].Destroy(DestroyMode.Vanish);
			}
			FilthMaker.toBeRemoved.Clear();
		}

		// Token: 0x0400A11A RID: 41242
		private static readonly List<Filth> toBeRemoved = new List<Filth>();
	}
}
