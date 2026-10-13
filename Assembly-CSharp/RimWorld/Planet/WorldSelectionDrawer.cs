using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02003E86 RID: 16006
	public static class WorldSelectionDrawer
	{
		// Token: 0x170036C2 RID: 14018
		// (get) Token: 0x06015E63 RID: 89699 RVA: 0x0068D75B File Offset: 0x0068B95B
		public static Dictionary<WorldObject, float> SelectTimes
		{
			get
			{
				return WorldSelectionDrawer.selectTimes;
			}
		}

		// Token: 0x06015E64 RID: 89700 RVA: 0x0068D762 File Offset: 0x0068B962
		public static void Notify_Selected(WorldObject t)
		{
			WorldSelectionDrawer.selectTimes[t] = Time.realtimeSinceStartup;
		}

		// Token: 0x06015E65 RID: 89701 RVA: 0x0068D774 File Offset: 0x0068B974
		public static void Clear()
		{
			WorldSelectionDrawer.selectTimes.Clear();
		}

		// Token: 0x06015E66 RID: 89702 RVA: 0x0068D780 File Offset: 0x0068B980
		public static void SelectionOverlaysOnGUI()
		{
			List<WorldObject> selectedObjects = Find.WorldSelector.SelectedObjects;
			for (int i = 0; i < selectedObjects.Count; i++)
			{
				WorldObject worldObject = selectedObjects[i];
				WorldSelectionDrawer.DrawSelectionBracketOnGUIFor(worldObject);
				worldObject.ExtraSelectionOverlaysOnGUI();
			}
		}

		// Token: 0x06015E67 RID: 89703 RVA: 0x0068D7BC File Offset: 0x0068B9BC
		public static void DrawSelectionOverlays()
		{
			List<WorldObject> selectedObjects = Find.WorldSelector.SelectedObjects;
			for (int i = 0; i < selectedObjects.Count; i++)
			{
				selectedObjects[i].DrawExtraSelectionOverlays();
			}
		}

		// Token: 0x06015E68 RID: 89704 RVA: 0x0068D7F4 File Offset: 0x0068B9F4
		private static void DrawSelectionBracketOnGUIFor(WorldObject obj)
		{
			Vector2 vector = obj.ScreenPos();
			Rect rect = new Rect(vector.x - 17.5f, vector.y - 17.5f, 35f, 35f);
			Vector2 textureSize = new Vector2((float)SelectionDrawerUtility.SelectedTexGUI.width * 0.4f, (float)SelectionDrawerUtility.SelectedTexGUI.height * 0.4f);
			SelectionDrawerUtility.CalculateSelectionBracketPositionsUI<WorldObject>(WorldSelectionDrawer.bracketLocs, obj, rect, WorldSelectionDrawer.selectTimes, textureSize, 25f);
			GUI.color = (obj.HiddenBehindTerrainNow() ? WorldSelectionDrawer.HiddenSelectionBracketColor : Color.white);
			int num = 90;
			for (int i = 0; i < 4; i++)
			{
				Widgets.DrawTextureRotated(WorldSelectionDrawer.bracketLocs[i], SelectionDrawerUtility.SelectedTexGUI, (float)num, 0.4f, null);
				num += 90;
			}
			GUI.color = Color.white;
		}

		// Token: 0x0400F2E6 RID: 62182
		private static readonly Dictionary<WorldObject, float> selectTimes = new Dictionary<WorldObject, float>();

		// Token: 0x0400F2E7 RID: 62183
		private const float BaseSelectedTexJump = 25f;

		// Token: 0x0400F2E8 RID: 62184
		private const float BaseSelectedTexScale = 0.4f;

		// Token: 0x0400F2E9 RID: 62185
		private const float BaseSelectionRectSize = 35f;

		// Token: 0x0400F2EA RID: 62186
		private static readonly Color HiddenSelectionBracketColor = new Color(1f, 1f, 1f, 0.35f);

		// Token: 0x0400F2EB RID: 62187
		private static readonly Vector2[] bracketLocs = new Vector2[4];
	}
}
