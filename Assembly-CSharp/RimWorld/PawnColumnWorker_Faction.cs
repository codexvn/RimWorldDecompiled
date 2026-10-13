using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200343C RID: 13372
	public class PawnColumnWorker_Faction : PawnColumnWorker_Icon
	{
		// Token: 0x06012CFE RID: 77054 RVA: 0x00594F68 File Offset: 0x00593168
		protected override Texture2D GetIconFor(Pawn pawn)
		{
			Faction homeFaction = pawn.HomeFaction;
			if (homeFaction != null && homeFaction != Faction.OfPlayer)
			{
				return homeFaction.def.FactionIcon;
			}
			return null;
		}

		// Token: 0x06012CFF RID: 77055 RVA: 0x00594F94 File Offset: 0x00593194
		protected override Color GetIconColor(Pawn pawn)
		{
			Faction homeFaction = pawn.HomeFaction;
			if (homeFaction != null && homeFaction != Faction.OfPlayer)
			{
				return homeFaction.Color;
			}
			return Color.white;
		}

		// Token: 0x06012D00 RID: 77056 RVA: 0x00594FC0 File Offset: 0x005931C0
		protected override string GetIconTip(Pawn pawn)
		{
			Faction homeFaction = pawn.HomeFaction;
			string text = (homeFaction != null) ? homeFaction.Name : null;
			if (!text.NullOrEmpty())
			{
				return "PawnFactionInfo".Translate(text, pawn);
			}
			return null;
		}

		// Token: 0x06012D01 RID: 77057 RVA: 0x00595008 File Offset: 0x00593208
		protected override void ClickedIcon(Pawn pawn)
		{
			Faction homeFaction = pawn.HomeFaction;
			if (homeFaction != null)
			{
				Find.MainTabsRoot.SetCurrentTab(MainButtonDefOf.Factions, true);
				((MainTabWindow_Factions)Find.MainTabsRoot.OpenTab.TabWindow).ScrollToFaction(homeFaction);
			}
		}
	}
}
