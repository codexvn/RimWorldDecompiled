using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02003443 RID: 13379
	[StaticConstructorOnStartup]
	public class PawnColumnWorker_MentalState : PawnColumnWorker_Icon
	{
		// Token: 0x06012D26 RID: 77094 RVA: 0x0059549B File Offset: 0x0059369B
		protected override Texture2D GetIconFor(Pawn pawn)
		{
			if (!pawn.InMentalState)
			{
				return null;
			}
			if (!pawn.InAggroMentalState)
			{
				return PawnColumnWorker_MentalState.IconNonAggro;
			}
			return PawnColumnWorker_MentalState.IconAggro;
		}

		// Token: 0x06012D27 RID: 77095 RVA: 0x005954BA File Offset: 0x005936BA
		protected override string GetIconTip(Pawn pawn)
		{
			return pawn.InMentalState ? "IsInMentalState".Translate(pawn.MentalState.def.LabelCap) : null;
		}

		// Token: 0x0400C354 RID: 50004
		private static readonly Texture2D IconNonAggro = ContentFinder<Texture2D>.Get("UI/Icons/ColonistBar/MentalStateNonAggro", true);

		// Token: 0x0400C355 RID: 50005
		private static readonly Texture2D IconAggro = ContentFinder<Texture2D>.Get("UI/Icons/ColonistBar/MentalStateAggro", true);
	}
}
