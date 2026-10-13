using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200213B RID: 8507
	public class CompPowerPlantSteam : CompPowerPlant
	{
		// Token: 0x0600B9AE RID: 47534 RVA: 0x00359217 File Offset: 0x00357417
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			base.PostSpawnSetup(respawningAfterLoad);
			this.steamSprayer = new IntermittentSteamSprayer(this.parent);
		}

		// Token: 0x0600B9AF RID: 47535 RVA: 0x00359234 File Offset: 0x00357434
		public override void CompTick()
		{
			base.CompTick();
			if (this.geyser == null)
			{
				this.geyser = (Building_SteamGeyser)this.parent.Map.thingGrid.ThingAt(this.parent.Position, ThingDefOf.SteamGeyser);
			}
			if (this.geyser != null)
			{
				this.geyser.harvester = (Building)this.parent;
				this.steamSprayer.SteamSprayerTick();
			}
		}

		// Token: 0x0600B9B0 RID: 47536 RVA: 0x003592A8 File Offset: 0x003574A8
		public override void PostDeSpawn(Map map, DestroyMode mode = DestroyMode.Vanish)
		{
			base.PostDeSpawn(map, mode);
			if (this.geyser != null)
			{
				this.geyser.harvester = null;
			}
		}

		// Token: 0x04007FF9 RID: 32761
		private IntermittentSteamSprayer steamSprayer;

		// Token: 0x04007FFA RID: 32762
		private Building_SteamGeyser geyser;
	}
}
