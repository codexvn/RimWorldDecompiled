using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020029B2 RID: 10674
	public class SlotGroup : ISlotGroup
	{
		// Token: 0x17002430 RID: 9264
		// (get) Token: 0x0600EB3B RID: 60219 RVA: 0x00445D53 File Offset: 0x00443F53
		private Map Map
		{
			get
			{
				return this.parent.Map;
			}
		}

		// Token: 0x17002431 RID: 9265
		// (get) Token: 0x0600EB3C RID: 60220 RVA: 0x00445D60 File Offset: 0x00443F60
		public StorageSettings Settings
		{
			get
			{
				return this.parent.GetStoreSettings();
			}
		}

		// Token: 0x17002432 RID: 9266
		// (get) Token: 0x0600EB3D RID: 60221 RVA: 0x00445D6D File Offset: 0x00443F6D
		public string GroupingLabel
		{
			get
			{
				return this.parent.GroupingLabel;
			}
		}

		// Token: 0x17002433 RID: 9267
		// (get) Token: 0x0600EB3E RID: 60222 RVA: 0x00445D7A File Offset: 0x00443F7A
		public int GroupingOrder
		{
			get
			{
				return this.parent.GroupingOrder;
			}
		}

		// Token: 0x17002434 RID: 9268
		// (get) Token: 0x0600EB3F RID: 60223 RVA: 0x00445D87 File Offset: 0x00443F87
		public IEnumerable<Thing> HeldThings
		{
			get
			{
				List<IntVec3> cellsList = this.CellsList;
				Map map = this.Map;
				int num;
				for (int i = 0; i < cellsList.Count; i = num + 1)
				{
					List<Thing> thingList = map.thingGrid.ThingsListAt(cellsList[i]);
					for (int j = 0; j < thingList.Count; j = num + 1)
					{
						if (thingList[j].def.EverStorable(false))
						{
							yield return thingList[j];
						}
						num = j;
					}
					thingList = null;
					num = i;
				}
				yield break;
			}
		}

		// Token: 0x17002435 RID: 9269
		// (get) Token: 0x0600EB40 RID: 60224 RVA: 0x00445D98 File Offset: 0x00443F98
		public int HeldThingsCount
		{
			get
			{
				int num = 0;
				List<IntVec3> cellsList = this.CellsList;
				ThingGrid thingGrid = this.Map.thingGrid;
				for (int i = 0; i < cellsList.Count; i++)
				{
					List<Thing> list = thingGrid.ThingsListAt(cellsList[i]);
					for (int j = 0; j < list.Count; j++)
					{
						if (list[j].def.EverStorable(false))
						{
							num++;
						}
					}
				}
				return num;
			}
		}

		// Token: 0x17002436 RID: 9270
		// (get) Token: 0x0600EB41 RID: 60225 RVA: 0x00445E0B File Offset: 0x0044400B
		public List<IntVec3> CellsList
		{
			get
			{
				return this.parent.AllSlotCellsList();
			}
		}

		// Token: 0x17002437 RID: 9271
		// (get) Token: 0x0600EB42 RID: 60226 RVA: 0x00445E18 File Offset: 0x00444018
		public StorageGroup StorageGroup
		{
			get
			{
				IStorageGroupMember storageGroupMember = this.parent as IStorageGroupMember;
				if (storageGroupMember != null)
				{
					return storageGroupMember.Group;
				}
				return null;
			}
		}

		// Token: 0x0600EB43 RID: 60227 RVA: 0x00445E3C File Offset: 0x0044403C
		public IEnumerator<IntVec3> GetEnumerator()
		{
			List<IntVec3> cellsList = this.CellsList;
			int num;
			for (int i = 0; i < cellsList.Count; i = num + 1)
			{
				yield return cellsList[i];
				num = i;
			}
			yield break;
		}

		// Token: 0x0600EB44 RID: 60228 RVA: 0x00445E4B File Offset: 0x0044404B
		public SlotGroup(ISlotGroupParent parent)
		{
			this.parent = parent;
		}

		// Token: 0x0600EB45 RID: 60229 RVA: 0x00445E5A File Offset: 0x0044405A
		public void Notify_AddedCell(IntVec3 c)
		{
			this.Map.haulDestinationManager.SetCellFor(c, this);
			this.Map.listerHaulables.RecalcAllInCell(c);
			this.Map.listerMergeables.RecalcAllInCell(c);
		}

		// Token: 0x0600EB46 RID: 60230 RVA: 0x00445E90 File Offset: 0x00444090
		public void Notify_LostCell(IntVec3 c)
		{
			this.Map.haulDestinationManager.ClearCellFor(c, this);
			this.Map.listerHaulables.RecalcAllInCell(c);
			this.Map.listerMergeables.RecalcAllInCell(c);
		}

		// Token: 0x0600EB47 RID: 60231 RVA: 0x00445EC8 File Offset: 0x004440C8
		public void RemoveHaulDesignationOnStoredThings()
		{
			if (this.parent.Map == null)
			{
				return;
			}
			foreach (Thing t in this.HeldThings)
			{
				if (this.Settings.AllowedToAccept(t))
				{
					Designation designation = this.Map.designationManager.DesignationOn(t, DesignationDefOf.Haul);
					if (designation != null)
					{
						this.Map.designationManager.RemoveDesignation(designation);
					}
				}
			}
		}

		// Token: 0x0600EB48 RID: 60232 RVA: 0x00445F58 File Offset: 0x00444158
		public override string ToString()
		{
			if (this.parent != null)
			{
				return this.parent.ToString();
			}
			return "NullParent";
		}

		// Token: 0x0600EB49 RID: 60233 RVA: 0x00445F74 File Offset: 0x00444174
		public string GetName()
		{
			Zone_Stockpile zone_Stockpile = this.parent as Zone_Stockpile;
			if (zone_Stockpile != null)
			{
				return zone_Stockpile.label;
			}
			Building_Storage building_Storage = this.parent as Building_Storage;
			if (building_Storage != null)
			{
				return building_Storage.Label;
			}
			return "UnresolvedSlotGroupName";
		}

		// Token: 0x0600EB4A RID: 60234 RVA: 0x00445FB4 File Offset: 0x004441B4
		public static string GetGroupLabel(ISlotGroup group)
		{
			SlotGroup slotGroup = group as SlotGroup;
			if (slotGroup != null)
			{
				return slotGroup.parent.SlotYielderLabel();
			}
			IRenameable renameable = group as IRenameable;
			if (renameable != null)
			{
				return renameable.RenamableLabel;
			}
			return "UNABLE TO GET LABEL";
		}

		// Token: 0x04009C26 RID: 39974
		public ISlotGroupParent parent;
	}
}
