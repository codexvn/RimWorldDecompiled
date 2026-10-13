using System;
using System.Text;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02002A67 RID: 10855
	public abstract class Bill_Mech : Bill_Autonomous
	{
		// Token: 0x170025AF RID: 9647
		// (get) Token: 0x0600F166 RID: 61798 RVA: 0x004605AD File Offset: 0x0045E7AD
		public Pawn BoundPawn
		{
			get
			{
				return this.boundPawn;
			}
		}

		// Token: 0x170025B0 RID: 9648
		// (get) Token: 0x0600F167 RID: 61799 RVA: 0x004605B5 File Offset: 0x0045E7B5
		public int GestationCyclesCompleted
		{
			get
			{
				return this.gestationCycles;
			}
		}

		// Token: 0x170025B1 RID: 9649
		// (get) Token: 0x0600F168 RID: 61800 RVA: 0x004605BD File Offset: 0x0045E7BD
		public int StartedTick
		{
			get
			{
				return this.startedTick;
			}
		}

		// Token: 0x170025B2 RID: 9650
		// (get) Token: 0x0600F169 RID: 61801 RVA: 0x004605C5 File Offset: 0x0045E7C5
		public float WorkSpeedMultiplier
		{
			get
			{
				if (this.recipe.workSpeedStat != null)
				{
					return this.boundPawn.GetStatValue(this.recipe.workSpeedStat, true, -1);
				}
				return 1f;
			}
		}

		// Token: 0x170025B3 RID: 9651
		// (get) Token: 0x0600F16A RID: 61802 RVA: 0x004605F2 File Offset: 0x0045E7F2
		public Building_MechGestator Gestator
		{
			get
			{
				return (Building_MechGestator)this.billStack.billGiver;
			}
		}

		// Token: 0x170025B4 RID: 9652
		// (get) Token: 0x0600F16B RID: 61803
		public abstract float BandwidthCost { get; }

		// Token: 0x170025B5 RID: 9653
		// (get) Token: 0x0600F16C RID: 61804 RVA: 0x00460604 File Offset: 0x0045E804
		protected override string StatusString
		{
			get
			{
				switch (base.State)
				{
				case FormingState.Gathering:
				case FormingState.Preparing:
					if (this.BoundPawn != null)
					{
						return "Worker".Translate() + ": " + this.BoundPawn.LabelShortCap;
					}
					break;
				case FormingState.Forming:
					return "Gestating".Translate();
				case FormingState.Formed:
					if (this.BoundPawn != null)
					{
						return "WaitingFor".Translate() + ": " + this.BoundPawn.LabelShortCap;
					}
					break;
				}
				return null;
			}
		}

		// Token: 0x170025B6 RID: 9654
		// (get) Token: 0x0600F16D RID: 61805 RVA: 0x00356058 File Offset: 0x00354258
		protected override float StatusLineMinHeight
		{
			get
			{
				return 20f;
			}
		}

		// Token: 0x170025B7 RID: 9655
		// (get) Token: 0x0600F16E RID: 61806 RVA: 0x004606A5 File Offset: 0x0045E8A5
		protected override Color BaseColor
		{
			get
			{
				if (this.suspended)
				{
					return base.BaseColor;
				}
				return Color.white;
			}
		}

		// Token: 0x0600F16F RID: 61807 RVA: 0x004606BB File Offset: 0x0045E8BB
		public Bill_Mech()
		{
		}

		// Token: 0x0600F170 RID: 61808 RVA: 0x004606C3 File Offset: 0x0045E8C3
		public Bill_Mech(RecipeDef recipe, Precept_ThingStyle precept = null) : base(recipe, precept)
		{
		}

		// Token: 0x0600F171 RID: 61809 RVA: 0x004606CD File Offset: 0x0045E8CD
		protected override Window GetBillDialog()
		{
			return new Dialog_MechBillConfig(this, ((Thing)this.billStack.billGiver).Position);
		}

		// Token: 0x0600F172 RID: 61810 RVA: 0x004606EC File Offset: 0x0045E8EC
		public override bool ShouldDoNow()
		{
			Pawn pawn = this.BoundPawn;
			if (((pawn != null) ? pawn.mechanitor : null) != null && !this.BoundPawn.mechanitor.HasBandwidthForBill(this))
			{
				JobFailReason.Is("NotEnoughBandwidth".Translate(), null);
				return false;
			}
			return base.ShouldDoNow();
		}

		// Token: 0x0600F173 RID: 61811 RVA: 0x00460740 File Offset: 0x0045E940
		public override bool PawnAllowedToStartAnew(Pawn p)
		{
			if (!ModLister.CheckBiotech("Mech bill"))
			{
				return false;
			}
			if (!base.PawnAllowedToStartAnew(p))
			{
				return false;
			}
			if (this.BoundPawn != null && this.BoundPawn != p)
			{
				JobFailReason.Is("AlreadyAssigned".Translate() + " (" + this.BoundPawn.LabelShort + ")", null);
				return false;
			}
			if (!p.mechanitor.HasBandwidthForBill(this))
			{
				JobFailReason.Is("NotEnoughBandwidth".Translate(), null);
				return false;
			}
			return true;
		}

		// Token: 0x0600F174 RID: 61812 RVA: 0x004607D8 File Offset: 0x0045E9D8
		public override void Notify_DoBillStarted(Pawn billDoer)
		{
			base.Notify_DoBillStarted(billDoer);
			if (this.boundPawn != billDoer)
			{
				this.boundPawn = billDoer;
			}
		}

		// Token: 0x0600F175 RID: 61813 RVA: 0x004607F1 File Offset: 0x0045E9F1
		public override void Reset()
		{
			base.Reset();
			this.gestationCycles = 0;
			this.boundPawn = null;
		}

		// Token: 0x0600F176 RID: 61814 RVA: 0x00460807 File Offset: 0x0045EA07
		public void ForceCompleteAllCycles()
		{
			this.gestationCycles = this.recipe.gestationCycles;
			this.formingTicks = 0f;
		}

		// Token: 0x0600F177 RID: 61815 RVA: 0x00460828 File Offset: 0x0045EA28
		public override void BillTick()
		{
			if (this.suspended || this.state != FormingState.Forming)
			{
				return;
			}
			this.formingTicks -= 1f * this.WorkSpeedMultiplier;
			if (this.formingTicks <= 0f)
			{
				this.gestationCycles++;
				if (this.gestationCycles >= this.recipe.gestationCycles)
				{
					this.state = FormingState.Formed;
					this.Gestator.Notify_FormingCompleted();
					return;
				}
				this.formingTicks = (float)this.recipe.formingTicks;
				this.state = FormingState.Preparing;
			}
		}

		// Token: 0x0600F178 RID: 61816 RVA: 0x004608BC File Offset: 0x0045EABC
		public override void AppendInspectionData(StringBuilder sb)
		{
			if (base.State == FormingState.Forming || base.State == FormingState.Preparing)
			{
				sb.AppendLine("CurrentGestationCycle".Translate() + ": " + ((int)(this.formingTicks * (1f / this.WorkSpeedMultiplier))).ToStringTicksToPeriod(true, false, true, true, false));
				sb.AppendLine("RemainingGestationCycles".Translate() + ": " + (this.recipe.gestationCycles - this.GestationCyclesCompleted).ToString() + " (" + "OfLower".Translate() + " " + this.recipe.gestationCycles.ToString() + ")");
			}
		}

		// Token: 0x0600F179 RID: 61817 RVA: 0x0046099D File Offset: 0x0045EB9D
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Pawn>(ref this.boundPawn, "boundPawn", false);
			Scribe_Values.Look<int>(ref this.gestationCycles, "gestationCycles", 0, false);
		}

		// Token: 0x04009F37 RID: 40759
		private const float StatusStringLineHeight = 20f;

		// Token: 0x04009F38 RID: 40760
		private Pawn boundPawn;

		// Token: 0x04009F39 RID: 40761
		private int gestationCycles;
	}
}
