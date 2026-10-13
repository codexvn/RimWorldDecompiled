using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02003184 RID: 12676
	public class Dialog_IdeosDuringLanding : Window
	{
		// Token: 0x17002E54 RID: 11860
		// (get) Token: 0x06011D4D RID: 73037 RVA: 0x0052C0FF File Offset: 0x0052A2FF
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(1010f, Mathf.Min(1000f, (float)UI.screenHeight));
			}
		}

		// Token: 0x06011D4E RID: 73038 RVA: 0x005106F7 File Offset: 0x0050E8F7
		public Dialog_IdeosDuringLanding() : base(null)
		{
			this.doCloseButton = true;
			this.forcePause = true;
			this.absorbInputAroundWindow = true;
		}

		// Token: 0x06011D4F RID: 73039 RVA: 0x0052C11C File Offset: 0x0052A31C
		public override void DoWindowContents(Rect inRect)
		{
			IdeoUIUtility.DoIdeoListAndDetails(new Rect(inRect.x, inRect.y, inRect.width, inRect.height - Window.CloseButSize.y), ref this.scrollPosition_ideoList, ref this.scrollViewHeight_ideoList, ref this.scrollPosition_ideoDetails, ref this.scrollViewHeight_ideoDetails, false, false, null, null, null, false, null, null, false, false, null);
		}

		// Token: 0x0400B867 RID: 47207
		private Vector2 scrollPosition_ideoList;

		// Token: 0x0400B868 RID: 47208
		private float scrollViewHeight_ideoList;

		// Token: 0x0400B869 RID: 47209
		private Vector2 scrollPosition_ideoDetails;

		// Token: 0x0400B86A RID: 47210
		private float scrollViewHeight_ideoDetails;
	}
}
