using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020033F0 RID: 13296
	public class MainButtonWorker_ToggleTab : MainButtonWorker
	{
		// Token: 0x06012B2A RID: 76586 RVA: 0x00589240 File Offset: 0x00587440
		public override void Activate()
		{
			Find.MainTabsRoot.ToggleTab(this.def, true);
		}
	}
}
