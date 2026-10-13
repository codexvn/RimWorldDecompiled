using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000C45 RID: 3141
	public class DefaultWindowDrawing : IWindowDrawing
	{
		// Token: 0x17000C35 RID: 3125
		// (get) Token: 0x06005041 RID: 20545 RVA: 0x0019D45C File Offset: 0x0019B65C
		public GUIStyle EmptyStyle
		{
			get
			{
				return Widgets.EmptyStyle;
			}
		}

		// Token: 0x06005042 RID: 20546 RVA: 0x0019D464 File Offset: 0x0019B664
		public bool DoCloseButton(Rect rect, string text)
		{
			return Widgets.ButtonText(rect, text, true, true, true, null);
		}

		// Token: 0x06005043 RID: 20547 RVA: 0x0019D484 File Offset: 0x0019B684
		public bool DoClostButtonSmall(Rect rect)
		{
			return Widgets.CloseButtonFor(rect);
		}

		// Token: 0x06005044 RID: 20548 RVA: 0x0019D48C File Offset: 0x0019B68C
		public void DoGrayOut(Rect rect)
		{
			Widgets.DrawRectFast(rect, new Color(0f, 0f, 0f, 0.5f), null);
		}

		// Token: 0x06005045 RID: 20549 RVA: 0x0019D4AE File Offset: 0x0019B6AE
		public void DoWindowBackground(Rect rect)
		{
			Widgets.DrawWindowBackground(rect);
		}

		// Token: 0x06005046 RID: 20550 RVA: 0x0019D4B6 File Offset: 0x0019B6B6
		public void BeginGroup(Rect rect)
		{
			Widgets.BeginGroup(rect);
		}

		// Token: 0x06005047 RID: 20551 RVA: 0x00185E2E File Offset: 0x0018402E
		public void EndGroup()
		{
			Widgets.EndGroup();
		}
	}
}
