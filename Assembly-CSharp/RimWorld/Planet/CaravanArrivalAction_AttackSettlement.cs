using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02003D1E RID: 15646
	public class CaravanArrivalAction_AttackSettlement : CaravanArrivalAction
	{
		// Token: 0x17003517 RID: 13591
		// (get) Token: 0x060154BB RID: 87227 RVA: 0x0065EA17 File Offset: 0x0065CC17
		public override string Label
		{
			get
			{
				return "AttackSettlement".Translate(this.settlement.Label);
			}
		}

		// Token: 0x17003518 RID: 13592
		// (get) Token: 0x060154BC RID: 87228 RVA: 0x0065EA38 File Offset: 0x0065CC38
		public override string ReportString
		{
			get
			{
				return "CaravanAttacking".Translate(this.settlement.Label);
			}
		}

		// Token: 0x060154BD RID: 87229 RVA: 0x0065EA59 File Offset: 0x0065CC59
		public CaravanArrivalAction_AttackSettlement()
		{
		}

		// Token: 0x060154BE RID: 87230 RVA: 0x0065EA61 File Offset: 0x0065CC61
		public CaravanArrivalAction_AttackSettlement(Settlement settlement)
		{
			this.settlement = settlement;
		}

		// Token: 0x060154BF RID: 87231 RVA: 0x0065EA70 File Offset: 0x0065CC70
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
			return CaravanArrivalAction_AttackSettlement.CanAttack(caravan, this.settlement);
		}

		// Token: 0x060154C0 RID: 87232 RVA: 0x0065EABE File Offset: 0x0065CCBE
		public override void Arrived(Caravan caravan)
		{
			SettlementUtility.Attack(caravan, this.settlement);
		}

		// Token: 0x060154C1 RID: 87233 RVA: 0x0065EACC File Offset: 0x0065CCCC
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Settlement>(ref this.settlement, "settlement", false);
		}

		// Token: 0x060154C2 RID: 87234 RVA: 0x0065EAE8 File Offset: 0x0065CCE8
		public static FloatMenuAcceptanceReport CanAttack(Caravan caravan, Settlement settlement)
		{
			if (settlement == null || !settlement.Spawned || !settlement.Attackable)
			{
				return false;
			}
			if (settlement.EnterCooldownBlocksEntering())
			{
				return FloatMenuAcceptanceReport.WithFailMessage("MessageEnterCooldownBlocksEntering".Translate(settlement.EnterCooldownTicksLeft().ToStringTicksToPeriod(true, false, true, true, false)));
			}
			return true;
		}

		// Token: 0x060154C3 RID: 87235 RVA: 0x0065EB48 File Offset: 0x0065CD48
		public static IEnumerable<FloatMenuOption> GetFloatMenuOptions(Caravan caravan, Settlement settlement)
		{
			return CaravanArrivalActionUtility.GetFloatMenuOptions<CaravanArrivalAction_AttackSettlement>(() => CaravanArrivalAction_AttackSettlement.CanAttack(caravan, settlement), () => new CaravanArrivalAction_AttackSettlement(settlement), "AttackSettlement".Translate(settlement.Label), caravan, settlement.Tile, settlement, settlement.Faction.AllyOrNeutralTo(Faction.OfPlayer) ? delegate(Action action)
			{
				Find.WindowStack.Add(Dialog_MessageBox.CreateConfirmation("ConfirmAttackFriendlyFaction".Translate(settlement.LabelCap, settlement.Faction.Name), delegate()
				{
					action();
				}, false, null, WindowLayer.Dialog));
			} : null);
		}

		// Token: 0x0400ED78 RID: 60792
		private Settlement settlement;
	}
}
