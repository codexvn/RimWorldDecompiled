using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200343D RID: 13373
	public class PawnColumnWorker_Gender : PawnColumnWorker_Icon
	{
		// Token: 0x06012D03 RID: 77059 RVA: 0x00595049 File Offset: 0x00593249
		protected override Texture2D GetIconFor(Pawn pawn)
		{
			return pawn.gender.GetIcon();
		}

		// Token: 0x06012D04 RID: 77060 RVA: 0x00595056 File Offset: 0x00593256
		protected override string GetIconTip(Pawn pawn)
		{
			return pawn.GetGenderLabel().CapitalizeFirst();
		}
	}
}
