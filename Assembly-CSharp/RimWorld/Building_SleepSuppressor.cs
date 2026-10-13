using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02002A49 RID: 10825
	public class Building_SleepSuppressor : Building
	{
		// Token: 0x17002567 RID: 9575
		// (get) Token: 0x0600F05E RID: 61534 RVA: 0x0045B51C File Offset: 0x0045971C
		private CompPowerTrader PowerTraderComp
		{
			get
			{
				CompPowerTrader result;
				if ((result = this.powerTraderComp) == null)
				{
					result = (this.powerTraderComp = base.GetComp<CompPowerTrader>());
				}
				return result;
			}
		}

		// Token: 0x17002568 RID: 9576
		// (get) Token: 0x0600F05F RID: 61535 RVA: 0x0045B544 File Offset: 0x00459744
		private CompNoiseSource NoiseSourceComp
		{
			get
			{
				CompNoiseSource result;
				if ((result = this.noiseSourceComp) == null)
				{
					result = (this.noiseSourceComp = base.GetComp<CompNoiseSource>());
				}
				return result;
			}
		}

		// Token: 0x0600F060 RID: 61536 RVA: 0x0045B56A File Offset: 0x0045976A
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			if (!ModLister.CheckAnomaly("Sleep suppressor"))
			{
				this.Destroy(DestroyMode.Vanish);
				return;
			}
			base.SpawnSetup(map, respawningAfterLoad);
		}

		// Token: 0x0600F061 RID: 61537 RVA: 0x0045B588 File Offset: 0x00459788
		protected override void Tick()
		{
			base.Tick();
			if (!this.IsHashIntervalTick(30))
			{
				return;
			}
			if (!this.PowerTraderComp.PowerOn)
			{
				return;
			}
			List<Pawn> allHumanlikeSpawned = base.Map.mapPawns.AllHumanlikeSpawned;
			for (int i = 0; i < allHumanlikeSpawned.Count; i++)
			{
				Pawn pawn = allHumanlikeSpawned[i];
				if (pawn.RaceProps.Humanlike && base.Position.InHorDistOf(pawn.PositionHeld, this.NoiseSourceComp.Props.radius))
				{
					((Hediff_SleepSuppression)pawn.health.GetOrAddHediff(HediffDefOf.SleepSuppression, null, null, null)).lastTickInRangeOfSuppressor = GenTicks.TicksGame;
				}
			}
		}

		// Token: 0x0600F062 RID: 61538 RVA: 0x0045B63C File Offset: 0x0045983C
		public override string GetInspectString()
		{
			string text = base.GetInspectString();
			if (!this.PowerTraderComp.PowerOn)
			{
				return text;
			}
			if (!text.NullOrEmpty())
			{
				text += "\n";
			}
			return text + "SleepSuppressorEffectDescription".Translate().Resolve().CapitalizeFirst();
		}

		// Token: 0x04009EA6 RID: 40614
		private const int UpdateHediffInterval = 30;

		// Token: 0x04009EA7 RID: 40615
		private CompPowerTrader powerTraderComp;

		// Token: 0x04009EA8 RID: 40616
		private CompNoiseSource noiseSourceComp;
	}
}
