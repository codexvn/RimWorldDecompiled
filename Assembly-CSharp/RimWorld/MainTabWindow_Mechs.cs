using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02003401 RID: 13313
	public class MainTabWindow_Mechs : MainTabWindow_PawnTable
	{
		// Token: 0x17003113 RID: 12563
		// (get) Token: 0x06012B93 RID: 76691 RVA: 0x0058B935 File Offset: 0x00589B35
		protected override PawnTableDef PawnTableDef
		{
			get
			{
				return PawnTableDefOf.Mechs;
			}
		}

		// Token: 0x17003114 RID: 12564
		// (get) Token: 0x06012B94 RID: 76692 RVA: 0x0058B93C File Offset: 0x00589B3C
		protected override IEnumerable<Pawn> Pawns
		{
			get
			{
				return from p in Find.CurrentMap.mapPawns.PawnsInFaction(Faction.OfPlayer)
				where p.RaceProps.IsMechanoid && p.OverseerSubject != null
				select p;
			}
		}

		// Token: 0x06012B95 RID: 76693 RVA: 0x0058B978 File Offset: 0x00589B78
		public override void PostOpen()
		{
			base.PostOpen();
			this.cachedColors = (from c in DefDatabase<ColorDef>.AllDefsListForReading
			select c.color).Concat(from f in Find.FactionManager.AllFactionsVisible
			select f.Color).Distinct<Color>().ToList<Color>();
			this.cachedColors.SortByColor((Color c) => c);
		}

		// Token: 0x06012B96 RID: 76694 RVA: 0x0058BA24 File Offset: 0x00589C24
		public override void DoWindowContents(Rect rect)
		{
			if (!ModLister.BiotechInstalled)
			{
				return;
			}
			base.DoWindowContents(rect);
			Rect rect2 = new Rect(rect.x, rect.y, 240f, 32f);
			Text.Font = GameFont.Small;
			if (Widgets.ButtonText(rect2, "ChooseMechColor".Translate(), true, true, true, null))
			{
				Find.WindowStack.Add(new Dialog_ChooseColor("ChooseMechAccentColor".Translate(), Find.FactionManager.OfPlayer.AllegianceColor, this.cachedColors, delegate(Color color)
				{
					Find.FactionManager.OfPlayer.AllegianceColor = color;
					foreach (Pawn dirty in MechanitorUtility.MechsInPlayerFaction())
					{
						PortraitsCache.SetDirty(dirty);
					}
				}));
			}
		}

		// Token: 0x0400C271 RID: 49777
		private List<Color> cachedColors = new List<Color>();

		// Token: 0x0400C272 RID: 49778
		private const int SetColorButtonHeight = 32;

		// Token: 0x0400C273 RID: 49779
		private const int SetColorButtonWidth = 240;
	}
}
