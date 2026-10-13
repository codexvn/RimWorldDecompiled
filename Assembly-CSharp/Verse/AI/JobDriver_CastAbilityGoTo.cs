using System;
using System.Collections.Generic;

namespace Verse.AI
{
	// Token: 0x02000E9C RID: 3740
	public class JobDriver_CastAbilityGoTo : JobDriver_CastVerbOnce
	{
		// Token: 0x06005E43 RID: 24131 RVA: 0x001E0293 File Offset: 0x001DE493
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedOrNull(TargetIndex.A);
			yield return Toils_Goto.Goto(TargetIndex.B, PathEndMode.OnCell);
			yield return Toils_Combat.CastVerb(TargetIndex.A, TargetIndex.B, false);
			yield break;
		}
	}
}
