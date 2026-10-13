using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000C68 RID: 3176
	[StaticConstructorOnStartup]
	public static class ContentSourceUtility
	{
		// Token: 0x06005132 RID: 20786 RVA: 0x001A5FC8 File Offset: 0x001A41C8
		public static Texture2D GetIcon(this ContentSource s)
		{
			switch (s)
			{
			case ContentSource.Undefined:
				return BaseContent.BadTex;
			case ContentSource.OfficialModsFolder:
				return ContentSourceUtility.ContentSourceIcon_OfficialModsFolder;
			case ContentSource.ModsFolder:
				return ContentSourceUtility.ContentSourceIcon_ModsFolder;
			case ContentSource.SteamWorkshop:
				return ContentSourceUtility.ContentSourceIcon_SteamWorkshop;
			default:
				throw new NotImplementedException();
			}
		}

		// Token: 0x06005133 RID: 20787 RVA: 0x001A6000 File Offset: 0x001A4200
		public static void DrawContentSource(Rect r, ContentSource source, Action clickAction = null)
		{
			Rect rect = new Rect(r.x, r.y + r.height / 2f - 12f, 24f, 24f);
			GUI.DrawTexture(rect, source.GetIcon());
			if (Mouse.IsOver(rect))
			{
				TooltipHandler.TipRegion(rect, () => "Source".Translate() + ": " + source.HumanLabel(), (int)(r.x + r.y * 56161f));
				Widgets.DrawHighlight(rect);
			}
			if (clickAction != null && Widgets.ButtonInvisible(rect, true))
			{
				clickAction();
			}
		}

		// Token: 0x06005134 RID: 20788 RVA: 0x001A60A6 File Offset: 0x001A42A6
		public static string HumanLabel(this ContentSource s)
		{
			return ("ContentSource_" + s.ToString()).Translate();
		}

		// Token: 0x04003A96 RID: 14998
		public const float IconSize = 24f;

		// Token: 0x04003A97 RID: 14999
		private static readonly Texture2D ContentSourceIcon_OfficialModsFolder = ContentFinder<Texture2D>.Get("UI/Icons/ContentSources/OfficialModsFolder", true);

		// Token: 0x04003A98 RID: 15000
		private static readonly Texture2D ContentSourceIcon_ModsFolder = ContentFinder<Texture2D>.Get("UI/Icons/ContentSources/ModsFolder", true);

		// Token: 0x04003A99 RID: 15001
		private static readonly Texture2D ContentSourceIcon_SteamWorkshop = ContentFinder<Texture2D>.Get("UI/Icons/ContentSources/SteamWorkshop", true);
	}
}
