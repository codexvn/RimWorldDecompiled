using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020033F3 RID: 13299
	public class MainTabWindow_Animals : MainTabWindow_PawnTable
	{
		// Token: 0x170030FB RID: 12539
		// (get) Token: 0x06012B36 RID: 76598 RVA: 0x00589526 File Offset: 0x00587726
		protected override PawnTableDef PawnTableDef
		{
			get
			{
				return PawnTableDefOf.Animals;
			}
		}

		// Token: 0x170030FC RID: 12540
		// (get) Token: 0x06012B37 RID: 76599 RVA: 0x0058952D File Offset: 0x0058772D
		protected override IEnumerable<Pawn> Pawns
		{
			get
			{
				return Find.CurrentMap.mapPawns.ColonyAnimals;
			}
		}

		// Token: 0x06012B38 RID: 76600 RVA: 0x00589540 File Offset: 0x00587740
		public override void DoWindowContents(Rect rect)
		{
			base.DoWindowContents(rect);
			if (Widgets.ButtonText(new Rect(rect.x, rect.y, Mathf.Min(rect.width, 260f), 32f), "ManageAutoSlaughter".Translate(), true, true, true, null))
			{
				Find.WindowStack.Add(new Dialog_AutoSlaughter(Find.CurrentMap));
			}
		}
	}
}
