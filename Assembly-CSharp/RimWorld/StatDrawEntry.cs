using System;
using System.Collections.Generic;
using System.Text;
using JetBrains.Annotations;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02003244 RID: 12868
	public class StatDrawEntry
	{
		// Token: 0x17002EC0 RID: 11968
		// (get) Token: 0x0601215F RID: 74079 RVA: 0x0054BB8D File Offset: 0x00549D8D
		public string LabelCap
		{
			get
			{
				if (this.labelInt != null)
				{
					return this.labelInt.CapitalizeFirst();
				}
				return this.stat.LabelCap;
			}
		}

		// Token: 0x17002EC1 RID: 11969
		// (get) Token: 0x06012160 RID: 74080 RVA: 0x0054BBB4 File Offset: 0x00549DB4
		public string ValueString
		{
			get
			{
				if (this.numberSense == ToStringNumberSense.Factor)
				{
					return this.value.ToStringByStyle(ToStringStyle.PercentZero, ToStringNumberSense.Absolute);
				}
				if (this.valueStringInt == null)
				{
					return this.stat.Worker.GetStatDrawEntryLabel(this.stat, this.value, this.numberSense, this.optionalReq, !this.forceUnfinalizedMode);
				}
				return this.valueStringInt;
			}
		}

		// Token: 0x17002EC2 RID: 11970
		// (get) Token: 0x06012161 RID: 74081 RVA: 0x0054BC18 File Offset: 0x00549E18
		public int DisplayPriorityWithinCategory
		{
			get
			{
				return this.displayOrderWithinCategory;
			}
		}

		// Token: 0x06012162 RID: 74082 RVA: 0x0054BC20 File Offset: 0x00549E20
		public StatDrawEntry(StatCategoryDef category, StatDef stat, float value, StatRequest optionalReq, ToStringNumberSense numberSense = ToStringNumberSense.Undefined, int? overrideDisplayPriorityWithinCategory = null, bool forceUnfinalizedMode = false)
		{
			this.category = category;
			this.stat = stat;
			this.labelInt = null;
			this.value = value;
			this.valueStringInt = null;
			this.displayOrderWithinCategory = ((overrideDisplayPriorityWithinCategory != null) ? overrideDisplayPriorityWithinCategory.Value : stat.displayPriorityInCategory);
			this.optionalReq = optionalReq;
			this.forceUnfinalizedMode = forceUnfinalizedMode;
			this.overridesHideStats = stat.overridesHideStats;
			this.hasOptionalReq = true;
			if (numberSense == ToStringNumberSense.Undefined)
			{
				this.numberSense = stat.toStringNumberSense;
				return;
			}
			this.numberSense = numberSense;
		}

		// Token: 0x06012163 RID: 74083 RVA: 0x0054BCB0 File Offset: 0x00549EB0
		public StatDrawEntry(StatCategoryDef category, StatDef stat, string value)
		{
			this.category = category;
			this.stat = stat;
			this.labelInt = null;
			this.valueStringInt = value;
			this.overridesHideStats = stat.overridesHideStats;
			this.hasOptionalReq = false;
			this.numberSense = ToStringNumberSense.Undefined;
		}

		// Token: 0x06012164 RID: 74084 RVA: 0x0054BCF0 File Offset: 0x00549EF0
		public StatDrawEntry(StatCategoryDef category, string label, string valueString, string reportText, int displayPriorityWithinCategory, string overrideReportTitle = null, IEnumerable<Dialog_InfoCard.Hyperlink> hyperlinks = null, bool forceUnfinalizedMode = false, bool overridesHideStats = false)
		{
			this.category = category;
			this.stat = null;
			this.labelInt = label;
			this.value = 0f;
			this.valueStringInt = valueString;
			this.displayOrderWithinCategory = displayPriorityWithinCategory;
			this.numberSense = ToStringNumberSense.Absolute;
			this.overrideReportText = reportText;
			this.overrideReportTitle = overrideReportTitle;
			this.hyperlinks = hyperlinks;
			this.forceUnfinalizedMode = forceUnfinalizedMode;
			this.overridesHideStats = overridesHideStats;
		}

		// Token: 0x06012165 RID: 74085 RVA: 0x0054BD64 File Offset: 0x00549F64
		public StatDrawEntry(StatCategoryDef category, StatDef stat)
		{
			this.category = category;
			this.stat = stat;
			this.labelInt = null;
			this.value = 0f;
			this.valueStringInt = "-";
			this.displayOrderWithinCategory = stat.displayPriorityInCategory;
			this.numberSense = ToStringNumberSense.Undefined;
		}

		// Token: 0x06012166 RID: 74086 RVA: 0x0054BDB8 File Offset: 0x00549FB8
		public bool ShouldDisplay([CanBeNull] Thing thing = null)
		{
			return (thing == null || !thing.def.hideStats || this.overridesHideStats || DebugSettings.showHiddenInfo) && (this.stat == null || !Mathf.Approximately(this.value, this.stat.hideAtValue));
		}

		// Token: 0x06012167 RID: 74087 RVA: 0x0054BE09 File Offset: 0x0054A009
		public IEnumerable<Dialog_InfoCard.Hyperlink> GetHyperlinks(StatRequest req)
		{
			if (this.hyperlinks != null)
			{
				return this.hyperlinks;
			}
			if (this.stat != null)
			{
				return this.stat.Worker.GetInfoCardHyperlinks(req);
			}
			return null;
		}

		// Token: 0x06012168 RID: 74088 RVA: 0x0054BE38 File Offset: 0x0054A038
		public string GetExplanationText(StatRequest optionalReq)
		{
			if (this.explanationText == null)
			{
				this.WriteExplanationTextInt();
			}
			string result;
			if (optionalReq.Empty || this.stat == null)
			{
				result = this.explanationText;
			}
			else
			{
				result = string.Format("{0}\n\n{1}", this.explanationText, this.stat.Worker.GetExplanationFull(optionalReq, this.numberSense, this.value));
			}
			return result;
		}

		// Token: 0x06012169 RID: 74089 RVA: 0x0054BEA0 File Offset: 0x0054A0A0
		private void WriteExplanationTextInt()
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (!this.overrideReportTitle.NullOrEmpty())
			{
				stringBuilder.AppendLine(this.overrideReportTitle);
			}
			if (!this.overrideReportText.NullOrEmpty())
			{
				stringBuilder.AppendLine(this.overrideReportText);
			}
			else if (this.stat != null)
			{
				stringBuilder.AppendLine(this.stat.description);
			}
			stringBuilder.AppendLine();
			this.explanationText = stringBuilder.ToString().TrimEndNewlines();
		}

		// Token: 0x0601216A RID: 74090 RVA: 0x0054BF1B File Offset: 0x0054A11B
		public StatDrawEntry SetReportText(string reportText)
		{
			this.overrideReportText = reportText;
			return this;
		}

		// Token: 0x0601216B RID: 74091 RVA: 0x0054BF28 File Offset: 0x0054A128
		public float Draw(float x, float y, float width, bool selected, bool highlightLabel, bool lowlightLabel, Action clickedCallback, Action mousedOverCallback, Vector2 scrollPosition, Rect scrollOutRect, string valueCached = null)
		{
			float num = width * 0.45f;
			string text = valueCached ?? this.ValueString;
			Rect rect = new Rect(8f, y, width, Text.CalcHeight(text, num));
			if (y - scrollPosition.y + rect.height >= 0f && y - scrollPosition.y <= scrollOutRect.height)
			{
				GUI.color = Color.white;
				if (selected)
				{
					Widgets.DrawHighlightSelected(rect);
				}
				else if (Mouse.IsOver(rect))
				{
					Widgets.DrawHighlight(rect);
				}
				if (highlightLabel)
				{
					Widgets.DrawTextHighlight(rect, 4f, null);
				}
				if (lowlightLabel)
				{
					GUI.color = Color.grey;
				}
				Rect rect2 = rect;
				rect2.width -= num;
				Widgets.Label(rect2, this.LabelCap);
				Rect rect3 = rect;
				rect3.x = rect2.xMax;
				rect3.width = num;
				Widgets.Label(rect3, text);
				GUI.color = Color.white;
				if (this.stat != null && Mouse.IsOver(rect))
				{
					StatDef localStat = this.stat;
					TooltipHandler.TipRegion(rect, new TipSignal(() => localStat.LabelCap + ": " + localStat.description, this.stat.GetHashCode()));
				}
				if (Widgets.ButtonInvisible(rect, true))
				{
					clickedCallback();
				}
				if (Mouse.IsOver(rect))
				{
					mousedOverCallback();
				}
			}
			return rect.height;
		}

		// Token: 0x0601216C RID: 74092 RVA: 0x0054C091 File Offset: 0x0054A291
		public bool Same(StatDrawEntry other)
		{
			return other != null && (this == other || (this.stat == other.stat && this.labelInt == other.labelInt));
		}

		// Token: 0x0601216D RID: 74093 RVA: 0x0054C0BF File Offset: 0x0054A2BF
		public override string ToString()
		{
			return string.Concat(new string[]
			{
				"(",
				this.LabelCap,
				": ",
				this.ValueString,
				")"
			});
		}

		// Token: 0x0400BBD2 RID: 48082
		public StatCategoryDef category;

		// Token: 0x0400BBD3 RID: 48083
		private int displayOrderWithinCategory;

		// Token: 0x0400BBD4 RID: 48084
		public StatDef stat;

		// Token: 0x0400BBD5 RID: 48085
		private float value;

		// Token: 0x0400BBD6 RID: 48086
		public StatRequest optionalReq;

		// Token: 0x0400BBD7 RID: 48087
		public bool hasOptionalReq;

		// Token: 0x0400BBD8 RID: 48088
		public bool forceUnfinalizedMode;

		// Token: 0x0400BBD9 RID: 48089
		public bool overridesHideStats;

		// Token: 0x0400BBDA RID: 48090
		private IEnumerable<Dialog_InfoCard.Hyperlink> hyperlinks;

		// Token: 0x0400BBDB RID: 48091
		private string labelInt;

		// Token: 0x0400BBDC RID: 48092
		private string valueStringInt;

		// Token: 0x0400BBDD RID: 48093
		private string overrideReportText;

		// Token: 0x0400BBDE RID: 48094
		private string overrideReportTitle;

		// Token: 0x0400BBDF RID: 48095
		private string explanationText;

		// Token: 0x0400BBE0 RID: 48096
		private ToStringNumberSense numberSense;
	}
}
