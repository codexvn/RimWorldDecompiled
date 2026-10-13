using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02002B3C RID: 11068
	public class FireArrow : Projectile
	{
		// Token: 0x0600F785 RID: 63365 RVA: 0x0047F594 File Offset: 0x0047D794
		protected override void Impact(Thing hitThing, bool blockedByShield = false)
		{
			if (!base.Position.GetTerrain(base.Map).IsWater)
			{
				int remainingFuel = 4;
				base.Map.floodFiller.FloodFill(base.Position, (IntVec3 c) => c.InBounds(base.Map) && !c.Filled(base.Map), delegate(IntVec3 c)
				{
					using (List<Thing>.Enumerator enumerator = c.GetThingList(this.Map).GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							if (enumerator.Current.def == ThingDefOf.Filth_Fuel)
							{
								return false;
							}
						}
					}
					int remainingFuel;
					if (FilthMaker.TryMakeFilth(c, this.Map, ThingDefOf.Filth_Fuel, 1, FilthSourceFlags.None, true))
					{
						remainingFuel = remainingFuel;
						remainingFuel--;
					}
					return remainingFuel <= 0;
				}, 30, false, null);
				FireUtility.TryStartFireIn(base.Position, base.Map, FireArrow.FireSizeRange.RandomInRange, this.launcher, null);
			}
			FleckCreationData dataStatic = FleckMaker.GetDataStatic(this.DrawPos, base.Map, FleckDefOf.MicroSparksFast, 1f);
			dataStatic.velocitySpeed = 0.8f;
			dataStatic.velocityAngle = this.ExactRotation.eulerAngles.y;
			base.Map.flecks.CreateFleck(dataStatic);
			base.Impact(hitThing, blockedByShield);
		}

		// Token: 0x0400A2DE RID: 41694
		private static FloatRange FireSizeRange = new FloatRange(0.5f, 0.8f);

		// Token: 0x0400A2DF RID: 41695
		private const int FuelToSpreadOnImpact = 4;

		// Token: 0x0400A2E0 RID: 41696
		private const int MaxCellsToSpread = 30;
	}
}
