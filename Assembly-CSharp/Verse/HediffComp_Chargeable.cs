using System;

namespace Verse
{
	// Token: 0x0200069A RID: 1690
	[StaticConstructorOnStartup]
	public class HediffComp_Chargeable : HediffComp
	{
		// Token: 0x17000766 RID: 1894
		// (get) Token: 0x06002A84 RID: 10884 RVA: 0x000DFB81 File Offset: 0x000DDD81
		public HediffCompProperties_Chargeable Props
		{
			get
			{
				return (HediffCompProperties_Chargeable)this.props;
			}
		}

		// Token: 0x17000767 RID: 1895
		// (get) Token: 0x06002A85 RID: 10885 RVA: 0x000DFB8E File Offset: 0x000DDD8E
		// (set) Token: 0x06002A86 RID: 10886 RVA: 0x000DFB96 File Offset: 0x000DDD96
		public float Charge
		{
			get
			{
				return this.charge;
			}
			protected set
			{
				this.charge = value;
				if (this.charge > this.Props.fullChargeAmount)
				{
					this.charge = this.Props.fullChargeAmount;
				}
			}
		}

		// Token: 0x17000768 RID: 1896
		// (get) Token: 0x06002A87 RID: 10887 RVA: 0x000DFBC3 File Offset: 0x000DDDC3
		public bool CanActivate
		{
			get
			{
				return this.charge >= this.Props.minChargeToActivate;
			}
		}

		// Token: 0x17000769 RID: 1897
		// (get) Token: 0x06002A88 RID: 10888 RVA: 0x000DFBDC File Offset: 0x000DDDDC
		public override string CompLabelInBracketsExtra
		{
			get
			{
				return this.Props.labelInBrackets.Formatted(this.charge.Named("CHARGE"), (this.charge / this.Props.fullChargeAmount).Named("CHARGEFACTOR"));
			}
		}

		// Token: 0x06002A89 RID: 10889 RVA: 0x000DFC34 File Offset: 0x000DDE34
		public override void CompPostMake()
		{
			base.CompPostMake();
			this.charge = this.Props.initialCharge;
		}

		// Token: 0x06002A8A RID: 10890 RVA: 0x000DFC4D File Offset: 0x000DDE4D
		public override void CompExposeData()
		{
			base.CompExposeData();
			Scribe_Values.Look<float>(ref this.charge, "charge", this.Props.initialCharge, false);
		}

		// Token: 0x06002A8B RID: 10891 RVA: 0x000DFC71 File Offset: 0x000DDE71
		public virtual void TryCharge(float desiredChargeAmount)
		{
			this.Charge += desiredChargeAmount;
		}

		// Token: 0x06002A8C RID: 10892 RVA: 0x000DFC84 File Offset: 0x000DDE84
		public override void CompPostTickInterval(ref float severityAdjustment, int delta)
		{
			base.CompPostTickInterval(ref severityAdjustment, delta);
			if (this.Props.ticksToFullCharge > 0)
			{
				this.TryCharge(this.Props.fullChargeAmount / (float)this.Props.ticksToFullCharge * (float)delta);
				return;
			}
			if (this.Props.ticksToFullCharge == 0)
			{
				this.TryCharge(this.Props.fullChargeAmount);
			}
		}

		// Token: 0x06002A8D RID: 10893 RVA: 0x000DFCE8 File Offset: 0x000DDEE8
		public float GreedyConsume(float desiredCharge)
		{
			float num;
			if (desiredCharge >= this.charge)
			{
				num = this.charge;
				this.charge = 0f;
			}
			else
			{
				num = desiredCharge;
				this.charge -= num;
			}
			return num;
		}

		// Token: 0x04002278 RID: 8824
		private float charge;
	}
}
