using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002AC9 RID: 10953
	public class TunnelJellySpawner : TunnelHiveSpawner
	{
		// Token: 0x0600F407 RID: 62471 RVA: 0x0046EA10 File Offset: 0x0046CC10
		protected override void Spawn(Map map, IntVec3 loc)
		{
			if (this.jellyCount > 0)
			{
				int num = Rand.Range(2, 3);
				int num2 = this.jellyCount;
				for (int i = 0; i < num; i++)
				{
					Thing thing = ThingMaker.MakeThing(ThingDefOf.InsectJelly, null);
					if (i < num - 1)
					{
						thing.stackCount = this.jellyCount / num;
						num2 -= thing.stackCount;
					}
					else
					{
						thing.stackCount = num2;
					}
					GenSpawn.Spawn(thing, CellFinder.RandomClosewalkCellNear(loc, map, 2, null), map, WipeMode.Vanish);
				}
				this.jellyCount = 0;
			}
		}

		// Token: 0x0600F408 RID: 62472 RVA: 0x0046EA8C File Offset: 0x0046CC8C
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.jellyCount, "jellyCount", 0, false);
		}

		// Token: 0x0400A0E0 RID: 41184
		public int jellyCount;
	}
}
