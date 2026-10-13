using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x02003914 RID: 14612
	public class SymbolResolver_EdgeWalls : SymbolResolver
	{
		// Token: 0x06014243 RID: 82499 RVA: 0x0060001C File Offset: 0x005FE21C
		public override void Resolve(ResolveParams rp)
		{
			ThingDef wallStuff = rp.wallStuff ?? BaseGenUtility.RandomCheapWallStuff(rp.faction, false);
			foreach (IntVec3 c in rp.rect.EdgeCells)
			{
				if (c.InBounds(BaseGen.globalSettings.map))
				{
					this.TrySpawnWall(c, rp, wallStuff);
				}
			}
		}

		// Token: 0x06014244 RID: 82500 RVA: 0x0060009C File Offset: 0x005FE29C
		private Thing TrySpawnWall(IntVec3 c, ResolveParams rp, ThingDef wallStuff)
		{
			Map map = BaseGen.globalSettings.map;
			List<Thing> thingList = c.GetThingList(map);
			for (int i = 0; i < thingList.Count; i++)
			{
				if (!thingList[i].def.destroyable)
				{
					return null;
				}
				if (thingList[i] is Building_Door)
				{
					return null;
				}
			}
			for (int j = thingList.Count - 1; j >= 0; j--)
			{
				thingList[j].Destroy(DestroyMode.Vanish);
			}
			if (rp.chanceToSkipWallBlock != null && Rand.Chance(rp.chanceToSkipWallBlock.Value))
			{
				return null;
			}
			ThingDef thingDef = rp.wallThingDef ?? ThingDefOf.Wall;
			Thing thing = ThingMaker.MakeThing(thingDef, thingDef.MadeFromStuff ? wallStuff : null);
			thing.SetFaction(rp.faction, null);
			return GenSpawn.Spawn(thing, c, map, WipeMode.Vanish);
		}
	}
}
