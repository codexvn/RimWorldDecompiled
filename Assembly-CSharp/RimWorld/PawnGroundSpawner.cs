using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02002AB6 RID: 10934
	public class PawnGroundSpawner : GroundSpawner, IThingHolder
	{
		// Token: 0x17002619 RID: 9753
		// (get) Token: 0x0600F3A6 RID: 62374 RVA: 0x0046CAB4 File Offset: 0x0046ACB4
		protected override IntRange ResultSpawnDelay
		{
			get
			{
				return this.spawnDelay;
			}
		}

		// Token: 0x0600F3A7 RID: 62375 RVA: 0x0046CABC File Offset: 0x0046ACBC
		public PawnGroundSpawner()
		{
			this.dustMoteSpawnMTB = 0.5f;
			this.filthSpawnMTB = 0.5f;
			this.filthSpawnRadius = 1f;
			this.innerContainer = new ThingOwner<Pawn>(this, true, LookMode.Deep, true);
		}

		// Token: 0x0600F3A8 RID: 62376 RVA: 0x0046CAF4 File Offset: 0x0046ACF4
		public void Init(Pawn pawnToSpawn, IntRange delayTicksRange)
		{
			this.spawnDelay = delayTicksRange;
			this.innerContainer.ClearAndDestroyContentsOrPassToWorld(DestroyMode.Vanish);
			this.innerContainer.TryAdd(pawnToSpawn, true);
		}

		// Token: 0x0600F3A9 RID: 62377 RVA: 0x0046CB17 File Offset: 0x0046AD17
		protected override void Spawn(Map map, IntVec3 loc)
		{
			this.innerContainer.TryDropAll(loc, map, ThingPlaceMode.Direct, null, null, false);
		}

		// Token: 0x0600F3AA RID: 62378 RVA: 0x0046CB2B File Offset: 0x0046AD2B
		public void GetChildHolders(List<IThingHolder> outChildren)
		{
			ThingOwnerUtility.AppendThingHoldersFromThings(outChildren, this.GetDirectlyHeldThings());
		}

		// Token: 0x0600F3AB RID: 62379 RVA: 0x0046CB39 File Offset: 0x0046AD39
		public ThingOwner GetDirectlyHeldThings()
		{
			return this.innerContainer;
		}

		// Token: 0x0600F3AC RID: 62380 RVA: 0x0046CB44 File Offset: 0x0046AD44
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<IntRange>(ref this.spawnDelay, "spawnDelay", default(IntRange), false);
			Scribe_Deep.Look<ThingOwner<Pawn>>(ref this.innerContainer, "innerContainer", new object[]
			{
				this
			});
		}

		// Token: 0x0400A080 RID: 41088
		private IntRange spawnDelay;

		// Token: 0x0400A081 RID: 41089
		private ThingOwner<Pawn> innerContainer;
	}
}
