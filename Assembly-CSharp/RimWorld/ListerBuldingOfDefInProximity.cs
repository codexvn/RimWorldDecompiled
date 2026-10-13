using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001F6D RID: 8045
	public class ListerBuldingOfDefInProximity
	{
		// Token: 0x0600B1BE RID: 45502 RVA: 0x0032CC2B File Offset: 0x0032AE2B
		public ListerBuldingOfDefInProximity(Map map)
		{
			this.map = map;
		}

		// Token: 0x0600B1BF RID: 45503 RVA: 0x0032CC45 File Offset: 0x0032AE45
		public void Notify_BuildingSpawned(Building b)
		{
			this.requestCache.Clear();
		}

		// Token: 0x0600B1C0 RID: 45504 RVA: 0x0032CC45 File Offset: 0x0032AE45
		public void Notify_BuildingDeSpawned(Building b)
		{
			this.requestCache.Clear();
		}

		// Token: 0x0600B1C1 RID: 45505 RVA: 0x0032CC54 File Offset: 0x0032AE54
		public List<Thing> GetForCell(IntVec3 cell, float radius, List<MeditationFocusOffsetPerBuilding> defs, Thing forThing = null)
		{
			ListerBuldingOfDefInProximity.CellRequest key = new ListerBuldingOfDefInProximity.CellRequest(cell, radius, defs, forThing);
			List<Thing> list;
			if (!this.requestCache.TryGetValue(key, out list))
			{
				list = new List<Thing>();
				using (IEnumerator<Thing> enumerator = GenRadial.RadialDistinctThingsAround(cell, this.map, radius, false).GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Thing t = enumerator.Current;
						if (defs.Any((MeditationFocusOffsetPerBuilding d) => d.building == t.def) && t.GetRoom(RegionType.Set_All) == cell.GetRoom(this.map) && t != forThing)
						{
							list.Add(t);
						}
					}
				}
				list.SortBy(delegate(Thing t)
				{
					float num = t.Position.DistanceTo(cell);
					MeditationFocusOffsetPerBuilding meditationFocusOffsetPerBuilding = defs.FirstOrDefault((MeditationFocusOffsetPerBuilding d) => d.building == t.def);
					if (meditationFocusOffsetPerBuilding != null)
					{
						num -= meditationFocusOffsetPerBuilding.offset * 100000f;
					}
					return num;
				});
				this.requestCache[key] = list;
			}
			return list;
		}

		// Token: 0x0600B1C2 RID: 45506 RVA: 0x0032CD6C File Offset: 0x0032AF6C
		public List<Thing> GetForCell(IntVec3 cell, float radius, ThingDef def, Thing forThing = null)
		{
			ListerBuldingOfDefInProximity.CellRequest key = new ListerBuldingOfDefInProximity.CellRequest(cell, radius, def, forThing);
			List<Thing> list;
			if (!this.requestCache.TryGetValue(key, out list))
			{
				list = new List<Thing>();
				foreach (Thing thing in GenRadial.RadialDistinctThingsAround(cell, this.map, radius, false))
				{
					if (def == thing.def && thing.GetRoom(RegionType.Set_All) == cell.GetRoom(this.map) && thing != forThing)
					{
						list.Add(thing);
					}
				}
				this.requestCache[key] = list;
			}
			return list;
		}

		// Token: 0x04007993 RID: 31123
		private Map map;

		// Token: 0x04007994 RID: 31124
		private Dictionary<ListerBuldingOfDefInProximity.CellRequest, List<Thing>> requestCache = new Dictionary<ListerBuldingOfDefInProximity.CellRequest, List<Thing>>();

		// Token: 0x02001F6E RID: 8046
		public struct CellRequest : IEquatable<ListerBuldingOfDefInProximity.CellRequest>
		{
			// Token: 0x0600B1C3 RID: 45507 RVA: 0x0032CE18 File Offset: 0x0032B018
			public CellRequest(IntVec3 c, float r, List<MeditationFocusOffsetPerBuilding> d, Thing t = null)
			{
				this.cell = c;
				this.radius = r;
				this.defs = d;
				this.forThing = t;
				this.def = null;
			}

			// Token: 0x0600B1C4 RID: 45508 RVA: 0x0032CE3E File Offset: 0x0032B03E
			public CellRequest(IntVec3 c, float r, ThingDef d, Thing t = null)
			{
				this.cell = c;
				this.radius = r;
				this.def = d;
				this.defs = null;
				this.forThing = t;
			}

			// Token: 0x0600B1C5 RID: 45509 RVA: 0x0032CE64 File Offset: 0x0032B064
			public bool Equals(ListerBuldingOfDefInProximity.CellRequest other)
			{
				return this.cell.Equals(other.cell) && this.radius.Equals(other.radius) && GenCollection.ListsEqual<MeditationFocusOffsetPerBuilding>(this.defs, other.defs) && this.def == other.def && this.forThing == other.forThing;
			}

			// Token: 0x0600B1C6 RID: 45510 RVA: 0x0032CED0 File Offset: 0x0032B0D0
			public override int GetHashCode()
			{
				int num = this.cell.GetHashCode();
				num = (num * 397 ^ this.radius.GetHashCode());
				if (this.forThing != null)
				{
					num = (num * 397 ^ this.forThing.GetHashCode());
				}
				if (this.defs != null)
				{
					for (int i = 0; i < this.defs.Count; i++)
					{
						num ^= this.defs[i].GetHashCode();
					}
				}
				if (this.def != null)
				{
					num ^= this.def.GetHashCode();
				}
				return num;
			}

			// Token: 0x04007995 RID: 31125
			public readonly IntVec3 cell;

			// Token: 0x04007996 RID: 31126
			public readonly float radius;

			// Token: 0x04007997 RID: 31127
			public readonly Thing forThing;

			// Token: 0x04007998 RID: 31128
			public readonly ThingDef def;

			// Token: 0x04007999 RID: 31129
			public readonly List<MeditationFocusOffsetPerBuilding> defs;
		}
	}
}
