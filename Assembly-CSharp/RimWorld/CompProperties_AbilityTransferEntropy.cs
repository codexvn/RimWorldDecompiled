using System;

namespace RimWorld
{
	// Token: 0x02002299 RID: 8857
	public class CompProperties_AbilityTransferEntropy : CompProperties_AbilityEffect
	{
		// Token: 0x0600C0C9 RID: 49353 RVA: 0x0037D3B3 File Offset: 0x0037B5B3
		public CompProperties_AbilityTransferEntropy()
		{
			this.compClass = typeof(CompAbilityEffect_TransferEntropy);
		}

		// Token: 0x040083F0 RID: 33776
		public bool targetReceivesEntropy = true;
	}
}
