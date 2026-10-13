using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000B81 RID: 2945
	[StaticConstructorOnStartup]
	public class Command_ActionWithCooldown : Command_Action
	{
		// Token: 0x06004B79 RID: 19321 RVA: 0x00180768 File Offset: 0x0017E968
		public override GizmoResult GizmoOnGUI(Vector2 topLeft, float maxWidth, GizmoRenderParms parms)
		{
			GizmoResult result = base.GizmoOnGUI(topLeft, maxWidth, parms);
			Rect rect = new Rect(topLeft.x, topLeft.y, this.GetWidth(maxWidth), 75f);
			if (this.cooldownPercentGetter != null)
			{
				float num = this.cooldownPercentGetter();
				if (num < 1f)
				{
					Widgets.FillableBar(rect, Mathf.Clamp01(num), Command_ActionWithCooldown.CooldownBarTex, null, false);
					Text.Font = GameFont.Tiny;
					Text.Anchor = TextAnchor.UpperCenter;
					Widgets.Label(rect, num.ToStringPercent("F0"));
					Text.Anchor = TextAnchor.UpperLeft;
				}
			}
			return result;
		}

		// Token: 0x040035CA RID: 13770
		public Func<float> cooldownPercentGetter;

		// Token: 0x040035CB RID: 13771
		private static readonly Texture2D CooldownBarTex = SolidColorMaterials.NewSolidColorTexture(new Color32(9, 203, 4, 64));
	}
}
