using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000717 RID: 1815
	public class HediffComp_VatGrowing : HediffComp
	{
		// Token: 0x170007DF RID: 2015
		// (get) Token: 0x06002C53 RID: 11347 RVA: 0x000E6118 File Offset: 0x000E4318
		public override bool CompShouldRemove
		{
			get
			{
				return base.Pawn.Spawned || !(base.Pawn.ParentHolder is Building_GrowthVat);
			}
		}

		// Token: 0x170007E0 RID: 2016
		// (get) Token: 0x06002C54 RID: 11348 RVA: 0x000E6140 File Offset: 0x000E4340
		public override string CompTipStringExtra
		{
			get
			{
				return "AgingSpeed".Translate() + ": x" + 20.ToString();
			}
		}
	}
}
