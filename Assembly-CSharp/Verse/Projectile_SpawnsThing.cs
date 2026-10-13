using System;

namespace Verse
{
	// Token: 0x020008CA RID: 2250
	public class Projectile_SpawnsThing : Projectile
	{
		// Token: 0x060038EE RID: 14574 RVA: 0x0012FB50 File Offset: 0x0012DD50
		protected override void Impact(Thing hitThing, bool blockedByShield = false)
		{
			Map map = base.Map;
			base.Impact(hitThing, blockedByShield);
			IntVec3 loc = base.Position;
			if (this.def.projectile.tryAdjacentFreeSpaces && base.Position.GetFirstBuilding(map) != null)
			{
				foreach (IntVec3 intVec in GenAdjFast.AdjacentCells8Way(base.Position))
				{
					if (intVec.GetFirstBuilding(map) == null && intVec.Standable(map))
					{
						loc = intVec;
						break;
					}
				}
			}
			Thing thing = GenSpawn.Spawn(ThingMaker.MakeThing(this.def.projectile.spawnsThingDef, null), loc, map, WipeMode.Vanish);
			if (thing.def.CanHaveFaction)
			{
				thing.SetFaction(base.Launcher.Faction, null);
			}
		}
	}
}
