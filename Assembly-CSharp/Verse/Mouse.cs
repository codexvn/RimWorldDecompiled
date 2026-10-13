using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000BFA RID: 3066
	public static class Mouse
	{
		// Token: 0x17000C06 RID: 3078
		// (get) Token: 0x06004E68 RID: 20072 RVA: 0x0018EF1C File Offset: 0x0018D11C
		public static bool IsInputBlockedNow
		{
			get
			{
				WindowStack windowStack = Find.WindowStack;
				return (Widgets.mouseOverScrollViewStack.Count > 0 && !Widgets.mouseOverScrollViewStack.Peek()) || windowStack.MouseObscuredNow || !windowStack.CurrentWindowGetsInput;
			}
		}

		// Token: 0x06004E69 RID: 20073 RVA: 0x0018EF5F File Offset: 0x0018D15F
		public static bool IsOver(Rect rect)
		{
			return rect.Contains(Event.current.mousePosition) && !Mouse.IsInputBlockedNow;
		}
	}
}
