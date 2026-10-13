using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02001E9C RID: 7836
	public class SpawnRequest : IExposable
	{
		// Token: 0x17001AA0 RID: 6816
		// (get) Token: 0x0600ADED RID: 44525 RVA: 0x00319A83 File Offset: 0x00317C83
		public int PreEffecterTick
		{
			get
			{
				return this.startedTick + this.initialDelay + this.preSpawnEffecterOffsetTicks;
			}
		}

		// Token: 0x0600ADEE RID: 44526 RVA: 0x00319A99 File Offset: 0x00317C99
		public SpawnRequest()
		{
		}

		// Token: 0x0600ADEF RID: 44527 RVA: 0x00319AAC File Offset: 0x00317CAC
		public SpawnRequest(List<Thing> thingsToSpawn, List<IntVec3> spawnPositions, int batchSize, float intervalSeconds, Lord lord = null)
		{
			this.thingsToSpawn = thingsToSpawn;
			this.unspawnedThings = thingsToSpawn.ToList<Thing>();
			this.spawnPositions = spawnPositions;
			this.batchSize = batchSize;
			this.lord = lord;
			this.intervalTicks = intervalSeconds.SecondsToTicks();
			this.startedTick = Find.TickManager.TicksGame;
		}

		// Token: 0x0600ADF0 RID: 44528 RVA: 0x00319B10 File Offset: 0x00317D10
		public SpawnRequest(List<Thing> thingsToSpawn, int batchSize, float intervalSeconds, Lord lord = null) : this(thingsToSpawn, (from t in thingsToSpawn
		select t.Position).ToList<IntVec3>(), batchSize, intervalSeconds, lord)
		{
		}

		// Token: 0x0600ADF1 RID: 44529 RVA: 0x00319B48 File Offset: 0x00317D48
		public void ExposeData()
		{
			Scribe_Collections.Look<Thing>(ref this.unspawnedThings, "unspawnedThings", LookMode.Deep, Array.Empty<object>());
			Scribe_Collections.Look<Thing>(ref this.thingsToSpawn, "thingsToSpawn", LookMode.Reference, Array.Empty<object>());
			Scribe_Collections.Look<IntVec3>(ref this.spawnPositions, "spawnPositions", LookMode.Value, Array.Empty<object>());
			Scribe_Collections.Look<Thing>(ref this.spawnedThings, "spawnedThings", LookMode.Reference, Array.Empty<object>());
			Scribe_Deep.Look<DeferredSpawnWorker>(ref this.spawnWorker, "spawnWorker", Array.Empty<object>());
			Scribe_Values.Look<int>(ref this.batchSize, "batchSize", 0, false);
			Scribe_Values.Look<int>(ref this.intervalTicks, "intervalTicks", 0, false);
			Scribe_Values.Look<int>(ref this.initialDelay, "initialDelay", 0, false);
			Scribe_Values.Look<int>(ref this.startedTick, "startedTick", 0, false);
			Scribe_References.Look<Lord>(ref this.lord, "lord", false);
			Scribe_Values.Look<int>(ref this.preSpawnEffecterOffsetTicks, "effecterOffsetTicks", 0, false);
		}

		// Token: 0x04007796 RID: 30614
		public List<Thing> thingsToSpawn;

		// Token: 0x04007797 RID: 30615
		public List<Thing> spawnedThings = new List<Thing>();

		// Token: 0x04007798 RID: 30616
		public List<Thing> unspawnedThings;

		// Token: 0x04007799 RID: 30617
		public List<IntVec3> spawnPositions;

		// Token: 0x0400779A RID: 30618
		public DeferredSpawnWorker spawnWorker;

		// Token: 0x0400779B RID: 30619
		public Lord lord;

		// Token: 0x0400779C RID: 30620
		public int batchSize;

		// Token: 0x0400779D RID: 30621
		public int intervalTicks;

		// Token: 0x0400779E RID: 30622
		public int initialDelay;

		// Token: 0x0400779F RID: 30623
		public int startedTick;

		// Token: 0x040077A0 RID: 30624
		public bool done;

		// Token: 0x040077A1 RID: 30625
		public SoundDef spawnSound;

		// Token: 0x040077A2 RID: 30626
		public EffecterDef spawnEffect;

		// Token: 0x040077A3 RID: 30627
		public EffecterDef preSpawnEffect;

		// Token: 0x040077A4 RID: 30628
		public int preSpawnEffecterOffsetTicks;
	}
}
