using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse;
using Verse.Sound;
using Verse.Steam;

namespace LudeonTK
{
	// Token: 0x0200006A RID: 106
	[StaticConstructorOnStartup]
	public static class DevGUI
	{
		// Token: 0x06000247 RID: 583 RVA: 0x0000C994 File Offset: 0x0000AB94
		static DevGUI()
		{
			Color color = new Color(1f, 1f, 1f, 0f);
			DevGUI.LineTexAA = new Texture2D(1, 3, TextureFormat.ARGB32, false);
			DevGUI.LineTexAA.name = "LineTexAA";
			DevGUI.LineTexAA.SetPixel(0, 0, color);
			DevGUI.LineTexAA.SetPixel(0, 1, Color.white);
			DevGUI.LineTexAA.SetPixel(0, 2, color);
			DevGUI.LineTexAA.Apply();
			DevGUI.ButtonBackground = new Texture2D(1, 1, TextureFormat.ARGB32, false);
			DevGUI.ButtonBackground.name = "ButtonBGAtlas";
			DevGUI.ButtonBackground.SetPixel(0, 0, new Color32(65, 65, 65, byte.MaxValue));
			DevGUI.ButtonBackground.Apply();
			DevGUI.ButtonBackgroundMouseover = new Texture2D(1, 1, TextureFormat.ARGB32, false);
			DevGUI.ButtonBackgroundMouseover.name = "ButtonBGAtlasMouseover";
			DevGUI.ButtonBackgroundMouseover.SetPixel(0, 0, new Color32(85, 85, 85, byte.MaxValue));
			DevGUI.ButtonBackgroundMouseover.Apply();
			DevGUI.ButtonBackgroundClick = new Texture2D(1, 1, TextureFormat.ARGB32, false);
			DevGUI.ButtonBackgroundClick.name = "ButtonBGAtlasClick";
			DevGUI.ButtonBackgroundClick.SetPixel(0, 0, new Color32(45, 45, 45, byte.MaxValue));
			DevGUI.ButtonBackgroundClick.Apply();
		}

		// Token: 0x06000248 RID: 584 RVA: 0x0000CBAD File Offset: 0x0000ADAD
		public static void BeginGroup(Rect rect)
		{
			GUI.BeginGroup(rect);
			UnityGUIBugsFixer.Notify_BeginGroup();
		}

		// Token: 0x06000249 RID: 585 RVA: 0x0000CBBA File Offset: 0x0000ADBA
		public static void EndGroup()
		{
			GUI.EndGroup();
			UnityGUIBugsFixer.Notify_EndGroup();
		}

		// Token: 0x0600024A RID: 586 RVA: 0x0000CBC8 File Offset: 0x0000ADC8
		public static void DrawLine(Vector2 start, Vector2 end, Color color, float width)
		{
			float num = end.x - start.x;
			float num2 = end.y - start.y;
			float num3 = Mathf.Sqrt(num * num + num2 * num2);
			if (num3 < 0.01f)
			{
				return;
			}
			width *= 3f;
			float num4 = width * num2 / num3;
			float num5 = width * num / num3;
			float z = -Mathf.Atan2(-num2, num) * 57.29578f;
			Vector2 vector = start + new Vector2(0.5f * num4, -0.5f * num5);
			Matrix4x4 m = Matrix4x4.TRS(vector, Quaternion.Euler(0f, 0f, z), Vector3.one) * Matrix4x4.TRS(-vector, Quaternion.identity, Vector3.one);
			Rect position = new Rect(start.x, start.y - 0.5f * num5, num3, width);
			GL.PushMatrix();
			GL.MultMatrix(m);
			GUI.DrawTexture(position, DevGUI.LineTexAA, ScaleMode.StretchToFill, true, 0f, color, 0f, 0f);
			GL.PopMatrix();
		}

		// Token: 0x0600024B RID: 587 RVA: 0x0000CCD8 File Offset: 0x0000AED8
		public static void DrawBox(Rect rect, int thickness = 1, Texture2D lineTexture = null)
		{
			Vector2 vector = new Vector2(rect.x, rect.y);
			Vector2 vector2 = new Vector2(rect.x + rect.width, rect.y + rect.height);
			if (vector.x > vector2.x)
			{
				float x = vector.x;
				vector.x = vector2.x;
				vector2.x = x;
			}
			if (vector.y > vector2.y)
			{
				float y = vector.y;
				vector.y = vector2.y;
				vector2.y = y;
			}
			Vector3 vector3 = vector2 - vector;
			GUI.DrawTexture(UIScaling.AdjustRectToUIScaling(new Rect(vector.x, vector.y, (float)thickness, vector3.y)), lineTexture ?? BaseContent.WhiteTex);
			GUI.DrawTexture(UIScaling.AdjustRectToUIScaling(new Rect(vector2.x - (float)thickness, vector.y, (float)thickness, vector3.y)), lineTexture ?? BaseContent.WhiteTex);
			GUI.DrawTexture(UIScaling.AdjustRectToUIScaling(new Rect(vector.x + (float)thickness, vector.y, vector3.x - (float)(thickness * 2), (float)thickness)), lineTexture ?? BaseContent.WhiteTex);
			GUI.DrawTexture(UIScaling.AdjustRectToUIScaling(new Rect(vector.x + (float)thickness, vector2.y - (float)thickness, vector3.x - (float)(thickness * 2), (float)thickness)), lineTexture ?? BaseContent.WhiteTex);
		}

		// Token: 0x0600024C RID: 588 RVA: 0x0000CE4C File Offset: 0x0000B04C
		public static void Label(Rect rect, string label)
		{
			Rect position = rect;
			float num = Prefs.UIScale / 2f;
			if (Prefs.UIScale > 1f && Math.Abs(num - Mathf.Floor(num)) > 1E-45f)
			{
				position = UIScaling.AdjustRectToUIScaling(rect);
			}
			GUI.Label(position, label, Text.CurFontStyle);
		}

		// Token: 0x0600024D RID: 589 RVA: 0x0000CE9C File Offset: 0x0000B09C
		public static void CheckboxLabeled(Rect rect, string label, ref bool checkOn)
		{
			TextAnchor anchor = Text.Anchor;
			Text.Anchor = TextAnchor.MiddleLeft;
			DevGUI.Label(rect, label);
			if (DevGUI.ButtonInvisible(rect))
			{
				checkOn = !checkOn;
				if (checkOn)
				{
					SoundDefOf.Checkbox_TurnedOn.PlayOneShotOnCamera(null);
				}
				else
				{
					SoundDefOf.Checkbox_TurnedOff.PlayOneShotOnCamera(null);
				}
			}
			DevGUI.Checkbox(new Rect(rect.x + rect.width - 24f, rect.y + (rect.height - 24f) / 2f, 24f, 24f), ref checkOn);
			Text.Anchor = anchor;
		}

		// Token: 0x0600024E RID: 590 RVA: 0x0000CF30 File Offset: 0x0000B130
		public static void CheckboxImage(Rect rect, Texture2D icon, ref bool checkOn)
		{
			if (DevGUI.ButtonImage(rect, icon))
			{
				checkOn = !checkOn;
				if (checkOn)
				{
					SoundDefOf.Checkbox_TurnedOn.PlayOneShotOnCamera(null);
				}
				else
				{
					SoundDefOf.Checkbox_TurnedOff.PlayOneShotOnCamera(null);
				}
			}
			float num = rect.height / 2f;
			DevGUI.Checkbox(new Rect(rect.center.x, rect.yMin, num, num), ref checkOn);
		}

		// Token: 0x0600024F RID: 591 RVA: 0x0000CF97 File Offset: 0x0000B197
		public static void Checkbox(Rect rect, ref bool checkOn)
		{
			if (DevGUI.ButtonImage(rect, checkOn ? DevGUI.CheckOn : DevGUI.CheckOff))
			{
				checkOn = !checkOn;
				if (checkOn)
				{
					SoundDefOf.Checkbox_TurnedOn.PlayOneShotOnCamera(null);
					return;
				}
				SoundDefOf.Checkbox_TurnedOff.PlayOneShotOnCamera(null);
			}
		}

		// Token: 0x06000250 RID: 592 RVA: 0x0000CFD4 File Offset: 0x0000B1D4
		public static bool ButtonText(Rect rect, string label, TextAnchor? overrideTextAnchor = null)
		{
			TextAnchor anchor = Text.Anchor;
			Color color = GUI.color;
			Texture2D image = DevGUI.ButtonBackground;
			if (Mouse.IsOver(rect))
			{
				image = DevGUI.ButtonBackgroundMouseover;
				if (Input.GetMouseButton(0))
				{
					image = DevGUI.ButtonBackgroundClick;
				}
			}
			GUI.DrawTexture(rect, image);
			if (overrideTextAnchor != null)
			{
				Text.Anchor = overrideTextAnchor.Value;
			}
			else
			{
				Text.Anchor = TextAnchor.MiddleCenter;
			}
			bool wordWrap = Text.WordWrap;
			if (rect.height < Text.LineHeight * 2f)
			{
				Text.WordWrap = false;
			}
			GUI.color = Color.white;
			DevGUI.Label(rect, label);
			Text.Anchor = anchor;
			GUI.color = color;
			Text.WordWrap = wordWrap;
			return DevGUI.ButtonInvisible(rect);
		}

		// Token: 0x06000251 RID: 593 RVA: 0x0000D07A File Offset: 0x0000B27A
		public static void DrawRect(Rect position, Color color)
		{
			Color backgroundColor = GUI.backgroundColor;
			GUI.backgroundColor = color;
			GUI.Box(position, GUIContent.none, TexUI.FastFillStyle);
			GUI.backgroundColor = backgroundColor;
		}

		// Token: 0x06000252 RID: 594 RVA: 0x0000D09C File Offset: 0x0000B29C
		public static bool ButtonImage(Rect butRect, Texture2D tex)
		{
			if (Mouse.IsOver(butRect) && !ReorderableWidget.Dragging)
			{
				GUI.color = GenUI.MouseoverColor;
			}
			else
			{
				GUI.color = Color.white;
			}
			GUI.DrawTexture(butRect, tex);
			GUI.color = Color.white;
			return DevGUI.ButtonInvisible(butRect);
		}

		// Token: 0x06000253 RID: 595 RVA: 0x0000D0DA File Offset: 0x0000B2DA
		public static bool ButtonInvisible(Rect butRect)
		{
			MouseoverSounds.DoRegion(butRect);
			return GUI.Button(butRect, "", DevGUI.EmptyStyle);
		}

		// Token: 0x06000254 RID: 596 RVA: 0x0000D0F2 File Offset: 0x0000B2F2
		public static string TextField(Rect rect, string text)
		{
			if (text == null)
			{
				text = "";
			}
			return GUI.TextField(rect, text, Text.CurTextFieldStyle);
		}

		// Token: 0x06000255 RID: 597 RVA: 0x0000D10C File Offset: 0x0000B30C
		public static void BeginScrollView(Rect outRect, ref Vector2 scrollPosition, Rect viewRect, bool showScrollbars = true)
		{
			if (DevGUI.mouseOverScrollViewStack.Count > 0)
			{
				DevGUI.mouseOverScrollViewStack.Push(DevGUI.mouseOverScrollViewStack.Peek() && outRect.Contains(Event.current.mousePosition));
			}
			else
			{
				DevGUI.mouseOverScrollViewStack.Push(outRect.Contains(Event.current.mousePosition));
			}
			SteamDeck.HandleTouchScreenScrollViewScroll(outRect, ref scrollPosition);
			if (showScrollbars)
			{
				scrollPosition = GUI.BeginScrollView(outRect, scrollPosition, viewRect);
			}
			else
			{
				scrollPosition = GUI.BeginScrollView(outRect, scrollPosition, viewRect, GUIStyle.none, GUIStyle.none);
			}
			UnityGUIBugsFixer.Notify_BeginScrollView();
		}

		// Token: 0x06000256 RID: 598 RVA: 0x0000D1AE File Offset: 0x0000B3AE
		public static void EndScrollView()
		{
			DevGUI.mouseOverScrollViewStack.Pop();
			GUI.EndScrollView();
		}

		// Token: 0x06000257 RID: 599 RVA: 0x0000D1C0 File Offset: 0x0000B3C0
		public static void DrawHighlightSelected(Rect rect)
		{
			GUI.DrawTexture(rect, TexUI.HighlightSelectedTex);
		}

		// Token: 0x06000258 RID: 600 RVA: 0x0000D1CD File Offset: 0x0000B3CD
		public static void DrawHighlightIfMouseover(Rect rect)
		{
			if (Mouse.IsOver(rect))
			{
				DevGUI.DrawHighlight(rect);
			}
		}

		// Token: 0x06000259 RID: 601 RVA: 0x0000D1DD File Offset: 0x0000B3DD
		public static void DrawHighlight(Rect rect)
		{
			GUI.DrawTexture(rect, TexUI.HighlightTex);
		}

		// Token: 0x0600025A RID: 602 RVA: 0x0000D1EA File Offset: 0x0000B3EA
		public static void DrawLightHighlight(Rect rect)
		{
			GUI.DrawTexture(rect, DevGUI.LightHighlight);
		}

		// Token: 0x0600025B RID: 603 RVA: 0x0000D1F8 File Offset: 0x0000B3F8
		public static DebugActionButtonResult CheckboxPinnable(Rect rect, string label, ref bool checkOn, bool highlight, bool pinned)
		{
			new Rect(rect.x, rect.y, rect.width - rect.height, rect.height);
			DebugActionButtonResult result = DebugActionButtonResult.None;
			DevGUI.CheckboxLabeled(rect, label.Truncate(rect.width - 15f, null), ref checkOn);
			if (highlight)
			{
				GUI.color = Color.yellow;
				DevGUI.DrawBox(rect, 2, null);
				GUI.color = Color.white;
			}
			Rect rect2 = new Rect(rect.xMax + 2f, rect.y, rect.height, rect.height).ContractedBy(4f);
			GUI.color = (pinned ? Color.white : new Color(1f, 1f, 1f, 0.2f));
			GUI.DrawTexture(rect2, pinned ? DevGUI.PinTex : DevGUI.PinOutlineTex);
			GUI.color = Color.white;
			if (DevGUI.ButtonInvisible(rect2))
			{
				result = DebugActionButtonResult.PinPressed;
			}
			DevGUI.DrawHighlightIfMouseover(rect2);
			return result;
		}

		// Token: 0x0600025C RID: 604 RVA: 0x0000D2F8 File Offset: 0x0000B4F8
		public static DebugActionButtonResult ButtonDebugPinnable(Rect rect, string label, bool highlight, bool pinned)
		{
			DebugActionButtonResult result = DebugActionButtonResult.None;
			bool wordWrap = Text.WordWrap;
			Text.WordWrap = false;
			if (DevGUI.ButtonText(rect, "  " + label, new TextAnchor?(TextAnchor.MiddleLeft)))
			{
				result = DebugActionButtonResult.ButtonPressed;
			}
			Text.WordWrap = wordWrap;
			if (highlight)
			{
				GUI.color = Color.yellow;
				DevGUI.DrawBox(rect, 2, null);
				GUI.color = Color.white;
			}
			Rect rect2 = new Rect(rect.xMax + 2f, rect.y, rect.height, rect.height).ContractedBy(4f);
			GUI.color = (pinned ? Color.white : new Color(1f, 1f, 1f, 0.2f));
			GUI.DrawTexture(rect2, pinned ? DevGUI.PinTex : DevGUI.PinOutlineTex);
			GUI.color = Color.white;
			if (DevGUI.ButtonInvisible(rect2))
			{
				result = DebugActionButtonResult.PinPressed;
			}
			DevGUI.DrawHighlightIfMouseover(rect2);
			return result;
		}

		// Token: 0x0600025D RID: 605 RVA: 0x0000D3DC File Offset: 0x0000B5DC
		public static string TextAreaScrollable(Rect rect, string text, ref Vector2 scrollbarPosition, bool readOnly = false)
		{
			Rect rect2 = new Rect(0f, 0f, rect.width - 16f, Mathf.Max(Text.CalcHeight(text, rect.width) + 10f, rect.height));
			DevGUI.BeginScrollView(rect, ref scrollbarPosition, rect2, true);
			string result = GUI.TextArea(rect2, text, readOnly ? Text.CurTextAreaReadOnlyStyle : Text.CurTextAreaStyle);
			DevGUI.EndScrollView();
			return result;
		}

		// Token: 0x0600025E RID: 606 RVA: 0x0000D44C File Offset: 0x0000B64C
		public static float HorizontalSlider(Rect rect, float value, float leftValue, float rightValue)
		{
			float num = GUI.HorizontalSlider(rect, value, leftValue, rightValue);
			if (value != num)
			{
				SoundDefOf.DragSlider.PlayOneShotOnCamera(null);
			}
			return num;
		}

		// Token: 0x0400011D RID: 285
		private static Stack<bool> mouseOverScrollViewStack = new Stack<bool>();

		// Token: 0x0400011E RID: 286
		public static readonly GUIStyle EmptyStyle = new GUIStyle();

		// Token: 0x0400011F RID: 287
		public static readonly Color WindowBGFillColor = new ColorInt(21, 25, 29).ToColor;

		// Token: 0x04000120 RID: 288
		public const float CheckboxSize = 24f;

		// Token: 0x04000121 RID: 289
		private static Texture2D ButtonBackground = null;

		// Token: 0x04000122 RID: 290
		private static Texture2D ButtonBackgroundMouseover = null;

		// Token: 0x04000123 RID: 291
		public static Texture2D ButtonBackgroundClick = null;

		// Token: 0x04000124 RID: 292
		public static readonly Texture2D LightHighlight = SolidColorMaterials.NewSolidColorTexture(new Color(1f, 1f, 1f, 0.04f));

		// Token: 0x04000125 RID: 293
		private static readonly Texture2D PinTex = ContentFinder<Texture2D>.Get("UI/Developer/Pin", true);

		// Token: 0x04000126 RID: 294
		private static readonly Texture2D PinOutlineTex = ContentFinder<Texture2D>.Get("UI/Developer/Pin-Outline", true);

		// Token: 0x04000127 RID: 295
		public static readonly Texture2D CheckOn = ContentFinder<Texture2D>.Get("UI/Developer/CheckOn", true);

		// Token: 0x04000128 RID: 296
		public static readonly Texture2D CheckOff = ContentFinder<Texture2D>.Get("UI/Developer/CheckOff", true);

		// Token: 0x04000129 RID: 297
		public static readonly Texture2D InspectMode = ContentFinder<Texture2D>.Get("UI/Developer/InspectModeToggle", true);

		// Token: 0x0400012A RID: 298
		public static readonly Texture2D Close = ContentFinder<Texture2D>.Get("UI/Developer/Close", true);

		// Token: 0x0400012B RID: 299
		private static Texture2D LineTexAA = null;

		// Token: 0x0400012C RID: 300
		public const float CloseButtonSize = 22f;

		// Token: 0x0400012D RID: 301
		public const float CloseButtonMargin = 4f;
	}
}
