using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02001332 RID: 4914
	public class JobDriver_TalkCreepJoiner : JobDriver
	{
		// Token: 0x060078BE RID: 30910 RVA: 0x0022F6AD File Offset: 0x0022D8AD
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(base.TargetA, this.job, 1, -1, null, errorOnFailed, false);
		}

		// Token: 0x060078BF RID: 30911 RVA: 0x00248B7F File Offset: 0x00246D7F
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedOrNull(TargetIndex.A);
			Toil toil = ToilMaker.MakeToil("MakeNewToils");
			toil.initAction = (Action)Delegate.Combine(toil.initAction, new Action(delegate()
			{
				if (!this.notified)
				{
					this.notified = true;
					Lord lord = base.TargetPawnA.GetLord();
					if (lord == null)
					{
						return;
					}
					lord.ReceiveMemo("SpokenTo");
				}
			}));
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch, false);
			yield return Toils_General.Do(delegate
			{
				base.TargetPawnA.creepjoiner.Notify_CreepJoinerSpokenTo(this.pawn);
			});
			yield break;
		}

		// Token: 0x060078C0 RID: 30912 RVA: 0x00248B8F File Offset: 0x00246D8F
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<bool>(ref this.notified, "notified", false, false);
		}

		// Token: 0x04005207 RID: 20999
		private bool notified;
	}
}
