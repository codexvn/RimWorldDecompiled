using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02003E03 RID: 15875
	public class TransportersArrivalAction_AttackSettlement : TransportersArrivalAction
	{
		// Token: 0x170035F4 RID: 13812
		// (get) Token: 0x06015A7C RID: 88700 RVA: 0x000028E7 File Offset: 0x00000AE7
		public override bool GeneratesMap
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06015A7D RID: 88701 RVA: 0x0067B2C7 File Offset: 0x006794C7
		public TransportersArrivalAction_AttackSettlement()
		{
		}

		// Token: 0x06015A7E RID: 88702 RVA: 0x0067B2CF File Offset: 0x006794CF
		public TransportersArrivalAction_AttackSettlement(Settlement settlement, PawnsArrivalModeDef arrivalMode)
		{
			this.settlement = settlement;
			this.arrivalMode = arrivalMode;
		}

		// Token: 0x06015A7F RID: 88703 RVA: 0x0067B2E5 File Offset: 0x006794E5
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Settlement>(ref this.settlement, "settlement", false);
			Scribe_Defs.Look<PawnsArrivalModeDef>(ref this.arrivalMode, "arrivalMode");
		}

		// Token: 0x06015A80 RID: 88704 RVA: 0x0067B310 File Offset: 0x00679510
		public override FloatMenuAcceptanceReport StillValid(IEnumerable<IThingHolder> pods, PlanetTile destinationTile)
		{
			FloatMenuAcceptanceReport floatMenuAcceptanceReport = base.StillValid(pods, destinationTile);
			if (!floatMenuAcceptanceReport)
			{
				return floatMenuAcceptanceReport;
			}
			if (this.settlement != null && this.settlement.Tile != destinationTile)
			{
				return false;
			}
			return TransportersArrivalAction_AttackSettlement.CanAttack(pods, this.settlement);
		}

		// Token: 0x06015A81 RID: 88705 RVA: 0x0067B35E File Offset: 0x0067955E
		public override bool ShouldUseLongEvent(List<ActiveTransporterInfo> pods, PlanetTile tile)
		{
			return !this.settlement.HasMap;
		}

		// Token: 0x06015A82 RID: 88706 RVA: 0x0067B370 File Offset: 0x00679570
		public override void Arrived(List<ActiveTransporterInfo> transporters, PlanetTile tile)
		{
			Thing lookTarget = TransportersArrivalActionUtility.GetLookTarget(transporters);
			bool flag = !this.settlement.HasMap;
			Map orGenerateMap = GetOrGenerateMapUtility.GetOrGenerateMap(this.settlement.Tile, null, null);
			TaggedString label = "LetterLabelCaravanEnteredEnemyBase".Translate();
			TaggedString text = "LetterTransportPodsLandedInEnemyBase".Translate(this.settlement.Label).CapitalizeFirst();
			SettlementUtility.AffectRelationsOnAttacked(this.settlement, ref text);
			if (flag)
			{
				Find.TickManager.Notify_GeneratedPotentiallyHostileMap();
				PawnRelationUtility.Notify_PawnsSeenByPlayer_Letter(orGenerateMap.mapPawns.AllPawns, ref label, ref text, "LetterRelatedPawnsInMapWherePlayerLanded".Translate(Faction.OfPlayer.def.pawnsPlural), true, true);
			}
			Find.LetterStack.ReceiveLetter(label, text, LetterDefOf.NeutralEvent, lookTarget, this.settlement.Faction, null, null, null, 0, true);
			this.arrivalMode.Worker.TravellingTransportersArrived(transporters, orGenerateMap);
		}

		// Token: 0x06015A83 RID: 88707 RVA: 0x0067B460 File Offset: 0x00679660
		public static FloatMenuAcceptanceReport CanAttack(IEnumerable<IThingHolder> pods, Settlement settlement)
		{
			if (settlement == null || !settlement.Spawned || !settlement.Attackable)
			{
				return false;
			}
			if (!TransportersArrivalActionUtility.AnyNonDownedColonist(pods))
			{
				return false;
			}
			if (settlement.EnterCooldownBlocksEntering())
			{
				return FloatMenuAcceptanceReport.WithFailReasonAndMessage("EnterCooldownBlocksEntering".Translate(), "MessageEnterCooldownBlocksEntering".Translate(settlement.EnterCooldownTicksLeft().ToStringTicksToPeriod(true, false, true, true, false)));
			}
			return true;
		}

		// Token: 0x06015A84 RID: 88708 RVA: 0x0067B4DD File Offset: 0x006796DD
		public static IEnumerable<FloatMenuOption> GetFloatMenuOptions(Action<PlanetTile, TransportersArrivalAction> launchAction, IEnumerable<IThingHolder> pods, Settlement settlement)
		{
			Func<FloatMenuAcceptanceReport> <>9__0;
			Func<FloatMenuAcceptanceReport> acceptanceReportGetter;
			if ((acceptanceReportGetter = <>9__0) == null)
			{
				acceptanceReportGetter = (<>9__0 = (() => TransportersArrivalAction_AttackSettlement.CanAttack(pods, settlement)));
			}
			Func<TransportersArrivalAction_AttackSettlement> <>9__1;
			Func<TransportersArrivalAction_AttackSettlement> arrivalActionGetter;
			if ((arrivalActionGetter = <>9__1) == null)
			{
				arrivalActionGetter = (<>9__1 = (() => new TransportersArrivalAction_AttackSettlement(settlement, PawnsArrivalModeDefOf.EdgeDrop)));
			}
			foreach (FloatMenuOption floatMenuOption in TransportersArrivalActionUtility.GetFloatMenuOptions<TransportersArrivalAction_AttackSettlement>(acceptanceReportGetter, arrivalActionGetter, "AttackAndDropAtEdge".Translate(settlement.Label), launchAction, settlement.Tile, null))
			{
				yield return floatMenuOption;
			}
			IEnumerator<FloatMenuOption> enumerator = null;
			Func<FloatMenuAcceptanceReport> <>9__2;
			Func<FloatMenuAcceptanceReport> acceptanceReportGetter2;
			if ((acceptanceReportGetter2 = <>9__2) == null)
			{
				acceptanceReportGetter2 = (<>9__2 = (() => TransportersArrivalAction_AttackSettlement.CanAttack(pods, settlement)));
			}
			Func<TransportersArrivalAction_AttackSettlement> <>9__3;
			Func<TransportersArrivalAction_AttackSettlement> arrivalActionGetter2;
			if ((arrivalActionGetter2 = <>9__3) == null)
			{
				arrivalActionGetter2 = (<>9__3 = (() => new TransportersArrivalAction_AttackSettlement(settlement, PawnsArrivalModeDefOf.CenterDrop)));
			}
			foreach (FloatMenuOption floatMenuOption2 in TransportersArrivalActionUtility.GetFloatMenuOptions<TransportersArrivalAction_AttackSettlement>(acceptanceReportGetter2, arrivalActionGetter2, "AttackAndDropInCenter".Translate(settlement.Label), launchAction, settlement.Tile, null))
			{
				yield return floatMenuOption2;
			}
			enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x0400F099 RID: 61593
		private Settlement settlement;

		// Token: 0x0400F09A RID: 61594
		private PawnsArrivalModeDef arrivalMode;
	}
}
