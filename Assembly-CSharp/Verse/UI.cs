using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000131 RID: 305
	public static class UI
	{
		// Token: 0x1700016A RID: 362
		// (get) Token: 0x06000965 RID: 2405 RVA: 0x0002CFBC File Offset: 0x0002B1BC
		public static Vector2 MousePositionOnUI
		{
			get
			{
				return Input.mousePosition / Prefs.UIScale;
			}
		}

		// Token: 0x1700016B RID: 363
		// (get) Token: 0x06000966 RID: 2406 RVA: 0x0002CFD4 File Offset: 0x0002B1D4
		public static Vector2 MousePositionOnUIInverted
		{
			get
			{
				Vector2 mousePositionOnUI = UI.MousePositionOnUI;
				mousePositionOnUI.y = (float)UI.screenHeight - mousePositionOnUI.y;
				return mousePositionOnUI;
			}
		}

		// Token: 0x1700016C RID: 364
		// (get) Token: 0x06000967 RID: 2407 RVA: 0x0002CFFC File Offset: 0x0002B1FC
		public static Vector2 MousePosUIInvertedUseEventIfCan
		{
			get
			{
				if (Event.current != null)
				{
					return UI.GUIToScreenPoint(Event.current.mousePosition);
				}
				return UI.MousePositionOnUIInverted;
			}
		}

		// Token: 0x06000968 RID: 2408 RVA: 0x0002D01C File Offset: 0x0002B21C
		public static void ApplyUIScale()
		{
			if (Prefs.UIScale == 1f)
			{
				UI.screenWidth = Screen.width;
				UI.screenHeight = Screen.height;
				return;
			}
			UI.screenWidth = Mathf.RoundToInt((float)Screen.width / Prefs.UIScale);
			UI.screenHeight = Mathf.RoundToInt((float)Screen.height / Prefs.UIScale);
			float uiscale = Prefs.UIScale;
			float uiscale2 = Prefs.UIScale;
			GUI.matrix = Matrix4x4.TRS(new Vector3(0f, 0f, 0f), Quaternion.identity, new Vector3(uiscale, uiscale2, 1f));
		}

		// Token: 0x06000969 RID: 2409 RVA: 0x0002D0B1 File Offset: 0x0002B2B1
		public static void FocusControl(string controlName, Window window)
		{
			GUI.FocusControl(controlName);
			Find.WindowStack.Notify_ManuallySetFocus(window);
		}

		// Token: 0x0600096A RID: 2410 RVA: 0x0002D0C4 File Offset: 0x0002B2C4
		public static void UnfocusCurrentControl()
		{
			GUI.FocusControl(null);
		}

		// Token: 0x0600096B RID: 2411 RVA: 0x0002D0CC File Offset: 0x0002B2CC
		public static void UnfocusCurrentTextField()
		{
			GUI.SetNextControlName("FOR_UNFOCUS");
			GUI.TextField(default(Rect), "");
			GUI.FocusControl("FOR_UNFOCUS");
		}

		// Token: 0x0600096C RID: 2412 RVA: 0x0002D101 File Offset: 0x0002B301
		public static Vector2 GUIToScreenPoint(Vector2 guiPoint)
		{
			return GUIUtility.GUIToScreenPoint(guiPoint / Prefs.UIScale);
		}

		// Token: 0x0600096D RID: 2413 RVA: 0x0002D114 File Offset: 0x0002B314
		public static Rect GUIToScreenRect(Rect guiRect)
		{
			return new Rect
			{
				min = UI.GUIToScreenPoint(guiRect.min),
				max = UI.GUIToScreenPoint(guiRect.max)
			};
		}

		// Token: 0x0600096E RID: 2414 RVA: 0x0002D150 File Offset: 0x0002B350
		public static void RotateAroundPivot(float angle, Vector2 center)
		{
			GUIUtility.RotateAroundPivot(angle, center * Prefs.UIScale);
		}

		// Token: 0x0600096F RID: 2415 RVA: 0x0002D164 File Offset: 0x0002B364
		public static Vector2 MapToUIPosition(this Vector3 v)
		{
			Vector3 vector = Find.Camera.WorldToScreenPoint(v) / Prefs.UIScale;
			return new Vector2(vector.x, (float)UI.screenHeight - vector.y);
		}

		// Token: 0x06000970 RID: 2416 RVA: 0x0002D19F File Offset: 0x0002B39F
		public static Vector3 UIToMapPosition(float x, float y)
		{
			return UI.UIToMapPosition(new Vector2(x, y));
		}

		// Token: 0x06000971 RID: 2417 RVA: 0x0002D1B0 File Offset: 0x0002B3B0
		public static Vector3 UIToMapPosition(Vector2 screenLoc)
		{
			Ray ray = Find.Camera.ScreenPointToRay(screenLoc * Prefs.UIScale);
			return new Vector3(ray.origin.x, 0f, ray.origin.z);
		}

		// Token: 0x06000972 RID: 2418 RVA: 0x0002D1FA File Offset: 0x0002B3FA
		public static float CurUICellSize()
		{
			return (new Vector3(1f, 0f, 0f).MapToUIPosition() - new Vector3(0f, 0f, 0f).MapToUIPosition()).x;
		}

		// Token: 0x06000973 RID: 2419 RVA: 0x0002D238 File Offset: 0x0002B438
		public static Vector3 MouseMapPosition()
		{
			return UI.UIToMapPosition(UI.MousePositionOnUI);
		}

		// Token: 0x06000974 RID: 2420 RVA: 0x0002D244 File Offset: 0x0002B444
		public static IntVec3 MouseCell()
		{
			return UI.UIToMapPosition(UI.MousePositionOnUI).ToIntVec3();
		}

		// Token: 0x040004B1 RID: 1201
		public static int screenWidth;

		// Token: 0x040004B2 RID: 1202
		public static int screenHeight;
	}
}
