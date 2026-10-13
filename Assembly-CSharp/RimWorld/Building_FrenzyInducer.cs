using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02002A00 RID: 10752
	public class Building_FrenzyInducer : Building
	{
		// Token: 0x170024CE RID: 9422
		// (get) Token: 0x0600EDBB RID: 60859 RVA: 0x0044F710 File Offset: 0x0044D910
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

		// Token: 0x0600EDBC RID: 60860 RVA: 0x0044F736 File Offset: 0x0044D936
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			if (!ModLister.CheckAnomaly("Frenzy inducer"))
			{
				this.Destroy(DestroyMode.Vanish);
				return;
			}
			base.SpawnSetup(map, respawningAfterLoad);
		}

		// Token: 0x0600EDBD RID: 60861 RVA: 0x0044F754 File Offset: 0x0044D954
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
				if (pawn.RaceProps.Humanlike && base.Position.InHorDistOf(pawn.PositionHeld, 8.9f))
				{
					((Hediff_FrenzyField)pawn.health.GetOrAddHediff(HediffDefOf.FrenzyField, null, null, null)).lastTickInRangeOfInducer = GenTicks.TicksGame;
				}
			}
		}

		// Token: 0x0600EDBE RID: 60862 RVA: 0x0044F800 File Offset: 0x0044DA00
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
			return text + "FrenzyInducerEffectDescription".Translate().Resolve().CapitalizeFirst();
		}

		// Token: 0x04009D4D RID: 40269
		private const int UpdateHediffInterval = 30;

		// Token: 0x04009D4E RID: 40270
		private const float Radius = 8.9f;

		// Token: 0x04009D4F RID: 40271
		private CompPowerTrader powerTraderComp;
	}
}
