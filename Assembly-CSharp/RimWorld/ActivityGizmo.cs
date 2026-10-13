using System;
using System.Text;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02002BA8 RID: 11176
	[StaticConstructorOnStartup]
	public class ActivityGizmo : Gizmo_Slider
	{
		// Token: 0x1700271C RID: 10012
		// (get) Token: 0x0600F9D0 RID: 63952 RVA: 0x0048A7CB File Offset: 0x004889CB
		private CompActivity Comp
		{
			get
			{
				return this.thing.GetComp<CompActivity>();
			}
		}

		// Token: 0x1700271D RID: 10013
		// (get) Token: 0x0600F9D1 RID: 63953 RVA: 0x0048A7D8 File Offset: 0x004889D8
		protected override string Title
		{
			get
			{
				return "ActivityGizmo".Translate();
			}
		}

		// Token: 0x1700271E RID: 10014
		// (get) Token: 0x0600F9D2 RID: 63954 RVA: 0x0048A7E9 File Offset: 0x004889E9
		protected override float ValuePercent
		{
			get
			{
				return this.Comp.ActivityLevel;
			}
		}

		// Token: 0x1700271F RID: 10015
		// (get) Token: 0x0600F9D3 RID: 63955 RVA: 0x0048A7F6 File Offset: 0x004889F6
		protected override bool IsDraggable
		{
			get
			{
				return this.Comp.CanBeSuppressed;
			}
		}

		// Token: 0x17002720 RID: 10016
		// (get) Token: 0x0600F9D4 RID: 63956 RVA: 0x0048A803 File Offset: 0x00488A03
		protected override FloatRange DragRange
		{
			get
			{
				return new FloatRange(0.1f, 0.9f);
			}
		}

		// Token: 0x17002721 RID: 10017
		// (get) Token: 0x0600F9D5 RID: 63957 RVA: 0x0048A814 File Offset: 0x00488A14
		protected override string HighlightTag
		{
			get
			{
				return "ActivityGizmo";
			}
		}

		// Token: 0x17002722 RID: 10018
		// (get) Token: 0x0600F9D6 RID: 63958 RVA: 0x0048A81B File Offset: 0x00488A1B
		// (set) Token: 0x0600F9D7 RID: 63959 RVA: 0x0048A822 File Offset: 0x00488A22
		protected override bool DraggingBar
		{
			get
			{
				return ActivityGizmo.draggingBar;
			}
			set
			{
				ActivityGizmo.draggingBar = value;
			}
		}

		// Token: 0x17002723 RID: 10019
		// (get) Token: 0x0600F9D8 RID: 63960 RVA: 0x0048A82A File Offset: 0x00488A2A
		// (set) Token: 0x0600F9D9 RID: 63961 RVA: 0x0048A837 File Offset: 0x00488A37
		protected override float Target
		{
			get
			{
				return this.Comp.suppressIfAbove;
			}
			set
			{
				this.Comp.suppressIfAbove = value;
			}
		}

		// Token: 0x0600F9DA RID: 63962 RVA: 0x0048A845 File Offset: 0x00488A45
		public ActivityGizmo(ThingWithComps thing)
		{
			this.thing = thing;
		}

		// Token: 0x0600F9DB RID: 63963 RVA: 0x0048A854 File Offset: 0x00488A54
		protected override string GetTooltip()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("ActivitySuppressionTooltipTitle".Translate().CapitalizeFirst().Colorize(ColoredText.TipSectionTitleColor));
			stringBuilder.AppendLine();
			stringBuilder.Append("ActivitySuppressionTooltipDesc".Translate(this.thing.LabelNoParenthesis, this.Comp.suppressIfAbove.ToStringPercent("0").Colorize(ColoredText.TipSectionTitleColor).Named("LEVEL")).Resolve());
			this.Comp.Props.Worker.GetSummary(this.thing, stringBuilder);
			if (!this.Comp.suppressionEnabled)
			{
				stringBuilder.Append("\n\n" + "ActivitySuppressionTooltipDisabled".Translate(this.thing.LabelNoParenthesis).CapitalizeFirst().Colorize(ColoredText.FactionColor_Hostile));
			}
			foreach (IActivity activity in this.thing.GetComps<IActivity>())
			{
				if (!activity.ActivityTooltipExtra().NullOrEmpty())
				{
					stringBuilder.Append("\n\n" + activity.ActivityTooltipExtra());
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x0600F9DC RID: 63964 RVA: 0x0048A9B4 File Offset: 0x00488BB4
		protected override void DrawHeader(Rect headerRect, ref bool mouseOverElement)
		{
			headerRect.xMax -= 24f;
			if (this.Comp.CanBeSuppressed)
			{
				Rect rect = new Rect(headerRect.xMax, headerRect.y, 24f, 24f);
				GUI.DrawTexture(rect, ActivityGizmo.IconTex);
				GUI.DrawTexture(new Rect(rect.center.x, rect.y, rect.width / 2f, rect.height / 2f), this.Comp.suppressionEnabled ? Widgets.CheckboxOnTex : Widgets.CheckboxOffTex);
				if (Widgets.ButtonInvisible(rect, true))
				{
					this.Comp.suppressionEnabled = !this.Comp.suppressionEnabled;
					if (this.Comp.suppressionEnabled)
					{
						SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
					}
					else
					{
						SoundDefOf.Tick_Low.PlayOneShotOnCamera(null);
					}
				}
				if (Mouse.IsOver(rect))
				{
					Widgets.DrawHighlight(rect);
					TooltipHandler.TipRegion(rect, new Func<string>(this.GetTooltipDesc), 828267373);
					mouseOverElement = true;
				}
			}
			base.DrawHeader(headerRect, ref mouseOverElement);
		}

		// Token: 0x0600F9DD RID: 63965 RVA: 0x0048AAD8 File Offset: 0x00488CD8
		private string GetTooltipDesc()
		{
			string arg = (this.Comp.suppressionEnabled ? "On" : "Off").Translate().ToString().UncapitalizeFirst();
			string arg2 = this.Comp.suppressIfAbove.ToStringPercent("0").Colorize(ColoredText.TipSectionTitleColor);
			return "ActivitySuppressionToggleTooltipDesc".Translate(this.thing.LabelNoParenthesis, arg2.Named("LEVEL"), arg.Named("ONOFF")).Resolve();
		}

		// Token: 0x0400A442 RID: 42050
		private readonly ThingWithComps thing;

		// Token: 0x0400A443 RID: 42051
		private static bool draggingBar;

		// Token: 0x0400A444 RID: 42052
		private static readonly Texture2D IconTex = ContentFinder<Texture2D>.Get("UI/Icons/SuppressionToggle", true);
	}
}
