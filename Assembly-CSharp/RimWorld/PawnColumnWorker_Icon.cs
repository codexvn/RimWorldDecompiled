using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200343F RID: 13375
	public abstract class PawnColumnWorker_Icon : PawnColumnWorker
	{
		// Token: 0x1700313E RID: 12606
		// (get) Token: 0x06012D09 RID: 77065 RVA: 0x005950A7 File Offset: 0x005932A7
		protected virtual int Width
		{
			get
			{
				return 26;
			}
		}

		// Token: 0x1700313F RID: 12607
		// (get) Token: 0x06012D0A RID: 77066 RVA: 0x000E96BB File Offset: 0x000E78BB
		protected virtual int Padding
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x06012D0B RID: 77067 RVA: 0x005950AC File Offset: 0x005932AC
		public override void DoCell(Rect rect, Pawn pawn, PawnTable table)
		{
			Texture2D iconFor = this.GetIconFor(pawn);
			if (iconFor != null)
			{
				Vector2 iconSize = this.GetIconSize(pawn);
				int num = (int)((rect.width - iconSize.x) / 2f);
				int num2 = Mathf.Max((int)((30f - iconSize.y) / 2f), 0);
				Rect rect2 = new Rect(rect.x + (float)num, rect.y + (float)num2, iconSize.x, iconSize.y);
				GUI.color = this.GetIconColor(pawn);
				GUI.DrawTexture(rect2.ContractedBy((float)this.Padding), iconFor);
				GUI.color = Color.white;
				if (Mouse.IsOver(rect2))
				{
					string iconTip = this.GetIconTip(pawn);
					if (!iconTip.NullOrEmpty())
					{
						TooltipHandler.TipRegion(rect2, iconTip);
					}
				}
				if (Widgets.ButtonInvisible(rect2, false))
				{
					this.ClickedIcon(pawn);
				}
				if (Mouse.IsOver(rect2) && Input.GetMouseButton(0))
				{
					this.PaintedIcon(pawn);
				}
			}
		}

		// Token: 0x06012D0C RID: 77068 RVA: 0x005951A8 File Offset: 0x005933A8
		public override int GetMinWidth(PawnTable table)
		{
			return Mathf.Max(base.GetMinWidth(table), this.Width);
		}

		// Token: 0x06012D0D RID: 77069 RVA: 0x0059455B File Offset: 0x0059275B
		public override int GetMaxWidth(PawnTable table)
		{
			return Mathf.Min(base.GetMaxWidth(table), this.GetMinWidth(table));
		}

		// Token: 0x06012D0E RID: 77070 RVA: 0x005951BC File Offset: 0x005933BC
		public override int GetMinCellHeight(Pawn pawn)
		{
			return Mathf.Max(base.GetMinCellHeight(pawn), Mathf.CeilToInt(this.GetIconSize(pawn).y));
		}

		// Token: 0x06012D0F RID: 77071 RVA: 0x005951DC File Offset: 0x005933DC
		public override int Compare(Pawn a, Pawn b)
		{
			return this.GetValueToCompare(a).CompareTo(this.GetValueToCompare(b));
		}

		// Token: 0x06012D10 RID: 77072 RVA: 0x00595200 File Offset: 0x00593400
		private int GetValueToCompare(Pawn pawn)
		{
			Texture2D iconFor = this.GetIconFor(pawn);
			if (!(iconFor != null))
			{
				return int.MinValue;
			}
			return iconFor.GetInstanceID();
		}

		// Token: 0x06012D11 RID: 77073
		protected abstract Texture2D GetIconFor(Pawn pawn);

		// Token: 0x06012D12 RID: 77074 RVA: 0x00002C42 File Offset: 0x00000E42
		protected virtual string GetIconTip(Pawn pawn)
		{
			return null;
		}

		// Token: 0x06012D13 RID: 77075 RVA: 0x000440FD File Offset: 0x000422FD
		protected virtual Color GetIconColor(Pawn pawn)
		{
			return Color.white;
		}

		// Token: 0x06012D14 RID: 77076 RVA: 0x000026EA File Offset: 0x000008EA
		protected virtual void ClickedIcon(Pawn pawn)
		{
		}

		// Token: 0x06012D15 RID: 77077 RVA: 0x000026EA File Offset: 0x000008EA
		protected virtual void PaintedIcon(Pawn pawn)
		{
		}

		// Token: 0x06012D16 RID: 77078 RVA: 0x0059522A File Offset: 0x0059342A
		protected virtual Vector2 GetIconSize(Pawn pawn)
		{
			if (this.GetIconFor(pawn) == null)
			{
				return Vector2.zero;
			}
			return new Vector2((float)this.Width, (float)this.Width);
		}
	}
}
