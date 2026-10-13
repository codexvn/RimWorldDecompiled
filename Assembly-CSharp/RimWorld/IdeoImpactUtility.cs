using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020025F0 RID: 9712
	[StaticConstructorOnStartup]
	public static class IdeoImpactUtility
	{
		// Token: 0x0600D507 RID: 54535 RVA: 0x003EE950 File Offset: 0x003ECB50
		public static string MemeImpactLabel(int impact)
		{
			impact = Mathf.Clamp(impact, 1, 3);
			return string.Format("IdeoMemeImpactLabel_{0}", impact).Translate();
		}

		// Token: 0x0600D508 RID: 54536 RVA: 0x003EE976 File Offset: 0x003ECB76
		public static string OverallImpactLabel(int impact)
		{
			impact = Mathf.Clamp(impact, 1, 9);
			return string.Format("IdeoImpactLabel_{0}", impact).Translate();
		}

		// Token: 0x0600D509 RID: 54537 RVA: 0x003EE99D File Offset: 0x003ECB9D
		public static void DrawImpactIcon(Rect rect, int impact)
		{
			impact = Mathf.Clamp(impact, 1, 3);
			Color color = GUI.color;
			GUI.color = IdeoImpactUtility.IconTint;
			GUI.DrawTexture(rect, IdeoImpactUtility.ImpactIcons[impact]);
			GUI.color = color;
		}

		// Token: 0x040090B3 RID: 37043
		public const int MaxMemeImpact = 3;

		// Token: 0x040090B4 RID: 37044
		public const int MaxCombinedImpact = 9;

		// Token: 0x040090B5 RID: 37045
		private static readonly Color IconTint = Color.Lerp(ColoredText.ImpactColor, new Color(0.3f, 0.3f, 0.3f, 1f), 0.9f);

		// Token: 0x040090B6 RID: 37046
		private static readonly Texture2D[] ImpactIcons = new Texture2D[]
		{
			null,
			ContentFinder<Texture2D>.Get("UI/Icons/Impact/MemeImpact1", true),
			ContentFinder<Texture2D>.Get("UI/Icons/Impact/MemeImpact2", true),
			ContentFinder<Texture2D>.Get("UI/Icons/Impact/MemeImpact3", true)
		};
	}
}
