using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002E27 RID: 11815
	public class CompSpawnerFilth : ThingComp
	{
		// Token: 0x17002AA1 RID: 10913
		// (get) Token: 0x06010A37 RID: 68151 RVA: 0x004CC6DF File Offset: 0x004CA8DF
		private CompProperties_SpawnerFilth Props
		{
			get
			{
				return (CompProperties_SpawnerFilth)this.props;
			}
		}

		// Token: 0x17002AA2 RID: 10914
		// (get) Token: 0x06010A38 RID: 68152 RVA: 0x004CC6EC File Offset: 0x004CA8EC
		private bool CanSpawnFilth
		{
			get
			{
				Hive hive = this.parent as Hive;
				if (hive != null && !hive.CompDormant.Awake)
				{
					return false;
				}
				if (this.Props.requiredRotStage != null)
				{
					RotStage rotStage = this.parent.GetRotStage();
					RotStage? requiredRotStage = this.Props.requiredRotStage;
					if (!(rotStage == requiredRotStage.GetValueOrDefault() & requiredRotStage != null))
					{
						return false;
					}
				}
				return true;
			}
		}

		// Token: 0x06010A39 RID: 68153 RVA: 0x004CC756 File Offset: 0x004CA956
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<int>(ref this.nextSpawnTimestamp, "nextSpawnTimestamp", -1, false);
		}

		// Token: 0x06010A3A RID: 68154 RVA: 0x004CC770 File Offset: 0x004CA970
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			if (!respawningAfterLoad)
			{
				for (int i = 0; i < this.Props.spawnCountOnSpawn; i++)
				{
					this.TrySpawnFilth();
				}
			}
		}

		// Token: 0x06010A3B RID: 68155 RVA: 0x004CC79C File Offset: 0x004CA99C
		public override void CompTickInterval(int delta)
		{
			base.CompTickInterval(delta);
			this.TickIntervalDelta(delta);
		}

		// Token: 0x06010A3C RID: 68156 RVA: 0x004CC7AC File Offset: 0x004CA9AC
		public override void CompTickRare()
		{
			base.CompTickRare();
			this.TickIntervalDelta(250);
		}

		// Token: 0x06010A3D RID: 68157 RVA: 0x004CC7C0 File Offset: 0x004CA9C0
		private void TickIntervalDelta(int interval)
		{
			if (!this.CanSpawnFilth)
			{
				return;
			}
			if (this.Props.spawnMtbHours > 0f && Rand.MTBEventOccurs(this.Props.spawnMtbHours, 2500f, (float)interval))
			{
				this.TrySpawnFilth();
			}
			if (this.Props.spawnEveryDays >= 0f && Find.TickManager.TicksGame >= this.nextSpawnTimestamp)
			{
				if (this.nextSpawnTimestamp != -1)
				{
					this.TrySpawnFilth();
				}
				this.nextSpawnTimestamp = Find.TickManager.TicksGame + (int)(this.Props.spawnEveryDays * 60000f);
			}
		}

		// Token: 0x06010A3E RID: 68158 RVA: 0x004CC860 File Offset: 0x004CAA60
		public void TrySpawnFilth()
		{
			if (this.parent.Map == null)
			{
				return;
			}
			IntVec3 c;
			if (!CellFinder.TryFindRandomReachableNearbyCell(this.parent.Position, this.parent.Map, this.Props.spawnRadius, TraverseParms.For(TraverseMode.NoPassClosedDoors, Danger.Deadly, false, false, false, true, false), (IntVec3 x) => x.Standable(this.parent.Map), (Region x) => true, out c, 999999))
			{
				return;
			}
			FilthMaker.TryMakeFilth(c, this.parent.Map, this.Props.filthDef, 1, FilthSourceFlags.None, true);
		}

		// Token: 0x0400AD66 RID: 44390
		private int nextSpawnTimestamp = -1;
	}
}
