using System;
using System.Text;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02003D65 RID: 15717
	public static class CaravanRideableUtility
	{
		// Token: 0x0601568C RID: 87692 RVA: 0x00667B91 File Offset: 0x00665D91
		public static bool IsCaravanRideable(this Pawn pawn)
		{
			return !pawn.Downed && pawn.ageTracker.CurLifeStage.caravanRideable && pawn.GetStatValue(StatDefOf.CaravanRidingSpeedFactor, true, -1) > 1f;
		}

		// Token: 0x0601568D RID: 87693 RVA: 0x00667BC3 File Offset: 0x00665DC3
		public static bool IsCaravanRideable(this ThingDef def)
		{
			return def.StatBaseDefined(StatDefOf.CaravanRidingSpeedFactor);
		}

		// Token: 0x0601568E RID: 87694 RVA: 0x00667BD0 File Offset: 0x00665DD0
		public static string RideableLifeStagesDesc(RaceProperties raceProps)
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (LifeStageAge lifeStageAge in raceProps.lifeStageAges)
			{
				if (lifeStageAge.def.caravanRideable)
				{
					stringBuilder.AppendWithComma(lifeStageAge.def.label);
				}
			}
			return stringBuilder.ToString().CapitalizeFirst();
		}

		// Token: 0x0601568F RID: 87695 RVA: 0x00667C4C File Offset: 0x00665E4C
		public static string GetIconTooltipText(Pawn pawn)
		{
			float statValue = pawn.GetStatValue(StatDefOf.CaravanRidingSpeedFactor, true, -1);
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("RideableAnimalTip".Translate());
			stringBuilder.AppendLine();
			stringBuilder.AppendLine();
			stringBuilder.AppendLine(string.Format("{0}: {1}", StatDefOf.CaravanRidingSpeedFactor.LabelCap, statValue.ToStringPercent()));
			return stringBuilder.ToString();
		}
	}
}
