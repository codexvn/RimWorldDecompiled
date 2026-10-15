using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.AI.Group;
using Verse.Sound;

namespace RimWorld;

public class JobDriver_BestowingCeremony : JobDriver
{
	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass15_0
	{
		public JobDriver_BestowingCeremony _003C_003E4__this;

		public Toil waitToil;

		internal bool _003CMakeNewToils_003Eb__0()
		{
			if (_003C_003E4__this.Bestower.GetLord() != null && _003C_003E4__this.Bestower.GetLord().CurLordToil != null)
			{
				return !(_003C_003E4__this.Bestower.GetLord().CurLordToil is LordToil_BestowingCeremony_Perform);
			}
			return true;
		}

		internal void _003CMakeNewToils_003Eb__1()
		{
			Messages.Message("MessageBestowingCeremonyStarted".Translate(_003C_003E4__this.pawn.Named("PAWN")), _003C_003E4__this.Bestower, MessageTypeDefOf.PositiveEvent);
		}

		internal void _003CMakeNewToils_003Eb__2()
		{
			if (!AnalyzeThroneRoom(_003C_003E4__this.Bestower, _003C_003E4__this.pawn))
			{
				Messages.Message("BestowingCeremonyThroneroomRequirementsNotSatisfied".Translate(_003C_003E4__this.pawn.Named("PAWN"), _003C_003E4__this.pawn.royalty.GetTitleAwardedWhenUpdating(_003C_003E4__this.Bestower.Faction, _003C_003E4__this.pawn.royalty.GetFavor(_003C_003E4__this.Bestower.Faction)).label.Named("TITLE")), _003C_003E4__this.pawn, MessageTypeDefOf.NegativeEvent);
				((LordJob_BestowingCeremony)_003C_003E4__this.Bestower.GetLord().LordJob).MakeCeremonyFail();
			}
		}

		internal void _003CMakeNewToils_003Eb__3()
		{
			SoundDefOf.Bestowing_Start.PlayOneShot(_003C_003E4__this.pawn);
		}

		internal void _003CMakeNewToils_003Eb__4()
		{
			_003C_003E4__this.pawn.rotationTracker.FaceTarget(_003C_003E4__this.Bestower);
			if (_003C_003E4__this.mote == null || _003C_003E4__this.mote.Destroyed)
			{
				Vector3 loc = (_003C_003E4__this.pawn.TrueCenter() + _003C_003E4__this.Bestower.TrueCenter()) / 2f;
				_003C_003E4__this.mote = MoteMaker.MakeStaticMote(loc, _003C_003E4__this.pawn.Map, ThingDefOf.Mote_Bestow);
			}
			_003C_003E4__this.mote?.Maintain();
			if ((_003C_003E4__this.sound == null || _003C_003E4__this.sound.Ended) && waitToil.actor.jobs.curDriver.ticksLeftThisToil <= 307)
			{
				_003C_003E4__this.sound = SoundDefOf.Bestowing_Warmup.TrySpawnSustainer(SoundInfo.InMap(new TargetInfo(_003C_003E4__this.pawn.Position, _003C_003E4__this.pawn.Map), MaintenanceType.PerTick));
			}
			if (_003C_003E4__this.sound != null)
			{
				_003C_003E4__this.sound.Maintain();
			}
		}

		internal void _003CMakeNewToils_003Eb__5()
		{
			_003C_003E4__this.CeremonyJob.FinishCeremony(_003C_003E4__this.pawn);
			FleckMaker.Static((_003C_003E4__this.pawn.TrueCenter() + _003C_003E4__this.Bestower.TrueCenter()) / 2f, _003C_003E4__this.pawn.Map, FleckDefOf.PsycastAreaEffect, 2f);
			SoundDefOf.Bestowing_Finished.PlayOneShot(_003C_003E4__this.pawn);
		}
	}

	[CompilerGenerated]
	private sealed class _003CMakeNewToils_003Ed__15 : IEnumerable<Toil>, IEnumerable, IEnumerator<Toil>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Toil _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public JobDriver_BestowingCeremony _003C_003E4__this;

		private _003C_003Ec__DisplayClass15_0 _003C_003E8__1;

		Toil IEnumerator<Toil>.Current
		{
			[DebuggerHidden]
			get
			{
				return _003C_003E2__current;
			}
		}

		object IEnumerator.Current
		{
			[DebuggerHidden]
			get
			{
				return _003C_003E2__current;
			}
		}

		[DebuggerHidden]
		public _003CMakeNewToils_003Ed__15(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			_003C_003E8__1 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			int num = _003C_003E1__state;
			JobDriver_BestowingCeremony jobDriver_BestowingCeremony = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				_003C_003E8__1 = new _003C_003Ec__DisplayClass15_0();
				_003C_003E8__1._003C_003E4__this = _003C_003E4__this;
				if (!ModLister.CheckRoyalty("Bestowing ceremony"))
				{
					return false;
				}
				jobDriver_BestowingCeremony.AddFailCondition(() => _003C_003E8__1._003C_003E4__this.Bestower.GetLord() == null || _003C_003E8__1._003C_003E4__this.Bestower.GetLord().CurLordToil == null || !(_003C_003E8__1._003C_003E4__this.Bestower.GetLord().CurLordToil is LordToil_BestowingCeremony_Perform));
				_003C_003E2__current = Toils_Goto.GotoCell(TargetIndex.B, PathEndMode.OnCell);
				_003C_003E1__state = 1;
				return true;
			case 1:
				_003C_003E1__state = -1;
				_003C_003E8__1.waitToil = Toils_General.Wait(5000);
				_003C_003E8__1.waitToil.AddPreInitAction(delegate
				{
					Messages.Message("MessageBestowingCeremonyStarted".Translate(_003C_003E8__1._003C_003E4__this.pawn.Named("PAWN")), _003C_003E8__1._003C_003E4__this.Bestower, MessageTypeDefOf.PositiveEvent);
				});
				_003C_003E8__1.waitToil.AddPreInitAction(delegate
				{
					if (!AnalyzeThroneRoom(_003C_003E8__1._003C_003E4__this.Bestower, _003C_003E8__1._003C_003E4__this.pawn))
					{
						Messages.Message("BestowingCeremonyThroneroomRequirementsNotSatisfied".Translate(_003C_003E8__1._003C_003E4__this.pawn.Named("PAWN"), _003C_003E8__1._003C_003E4__this.pawn.royalty.GetTitleAwardedWhenUpdating(_003C_003E8__1._003C_003E4__this.Bestower.Faction, _003C_003E8__1._003C_003E4__this.pawn.royalty.GetFavor(_003C_003E8__1._003C_003E4__this.Bestower.Faction)).label.Named("TITLE")), _003C_003E8__1._003C_003E4__this.pawn, MessageTypeDefOf.NegativeEvent);
						((LordJob_BestowingCeremony)_003C_003E8__1._003C_003E4__this.Bestower.GetLord().LordJob).MakeCeremonyFail();
					}
				});
				_003C_003E8__1.waitToil.AddPreInitAction(delegate
				{
					SoundDefOf.Bestowing_Start.PlayOneShot(_003C_003E8__1._003C_003E4__this.pawn);
				});
				_003C_003E8__1.waitToil.tickAction = delegate
				{
					_003C_003E8__1._003C_003E4__this.pawn.rotationTracker.FaceTarget(_003C_003E8__1._003C_003E4__this.Bestower);
					if (_003C_003E8__1._003C_003E4__this.mote == null || _003C_003E8__1._003C_003E4__this.mote.Destroyed)
					{
						Vector3 loc = (_003C_003E8__1._003C_003E4__this.pawn.TrueCenter() + _003C_003E8__1._003C_003E4__this.Bestower.TrueCenter()) / 2f;
						_003C_003E8__1._003C_003E4__this.mote = MoteMaker.MakeStaticMote(loc, _003C_003E8__1._003C_003E4__this.pawn.Map, ThingDefOf.Mote_Bestow);
					}
					_003C_003E8__1._003C_003E4__this.mote?.Maintain();
					if ((_003C_003E8__1._003C_003E4__this.sound == null || _003C_003E8__1._003C_003E4__this.sound.Ended) && _003C_003E8__1.waitToil.actor.jobs.curDriver.ticksLeftThisToil <= 307)
					{
						_003C_003E8__1._003C_003E4__this.sound = SoundDefOf.Bestowing_Warmup.TrySpawnSustainer(SoundInfo.InMap(new TargetInfo(_003C_003E8__1._003C_003E4__this.pawn.Position, _003C_003E8__1._003C_003E4__this.pawn.Map), MaintenanceType.PerTick));
					}
					if (_003C_003E8__1._003C_003E4__this.sound != null)
					{
						_003C_003E8__1._003C_003E4__this.sound.Maintain();
					}
				};
				_003C_003E8__1.waitToil.handlingFacing = false;
				_003C_003E8__1.waitToil.socialMode = RandomSocialMode.Off;
				_003C_003E8__1.waitToil.WithProgressBarToilDelay(TargetIndex.A);
				_003C_003E2__current = _003C_003E8__1.waitToil;
				_003C_003E1__state = 2;
				return true;
			case 2:
				_003C_003E1__state = -1;
				_003C_003E2__current = Toils_General.Do(delegate
				{
					_003C_003E8__1._003C_003E4__this.CeremonyJob.FinishCeremony(_003C_003E8__1._003C_003E4__this.pawn);
					FleckMaker.Static((_003C_003E8__1._003C_003E4__this.pawn.TrueCenter() + _003C_003E8__1._003C_003E4__this.Bestower.TrueCenter()) / 2f, _003C_003E8__1._003C_003E4__this.pawn.Map, FleckDefOf.PsycastAreaEffect, 2f);
					SoundDefOf.Bestowing_Finished.PlayOneShot(_003C_003E8__1._003C_003E4__this.pawn);
				});
				_003C_003E1__state = 3;
				return true;
			case 3:
				_003C_003E1__state = -1;
				return false;
			}
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<Toil> IEnumerable<Toil>.GetEnumerator()
		{
			_003CMakeNewToils_003Ed__15 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CMakeNewToils_003Ed__15(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<Toil>)this).GetEnumerator();
		}
	}

	public const float WarmupSoundLength = 5.125f;

	public const int BestowTimeTicks = 5000;

	public const int PlayWarmupSoundAfterTicks = 307;

	protected const TargetIndex BestowerInd = TargetIndex.A;

	protected const TargetIndex BestowSpotInd = TargetIndex.B;

	private Mote mote;

	private Sustainer sound;

	private Pawn Bestower => base.TargetA.Pawn;

	private LordJob_BestowingCeremony CeremonyJob => (LordJob_BestowingCeremony)Bestower.GetLord().LordJob;

	private LocalTargetInfo BestowSpot => job.targetB;

	public static bool AnalyzeThroneRoom(Pawn bestower, Pawn target)
	{
		RoyalTitleDef titleAwardedWhenUpdating = target.royalty.GetTitleAwardedWhenUpdating(bestower.Faction, target.royalty.GetFavor(bestower.Faction));
		if (titleAwardedWhenUpdating != null && titleAwardedWhenUpdating.throneRoomRequirements != null)
		{
			foreach (RoomRequirement throneRoomRequirement in titleAwardedWhenUpdating.throneRoomRequirements)
			{
				if (!throneRoomRequirement.MetOrDisabled(bestower.GetRoom(), target))
				{
					return false;
				}
			}
		}
		return true;
	}

	public override bool TryMakePreToilReservations(bool errorOnFailed)
	{
		Bestower.ClearAllReservations();
		if (pawn.Reserve(BestowSpot, job, 1, -1, null, errorOnFailed))
		{
			return pawn.Reserve(Bestower, job, 1, -1, null, errorOnFailed);
		}
		return false;
	}

	[IteratorStateMachine(typeof(_003CMakeNewToils_003Ed__15))]
	protected override IEnumerable<Toil> MakeNewToils()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CMakeNewToils_003Ed__15(-2)
		{
			_003C_003E4__this = this
		};
	}
}
