using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02003130 RID: 12592
	public class Dialog_ChooseColonistsForIdeo : Window
	{
		// Token: 0x17002E07 RID: 11783
		// (get) Token: 0x06011B3E RID: 72510 RVA: 0x005181D8 File Offset: 0x005163D8
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(500f, 600f);
			}
		}

		// Token: 0x06011B3F RID: 72511 RVA: 0x005181EC File Offset: 0x005163EC
		public Dialog_ChooseColonistsForIdeo(Ideo ideo, IEnumerable<Pawn> pawns, Func<Pawn, bool> canChangeIdeo, Func<Pawn, Ideo> originalIdeo, Func<Pawn, Ideo> pawnIdeoGetter = null, Action<Pawn, Ideo> pawnIdeoSetter = null) : base(null)
		{
			this.forcePause = true;
			this.closeOnCancel = false;
			this.absorbInputAroundWindow = true;
			this.ideo = ideo;
			this.pawns.AddRange(pawns);
			this.canChangeIdeo = canChangeIdeo;
			this.originalIdeo = originalIdeo;
			this.pawnIdeoGetter = pawnIdeoGetter;
			this.pawnIdeoSetter = pawnIdeoSetter;
		}

		// Token: 0x06011B40 RID: 72512 RVA: 0x00518268 File Offset: 0x00516468
		public override void DoWindowContents(Rect inRect)
		{
			Text.Font = GameFont.Medium;
			Text.Anchor = TextAnchor.MiddleLeft;
			Widgets.Label(new Rect(0f, 0f, inRect.width, 40f), "ChooseColonistsForIdeoTitle".Translate());
			Text.Font = GameFont.Small;
			Text.Anchor = TextAnchor.UpperLeft;
			Widgets.Label(new Rect(0f, 40f, inRect.width, 40f), "ChooseColonistsForIdeoDesc".Translate());
			inRect.yMin += 112f;
			Listing_Standard listing_Standard = new Listing_Standard();
			listing_Standard.ColumnWidth = inRect.width;
			listing_Standard.Begin(inRect);
			for (int i = 0; i < this.pawns.Count; i++)
			{
				Pawn pawn = this.pawns[i];
				Rect rect = listing_Standard.GetRect(24f, 1f);
				if (i % 2 == 0)
				{
					Widgets.DrawLightHighlight(rect);
				}
				Widgets.BeginGroup(rect);
				WidgetRow widgetRow = new WidgetRow(0f, 0f, UIDirection.RightThenUp, 99999f, 4f);
				RenderTexture tex = PortraitsCache.Get(pawn, new Vector2(24f, 24f), Rot4.South, default(Vector3), 1f, true, true, true, true, null, null, false, null);
				widgetRow.Icon(tex, null, 0f);
				Ideo ideo = (this.pawnIdeoGetter != null) ? this.pawnIdeoGetter(pawn) : pawn.Ideo;
				GUI.color = ideo.Color;
				widgetRow.Icon(ideo.Icon, null, 0f);
				GUI.color = Color.white;
				widgetRow.Label(pawn.LabelShortCap, -1f, null, -1f);
				float width = listing_Standard.ColumnWidth - widgetRow.FinalX - 150f;
				widgetRow.Gap(width);
				if (this.canChangeIdeo(pawn))
				{
					if (ideo == this.ideo)
					{
						if (widgetRow.ButtonText("RevertToPreviousIdeoligion".Translate(), null, true, true, true, null))
						{
							if (this.pawnIdeoSetter != null)
							{
								this.pawnIdeoSetter(pawn, this.originalIdeo(pawn));
							}
							else
							{
								pawn.ideo.SetIdeo(this.originalIdeo(pawn));
							}
						}
					}
					else if (widgetRow.ButtonText("ConvertToPlayerIdeoligion".Translate(), null, true, true, true, null))
					{
						if (this.pawnIdeoSetter != null)
						{
							this.pawnIdeoSetter(pawn, this.ideo);
						}
						else
						{
							pawn.ideo.SetIdeo(this.ideo);
						}
					}
				}
				else
				{
					widgetRow.Label("ExistingFollowerOfPlayerIdeoligion".Translate(), -1f, null, -1f);
				}
				Widgets.EndGroup();
				listing_Standard.Gap(6f);
			}
			listing_Standard.End();
			if (Widgets.ButtonText(new Rect(inRect.width / 2f - this.BottomButtonSize.x / 2f, inRect.yMax - 55f, this.BottomButtonSize.x, this.BottomButtonSize.y), "Close".Translate(), true, true, true, null))
			{
				this.Close(true);
			}
		}

		// Token: 0x0400B64F RID: 46671
		private Ideo ideo;

		// Token: 0x0400B650 RID: 46672
		private List<Pawn> pawns = new List<Pawn>();

		// Token: 0x0400B651 RID: 46673
		private Func<Pawn, bool> canChangeIdeo;

		// Token: 0x0400B652 RID: 46674
		private Func<Pawn, Ideo> originalIdeo;

		// Token: 0x0400B653 RID: 46675
		private Func<Pawn, Ideo> pawnIdeoGetter;

		// Token: 0x0400B654 RID: 46676
		private Action<Pawn, Ideo> pawnIdeoSetter;

		// Token: 0x0400B655 RID: 46677
		private const float HeaderLabelHeight = 40f;

		// Token: 0x0400B656 RID: 46678
		private readonly Vector2 BottomButtonSize = new Vector2(160f, 40f);

		// Token: 0x0400B657 RID: 46679
		private const float BottomAreaHeight = 55f;

		// Token: 0x0400B658 RID: 46680
		private const float RowButtonWidth = 150f;
	}
}
