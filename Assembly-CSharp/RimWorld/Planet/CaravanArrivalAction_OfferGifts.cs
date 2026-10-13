using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02003D23 RID: 15651
	public class CaravanArrivalAction_OfferGifts : CaravanArrivalAction
	{
		// Token: 0x1700351B RID: 13595
		// (get) Token: 0x060154D6 RID: 87254 RVA: 0x0065EEFD File Offset: 0x0065D0FD
		public override string Label
		{
			get
			{
				return "OfferGifts".Translate();
			}
		}

		// Token: 0x1700351C RID: 13596
		// (get) Token: 0x060154D7 RID: 87255 RVA: 0x0065EF0E File Offset: 0x0065D10E
		public override string ReportString
		{
			get
			{
				return "CaravanOfferingGifts".Translate(this.settlement.Label);
			}
		}

		// Token: 0x060154D8 RID: 87256 RVA: 0x0065EA59 File Offset: 0x0065CC59
		public CaravanArrivalAction_OfferGifts()
		{
		}

		// Token: 0x060154D9 RID: 87257 RVA: 0x0065EF2F File Offset: 0x0065D12F
		public CaravanArrivalAction_OfferGifts(Settlement settlement)
		{
			this.settlement = settlement;
		}

		// Token: 0x060154DA RID: 87258 RVA: 0x0065EF40 File Offset: 0x0065D140
		public override FloatMenuAcceptanceReport StillValid(Caravan caravan, PlanetTile destinationTile)
		{
			FloatMenuAcceptanceReport floatMenuAcceptanceReport = base.StillValid(caravan, destinationTile);
			if (!floatMenuAcceptanceReport)
			{
				return floatMenuAcceptanceReport;
			}
			if (this.settlement != null && this.settlement.Tile != destinationTile)
			{
				return false;
			}
			return CaravanArrivalAction_OfferGifts.CanOfferGiftsTo(caravan, this.settlement);
		}

		// Token: 0x060154DB RID: 87259 RVA: 0x0065EF90 File Offset: 0x0065D190
		public override void Arrived(Caravan caravan)
		{
			CameraJumper.TryJumpAndSelect(caravan, CameraJumper.MovementMode.Pan);
			Pawn playerNegotiator = BestCaravanPawnUtility.FindBestNegotiator(caravan, null, null);
			Find.WindowStack.Add(new Dialog_Trade(playerNegotiator, this.settlement, true));
		}

		// Token: 0x060154DC RID: 87260 RVA: 0x0065EFC9 File Offset: 0x0065D1C9
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Settlement>(ref this.settlement, "settlement", false);
		}

		// Token: 0x060154DD RID: 87261 RVA: 0x0065EFE4 File Offset: 0x0065D1E4
		public static FloatMenuAcceptanceReport CanOfferGiftsTo(Caravan caravan, Settlement settlement)
		{
			return settlement != null && settlement.Spawned && !settlement.HasMap && settlement.Faction != null && settlement.Faction != Faction.OfPlayer && !settlement.Faction.def.permanentEnemy && settlement.Faction.HostileTo(Faction.OfPlayer) && settlement.CanTradeNow && CaravanArrivalAction_OfferGifts.HasNegotiator(caravan);
		}

		// Token: 0x060154DE RID: 87262 RVA: 0x0065F054 File Offset: 0x0065D254
		private static bool HasNegotiator(Caravan caravan)
		{
			Pawn pawn = BestCaravanPawnUtility.FindBestNegotiator(caravan, null, null);
			return pawn != null && !pawn.skills.GetSkill(SkillDefOf.Social).TotallyDisabled;
		}

		// Token: 0x060154DF RID: 87263 RVA: 0x0065F088 File Offset: 0x0065D288
		public static IEnumerable<FloatMenuOption> GetFloatMenuOptions(Caravan caravan, Settlement settlement)
		{
			return CaravanArrivalActionUtility.GetFloatMenuOptions<CaravanArrivalAction_OfferGifts>(() => CaravanArrivalAction_OfferGifts.CanOfferGiftsTo(caravan, settlement), () => new CaravanArrivalAction_OfferGifts(settlement), "OfferGifts".Translate(), caravan, settlement.Tile, settlement, null);
		}

		// Token: 0x0400ED7F RID: 60799
		private Settlement settlement;
	}
}
