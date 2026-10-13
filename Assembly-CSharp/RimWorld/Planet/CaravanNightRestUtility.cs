using System;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02003D63 RID: 15715
	public static class CaravanNightRestUtility
	{
		// Token: 0x06015681 RID: 87681 RVA: 0x00667823 File Offset: 0x00665A23
		public static bool RestingNowAt(PlanetTile tile)
		{
			return CaravanNightRestUtility.WouldBeRestingAt(tile, (long)GenTicks.TicksAbs);
		}

		// Token: 0x06015682 RID: 87682 RVA: 0x00667834 File Offset: 0x00665A34
		public static bool WouldBeRestingAt(PlanetTile tile, long ticksAbs)
		{
			float num = GenDate.HourFloat(ticksAbs, Find.WorldGrid.LongLatOf(tile).x);
			return num < 6f || num > 22f;
		}

		// Token: 0x06015683 RID: 87683 RVA: 0x0066786A File Offset: 0x00665A6A
		public static int LeftRestTicksAt(PlanetTile tile)
		{
			return CaravanNightRestUtility.LeftRestTicksAt(tile, (long)GenTicks.TicksAbs);
		}

		// Token: 0x06015684 RID: 87684 RVA: 0x00667878 File Offset: 0x00665A78
		public static int LeftRestTicksAt(PlanetTile tile, long ticksAbs)
		{
			if (!CaravanNightRestUtility.WouldBeRestingAt(tile, ticksAbs))
			{
				return 0;
			}
			float num = GenDate.HourFloat(ticksAbs, Find.WorldGrid.LongLatOf(tile).x);
			if (num < 6f)
			{
				return Mathf.CeilToInt((6f - num) * 2500f);
			}
			return Mathf.CeilToInt((24f - num + 6f) * 2500f);
		}

		// Token: 0x06015685 RID: 87685 RVA: 0x006678DA File Offset: 0x00665ADA
		public static int LeftNonRestTicksAt(PlanetTile tile)
		{
			return CaravanNightRestUtility.LeftNonRestTicksAt(tile, (long)GenTicks.TicksAbs);
		}

		// Token: 0x06015686 RID: 87686 RVA: 0x006678E8 File Offset: 0x00665AE8
		public static int LeftNonRestTicksAt(PlanetTile tile, long ticksAbs)
		{
			if (CaravanNightRestUtility.WouldBeRestingAt(tile, ticksAbs))
			{
				return 0;
			}
			float num = GenDate.HourFloat(ticksAbs, Find.WorldGrid.LongLatOf(tile).x);
			return Mathf.CeilToInt((22f - num) * 2500f);
		}

		// Token: 0x0400EE53 RID: 61011
		public const float WakeUpHour = 6f;

		// Token: 0x0400EE54 RID: 61012
		public const float RestStartHour = 22f;
	}
}
