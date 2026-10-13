using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001344 RID: 4932
	public class JobDriver_UseItem : JobDriver
	{
		// Token: 0x06007925 RID: 31013 RVA: 0x00249DA7 File Offset: 0x00247FA7
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.useDuration, "useDuration", 0, false);
		}

		// Token: 0x06007926 RID: 31014 RVA: 0x00249DC4 File Offset: 0x00247FC4
		public override void Notify_Starting()
		{
			base.Notify_Starting();
			this.useDuration = this.job.GetTarget(TargetIndex.A).Thing.TryGetComp<CompUsable>().Props.useDuration;
		}

		// Token: 0x06007927 RID: 31015 RVA: 0x00249E00 File Offset: 0x00248000
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.job.targetA, this.job, 1, -1, null, errorOnFailed, false) && (!this.job.targetB.IsValid || this.pawn.Reserve(this.job.targetB, this.job, 1, -1, null, errorOnFailed, false));
		}

		// Token: 0x06007928 RID: 31016 RVA: 0x00249E6A File Offset: 0x0024806A
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnIncapable(PawnCapacityDefOf.Manipulation);
			this.FailOn(() => !base.TargetThingA.TryGetComp<CompUsable>().CanBeUsedBy(this.pawn, false, false));
			yield return Toils_Goto.GotoThing(TargetIndex.A, base.TargetThingA.def.hasInteractionCell ? PathEndMode.InteractionCell : PathEndMode.Touch, false);
			yield return this.PrepareToUse();
			yield return this.Use();
			yield break;
		}

		// Token: 0x06007929 RID: 31017 RVA: 0x00249E7C File Offset: 0x0024807C
		protected Toil PrepareToUse()
		{
			Toil toil = Toils_General.Wait(this.useDuration, TargetIndex.A);
			toil.WithProgressBarToilDelay(TargetIndex.A, false, -0.5f);
			toil.FailOnDespawnedNullOrForbidden(TargetIndex.A);
			toil.FailOnCannotTouch(TargetIndex.A, base.TargetThingA.def.hasInteractionCell ? PathEndMode.InteractionCell : PathEndMode.Touch);
			toil.handlingFacing = true;
			toil.tickAction = delegate()
			{
				ThingWithComps thingWithComps = this.job.GetTarget(TargetIndex.A).Thing as ThingWithComps;
				if (thingWithComps != null)
				{
					using (IEnumerator<CompUseEffect> enumerator = thingWithComps.GetComps<CompUseEffect>().GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							CompUseEffect compUseEffect = enumerator.Current;
							if (compUseEffect != null)
							{
								compUseEffect.PrepareTick();
							}
						}
						goto IL_74;
					}
				}
				CompUseEffect compUseEffect2 = this.job.GetTarget(TargetIndex.A).Thing.TryGetComp<CompUseEffect>();
				if (compUseEffect2 != null)
				{
					compUseEffect2.PrepareTick();
				}
				IL_74:
				CompUsable compUsable = this.job.GetTarget(TargetIndex.A).Thing.TryGetComp<CompUsable>();
				if (compUsable != null && this.warmupMote == null && compUsable.Props.warmupMote != null)
				{
					this.warmupMote = MoteMaker.MakeAttachedOverlay(this.job.GetTarget(TargetIndex.B).Thing, compUsable.Props.warmupMote, Vector3.zero, 1f, -1f);
				}
				Mote mote = this.warmupMote;
				if (mote != null)
				{
					mote.Maintain();
				}
				this.pawn.rotationTracker.FaceTarget(base.TargetA);
			};
			if (this.job.targetB.IsValid)
			{
				toil.FailOnDespawnedOrNull(TargetIndex.B);
				CompTargetable compTargetable = this.job.GetTarget(TargetIndex.A).Thing.TryGetComp<CompTargetable>();
				if (compTargetable != null && compTargetable.Props.nonDownedPawnOnly)
				{
					toil.FailOnDestroyedOrNull(TargetIndex.B);
					toil.FailOnDowned(TargetIndex.B);
				}
			}
			return toil;
		}

		// Token: 0x0600792A RID: 31018 RVA: 0x00249F38 File Offset: 0x00248138
		protected Toil Use()
		{
			Toil use = ToilMaker.MakeToil("Use");
			use.initAction = delegate()
			{
				Pawn actor = use.actor;
				CompUsable compUsable = actor.CurJob.targetA.Thing.TryGetComp<CompUsable>();
				compUsable.UsedBy(actor);
				if (compUsable.Props.finalizeMote != null)
				{
					MoteMaker.MakeAttachedOverlay(actor.CurJob.GetTarget(TargetIndex.A).Thing, compUsable.Props.finalizeMote, Vector3.zero, 1f, -1f);
				}
			};
			use.defaultCompleteMode = ToilCompleteMode.Instant;
			return use;
		}

		// Token: 0x0400523B RID: 21051
		private int useDuration = -1;

		// Token: 0x0400523C RID: 21052
		private Mote warmupMote;
	}
}
