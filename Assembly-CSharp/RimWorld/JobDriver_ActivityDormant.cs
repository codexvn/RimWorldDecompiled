using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020012B0 RID: 4784
	public class JobDriver_ActivityDormant : JobDriver
	{
		// Token: 0x06007536 RID: 30006 RVA: 0x0023CF9B File Offset: 0x0023B19B
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return ModLister.AnomalyInstalled;
		}

		// Token: 0x06007537 RID: 30007 RVA: 0x0023CFA2 File Offset: 0x0023B1A2
		protected override IEnumerable<Toil> MakeNewToils()
		{
			yield return Toils_LayDown.ActivityDormant();
			yield break;
		}
	}
}
