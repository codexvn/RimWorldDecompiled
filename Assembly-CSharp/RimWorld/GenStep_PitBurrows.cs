using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001FDB RID: 8155
	public class GenStep_PitBurrows : GenStep
	{
		// Token: 0x17001B69 RID: 7017
		// (get) Token: 0x0600B3CD RID: 46029 RVA: 0x00338A53 File Offset: 0x00336C53
		public override int SeedPart
		{
			get
			{
				return 512346123;
			}
		}

		// Token: 0x0600B3CE RID: 46030 RVA: 0x00338A5C File Offset: 0x00336C5C
		public override void Generate(Map map, GenStepParams parms)
		{
			int randomInRange = GenStep_PitBurrows.NumBurrowsRange.RandomInRange;
			Predicate<IntVec3> <>9__0;
			for (int i = 0; i < randomInRange; i++)
			{
				IntVec3 center = map.Center;
				Predicate<IntVec3> validator;
				if ((validator = <>9__0) == null)
				{
					validator = (<>9__0 = ((IntVec3 c) => GenSpawn.CanSpawnAt(ThingDefOf.PitBurrow, c, map, null, true)));
				}
				IntVec3 loc;
				if (RCellFinder.TryFindRandomCellNearWith(center, validator, map, out loc, 30, 2147483647))
				{
					GenSpawn.Spawn(ThingDefOf.PitBurrow, loc, map, WipeMode.Vanish).SetFaction(Faction.OfEntities, null);
				}
			}
		}

		// Token: 0x0600B3CF RID: 46031 RVA: 0x00338AF0 File Offset: 0x00336CF0
		private bool Validator(CellRect rect, Map map)
		{
			foreach (IntVec3 c in rect.Cells)
			{
				if (!c.Standable(map))
				{
					return false;
				}
				if (!c.GetAffordances(map).Contains(ThingDefOf.PitBurrow.terrainAffordanceNeeded))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x04007B4E RID: 31566
		private static readonly IntRange NumBurrowsRange = new IntRange(2, 3);
	}
}
