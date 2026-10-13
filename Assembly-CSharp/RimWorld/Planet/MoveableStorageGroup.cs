using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02003C57 RID: 15447
	public class MoveableStorageGroup : IExposable
	{
		// Token: 0x06014FC2 RID: 85954 RVA: 0x0064830C File Offset: 0x0064650C
		public MoveableStorageGroup()
		{
		}

		// Token: 0x06014FC3 RID: 85955 RVA: 0x0064831F File Offset: 0x0064651F
		public MoveableStorageGroup(StorageGroup storageGroup)
		{
			this.label = storageGroup.RenamableLabel;
			this.settings = new StorageSettings();
			this.settings.CopyFrom(storageGroup.GetStoreSettings());
		}

		// Token: 0x06014FC4 RID: 85956 RVA: 0x0064835A File Offset: 0x0064655A
		public void ExposeData()
		{
			Scribe_Collections.Look<IStorageGroupMember>(ref this.members, "members", LookMode.Reference, Array.Empty<object>());
			Scribe_Values.Look<string>(ref this.label, "label", null, false);
			Scribe_Deep.Look<StorageSettings>(ref this.settings, "settings", Array.Empty<object>());
		}

		// Token: 0x06014FC5 RID: 85957 RVA: 0x0064839C File Offset: 0x0064659C
		public void TryCreateStorageGroup(Map map)
		{
			StorageGroup storageGroup = map.storageGroups.NewGroup(this.label);
			storageGroup.GetStoreSettings().CopyFrom(this.settings);
			foreach (IStorageGroupMember member in this.members)
			{
				member.SetStorageGroup(storageGroup, true);
			}
		}

		// Token: 0x0400E9EC RID: 59884
		public List<IStorageGroupMember> members = new List<IStorageGroupMember>();

		// Token: 0x0400E9ED RID: 59885
		private string label;

		// Token: 0x0400E9EE RID: 59886
		private StorageSettings settings;
	}
}
