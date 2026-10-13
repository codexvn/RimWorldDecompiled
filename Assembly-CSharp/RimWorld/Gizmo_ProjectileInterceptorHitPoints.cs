using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02002DC0 RID: 11712
	[StaticConstructorOnStartup]
	public class Gizmo_ProjectileInterceptorHitPoints : Gizmo
	{
		// Token: 0x0601078F RID: 67471 RVA: 0x0047FEB8 File Offset: 0x0047E0B8
		public Gizmo_ProjectileInterceptorHitPoints()
		{
			this.Order = -100f;
		}

		// Token: 0x06010790 RID: 67472 RVA: 0x0047FECB File Offset: 0x0047E0CB
		public override float GetWidth(float maxWidth)
		{
			return 140f;
		}

		// Token: 0x06010791 RID: 67473 RVA: 0x004C09DC File Offset: 0x004BEBDC
		public override GizmoResult GizmoOnGUI(Vector2 topLeft, float maxWidth, GizmoRenderParms parms)
		{
			Rect rect = new Rect(topLeft.x, topLeft.y, this.GetWidth(maxWidth), 75f);
			Rect rect2 = rect.ContractedBy(6f);
			Widgets.DrawWindowBackground(rect);
			bool flag = this.interceptor.ChargingTicksLeft > 0;
			TaggedString label = (!flag) ? "ShieldEnergy".Translate() : "ShieldTimeToRecovery".Translate();
			float fillPercent = (!flag) ? ((float)this.interceptor.currentHitPoints / (float)this.interceptor.HitPointsMax) : ((float)this.interceptor.ChargingTicksLeft / (float)this.interceptor.Props.chargeDurationTicks);
			string label2 = (!flag) ? (this.interceptor.currentHitPoints.ToString() + " / " + this.interceptor.HitPointsMax.ToString()) : this.interceptor.ChargingTicksLeft.ToStringTicksToPeriod(true, false, true, true, false);
			Text.Font = GameFont.Small;
			Text.Anchor = TextAnchor.UpperLeft;
			Rect rect3 = new Rect(rect2.x, rect2.y - 2f, rect2.width, rect2.height / 2f);
			Widgets.Label(rect3, label);
			Rect rect4 = new Rect(rect2.x, rect3.yMax, rect2.width, rect2.height / 2f);
			Widgets.FillableBar(rect4, fillPercent, Gizmo_ProjectileInterceptorHitPoints.FullBarTex, Gizmo_ProjectileInterceptorHitPoints.EmptyBarTex, false);
			Text.Font = GameFont.Small;
			Text.Anchor = TextAnchor.MiddleCenter;
			Widgets.Label(rect4, label2);
			Text.Anchor = TextAnchor.UpperLeft;
			if (!this.interceptor.Props.gizmoTipKey.NullOrEmpty())
			{
				TooltipHandler.TipRegion(rect2, this.interceptor.Props.gizmoTipKey.Translate());
			}
			return new GizmoResult(GizmoState.Clear);
		}

		// Token: 0x0400ABD3 RID: 43987
		public CompProjectileInterceptor interceptor;

		// Token: 0x0400ABD4 RID: 43988
		private static readonly Texture2D FullBarTex = SolidColorMaterials.NewSolidColorTexture(new Color(0.2f, 0.2f, 0.24f));

		// Token: 0x0400ABD5 RID: 43989
		private static readonly Texture2D EmptyBarTex = SolidColorMaterials.NewSolidColorTexture(Color.clear);

		// Token: 0x0400ABD6 RID: 43990
		private const float Width = 140f;

		// Token: 0x0400ABD7 RID: 43991
		public const int InRectPadding = 6;
	}
}
