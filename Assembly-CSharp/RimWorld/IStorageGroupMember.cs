using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020029AE RID: 10670
	public interface IStorageGroupMember
	{
		// Token: 0x1700241D RID: 9245
		// (get) Token: 0x0600EB1E RID: 60190
		// (set) Token: 0x0600EB1F RID: 60191
		StorageGroup Group { get; set; }

		// Token: 0x1700241E RID: 9246
		// (get) Token: 0x0600EB20 RID: 60192
		Map Map { get; }

		// Token: 0x1700241F RID: 9247
		// (get) Token: 0x0600EB21 RID: 60193
		StorageSettings StoreSettings { get; }

		// Token: 0x17002420 RID: 9248
		// (get) Token: 0x0600EB22 RID: 60194
		StorageSettings ParentStoreSettings { get; }

		// Token: 0x17002421 RID: 9249
		// (get) Token: 0x0600EB23 RID: 60195
		StorageSettings ThingStoreSettings { get; }

		// Token: 0x17002422 RID: 9250
		// (get) Token: 0x0600EB24 RID: 60196
		string StorageGroupTag { get; }

		// Token: 0x17002423 RID: 9251
		// (get) Token: 0x0600EB25 RID: 60197
		bool DrawConnectionOverlay { get; }

		// Token: 0x17002424 RID: 9252
		// (get) Token: 0x0600EB26 RID: 60198
		bool DrawStorageTab { get; }

		// Token: 0x17002425 RID: 9253
		// (get) Token: 0x0600EB27 RID: 60199
		bool ShowRenameButton { get; }
	}
}
