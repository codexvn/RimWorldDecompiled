using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002ADC RID: 10972
	public class FloorEtchingRambling : Building
	{
		// Token: 0x0600F47F RID: 62591 RVA: 0x00470CD4 File Offset: 0x0046EED4
		public override void PostMake()
		{
			base.PostMake();
			this.comp = base.GetComp<CompFloorEtchingRambling>();
		}

		// Token: 0x17002635 RID: 9781
		// (get) Token: 0x0600F480 RID: 62592 RVA: 0x00470CE8 File Offset: 0x0046EEE8
		public override string DescriptionFlavor
		{
			get
			{
				if (this.comp.deciphered)
				{
					return this.comp.message;
				}
				return base.DescriptionFlavor;
			}
		}

		// Token: 0x0400A11E RID: 41246
		private CompFloorEtchingRambling comp;
	}
}
