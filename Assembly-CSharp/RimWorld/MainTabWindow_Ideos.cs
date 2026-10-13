using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020033FD RID: 13309
	public class MainTabWindow_Ideos : MainTabWindow
	{
		// Token: 0x17003105 RID: 12549
		// (get) Token: 0x06012B6F RID: 76655 RVA: 0x0058B034 File Offset: 0x00589234
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(base.InitialSize.x, (float)(UI.screenHeight - 35));
			}
		}

		// Token: 0x06012B70 RID: 76656 RVA: 0x0058B04F File Offset: 0x0058924F
		public override void PreOpen()
		{
			base.PreOpen();
			this.scrollPosition_ideoDetails = Vector2.zero;
		}

		// Token: 0x06012B71 RID: 76657 RVA: 0x0051DAD4 File Offset: 0x0051BCD4
		public override void PostClose()
		{
			base.PostClose();
			IdeoUIUtility.UnselectCurrent();
		}

		// Token: 0x06012B72 RID: 76658 RVA: 0x0058B064 File Offset: 0x00589264
		public override void DoWindowContents(Rect rect)
		{
			IdeoUIUtility.DoIdeoListAndDetails(rect, ref this.scrollPosition_ideoList, ref this.scrollViewHeight_ideoList, ref this.scrollPosition_ideoDetails, ref this.scrollViewHeight_ideoDetails, false, false, null, null, null, false, null, null, false, false, null);
		}

		// Token: 0x0400C264 RID: 49764
		private Vector2 scrollPosition_ideoList;

		// Token: 0x0400C265 RID: 49765
		private float scrollViewHeight_ideoList;

		// Token: 0x0400C266 RID: 49766
		private Vector2 scrollPosition_ideoDetails;

		// Token: 0x0400C267 RID: 49767
		private float scrollViewHeight_ideoDetails;
	}
}
