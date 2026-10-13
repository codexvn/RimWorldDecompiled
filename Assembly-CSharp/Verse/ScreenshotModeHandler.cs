using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000BD3 RID: 3027
	public class ScreenshotModeHandler
	{
		// Token: 0x17000BEE RID: 3054
		// (get) Token: 0x06004D69 RID: 19817 RVA: 0x0018A08A File Offset: 0x0018828A
		// (set) Token: 0x06004D6A RID: 19818 RVA: 0x0018A092 File Offset: 0x00188292
		public bool Active
		{
			get
			{
				return this.active;
			}
			set
			{
				this.active = value;
			}
		}

		// Token: 0x17000BEF RID: 3055
		// (get) Token: 0x06004D6B RID: 19819 RVA: 0x0018A09C File Offset: 0x0018829C
		public bool FiltersCurrentEvent
		{
			get
			{
				return this.active && (Event.current.type == EventType.Repaint || Event.current.type == EventType.Layout || (Event.current.type == EventType.MouseDown || Event.current.type == EventType.MouseUp || Event.current.type == EventType.MouseDrag));
			}
		}

		// Token: 0x06004D6C RID: 19820 RVA: 0x0018A0F8 File Offset: 0x001882F8
		public void ScreenshotModesOnGUI()
		{
			if (KeyBindingDefOf.ToggleScreenshotMode.KeyDownEvent && !WorldComponent_GravshipController.CutsceneInProgress)
			{
				this.active = !this.active;
				Event.current.Use();
			}
		}

		// Token: 0x0400370B RID: 14091
		private bool active;
	}
}
