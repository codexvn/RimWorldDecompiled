using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200130C RID: 4876
	public class JobDriver_RevenantSleep : JobDriver
	{
		// Token: 0x060077A8 RID: 30632 RVA: 0x000028E7 File Offset: 0x00000AE7
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return true;
		}

		// Token: 0x170012D5 RID: 4821
		// (get) Token: 0x060077A9 RID: 30633 RVA: 0x00244A3E File Offset: 0x00242C3E
		private CompRevenant Comp
		{
			get
			{
				return this.pawn.TryGetComp<CompRevenant>();
			}
		}

		// Token: 0x060077AA RID: 30634 RVA: 0x002455EB File Offset: 0x002437EB
		protected override IEnumerable<Toil> MakeNewToils()
		{
			Toil toil = ToilMaker.MakeToil("MakeNewToils");
			Toil toil2 = toil;
			toil2.initAction = (Action)Delegate.Combine(toil2.initAction, new Action(delegate()
			{
				base.Map.pawnDestinationReservationManager.Reserve(this.pawn, this.job, this.pawn.Position);
				Pawn_PathFollower pather = this.pawn.pather;
				if (pather == null)
				{
					return;
				}
				pather.StopDead();
			}));
			Toil toil3 = toil;
			toil3.tickIntervalAction = (Action<int>)Delegate.Combine(toil3.tickIntervalAction, new Action<int>(delegate(int delta)
			{
				if (Find.TickManager.TicksGame >= this.Comp.nextHypnosis && Rand.MTBEventOccurs(2500f, 1f, (float)delta))
				{
					Pawn pawn = RevenantUtility.ScanForTarget(this.pawn, false);
					if (pawn != null)
					{
						this.pawn.mindState.enemyTarget = pawn;
						this.Comp.revenantState = RevenantState.Attack;
						base.EndJobWith(JobCondition.InterruptForced);
					}
				}
			}));
			toil.defaultCompleteMode = ToilCompleteMode.Never;
			yield return toil;
			yield break;
		}
	}
}
