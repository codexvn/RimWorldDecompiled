using System;

namespace Verse
{
	// Token: 0x02000907 RID: 2311
	public abstract class ThingComp_VacuumAware : ThingComp
	{
		// Token: 0x17000B16 RID: 2838
		// (get) Token: 0x06003BEC RID: 15340 RVA: 0x0013A8A7 File Offset: 0x00138AA7
		protected bool InVacuum
		{
			get
			{
				return this.parent.PositionHeld.GetVacuum(this.parent.MapHeld) > 0f;
			}
		}

		// Token: 0x17000B17 RID: 2839
		// (get) Token: 0x06003BED RID: 15341
		protected abstract bool FunctionsInVacuum { get; }

		// Token: 0x06003BEE RID: 15342 RVA: 0x0013A8CC File Offset: 0x00138ACC
		public override string CompInspectStringExtra()
		{
			if (this.InVacuum && !this.FunctionsInVacuum)
			{
				return "CannotFunctionInVacuum".Translate().CapitalizeFirst().Colorize(ColoredText.WarningColor);
			}
			return null;
		}
	}
}
