using System;

namespace RimWorld
{
	// Token: 0x020029AF RID: 10671
	public interface IStoreSettingsParent
	{
		// Token: 0x17002426 RID: 9254
		// (get) Token: 0x0600EB28 RID: 60200
		bool StorageTabVisible { get; }

		// Token: 0x0600EB29 RID: 60201
		StorageSettings GetStoreSettings();

		// Token: 0x0600EB2A RID: 60202
		StorageSettings GetParentStoreSettings();

		// Token: 0x0600EB2B RID: 60203
		void Notify_SettingsChanged();
	}
}
