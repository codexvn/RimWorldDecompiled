using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002CAC RID: 11436
	public class CompGasOnDestroyed : ThingComp
	{
		// Token: 0x17002893 RID: 10387
		// (get) Token: 0x060100D4 RID: 65748 RVA: 0x004A59E4 File Offset: 0x004A3BE4
		private CompProperties_GasOnDestroyed Props
		{
			get
			{
				return (CompProperties_GasOnDestroyed)this.props;
			}
		}

		// Token: 0x060100D5 RID: 65749 RVA: 0x004A59F1 File Offset: 0x004A3BF1
		public override void PostDestroy(DestroyMode mode, Map previousMap)
		{
			base.PostDestroy(mode, previousMap);
			if (mode != DestroyMode.Vanish)
			{
				GasUtility.AddGas(this.parent.PositionHeld, previousMap, this.Props.gasType, this.Props.amount);
			}
		}
	}
}
