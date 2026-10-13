using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001D53 RID: 7507
	public class GameCondition_VolcanicDebrisShower : GameCondition
	{
		// Token: 0x170019FF RID: 6655
		// (get) Token: 0x0600A845 RID: 43077 RVA: 0x00302AA5 File Offset: 0x00300CA5
		public override bool Expired
		{
			get
			{
				return this.remainingLavaRockCount <= 0;
			}
		}

		// Token: 0x0600A846 RID: 43078 RVA: 0x00302AB4 File Offset: 0x00300CB4
		public override void Init()
		{
			base.Init();
			this.nextLavaRockTick = Find.TickManager.TicksGame + GameCondition_VolcanicDebrisShower.IntervalRange.RandomInRange;
			this.remainingLavaRockCount = GameCondition_VolcanicDebrisShower.MeteoriteCount.RandomInRange;
		}

		// Token: 0x0600A847 RID: 43079 RVA: 0x00302AF8 File Offset: 0x00300CF8
		public override void GameConditionTick()
		{
			if (Find.TickManager.TicksGame < this.nextLavaRockTick)
			{
				return;
			}
			Map singleMap = base.SingleMap;
			IntVec3 pos;
			if (!GameCondition_VolcanicDebrisShower.TryFindCell(out pos, singleMap))
			{
				Log.Error("GameCondition_VolcanicDebrisShower could not find a suitable cell.");
				return;
			}
			SkyfallerMaker.SpawnSkyfaller(ThingDefOf.LavaRockIncoming, pos, singleMap);
			this.nextLavaRockTick = Find.TickManager.TicksGame + GameCondition_VolcanicDebrisShower.IntervalRange.RandomInRange;
			this.remainingLavaRockCount--;
		}

		// Token: 0x0600A848 RID: 43080 RVA: 0x00302B70 File Offset: 0x00300D70
		private static bool TryFindCell(out IntVec3 cell, Map map)
		{
			return CellFinderLoose.TryFindSkyfallerCell(ThingDefOf.LavaRockIncoming, map, TerrainAffordanceDefOf.Walkable, out cell, 10, default(IntVec3), -1, true, false, false, false, true, true, null);
		}

		// Token: 0x0600A849 RID: 43081 RVA: 0x00302BA1 File Offset: 0x00300DA1
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.nextLavaRockTick, "nextLavaRockTick", 0, false);
			Scribe_Values.Look<int>(ref this.remainingLavaRockCount, "remainingLavaRockCount", 0, false);
		}

		// Token: 0x04007466 RID: 29798
		private static readonly IntRange MeteoriteCount = new IntRange(8, 12);

		// Token: 0x04007467 RID: 29799
		private static readonly IntRange IntervalRange = new IntRange(60, 180);

		// Token: 0x04007468 RID: 29800
		private int nextLavaRockTick;

		// Token: 0x04007469 RID: 29801
		private int remainingLavaRockCount;
	}
}
