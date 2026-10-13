using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000704 RID: 1796
	public class HediffComp_SeverityFromGas : HediffComp
	{
		// Token: 0x170007C0 RID: 1984
		// (get) Token: 0x06002BF3 RID: 11251 RVA: 0x000E4B61 File Offset: 0x000E2D61
		public HediffCompProperties_SeverityFromGas Props
		{
			get
			{
				return (HediffCompProperties_SeverityFromGas)this.props;
			}
		}

		// Token: 0x06002BF4 RID: 11252 RVA: 0x000E4B70 File Offset: 0x000E2D70
		public override void CompPostTick(ref float severityAdjustment)
		{
			if (this.Props.gasType == GasType.ToxGas && !ModsConfig.BiotechActive)
			{
				return;
			}
			Pawn pawn = this.parent.pawn;
			if (!pawn.Spawned || !pawn.IsHashIntervalTick(this.Props.intervalTicks))
			{
				return;
			}
			if (pawn.Position.AnyGas(pawn.Map, this.Props.gasType))
			{
				float num = (float)pawn.Position.GasDensity(pawn.Map, this.Props.gasType) / 255f * this.Props.severityGasDensityFactor;
				if (this.Props.exposureStatFactor != null)
				{
					num *= 1f - pawn.GetStatValue(this.Props.exposureStatFactor, true, -1);
				}
				severityAdjustment += num;
				return;
			}
			severityAdjustment += this.Props.severityNotExposed;
		}
	}
}
