using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02003440 RID: 13376
	public class PawnColumnWorker_Ideo : PawnColumnWorker_Icon
	{
		// Token: 0x17003140 RID: 12608
		// (get) Token: 0x06012D18 RID: 77080 RVA: 0x00595254 File Offset: 0x00593454
		public override bool VisibleCurrently
		{
			get
			{
				return ModsConfig.IdeologyActive && !Find.IdeoManager.classicMode;
			}
		}

		// Token: 0x06012D19 RID: 77081 RVA: 0x0059526C File Offset: 0x0059346C
		protected override Texture2D GetIconFor(Pawn pawn)
		{
			if (Find.IdeoManager.classicMode)
			{
				return null;
			}
			Ideo ideo = pawn.Ideo;
			if (ideo == null)
			{
				return null;
			}
			return ideo.Icon;
		}

		// Token: 0x06012D1A RID: 77082 RVA: 0x0059528D File Offset: 0x0059348D
		protected override Color GetIconColor(Pawn pawn)
		{
			if (pawn.Ideo == null)
			{
				return Color.white;
			}
			return pawn.Ideo.Color;
		}

		// Token: 0x06012D1B RID: 77083 RVA: 0x005952A8 File Offset: 0x005934A8
		protected override string GetIconTip(Pawn pawn)
		{
			if (pawn.Ideo != null)
			{
				return pawn.Ideo.name + "\n\n" + "ClickForMoreInfo".Translate();
			}
			return null;
		}

		// Token: 0x06012D1C RID: 77084 RVA: 0x005952DD File Offset: 0x005934DD
		protected override void ClickedIcon(Pawn pawn)
		{
			if (pawn.Ideo != null)
			{
				Find.MainTabsRoot.SetCurrentTab(MainButtonDefOf.Ideos, true);
				IdeoUIUtility.OpenIdeoInfo(pawn.Ideo);
			}
		}
	}
}
