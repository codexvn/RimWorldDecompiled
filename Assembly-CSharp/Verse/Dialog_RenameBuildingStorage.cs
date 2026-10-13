using System;

namespace Verse
{
	// Token: 0x02000C35 RID: 3125
	public class Dialog_RenameBuildingStorage : Dialog_Rename<IRenameable>
	{
		// Token: 0x06005011 RID: 20497 RVA: 0x0019CAB8 File Offset: 0x0019ACB8
		public Dialog_RenameBuildingStorage(IRenameable storage) : base(storage)
		{
		}

		// Token: 0x06005012 RID: 20498 RVA: 0x0019CAC4 File Offset: 0x0019ACC4
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
	}
}
