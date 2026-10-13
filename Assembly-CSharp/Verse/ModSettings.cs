using System;

namespace Verse
{
	// Token: 0x0200058E RID: 1422
	public abstract class ModSettings : IExposable
	{
		// Token: 0x170006B5 RID: 1717
		// (get) Token: 0x06002541 RID: 9537 RVA: 0x000C08B9 File Offset: 0x000BEAB9
		// (set) Token: 0x06002542 RID: 9538 RVA: 0x000C08C1 File Offset: 0x000BEAC1
		public Mod Mod { get; internal set; }

		// Token: 0x06002543 RID: 9539 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void ExposeData()
		{
		}

		// Token: 0x06002544 RID: 9540 RVA: 0x000C08CA File Offset: 0x000BEACA
		public void Write()
		{
			LoadedModManager.WriteModSettings(this.Mod.Content.FolderName, this.Mod.GetType().Name, this);
		}
	}
}
