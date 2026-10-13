using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001305 RID: 4869
	public class JobDriver_ResurrectMech : JobDriver
	{
		// Token: 0x170012C9 RID: 4809
		// (get) Token: 0x0600776F RID: 30575 RVA: 0x0024480C File Offset: 0x00242A0C
		private Corpse Corpse
		{
			get
			{
				return (Corpse)this.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		// Token: 0x170012CA RID: 4810
		// (get) Token: 0x06007770 RID: 30576 RVA: 0x00244834 File Offset: 0x00242A34
		private IntVec3 Destination
		{
			get
			{
				return base.TargetB.Cell;
			}
		}

		// Token: 0x06007771 RID: 30577 RVA: 0x00244850 File Offset: 0x00242A50
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.Corpse, this.job, 1, -1, null, errorOnFailed, false) && this.pawn.Reserve(this.Destination, this.job, 1, -1, null, errorOnFailed, false);
		}

		// Token: 0x06007772 RID: 30578 RVA: 0x002448A3 File Offset: 0x00242AA3
		protected override IEnumerable<Toil> MakeNewToils()
		{
			yield return Toils_Goto.GotoThing(TargetIndex.B, PathEndMode.Touch, false).FailOnDespawnedOrNull(TargetIndex.B);
			yield return Toils_General.Do(delegate
			{
				Ability ability = this.pawn.abilities.GetAbility(AbilityDefOf.ResurrectionMech, false);
				this.pawn.jobs.TryTakeOrderedJob(ability.GetJob(this.Corpse, this.Destination), new JobTag?(JobTag.Misc), false);
			});
			yield break;
		}

		// Token: 0x04005169 RID: 20841
		private const TargetIndex CorpseInd = TargetIndex.A;

		// Token: 0x0400516A RID: 20842
		private const TargetIndex CastCellInd = TargetIndex.B;
	}
}
