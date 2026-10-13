using System;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02002A4A RID: 10826
	public class Building_SteamGeyser : Building
	{
		// Token: 0x0600F064 RID: 61540 RVA: 0x0045B692 File Offset: 0x00459892
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			this.steamSprayer = new IntermittentSteamSprayer(this)
			{
				startSprayCallback = new Action(this.StartSpray),
				endSprayCallback = new Action(this.EndSpray)
			};
		}

		// Token: 0x0600F065 RID: 61541 RVA: 0x0045B6CC File Offset: 0x004598CC
		private void StartSpray()
		{
			WeatherBuildupUtility.AddSnowRadial(this.OccupiedRect().RandomCell, base.Map, 4f, -0.06f);
			this.spraySustainer = SoundDefOf.GeyserSpray.TrySpawnSustainer(new TargetInfo(base.Position, base.Map, false));
			this.spraySustainerStartTick = Find.TickManager.TicksGame;
		}

		// Token: 0x0600F066 RID: 61542 RVA: 0x0045B733 File Offset: 0x00459933
		private void EndSpray()
		{
			if (this.spraySustainer != null)
			{
				this.spraySustainer.End();
				this.spraySustainer = null;
			}
		}

		// Token: 0x0600F067 RID: 61543 RVA: 0x0045B750 File Offset: 0x00459950
		protected override void Tick()
		{
			if (this.harvester == null)
			{
				this.steamSprayer.SteamSprayerTick();
			}
			if (this.spraySustainer != null && Find.TickManager.TicksGame > this.spraySustainerStartTick + 1000)
			{
				Log.Message("Geyser spray sustainer still playing after 1000 ticks. Force-ending.");
				this.spraySustainer.End();
				this.spraySustainer = null;
			}
		}

		// Token: 0x04009EA9 RID: 40617
		private IntermittentSteamSprayer steamSprayer;

		// Token: 0x04009EAA RID: 40618
		public Building harvester;

		// Token: 0x04009EAB RID: 40619
		private Sustainer spraySustainer;

		// Token: 0x04009EAC RID: 40620
		private int spraySustainerStartTick = -999;
	}
}
