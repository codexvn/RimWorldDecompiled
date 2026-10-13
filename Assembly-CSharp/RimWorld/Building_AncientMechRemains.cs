using System;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x020029CD RID: 10701
	public class Building_AncientMechRemains : Building
	{
		// Token: 0x0600EBDC RID: 60380 RVA: 0x00447EB3 File Offset: 0x004460B3
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			Find.History.Notify_MechanoidDatacoreOppurtunityAvailable();
		}

		// Token: 0x0600EBDD RID: 60381 RVA: 0x00447EC8 File Offset: 0x004460C8
		protected override void Tick()
		{
			base.Tick();
			if (this.effecter == null)
			{
				this.effecter = EffecterDefOf.AncientExostriderRemainsPulse.SpawnAttached(this, base.Map, 1f);
			}
			Effecter effecter = this.effecter;
			if (effecter == null)
			{
				return;
			}
			effecter.EffectTick(this, this);
		}

		// Token: 0x0600EBDE RID: 60382 RVA: 0x00447F1B File Offset: 0x0044611B
		public override void Notify_AbandonedAtTile(PlanetTile tile)
		{
			base.Notify_AbandonedAtTile(tile);
			Find.History.Notify_MechanoidDatacoreReadOrLost();
		}

		// Token: 0x04009C7C RID: 40060
		private Effecter effecter;
	}
}
