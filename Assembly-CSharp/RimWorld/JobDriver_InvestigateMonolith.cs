using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020012A3 RID: 4771
	public class JobDriver_InvestigateMonolith : JobDriver
	{
		// Token: 0x17001245 RID: 4677
		// (get) Token: 0x060074E2 RID: 29922 RVA: 0x0023C4DB File Offset: 0x0023A6DB
		private Building_VoidMonolith VoidMonolith
		{
			get
			{
				return base.TargetThingA as Building_VoidMonolith;
			}
		}

		// Token: 0x060074E3 RID: 29923 RVA: 0x0023C4E8 File Offset: 0x0023A6E8
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.VoidMonolith, this.job, 1, -1, null, errorOnFailed, false) && this.pawn.ReserveSittableOrSpot(this.VoidMonolith.InteractionCell, this.job, errorOnFailed);
		}

		// Token: 0x060074E4 RID: 29924 RVA: 0x0023C53C File Offset: 0x0023A73C
		protected override IEnumerable<Toil> MakeNewToils()
		{
			if (this.job.targetB == null)
			{
				this.job.targetB = base.TargetThingA.SpawnedParentOrMe;
			}
			yield return Toils_Goto.GotoThing(TargetIndex.B, PathEndMode.InteractionCell, false).FailOnDespawnedNullOrForbidden(TargetIndex.B).FailOnSomeonePhysicallyInteracting(TargetIndex.B);
			int ticks = Mathf.RoundToInt(300f);
			Toil toil = Toils_General.WaitWith(TargetIndex.A, ticks, true, true, false, TargetIndex.A, PathEndMode.Touch);
			toil.WithEffect(EffecterDefOf.MonolithStage1, () => base.TargetA, null);
			toil.PlaySustainerOrSound(SoundDefOf.VoidMonolith_InspectLoop, 1f);
			Toil toil2 = toil;
			toil2.tickAction = (Action)Delegate.Combine(toil2.tickAction, new Action(delegate()
			{
				Find.TickManager.slower.SignalForceNormalSpeed();
			}));
			yield return toil;
			yield return Toils_General.Do(delegate
			{
				this.VoidMonolith.Investigate(this.pawn);
			});
			yield break;
		}

		// Token: 0x04005035 RID: 20533
		private const int InvestigateSeconds = 5;
	}
}
