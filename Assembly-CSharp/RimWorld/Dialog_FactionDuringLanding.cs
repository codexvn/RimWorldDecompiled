using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02003176 RID: 12662
	public class Dialog_FactionDuringLanding : Window
	{
		// Token: 0x17002E38 RID: 11832
		// (get) Token: 0x06011CD1 RID: 72913 RVA: 0x002C6FD9 File Offset: 0x002C51D9
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(1010f, 684f);
			}
		}

		// Token: 0x06011CD2 RID: 72914 RVA: 0x00527D80 File Offset: 0x00525F80
		public Dialog_FactionDuringLanding() : base(null)
		{
			this.doCloseButton = true;
			this.forcePause = true;
			this.absorbInputAroundWindow = true;
		}

		// Token: 0x06011CD3 RID: 72915 RVA: 0x00527DA9 File Offset: 0x00525FA9
		public override void DoWindowContents(Rect inRect)
		{
			FactionUIUtility.DoWindowContents(new Rect(inRect.x, inRect.y, inRect.width, inRect.height - Window.CloseButSize.y), ref this.scrollPosition, ref this.scrollViewHeight, null);
		}

		// Token: 0x0400B7F9 RID: 47097
		private Vector2 scrollPosition = Vector2.zero;

		// Token: 0x0400B7FA RID: 47098
		private float scrollViewHeight;
	}
}
