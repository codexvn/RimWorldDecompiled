using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002A94 RID: 10900
	public class DyingRevenant : Thing
	{
		// Token: 0x0600F2AF RID: 62127 RVA: 0x0046783B File Offset: 0x00465A3B
		protected override void Tick()
		{
			if (Find.TickManager.TicksGame >= this.completeTick)
			{
				this.Complete();
			}
		}

		// Token: 0x0600F2B0 RID: 62128 RVA: 0x00467858 File Offset: 0x00465A58
		public void InitWith(Pawn revenant)
		{
			this.bioSignature = revenant.TryGetComp<CompRevenant>().biosignature;
			Effecter effecter = EffecterDefOf.RevenantDeath.SpawnMaintained(base.Position, base.Map, 1f);
			this.completeTick = base.TickSpawned + effecter.ticksLeft;
		}

		// Token: 0x0600F2B1 RID: 62129 RVA: 0x004678A8 File Offset: 0x00465AA8
		public void Complete()
		{
			if (FilthMaker.TryMakeFilth(base.PositionHeld, base.Map, ThingDefOf.Filth_RevenantBloodPool, 1, FilthSourceFlags.None, true))
			{
				EffecterDefOf.RevenantKilledCompleteBurst.SpawnMaintained(base.PositionHeld, base.Map, 1f);
				foreach (IntVec3 c in CellRect.CenteredOn(base.PositionHeld, 2))
				{
					Plant plant = c.GetPlant(base.Map);
					if (plant != null && plant.MaxHitPoints < 100)
					{
						plant.Destroy(DestroyMode.Vanish);
					}
				}
			}
			Thing thing = ThingMaker.MakeThing(ThingDefOf.RevenantSpine, null);
			thing.TryGetComp<CompBiosignatureOwner>().biosignature = this.bioSignature;
			GenSpawn.Spawn(thing, base.PositionHeld, base.Map, WipeMode.Vanish);
			this.Destroy(DestroyMode.Vanish);
		}

		// Token: 0x0600F2B2 RID: 62130 RVA: 0x0046798C File Offset: 0x00465B8C
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.completeTick, "completeTick", 0, false);
			Scribe_Values.Look<int>(ref this.bioSignature, "bioSignature", 0, false);
		}

		// Token: 0x04009FE5 RID: 40933
		private int bioSignature;

		// Token: 0x04009FE6 RID: 40934
		private int completeTick;
	}
}
