using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200130E RID: 4878
	public class JobDriver_RevenantWander : JobDriver
	{
		// Token: 0x170012D8 RID: 4824
		// (get) Token: 0x060077B6 RID: 30646 RVA: 0x00244A3E File Offset: 0x00242C3E
		private CompRevenant Comp
		{
			get
			{
				return this.pawn.TryGetComp<CompRevenant>();
			}
		}

		// Token: 0x060077B7 RID: 30647 RVA: 0x000028E7 File Offset: 0x00000AE7
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return true;
		}

		// Token: 0x060077B8 RID: 30648 RVA: 0x002457B3 File Offset: 0x002439B3
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.Comp.becomeInvisibleTick = Find.TickManager.TicksGame + 140;
			Toil toil = Toils_Goto.GotoCell(TargetIndex.A, PathEndMode.OnCell);
			Toil toil2 = toil;
			toil2.tickIntervalAction = (Action<int>)Delegate.Combine(toil2.tickIntervalAction, new Action<int>(delegate(int delta)
			{
				if (Find.TickManager.TicksGame > this.Comp.revenantLastLeftSmear + 60000 && Rand.MTBEventOccurs(2f, 60000f, (float)delta))
				{
					RevenantUtility.CreateRevenantSmear(this.pawn);
				}
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
				if (Rand.MTBEventOccurs(900f, 1f, (float)delta))
				{
					Pawn closestTargetInRadius = RevenantUtility.GetClosestTargetInRadius(this.pawn, 10f);
					if (closestTargetInRadius != null)
					{
						this.pawn.mindState.enemyTarget = closestTargetInRadius;
						this.Comp.revenantState = RevenantState.Attack;
						base.EndJobWith(JobCondition.InterruptForced);
					}
				}
			}));
			yield return toil;
			yield break;
		}

		// Token: 0x04005188 RID: 20872
		private const int SmearMTBDays = 2;

		// Token: 0x04005189 RID: 20873
		private const int MinSmearInterval = 60000;
	}
}
