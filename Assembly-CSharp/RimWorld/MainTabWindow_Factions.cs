using System;
using UnityEngine;

namespace RimWorld
{
	// Token: 0x020033F8 RID: 13304
	public class MainTabWindow_Factions : MainTabWindow
	{
		// Token: 0x06012B57 RID: 76631 RVA: 0x00589DD6 File Offset: 0x00587FD6
		public override void PreOpen()
		{
			base.PreOpen();
			this.scrollToFaction = null;
		}

		// Token: 0x06012B58 RID: 76632 RVA: 0x00589DE5 File Offset: 0x00587FE5
		public void ScrollToFaction(Faction faction)
		{
			this.scrollToFaction = faction;
		}

		// Token: 0x06012B59 RID: 76633 RVA: 0x00589DEE File Offset: 0x00587FEE
		public override void DoWindowContents(Rect fillRect)
		{
			FactionUIUtility.DoWindowContents(fillRect, ref this.scrollPosition, ref this.scrollViewHeight, this.scrollToFaction);
			if (this.scrollToFaction != null)
			{
				this.scrollToFaction = null;
			}
		}

		// Token: 0x0400C23E RID: 49726
		private Vector2 scrollPosition;

		// Token: 0x0400C23F RID: 49727
		private float scrollViewHeight;

		// Token: 0x0400C240 RID: 49728
		private Faction scrollToFaction;
	}
}
