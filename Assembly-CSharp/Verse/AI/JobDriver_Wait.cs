using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000E8C RID: 3724
	public class JobDriver_Wait : JobDriver
	{
		// Token: 0x06005DDF RID: 24031 RVA: 0x001DE884 File Offset: 0x001DCA84
		public override string GetReport()
		{
			if (this.job.def == JobDefOf.Wait_Combat && this.pawn.RaceProps.Humanlike && this.pawn.WorkTagIsDisabled(WorkTags.Violent))
			{
				return "ReportStanding".Translate();
			}
			return base.GetReport();
		}

		// Token: 0x06005DE0 RID: 24032 RVA: 0x000028E7 File Offset: 0x00000AE7
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return true;
		}

		// Token: 0x06005DE1 RID: 24033 RVA: 0x001DE8D9 File Offset: 0x001DCAD9
		protected override IEnumerable<Toil> MakeNewToils()
		{
			Toil toil = this.job.forceSleep ? Toils_LayDown.LayDown(TargetIndex.A, false, false, true, true, PawnPosture.LayingOnGroundNormal, false) : ToilMaker.MakeToil("MakeNewToils");
			Toil toil2 = toil;
			toil2.initAction = (Action)Delegate.Combine(toil2.initAction, new Action(delegate()
			{
				base.Map.pawnDestinationReservationManager.Reserve(this.pawn, this.job, this.pawn.Position);
				Pawn_PathFollower pather = this.pawn.pather;
				if (pather != null)
				{
					pather.StopDead();
				}
				this.CheckForAutoAttack();
			}));
			Toil toil3 = toil;
			toil3.tickIntervalAction = (Action<int>)Delegate.Combine(toil3.tickIntervalAction, new Action<int>(delegate(int delta)
			{
				if (this.job.expiryInterval == -1 && this.job.def == JobDefOf.Wait_Combat && !this.pawn.Drafted)
				{
					Pawn pawn = this.pawn;
					Log.Error(((pawn != null) ? pawn.ToString() : null) + " in eternal WaitCombat without being drafted.");
					base.ReadyForNextToil();
					return;
				}
				if (this.job.forceSleep)
				{
					this.asleep = true;
				}
				if (GenTicks.IsTickIntervalDelta(this.pawn.thingIDNumber, 4, delta))
				{
					this.CheckForAutoAttack();
				}
			}));
			this.DecorateWaitToil(toil);
			toil.defaultCompleteMode = ToilCompleteMode.Never;
			if (this.job.overrideFacing != Rot4.Invalid)
			{
				toil.handlingFacing = true;
				Toil toil4 = toil;
				toil4.tickAction = (Action)Delegate.Combine(toil4.tickAction, new Action(delegate()
				{
					this.pawn.rotationTracker.FaceTarget(this.pawn.Position + this.job.overrideFacing.FacingCell);
				}));
			}
			else if (this.pawn.mindState != null && this.pawn.mindState.duty != null && this.pawn.mindState.duty.focus != null && this.job.def != JobDefOf.Wait_Combat)
			{
				LocalTargetInfo focusLocal = this.pawn.mindState.duty.focus;
				toil.handlingFacing = true;
				Toil toil5 = toil;
				toil5.tickAction = (Action)Delegate.Combine(toil5.tickAction, new Action(delegate()
				{
					this.pawn.rotationTracker.FaceTarget(focusLocal);
				}));
			}
			yield return toil;
			yield break;
		}

		// Token: 0x06005DE2 RID: 24034 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void DecorateWaitToil(Toil wait)
		{
		}

		// Token: 0x06005DE3 RID: 24035 RVA: 0x001DE8E9 File Offset: 0x001DCAE9
		public override void Notify_StanceChanged()
		{
			if (this.pawn.stances.curStance is Stance_Mobile)
			{
				this.CheckForAutoAttack();
			}
		}

		// Token: 0x06005DE4 RID: 24036 RVA: 0x001DE908 File Offset: 0x001DCB08
		private void CheckForAutoAttack()
		{
			if (!this.pawn.kindDef.canMeleeAttack)
			{
				return;
			}
			if (this.pawn.Downed)
			{
				return;
			}
			if (this.pawn.stances.FullBodyBusy)
			{
				return;
			}
			if (this.pawn.IsCarryingPawn(null))
			{
				return;
			}
			if (!this.pawn.IsPlayerControlled && this.pawn.IsPsychologicallyInvisible())
			{
				return;
			}
			if (this.pawn.IsShambler)
			{
				return;
			}
			this.collideWithPawns = false;
			bool flag = !this.pawn.WorkTagIsDisabled(WorkTags.Violent);
			bool flag2 = this.pawn.RaceProps.ToolUser && this.pawn.Faction == Faction.OfPlayer && !this.pawn.WorkTagIsDisabled(WorkTags.Firefighting);
			if (flag || flag2)
			{
				Fire fire = null;
				for (int i = 0; i < 9; i++)
				{
					IntVec3 c = this.pawn.Position + GenAdj.AdjacentCellsAndInside[i];
					if (c.InBounds(this.pawn.Map))
					{
						List<Thing> thingList = c.GetThingList(base.Map);
						for (int j = 0; j < thingList.Count; j++)
						{
							if (flag && this.pawn.kindDef.canMeleeAttack)
							{
								Pawn pawn = thingList[j] as Pawn;
								if (pawn != null && !pawn.ThreatDisabled(this.pawn) && this.pawn.HostileTo(pawn))
								{
									CompActivity comp = pawn.GetComp<CompActivity>();
									if ((comp == null || comp.IsActive) && !this.pawn.ThreatDisabledBecauseNonAggressiveRoamer(pawn) && GenHostility.IsActiveThreatTo(pawn, this.pawn.Faction, false, false))
									{
										this.pawn.meleeVerbs.TryMeleeAttack(pawn, null, false);
										this.collideWithPawns = true;
										return;
									}
								}
							}
							if (flag2)
							{
								Fire fire2 = thingList[j] as Fire;
								if (fire2 != null && (fire == null || fire2.fireSize < fire.fireSize || i == 8) && (fire2.parent == null || fire2.parent != this.pawn))
								{
									fire = fire2;
								}
							}
						}
					}
				}
				if (fire != null && (!this.pawn.InMentalState || this.pawn.MentalState.def.allowBeatfire))
				{
					this.pawn.natives.TryBeatFire(fire);
					return;
				}
				if (flag && this.job.canUseRangedWeapon && this.job.def == JobDefOf.Wait_Combat && (this.pawn.drafter == null || this.pawn.drafter.FireAtWill))
				{
					Verb currentEffectiveVerb = this.pawn.CurrentEffectiveVerb;
					if (currentEffectiveVerb == null || currentEffectiveVerb.verbProps.IsMeleeAttack)
					{
						return;
					}
					TargetScanFlags targetScanFlags = TargetScanFlags.NeedLOSToPawns | TargetScanFlags.NeedLOSToNonPawns | TargetScanFlags.NeedThreat | TargetScanFlags.NeedAutoTargetable;
					if (currentEffectiveVerb.IsIncendiary_Ranged())
					{
						targetScanFlags |= TargetScanFlags.NeedNonBurning;
					}
					Thing thing = (Thing)AttackTargetFinder.BestShootTargetFromCurrentPosition(this.pawn, targetScanFlags, null, 0f, 9999f);
					if (thing != null)
					{
						this.pawn.TryStartAttack(thing);
						this.collideWithPawns = true;
					}
				}
			}
		}

		// Token: 0x040043EC RID: 17388
		private const int TargetSearchInterval = 4;
	}
}
