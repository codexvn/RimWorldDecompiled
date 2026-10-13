using System;
using System.Collections.Generic;

namespace Verse.AI
{
	// Token: 0x02000E56 RID: 3670
	public class JobDriver_GotoMindControlled : JobDriver_Goto
	{
		// Token: 0x06005C75 RID: 23669 RVA: 0x001D83A3 File Offset: 0x001D65A3
		protected override IEnumerable<Toil> MakeNewToils()
		{
			Toil toil = Toils_Goto.GotoCell(TargetIndex.A, PathEndMode.OnCell);
			yield return toil;
			if (this.job.def.waitAfterArriving > 0)
			{
				yield return Toils_General.Wait(this.job.def.waitAfterArriving, TargetIndex.None);
			}
			yield break;
		}
	}
}
