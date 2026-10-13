using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020012A6 RID: 4774
	public class JobDriver_Kidnap : JobDriver_TakeAndExitMap
	{
		// Token: 0x17001248 RID: 4680
		// (get) Token: 0x060074F3 RID: 29939 RVA: 0x0023C733 File Offset: 0x0023A933
		protected Pawn Takee
		{
			get
			{
				return (Pawn)base.Item;
			}
		}

		// Token: 0x060074F4 RID: 29940 RVA: 0x0023C740 File Offset: 0x0023A940
		public override string GetReport()
		{
			if (this.Takee == null || this.pawn.HostileTo(this.Takee))
			{
				return base.GetReport();
			}
			return JobUtility.GetResolvedJobReport(JobDefOf.Rescue.reportString, this.Takee);
		}

		// Token: 0x060074F5 RID: 29941 RVA: 0x0023C77E File Offset: 0x0023A97E
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOn(() => this.Takee == null || (!this.Takee.Downed && this.Takee.Awake()));
			foreach (Toil toil in base.MakeNewToils())
			{
				yield return toil;
			}
			IEnumerator<Toil> enumerator = null;
			yield break;
			yield break;
		}
	}
}
