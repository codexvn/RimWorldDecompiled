using System;
using System.Text;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02003D42 RID: 15682
	public static class CaravanBonusUtility
	{
		// Token: 0x060155DA RID: 87514 RVA: 0x006637A1 File Offset: 0x006619A1
		public static bool HasCaravanBonus(Pawn pawn)
		{
			return pawn.RaceProps.Humanlike && !pawn.Downed && pawn.GetStatValue(StatDefOf.CaravanBonusSpeedFactor, true, -1) > 1f;
		}

		// Token: 0x060155DB RID: 87515 RVA: 0x006637D0 File Offset: 0x006619D0
		public static string GetIconTooltipText(Pawn pawn)
		{
			float statValue = pawn.GetStatValue(StatDefOf.CaravanBonusSpeedFactor, true, -1);
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("CaravanBonusTip".Translate());
			stringBuilder.AppendLine();
			stringBuilder.AppendLine();
			stringBuilder.AppendLine(string.Format("{0}: {1}", StatDefOf.CaravanBonusSpeedFactor.LabelCap, statValue.ToStringPercent()));
			return stringBuilder.ToString();
		}
	}
}
