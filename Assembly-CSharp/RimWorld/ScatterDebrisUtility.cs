using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001FF5 RID: 8181
	public static class ScatterDebrisUtility
	{
		// Token: 0x0600B445 RID: 46149 RVA: 0x0033B3C4 File Offset: 0x003395C4
		public static bool CanPlaceThingAt(IntVec3 c, Rot4 rot, Map map, ThingDef thingDef)
		{
			if (!c.InBounds(map))
			{
				return false;
			}
			if (!GenConstruct.CanBuildOnTerrain(thingDef, c, map, rot, null, null))
			{
				return false;
			}
			foreach (IntVec3 c2 in GenAdj.OccupiedRect(c, rot, thingDef.size))
			{
				if (!c2.InBounds(map) || c2.Roofed(map) || c2.GetEdifice(map) != null || !map.reachability.CanReachMapEdge(c2, TraverseParms.For(TraverseMode.NoPassClosedDoors, Danger.Deadly, false, false, false, true, false)))
				{
					return false;
				}
				if (c2.GetThingList(map).Count > 0)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600B446 RID: 46150 RVA: 0x0033B484 File Offset: 0x00339684
		public static void ScatterFilthAroundThing(Thing thing, Map map, ThingDef filth, float scatterChance = 0.5f, int expandBy = 1, int maxFilth = 2147483647, Func<IntVec3, bool> cellValidator = null)
		{
			int num = 0;
			foreach (IntVec3 intVec in thing.OccupiedRect().ExpandedBy(expandBy).InRandomOrder(null))
			{
				if (intVec.InBounds(map) && Rand.Chance(scatterChance) && (cellValidator == null || cellValidator(intVec)) && FilthMaker.TryMakeFilth(intVec, map, filth, 1, FilthSourceFlags.None, true))
				{
					num++;
				}
				if (num >= maxFilth)
				{
					break;
				}
			}
		}

		// Token: 0x0600B447 RID: 46151 RVA: 0x0033B518 File Offset: 0x00339718
		public static void ScatterFilthAroundCell(IntVec3 pos, IntVec2 area, Map map, ThingDef filth, float scatterChance = 0.5f, int maxFilth = 2147483647, Func<IntVec3, bool> cellValidator = null)
		{
			int num = 0;
			foreach (IntVec3 intVec in pos.RectAbout(area).InRandomOrder(null))
			{
				if (intVec.InBounds(map) && Rand.Chance(scatterChance) && (cellValidator == null || cellValidator(intVec)) && FilthMaker.TryMakeFilth(intVec, map, filth, 1, FilthSourceFlags.None, true))
				{
					num++;
				}
				if (num >= maxFilth)
				{
					break;
				}
			}
		}

		// Token: 0x0600B448 RID: 46152 RVA: 0x0033B5A4 File Offset: 0x003397A4
		public static void ScatterFilthAroundThing(Thing thing, Map map, ThingDef filth, IntRange count, int expandBy = 1, Func<IntVec3, bool> cellValidator = null)
		{
			int num = 0;
			int randomInRange = count.RandomInRange;
			foreach (IntVec3 intVec in thing.OccupiedRect().ExpandedBy(expandBy).InRandomOrder(null))
			{
				if (intVec.InBounds(map) && (cellValidator == null || cellValidator(intVec)) && FilthMaker.TryMakeFilth(intVec, map, filth, 1, FilthSourceFlags.None, true))
				{
					num++;
				}
				if (num >= randomInRange)
				{
					break;
				}
			}
		}

		// Token: 0x0600B449 RID: 46153 RVA: 0x0033B63C File Offset: 0x0033983C
		public static void ScatterAround(IntVec3 center, IntVec2 size, Rot4 rot, Sketch sketch, ThingDef scatterThing, float scatterChance = 0.5f, int expandBy = 1, int maxFilth = 2147483647, Func<IntVec3, bool> cellValidator = null)
		{
			int num = 0;
			foreach (IntVec3 intVec in GenAdj.OccupiedRect(center, rot, size).ExpandedBy(expandBy).InRandomOrder(null))
			{
				if (Rand.Chance(scatterChance) && (cellValidator == null || cellValidator(intVec)))
				{
					sketch.AddThing(scatterThing, intVec, Rot4.North, null, 1, null, null, true, 2f);
					num++;
				}
				if (num >= maxFilth)
				{
					break;
				}
			}
		}
	}
}
