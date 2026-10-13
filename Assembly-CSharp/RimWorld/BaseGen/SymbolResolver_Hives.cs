using System;
using System.Linq;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x02003960 RID: 14688
	public class SymbolResolver_Hives : SymbolResolver
	{
		// Token: 0x06014332 RID: 82738 RVA: 0x00606898 File Offset: 0x00604A98
		public override bool CanResolve(ResolveParams rp)
		{
			IntVec3 intVec;
			return base.CanResolve(rp) && Faction.OfInsects != null && this.TryFindFirstHivePos(rp.rect, out intVec);
		}

		// Token: 0x06014333 RID: 82739 RVA: 0x006068CC File Offset: 0x00604ACC
		public override void Resolve(ResolveParams rp)
		{
			IntVec3 intVec;
			if (!this.TryFindFirstHivePos(rp.rect, out intVec))
			{
				return;
			}
			int num = rp.hivesCount ?? SymbolResolver_Hives.DefaultHivesCountRange.RandomInRange;
			IntVec3 spawnCell = intVec;
			Map map = BaseGen.globalSettings.map;
			int count = num;
			float radius = 5f;
			WipeMode wipeMode = WipeMode.VanishOrMoveAside;
			bool spawnInsectsImmediately = false;
			bool canSpawnHives = true;
			bool canSpawnInsects = true;
			bool? disableHives = rp.disableHives;
			bool flag = true;
			HiveUtility.SpawnHives(spawnCell, map, count, radius, wipeMode, spawnInsectsImmediately, canSpawnHives, canSpawnInsects, disableHives.GetValueOrDefault() == flag & disableHives != null, true, true);
		}

		// Token: 0x06014334 RID: 82740 RVA: 0x0060694C File Offset: 0x00604B4C
		private bool TryFindFirstHivePos(CellRect rect, out IntVec3 pos)
		{
			Map map = BaseGen.globalSettings.map;
			return (from mc in rect.Cells
			where mc.Standable(map)
			select mc).TryRandomElement(out pos);
		}

		// Token: 0x0400DF45 RID: 57157
		private static readonly IntRange DefaultHivesCountRange = new IntRange(1, 3);
	}
}
