using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001F6A RID: 8042
	public class ListerBuildingWithTagInProximity
	{
		// Token: 0x0600B1B5 RID: 45493 RVA: 0x0032C9D6 File Offset: 0x0032ABD6
		public ListerBuildingWithTagInProximity(Map map)
		{
			this.map = map;
		}

		// Token: 0x0600B1B6 RID: 45494 RVA: 0x0032C9F0 File Offset: 0x0032ABF0
		public void Notify_BuildingSpawned(Building b)
		{
			this.requestCache.Clear();
		}

		// Token: 0x0600B1B7 RID: 45495 RVA: 0x0032C9F0 File Offset: 0x0032ABF0
		public void Notify_BuildingDeSpawned(Building b)
		{
			this.requestCache.Clear();
		}

		// Token: 0x0600B1B8 RID: 45496 RVA: 0x0032CA00 File Offset: 0x0032AC00
		public List<Thing> GetForCell(IntVec3 cell, float radius, string tag, Thing forThing = null)
		{
			ListerBuildingWithTagInProximity.CellRequest key = new ListerBuildingWithTagInProximity.CellRequest(cell, radius, tag, forThing);
			List<Thing> list;
			if (!this.requestCache.TryGetValue(key, out list))
			{
				list = new List<Thing>();
				foreach (Thing thing in GenRadial.RadialDistinctThingsAround(cell, this.map, radius, false))
				{
					ThingDef thingDef = thing.def;
					if (thing.def.IsBlueprint || thing.def.isFrameInt)
					{
						ThingDef thingDef2 = thing.def.entityDefToBuild as ThingDef;
						if (thingDef2 != null)
						{
							thingDef = thingDef2;
						}
					}
					if (thingDef.building != null && thingDef.building.buildingTags.Contains(tag) && thing.GetRoom(RegionType.Set_All) == cell.GetRoom(this.map) && thing != forThing)
					{
						list.Add(thing);
					}
				}
				list.SortBy((Thing t) => t.Position.DistanceTo(cell));
				this.requestCache[key] = list;
			}
			return list;
		}

		// Token: 0x0400798C RID: 31116
		private Map map;

		// Token: 0x0400798D RID: 31117
		private Dictionary<ListerBuildingWithTagInProximity.CellRequest, List<Thing>> requestCache = new Dictionary<ListerBuildingWithTagInProximity.CellRequest, List<Thing>>();

		// Token: 0x02001F6B RID: 8043
		public struct CellRequest : IEquatable<ListerBuildingWithTagInProximity.CellRequest>
		{
			// Token: 0x0600B1B9 RID: 45497 RVA: 0x0032CB38 File Offset: 0x0032AD38
			public CellRequest(IntVec3 c, float r, string tag, Thing t = null)
			{
				this.cell = c;
				this.radius = r;
				this.tag = tag;
				this.forThing = t;
			}

			// Token: 0x0600B1BA RID: 45498 RVA: 0x0032CB58 File Offset: 0x0032AD58
			public bool Equals(ListerBuildingWithTagInProximity.CellRequest other)
			{
				return this.cell.Equals(other.cell) && this.radius.Equals(other.radius) && this.tag == other.tag && this.forThing == other.forThing;
			}

			// Token: 0x0600B1BB RID: 45499 RVA: 0x0032CBB4 File Offset: 0x0032ADB4
			public override int GetHashCode()
			{
				int num = this.cell.GetHashCode();
				num = (num * 397 ^ this.radius.GetHashCode());
				if (this.forThing != null)
				{
					num = (num * 397 ^ this.forThing.GetHashCode());
				}
				return num ^ this.tag.GetHashCode();
			}

			// Token: 0x0400798E RID: 31118
			public readonly IntVec3 cell;

			// Token: 0x0400798F RID: 31119
			public readonly float radius;

			// Token: 0x04007990 RID: 31120
			public readonly Thing forThing;

			// Token: 0x04007991 RID: 31121
			public readonly string tag;
		}
	}
}
