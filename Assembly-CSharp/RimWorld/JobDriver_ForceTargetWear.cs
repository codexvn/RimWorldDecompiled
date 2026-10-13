using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020013C1 RID: 5057
	public class JobDriver_ForceTargetWear : JobDriver
	{
		// Token: 0x1700137C RID: 4988
		// (get) Token: 0x06007BC4 RID: 31684 RVA: 0x002545B8 File Offset: 0x002527B8
		private Pawn TargetPawn
		{
			get
			{
				return (Pawn)this.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		// Token: 0x1700137D RID: 4989
		// (get) Token: 0x06007BC5 RID: 31685 RVA: 0x002545E0 File Offset: 0x002527E0
		private Apparel Apparel
		{
			get
			{
				return (Apparel)this.job.GetTarget(TargetIndex.B).Thing;
			}
		}

		// Token: 0x1700137E RID: 4990
		// (get) Token: 0x06007BC6 RID: 31686 RVA: 0x00254608 File Offset: 0x00252808
		private bool TargetIsOnApparelSource
		{
			get
			{
				Apparel apparel = this.Apparel;
				if (apparel != null && !apparel.Spawned)
				{
					IApparelSource apparelSource = apparel.ParentHolder as IApparelSource;
					if (apparelSource != null)
					{
						return apparelSource is Thing;
					}
				}
				return false;
			}
		}

		// Token: 0x1700137F RID: 4991
		// (get) Token: 0x06007BC7 RID: 31687 RVA: 0x00254644 File Offset: 0x00252844
		private IApparelSource ApparelSource
		{
			get
			{
				return (IApparelSource)this.job.GetTarget(TargetIndex.C).Thing;
			}
		}

		// Token: 0x06007BC8 RID: 31688 RVA: 0x0025466A File Offset: 0x0025286A
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.duration, "duration", 0, false);
			Scribe_Values.Look<int>(ref this.unequipBuffer, "unequipBuffer", 0, false);
		}

		// Token: 0x06007BC9 RID: 31689 RVA: 0x00254698 File Offset: 0x00252898
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			this.job.count = 1;
			if (this.pawn == this.TargetPawn)
			{
				Log.Error(string.Format("Pawn {0} tried to do ForceTargetWear with self as target; this should not happen.", this.pawn));
				return false;
			}
			if (!this.pawn.Reserve(this.TargetPawn, this.job, 1, -1, null, errorOnFailed, false))
			{
				return false;
			}
			if (this.TargetIsOnApparelSource)
			{
				return this.pawn.Reserve((Thing)this.Apparel.ParentHolder, this.job, 1, -1, null, errorOnFailed, false);
			}
			return this.pawn.Reserve(this.Apparel, this.job, 1, -1, null, errorOnFailed, false);
		}

		// Token: 0x06007BCA RID: 31690 RVA: 0x00254750 File Offset: 0x00252950
		public override void Notify_Starting()
		{
			base.Notify_Starting();
			if (this.TargetIsOnApparelSource)
			{
				this.job.targetC = (Thing)this.Apparel.ParentHolder;
			}
			this.duration = (int)(this.Apparel.GetStatValue(StatDefOf.EquipDelay, true, -1) * 60f);
			Apparel apparel = this.Apparel;
			List<Apparel> wornApparel = this.TargetPawn.apparel.WornApparel;
			for (int i = wornApparel.Count - 1; i >= 0; i--)
			{
				if (!ApparelUtility.CanWearTogether(apparel.def, wornApparel[i].def, this.TargetPawn.RaceProps.body))
				{
					this.duration += (int)(wornApparel[i].GetStatValue(StatDefOf.EquipDelay, true, -1) * 60f);
				}
			}
		}

		// Token: 0x06007BCB RID: 31691 RVA: 0x00254825 File Offset: 0x00252A25
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnBurningImmobile(TargetIndex.B);
			this.FailOnAggroMentalState(TargetIndex.A);
			bool usingApparelSource = this.TargetIsOnApparelSource;
			if (usingApparelSource)
			{
				yield return Toils_Goto.GotoCell(TargetIndex.C, PathEndMode.InteractionCell).FailOnDespawnedNullOrForbidden(TargetIndex.C);
			}
			else
			{
				yield return Toils_Goto.GotoThing(TargetIndex.B, PathEndMode.ClosestTouch, false).FailOnDespawnedNullOrForbidden(TargetIndex.B);
			}
			if (usingApparelSource)
			{
				Toil toil = ToilMaker.MakeToil("MakeNewToils");
				toil.defaultCompleteMode = ToilCompleteMode.Instant;
				toil.initAction = delegate()
				{
					IApparelSource apparelSource = (IApparelSource)this.Apparel.ParentHolder;
					if (apparelSource == null)
					{
						base.EndJobWith(JobCondition.Incompletable);
						return;
					}
					if (!apparelSource.RemoveApparel(this.Apparel))
					{
						base.EndJobWith(JobCondition.Incompletable);
						return;
					}
					if (!this.pawn.carryTracker.TryStartCarry(this.Apparel))
					{
						base.EndJobWith(JobCondition.Incompletable);
						return;
					}
					this.job.SetTarget(TargetIndex.B, this.Apparel);
					this.pawn.records.Increment(RecordDefOf.ThingsHauled);
				};
				yield return toil;
			}
			else
			{
				yield return Toils_Haul.StartCarryThing(TargetIndex.B, false, false, false, true, false);
			}
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch, false).FailOnDespawnedNullOrForbidden(TargetIndex.A);
			yield return Toils_Interpersonal.GotoInteractablePosition(TargetIndex.A);
			yield return Toils_Haul.PlaceHauledThingInCell(TargetIndex.A, null, true, false);
			Toil toil2 = ToilMaker.MakeToil("MakeNewToils");
			toil2.initAction = delegate()
			{
				PawnUtility.ForceWait(this.TargetPawn, this.duration + 360, null, true, true);
			};
			toil2.tickIntervalAction = delegate(int delta)
			{
				this.unequipBuffer += delta;
				this.TryUnequipSomething();
				this.pawn.rotationTracker.FaceTarget(this.TargetPawn);
			};
			toil2.WithProgressBarToilDelay(TargetIndex.B, false, -0.5f);
			toil2.FailOnDespawnedNullOrForbidden(TargetIndex.B);
			toil2.defaultCompleteMode = ToilCompleteMode.Delay;
			toil2.defaultDuration = this.duration;
			toil2.handlingFacing = true;
			toil2.PlaySustainerOrSound(new Func<SoundDef>(this.GetCurrentWearSound), 1f);
			yield return toil2;
			Toil toil3 = Toils_General.Do(delegate
			{
				Apparel apparel = this.Apparel;
				this.TargetPawn.apparel.Wear(apparel, true, false);
				if (this.TargetPawn.outfits != null && this.job.playerForced)
				{
					this.TargetPawn.outfits.forcedHandler.SetForced(apparel, true);
				}
			});
			toil3.AddFinishAction(delegate
			{
				Pawn targetPawn = this.TargetPawn;
				if (targetPawn != null && targetPawn.CurJobDef == JobDefOf.Wait_MaintainPosture)
				{
					targetPawn.jobs.EndCurrentJob(JobCondition.InterruptForced, true, true);
				}
				if (targetPawn != null)
				{
					Faction faction = targetPawn.Faction;
					if (faction == null)
					{
						return;
					}
					faction.Notify_MemberStripped(targetPawn, Faction.OfPlayer);
				}
			});
			yield return toil3;
			yield break;
		}

		// Token: 0x06007BCC RID: 31692 RVA: 0x00254838 File Offset: 0x00252A38
		private SoundDef GetCurrentWearSound()
		{
			Apparel apparel = this.Apparel;
			List<Apparel> wornApparel = this.TargetPawn.apparel.WornApparel;
			int i = wornApparel.Count - 1;
			while (i >= 0)
			{
				if (!ApparelUtility.CanWearTogether(apparel.def, wornApparel[i].def, this.TargetPawn.RaceProps.body))
				{
					if (this.unequipBuffer < (int)(wornApparel[i].GetStatValue(StatDefOf.EquipDelay, true, -1) * 60f))
					{
						return wornApparel[i].def.apparel.soundRemove;
					}
					break;
				}
				else
				{
					i--;
				}
			}
			return apparel.def.apparel.soundWear;
		}

		// Token: 0x06007BCD RID: 31693 RVA: 0x002548E4 File Offset: 0x00252AE4
		private void TryUnequipSomething()
		{
			Apparel apparel = this.Apparel;
			List<Apparel> wornApparel = this.TargetPawn.apparel.WornApparel;
			int i = wornApparel.Count - 1;
			while (i >= 0)
			{
				if (!ApparelUtility.CanWearTogether(apparel.def, wornApparel[i].def, this.TargetPawn.RaceProps.body))
				{
					int num = (int)(wornApparel[i].GetStatValue(StatDefOf.EquipDelay, true, -1) * 60f);
					if (this.unequipBuffer < num)
					{
						break;
					}
					bool forbid = this.TargetPawn.Faction != null && this.TargetPawn.Faction.HostileTo(Faction.OfPlayer);
					Apparel apparel2;
					if (!this.TargetPawn.apparel.TryDrop(wornApparel[i], out apparel2, this.TargetPawn.PositionHeld, forbid))
					{
						Pawn targetPawn = this.TargetPawn;
						Log.Error(((targetPawn != null) ? targetPawn.ToString() : null) + " could not drop " + wornApparel[i].ToStringSafe<Apparel>());
						base.EndJobWith(JobCondition.Errored);
						return;
					}
					break;
				}
				else
				{
					i--;
				}
			}
		}

		// Token: 0x040053A3 RID: 21411
		private int duration;

		// Token: 0x040053A4 RID: 21412
		private int unequipBuffer;

		// Token: 0x040053A5 RID: 21413
		private const TargetIndex PawnInd = TargetIndex.A;

		// Token: 0x040053A6 RID: 21414
		private const TargetIndex ApparelInd = TargetIndex.B;

		// Token: 0x040053A7 RID: 21415
		private const TargetIndex ApparelSourceIndex = TargetIndex.C;
	}
}
