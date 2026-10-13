using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02003445 RID: 13381
	[StaticConstructorOnStartup]
	public class PawnColumnWorker_Pregnant : PawnColumnWorker_Icon
	{
		// Token: 0x06012D2E RID: 77102 RVA: 0x0059554B File Offset: 0x0059374B
		protected override Texture2D GetIconFor(Pawn pawn)
		{
			if (PawnColumnWorker_Pregnant.GetPregnantHediff(pawn) == null)
			{
				return null;
			}
			return PawnColumnWorker_Pregnant.Icon;
		}

		// Token: 0x06012D2F RID: 77103 RVA: 0x0059555C File Offset: 0x0059375C
		protected override string GetIconTip(Pawn pawn)
		{
			return PawnColumnWorker_Pregnant.GetTooltipText(pawn);
		}

		// Token: 0x06012D30 RID: 77104 RVA: 0x00595564 File Offset: 0x00593764
		public static string GetTooltipText(Pawn pawn)
		{
			float gestationProgress = PawnColumnWorker_Pregnant.GetPregnantHediff(pawn).GestationProgress;
			int num = (int)(pawn.RaceProps.gestationPeriodDays * 60000f);
			int numTicks = (int)(gestationProgress * (float)num);
			return "PregnantIconDesc".Translate(numTicks.ToStringTicksToDays("F0"), num.ToStringTicksToDays("F0"));
		}

		// Token: 0x06012D31 RID: 77105 RVA: 0x005955C3 File Offset: 0x005937C3
		private static Hediff_Pregnant GetPregnantHediff(Pawn pawn)
		{
			return (Hediff_Pregnant)pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.Pregnant, true);
		}

		// Token: 0x0400C357 RID: 50007
		private static readonly Texture2D Icon = ContentFinder<Texture2D>.Get("UI/Icons/Animal/Pregnant", true);
	}
}
