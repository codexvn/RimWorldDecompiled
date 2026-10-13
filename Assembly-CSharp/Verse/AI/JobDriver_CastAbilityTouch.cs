using System;
using System.Collections.Generic;

namespace Verse.AI
{
	// Token: 0x02000E9A RID: 3738
	public class JobDriver_CastAbilityTouch : JobDriver_CastVerbOnce
	{
		// Token: 0x06005E39 RID: 24121 RVA: 0x001E018F File Offset: 0x001DE38F
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedOrNull(TargetIndex.A);
			this.FailOnCannotTouch(TargetIndex.B, PathEndMode.OnCell);
			yield return Toils_Goto.Goto(TargetIndex.A, PathEndMode.Touch);
			yield return Toils_Combat.CastVerb(TargetIndex.A, TargetIndex.B, false);
			yield break;
		}
	}
}
