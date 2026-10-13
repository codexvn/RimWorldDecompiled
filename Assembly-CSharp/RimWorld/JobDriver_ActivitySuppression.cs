using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200121B RID: 4635
	public class JobDriver_ActivitySuppression : JobDriver
	{
		// Token: 0x17001193 RID: 4499
		// (get) Token: 0x06007136 RID: 28982 RVA: 0x0023071F File Offset: 0x0022E91F
		private Building_HoldingPlatform Platform
		{
			get
			{
				return base.TargetThingA as Building_HoldingPlatform;
			}
		}

		// Token: 0x17001194 RID: 4500
		// (get) Token: 0x06007137 RID: 28983 RVA: 0x0023072C File Offset: 0x0022E92C
		private Thing ThingToSuppress
		{
			get
			{
				Building_HoldingPlatform platform = this.Platform;
				return ((platform != null) ? platform.HeldPawn : null) ?? base.TargetThingA;
			}
		}

		// Token: 0x06007138 RID: 28984 RVA: 0x0023074A File Offset: 0x0022E94A
		protected override string ReportStringProcessed(string str)
		{
			return JobUtility.GetResolvedJobReport(str, LocalTargetInfo.Invalid, LocalTargetInfo.Invalid, this.ThingToSuppress);
		}

		// Token: 0x06007139 RID: 28985 RVA: 0x00230768 File Offset: 0x0022E968
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			if (this.Platform != null)
			{
				return this.pawn.Reserve(this.Platform, this.job, 1, -1, null, errorOnFailed, false) && this.pawn.Reserve(this.ThingToSuppress, this.job, 1, -1, null, errorOnFailed, false);
			}
			return this.pawn.Reserve(this.ThingToSuppress, this.job, 1, -1, null, errorOnFailed, false);
		}

		// Token: 0x0600713A RID: 28986 RVA: 0x002307E5 File Offset: 0x0022E9E5
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.job.targetC = this.ThingToSuppress;
			this.FailOnDespawnedNullOrForbidden(TargetIndex.A);
			this.FailOn(() => !ActivitySuppressionUtility.CanBeSuppressed(this.ThingToSuppress, false, this.job.playerForced));
			yield return Toils_General.Do(delegate
			{
				IntVec3 adjacentInteractionCell = SocialInteractionUtility.GetAdjacentInteractionCell(this.pawn, base.TargetThingA, this.job.playerForced);
				this.pawn.Map.pawnDestinationReservationManager.Reserve(this.pawn, this.job, adjacentInteractionCell);
				this.job.SetTarget(TargetIndex.B, adjacentInteractionCell);
			});
			yield return Toils_Goto.GotoCell(TargetIndex.B, PathEndMode.OnCell);
			yield return this.TrySuppress(this.ThingToSuppress);
			yield break;
		}

		// Token: 0x0600713B RID: 28987 RVA: 0x002307F8 File Offset: 0x0022E9F8
		private Toil TrySuppress(Thing thingToSuppress)
		{
			Toil toil = ToilMaker.MakeToil("TrySuppress");
			toil.initAction = delegate()
			{
				toil.actor.pather.StopDead();
			};
			toil.tickIntervalAction = delegate(int delta)
			{
				Pawn actor = toil.actor;
				if (thingToSuppress == null)
				{
					Log.Error("Tried to execute activity suppression toil but thingToSuppress was null");
					actor.jobs.EndCurrentJob(JobCondition.Errored, true, true);
					return;
				}
				CompActivity compActivity = thingToSuppress.TryGetComp<CompActivity>();
				float num = actor.GetStatValue(StatDefOf.ActivitySuppressionRate, true, -1) / 2500f * (float)delta;
				compActivity.AdjustActivity(-num);
				this.pawn.skills.Learn(SkillDefOf.Intellectual, 0.1f, false, false);
				if (compActivity.ActivityLevel < thingToSuppress.TryGetComp<CompActivity>().suppressIfAbove - 0.1f && actor.IsHashIntervalTick(4000, delta))
				{
					actor.jobs.CheckForJobOverride(0f, true);
				}
				CompObelisk compObelisk = this.ThingToSuppress.TryGetComp<CompObelisk>();
				if (compObelisk == null)
				{
					return;
				}
				compObelisk.Notify_InteractedTick(this.pawn, delta);
			};
			toil.WithProgressBar(TargetIndex.A, delegate
			{
				if (thingToSuppress == null)
				{
					return 1f;
				}
				return 1f - thingToSuppress.TryGetComp<CompActivity>().ActivityLevel;
			}, false, -0.5f, false);
			toil.WithEffect(EffecterDefOf.ActivitySuppression, () => this.ThingToSuppress.SpawnedParentOrMe, null);
			toil.AddEndCondition(delegate
			{
				if (thingToSuppress == null)
				{
					Log.Error("Tried to execute activity suppression toil but thingToSuppress was null");
					return JobCondition.Errored;
				}
				if (thingToSuppress.TryGetComp<CompActivity>().ActivityLevel < 0.01f)
				{
					return JobCondition.Succeeded;
				}
				return JobCondition.Ongoing;
			});
			toil.FailOnCannotTouch(TargetIndex.A, PathEndMode.Touch);
			toil.activeSkill = (() => SkillDefOf.Intellectual);
			toil.socialMode = RandomSocialMode.Off;
			toil.defaultCompleteMode = ToilCompleteMode.Never;
			return toil;
		}

		// Token: 0x04004E7F RID: 20095
		private const TargetIndex ThingToSuppressOrPlatformIndex = TargetIndex.A;

		// Token: 0x04004E80 RID: 20096
		private const TargetIndex AdjacentCellIndex = TargetIndex.B;
	}
}
