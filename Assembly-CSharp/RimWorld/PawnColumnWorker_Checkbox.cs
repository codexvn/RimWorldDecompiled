using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Sound;
using Verse.Steam;

namespace RimWorld
{
	// Token: 0x0200342B RID: 13355
	public abstract class PawnColumnWorker_Checkbox : PawnColumnWorker
	{
		// Token: 0x06012CA3 RID: 76963 RVA: 0x0059446E File Offset: 0x0059266E
		public override void DoHeader(Rect rect, PawnTable table)
		{
			base.DoHeader(rect, table);
			MouseoverSounds.DoRegion(rect);
		}

		// Token: 0x06012CA4 RID: 76964 RVA: 0x00594480 File Offset: 0x00592680
		public override void DoCell(Rect rect, Pawn pawn, PawnTable table)
		{
			if (!this.HasCheckbox(pawn))
			{
				return;
			}
			int num = (int)((rect.width - 24f) / 2f);
			int num2 = Mathf.Max(3, 0);
			Vector2 vector = new Vector2(rect.x + (float)num, rect.y + (float)num2);
			Rect rect2 = new Rect(vector.x, vector.y, 24f, 24f);
			bool value = this.GetValue(pawn);
			bool flag = value;
			Widgets.Checkbox(vector, ref value, 24f, false, this.def.paintable, null, null);
			if (Mouse.IsOver(rect2))
			{
				string tip = this.GetTip(pawn);
				if (!tip.NullOrEmpty())
				{
					TooltipHandler.TipRegion(rect2, tip);
				}
			}
			if (value != flag)
			{
				this.SetValue(pawn, value, table);
			}
		}

		// Token: 0x06012CA5 RID: 76965 RVA: 0x0059454B File Offset: 0x0059274B
		public override int GetMinWidth(PawnTable table)
		{
			return Mathf.Max(base.GetMinWidth(table), 28);
		}

		// Token: 0x06012CA6 RID: 76966 RVA: 0x0059455B File Offset: 0x0059275B
		public override int GetMaxWidth(PawnTable table)
		{
			return Mathf.Min(base.GetMaxWidth(table), this.GetMinWidth(table));
		}

		// Token: 0x06012CA7 RID: 76967 RVA: 0x00594570 File Offset: 0x00592770
		public override int GetMinCellHeight(Pawn pawn)
		{
			return Mathf.Max(base.GetMinCellHeight(pawn), 24);
		}

		// Token: 0x06012CA8 RID: 76968 RVA: 0x00594580 File Offset: 0x00592780
		public override int Compare(Pawn a, Pawn b)
		{
			return this.GetValueToCompare(a).CompareTo(this.GetValueToCompare(b));
		}

		// Token: 0x06012CA9 RID: 76969 RVA: 0x005945A3 File Offset: 0x005927A3
		private int GetValueToCompare(Pawn pawn)
		{
			if (!this.HasCheckbox(pawn))
			{
				return 0;
			}
			if (!this.GetValue(pawn))
			{
				return 1;
			}
			return 2;
		}

		// Token: 0x06012CAA RID: 76970 RVA: 0x00002C42 File Offset: 0x00000E42
		protected virtual string GetTip(Pawn pawn)
		{
			return null;
		}

		// Token: 0x06012CAB RID: 76971 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected virtual bool HasCheckbox(Pawn pawn)
		{
			return true;
		}

		// Token: 0x06012CAC RID: 76972
		protected abstract bool GetValue(Pawn pawn);

		// Token: 0x06012CAD RID: 76973
		protected abstract void SetValue(Pawn pawn, bool value, PawnTable table);

		// Token: 0x06012CAE RID: 76974 RVA: 0x005945BC File Offset: 0x005927BC
		protected override void HeaderClicked(Rect headerRect, PawnTable table)
		{
			base.HeaderClicked(headerRect, table);
			if (Event.current.shift)
			{
				List<Pawn> pawnsListForReading = table.PawnsListForReading;
				for (int i = 0; i < pawnsListForReading.Count; i++)
				{
					if (this.HasCheckbox(pawnsListForReading[i]))
					{
						if (Event.current.button == 0)
						{
							if (!this.GetValue(pawnsListForReading[i]))
							{
								this.SetValue(pawnsListForReading[i], true, table);
							}
						}
						else if (Event.current.button == 1 && this.GetValue(pawnsListForReading[i]))
						{
							this.SetValue(pawnsListForReading[i], false, table);
						}
					}
				}
				if (Event.current.button == 0)
				{
					SoundDefOf.Checkbox_TurnedOn.PlayOneShotOnCamera(null);
					return;
				}
				if (Event.current.button == 1)
				{
					SoundDefOf.Checkbox_TurnedOff.PlayOneShotOnCamera(null);
				}
			}
		}

		// Token: 0x06012CAF RID: 76975 RVA: 0x00594690 File Offset: 0x00592890
		protected override string GetHeaderTip(PawnTable table)
		{
			string text = base.GetHeaderTip(table);
			if (!SteamDeck.IsSteamDeckInNonKeyboardMode)
			{
				text += "\n" + "CheckboxShiftClickTip".Translate();
			}
			return text;
		}

		// Token: 0x0400C346 RID: 49990
		public const int HorizontalPadding = 2;
	}
}
