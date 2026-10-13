using System;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02003D7E RID: 15742
	public class Gizmo_CaravanInfo : Gizmo
	{
		// Token: 0x0601572F RID: 87855 RVA: 0x0066D1BC File Offset: 0x0066B3BC
		public Gizmo_CaravanInfo(Caravan caravan)
		{
			this.caravan = caravan;
			this.Order = -100f;
		}

		// Token: 0x06015730 RID: 87856 RVA: 0x0066D1D6 File Offset: 0x0066B3D6
		public override float GetWidth(float maxWidth)
		{
			return Mathf.Min(520f, maxWidth);
		}

		// Token: 0x06015731 RID: 87857 RVA: 0x0066D1E4 File Offset: 0x0066B3E4
		public override GizmoResult GizmoOnGUI(Vector2 topLeft, float maxWidth, GizmoRenderParms parms)
		{
			if (!this.caravan.Spawned)
			{
				return new GizmoResult(GizmoState.Clear);
			}
			Rect rect = new Rect(topLeft.x, topLeft.y, this.GetWidth(maxWidth), 75f);
			Widgets.DrawWindowBackground(rect);
			Widgets.BeginGroup(rect);
			Rect rect2 = rect.AtZero();
			int? ticksToArrive = this.caravan.pather.Moving ? new int?(CaravanArrivalTimeEstimator.EstimatedTicksToArrive(this.caravan, true)) : null;
			StringBuilder stringBuilder = new StringBuilder();
			float tilesPerDay = TilesPerDayCalculator.ApproxTilesPerDay(this.caravan, stringBuilder);
			CaravanUIUtility.DrawCaravanInfo(new CaravanUIUtility.CaravanInfo(this.caravan.MassUsage, this.caravan.MassCapacity, this.caravan.MassCapacityExplanation, tilesPerDay, stringBuilder.ToString(), this.caravan.DaysWorthOfFood, this.caravan.forage.ForagedFoodPerDay, this.caravan.forage.ForagedFoodPerDayExplanation, this.caravan.Visibility, this.caravan.VisibilityExplanation, -1f, -1f, null), null, this.caravan.Tile, ticksToArrive, -9999f, rect2, true, null, true, true);
			Widgets.EndGroup();
			GenUI.AbsorbClicksInRect(rect);
			return new GizmoResult(GizmoState.Clear);
		}

		// Token: 0x0400EECA RID: 61130
		private Caravan caravan;
	}
}
