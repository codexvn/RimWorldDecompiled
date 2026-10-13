using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002CA4 RID: 11428
	public static class FoodPoisonCauseExtension
	{
		// Token: 0x060100A4 RID: 65700 RVA: 0x004A4DF0 File Offset: 0x004A2FF0
		public static string ToStringHuman(this FoodPoisonCause cause)
		{
			switch (cause)
			{
			case FoodPoisonCause.Unknown:
				return "UnknownLower".Translate().CapitalizeFirst();
			case FoodPoisonCause.IncompetentCook:
				return "FoodPoisonCause_IncompetentCook".Translate();
			case FoodPoisonCause.FilthyKitchen:
				return "FoodPoisonCause_FilthyKitchen".Translate();
			case FoodPoisonCause.Rotten:
				return "FoodPoisonCause_Rotten".Translate();
			case FoodPoisonCause.DangerousFoodType:
				return "FoodPoisonCause_DangerousFoodType".Translate();
			default:
				return cause.ToString();
			}
		}
	}
}
