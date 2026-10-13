using System;
using System.Collections.Generic;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020034AD RID: 13485
	public static class TargetHighlighter
	{
		// Token: 0x06012F51 RID: 77649 RVA: 0x0059F04C File Offset: 0x0059D24C
		public static void Highlight(GlobalTargetInfo target, bool arrow = true, bool colonistBar = true, bool circleOverlay = false)
		{
			if (!target.IsValid)
			{
				return;
			}
			if (arrow)
			{
				GlobalTargetInfo adjustedTarget = CameraJumper.GetAdjustedTarget(target);
				if (adjustedTarget.IsMapTarget && adjustedTarget.Map != null && adjustedTarget.Map == Find.CurrentMap)
				{
					Vector3 centerVector = ((TargetInfo)adjustedTarget).CenterVector3;
					if (!TargetHighlighter.arrowPositions.Contains(centerVector))
					{
						TargetHighlighter.arrowPositions.Add(centerVector);
					}
				}
			}
			if (colonistBar)
			{
				if (target.Thing is Pawn)
				{
					Find.ColonistBar.Highlight((Pawn)target.Thing);
				}
				else if (target.Thing is Corpse)
				{
					Find.ColonistBar.Highlight(((Corpse)target.Thing).InnerPawn);
				}
				else if (target.WorldObject is Caravan)
				{
					Caravan caravan = (Caravan)target.WorldObject;
					if (caravan != null)
					{
						for (int i = 0; i < caravan.pawns.Count; i++)
						{
							Find.ColonistBar.Highlight(caravan.pawns[i]);
						}
					}
				}
			}
			if (circleOverlay && target.Thing != null && target.Thing.Spawned && target.Thing.Map == Find.CurrentMap)
			{
				Pawn pawn = target.Thing as Pawn;
				float num;
				if (pawn != null)
				{
					if (pawn.RaceProps.Humanlike)
					{
						num = 1.6f;
					}
					else
					{
						num = pawn.ageTracker.CurLifeStage.bodySizeFactor * pawn.ageTracker.CurKindLifeStage.bodyGraphicData.drawSize.y;
						num = Mathf.Max(num, 1f);
					}
				}
				else
				{
					num = (float)Mathf.Max(target.Thing.def.size.x, target.Thing.def.size.z);
				}
				Pair<Vector3, float> item = new Pair<Vector3, float>(target.Thing.DrawPos, num * 0.5f);
				if (!TargetHighlighter.circleOverlays.Contains(item))
				{
					TargetHighlighter.circleOverlays.Add(item);
				}
			}
		}

		// Token: 0x06012F52 RID: 77650 RVA: 0x0059F264 File Offset: 0x0059D464
		public static void TargetHighlighterUpdate()
		{
			if (WorldRendererUtility.DrawingMap)
			{
				for (int i = 0; i < TargetHighlighter.arrowPositions.Count; i++)
				{
					GenDraw.DrawArrowPointingAt(TargetHighlighter.arrowPositions[i], false);
				}
			}
			TargetHighlighter.arrowPositions.Clear();
			if (WorldRendererUtility.DrawingMap)
			{
				for (int j = 0; j < TargetHighlighter.circleOverlays.Count; j++)
				{
					GenDraw.DrawCircleOutline(TargetHighlighter.circleOverlays[j].First, TargetHighlighter.circleOverlays[j].Second);
				}
			}
			TargetHighlighter.circleOverlays.Clear();
		}

		// Token: 0x0400C480 RID: 50304
		private static List<Vector3> arrowPositions = new List<Vector3>();

		// Token: 0x0400C481 RID: 50305
		private static List<Pair<Vector3, float>> circleOverlays = new List<Pair<Vector3, float>>();
	}
}
