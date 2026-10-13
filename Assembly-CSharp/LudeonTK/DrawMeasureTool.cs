using System;
using UnityEngine;
using Verse;

namespace LudeonTK
{
	// Token: 0x020000A0 RID: 160
	public class DrawMeasureTool : MeasureTool
	{
		// Token: 0x06000369 RID: 873 RVA: 0x00013279 File Offset: 0x00011479
		public DrawMeasureTool(string label, Action clickAction, Action onGUIAction = null)
		{
			this.label = label;
			this.clickAction = clickAction;
			this.onGUIAction = onGUIAction;
		}

		// Token: 0x0600036A RID: 874 RVA: 0x00013298 File Offset: 0x00011498
		public DrawMeasureTool(string label, Action clickAction, Vector3 firstRectCorner)
		{
			this.label = label;
			this.clickAction = clickAction;
			this.onGUIAction = delegate()
			{
				Vector3 v = UI.MouseMapPosition();
				Vector2 start = firstRectCorner.MapToUIPosition();
				Vector2 end = v.MapToUIPosition();
				DevGUI.DrawLine(start, end, Color.white, 0.25f);
			};
		}
	}
}
