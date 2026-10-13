using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001212 RID: 4626
	public class JobDriver_GetReimplanted : JobDriver
	{
		// Token: 0x1700118B RID: 4491
		// (get) Token: 0x06007103 RID: 28931 RVA: 0x001E5D9E File Offset: 0x001E3F9E
		public Pawn Target
		{
			get
			{
				return this.job.targetA.Thing as Pawn;
			}
		}

		// Token: 0x06007104 RID: 28932 RVA: 0x0022FF57 File Offset: 0x0022E157
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.Target, this.job, 1, -1, null, errorOnFailed, false);
		}

		// Token: 0x06007105 RID: 28933 RVA: 0x0022FF7A File Offset: 0x0022E17A
		protected override IEnumerable<Toil> MakeNewToils()
		{
			if (!ModLister.CheckBiotech("xenogerm reimplanting"))
			{
				yield break;
			}
			this.FailOnDespawnedNullOrForbidden(TargetIndex.A);
			this.FailOnDowned(TargetIndex.A);
			this.FailOn(() => this.pawn.genes == null || this.Target.genes == null || GeneUtility.SameXenotype(this.Target, this.pawn));
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch, false);
			yield return Toils_General.Do(delegate
			{
				Ability ability = this.Target.abilities.abilities.FirstOrFallback((Ability x) => x.def == AbilityDefOf.ReimplantXenogerm, null);
				if (ability != null)
				{
					this.Target.jobs.TryTakeOrderedJob(ability.GetJob(this.pawn, this.pawn), new JobTag?(JobTag.Misc), false);
				}
			});
			yield break;
		}
	}
}
