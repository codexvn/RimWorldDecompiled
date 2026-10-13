using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02003426 RID: 13350
	public class MainTabWindow_Work : MainTabWindow_PawnTable
	{
		// Token: 0x1700312F RID: 12591
		// (get) Token: 0x06012C87 RID: 76935 RVA: 0x00593AE8 File Offset: 0x00591CE8
		protected override PawnTableDef PawnTableDef
		{
			get
			{
				return PawnTableDefOf.Work;
			}
		}

		// Token: 0x17003130 RID: 12592
		// (get) Token: 0x06012C88 RID: 76936 RVA: 0x00354414 File Offset: 0x00352614
		protected override float ExtraTopSpace
		{
			get
			{
				return 40f;
			}
		}

		// Token: 0x17003131 RID: 12593
		// (get) Token: 0x06012C89 RID: 76937 RVA: 0x00593AEF File Offset: 0x00591CEF
		protected override IEnumerable<Pawn> Pawns
		{
			get
			{
				return from pawn in base.Pawns
				where !pawn.DevelopmentalStage.Baby()
				select pawn;
			}
		}

		// Token: 0x06012C8A RID: 76938 RVA: 0x00593B1C File Offset: 0x00591D1C
		public override void DoWindowContents(Rect rect)
		{
			base.DoWindowContents(rect);
			if (Event.current.type == EventType.Layout)
			{
				return;
			}
			this.DoManualPrioritiesCheckbox();
			GUI.color = new Color(1f, 1f, 1f, 0.5f);
			Text.Anchor = TextAnchor.UpperCenter;
			Text.Font = GameFont.Tiny;
			Widgets.Label(new Rect(370f, rect.y + 5f, 160f, 30f), "<= " + "HigherPriority".Translate());
			Widgets.Label(new Rect(630f, rect.y + 5f, 160f, 30f), "LowerPriority".Translate() + " =>");
			GUI.color = Color.white;
			Text.Font = GameFont.Small;
			Text.Anchor = TextAnchor.UpperLeft;
		}

		// Token: 0x06012C8B RID: 76939 RVA: 0x00593BF8 File Offset: 0x00591DF8
		private void DoManualPrioritiesCheckbox()
		{
			Text.Font = GameFont.Small;
			GUI.color = Color.white;
			Text.Anchor = TextAnchor.UpperLeft;
			Rect rect = new Rect(5f, 5f, 140f, 30f);
			bool useWorkPriorities = Current.Game.playSettings.useWorkPriorities;
			Widgets.CheckboxLabeled(rect, "ManualPriorities".Translate(), ref Current.Game.playSettings.useWorkPriorities, false, null, null, false, false);
			if (useWorkPriorities != Current.Game.playSettings.useWorkPriorities)
			{
				foreach (Pawn pawn in PawnsFinder.AllMapsWorldAndTemporary_Alive)
				{
					if (pawn.Faction == Faction.OfPlayer && pawn.workSettings != null)
					{
						pawn.workSettings.Notify_UseWorkPrioritiesChanged();
					}
				}
			}
			if (Current.Game.playSettings.useWorkPriorities)
			{
				using (new TextBlock(new Color(1f, 1f, 1f, 0.5f)))
				{
					Widgets.Label(new Rect(rect.x, rect.yMax - 6f, rect.width, 60f), "PriorityOneDoneFirst".Translate());
				}
			}
			if (!Current.Game.playSettings.useWorkPriorities)
			{
				UIHighlighter.HighlightOpportunity(rect, "ManualPriorities-Off");
			}
		}

		// Token: 0x0400C338 RID: 49976
		private const int SpaceBetweenPriorityArrowsAndWorkLabels = 40;
	}
}
