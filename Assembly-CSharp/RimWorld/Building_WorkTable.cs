using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02002A72 RID: 10866
	public class Building_WorkTable : Building, IBillGiver, IBillGiverWithTickAction
	{
		// Token: 0x170025CF RID: 9679
		// (get) Token: 0x0600F1E1 RID: 61921 RVA: 0x00462AAF File Offset: 0x00460CAF
		public bool CanWorkWithoutPower
		{
			get
			{
				return this.powerComp == null || this.def.building.unpoweredWorkTableWorkSpeedFactor > 0f;
			}
		}

		// Token: 0x170025D0 RID: 9680
		// (get) Token: 0x0600F1E2 RID: 61922 RVA: 0x00462AD5 File Offset: 0x00460CD5
		public bool CanWorkWithoutFuel
		{
			get
			{
				return this.refuelableComp == null;
			}
		}

		// Token: 0x0600F1E3 RID: 61923 RVA: 0x00462AE0 File Offset: 0x00460CE0
		public Building_WorkTable()
		{
			this.billStack = new BillStack(this);
		}

		// Token: 0x0600F1E4 RID: 61924 RVA: 0x00462AF4 File Offset: 0x00460CF4
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Deep.Look<BillStack>(ref this.billStack, "billStack", new object[]
			{
				this
			});
		}

		// Token: 0x0600F1E5 RID: 61925 RVA: 0x00462B18 File Offset: 0x00460D18
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			this.powerComp = base.GetComp<CompPowerTrader>();
			this.refuelableComp = base.GetComp<CompRefuelable>();
			this.breakdownableComp = base.GetComp<CompBreakdownable>();
			this.moteEmitterComp = base.GetComp<CompMoteEmitter>();
			if (base.BeingTransportedOnGravship)
			{
				return;
			}
			foreach (Bill bill in this.billStack)
			{
				bill.ValidateSettings();
			}
		}

		// Token: 0x0600F1E6 RID: 61926 RVA: 0x00462BA4 File Offset: 0x00460DA4
		public virtual void UsedThisTick()
		{
			if (this.refuelableComp != null)
			{
				this.refuelableComp.Notify_UsedThisTick();
			}
			if (this.moteEmitterComp != null)
			{
				if (!this.moteEmitterComp.MoteLive)
				{
					this.moteEmitterComp.Emit();
				}
				this.moteEmitterComp.Maintain();
			}
		}

		// Token: 0x170025D1 RID: 9681
		// (get) Token: 0x0600F1E7 RID: 61927 RVA: 0x00462BE4 File Offset: 0x00460DE4
		public BillStack BillStack
		{
			get
			{
				return this.billStack;
			}
		}

		// Token: 0x170025D2 RID: 9682
		// (get) Token: 0x0600F1E8 RID: 61928 RVA: 0x00462BEC File Offset: 0x00460DEC
		public IntVec3 BillInteractionCell
		{
			get
			{
				return this.InteractionCell;
			}
		}

		// Token: 0x170025D3 RID: 9683
		// (get) Token: 0x0600F1E9 RID: 61929 RVA: 0x00462BF4 File Offset: 0x00460DF4
		public IEnumerable<IntVec3> IngredientStackCells
		{
			get
			{
				return GenAdj.CellsOccupiedBy(this);
			}
		}

		// Token: 0x0600F1EA RID: 61930 RVA: 0x00462BFC File Offset: 0x00460DFC
		public bool CurrentlyUsableForBills()
		{
			return this.UsableForBillsAfterFueling() && (this.CanWorkWithoutPower || (this.powerComp != null && this.powerComp.PowerOn)) && (this.CanWorkWithoutFuel || (this.refuelableComp != null && this.refuelableComp.HasFuel));
		}

		// Token: 0x0600F1EB RID: 61931 RVA: 0x00462C52 File Offset: 0x00460E52
		public bool UsableForBillsAfterFueling()
		{
			return (this.CanWorkWithoutPower || (this.powerComp != null && this.powerComp.PowerOn)) && (this.breakdownableComp == null || !this.breakdownableComp.BrokenDown);
		}

		// Token: 0x0600F1EC RID: 61932 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void Notify_BillDeleted(Bill bill)
		{
		}

		// Token: 0x04009F63 RID: 40803
		public BillStack billStack;

		// Token: 0x04009F64 RID: 40804
		private CompPowerTrader powerComp;

		// Token: 0x04009F65 RID: 40805
		private CompRefuelable refuelableComp;

		// Token: 0x04009F66 RID: 40806
		private CompBreakdownable breakdownableComp;

		// Token: 0x04009F67 RID: 40807
		private CompMoteEmitter moteEmitterComp;
	}
}
