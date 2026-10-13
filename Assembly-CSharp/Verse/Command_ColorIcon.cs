using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000B83 RID: 2947
	[StaticConstructorOnStartup]
	public class Command_ColorIcon : Command_Action
	{
		// Token: 0x06004B7F RID: 19327 RVA: 0x0018087C File Offset: 0x0017EA7C
		public override GizmoResult GizmoOnGUI(Vector2 topLeft, float maxWidth, GizmoRenderParms parms)
		{
			GizmoResult result = base.GizmoOnGUI(topLeft, maxWidth, parms);
			if (this.color != null)
			{
				Rect rect = new Rect(topLeft.x, topLeft.y, this.GetWidth(maxWidth), 75f);
				RectDivider rectDivider = new RectDivider(rect.ContractedBy(4f), 1552930585, null);
				GUI.DrawTexture(rectDivider.NewCol(16f, HorizontalJustification.Right, null).NewRow(16f, VerticalJustification.Top, null), Command_ColorIcon.ColorIndicatorTex, ScaleMode.ScaleToFit, true, 1f, this.color.Value, 0f, 0f);
			}
			return result;
		}

		// Token: 0x040035CE RID: 13774
		private static readonly Texture2D ColorIndicatorTex = ContentFinder<Texture2D>.Get("UI/Icons/ColorIndicatorBulb", true);

		// Token: 0x040035CF RID: 13775
		public Color32? color;

		// Token: 0x040035D0 RID: 13776
		private const int colorCircleDiameter = 16;

		// Token: 0x040035D1 RID: 13777
		private const float colorCircleGap = 4f;
	}
}
