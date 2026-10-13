using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200343E RID: 13374
	public class PawnColumnWorker_Guest : PawnColumnWorker_Icon
	{
		// Token: 0x06012D06 RID: 77062 RVA: 0x00595063 File Offset: 0x00593263
		protected override Texture2D GetIconFor(Pawn pawn)
		{
			if (pawn == null)
			{
				return null;
			}
			return pawn.guest.GetIcon();
		}

		// Token: 0x06012D07 RID: 77063 RVA: 0x00595078 File Offset: 0x00593278
		protected override string GetIconTip(Pawn pawn)
		{
			string str = (pawn != null) ? pawn.guest.GetLabel() : null;
			if (!str.NullOrEmpty())
			{
				return str.CapitalizeFirst();
			}
			return null;
		}
	}
}
