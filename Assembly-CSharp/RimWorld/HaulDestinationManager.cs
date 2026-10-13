using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020029A4 RID: 10660
	public sealed class HaulDestinationManager
	{
		// Token: 0x17002408 RID: 9224
		// (get) Token: 0x0600EAE8 RID: 60136 RVA: 0x00445403 File Offset: 0x00443603
		public IEnumerable<IHaulDestination> AllHaulDestinations
		{
			get
			{
				return this.allHaulDestinationsInOrder;
			}
		}

		// Token: 0x17002409 RID: 9225
		// (get) Token: 0x0600EAE9 RID: 60137 RVA: 0x00445403 File Offset: 0x00443603
		public List<IHaulDestination> AllHaulDestinationsListForReading
		{
			get
			{
				return this.allHaulDestinationsInOrder;
			}
		}

		// Token: 0x1700240A RID: 9226
		// (get) Token: 0x0600EAEA RID: 60138 RVA: 0x00445403 File Offset: 0x00443603
		public List<IHaulDestination> AllHaulDestinationsListInPriorityOrder
		{
			get
			{
				return this.allHaulDestinationsInOrder;
			}
		}

		// Token: 0x1700240B RID: 9227
		// (get) Token: 0x0600EAEB RID: 60139 RVA: 0x0044540B File Offset: 0x0044360B
		public List<IHaulSource> AllHaulSourcesListInPriorityOrder
		{
			get
			{
				return this.allHaulSourcesInOrder;
			}
		}

		// Token: 0x1700240C RID: 9228
		// (get) Token: 0x0600EAEC RID: 60140 RVA: 0x00445413 File Offset: 0x00443613
		public IEnumerable<SlotGroup> AllGroups
		{
			get
			{
				return this.allGroupsInOrder;
			}
		}

		// Token: 0x1700240D RID: 9229
		// (get) Token: 0x0600EAED RID: 60141 RVA: 0x00445413 File Offset: 0x00443613
		public List<SlotGroup> AllGroupsListForReading
		{
			get
			{
				return this.allGroupsInOrder;
			}
		}

		// Token: 0x1700240E RID: 9230
		// (get) Token: 0x0600EAEE RID: 60142 RVA: 0x0044540B File Offset: 0x0044360B
		public List<IHaulSource> AllHaulSourcesListForReading
		{
			get
			{
				return this.allHaulSourcesInOrder;
			}
		}

		// Token: 0x1700240F RID: 9231
		// (get) Token: 0x0600EAEF RID: 60143 RVA: 0x00445413 File Offset: 0x00443613
		public List<SlotGroup> AllGroupsListInPriorityOrder
		{
			get
			{
				return this.allGroupsInOrder;
			}
		}

		// Token: 0x17002410 RID: 9232
		// (get) Token: 0x0600EAF0 RID: 60144 RVA: 0x0044541B File Offset: 0x0044361B
		public IEnumerable<IntVec3> AllSlots
		{
			get
			{
				int num;
				for (int i = 0; i < this.allGroupsInOrder.Count; i = num + 1)
				{
					List<IntVec3> cellsList = this.allGroupsInOrder[i].CellsList;
					int j = 0;
					while (j < this.allGroupsInOrder.Count)
					{
						yield return cellsList[j];
						num = i;
						i = num + 1;
					}
					cellsList = null;
					num = i;
				}
				yield break;
			}
		}

		// Token: 0x0600EAF1 RID: 60145 RVA: 0x0044542C File Offset: 0x0044362C
		public HaulDestinationManager(Map map)
		{
			this.map = map;
			this.groupGrid = new SlotGroup[map.Size.x, map.Size.y, map.Size.z];
		}

		// Token: 0x0600EAF2 RID: 60146 RVA: 0x00445494 File Offset: 0x00443694
		public void AddHaulDestination(IHaulDestination haulDestination)
		{
			if (this.allHaulDestinationsInOrder.Contains(haulDestination))
			{
				Log.Error("Double-added haul destination " + haulDestination.ToStringSafe<IHaulDestination>());
				return;
			}
			this.allHaulDestinationsInOrder.Add(haulDestination);
			this.allHaulDestinationsInOrder.InsertionSort(new Comparison<IHaulDestination>(HaulDestinationManager.CompareHaulDestinationPrioritiesDescending));
			ISlotGroupParent slotGroupParent = haulDestination as ISlotGroupParent;
			if (slotGroupParent != null)
			{
				SlotGroup slotGroup = slotGroupParent.GetSlotGroup();
				if (slotGroup == null)
				{
					Log.Error("ISlotGroupParent gave null slot group: " + slotGroupParent.ToStringSafe<ISlotGroupParent>());
					return;
				}
				this.allGroupsInOrder.Add(slotGroup);
				this.allGroupsInOrder.InsertionSort(new Comparison<SlotGroup>(HaulDestinationManager.CompareSlotGroupPrioritiesDescending));
				List<IntVec3> cellsList = slotGroup.CellsList;
				for (int i = 0; i < cellsList.Count; i++)
				{
					this.SetCellFor(cellsList[i], slotGroup);
				}
				this.map.listerHaulables.Notify_SlotGroupChanged(slotGroup);
				this.map.listerMergeables.Notify_SlotGroupChanged(slotGroup);
			}
		}

		// Token: 0x0600EAF3 RID: 60147 RVA: 0x00445580 File Offset: 0x00443780
		public void RemoveHaulDestination(IHaulDestination haulDestination)
		{
			if (!this.allHaulDestinationsInOrder.Contains(haulDestination))
			{
				Log.Error("Removing haul destination that isn't registered " + haulDestination.ToStringSafe<IHaulDestination>());
				return;
			}
			this.allHaulDestinationsInOrder.Remove(haulDestination);
			ISlotGroupParent slotGroupParent = haulDestination as ISlotGroupParent;
			if (slotGroupParent == null)
			{
				return;
			}
			SlotGroup slotGroup = slotGroupParent.GetSlotGroup();
			if (slotGroup == null)
			{
				Log.Error("ISlotGroupParent gave null slot group: " + slotGroupParent.ToStringSafe<ISlotGroupParent>());
				return;
			}
			this.allGroupsInOrder.Remove(slotGroup);
			List<IntVec3> cellsList = slotGroup.CellsList;
			for (int i = 0; i < cellsList.Count; i++)
			{
				IntVec3 intVec = cellsList[i];
				this.groupGrid[intVec.x, intVec.y, intVec.z] = null;
			}
			this.map.listerHaulables.Notify_SlotGroupChanged(slotGroup);
			this.map.listerMergeables.Notify_SlotGroupChanged(slotGroup);
		}

		// Token: 0x0600EAF4 RID: 60148 RVA: 0x0044565C File Offset: 0x0044385C
		public void AddHaulSource(IHaulSource source)
		{
			if (this.allHaulSourcesInOrder.Contains(source))
			{
				Log.Error("Double-added haul destination " + source.ToStringSafe<IHaulSource>());
				return;
			}
			this.allHaulSourcesInOrder.Add(source);
			this.allHaulSourcesInOrder.InsertionSort(new Comparison<IHaulSource>(HaulDestinationManager.CompareHaulSourcePrioritiesDescending));
			this.map.listerHaulables.Notify_HaulSourceChanged(source);
		}

		// Token: 0x0600EAF5 RID: 60149 RVA: 0x004456C1 File Offset: 0x004438C1
		public void RemoveHaulSource(IHaulSource source)
		{
			if (!this.allHaulSourcesInOrder.Remove(source))
			{
				Log.Error("Removing haul source that isn't registered " + source.ToStringSafe<IHaulSource>());
			}
		}

		// Token: 0x0600EAF6 RID: 60150 RVA: 0x004456E8 File Offset: 0x004438E8
		public void Notify_HaulDestinationChangedPriority()
		{
			this.allHaulDestinationsInOrder.InsertionSort(new Comparison<IHaulDestination>(HaulDestinationManager.CompareHaulDestinationPrioritiesDescending));
			this.allGroupsInOrder.InsertionSort(new Comparison<SlotGroup>(HaulDestinationManager.CompareSlotGroupPrioritiesDescending));
			this.allHaulSourcesInOrder.InsertionSort(new Comparison<IHaulSource>(HaulDestinationManager.CompareHaulSourcePrioritiesDescending));
		}

		// Token: 0x0600EAF7 RID: 60151 RVA: 0x0044573C File Offset: 0x0044393C
		private static int CompareHaulDestinationPrioritiesDescending(IHaulDestination a, IHaulDestination b)
		{
			return ((int)b.GetStoreSettings().Priority).CompareTo((int)a.GetStoreSettings().Priority);
		}

		// Token: 0x0600EAF8 RID: 60152 RVA: 0x00445768 File Offset: 0x00443968
		private static int CompareHaulSourcePrioritiesDescending(IHaulSource a, IHaulSource b)
		{
			return ((int)b.GetStoreSettings().Priority).CompareTo((int)a.GetStoreSettings().Priority);
		}

		// Token: 0x0600EAF9 RID: 60153 RVA: 0x00445794 File Offset: 0x00443994
		private static int CompareSlotGroupPrioritiesDescending(SlotGroup a, SlotGroup b)
		{
			return ((int)b.Settings.Priority).CompareTo((int)a.Settings.Priority);
		}

		// Token: 0x0600EAFA RID: 60154 RVA: 0x004457BF File Offset: 0x004439BF
		public SlotGroup SlotGroupAt(IntVec3 loc)
		{
			return this.groupGrid[loc.x, loc.y, loc.z];
		}

		// Token: 0x0600EAFB RID: 60155 RVA: 0x004457DE File Offset: 0x004439DE
		public ISlotGroupParent SlotGroupParentAt(IntVec3 loc)
		{
			SlotGroup slotGroup = this.SlotGroupAt(loc);
			if (slotGroup == null)
			{
				return null;
			}
			return slotGroup.parent;
		}

		// Token: 0x0600EAFC RID: 60156 RVA: 0x004457F4 File Offset: 0x004439F4
		public void SetCellFor(IntVec3 c, SlotGroup group)
		{
			if (this.SlotGroupAt(c) != null)
			{
				string[] array = new string[5];
				array[0] = ((group != null) ? group.ToString() : null);
				array[1] = " overwriting slot group square ";
				int num = 2;
				IntVec3 intVec = c;
				array[num] = intVec.ToString();
				array[3] = " of ";
				int num2 = 4;
				SlotGroup slotGroup = this.SlotGroupAt(c);
				array[num2] = ((slotGroup != null) ? slotGroup.ToString() : null);
				Log.Error(string.Concat(array));
			}
			this.groupGrid[c.x, c.y, c.z] = group;
		}

		// Token: 0x0600EAFD RID: 60157 RVA: 0x00445880 File Offset: 0x00443A80
		public void ClearCellFor(IntVec3 c, SlotGroup group)
		{
			if (this.SlotGroupAt(c) != group)
			{
				string[] array = new string[5];
				array[0] = ((group != null) ? group.ToString() : null);
				array[1] = " clearing group grid square ";
				int num = 2;
				IntVec3 intVec = c;
				array[num] = intVec.ToString();
				array[3] = " containing ";
				int num2 = 4;
				SlotGroup slotGroup = this.SlotGroupAt(c);
				array[num2] = ((slotGroup != null) ? slotGroup.ToString() : null);
				Log.Error(string.Concat(array));
			}
			this.groupGrid[c.x, c.y, c.z] = null;
		}

		// Token: 0x04009C0F RID: 39951
		private Map map;

		// Token: 0x04009C10 RID: 39952
		private readonly List<IHaulDestination> allHaulDestinationsInOrder = new List<IHaulDestination>();

		// Token: 0x04009C11 RID: 39953
		private readonly List<IHaulSource> allHaulSourcesInOrder = new List<IHaulSource>();

		// Token: 0x04009C12 RID: 39954
		private readonly List<SlotGroup> allGroupsInOrder = new List<SlotGroup>();

		// Token: 0x04009C13 RID: 39955
		private readonly SlotGroup[,,] groupGrid;
	}
}
