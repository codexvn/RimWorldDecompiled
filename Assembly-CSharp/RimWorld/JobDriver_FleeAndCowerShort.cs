using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001281 RID: 4737
	public class JobDriver_FleeAndCowerShort : JobDriver_Flee
	{
		// Token: 0x060073E6 RID: 29670 RVA: 0x0023900C File Offset: 0x0023720C
		public override string GetReport()
		{
			if (this.pawn.CurJob == this.job && this.pawn.Position == this.job.GetTarget(TargetIndex.A).Cell)
			{
				return "ReportCowering".Translate();
			}
			return base.GetReport();
		}

		// Token: 0x060073E7 RID: 29671 RVA: 0x00239068 File Offset: 0x00237268
		protected override IEnumerable<Toil> MakeNewToils()
		{
			foreach (Toil toil in base.MakeNewToils())
			{
				yield return toil;
			}
			IEnumerator<Toil> enumerator = null;
			yield return Toils_General.Wait(30, TargetIndex.None);
			yield break;
			yield break;
		}

		// Token: 0x04004FC9 RID: 20425
		private const int CowerTicks = 30;
	}
}
