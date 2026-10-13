using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200358A RID: 13706
	[DefOf]
	public static class GameConditionDefOf
	{
		// Token: 0x06013378 RID: 78712 RVA: 0x005B5983 File Offset: 0x005B3B83
		static GameConditionDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(GameConditionDefOf));
		}

		// Token: 0x0400CB9C RID: 52124
		public static GameConditionDef Eclipse;

		// Token: 0x0400CB9D RID: 52125
		public static GameConditionDef PsychicDrone;

		// Token: 0x0400CB9E RID: 52126
		public static GameConditionDef PsychicSoothe;

		// Token: 0x0400CB9F RID: 52127
		public static GameConditionDef HeatWave;

		// Token: 0x0400CBA0 RID: 52128
		public static GameConditionDef ColdSnap;

		// Token: 0x0400CBA1 RID: 52129
		public static GameConditionDef Flashstorm;

		// Token: 0x0400CBA2 RID: 52130
		public static GameConditionDef VolcanicWinter;

		// Token: 0x0400CBA3 RID: 52131
		public static GameConditionDef ToxicFallout;

		// Token: 0x0400CBA4 RID: 52132
		public static GameConditionDef Aurora;

		// Token: 0x0400CBA5 RID: 52133
		[MayRequireRoyalty]
		public static GameConditionDef PsychicSuppression;

		// Token: 0x0400CBA6 RID: 52134
		[MayRequireRoyalty]
		public static GameConditionDef WeatherController;

		// Token: 0x0400CBA7 RID: 52135
		[MayRequireRoyalty]
		public static GameConditionDef EMIField;

		// Token: 0x0400CBA8 RID: 52136
		[MayRequireBiotech]
		public static GameConditionDef NoxiousHaze;

		// Token: 0x0400CBA9 RID: 52137
		[MayRequireAnomaly]
		public static GameConditionDef DeathPall;

		// Token: 0x0400CBAA RID: 52138
		[MayRequireAnomaly]
		public static GameConditionDef GrayPall;

		// Token: 0x0400CBAB RID: 52139
		[MayRequireAnomaly]
		public static GameConditionDef HateChantDrone;

		// Token: 0x0400CBAC RID: 52140
		[MayRequireAnomaly]
		public static GameConditionDef UnnaturalHeat;

		// Token: 0x0400CBAD RID: 52141
		[MayRequireAnomaly]
		public static GameConditionDef UnnaturalDarkness;

		// Token: 0x0400CBAE RID: 52142
		[MayRequireAnomaly]
		public static GameConditionDef BloodRain;

		// Token: 0x0400CBAF RID: 52143
		[MayRequireOdyssey]
		public static GameConditionDef Drought;

		// Token: 0x0400CBB0 RID: 52144
		[MayRequireOdyssey]
		public static GameConditionDef LavaFlow;

		// Token: 0x0400CBB1 RID: 52145
		[MayRequireOdyssey]
		public static GameConditionDef GillRot;
	}
}
