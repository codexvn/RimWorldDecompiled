using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002D77 RID: 11639
	public class CompNoiseSource : ThingComp
	{
		// Token: 0x170029A2 RID: 10658
		// (get) Token: 0x060105B0 RID: 66992 RVA: 0x004B890F File Offset: 0x004B6B0F
		public CompProperties_NoiseSource Props
		{
			get
			{
				return (CompProperties_NoiseSource)this.props;
			}
		}

		// Token: 0x170029A3 RID: 10659
		// (get) Token: 0x060105B1 RID: 66993 RVA: 0x004B891C File Offset: 0x004B6B1C
		public bool Active
		{
			get
			{
				return this.PowerComp == null || (this.powerComp.PowerOn && this.PowerComp.PowerOutput < 0f) || this.powerComp.PowerOutput > 0f;
			}
		}

		// Token: 0x170029A4 RID: 10660
		// (get) Token: 0x060105B2 RID: 66994 RVA: 0x004B895C File Offset: 0x004B6B5C
		private CompPowerTrader PowerComp
		{
			get
			{
				CompPowerTrader result;
				if ((result = this.powerComp) == null)
				{
					result = (this.powerComp = this.parent.GetComp<CompPowerTrader>());
				}
				return result;
			}
		}

		// Token: 0x060105B3 RID: 66995 RVA: 0x004B8987 File Offset: 0x004B6B87
		public override void PostDrawExtraSelectionOverlays()
		{
			GenDraw.DrawRadiusRing(this.parent.Position, this.Props.radius);
		}

		// Token: 0x0400AAAD RID: 43693
		private CompPowerTrader powerComp;
	}
}
