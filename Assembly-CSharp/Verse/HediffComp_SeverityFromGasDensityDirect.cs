using System;

namespace Verse
{
	// Token: 0x02000707 RID: 1799
	public class HediffComp_SeverityFromGasDensityDirect : HediffComp
	{
		// Token: 0x170007C3 RID: 1987
		// (get) Token: 0x06002C02 RID: 11266 RVA: 0x000E4EC3 File Offset: 0x000E30C3
		public HediffCompProperties_SeverityFromGasDensityDirect Props
		{
			get
			{
				return (HediffCompProperties_SeverityFromGasDensityDirect)this.props;
			}
		}

		// Token: 0x170007C4 RID: 1988
		// (get) Token: 0x06002C03 RID: 11267 RVA: 0x000E4ED0 File Offset: 0x000E30D0
		public override bool CompShouldRemove
		{
			get
			{
				return base.Pawn.Dead || (!base.Pawn.Spawned && base.Pawn.CarriedBy == null) || !base.Pawn.PositionHeld.AnyGas(base.Pawn.MapHeld, this.Props.gasType);
			}
		}

		// Token: 0x06002C04 RID: 11268 RVA: 0x000E4F2F File Offset: 0x000E312F
		public override void CompPostPostAdd(DamageInfo? dinfo)
		{
			this.UpdateSeverity();
		}

		// Token: 0x06002C05 RID: 11269 RVA: 0x000E4F37 File Offset: 0x000E3137
		public override void CompPostTick(ref float severityAdjustment)
		{
			if (base.Pawn.IsHashIntervalTick(this.Props.intervalTicks))
			{
				this.UpdateSeverity();
			}
		}

		// Token: 0x06002C06 RID: 11270 RVA: 0x000E4F58 File Offset: 0x000E3158
		private void UpdateSeverity()
		{
			if (!base.Pawn.Spawned)
			{
				return;
			}
			if (this.Props.gasType == GasType.ToxGas && !ModsConfig.BiotechActive)
			{
				return;
			}
			float num = base.Pawn.MapHeld.gasGrid.DensityPercentAt(base.Pawn.PositionHeld, this.Props.gasType);
			for (int i = 0; i <= this.Props.densityStages.Count; i++)
			{
				if (num <= this.Props.densityStages[i])
				{
					this.parent.Severity = (float)(i + 1);
					return;
				}
			}
		}
	}
}
