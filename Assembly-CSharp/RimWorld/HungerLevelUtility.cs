using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200247B RID: 9339
	public static class HungerLevelUtility
	{
		// Token: 0x0600CA95 RID: 51861 RVA: 0x003B56A4 File Offset: 0x003B38A4
		public static string GetLabel(this HungerCategory hunger)
		{
			switch (hunger)
			{
			case HungerCategory.Fed:
				return "HungerLevel_Fed".Translate();
			case HungerCategory.Hungry:
				return "HungerLevel_Hungry".Translate();
			case HungerCategory.UrgentlyHungry:
				return "HungerLevel_UrgentlyHungry".Translate();
			case HungerCategory.Starving:
				return "HungerLevel_Starving".Translate();
			default:
				throw new InvalidOperationException();
			}
		}

		// Token: 0x0600CA96 RID: 51862 RVA: 0x003B570E File Offset: 0x003B390E
		public static float HungerMultiplier(this HungerCategory cat)
		{
			switch (cat)
			{
			case HungerCategory.Fed:
				return 1f;
			case HungerCategory.Hungry:
				return 0.5f;
			case HungerCategory.UrgentlyHungry:
				return 0.25f;
			case HungerCategory.Starving:
				return 0f;
			default:
				throw new NotImplementedException();
			}
		}

		// Token: 0x040089FF RID: 35327
		public const float FallPerTickFactor_Hungry = 0.5f;

		// Token: 0x04008A00 RID: 35328
		public const float FallPerTickFactor_UrgentlyHungry = 0.25f;
	}
}
