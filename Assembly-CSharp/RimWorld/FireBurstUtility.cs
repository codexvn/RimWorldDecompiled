using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002226 RID: 8742
	public static class FireBurstUtility
	{
		// Token: 0x0600BF23 RID: 48931 RVA: 0x00376DD4 File Offset: 0x00374FD4
		public static void ThrowFuelTick(IntVec3 position, float radius, Map map)
		{
			if (!Rand.Chance(0.15f))
			{
				return;
			}
			foreach (IntVec3 intVec in GenRadial.RadialCellsAround(position, radius, true).InRandomOrder(null))
			{
				if (GenSight.LineOfSight(position, intVec, map, true, null, 0, 0) && FilthMaker.TryMakeFilth(intVec, map, ThingDefOf.Filth_Fuel, 1, FilthSourceFlags.None, true))
				{
					break;
				}
			}
		}

		// Token: 0x04008338 RID: 33592
		private const float FuelSpawnChancePerCastingTick = 0.15f;
	}
}
