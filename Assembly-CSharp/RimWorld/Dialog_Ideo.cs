using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02003183 RID: 12675
	public class Dialog_Ideo : Window
	{
		// Token: 0x17002E53 RID: 11859
		// (get) Token: 0x06011D49 RID: 73033 RVA: 0x0052C06B File Offset: 0x0052A26B
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(600f, Mathf.Min(1000f, (float)UI.screenHeight));
			}
		}

		// Token: 0x06011D4A RID: 73034 RVA: 0x0052C087 File Offset: 0x0052A287
		public Dialog_Ideo(Ideo ideo) : base(null)
		{
			this.forcePause = true;
			this.doCloseX = true;
			this.doCloseButton = true;
			this.closeOnClickedOutside = true;
			this.absorbInputAroundWindow = true;
			this.ideo = ideo;
		}

		// Token: 0x06011D4B RID: 73035 RVA: 0x0051DAD4 File Offset: 0x0051BCD4
		public override void PostClose()
		{
			base.PostClose();
			IdeoUIUtility.UnselectCurrent();
		}

		// Token: 0x06011D4C RID: 73036 RVA: 0x0052C0BC File Offset: 0x0052A2BC
		public override void DoWindowContents(Rect inRect)
		{
			inRect.height -= Window.CloseButSize.y;
			IdeoUIUtility.DoIdeoDetails(inRect, this.ideo, ref this.scrollPosition, ref this.viewHeight, false, null, true, true, false, false);
		}

		// Token: 0x0400B864 RID: 47204
		private Ideo ideo;

		// Token: 0x0400B865 RID: 47205
		private float viewHeight;

		// Token: 0x0400B866 RID: 47206
		private Vector2 scrollPosition;
	}
}
