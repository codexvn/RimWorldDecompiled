using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020002A4 RID: 676
	public class MechWorkModeDef : Def
	{
		// Token: 0x170002D1 RID: 721
		// (get) Token: 0x060010B7 RID: 4279 RVA: 0x0004F064 File Offset: 0x0004D264
		public WorkModeDrawer Worker
		{
			get
			{
				if (this.workerInt == null)
				{
					this.workerInt = (WorkModeDrawer)Activator.CreateInstance(this.workerClass);
					this.workerInt.def = this;
				}
				return this.workerInt;
			}
		}

		// Token: 0x060010B8 RID: 4280 RVA: 0x0004F096 File Offset: 0x0004D296
		public override void PostLoad()
		{
			if (!string.IsNullOrEmpty(this.iconPath))
			{
				LongEventHandler.ExecuteWhenFinished(delegate
				{
					this.uiIcon = ContentFinder<Texture2D>.Get(this.iconPath, true);
				});
			}
		}

		// Token: 0x04000E04 RID: 3588
		[NoTranslate]
		public string iconPath;

		// Token: 0x04000E05 RID: 3589
		public Texture2D uiIcon;

		// Token: 0x04000E06 RID: 3590
		public int uiOrder;

		// Token: 0x04000E07 RID: 3591
		public bool ignoreGroupChargeLimits;

		// Token: 0x04000E08 RID: 3592
		public Type workerClass = typeof(WorkModeDrawer);

		// Token: 0x04000E09 RID: 3593
		[Unsaved(false)]
		private WorkModeDrawer workerInt;
	}
}
