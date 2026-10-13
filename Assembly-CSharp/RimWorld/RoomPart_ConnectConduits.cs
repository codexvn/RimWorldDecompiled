using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02001F50 RID: 8016
	public class RoomPart_ConnectConduits : RoomPartWorker
	{
		// Token: 0x0600B13A RID: 45370 RVA: 0x00329C8D File Offset: 0x00327E8D
		public RoomPart_ConnectConduits(RoomPartDef def) : base(def)
		{
		}

		// Token: 0x17001B07 RID: 6919
		// (get) Token: 0x0600B13B RID: 45371 RVA: 0x000028E7 File Offset: 0x00000AE7
		public override bool FillOnPost
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600B13C RID: 45372 RVA: 0x0032A584 File Offset: 0x00328784
		public override void FillRoom(Map map, LayoutRoom room, Faction faction, float threatPoints)
		{
			HashSet<Building> hashSet = new HashSet<Building>();
			HashSet<Building> hashSet2 = new HashSet<Building>();
			HashSet<Building> hashSet3 = new HashSet<Building>();
			foreach (CellRect cellRect in room.rects)
			{
				foreach (IntVec3 c in cellRect.Cells)
				{
					List<Thing> list = map.thingGrid.ThingsListAt(c);
					for (int i = 0; i < list.Count; i++)
					{
						Building building = list[i] as Building;
						if (building != null && building.PowerComp != null)
						{
							if (building.PowerComp is CompPowerPlant)
							{
								hashSet.Add(building);
							}
							else if (building.def == ThingDefOf.HiddenConduit)
							{
								hashSet3.Add(building);
							}
							else if (!(building.PowerComp is CompPowerTransmitter))
							{
								hashSet2.Add(building);
							}
						}
					}
				}
			}
			if (hashSet.Any<Building>() && hashSet2.Any<Building>())
			{
				using (HashSet<Building>.Enumerator enumerator3 = hashSet3.GetEnumerator())
				{
					while (enumerator3.MoveNext())
					{
						Building conduit = enumerator3.Current;
						Building building2 = (from ps in hashSet
						orderby (ps.Position - conduit.Position).LengthHorizontalSquared
						select ps).FirstOrDefault<Building>();
						if (building2 != null)
						{
							this.CreateHiddenConduitPath(map, building2.Position, conduit.Position);
						}
					}
				}
			}
			if (hashSet.Count > 1)
			{
				List<Building> list2 = hashSet.ToList<Building>();
				Building building3 = list2[0];
				for (int j = 1; j < list2.Count; j++)
				{
					this.CreateHiddenConduitPath(map, building3.Position, list2[j].Position);
				}
			}
			foreach (Building building4 in hashSet2)
			{
				CompPower powerComp = building4.PowerComp;
				PowerConnectionMaker.DisconnectFromPowerNet(powerComp);
				Building transmitter = powerComp.parent.Position.GetTransmitter(map);
				if (((transmitter != null) ? transmitter.PowerComp : null) != null)
				{
					powerComp.ConnectToTransmitter(transmitter.PowerComp, false);
				}
				else
				{
					map.powerNetManager.Notify_ConnectorWantsConnect(powerComp);
				}
			}
		}

		// Token: 0x0600B13D RID: 45373 RVA: 0x0032A818 File Offset: 0x00328A18
		private void CreateHiddenConduitPath(Map map, IntVec3 from, IntVec3 to)
		{
			List<IntVec3> list = this.FindCardinalOnlyPath(map, from, to);
			if (list == null || list.Count == 0)
			{
				return;
			}
			foreach (IntVec3 intVec in list)
			{
				if (intVec.GetTransmitter(map) == null)
				{
					Thing thing;
					GenSpawn.TrySpawn(ThingDefOf.HiddenConduit, intVec, map, out thing, WipeMode.Vanish, true);
				}
			}
		}

		// Token: 0x0600B13E RID: 45374 RVA: 0x0032A890 File Offset: 0x00328A90
		private List<IntVec3> FindCardinalOnlyPath(Map map, IntVec3 from, IntVec3 to)
		{
			new List<IntVec3>();
			List<IntVec3> list = this.TryLShapedPath(map, from, to, true);
			if (list != null)
			{
				return list;
			}
			List<IntVec3> list2 = this.TryLShapedPath(map, from, to, false);
			if (list2 != null)
			{
				return list2;
			}
			return null;
		}

		// Token: 0x0600B13F RID: 45375 RVA: 0x0032A8C4 File Offset: 0x00328AC4
		private List<IntVec3> TryLShapedPath(Map map, IntVec3 from, IntVec3 to, bool horizontalFirst)
		{
			List<IntVec3> list = new List<IntVec3>();
			IntVec3 intVec = from;
			if (horizontalFirst)
			{
				while (intVec.x != to.x)
				{
					if (intVec.x < to.x)
					{
						intVec.x++;
					}
					else
					{
						intVec.x--;
					}
					if (intVec != to && intVec != from)
					{
						if (!this.CanPlaceConduitAt(map, intVec))
						{
							return null;
						}
						list.Add(intVec);
					}
				}
				while (intVec.z != to.z)
				{
					if (intVec.z < to.z)
					{
						intVec.z++;
					}
					else
					{
						intVec.z--;
					}
					if (intVec != to && intVec != from)
					{
						if (!this.CanPlaceConduitAt(map, intVec))
						{
							return null;
						}
						list.Add(intVec);
					}
				}
			}
			else
			{
				while (intVec.z != to.z)
				{
					if (intVec.z < to.z)
					{
						intVec.z++;
					}
					else
					{
						intVec.z--;
					}
					if (intVec != to && intVec != from)
					{
						if (!this.CanPlaceConduitAt(map, intVec))
						{
							return null;
						}
						list.Add(intVec);
					}
				}
				while (intVec.x != to.x)
				{
					if (intVec.x < to.x)
					{
						intVec.x++;
					}
					else
					{
						intVec.x--;
					}
					if (intVec != to && intVec != from)
					{
						if (!this.CanPlaceConduitAt(map, intVec))
						{
							return null;
						}
						list.Add(intVec);
					}
				}
			}
			return list;
		}

		// Token: 0x0600B140 RID: 45376 RVA: 0x0032AA58 File Offset: 0x00328C58
		private bool CanPlaceConduitAt(Map map, IntVec3 cell)
		{
			if (!cell.InBounds(map))
			{
				return false;
			}
			Building transmitter = cell.GetTransmitter(map);
			return transmitter == null || transmitter.def == ThingDefOf.HiddenConduit;
		}
	}
}
