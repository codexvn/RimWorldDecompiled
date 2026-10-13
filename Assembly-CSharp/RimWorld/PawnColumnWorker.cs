using System;
using System.Text;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02001AA7 RID: 6823
	[StaticConstructorOnStartup]
	public abstract class PawnColumnWorker
	{
		// Token: 0x1700176C RID: 5996
		// (get) Token: 0x060098C5 RID: 39109 RVA: 0x000440FD File Offset: 0x000422FD
		protected virtual Color DefaultHeaderColor
		{
			get
			{
				return Color.white;
			}
		}

		// Token: 0x1700176D RID: 5997
		// (get) Token: 0x060098C6 RID: 39110 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected virtual GameFont DefaultHeaderFont
		{
			get
			{
				return GameFont.Small;
			}
		}

		// Token: 0x1700176E RID: 5998
		// (get) Token: 0x060098C7 RID: 39111 RVA: 0x002A994D File Offset: 0x002A7B4D
		protected virtual TextAnchor DefaultHeaderAlignment
		{
			get
			{
				return TextAnchor.LowerCenter;
			}
		}

		// Token: 0x1700176F RID: 5999
		// (get) Token: 0x060098C8 RID: 39112 RVA: 0x000028E7 File Offset: 0x00000AE7
		public virtual bool VisibleCurrently
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060098C9 RID: 39113 RVA: 0x002C8B74 File Offset: 0x002C6D74
		public virtual void DoHeader(Rect rect, PawnTable table)
		{
			if (!this.def.label.NullOrEmpty())
			{
				Text.Font = this.DefaultHeaderFont;
				GUI.color = this.DefaultHeaderColor;
				Text.Anchor = this.DefaultHeaderAlignment;
				Rect rect2 = rect;
				rect2.xMin += this.GetHeaderOffsetX(rect);
				Widgets.Label(rect2, this.def.LabelCap.Resolve().Truncate(rect.width, null));
				Text.Anchor = TextAnchor.UpperLeft;
				GUI.color = Color.white;
				Text.Font = GameFont.Small;
			}
			else if (this.def.HeaderIcon != null)
			{
				Vector2 headerIconSize = this.def.HeaderIconSize;
				int num = (int)((rect.width - headerIconSize.x) / 2f);
				GUI.DrawTexture(new Rect(rect.x + (float)num, rect.yMax - headerIconSize.y, headerIconSize.x, headerIconSize.y).ContractedBy(2f), this.def.HeaderIcon);
			}
			if (table.SortingBy == this.def)
			{
				Texture2D texture2D = table.SortingDescending ? PawnColumnWorker.SortingDescendingIcon : PawnColumnWorker.SortingIcon;
				GUI.DrawTexture(new Rect(rect.xMax - (float)texture2D.width - 1f, rect.yMax - (float)texture2D.height - 1f, (float)texture2D.width, (float)texture2D.height), texture2D);
			}
			if (this.def.HeaderInteractable)
			{
				Rect interactableHeaderRect = this.GetInteractableHeaderRect(rect, table);
				if (Mouse.IsOver(interactableHeaderRect))
				{
					Widgets.DrawHighlight(interactableHeaderRect);
					string headerTip = this.GetHeaderTip(table);
					if (!headerTip.NullOrEmpty())
					{
						TooltipHandler.TipRegion(interactableHeaderRect, headerTip);
					}
				}
				if (Widgets.ButtonInvisible(interactableHeaderRect, true))
				{
					this.HeaderClicked(rect, table);
				}
			}
		}

		// Token: 0x060098CA RID: 39114 RVA: 0x00006AAC File Offset: 0x00004CAC
		protected virtual float GetHeaderOffsetX(Rect rect)
		{
			return 0f;
		}

		// Token: 0x060098CB RID: 39115
		public abstract void DoCell(Rect rect, Pawn pawn, PawnTable table);

		// Token: 0x060098CC RID: 39116 RVA: 0x00002501 File Offset: 0x00000701
		public virtual bool CanGroupWith(Pawn pawn, Pawn other)
		{
			return false;
		}

		// Token: 0x060098CD RID: 39117 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void Recache()
		{
		}

		// Token: 0x060098CE RID: 39118 RVA: 0x002C8D48 File Offset: 0x002C6F48
		public virtual int GetMinWidth(PawnTable table)
		{
			if (!this.def.label.NullOrEmpty())
			{
				Text.Font = this.DefaultHeaderFont;
				int result = Mathf.CeilToInt(Text.CalcSize(this.def.LabelCap).x);
				Text.Font = GameFont.Small;
				return result;
			}
			if (this.def.HeaderIcon != null)
			{
				return Mathf.CeilToInt(this.def.HeaderIconSize.x);
			}
			return 1;
		}

		// Token: 0x060098CF RID: 39119 RVA: 0x002C8DC2 File Offset: 0x002C6FC2
		public virtual int GetMaxWidth(PawnTable table)
		{
			return 1000000;
		}

		// Token: 0x060098D0 RID: 39120 RVA: 0x002C8DC9 File Offset: 0x002C6FC9
		public virtual int GetOptimalWidth(PawnTable table)
		{
			return this.GetMinWidth(table);
		}

		// Token: 0x060098D1 RID: 39121 RVA: 0x002C8DD2 File Offset: 0x002C6FD2
		public virtual int GetMinCellHeight(Pawn pawn)
		{
			return 30;
		}

		// Token: 0x060098D2 RID: 39122 RVA: 0x002C8DD8 File Offset: 0x002C6FD8
		public virtual int GetMinHeaderHeight(PawnTable table)
		{
			if (!this.def.label.NullOrEmpty())
			{
				Text.Font = this.DefaultHeaderFont;
				int result = Mathf.CeilToInt(Text.CalcSize(this.def.LabelCap).y);
				Text.Font = GameFont.Small;
				return result;
			}
			if (this.def.HeaderIcon != null)
			{
				return Mathf.CeilToInt(this.def.HeaderIconSize.y);
			}
			return 0;
		}

		// Token: 0x060098D3 RID: 39123 RVA: 0x00002501 File Offset: 0x00000701
		public virtual int Compare(Pawn a, Pawn b)
		{
			return 0;
		}

		// Token: 0x060098D4 RID: 39124 RVA: 0x002C8E54 File Offset: 0x002C7054
		protected virtual Rect GetInteractableHeaderRect(Rect headerRect, PawnTable table)
		{
			float num = Mathf.Min(25f, headerRect.height);
			return new Rect(headerRect.x, headerRect.yMax - num, headerRect.width, num);
		}

		// Token: 0x060098D5 RID: 39125 RVA: 0x002C8E90 File Offset: 0x002C7090
		protected virtual void HeaderClicked(Rect headerRect, PawnTable table)
		{
			if (this.def.sortable && !Event.current.shift)
			{
				if (Event.current.button == 0)
				{
					if (table.SortingBy != this.def)
					{
						table.SortBy(this.def, true);
						SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
						return;
					}
					if (table.SortingDescending)
					{
						table.SortBy(this.def, false);
						SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
						return;
					}
					table.SortBy(null, false);
					SoundDefOf.Tick_Low.PlayOneShotOnCamera(null);
					return;
				}
				else if (Event.current.button == 1)
				{
					if (table.SortingBy != this.def)
					{
						table.SortBy(this.def, false);
						SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
						return;
					}
					if (table.SortingDescending)
					{
						table.SortBy(null, false);
						SoundDefOf.Tick_Low.PlayOneShotOnCamera(null);
						return;
					}
					table.SortBy(this.def, true);
					SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
				}
			}
		}

		// Token: 0x060098D6 RID: 39126 RVA: 0x002C8F8C File Offset: 0x002C718C
		protected virtual string GetHeaderTip(PawnTable table)
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (!this.def.headerTip.NullOrEmpty())
			{
				stringBuilder.Append(this.def.headerTip);
			}
			if (this.def.sortable)
			{
				if (stringBuilder.Length != 0)
				{
					stringBuilder.AppendLine();
					stringBuilder.AppendLine();
				}
				stringBuilder.Append("ClickToSortByThisColumn".Translate());
			}
			return stringBuilder.ToString();
		}

		// Token: 0x040067AD RID: 26541
		public PawnColumnDef def;

		// Token: 0x040067AE RID: 26542
		protected const int DefaultCellHeight = 30;

		// Token: 0x040067AF RID: 26543
		private static readonly Texture2D SortingIcon = ContentFinder<Texture2D>.Get("UI/Icons/Sorting", true);

		// Token: 0x040067B0 RID: 26544
		private static readonly Texture2D SortingDescendingIcon = ContentFinder<Texture2D>.Get("UI/Icons/SortingDescending", true);

		// Token: 0x040067B1 RID: 26545
		private const int IconMargin = 2;
	}
}
