using System;
using UnityEngine;
using Verse;

namespace LudeonTK
{
	// Token: 0x020000AF RID: 175
	public class MeasureWorldDistanceTool : MeasureTool
	{
		// Token: 0x060003C2 RID: 962 RVA: 0x00013279 File Offset: 0x00011479
		public MeasureWorldDistanceTool(string label, Action clickAction, Action onGUIAction = null)
		{
			this.label = label;
			this.clickAction = clickAction;
			this.onGUIAction = onGUIAction;
		}

		// Token: 0x060003C3 RID: 963 RVA: 0x00016568 File Offset: 0x00014768
		public MeasureWorldDistanceTool(string label, Action clickAction, Vector3 firstRectCorner)
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
