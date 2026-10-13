using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02003D27 RID: 15655
	public class CaravanArrivalAction_VisitEscapeShip : CaravanArrivalAction
	{
		// Token: 0x1700351F RID: 13599
		// (get) Token: 0x060154F0 RID: 87280 RVA: 0x0065F35D File Offset: 0x0065D55D
		public override string Label
		{
			get
			{
				return "VisitEscapeShip".Translate(this.target.Label);
			}
		}

		// Token: 0x17003520 RID: 13600
		// (get) Token: 0x060154F1 RID: 87281 RVA: 0x0065F37E File Offset: 0x0065D57E
		public override string ReportString
		{
			get
			{
				return "CaravanVisiting".Translate(this.target.Label);
			}
		}

		// Token: 0x060154F2 RID: 87282 RVA: 0x0065EA59 File Offset: 0x0065CC59
		public CaravanArrivalAction_VisitEscapeShip()
		{
		}

		// Token: 0x060154F3 RID: 87283 RVA: 0x0065F39F File Offset: 0x0065D59F
		public CaravanArrivalAction_VisitEscapeShip(EscapeShipComp escapeShip)
		{
			this.target = (MapParent)escapeShip.parent;
		}

		// Token: 0x060154F4 RID: 87284 RVA: 0x0065F3B8 File Offset: 0x0065D5B8
		public override FloatMenuAcceptanceReport StillValid(Caravan caravan, PlanetTile destinationTile)
		{
			FloatMenuAcceptanceReport floatMenuAcceptanceReport = base.StillValid(caravan, destinationTile);
			if (!floatMenuAcceptanceReport)
			{
				return floatMenuAcceptanceReport;
			}
			if (this.target != null && this.target.Tile != destinationTile)
			{
				return false;
			}
			return CaravanArrivalAction_VisitEscapeShip.CanVisit(caravan, this.target);
		}

		// Token: 0x060154F5 RID: 87285 RVA: 0x0065F408 File Offset: 0x0065D608
		public override void Arrived(Caravan caravan)
		{
			if (!this.target.HasMap)
			{
				LongEventHandler.QueueLongEvent(delegate()
				{
					this.DoArrivalAction(caravan);
				}, "GeneratingMapForNewEncounter", false, null, true, false, null);
				return;
			}
			this.DoArrivalAction(caravan);
		}

		// Token: 0x060154F6 RID: 87286 RVA: 0x0065F45E File Offset: 0x0065D65E
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<MapParent>(ref this.target, "target", false);
		}

		// Token: 0x060154F7 RID: 87287 RVA: 0x0065F478 File Offset: 0x0065D678
		private void DoArrivalAction(Caravan caravan)
		{
			bool flag = !this.target.HasMap;
			if (flag)
			{
				this.target.SetFaction(Faction.OfPlayer);
			}
			Map orGenerateMap = GetOrGenerateMapUtility.GetOrGenerateMap(this.target.Tile, null, null);
			LookTargets lookTargets = new LookTargets(caravan.PawnsListForReading);
			CaravanEnterMapUtility.Enter(caravan, orGenerateMap, CaravanEnterMode.Edge, CaravanDropInventoryMode.UnloadIndividually, false, null);
			if (flag)
			{
				Find.TickManager.Notify_GeneratedPotentiallyHostileMap();
				Find.LetterStack.ReceiveLetter("EscapeShipFoundLabel".Translate(), (!Find.Storyteller.difficulty.allowBigThreats) ? "EscapeShipFoundPeaceful".Translate() : "EscapeShipFound".Translate(), LetterDefOf.PositiveEvent, new GlobalTargetInfo(this.target.Map.Center, this.target.Map, false), null, null, null, null, 0, true);
				return;
			}
			Find.LetterStack.ReceiveLetter("LetterLabelCaravanEnteredMap".Translate(this.target), "LetterCaravanEnteredMap".Translate(caravan.Label, this.target).CapitalizeFirst(), LetterDefOf.NeutralEvent, lookTargets, null, null, null, null, 0, true);
		}

		// Token: 0x060154F8 RID: 87288 RVA: 0x0065F59C File Offset: 0x0065D79C
		public static FloatMenuAcceptanceReport CanVisit(Caravan caravan, MapParent escapeShip)
		{
			if (escapeShip == null || !escapeShip.Spawned || escapeShip.GetComponent<EscapeShipComp>() == null)
			{
				return false;
			}
			if (escapeShip.EnterCooldownBlocksEntering())
			{
				return FloatMenuAcceptanceReport.WithFailMessage("MessageEnterCooldownBlocksEntering".Translate(escapeShip.EnterCooldownTicksLeft().ToStringTicksToPeriod(true, false, true, true, false)));
			}
			return true;
		}

		// Token: 0x060154F9 RID: 87289 RVA: 0x0065F5FC File Offset: 0x0065D7FC
		public static IEnumerable<FloatMenuOption> GetFloatMenuOptions(Caravan caravan, MapParent escapeShip)
		{
			return CaravanArrivalActionUtility.GetFloatMenuOptions<CaravanArrivalAction_VisitEscapeShip>(() => CaravanArrivalAction_VisitEscapeShip.CanVisit(caravan, escapeShip), () => new CaravanArrivalAction_VisitEscapeShip(escapeShip.GetComponent<EscapeShipComp>()), "VisitEscapeShip".Translate(escapeShip.Label), caravan, escapeShip.Tile, escapeShip, null);
		}

		// Token: 0x0400ED85 RID: 60805
		private MapParent target;
	}
}
