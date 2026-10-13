using System;

namespace Verse
{
	// Token: 0x02000779 RID: 1913
	public class HediffGiver_Event : HediffGiver
	{
		// Token: 0x06002EE4 RID: 12004 RVA: 0x000F0F89 File Offset: 0x000EF189
		public bool EventOccurred(Pawn pawn)
		{
			return Rand.Value < this.chance && base.TryApply(pawn, null);
		}

		// Token: 0x040024AA RID: 9386
		private float chance = 1f;
	}
}
