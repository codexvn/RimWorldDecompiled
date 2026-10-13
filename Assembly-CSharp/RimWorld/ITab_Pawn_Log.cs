using System;
using System.Collections.Generic;
using System.Text;
using LudeonTK;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020033B9 RID: 13241
	public class ITab_Pawn_Log : ITab
	{
		// Token: 0x170030AC RID: 12460
		// (get) Token: 0x060129C0 RID: 76224 RVA: 0x005819E0 File Offset: 0x0057FBE0
		private Pawn SelPawnForCombatInfo
		{
			get
			{
				if (this.SelPawn != null)
				{
					return this.SelPawn;
				}
				Corpse corpse = base.SelThing as Corpse;
				if (corpse != null)
				{
					return corpse.InnerPawn;
				}
				string str = "Social tab on non-pawn non-corpse ";
				Thing selThing = base.SelThing;
				throw new InvalidOperationException(str + ((selThing != null) ? selThing.ToString() : null));
			}
		}

		// Token: 0x060129C1 RID: 76225 RVA: 0x00581A34 File Offset: 0x0057FC34
		public ITab_Pawn_Log()
		{
			this.size = new Vector2(630f, 510f);
			this.labelKey = "TabLog";
		}

		// Token: 0x060129C2 RID: 76226 RVA: 0x00581A90 File Offset: 0x0057FC90
		protected override void FillTab()
		{
			Pawn selPawnForCombatInfo = this.SelPawnForCombatInfo;
			Rect rect = new Rect(0f, 0f, this.size.x, this.size.y);
			GameFont font = Text.Font;
			Text.Font = GameFont.Small;
			Rect rect2 = new Rect(ITab_Pawn_Log.ShowAllX, ITab_Pawn_Log.ToolbarHeight, ITab_Pawn_Log.ShowAllWidth, 24f);
			bool flag = this.showAll;
			Widgets.CheckboxLabeled(rect2, "ShowAll".Translate(), ref this.showAll, false, null, null, false, false);
			if (flag != this.showAll)
			{
				this.cachedLogDisplay = null;
			}
			Rect rect3 = new Rect(ITab_Pawn_Log.ShowCombatX, ITab_Pawn_Log.ToolbarHeight, ITab_Pawn_Log.ShowCombatWidth, 24f);
			bool flag2 = this.showCombat;
			Widgets.CheckboxLabeled(rect3, "ShowCombat".Translate(), ref this.showCombat, false, null, null, false, false);
			if (flag2 != this.showCombat)
			{
				this.cachedLogDisplay = null;
			}
			Rect rect4 = new Rect(ITab_Pawn_Log.ShowSocialX, ITab_Pawn_Log.ToolbarHeight, ITab_Pawn_Log.ShowSocialWidth, 24f);
			bool flag3 = this.showSocial;
			Widgets.CheckboxLabeled(rect4, "ShowSocial".Translate(), ref this.showSocial, false, null, null, false, false);
			if (flag3 != this.showSocial)
			{
				this.cachedLogDisplay = null;
			}
			Text.Font = font;
			if (this.cachedLogDisplay == null || this.cachedLogDisplayLastTick != selPawnForCombatInfo.records.LastBattleTick || this.cachedLogPlayLastTick != Find.PlayLog.LastTick || this.cachedLogForPawn != selPawnForCombatInfo)
			{
				this.cachedLogDisplay = ITab_Pawn_Log_Utility.GenerateLogLinesFor(selPawnForCombatInfo, this.showAll, this.showCombat, this.showSocial, 300);
				this.cachedLogDisplayLastTick = selPawnForCombatInfo.records.LastBattleTick;
				this.cachedLogPlayLastTick = Find.PlayLog.LastTick;
				this.cachedLogForPawn = selPawnForCombatInfo;
			}
			Rect rect5 = new Rect(rect.width - ITab_Pawn_Log.ButtonOffset, 0f, 18f, 24f);
			if (Widgets.ButtonImage(rect5, TexButton.Copy, true, null))
			{
				StringBuilder stringBuilder = new StringBuilder();
				foreach (ITab_Pawn_Log_Utility.LogLineDisplayable logLineDisplayable in this.cachedLogDisplay)
				{
					logLineDisplayable.AppendTo(stringBuilder);
				}
				GUIUtility.systemCopyBuffer = stringBuilder.ToString();
			}
			TooltipHandler.TipRegionByKey(rect5, "CopyLogTip");
			rect.yMin = 24f;
			rect = rect.ContractedBy(10f);
			float width = rect.width - 16f - 10f;
			float num = 0f;
			foreach (ITab_Pawn_Log_Utility.LogLineDisplayable logLineDisplayable2 in this.cachedLogDisplay)
			{
				if (logLineDisplayable2.Matches(this.logSeek))
				{
					this.scrollPosition.y = num - (logLineDisplayable2.GetHeight(width) + rect.height) / 2f;
				}
				num += logLineDisplayable2.GetHeight(width);
			}
			this.logSeek = null;
			if (num > 0f)
			{
				Rect rect6 = new Rect(0f, 0f, rect.width - 16f, num);
				this.data.StartNewDraw();
				Rect rect7 = rect6;
				rect7.yMin += this.scrollPosition.y;
				rect7.height = rect.height;
				Widgets.BeginScrollView(rect, ref this.scrollPosition, rect6, true);
				float num2 = 0f;
				foreach (ITab_Pawn_Log_Utility.LogLineDisplayable logLineDisplayable3 in this.cachedLogDisplay)
				{
					float height = logLineDisplayable3.GetHeight(width);
					if (rect7.Overlaps(new Rect(0f, num2, width, height)))
					{
						logLineDisplayable3.Draw(num2, width, this.data);
					}
					else
					{
						this.data.alternatingBackground = !this.data.alternatingBackground;
					}
					num2 += height;
				}
				Widgets.EndScrollView();
				return;
			}
			Text.Anchor = TextAnchor.MiddleCenter;
			GUI.color = Color.grey;
			Widgets.Label(new Rect(0f, 0f, this.size.x, this.size.y), "(" + "NoRecentEntries".Translate() + ")");
			Text.Anchor = TextAnchor.UpperLeft;
			GUI.color = Color.white;
		}

		// Token: 0x060129C3 RID: 76227 RVA: 0x00581F10 File Offset: 0x00580110
		public void SeekTo(LogEntry entry)
		{
			this.logSeek = entry;
		}

		// Token: 0x060129C4 RID: 76228 RVA: 0x00581F19 File Offset: 0x00580119
		public void Highlight(LogEntry entry)
		{
			this.data.highlightEntry = entry;
			this.data.highlightIntensity = 1f;
		}

		// Token: 0x060129C5 RID: 76229 RVA: 0x00581F37 File Offset: 0x00580137
		public override void Notify_ClearingAllMapsMemory()
		{
			base.Notify_ClearingAllMapsMemory();
			this.cachedLogForPawn = null;
		}

		// Token: 0x0400C142 RID: 49474
		public const float Width = 630f;

		// Token: 0x0400C143 RID: 49475
		[TweakValue("Interface", 0f, 1000f)]
		private static float ShowAllX = 60f;

		// Token: 0x0400C144 RID: 49476
		[TweakValue("Interface", 0f, 1000f)]
		private static float ShowAllWidth = 100f;

		// Token: 0x0400C145 RID: 49477
		[TweakValue("Interface", 0f, 1000f)]
		private static float ShowCombatX = 445f;

		// Token: 0x0400C146 RID: 49478
		[TweakValue("Interface", 0f, 1000f)]
		private static float ShowCombatWidth = 115f;

		// Token: 0x0400C147 RID: 49479
		[TweakValue("Interface", 0f, 1000f)]
		private static float ShowSocialX = 330f;

		// Token: 0x0400C148 RID: 49480
		[TweakValue("Interface", 0f, 1000f)]
		private static float ShowSocialWidth = 105f;

		// Token: 0x0400C149 RID: 49481
		[TweakValue("Interface", 0f, 20f)]
		private static float ToolbarHeight = 2f;

		// Token: 0x0400C14A RID: 49482
		[TweakValue("Interface", 0f, 100f)]
		private static float ButtonOffset = 60f;

		// Token: 0x0400C14B RID: 49483
		private const int MaxLogLines = 300;

		// Token: 0x0400C14C RID: 49484
		public bool showAll;

		// Token: 0x0400C14D RID: 49485
		public bool showCombat = true;

		// Token: 0x0400C14E RID: 49486
		public bool showSocial = true;

		// Token: 0x0400C14F RID: 49487
		public LogEntry logSeek;

		// Token: 0x0400C150 RID: 49488
		public ITab_Pawn_Log_Utility.LogDrawData data = new ITab_Pawn_Log_Utility.LogDrawData();

		// Token: 0x0400C151 RID: 49489
		public List<ITab_Pawn_Log_Utility.LogLineDisplayable> cachedLogDisplay;

		// Token: 0x0400C152 RID: 49490
		public int cachedLogDisplayLastTick = -1;

		// Token: 0x0400C153 RID: 49491
		public int cachedLogPlayLastTick = -1;

		// Token: 0x0400C154 RID: 49492
		private Pawn cachedLogForPawn;

		// Token: 0x0400C155 RID: 49493
		private Vector2 scrollPosition;
	}
}
