using System;
using RimWorld;

namespace Verse
{
	// Token: 0x020006C3 RID: 1731
	public class HediffComp_GiveHemogen : HediffComp
	{
		// Token: 0x1700078F RID: 1935
		// (get) Token: 0x06002B1F RID: 11039 RVA: 0x000E1F18 File Offset: 0x000E0118
		public HediffCompProperties_GiveHemogen Props
		{
			get
			{
				return (HediffCompProperties_GiveHemogen)this.props;
			}
		}

		// Token: 0x06002B20 RID: 11040 RVA: 0x000E1F25 File Offset: 0x000E0125
		public override void CompPostTickInterval(ref float severityAdjustment, int delta)
		{
			GeneUtility.OffsetHemogen(base.Pawn, this.Props.amountPerDay / 60000f * (float)delta, true);
		}
	}
}
