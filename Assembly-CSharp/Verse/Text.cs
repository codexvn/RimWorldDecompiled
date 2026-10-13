using System;
using UnityEngine;
using Verse.Steam;

namespace Verse
{
	// Token: 0x02000BD7 RID: 3031
	public static class Text
	{
		// Token: 0x17000BF4 RID: 3060
		// (get) Token: 0x06004D8D RID: 19853 RVA: 0x0018A566 File Offset: 0x00188766
		public static bool TinyFontSupported
		{
			get
			{
				return (LongEventHandler.AnyEventNowOrWaiting || LanguageDatabase.activeLanguage.info.canBeTiny) && !Prefs.DisableTinyText && !SteamDeck.IsSteamDeck;
			}
		}

		// Token: 0x17000BF5 RID: 3061
		// (get) Token: 0x06004D8E RID: 19854 RVA: 0x0018A595 File Offset: 0x00188795
		// (set) Token: 0x06004D8F RID: 19855 RVA: 0x0018A59C File Offset: 0x0018879C
		public static GameFont Font
		{
			get
			{
				return Text.fontInt;
			}
			set
			{
				if (value == GameFont.Tiny && !Text.TinyFontSupported)
				{
					Text.fontInt = GameFont.Small;
					return;
				}
				Text.fontInt = value;
			}
		}

		// Token: 0x17000BF6 RID: 3062
		// (get) Token: 0x06004D90 RID: 19856 RVA: 0x0018A5B5 File Offset: 0x001887B5
		// (set) Token: 0x06004D91 RID: 19857 RVA: 0x0018A5BC File Offset: 0x001887BC
		public static TextAnchor Anchor
		{
			get
			{
				return Text.anchorInt;
			}
			set
			{
				Text.anchorInt = value;
			}
		}

		// Token: 0x17000BF7 RID: 3063
		// (get) Token: 0x06004D92 RID: 19858 RVA: 0x0018A5C4 File Offset: 0x001887C4
		// (set) Token: 0x06004D93 RID: 19859 RVA: 0x0018A5CB File Offset: 0x001887CB
		public static bool WordWrap
		{
			get
			{
				return Text.wordWrapInt;
			}
			set
			{
				Text.wordWrapInt = value;
			}
		}

		// Token: 0x17000BF8 RID: 3064
		// (get) Token: 0x06004D94 RID: 19860 RVA: 0x0018A5D3 File Offset: 0x001887D3
		public static float LineHeight
		{
			get
			{
				return Text.lineHeights[(int)Text.Font];
			}
		}

		// Token: 0x17000BF9 RID: 3065
		// (get) Token: 0x06004D95 RID: 19861 RVA: 0x0018A5E0 File Offset: 0x001887E0
		public static float SpaceBetweenLines
		{
			get
			{
				return Text.spaceBetweenLines[(int)Text.Font];
			}
		}

		// Token: 0x17000BFA RID: 3066
		// (get) Token: 0x06004D96 RID: 19862 RVA: 0x0018A5F0 File Offset: 0x001887F0
		public static GUIStyle CurFontStyle
		{
			get
			{
				GUIStyle guistyle;
				switch (Text.fontInt)
				{
				case GameFont.Tiny:
					guistyle = Text.fontStyles[0];
					break;
				case GameFont.Small:
					guistyle = Text.fontStyles[1];
					break;
				case GameFont.Medium:
					guistyle = Text.fontStyles[2];
					break;
				default:
					throw new NotImplementedException();
				}
				guistyle.alignment = Text.anchorInt;
				guistyle.wordWrap = Text.wordWrapInt;
				return guistyle;
			}
		}

		// Token: 0x17000BFB RID: 3067
		// (get) Token: 0x06004D97 RID: 19863 RVA: 0x0018A654 File Offset: 0x00188854
		public static GUIStyle CurTextFieldStyle
		{
			get
			{
				switch (Text.fontInt)
				{
				case GameFont.Tiny:
					return Text.textFieldStyles[0];
				case GameFont.Small:
					return Text.textFieldStyles[1];
				case GameFont.Medium:
					return Text.textFieldStyles[2];
				default:
					throw new NotImplementedException();
				}
			}
		}

		// Token: 0x17000BFC RID: 3068
		// (get) Token: 0x06004D98 RID: 19864 RVA: 0x0018A698 File Offset: 0x00188898
		public static GUIStyle CurTextAreaStyle
		{
			get
			{
				switch (Text.fontInt)
				{
				case GameFont.Tiny:
					return Text.textAreaStyles[0];
				case GameFont.Small:
					return Text.textAreaStyles[1];
				case GameFont.Medium:
					return Text.textAreaStyles[2];
				default:
					throw new NotImplementedException();
				}
			}
		}

		// Token: 0x17000BFD RID: 3069
		// (get) Token: 0x06004D99 RID: 19865 RVA: 0x0018A6DC File Offset: 0x001888DC
		public static GUIStyle CurTextAreaReadOnlyStyle
		{
			get
			{
				switch (Text.fontInt)
				{
				case GameFont.Tiny:
					return Text.textAreaReadOnlyStyles[0];
				case GameFont.Small:
					return Text.textAreaReadOnlyStyles[1];
				case GameFont.Medium:
					return Text.textAreaReadOnlyStyles[2];
				default:
					throw new NotImplementedException();
				}
			}
		}

		// Token: 0x06004D9A RID: 19866 RVA: 0x0018A720 File Offset: 0x00188920
		static Text()
		{
			Text.fonts[0] = (Font)Resources.Load("Fonts/Calibri_tiny");
			Text.fonts[1] = (Font)Resources.Load("Fonts/Arial_small");
			Text.fonts[2] = (Font)Resources.Load("Fonts/Arial_medium");
			Text.fontStyles[0] = new GUIStyle(GUI.skin.label);
			Text.fontStyles[0].font = Text.fonts[0];
			Text.fontStyles[1] = new GUIStyle(GUI.skin.label);
			Text.fontStyles[1].font = Text.fonts[1];
			Text.fontStyles[1].contentOffset = new Vector2(0f, -1f);
			Text.fontStyles[2] = new GUIStyle(GUI.skin.label);
			Text.fontStyles[2].font = Text.fonts[2];
			for (int i = 0; i < Text.textFieldStyles.Length; i++)
			{
				Text.textFieldStyles[i] = new GUIStyle(GUI.skin.textField);
				Text.textFieldStyles[i].alignment = TextAnchor.MiddleLeft;
			}
			Text.textFieldStyles[0].font = Text.fonts[0];
			Text.textFieldStyles[1].font = Text.fonts[1];
			Text.textFieldStyles[2].font = Text.fonts[2];
			for (int j = 0; j < Text.textAreaStyles.Length; j++)
			{
				Text.textAreaStyles[j] = new GUIStyle(Text.textFieldStyles[j]);
				Text.textAreaStyles[j].alignment = TextAnchor.UpperLeft;
				Text.textAreaStyles[j].wordWrap = true;
			}
			for (int k = 0; k < Text.textAreaReadOnlyStyles.Length; k++)
			{
				Text.textAreaReadOnlyStyles[k] = new GUIStyle(Text.textAreaStyles[k]);
				GUIStyle guistyle = Text.textAreaReadOnlyStyles[k];
				guistyle.normal.background = null;
				guistyle.active.background = null;
				guistyle.onHover.background = null;
				guistyle.hover.background = null;
				guistyle.onFocused.background = null;
				guistyle.focused.background = null;
			}
			GUI.skin.settings.doubleClickSelectsWord = true;
			int num = 0;
			foreach (object obj in Enum.GetValues(typeof(GameFont)))
			{
				Text.Font = (GameFont)obj;
				Text.lineHeights[num] = Text.CalcHeight("W", 999f);
				Text.spaceBetweenLines[num] = Text.CalcHeight("W\nW", 999f) - Text.CalcHeight("W", 999f) * 2f;
				num++;
			}
			Text.Font = GameFont.Small;
		}

		// Token: 0x06004D9B RID: 19867 RVA: 0x0018AA48 File Offset: 0x00188C48
		public static float LineHeightOf(GameFont font)
		{
			return Text.lineHeights[(int)font];
		}

		// Token: 0x06004D9C RID: 19868 RVA: 0x0018AA51 File Offset: 0x00188C51
		public static float CalcHeight(string text, float width)
		{
			Text.tmpTextGUIContent.text = text.StripTags();
			return Text.CurFontStyle.CalcHeight(Text.tmpTextGUIContent, width);
		}

		// Token: 0x06004D9D RID: 19869 RVA: 0x0018AA73 File Offset: 0x00188C73
		public static Vector2 CalcSize(string text)
		{
			Text.tmpTextGUIContent.text = text.StripTags();
			return Text.CurFontStyle.CalcSize(Text.tmpTextGUIContent);
		}

		// Token: 0x06004D9E RID: 19870 RVA: 0x0018AA94 File Offset: 0x00188C94
		public static string ClampTextWithEllipsis(Rect rect, string text)
		{
			if (text.Length <= 4)
			{
				return text;
			}
			Vector2 vector = Text.CalcSize(text);
			if (vector.x <= rect.width - 13f)
			{
				return text;
			}
			while (vector.x > rect.width - 13f)
			{
				if (text.Length == 0)
				{
					return "";
				}
				text = text.Substring(0, text.Length - 1);
				vector = Text.CalcSize(text + "...");
			}
			return text + "...";
		}

		// Token: 0x06004D9F RID: 19871 RVA: 0x0018AB1C File Offset: 0x00188D1C
		public static void StartOfOnGUI()
		{
			if (!Text.WordWrap)
			{
				Log.ErrorOnce("Word wrap was false at end of frame.", 764362);
				Text.WordWrap = true;
			}
			if (Text.Anchor != TextAnchor.UpperLeft)
			{
				Log.ErrorOnce("Alignment was " + Text.Anchor.ToString() + " at end of frame.", 15558);
				Text.Anchor = TextAnchor.UpperLeft;
			}
			Text.Font = GameFont.Small;
		}

		// Token: 0x04003714 RID: 14100
		private static GameFont fontInt = GameFont.Small;

		// Token: 0x04003715 RID: 14101
		private static TextAnchor anchorInt = TextAnchor.UpperLeft;

		// Token: 0x04003716 RID: 14102
		private static bool wordWrapInt = true;

		// Token: 0x04003717 RID: 14103
		private static Font[] fonts = new Font[3];

		// Token: 0x04003718 RID: 14104
		public static readonly GUIStyle[] fontStyles = new GUIStyle[3];

		// Token: 0x04003719 RID: 14105
		public static readonly GUIStyle[] textFieldStyles = new GUIStyle[3];

		// Token: 0x0400371A RID: 14106
		public static readonly GUIStyle[] textAreaStyles = new GUIStyle[3];

		// Token: 0x0400371B RID: 14107
		public static readonly GUIStyle[] textAreaReadOnlyStyles = new GUIStyle[3];

		// Token: 0x0400371C RID: 14108
		private static readonly float[] lineHeights = new float[3];

		// Token: 0x0400371D RID: 14109
		private static readonly float[] spaceBetweenLines = new float[3];

		// Token: 0x0400371E RID: 14110
		private static GUIContent tmpTextGUIContent = new GUIContent();

		// Token: 0x0400371F RID: 14111
		private const int NumFonts = 3;

		// Token: 0x04003720 RID: 14112
		public const float SmallFontHeight = 22f;
	}
}
