using System;

namespace RimWorld
{
	// Token: 0x02001A25 RID: 6693
	public struct PawnGenOptionWithXenotype
	{
		// Token: 0x1700170C RID: 5900
		// (get) Token: 0x060096E0 RID: 38624 RVA: 0x002C1B6A File Offset: 0x002BFD6A
		public PawnGenOption Option
		{
			get
			{
				return this.option;
			}
		}

		// Token: 0x1700170D RID: 5901
		// (get) Token: 0x060096E1 RID: 38625 RVA: 0x002C1B72 File Offset: 0x002BFD72
		public XenotypeDef Xenotype
		{
			get
			{
				return this.xenotype;
			}
		}

		// Token: 0x1700170E RID: 5902
		// (get) Token: 0x060096E2 RID: 38626 RVA: 0x002C1B7A File Offset: 0x002BFD7A
		public float SelectionWeight
		{
			get
			{
				return this.selectionWeight;
			}
		}

		// Token: 0x060096E3 RID: 38627 RVA: 0x002C1B82 File Offset: 0x002BFD82
		public PawnGenOptionWithXenotype(PawnGenOption option, XenotypeDef xenotype, float selectionWeight)
		{
			this.option = option;
			this.xenotype = xenotype;
			this.selectionWeight = selectionWeight;
		}

		// Token: 0x1700170F RID: 5903
		// (get) Token: 0x060096E4 RID: 38628 RVA: 0x002C1B99 File Offset: 0x002BFD99
		public float Cost
		{
			get
			{
				if (this.xenotype == null)
				{
					return this.option.Cost;
				}
				return this.option.Cost * this.xenotype.combatPowerFactor;
			}
		}

		// Token: 0x04006366 RID: 25446
		private PawnGenOption option;

		// Token: 0x04006367 RID: 25447
		private XenotypeDef xenotype;

		// Token: 0x04006368 RID: 25448
		private float selectionWeight;
	}
}
