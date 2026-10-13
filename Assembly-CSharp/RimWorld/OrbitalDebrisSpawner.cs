using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002AB4 RID: 10932
	public class OrbitalDebrisSpawner : Thing
	{
		// Token: 0x0600F397 RID: 62359 RVA: 0x0046C798 File Offset: 0x0046A998
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.startTick, "startTick", 0, false);
			Scribe_Values.Look<int>(ref this.endTick, "endTick", 0, false);
			Scribe_Values.Look<int>(ref this.meteoriteCount, "meteoriteCount", 0, false);
			Scribe_Values.Look<int>(ref this.meteoritesSpawned, "meteoritesSpawned", 0, false);
		}

		// Token: 0x0600F398 RID: 62360 RVA: 0x0046C7F4 File Offset: 0x0046A9F4
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			if (respawningAfterLoad)
			{
				return;
			}
			this.startTick = GenTicks.TicksGame + OrbitalDebrisSpawner.DelaySecondsRange.RandomInRange.SecondsToTicks();
			this.endTick = this.startTick + OrbitalDebrisSpawner.DurationSecondsRange.RandomInRange.SecondsToTicks();
			this.meteoriteCount = OrbitalDebrisSpawner.MeteoriteRange.RandomInRange;
		}

		// Token: 0x0600F399 RID: 62361 RVA: 0x0046C860 File Offset: 0x0046AA60
		protected override void Tick()
		{
			if (GenTicks.TicksGame > this.endTick)
			{
				this.Destroy(DestroyMode.Vanish);
				return;
			}
			if (GenTicks.TicksGame < this.startTick || this.meteoritesSpawned >= this.meteoriteCount)
			{
				return;
			}
			int positionsRemaining = this.endTick - GenTicks.TicksGame;
			if (Rand.DynamicChance(this.meteoritesSpawned, this.meteoriteCount, positionsRemaining))
			{
				this.meteoritesSpawned++;
				this.SpawnMeteoriteIncoming();
			}
		}

		// Token: 0x0600F39A RID: 62362 RVA: 0x0046C8D4 File Offset: 0x0046AAD4
		private void SpawnMeteoriteIncoming()
		{
			IntVec3 pos;
			if (this.TryFindCell(out pos))
			{
				SkyfallerMaker.SpawnSkyfaller(ThingDefOf.MeteoriteCraterIncoming, pos, base.Map);
			}
		}

		// Token: 0x0600F39B RID: 62363 RVA: 0x0046C900 File Offset: 0x0046AB00
		private bool TryFindCell(out IntVec3 cell)
		{
			return CellFinderLoose.TryFindSkyfallerCell(ThingDefOf.MeteoriteCraterIncoming, base.Map, TerrainAffordanceDefOf.Light, out cell, 10, default(IntVec3), -1, false, false, false, false, false, false, null);
		}

		// Token: 0x0400A070 RID: 41072
		private static readonly FloatRange DelaySecondsRange = new FloatRange(3f, 12f);

		// Token: 0x0400A071 RID: 41073
		private static readonly FloatRange DurationSecondsRange = new FloatRange(5f, 5f);

		// Token: 0x0400A072 RID: 41074
		private static readonly IntRange MeteoriteRange = new IntRange(12, 16);

		// Token: 0x0400A073 RID: 41075
		private int startTick;

		// Token: 0x0400A074 RID: 41076
		private int endTick;

		// Token: 0x0400A075 RID: 41077
		private int meteoriteCount;

		// Token: 0x0400A076 RID: 41078
		private int meteoritesSpawned;
	}
}
