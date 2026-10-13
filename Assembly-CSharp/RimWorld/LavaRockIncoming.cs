using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002B4E RID: 11086
	public class LavaRockIncoming : Skyfaller
	{
		// Token: 0x0600F7D5 RID: 63445 RVA: 0x00481120 File Offset: 0x0047F320
		protected override void Impact()
		{
			int randomInRange = LavaRockIncoming.LavaRadiusRange.RandomInRange;
			IntVec3 position = base.Position;
			Map map = base.Map;
			base.Impact();
			LavaEmergenceImmediate lavaEmergenceImmediate = (LavaEmergenceImmediate)ThingMaker.MakeThing(ThingDefOf.LavaEmergenceImmediate, null);
			lavaEmergenceImmediate.forceCoolDelay = LavaRockIncoming.CoolTicksRange.RandomInRange;
			lavaEmergenceImmediate.forcePoolSize = randomInRange * randomInRange * 3;
			GenSpawn.Spawn(lavaEmergenceImmediate, position, map, WipeMode.Vanish);
		}

		// Token: 0x0400A30A RID: 41738
		private static readonly IntRange LavaRadiusRange = new IntRange(3, 5);

		// Token: 0x0400A30B RID: 41739
		private static readonly IntRange CoolTicksRange = new IntRange(48000, 72000);
	}
}
