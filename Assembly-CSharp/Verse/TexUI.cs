using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000C0C RID: 3084
	[StaticConstructorOnStartup]
	public static class TexUI
	{
		// Token: 0x06004EA2 RID: 20130 RVA: 0x00190C48 File Offset: 0x0018EE48
		static TexUI()
		{
			TexUI.GrayscaleGUI.SetTexture(ShaderPropertyIDs.MaskTex, Texture2D.redTexture);
		}

		// Token: 0x04003843 RID: 14403
		public static readonly Texture2D TitleBGTex = SolidColorMaterials.NewSolidColorTexture(new Color(1f, 1f, 1f, 0.05f));

		// Token: 0x04003844 RID: 14404
		public static readonly Texture2D HighlightTex = SolidColorMaterials.NewSolidColorTexture(new Color(1f, 1f, 1f, 0.1f));

		// Token: 0x04003845 RID: 14405
		public static readonly Texture2D HighlightSelectedTex = SolidColorMaterials.NewSolidColorTexture(new Color(1f, 0.94f, 0.5f, 0.18f));

		// Token: 0x04003846 RID: 14406
		public static readonly Texture2D ArrowTexRight = ContentFinder<Texture2D>.Get("UI/Widgets/ArrowRight", true);

		// Token: 0x04003847 RID: 14407
		public static readonly Texture2D ArrowTexLeft = ContentFinder<Texture2D>.Get("UI/Widgets/ArrowLeft", true);

		// Token: 0x04003848 RID: 14408
		public static readonly Texture2D ConcaveArrowTexRight = ContentFinder<Texture2D>.Get("UI/Widgets/ConcaveArrowRight", true);

		// Token: 0x04003849 RID: 14409
		public static readonly Texture2D ConcaveArrowTexLeft = ContentFinder<Texture2D>.Get("UI/Widgets/ConcaveArrowLeft", true);

		// Token: 0x0400384A RID: 14410
		public static readonly Texture2D WinExpandWidget = ContentFinder<Texture2D>.Get("UI/Widgets/WinExpandWidget", true);

		// Token: 0x0400384B RID: 14411
		public static readonly Texture2D ArrowTex = ContentFinder<Texture2D>.Get("UI/Misc/AlertFlashArrow", true);

		// Token: 0x0400384C RID: 14412
		public static readonly Texture2D RotLeftTex = ContentFinder<Texture2D>.Get("UI/Widgets/RotLeft", true);

		// Token: 0x0400384D RID: 14413
		public static readonly Texture2D RotRightTex = ContentFinder<Texture2D>.Get("UI/Widgets/RotRight", true);

		// Token: 0x0400384E RID: 14414
		public static readonly Texture2D GuiltyTex = ContentFinder<Texture2D>.Get("UI/Icons/Guilty", true);

		// Token: 0x0400384F RID: 14415
		public static readonly Texture2D CopyTex = ContentFinder<Texture2D>.Get("UI/Buttons/Copy", true);

		// Token: 0x04003850 RID: 14416
		public static readonly Texture2D DismissTex = ContentFinder<Texture2D>.Get("UI/Buttons/Dismiss", true);

		// Token: 0x04003851 RID: 14417
		public static readonly Texture2D RenameTex = ContentFinder<Texture2D>.Get("UI/Buttons/Rename", true);

		// Token: 0x04003852 RID: 14418
		public static readonly Texture2D MakeDefault = ContentFinder<Texture2D>.Get("UI/Buttons/MakeDefault", true);

		// Token: 0x04003853 RID: 14419
		public static readonly Texture2D RectHighlight = ContentFinder<Texture2D>.Get("UI/Overlays/HighlightAtlas", true);

		// Token: 0x04003854 RID: 14420
		public static readonly Texture2D GrayBg = SolidColorMaterials.NewSolidColorTexture(new ColorInt(51, 63, 51, 200).ToColor);

		// Token: 0x04003855 RID: 14421
		public static readonly Texture2D DotHighlight = ContentFinder<Texture2D>.Get("UI/Overlays/DotHighlight", true);

		// Token: 0x04003856 RID: 14422
		public static readonly Texture2D SelectionBracketWhole = ContentFinder<Texture2D>.Get("UI/Overlays/SelectionBracketWhole", true);

		// Token: 0x04003857 RID: 14423
		public static readonly Texture2D Placeholder = ContentFinder<Texture2D>.Get("PlaceholderImage", true);

		// Token: 0x04003858 RID: 14424
		public static readonly Color OldActiveResearchColor = new ColorInt(0, 64, 64, 255).ToColor;

		// Token: 0x04003859 RID: 14425
		public static readonly Color OldFinishedResearchColor = new ColorInt(0, 64, 16, 255).ToColor;

		// Token: 0x0400385A RID: 14426
		public static readonly Color AvailResearchColor = new ColorInt(32, 32, 32, 255).ToColor;

		// Token: 0x0400385B RID: 14427
		public static readonly Color ActiveResearchColor = new ColorInt(81, 66, 7, 255).ToColor;

		// Token: 0x0400385C RID: 14428
		public static readonly Color OtherActiveResearchColor = new ColorInt(78, 109, 129, 130).ToColor;

		// Token: 0x0400385D RID: 14429
		public static readonly Color FinishedResearchColor = new ColorInt(0, 64, 64, 255).ToColor;

		// Token: 0x0400385E RID: 14430
		public static readonly Color LockedResearchColor = new ColorInt(42, 42, 42, 255).ToColor;

		// Token: 0x0400385F RID: 14431
		public static readonly Color HiddenResearchColor = new ColorInt(42, 42, 42, 255).ToColor;

		// Token: 0x04003860 RID: 14432
		public static readonly Color HighlightBgResearchColor = new ColorInt(30, 30, 30, 255).ToColor;

		// Token: 0x04003861 RID: 14433
		public static readonly Color HighlightBorderResearchColor = new ColorInt(160, 160, 160, 255).ToColor;

		// Token: 0x04003862 RID: 14434
		public static readonly Color BorderResearchSelectedColor = new ColorInt(240, 240, 240, 255).ToColor;

		// Token: 0x04003863 RID: 14435
		public static readonly Color BorderResearchingColor = new ColorInt(253, 225, 114, 255).ToColor;

		// Token: 0x04003864 RID: 14436
		public static readonly Color DefaultBorderResearchColor = new ColorInt(80, 80, 80, 255).ToColor;

		// Token: 0x04003865 RID: 14437
		public static readonly Color ResearchMainTabColor = new Color(0.2f, 0.8f, 0.85f);

		// Token: 0x04003866 RID: 14438
		public static readonly Color FinishedResearchColorTransparent = new ColorInt(78, 109, 129, 140).ToColor;

		// Token: 0x04003867 RID: 14439
		public static readonly Color DefaultLineResearchColor = new ColorInt(60, 60, 60, 255).ToColor;

		// Token: 0x04003868 RID: 14440
		public static readonly Color HighlightLineResearchColor = new ColorInt(51, 205, 217, 255).ToColor;

		// Token: 0x04003869 RID: 14441
		public static readonly Color DependencyOutlineResearchColor = new ColorInt(217, 20, 51, 255).ToColor;

		// Token: 0x0400386A RID: 14442
		public static readonly Texture2D FastFillTex = Texture2D.whiteTexture;

		// Token: 0x0400386B RID: 14443
		public static readonly GUIStyle FastFillStyle = new GUIStyle
		{
			normal = new GUIStyleState
			{
				background = TexUI.FastFillTex
			}
		};

		// Token: 0x0400386C RID: 14444
		public static readonly Texture2D TextBGBlack = ContentFinder<Texture2D>.Get("UI/Widgets/TextBGBlack", true);

		// Token: 0x0400386D RID: 14445
		public static readonly Texture2D GrayTextBG = ContentFinder<Texture2D>.Get("UI/Overlays/GrayTextBG", true);

		// Token: 0x0400386E RID: 14446
		public static readonly Texture2D FloatMenuOptionBG = ContentFinder<Texture2D>.Get("UI/Widgets/FloatMenuOptionBG", true);

		// Token: 0x0400386F RID: 14447
		public static readonly Material GrayscaleGUI = MatLoader.LoadMat("Misc/GrayscaleGUI", -1);

		// Token: 0x04003870 RID: 14448
		public static Texture2D SteamDeck_ButtonA = ContentFinder<Texture2D>.Get("UI/Icons/SteamDeck/button_a", true);
	}
}
