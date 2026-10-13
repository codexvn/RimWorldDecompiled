using System;
using System.Collections.Generic;

namespace Verse.AI
{
	// Token: 0x02000EAC RID: 3756
	public class JobDriver_CastVerbOnceStatic : JobDriver_CastVerbOnce
	{
		// Token: 0x06005E97 RID: 24215 RVA: 0x001E0C4B File Offset: 0x001DEE4B
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedOrNull(TargetIndex.A);
			yield return Toils_General.StopDead();
			yield return Toils_Combat.CastVerb(TargetIndex.A, true);
			yield break;
		}
	}
}
