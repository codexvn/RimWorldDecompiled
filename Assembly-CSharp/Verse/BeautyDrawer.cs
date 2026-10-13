using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000B73 RID: 2931
	public static class BeautyDrawer
	{
		// Token: 0x06004B27 RID: 19239 RVA: 0x0017E2CF File Offset: 0x0017C4CF
		public static void BeautyDrawerOnGUI()
		{
			if (Event.current.type != EventType.Repaint || !BeautyDrawer.ShouldShow())
			{
				return;
			}
			BeautyDrawer.DrawBeautyAroundMouse();
		}

		// Token: 0x06004B28 RID: 19240 RVA: 0x0017E2EC File Offset: 0x0017C4EC
		private static bool ShouldShow()
		{
			return !Mouse.IsInputBlockedNow && UI.MouseCell().InBounds(Find.CurrentMap) && !UI.MouseCell().Fogged(Find.CurrentMap) && (CellInspectorDrawer.active || Find.PlaySettings.showBeauty);
		}

		// Token: 0x06004B29 RID: 19241 RVA: 0x0017E340 File Offset: 0x0017C540
		private static void DrawBeautyAroundMouse()
		{
			BeautyUtility.FillBeautyRelevantCells(UI.MouseCell(), Find.CurrentMap);
			for (int i = 0; i < BeautyUtility.beautyRelevantCells.Count; i++)
			{
				IntVec3 intVec = BeautyUtility.beautyRelevantCells[i];
				float num = BeautyUtility.CellBeauty(intVec, Find.CurrentMap, BeautyDrawer.beautyCountedThings);
				if (num != 0f)
				{
					GenMapUI.DrawThingLabel(GenMapUI.LabelDrawPosFor(intVec), Mathf.RoundToInt(num).ToStringCached(), BeautyDrawer.BeautyColor(num, 8f));
				}
			}
			BeautyDrawer.beautyCountedThings.Clear();
		}

		// Token: 0x06004B2A RID: 19242 RVA: 0x0017E3CC File Offset: 0x0017C5CC
		public static Color BeautyColor(float beauty, float scale)
		{
			float num = Mathf.InverseLerp(-scale, scale, beauty);
			num = Mathf.Clamp01(num);
			return Color.Lerp(Color.Lerp(BeautyDrawer.ColorUgly, BeautyDrawer.ColorBeautiful, num), Color.white, 0.5f);
		}

		// Token: 0x04003579 RID: 13689
		private static HashSet<Thing> beautyCountedThings = new HashSet<Thing>();

		// Token: 0x0400357A RID: 13690
		private static Color ColorUgly = Color.red;

		// Token: 0x0400357B RID: 13691
		private static Color ColorBeautiful = Color.green;
	}
}
