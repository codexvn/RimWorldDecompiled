using System;
using System.Collections.Generic;

namespace Verse.AI
{
	// Token: 0x02000E4C RID: 3660
	public class JobDriver_BabyPassive : JobDriver
	{
		// Token: 0x06005C35 RID: 23605 RVA: 0x000028E7 File Offset: 0x00000AE7
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return true;
		}

		// Token: 0x06005C36 RID: 23606 RVA: 0x001D7BAF File Offset: 0x001D5DAF
		protected override IEnumerable<Toil> MakeNewToils()
		{
			yield return JobDriver_BabyPassive.Passive();
			yield break;
		}

		// Token: 0x06005C37 RID: 23607 RVA: 0x001D7BB8 File Offset: 0x001D5DB8
		private static Toil Passive()
		{
			Toil toil = ToilMaker.MakeToil("Passive");
			toil.defaultCompleteMode = ToilCompleteMode.Never;
			return toil;
		}
	}
}
