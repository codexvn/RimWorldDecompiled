using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000C37 RID: 3127
	public class Dialog_RenameBuildingStorage_CreateNew : Dialog_Rename<IRenameable>
	{
		// Token: 0x06005015 RID: 20501 RVA: 0x0019CB62 File Offset: 0x0019AD62
		public Dialog_RenameBuildingStorage_CreateNew(IStorageGroupMember building) : base(null)
		{
			this.building = building;
			StorageGroup group = building.Group;
			this.curName = (((group != null) ? group.RenamableLabel : null) ?? string.Empty);
		}

		// Token: 0x06005016 RID: 20502 RVA: 0x0019CB94 File Offset: 0x0019AD94
		protected override AcceptanceReport NameIsValid(string name)
		{
			AcceptanceReport result = base.NameIsValid(name);
			if (!result.Accepted)
			{
				return result;
			}
			if (Current.Game.CurrentMap.zoneManager.AllZones.Any((Zone z) => z.label == name) || Current.Game.CurrentMap.storageGroups.HasGroupWithName(name))
			{
				return "NameIsInUse".Translate();
			}
			return true;
		}

		// Token: 0x06005017 RID: 20503 RVA: 0x0019CC20 File Offset: 0x0019AE20
		protected override void OnRenamed(string name)
		{
			StorageGroup storageGroup = this.building.Map.storageGroups.NewGroup(null);
			storageGroup.RenamableLabel = name;
			storageGroup.InitFrom(this.building);
			this.building.SetStorageGroup(storageGroup, true);
		}

		// Token: 0x040039BA RID: 14778
		private IStorageGroupMember building;
	}
}
