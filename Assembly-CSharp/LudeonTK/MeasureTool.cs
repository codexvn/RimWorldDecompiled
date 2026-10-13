using System;
using UnityEngine;
using Verse;

namespace LudeonTK
{
	// Token: 0x020000AE RID: 174
	public class MeasureTool
	{
		// Token: 0x060003C0 RID: 960 RVA: 0x000164BC File Offset: 0x000146BC
		public void DebugToolOnGUI()
		{
			if (Event.current.type == EventType.MouseDown)
			{
				if (Event.current.button == 0)
				{
					this.clickAction();
				}
				if (Event.current.button == 1)
				{
					DebugTools.curMeasureTool = null;
				}
				Event.current.Use();
			}
			Vector2 vector = Event.current.mousePosition + new Vector2(15f, 15f);
			Rect rect = new Rect(vector.x, vector.y, 999f, 999f);
			Text.Font = GameFont.Small;
			DevGUI.Label(rect, this.label);
			if (this.onGUIAction != null)
			{
				this.onGUIAction();
			}
		}

		// Token: 0x0400025F RID: 607
		public string label;

		// Token: 0x04000260 RID: 608
		public Action clickAction;

		// Token: 0x04000261 RID: 609
		public Action onGUIAction;
	}
}
