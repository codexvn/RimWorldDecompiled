using System;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02003E4D RID: 15949
	[StaticConstructorOnStartup]
	public static class CompassWidget
	{
		// Token: 0x1700367C RID: 13948
		// (get) Token: 0x06015D04 RID: 89348 RVA: 0x00684C88 File Offset: 0x00682E88
		private static float Angle
		{
			get
			{
				Vector2 vector = GenWorldUI.WorldToUIPosition(Find.WorldGrid.NorthPolePos);
				Vector2 a = new Vector2((float)UI.screenWidth / 2f, (float)UI.screenHeight / 2f);
				vector.y = (float)UI.screenHeight - vector.y;
				return a.AngleTo(vector);
			}
		}

		// Token: 0x06015D05 RID: 89349 RVA: 0x00684CDC File Offset: 0x00682EDC
		public static void CompassOnGUI(ref float curBaseY)
		{
			CompassWidget.CompassOnGUI(new Vector2((float)UI.screenWidth - 10f - 32f, curBaseY - 10f - 32f));
			curBaseY -= 84f;
		}

		// Token: 0x06015D06 RID: 89350 RVA: 0x00684D14 File Offset: 0x00682F14
		private static void CompassOnGUI(Vector2 center)
		{
			Widgets.DrawTextureRotated(center, CompassWidget.CompassTex, CompassWidget.Angle, 1f, null);
			Rect rect = new Rect(center.x - 32f, center.y - 32f, 64f, 64f);
			TooltipHandler.TipRegionByKey(rect, "CompassTip");
			if (Widgets.ButtonInvisible(rect, true))
			{
				Find.WorldCameraDriver.RotateSoNorthIsUp(true);
			}
		}

		// Token: 0x0400F1E2 RID: 61922
		private const float Padding = 10f;

		// Token: 0x0400F1E3 RID: 61923
		private const float Size = 64f;

		// Token: 0x0400F1E4 RID: 61924
		private static readonly Texture2D CompassTex = ContentFinder<Texture2D>.Get("UI/Misc/Compass", true);
	}
}
