using System;
using RimWorld;
using Verse.AI;
using Verse.AI.Group;

namespace Verse
{
	// Token: 0x02000495 RID: 1173
	public class MapEvents
	{
		// Token: 0x06001E97 RID: 7831 RVA: 0x000A0BFB File Offset: 0x0009EDFB
		public MapEvents(Map map)
		{
			this.map = map;
		}

		// Token: 0x14000002 RID: 2
		// (add) Token: 0x06001E98 RID: 7832 RVA: 0x000A0C0C File Offset: 0x0009EE0C
		// (remove) Token: 0x06001E99 RID: 7833 RVA: 0x000A0C44 File Offset: 0x0009EE44
		public event Action<Building> BuildingSpawned;

		// Token: 0x14000003 RID: 3
		// (add) Token: 0x06001E9A RID: 7834 RVA: 0x000A0C7C File Offset: 0x0009EE7C
		// (remove) Token: 0x06001E9B RID: 7835 RVA: 0x000A0CB4 File Offset: 0x0009EEB4
		public event Action<Building> BuildingDespawned;

		// Token: 0x06001E9C RID: 7836 RVA: 0x000A0CE9 File Offset: 0x0009EEE9
		public void Notify_BuildingSpawned(Building building)
		{
			Action<Building> buildingSpawned = this.BuildingSpawned;
			if (buildingSpawned == null)
			{
				return;
			}
			buildingSpawned(building);
		}

		// Token: 0x06001E9D RID: 7837 RVA: 0x000A0CFC File Offset: 0x0009EEFC
		public void Notify_BuildingDespawned(Building building)
		{
			Action<Building> buildingDespawned = this.BuildingDespawned;
			if (buildingDespawned == null)
			{
				return;
			}
			buildingDespawned(building);
		}

		// Token: 0x14000004 RID: 4
		// (add) Token: 0x06001E9E RID: 7838 RVA: 0x000A0D10 File Offset: 0x0009EF10
		// (remove) Token: 0x06001E9F RID: 7839 RVA: 0x000A0D48 File Offset: 0x0009EF48
		public event Action<Thing> ThingSpawned;

		// Token: 0x14000005 RID: 5
		// (add) Token: 0x06001EA0 RID: 7840 RVA: 0x000A0D80 File Offset: 0x0009EF80
		// (remove) Token: 0x06001EA1 RID: 7841 RVA: 0x000A0DB8 File Offset: 0x0009EFB8
		public event Action<Thing> ThingDespawned;

		// Token: 0x06001EA2 RID: 7842 RVA: 0x000A0DED File Offset: 0x0009EFED
		public void Notify_ThingSpawned(Thing thing)
		{
			Action<Thing> thingSpawned = this.ThingSpawned;
			if (thingSpawned == null)
			{
				return;
			}
			thingSpawned(thing);
		}

		// Token: 0x06001EA3 RID: 7843 RVA: 0x000A0E00 File Offset: 0x0009F000
		public void Notify_ThingDespawned(Thing thing)
		{
			Action<Thing> thingDespawned = this.ThingDespawned;
			if (thingDespawned == null)
			{
				return;
			}
			thingDespawned(thing);
		}

		// Token: 0x14000006 RID: 6
		// (add) Token: 0x06001EA4 RID: 7844 RVA: 0x000A0E14 File Offset: 0x0009F014
		// (remove) Token: 0x06001EA5 RID: 7845 RVA: 0x000A0E4C File Offset: 0x0009F04C
		public event Action<GameCondition> GameConditionAdded;

		// Token: 0x14000007 RID: 7
		// (add) Token: 0x06001EA6 RID: 7846 RVA: 0x000A0E84 File Offset: 0x0009F084
		// (remove) Token: 0x06001EA7 RID: 7847 RVA: 0x000A0EBC File Offset: 0x0009F0BC
		public event Action<GameCondition> GameConditionRemoved;

		// Token: 0x06001EA8 RID: 7848 RVA: 0x000A0EF1 File Offset: 0x0009F0F1
		public void Notify_GameConditionAdded(GameCondition condition)
		{
			Action<GameCondition> gameConditionAdded = this.GameConditionAdded;
			if (gameConditionAdded == null)
			{
				return;
			}
			gameConditionAdded(condition);
		}

		// Token: 0x06001EA9 RID: 7849 RVA: 0x000A0F04 File Offset: 0x0009F104
		public void Notify_GameConditionRemoved(GameCondition condition)
		{
			Action<GameCondition> gameConditionRemoved = this.GameConditionRemoved;
			if (gameConditionRemoved == null)
			{
				return;
			}
			gameConditionRemoved(condition);
		}

		// Token: 0x14000008 RID: 8
		// (add) Token: 0x06001EAA RID: 7850 RVA: 0x000A0F18 File Offset: 0x0009F118
		// (remove) Token: 0x06001EAB RID: 7851 RVA: 0x000A0F50 File Offset: 0x0009F150
		public event Action<Lord> LordAdded;

		// Token: 0x14000009 RID: 9
		// (add) Token: 0x06001EAC RID: 7852 RVA: 0x000A0F88 File Offset: 0x0009F188
		// (remove) Token: 0x06001EAD RID: 7853 RVA: 0x000A0FC0 File Offset: 0x0009F1C0
		public event Action<Lord> LordRemoved;

		// Token: 0x06001EAE RID: 7854 RVA: 0x000A0FF5 File Offset: 0x0009F1F5
		public void Notify_LordAdded(Lord lord)
		{
			Action<Lord> lordAdded = this.LordAdded;
			if (lordAdded == null)
			{
				return;
			}
			lordAdded(lord);
		}

		// Token: 0x06001EAF RID: 7855 RVA: 0x000A1008 File Offset: 0x0009F208
		public void Notify_LordRemoved(Lord lord)
		{
			Action<Lord> lordRemoved = this.LordRemoved;
			if (lordRemoved == null)
			{
				return;
			}
			lordRemoved(lord);
		}

		// Token: 0x1400000A RID: 10
		// (add) Token: 0x06001EB0 RID: 7856 RVA: 0x000A101C File Offset: 0x0009F21C
		// (remove) Token: 0x06001EB1 RID: 7857 RVA: 0x000A1054 File Offset: 0x0009F254
		public event Action<Faction> FactionAdded;

		// Token: 0x1400000B RID: 11
		// (add) Token: 0x06001EB2 RID: 7858 RVA: 0x000A108C File Offset: 0x0009F28C
		// (remove) Token: 0x06001EB3 RID: 7859 RVA: 0x000A10C4 File Offset: 0x0009F2C4
		public event Action<Faction> FactionRemoved;

		// Token: 0x06001EB4 RID: 7860 RVA: 0x000A10F9 File Offset: 0x0009F2F9
		public void Notify_FactionAdded(Faction faction)
		{
			Action<Faction> factionAdded = this.FactionAdded;
			if (factionAdded == null)
			{
				return;
			}
			factionAdded(faction);
		}

		// Token: 0x06001EB5 RID: 7861 RVA: 0x000A110C File Offset: 0x0009F30C
		public void Notify_FactionRemoved(Faction faction)
		{
			Action<Faction> factionRemoved = this.FactionRemoved;
			if (factionRemoved == null)
			{
				return;
			}
			factionRemoved(faction);
		}

		// Token: 0x1400000C RID: 12
		// (add) Token: 0x06001EB6 RID: 7862 RVA: 0x000A1120 File Offset: 0x0009F320
		// (remove) Token: 0x06001EB7 RID: 7863 RVA: 0x000A1158 File Offset: 0x0009F358
		public event Action<Faction, Faction> ThingFactionChanged;

		// Token: 0x06001EB8 RID: 7864 RVA: 0x000A118D File Offset: 0x0009F38D
		public void Notify_ThingFactionChanged(Faction previous, Faction faction)
		{
			Action<Faction, Faction> thingFactionChanged = this.ThingFactionChanged;
			if (thingFactionChanged == null)
			{
				return;
			}
			thingFactionChanged(previous, faction);
		}

		// Token: 0x1400000D RID: 13
		// (add) Token: 0x06001EB9 RID: 7865 RVA: 0x000A11A4 File Offset: 0x0009F3A4
		// (remove) Token: 0x06001EBA RID: 7866 RVA: 0x000A11DC File Offset: 0x0009F3DC
		public event Action<Building> BuildingHitPointsChanged;

		// Token: 0x06001EBB RID: 7867 RVA: 0x000A1211 File Offset: 0x0009F411
		public void Notify_BuildingHitPointsChanged(Building building)
		{
			Action<Building> buildingHitPointsChanged = this.BuildingHitPointsChanged;
			if (buildingHitPointsChanged == null)
			{
				return;
			}
			buildingHitPointsChanged(building);
		}

		// Token: 0x1400000E RID: 14
		// (add) Token: 0x06001EBC RID: 7868 RVA: 0x000A1224 File Offset: 0x0009F424
		// (remove) Token: 0x06001EBD RID: 7869 RVA: 0x000A125C File Offset: 0x0009F45C
		public event Action<IntVec3> TerrainChanged;

		// Token: 0x06001EBE RID: 7870 RVA: 0x000A1291 File Offset: 0x0009F491
		public void Notify_TerrainChanged(IntVec3 cell)
		{
			Action<IntVec3> terrainChanged = this.TerrainChanged;
			if (terrainChanged == null)
			{
				return;
			}
			terrainChanged(cell);
		}

		// Token: 0x1400000F RID: 15
		// (add) Token: 0x06001EBF RID: 7871 RVA: 0x000A12A4 File Offset: 0x0009F4A4
		// (remove) Token: 0x06001EC0 RID: 7872 RVA: 0x000A12DC File Offset: 0x0009F4DC
		public event Action<IntVec3> PathCostRecalculate;

		// Token: 0x06001EC1 RID: 7873 RVA: 0x000A1311 File Offset: 0x0009F511
		public void Notify_PathCostRecalculated(IntVec3 cell)
		{
			Action<IntVec3> pathCostRecalculate = this.PathCostRecalculate;
			if (pathCostRecalculate == null)
			{
				return;
			}
			pathCostRecalculate(cell);
		}

		// Token: 0x14000010 RID: 16
		// (add) Token: 0x06001EC2 RID: 7874 RVA: 0x000A1324 File Offset: 0x0009F524
		// (remove) Token: 0x06001EC3 RID: 7875 RVA: 0x000A135C File Offset: 0x0009F55C
		public event Action<ReservationManager.Reservation> ReservationAdded;

		// Token: 0x14000011 RID: 17
		// (add) Token: 0x06001EC4 RID: 7876 RVA: 0x000A1394 File Offset: 0x0009F594
		// (remove) Token: 0x06001EC5 RID: 7877 RVA: 0x000A13CC File Offset: 0x0009F5CC
		public event Action<ReservationManager.Reservation> ReservationRemoved;

		// Token: 0x06001EC6 RID: 7878 RVA: 0x000A1401 File Offset: 0x0009F601
		public void Notify_ReservationAdded(ReservationManager.Reservation reservation)
		{
			Action<ReservationManager.Reservation> reservationAdded = this.ReservationAdded;
			if (reservationAdded == null)
			{
				return;
			}
			reservationAdded(reservation);
		}

		// Token: 0x06001EC7 RID: 7879 RVA: 0x000A1414 File Offset: 0x0009F614
		public void Notify_ReservationRemoved(ReservationManager.Reservation reservation)
		{
			Action<ReservationManager.Reservation> reservationRemoved = this.ReservationRemoved;
			if (reservationRemoved == null)
			{
				return;
			}
			reservationRemoved(reservation);
		}

		// Token: 0x14000012 RID: 18
		// (add) Token: 0x06001EC8 RID: 7880 RVA: 0x000A1428 File Offset: 0x0009F628
		// (remove) Token: 0x06001EC9 RID: 7881 RVA: 0x000A1460 File Offset: 0x0009F660
		public event Action<Thing, Pawn, ThingDef, int> HaulEnrouteAdded;

		// Token: 0x14000013 RID: 19
		// (add) Token: 0x06001ECA RID: 7882 RVA: 0x000A1498 File Offset: 0x0009F698
		// (remove) Token: 0x06001ECB RID: 7883 RVA: 0x000A14D0 File Offset: 0x0009F6D0
		public event Action<Thing, Pawn> HaulEnrouteReleased;

		// Token: 0x06001ECC RID: 7884 RVA: 0x000A1505 File Offset: 0x0009F705
		public void Notify_HaulEnrouteAdded(Thing enroute, Pawn pawn, ThingDef stuff, int count)
		{
			Action<Thing, Pawn, ThingDef, int> haulEnrouteAdded = this.HaulEnrouteAdded;
			if (haulEnrouteAdded == null)
			{
				return;
			}
			haulEnrouteAdded(enroute, pawn, stuff, count);
		}

		// Token: 0x06001ECD RID: 7885 RVA: 0x000A151C File Offset: 0x0009F71C
		public void Notify_HaulEnrouteReleased(Thing enroute, Pawn pawn)
		{
			Action<Thing, Pawn> haulEnrouteReleased = this.HaulEnrouteReleased;
			if (haulEnrouteReleased == null)
			{
				return;
			}
			haulEnrouteReleased(enroute, pawn);
		}

		// Token: 0x14000014 RID: 20
		// (add) Token: 0x06001ECE RID: 7886 RVA: 0x000A1530 File Offset: 0x0009F730
		// (remove) Token: 0x06001ECF RID: 7887 RVA: 0x000A1568 File Offset: 0x0009F768
		public event Action<IntVec3, bool> CellFogChanged;

		// Token: 0x14000015 RID: 21
		// (add) Token: 0x06001ED0 RID: 7888 RVA: 0x000A15A0 File Offset: 0x0009F7A0
		// (remove) Token: 0x06001ED1 RID: 7889 RVA: 0x000A15D8 File Offset: 0x0009F7D8
		public event Action MapFogged;

		// Token: 0x06001ED2 RID: 7890 RVA: 0x000A160D File Offset: 0x0009F80D
		public void Notify_CellFogChanged(IntVec3 cell, bool fogged)
		{
			Action<IntVec3, bool> cellFogChanged = this.CellFogChanged;
			if (cellFogChanged == null)
			{
				return;
			}
			cellFogChanged(cell, fogged);
		}

		// Token: 0x06001ED3 RID: 7891 RVA: 0x000A1621 File Offset: 0x0009F821
		public void Notify_MapFogged()
		{
			Action mapFogged = this.MapFogged;
			if (mapFogged == null)
			{
				return;
			}
			mapFogged();
		}

		// Token: 0x14000016 RID: 22
		// (add) Token: 0x06001ED4 RID: 7892 RVA: 0x000A1634 File Offset: 0x0009F834
		// (remove) Token: 0x06001ED5 RID: 7893 RVA: 0x000A166C File Offset: 0x0009F86C
		public event Action RegionsRoomsChanged;

		// Token: 0x06001ED6 RID: 7894 RVA: 0x000A16A1 File Offset: 0x0009F8A1
		public void Notify_RegionsRoomsChanged()
		{
			Action regionsRoomsChanged = this.RegionsRoomsChanged;
			if (regionsRoomsChanged == null)
			{
				return;
			}
			regionsRoomsChanged();
		}

		// Token: 0x14000017 RID: 23
		// (add) Token: 0x06001ED7 RID: 7895 RVA: 0x000A16B4 File Offset: 0x0009F8B4
		// (remove) Token: 0x06001ED8 RID: 7896 RVA: 0x000A16EC File Offset: 0x0009F8EC
		public event Action<Building_Door> DoorOpened;

		// Token: 0x14000018 RID: 24
		// (add) Token: 0x06001ED9 RID: 7897 RVA: 0x000A1724 File Offset: 0x0009F924
		// (remove) Token: 0x06001EDA RID: 7898 RVA: 0x000A175C File Offset: 0x0009F95C
		public event Action<Building_Door> DoorClosed;

		// Token: 0x06001EDB RID: 7899 RVA: 0x000A1791 File Offset: 0x0009F991
		public void Notify_DoorOpened(Building_Door door)
		{
			Action<Building_Door> doorOpened = this.DoorOpened;
			if (doorOpened == null)
			{
				return;
			}
			doorOpened(door);
		}

		// Token: 0x06001EDC RID: 7900 RVA: 0x000A17A4 File Offset: 0x0009F9A4
		public void Notify_DoorClosed(Building_Door door)
		{
			Action<Building_Door> doorClosed = this.DoorClosed;
			if (doorClosed == null)
			{
				return;
			}
			doorClosed(door);
		}

		// Token: 0x14000019 RID: 25
		// (add) Token: 0x06001EDD RID: 7901 RVA: 0x000A17B8 File Offset: 0x0009F9B8
		// (remove) Token: 0x06001EDE RID: 7902 RVA: 0x000A17F0 File Offset: 0x0009F9F0
		public event Action<IntVec3> RoofChanged;

		// Token: 0x06001EDF RID: 7903 RVA: 0x000A1825 File Offset: 0x0009FA25
		public void Notify_RoofChanged(IntVec3 cell)
		{
			Action<IntVec3> roofChanged = this.RoofChanged;
			if (roofChanged == null)
			{
				return;
			}
			roofChanged(cell);
		}

		// Token: 0x1400001A RID: 26
		// (add) Token: 0x06001EE0 RID: 7904 RVA: 0x000A1838 File Offset: 0x0009FA38
		// (remove) Token: 0x06001EE1 RID: 7905 RVA: 0x000A1870 File Offset: 0x0009FA70
		public event Action<IntVec3> GlowChanged;

		// Token: 0x06001EE2 RID: 7906 RVA: 0x000A18A5 File Offset: 0x0009FAA5
		public void Notify_GlowChanged(IntVec3 cell)
		{
			Action<IntVec3> glowChanged = this.GlowChanged;
			if (glowChanged == null)
			{
				return;
			}
			glowChanged(cell);
		}

		// Token: 0x04001A56 RID: 6742
		public readonly Map map;
	}
}
