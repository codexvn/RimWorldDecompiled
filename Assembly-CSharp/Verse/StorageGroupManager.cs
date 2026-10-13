using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;

namespace Verse
{
	// Token: 0x02000531 RID: 1329
	public class StorageGroupManager : IExposable
	{
		// Token: 0x17000624 RID: 1572
		// (get) Token: 0x06002275 RID: 8821 RVA: 0x000B32DB File Offset: 0x000B14DB
		public List<StorageGroup> StorageGroupsForReading
		{
			get
			{
				return this.groups;
			}
		}

		// Token: 0x06002276 RID: 8822 RVA: 0x000B32E3 File Offset: 0x000B14E3
		public StorageGroupManager(Map map)
		{
			this.map = map;
		}

		// Token: 0x06002277 RID: 8823 RVA: 0x000B3300 File Offset: 0x000B1500
		public StorageGroup NewGroup(string label = null)
		{
			StorageGroup storageGroup = new StorageGroup(this.map, label);
			storageGroup.loadID = Find.UniqueIDsManager.GetNextStorageGroupID();
			this.groups.Add(storageGroup);
			return storageGroup;
		}

		// Token: 0x06002278 RID: 8824 RVA: 0x000B3338 File Offset: 0x000B1538
		public void Notify_MemberRemoved(StorageGroup group)
		{
			if (group.MemberCount <= 1)
			{
				for (int i = group.MemberCount - 1; i >= 0; i--)
				{
					group.members[i].SetStorageGroup(null, true);
				}
				this.groups.Remove(group);
			}
		}

		// Token: 0x06002279 RID: 8825 RVA: 0x000B3384 File Offset: 0x000B1584
		public static string NewStorageName(string nameBase)
		{
			for (int i = 1; i <= 1000; i++)
			{
				string cand = nameBase + " " + i.ToString();
				if (!Current.Game.CurrentMap.zoneManager.AllZones.Any((Zone z) => z.label == cand) && !Current.Game.CurrentMap.haulDestinationManager.AllGroups.Any(delegate(SlotGroup x)
				{
					Building_Storage building_Storage = x.parent as Building_Storage;
					return building_Storage != null && building_Storage.label == cand;
				}) && !Current.Game.CurrentMap.storageGroups.groups.Any((StorageGroup g) => g.RenamableLabel == cand))
				{
					return cand;
				}
			}
			Log.Error("Ran out of storage " + nameBase + " names.");
			return nameBase + " X";
		}

		// Token: 0x0600227A RID: 8826 RVA: 0x000B3464 File Offset: 0x000B1664
		public bool HasGroupWithName(string label)
		{
			return this.groups.Any((StorageGroup x) => x.RenamableLabel == label);
		}

		// Token: 0x0600227B RID: 8827 RVA: 0x000B3495 File Offset: 0x000B1695
		public bool HasStorageGroup(StorageGroup group)
		{
			return this.groups.Contains(group);
		}

		// Token: 0x0600227C RID: 8828 RVA: 0x000B34A3 File Offset: 0x000B16A3
		public void ExposeData()
		{
			Scribe_Collections.Look<StorageGroup>(ref this.groups, "groups", LookMode.Deep, Array.Empty<object>());
			Scribe_References.Look<Map>(ref this.map, "map", false);
		}

		// Token: 0x04001CF1 RID: 7409
		public Map map;

		// Token: 0x04001CF2 RID: 7410
		private List<StorageGroup> groups = new List<StorageGroup>();
	}
}
