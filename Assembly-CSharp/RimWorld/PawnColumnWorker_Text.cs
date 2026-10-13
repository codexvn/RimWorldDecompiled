using System;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02003485 RID: 13445
	public abstract class PawnColumnWorker_Text : PawnColumnWorker
	{
		// Token: 0x17003155 RID: 12629
		// (get) Token: 0x06012E82 RID: 77442 RVA: 0x005995CD File Offset: 0x005977CD
		protected virtual int Width
		{
			get
			{
				return this.def.width;
			}
		}

		// Token: 0x17003156 RID: 12630
		// (get) Token: 0x06012E83 RID: 77443 RVA: 0x00075162 File Offset: 0x00073362
		protected virtual TextAnchor Anchor
		{
			get
			{
				return TextAnchor.MiddleLeft;
			}
		}

		// Token: 0x06012E84 RID: 77444 RVA: 0x0059446E File Offset: 0x0059266E
		public override void DoHeader(Rect rect, PawnTable table)
		{
			base.DoHeader(rect, table);
			MouseoverSounds.DoRegion(rect);
		}

		// Token: 0x06012E85 RID: 77445 RVA: 0x005995DC File Offset: 0x005977DC
		public override void DoCell(Rect rect, Pawn pawn, PawnTable table)
		{
			Rect rect2 = new Rect(rect.x, rect.y, rect.width, Mathf.Min(rect.height, 30f));
			string textFor = this.GetTextFor(pawn);
			if (textFor != null)
			{
				Text.Font = GameFont.Small;
				Text.Anchor = this.Anchor;
				Text.WordWrap = false;
				Widgets.Label(rect2, textFor);
				Text.WordWrap = true;
				Text.Anchor = TextAnchor.UpperLeft;
				if (Mouse.IsOver(rect2))
				{
					string tip = this.GetTip(pawn);
					if (!tip.NullOrEmpty())
					{
						TooltipHandler.TipRegion(rect2, tip);
					}
				}
			}
		}

		// Token: 0x06012E86 RID: 77446 RVA: 0x0059966F File Offset: 0x0059786F
		public override int GetMinWidth(PawnTable table)
		{
			return Mathf.Max(base.GetMinWidth(table), this.Width);
		}

		// Token: 0x06012E87 RID: 77447 RVA: 0x00599683 File Offset: 0x00597883
		public override int Compare(Pawn a, Pawn b)
		{
			return PawnColumnWorker_Text.comparer.Compare(this.GetTextFor(a), this.GetTextFor(b));
		}

		// Token: 0x06012E88 RID: 77448
		protected abstract string GetTextFor(Pawn pawn);

		// Token: 0x06012E89 RID: 77449 RVA: 0x00002C42 File Offset: 0x00000E42
		protected virtual string GetTip(Pawn pawn)
		{
			return null;
		}

		// Token: 0x0400C3DB RID: 50139
		private static NumericStringComparer comparer = new NumericStringComparer();
	}
}
