using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001FA1 RID: 8097
	public class GenStep_BurntPatches : GenStep
	{
		// Token: 0x17001B3B RID: 6971
		// (get) Token: 0x0600B2D5 RID: 45781 RVA: 0x0033383C File Offset: 0x00331A3C
		public override int SeedPart
		{
			get
			{
				return 823478234;
			}
		}

		// Token: 0x0600B2D6 RID: 45782 RVA: 0x00333844 File Offset: 0x00331A44
		public override void Generate(Map map, GenStepParams parms)
		{
			int randomInRange = GenStep_BurntPatches.PatchCountRange.RandomInRange;
			for (int i = 0; i < randomInRange; i++)
			{
				this.GeneratePatch(map);
			}
		}

		// Token: 0x0600B2D7 RID: 45783 RVA: 0x00333874 File Offset: 0x00331A74
		private void GeneratePatch(Map map)
		{
			IntVec3 center;
			if (!RCellFinder.TryFindRandomCellNearWith(map.Center, (IntVec3 c) => c.Standable(map), map, out center, 30, 2147483647))
			{
				return;
			}
			foreach (IntVec3 c2 in GridShapeMaker.IrregularLump(center, map, GenStep_BurntPatches.PatchSizeRange.RandomInRange, null))
			{
				List<Thing> thingList = c2.GetThingList(map);
				for (int i = thingList.Count - 1; i >= 0; i--)
				{
					Thing thing = thingList[i];
					if (thing.def.IsPlant)
					{
						thing.Kill(new DamageInfo?(new DamageInfo(DamageDefOf.Flame, 9999f, 0f, -1f, null, null, null, DamageInfo.SourceCategory.ThingOrUnknown, null, true, true, QualityCategory.Normal, true, false)), null);
					}
				}
				map.snowGrid.SetDepth(c2, 0f);
				FilthMaker.TryMakeFilth(c2, map, ThingDefOf.Filth_Ash, 1, FilthSourceFlags.None, true);
			}
		}

		// Token: 0x04007A74 RID: 31348
		private static readonly IntRange PatchCountRange = new IntRange(0, 2);

		// Token: 0x04007A75 RID: 31349
		private static readonly IntRange PatchSizeRange = new IntRange(50, 200);
	}
}
