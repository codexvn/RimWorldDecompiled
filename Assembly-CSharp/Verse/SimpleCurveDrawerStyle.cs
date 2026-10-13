using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000C84 RID: 3204
	public class SimpleCurveDrawerStyle
	{
		// Token: 0x17000C83 RID: 3203
		// (get) Token: 0x0600521B RID: 21019 RVA: 0x001A9C64 File Offset: 0x001A7E64
		// (set) Token: 0x0600521C RID: 21020 RVA: 0x001A9C6C File Offset: 0x001A7E6C
		public bool DrawBackground { get; set; }

		// Token: 0x17000C84 RID: 3204
		// (get) Token: 0x0600521D RID: 21021 RVA: 0x001A9C75 File Offset: 0x001A7E75
		// (set) Token: 0x0600521E RID: 21022 RVA: 0x001A9C7D File Offset: 0x001A7E7D
		public bool DrawBackgroundLines { get; set; }

		// Token: 0x17000C85 RID: 3205
		// (get) Token: 0x0600521F RID: 21023 RVA: 0x001A9C86 File Offset: 0x001A7E86
		// (set) Token: 0x06005220 RID: 21024 RVA: 0x001A9C8E File Offset: 0x001A7E8E
		public bool DrawMeasures { get; set; }

		// Token: 0x17000C86 RID: 3206
		// (get) Token: 0x06005221 RID: 21025 RVA: 0x001A9C97 File Offset: 0x001A7E97
		// (set) Token: 0x06005222 RID: 21026 RVA: 0x001A9C9F File Offset: 0x001A7E9F
		public bool DrawPoints { get; set; }

		// Token: 0x17000C87 RID: 3207
		// (get) Token: 0x06005223 RID: 21027 RVA: 0x001A9CA8 File Offset: 0x001A7EA8
		// (set) Token: 0x06005224 RID: 21028 RVA: 0x001A9CB0 File Offset: 0x001A7EB0
		public bool DrawLegend { get; set; }

		// Token: 0x17000C88 RID: 3208
		// (get) Token: 0x06005225 RID: 21029 RVA: 0x001A9CB9 File Offset: 0x001A7EB9
		// (set) Token: 0x06005226 RID: 21030 RVA: 0x001A9CC1 File Offset: 0x001A7EC1
		public bool DrawCurveMousePoint { get; set; }

		// Token: 0x17000C89 RID: 3209
		// (get) Token: 0x06005227 RID: 21031 RVA: 0x001A9CCA File Offset: 0x001A7ECA
		// (set) Token: 0x06005228 RID: 21032 RVA: 0x001A9CD2 File Offset: 0x001A7ED2
		public bool OnlyPositiveValues { get; set; }

		// Token: 0x17000C8A RID: 3210
		// (get) Token: 0x06005229 RID: 21033 RVA: 0x001A9CDB File Offset: 0x001A7EDB
		// (set) Token: 0x0600522A RID: 21034 RVA: 0x001A9CE3 File Offset: 0x001A7EE3
		public bool UseFixedSection { get; set; }

		// Token: 0x17000C8B RID: 3211
		// (get) Token: 0x0600522B RID: 21035 RVA: 0x001A9CEC File Offset: 0x001A7EEC
		// (set) Token: 0x0600522C RID: 21036 RVA: 0x001A9CF4 File Offset: 0x001A7EF4
		public bool UseFixedScale { get; set; }

		// Token: 0x17000C8C RID: 3212
		// (get) Token: 0x0600522D RID: 21037 RVA: 0x001A9CFD File Offset: 0x001A7EFD
		// (set) Token: 0x0600522E RID: 21038 RVA: 0x001A9D05 File Offset: 0x001A7F05
		public bool UseAntiAliasedLines { get; set; }

		// Token: 0x17000C8D RID: 3213
		// (get) Token: 0x0600522F RID: 21039 RVA: 0x001A9D0E File Offset: 0x001A7F0E
		// (set) Token: 0x06005230 RID: 21040 RVA: 0x001A9D16 File Offset: 0x001A7F16
		public bool PointsRemoveOptimization { get; set; }

		// Token: 0x17000C8E RID: 3214
		// (get) Token: 0x06005231 RID: 21041 RVA: 0x001A9D1F File Offset: 0x001A7F1F
		// (set) Token: 0x06005232 RID: 21042 RVA: 0x001A9D27 File Offset: 0x001A7F27
		public int MeasureLabelsXCount { get; set; }

		// Token: 0x17000C8F RID: 3215
		// (get) Token: 0x06005233 RID: 21043 RVA: 0x001A9D30 File Offset: 0x001A7F30
		// (set) Token: 0x06005234 RID: 21044 RVA: 0x001A9D38 File Offset: 0x001A7F38
		public int MeasureLabelsYCount { get; set; }

		// Token: 0x17000C90 RID: 3216
		// (get) Token: 0x06005235 RID: 21045 RVA: 0x001A9D41 File Offset: 0x001A7F41
		// (set) Token: 0x06005236 RID: 21046 RVA: 0x001A9D49 File Offset: 0x001A7F49
		public bool XIntegersOnly { get; set; }

		// Token: 0x17000C91 RID: 3217
		// (get) Token: 0x06005237 RID: 21047 RVA: 0x001A9D52 File Offset: 0x001A7F52
		// (set) Token: 0x06005238 RID: 21048 RVA: 0x001A9D5A File Offset: 0x001A7F5A
		public bool YIntegersOnly { get; set; }

		// Token: 0x17000C92 RID: 3218
		// (get) Token: 0x06005239 RID: 21049 RVA: 0x001A9D63 File Offset: 0x001A7F63
		// (set) Token: 0x0600523A RID: 21050 RVA: 0x001A9D6B File Offset: 0x001A7F6B
		public string LabelX { get; set; }

		// Token: 0x17000C93 RID: 3219
		// (get) Token: 0x0600523B RID: 21051 RVA: 0x001A9D74 File Offset: 0x001A7F74
		// (set) Token: 0x0600523C RID: 21052 RVA: 0x001A9D7C File Offset: 0x001A7F7C
		public FloatRange FixedSection { get; set; }

		// Token: 0x17000C94 RID: 3220
		// (get) Token: 0x0600523D RID: 21053 RVA: 0x001A9D85 File Offset: 0x001A7F85
		// (set) Token: 0x0600523E RID: 21054 RVA: 0x001A9D8D File Offset: 0x001A7F8D
		public Vector2 FixedScale { get; set; }

		// Token: 0x0600523F RID: 21055 RVA: 0x001A9D98 File Offset: 0x001A7F98
		public SimpleCurveDrawerStyle()
		{
			this.DrawBackground = false;
			this.DrawBackgroundLines = true;
			this.DrawMeasures = false;
			this.DrawPoints = true;
			this.DrawLegend = false;
			this.DrawCurveMousePoint = false;
			this.OnlyPositiveValues = false;
			this.UseFixedSection = false;
			this.UseFixedScale = false;
			this.UseAntiAliasedLines = false;
			this.PointsRemoveOptimization = false;
			this.MeasureLabelsXCount = 5;
			this.MeasureLabelsYCount = 5;
			this.XIntegersOnly = false;
			this.YIntegersOnly = false;
			this.LabelX = "x";
		}
	}
}
