using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02001303 RID: 4867
	public class JobDriver_Resurrect : JobDriver
	{
		// Token: 0x170012C5 RID: 4805
		// (get) Token: 0x06007760 RID: 30560 RVA: 0x00244454 File Offset: 0x00242654
		private Corpse Corpse
		{
			get
			{
				return (Corpse)this.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		// Token: 0x170012C6 RID: 4806
		// (get) Token: 0x06007761 RID: 30561 RVA: 0x0024447C File Offset: 0x0024267C
		private Thing Item
		{
			get
			{
				return this.job.GetTarget(TargetIndex.B).Thing;
			}
		}

		// Token: 0x06007762 RID: 30562 RVA: 0x002444A0 File Offset: 0x002426A0
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.Corpse, this.job, 1, -1, null, errorOnFailed, false) && this.pawn.Reserve(this.Item, this.job, 1, -1, null, errorOnFailed, false);
		}

		// Token: 0x06007763 RID: 30563 RVA: 0x002444F3 File Offset: 0x002426F3
		protected override IEnumerable<Toil> MakeNewToils()
		{
			yield return Toils_Goto.GotoThing(TargetIndex.B, PathEndMode.Touch, false).FailOnDespawnedOrNull(TargetIndex.B).FailOnDespawnedOrNull(TargetIndex.A);
			yield return Toils_Haul.StartCarryThing(TargetIndex.B, false, false, false, true, false);
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch, false).FailOnDespawnedOrNull(TargetIndex.A);
			Toil toil = Toils_General.Wait(600, TargetIndex.None);
			toil.WithProgressBarToilDelay(TargetIndex.A, false, -0.5f);
			toil.FailOnDespawnedOrNull(TargetIndex.A);
			toil.FailOnCannotTouch(TargetIndex.A, PathEndMode.Touch);
			toil.tickAction = delegate()
			{
				CompUsable compUsable = this.Item.TryGetComp<CompUsable>();
				if (compUsable != null && this.warmupMote == null && compUsable.Props.warmupMote != null)
				{
					this.warmupMote = MoteMaker.MakeAttachedOverlay(this.Corpse, compUsable.Props.warmupMote, Vector3.zero, 1f, -1f);
				}
				Mote mote = this.warmupMote;
				if (mote == null)
				{
					return;
				}
				mote.Maintain();
			};
			yield return toil;
			yield return Toils_General.Do(new Action(this.Resurrect));
			yield break;
		}

		// Token: 0x06007764 RID: 30564 RVA: 0x00244504 File Offset: 0x00242704
		private void Resurrect()
		{
			Pawn innerPawn = this.Corpse.InnerPawn;
			CompTargetEffect_Resurrect compTargetEffect_Resurrect = this.Item.TryGetComp<CompTargetEffect_Resurrect>();
			bool flag = true;
			if (!compTargetEffect_Resurrect.Props.withSideEffects)
			{
				if (!ResurrectionUtility.TryResurrect(innerPawn, null))
				{
					flag = false;
				}
			}
			else if (!ResurrectionUtility.TryResurrectWithSideEffects(innerPawn))
			{
				flag = false;
			}
			if (flag)
			{
				SoundDefOf.MechSerumUsed.PlayOneShot(SoundInfo.InMap(innerPawn, MaintenanceType.None));
				Messages.Message("MessagePawnResurrected".Translate(innerPawn), innerPawn, MessageTypeDefOf.PositiveEvent, true);
				if (compTargetEffect_Resurrect.Props.moteDef != null)
				{
					MoteMaker.MakeAttachedOverlay(innerPawn, compTargetEffect_Resurrect.Props.moteDef, Vector3.zero, 1f, -1f);
				}
				if (compTargetEffect_Resurrect.Props.addsHediff != null)
				{
					innerPawn.health.AddHediff(compTargetEffect_Resurrect.Props.addsHediff, null, null, null);
				}
			}
			this.Item.SplitOff(1).Destroy(DestroyMode.Vanish);
		}

		// Token: 0x04005161 RID: 20833
		private const TargetIndex CorpseInd = TargetIndex.A;

		// Token: 0x04005162 RID: 20834
		private const TargetIndex ItemInd = TargetIndex.B;

		// Token: 0x04005163 RID: 20835
		private const int DurationTicks = 600;

		// Token: 0x04005164 RID: 20836
		private Mote warmupMote;
	}
}
