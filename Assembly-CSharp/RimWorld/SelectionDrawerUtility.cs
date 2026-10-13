using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020033DD RID: 13277
	[StaticConstructorOnStartup]
	public static class SelectionDrawerUtility
	{
		// Token: 0x06012A82 RID: 76418 RVA: 0x005856C4 File Offset: 0x005838C4
		public static void CalculateSelectionBracketPositionsUI<T>(Vector2[] bracketLocs, T obj, Rect rect, Dictionary<T, float> selectTimes, Vector2 textureSize, float jumpDistanceFactor = 1f)
		{
			float num;
			float num2;
			if (!selectTimes.TryGetValue(obj, out num))
			{
				num2 = 1f;
			}
			else
			{
				num2 = Mathf.Max(0f, 1f - (Time.realtimeSinceStartup - num) / 0.07f);
			}
			float num3 = num2 * 0.2f * jumpDistanceFactor;
			float num4 = 0.5f * (rect.width - textureSize.x) + num3;
			float num5 = 0.5f * (rect.height - textureSize.y) + num3;
			bracketLocs[0] = new Vector2(rect.center.x - num4, rect.center.y - num5);
			bracketLocs[1] = new Vector2(rect.center.x + num4, rect.center.y - num5);
			bracketLocs[2] = new Vector2(rect.center.x + num4, rect.center.y + num5);
			bracketLocs[3] = new Vector2(rect.center.x - num4, rect.center.y + num5);
		}

		// Token: 0x06012A83 RID: 76419 RVA: 0x005857E4 File Offset: 0x005839E4
		public static void CalculateSelectionBracketPositionsWorld<T>(Vector3[] bracketLocs, T obj, Vector3 worldPos, Vector2 worldSize, Dictionary<T, float> selectTimes, Vector2 textureSize, float jumpDistanceFactor = 1f, float deselectedJumpFactor = 1f)
		{
			float num;
			float num2;
			if (!selectTimes.TryGetValue(obj, out num))
			{
				num2 = deselectedJumpFactor;
			}
			else
			{
				num2 = Mathf.Max(0f, 1f - (Time.realtimeSinceStartup - num) / 0.07f);
			}
			float num3 = num2 * 0.2f * jumpDistanceFactor;
			Vector3 vector = worldPos;
			float num4 = 0.5f * (worldSize.x - textureSize.x) + num3;
			float num5 = 0.5f * (worldSize.y - textureSize.y) + num3;
			float y = AltitudeLayer.MetaOverlays.AltitudeFor();
			Thing thing = obj as Thing;
			if (thing != null)
			{
				ThingDef thingDef = GenConstruct.BuiltDefOf(thing.def) as ThingDef;
				if (((thingDef != null) ? thingDef.building : null) != null && thingDef.building.isAttachment)
				{
					vector += (thing.Rotation.AsVector2 * 0.5f).ToVector3();
				}
			}
			bracketLocs[0] = new Vector3(vector.x - num4, y, vector.z - num5);
			bracketLocs[1] = new Vector3(vector.x + num4, y, vector.z - num5);
			bracketLocs[2] = new Vector3(vector.x + num4, y, vector.z + num5);
			bracketLocs[3] = new Vector3(vector.x - num4, y, vector.z + num5);
		}

		// Token: 0x06012A84 RID: 76420 RVA: 0x00585950 File Offset: 0x00583B50
		public static void DrawSelectionOverlayOnGUI(object target, Rect rect, float scale, float selectedTextJump)
		{
			Vector2 textureSize = new Vector2((float)SelectionDrawerUtility.SelectedTexGUI.width * scale, (float)SelectionDrawerUtility.SelectedTexGUI.height * scale);
			SelectionDrawerUtility.CalculateSelectionBracketPositionsUI<object>(SelectionDrawerUtility.bracketLocs, target, rect, SelectionDrawer.SelectTimes, textureSize, selectedTextJump);
			SelectionDrawerUtility.DrawSelectionOverlayOnGUI(SelectionDrawerUtility.bracketLocs, scale);
		}

		// Token: 0x06012A85 RID: 76421 RVA: 0x0058599C File Offset: 0x00583B9C
		public static void DrawSelectionOverlayOnGUI(Vector2[] bracketLocs, float selectedTexScale)
		{
			int num = 90;
			for (int i = 0; i < 4; i++)
			{
				Widgets.DrawTextureRotated(bracketLocs[i], SelectionDrawerUtility.SelectedTexGUI, (float)num, selectedTexScale, null);
				num += 90;
			}
		}

		// Token: 0x06012A86 RID: 76422 RVA: 0x005859D2 File Offset: 0x00583BD2
		public static void DrawSelectionOverlayWholeGUI(Rect rect)
		{
			GUI.DrawTexture(rect, TexUI.SelectionBracketWhole);
		}

		// Token: 0x0400C1C2 RID: 49602
		private const float SelJumpDuration = 0.07f;

		// Token: 0x0400C1C3 RID: 49603
		private const float SelJumpDistance = 0.2f;

		// Token: 0x0400C1C4 RID: 49604
		public static readonly Texture2D SelectedTexGUI = ContentFinder<Texture2D>.Get("UI/Overlays/SelectionBracketGUI", true);

		// Token: 0x0400C1C5 RID: 49605
		private static Vector2[] bracketLocs = new Vector2[4];
	}
}
