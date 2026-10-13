using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Sound;
using Verse.Steam;

namespace RimWorld
{
	// Token: 0x02003454 RID: 13396
	public class PawnColumnWorker_AllowedArea : PawnColumnWorker
	{
		// Token: 0x06012D76 RID: 77174 RVA: 0x00595E70 File Offset: 0x00594070
		public override int GetMinWidth(PawnTable table)
		{
			return Mathf.Max(base.GetMinWidth(table), 200);
		}

		// Token: 0x06012D77 RID: 77175 RVA: 0x00595E83 File Offset: 0x00594083
		public override int GetOptimalWidth(PawnTable table)
		{
			return Mathf.Clamp(273, this.GetMinWidth(table), this.GetMaxWidth(table));
		}

		// Token: 0x06012D78 RID: 77176 RVA: 0x00595E9D File Offset: 0x0059409D
		public override int GetMinHeaderHeight(PawnTable table)
		{
			return Mathf.Max(base.GetMinHeaderHeight(table), 65);
		}

		// Token: 0x06012D79 RID: 77177 RVA: 0x00595EB0 File Offset: 0x005940B0
		public override void DoCell(Rect rect, Pawn pawn, PawnTable table)
		{
			if (pawn.Faction == Faction.OfPlayer)
			{
				if (pawn.IsMutant && !pawn.mutant.Def.respectsAllowedArea)
				{
					return;
				}
				if (pawn.RaceProps.IsMechanoid && pawn.GetOverseer() == null)
				{
					return;
				}
				if (pawn.playerSettings.SupportsAllowedAreas)
				{
					AreaAllowedGUI.DoAllowedAreaSelectors(rect, pawn);
					return;
				}
				if (AnimalPenUtility.NeedsToBeManagedByRope(pawn))
				{
					AnimalPenGUI.DoAllowedAreaMessage(rect, pawn);
					return;
				}
				if (pawn.RaceProps.Dryad)
				{
					Text.Anchor = TextAnchor.MiddleCenter;
					Text.Font = GameFont.Tiny;
					GUI.color = Color.gray;
					Widgets.Label(rect, "CannotAssignAllowedAreaToDryad".Translate());
					GUI.color = Color.white;
					Text.Font = GameFont.Small;
					Text.Anchor = TextAnchor.UpperLeft;
				}
			}
		}

		// Token: 0x06012D7A RID: 77178 RVA: 0x00595F6C File Offset: 0x0059416C
		public override void DoHeader(Rect rect, PawnTable table)
		{
			base.DoHeader(rect, table);
			if (Widgets.ButtonText(new Rect(rect.x, rect.y + (rect.height - 65f), Mathf.Min(rect.width, 360f), 32f), "ManageAreas".Translate(), true, true, true, null))
			{
				Find.WindowStack.Add(new Dialog_ManageAreas(Find.CurrentMap));
			}
		}

		// Token: 0x06012D7B RID: 77179 RVA: 0x00595FF0 File Offset: 0x005941F0
		public override int Compare(Pawn a, Pawn b)
		{
			return this.GetValueToCompare(a).CompareTo(this.GetValueToCompare(b));
		}

		// Token: 0x06012D7C RID: 77180 RVA: 0x00596014 File Offset: 0x00594214
		private int GetValueToCompare(Pawn pawn)
		{
			if (pawn.Faction != Faction.OfPlayer || !pawn.playerSettings.SupportsAllowedAreas)
			{
				return int.MinValue;
			}
			Area areaRestrictionInPawnCurrentMap = pawn.playerSettings.AreaRestrictionInPawnCurrentMap;
			if (areaRestrictionInPawnCurrentMap == null)
			{
				return -2147483647;
			}
			return areaRestrictionInPawnCurrentMap.ID;
		}

		// Token: 0x06012D7D RID: 77181 RVA: 0x0059605C File Offset: 0x0059425C
		protected override void HeaderClicked(Rect headerRect, PawnTable table)
		{
			base.HeaderClicked(headerRect, table);
			if (Event.current.shift && Find.CurrentMap != null)
			{
				List<Pawn> pawnsListForReading = table.PawnsListForReading;
				for (int i = 0; i < pawnsListForReading.Count; i++)
				{
					if (pawnsListForReading[i].Faction == Faction.OfPlayer && pawnsListForReading[i].playerSettings.SupportsAllowedAreas)
					{
						if (Event.current.button == 0)
						{
							pawnsListForReading[i].playerSettings.AreaRestrictionInPawnCurrentMap = Find.CurrentMap.areaManager.Home;
						}
						else if (Event.current.button == 1)
						{
							pawnsListForReading[i].playerSettings.AreaRestrictionInPawnCurrentMap = null;
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

		// Token: 0x06012D7E RID: 77182 RVA: 0x00596148 File Offset: 0x00594348
		protected override string GetHeaderTip(PawnTable table)
		{
			string text = base.GetHeaderTip(table);
			if (!SteamDeck.IsSteamDeckInNonKeyboardMode)
			{
				text += "\n" + "AllowedAreaShiftClickTip".Translate();
			}
			return text;
		}

		// Token: 0x0400C36D RID: 50029
		private const int TopAreaHeight = 65;

		// Token: 0x0400C36E RID: 50030
		private const int ManageAreasButtonHeight = 32;
	}
}
