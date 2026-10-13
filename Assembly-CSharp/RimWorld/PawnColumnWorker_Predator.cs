using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02003444 RID: 13380
	[StaticConstructorOnStartup]
	public class PawnColumnWorker_Predator : PawnColumnWorker_Icon
	{
		// Token: 0x06012D2A RID: 77098 RVA: 0x00595512 File Offset: 0x00593712
		protected override Texture2D GetIconFor(Pawn pawn)
		{
			if (pawn.RaceProps.predator)
			{
				return PawnColumnWorker_Predator.Icon;
			}
			return null;
		}

		// Token: 0x06012D2B RID: 77099 RVA: 0x00595528 File Offset: 0x00593728
		protected override string GetIconTip(Pawn pawn)
		{
			return "IsPredator".Translate();
		}

		// Token: 0x0400C356 RID: 50006
		private static readonly Texture2D Icon = ContentFinder<Texture2D>.Get("UI/Icons/Animal/Predator", true);
	}
}
