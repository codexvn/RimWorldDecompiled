using System;

namespace Verse
{
	// Token: 0x02000530 RID: 1328
	public static class WeatherBuildupUtility
	{
		// Token: 0x0600226F RID: 8815 RVA: 0x000B30B7 File Offset: 0x000B12B7
		public static WeatherBuildupCategory GetBuildupCategory(float depth)
		{
			if (depth < 0.03f)
			{
				return WeatherBuildupCategory.None;
			}
			if (depth < 0.25f)
			{
				return WeatherBuildupCategory.Dusting;
			}
			if (depth < 0.5f)
			{
				return WeatherBuildupCategory.Thin;
			}
			if (depth < 0.75f)
			{
				return WeatherBuildupCategory.Medium;
			}
			return WeatherBuildupCategory.Thick;
		}

		// Token: 0x06002270 RID: 8816 RVA: 0x000B30E4 File Offset: 0x000B12E4
		public static string GetSnowDescription(WeatherBuildupCategory category)
		{
			switch (category)
			{
			case WeatherBuildupCategory.None:
				return "SnowNone".Translate();
			case WeatherBuildupCategory.Dusting:
				return "SnowDusting".Translate();
			case WeatherBuildupCategory.Thin:
				return "SnowThin".Translate();
			case WeatherBuildupCategory.Medium:
				return "SnowMedium".Translate();
			case WeatherBuildupCategory.Thick:
				return "SnowThick".Translate();
			default:
				return "Unknown snow";
			}
		}

		// Token: 0x06002271 RID: 8817 RVA: 0x000B3164 File Offset: 0x000B1364
		public static string GetSandDescription(WeatherBuildupCategory category)
		{
			switch (category)
			{
			case WeatherBuildupCategory.None:
				return "SandNone".Translate();
			case WeatherBuildupCategory.Dusting:
				return "SandDusting".Translate();
			case WeatherBuildupCategory.Thin:
				return "SandThin".Translate();
			case WeatherBuildupCategory.Medium:
				return "SandMedium".Translate();
			case WeatherBuildupCategory.Thick:
				return "SandThick".Translate();
			default:
				return "Unknown sand";
			}
		}

		// Token: 0x06002272 RID: 8818 RVA: 0x000B31E2 File Offset: 0x000B13E2
		public static int MovementTicksAddOn(WeatherBuildupCategory category)
		{
			switch (category)
			{
			case WeatherBuildupCategory.None:
				return 0;
			case WeatherBuildupCategory.Dusting:
				return 0;
			case WeatherBuildupCategory.Thin:
				return 4;
			case WeatherBuildupCategory.Medium:
				return 8;
			case WeatherBuildupCategory.Thick:
				return 12;
			default:
				return 0;
			}
		}

		// Token: 0x06002273 RID: 8819 RVA: 0x000B320C File Offset: 0x000B140C
		public static void AddSnowRadial(IntVec3 center, Map map, float radius, float depth)
		{
			int num = GenRadial.NumCellsInRadius(radius);
			for (int i = 0; i < num; i++)
			{
				IntVec3 intVec = center + GenRadial.RadialPattern[i];
				if (intVec.InBounds(map))
				{
					float lengthHorizontal = (center - intVec).LengthHorizontal;
					float num2 = 1f - lengthHorizontal / radius;
					map.snowGrid.AddDepth(intVec, num2 * depth);
				}
			}
		}

		// Token: 0x06002274 RID: 8820 RVA: 0x000B3274 File Offset: 0x000B1474
		public static void AddSandRadial(IntVec3 center, Map map, float radius, float depth)
		{
			int num = GenRadial.NumCellsInRadius(radius);
			for (int i = 0; i < num; i++)
			{
				IntVec3 intVec = center + GenRadial.RadialPattern[i];
				if (intVec.InBounds(map))
				{
					float lengthHorizontal = (center - intVec).LengthHorizontal;
					float num2 = 1f - lengthHorizontal / radius;
					map.sandGrid.AddDepth(intVec, num2 * depth);
				}
			}
		}
	}
}
