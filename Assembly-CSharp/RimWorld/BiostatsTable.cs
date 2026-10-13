using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02003066 RID: 12390
	public static class BiostatsTable
	{
		// Token: 0x0601164E RID: 71246 RVA: 0x004FE3A8 File Offset: 0x004FC5A8
		private static float MaxLabelWidth(int arc)
		{
			float num = 0f;
			int num2 = (arc > 0) ? BiostatsTable.Biostats.Length : (BiostatsTable.Biostats.Length - 1);
			for (int i = 0; i < num2; i++)
			{
				num = Mathf.Max(num, Text.CalcSize(BiostatsTable.Biostats[i].labelKey.Translate().CapitalizeFirst()).x);
			}
			return num;
		}

		// Token: 0x0601164F RID: 71247 RVA: 0x004FE414 File Offset: 0x004FC614
		public static float HeightForBiostats(int arc)
		{
			float num = Text.LineHeight * 3f;
			if (arc > 0)
			{
				num += Text.LineHeight * 1.5f;
			}
			return num;
		}

		// Token: 0x06011650 RID: 71248 RVA: 0x004FE440 File Offset: 0x004FC640
		public static void Draw(Rect rect, int gcx, int met, int arc, bool drawMax, bool ignoreLimits, int maxGCX = -1)
		{
			int num = (arc > 0) ? BiostatsTable.Biostats.Length : (BiostatsTable.Biostats.Length - 1);
			float num2 = BiostatsTable.MaxLabelWidth(arc);
			float num3 = rect.height / (float)num;
			GUI.BeginGroup(rect);
			for (int i = 0; i < num; i++)
			{
				Rect position = new Rect(0f, (float)i * num3 + (num3 - 22f) / 2f, 22f, 22f);
				Rect rect2 = new Rect(position.xMax + 4f, (float)i * num3, num2, num3);
				Rect rect3 = new Rect(0f, rect2.y, rect.width, rect2.height);
				if (i % 2 == 1)
				{
					Widgets.DrawLightHighlight(rect3);
				}
				Widgets.DrawHighlightIfMouseover(rect3);
				rect3.xMax = rect2.xMax + 4f + 90f;
				TaggedString taggedString = BiostatsTable.Biostats[i].descKey.Translate();
				if (maxGCX >= 0 && BiostatsTable.Biostats[i].displayMaxGCXInfo)
				{
					taggedString += "\n\n" + "MaxComplexityDesc".Translate();
				}
				TooltipHandler.TipRegion(rect3, taggedString);
				GUI.DrawTexture(position, BiostatsTable.Biostats[i].icon);
				Text.Anchor = TextAnchor.MiddleLeft;
				Widgets.Label(rect2, BiostatsTable.Biostats[i].labelKey.Translate().CapitalizeFirst());
				Text.Anchor = TextAnchor.UpperLeft;
			}
			float num4 = num2 + 4f + 22f + 4f;
			string text = gcx.ToString();
			string text2 = met.ToStringWithSign();
			if (drawMax && !ignoreLimits)
			{
				if (maxGCX >= 0)
				{
					if (gcx > maxGCX)
					{
						text = text.Colorize(ColorLibrary.RedReadable);
					}
					text = text + " / " + maxGCX.ToString();
				}
				if (met < GeneTuning.BiostatRange.TrueMin)
				{
					text2 = text2 + (" (" + "min".Translate() + " ") + GeneTuning.BiostatRange.TrueMin.ToString() + ")";
					text2 = text2.Colorize(ColorLibrary.RedReadable);
				}
			}
			Text.Anchor = TextAnchor.MiddleCenter;
			Widgets.Label(new Rect(num4, 0f, 90f, num3), text);
			Widgets.Label(new Rect(num4, num3, 90f, num3), text2);
			if (arc > 0)
			{
				Widgets.Label(new Rect(num4, num3 * 2f, 90f, num3), arc.ToString());
			}
			Text.Anchor = TextAnchor.MiddleLeft;
			float width = rect.width - num2 - 90f - 22f - 4f;
			Rect rect4 = new Rect(num4 + 90f + 4f, num3, width, num3);
			if (rect4.width != BiostatsTable.cachedWidth)
			{
				BiostatsTable.cachedWidth = rect4.width;
				BiostatsTable.truncateCache.Clear();
			}
			string str = BiostatsTable.MetabolismDescAt(met);
			Widgets.Label(rect4, str.Truncate(rect4.width, BiostatsTable.truncateCache));
			if (Mouse.IsOver(rect4) && !str.NullOrEmpty())
			{
				TooltipHandler.TipRegion(rect4, str);
			}
			Text.Anchor = TextAnchor.UpperLeft;
			GUI.EndGroup();
		}

		// Token: 0x06011651 RID: 71249 RVA: 0x004FE793 File Offset: 0x004FC993
		private static string MetabolismDescAt(int met)
		{
			if (met == 0)
			{
				return string.Empty;
			}
			return "HungerRate".Translate() + " x" + GeneTuning.MetabolismToFoodConsumptionFactorCurve.Evaluate((float)met).ToStringPercent();
		}

		// Token: 0x0400B40D RID: 46093
		private static float cachedWidth;

		// Token: 0x0400B40E RID: 46094
		private const float NumberWidth = 90f;

		// Token: 0x0400B40F RID: 46095
		private const float IconSize = 22f;

		// Token: 0x0400B410 RID: 46096
		private static readonly BiostatsTable.BiostatData[] Biostats = new BiostatsTable.BiostatData[]
		{
			new BiostatsTable.BiostatData("Complexity", "ComplexityDesc", GeneUtility.GCXTex.Texture, true),
			new BiostatsTable.BiostatData("Metabolism", "MetabolismDesc", GeneUtility.METTex.Texture, false),
			new BiostatsTable.BiostatData("ArchitesRequired", "ArchitesRequiredDesc", GeneUtility.ARCTex.Texture, false)
		};

		// Token: 0x0400B411 RID: 46097
		private static Dictionary<string, string> truncateCache = new Dictionary<string, string>();

		// Token: 0x02003067 RID: 12391
		private struct BiostatData
		{
			// Token: 0x06011653 RID: 71251 RVA: 0x004FE855 File Offset: 0x004FCA55
			public BiostatData(string labelKey, string descKey, Texture2D icon, bool displayMaxGCXInfo)
			{
				this.labelKey = labelKey;
				this.descKey = descKey;
				this.icon = icon;
				this.displayMaxGCXInfo = displayMaxGCXInfo;
			}

			// Token: 0x0400B412 RID: 46098
			public string labelKey;

			// Token: 0x0400B413 RID: 46099
			public string descKey;

			// Token: 0x0400B414 RID: 46100
			public Texture2D icon;

			// Token: 0x0400B415 RID: 46101
			public bool displayMaxGCXInfo;
		}
	}
}
