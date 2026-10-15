using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.Sound;

namespace RimWorld;

public class JobDriver_Meditate : JobDriver
{
	[CompilerGenerated]
	private sealed class _003CMakeNewToils_003Ed__16 : IEnumerable<Toil>, IEnumerable, IEnumerator<Toil>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Toil _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public JobDriver_Meditate _003C_003E4__this;

		private Toil _003Cmeditate_003E5__2;

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
		public _003CMakeNewToils_003Ed__16(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			_003Cmeditate_003E5__2 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			int num = _003C_003E1__state;
			JobDriver_Meditate CS_0024_003C_003E8__locals40 = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				_003Cmeditate_003E5__2 = ToilMaker.MakeToil("MakeNewToils");
				_003Cmeditate_003E5__2.socialMode = RandomSocialMode.Off;
				if (CS_0024_003C_003E8__locals40.FromBed)
				{
					CS_0024_003C_003E8__locals40.KeepLyingDown(TargetIndex.B);
					_003Cmeditate_003E5__2 = Toils_LayDown.LayDown(TargetIndex.B, CS_0024_003C_003E8__locals40.job.GetTarget(TargetIndex.B).Thing is Building_Bed, lookForOtherJobs: false, canSleep: false);
					goto IL_0128;
				}
				_003C_003E2__current = Toils_Goto.GotoCell(TargetIndex.A, PathEndMode.OnCell);
				_003C_003E1__state = 1;
				return true;
			case 1:
				_003C_003E1__state = -1;
				_003Cmeditate_003E5__2.initAction = delegate
				{
					LocalTargetInfo target = CS_0024_003C_003E8__locals40.job.GetTarget(TargetIndex.C);
					if (target.IsValid)
					{
						CS_0024_003C_003E8__locals40.faceDir = target.Cell - CS_0024_003C_003E8__locals40.pawn.Position;
					}
					else
					{
						CS_0024_003C_003E8__locals40.faceDir = (CS_0024_003C_003E8__locals40.job.def.faceDir.IsValid ? CS_0024_003C_003E8__locals40.job.def.faceDir : Rot4.Random).FacingCell;
					}
				};
				if (CS_0024_003C_003E8__locals40.Focus != null)
				{
					_003Cmeditate_003E5__2.FailOnDespawnedOrNull(TargetIndex.C);
					_003Cmeditate_003E5__2.FailOnForbidden(TargetIndex.A);
					if (CS_0024_003C_003E8__locals40.pawn.HasPsylink && CS_0024_003C_003E8__locals40.Focus.Thing != null)
					{
						_003Cmeditate_003E5__2.FailOn(() => CS_0024_003C_003E8__locals40.Focus.Thing.GetStatValueForPawn(StatDefOf.MeditationFocusStrength, CS_0024_003C_003E8__locals40.pawn) < float.Epsilon);
					}
				}
				_003Cmeditate_003E5__2.handlingFacing = true;
				goto IL_0128;
			case 2:
				{
					_003C_003E1__state = -1;
					return false;
				}
				IL_0128:
				_003Cmeditate_003E5__2.defaultCompleteMode = ToilCompleteMode.Delay;
				_003Cmeditate_003E5__2.defaultDuration = CS_0024_003C_003E8__locals40.job.def.joyDuration;
				_003Cmeditate_003E5__2.FailOn(() => !MeditationUtility.CanMeditateNow(CS_0024_003C_003E8__locals40.pawn) || !MeditationUtility.SafeEnvironmentalConditions(CS_0024_003C_003E8__locals40.pawn, CS_0024_003C_003E8__locals40.TargetLocA, CS_0024_003C_003E8__locals40.Map));
				_003Cmeditate_003E5__2.AddPreTickAction(delegate
				{
					bool flag = CS_0024_003C_003E8__locals40.pawn.GetTimeAssignment() == TimeAssignmentDefOf.Meditate;
					if (CS_0024_003C_003E8__locals40.job.ignoreJoyTimeAssignment)
					{
						Pawn_PsychicEntropyTracker psychicEntropy = CS_0024_003C_003E8__locals40.pawn.psychicEntropy;
						bool flag2 = !flag && CS_0024_003C_003E8__locals40.job.wasOnMeditationTimeAssignment;
						if (CS_0024_003C_003E8__locals40.pawn.IsHashIntervalTick(4000) && psychicEntropy != null && !flag && psychicEntropy.CurrentPsyfocus >= Mathf.Max(psychicEntropy.TargetPsyfocus + 0.05f, 0.99f))
						{
							CS_0024_003C_003E8__locals40.pawn.jobs.CheckForJobOverride();
							return;
						}
						if (flag2 && psychicEntropy.TargetPsyfocus < psychicEntropy.CurrentPsyfocus)
						{
							CS_0024_003C_003E8__locals40.EndJobWith(JobCondition.InterruptForced);
							return;
						}
						CS_0024_003C_003E8__locals40.job.psyfocusTargetLast = psychicEntropy.TargetPsyfocus;
						CS_0024_003C_003E8__locals40.job.wasOnMeditationTimeAssignment = flag;
					}
					else if (CS_0024_003C_003E8__locals40.pawn.needs.joy.CurLevelPercentage >= 1f)
					{
						CS_0024_003C_003E8__locals40.EndJobWith(JobCondition.InterruptForced);
						return;
					}
					if (CS_0024_003C_003E8__locals40.faceDir.IsValid && !CS_0024_003C_003E8__locals40.FromBed)
					{
						CS_0024_003C_003E8__locals40.pawn.rotationTracker.FaceCell(CS_0024_003C_003E8__locals40.pawn.Position + CS_0024_003C_003E8__locals40.faceDir);
					}
					CS_0024_003C_003E8__locals40.MeditationTick();
					if (ModsConfig.RoyaltyActive && MeditationFocusDefOf.Natural.CanPawnUse(CS_0024_003C_003E8__locals40.pawn))
					{
						int num2 = GenRadial.NumCellsInRadius(MeditationUtility.FocusObjectSearchRadius);
						for (int i = 0; i < num2; i++)
						{
							IntVec3 c = CS_0024_003C_003E8__locals40.pawn.Position + GenRadial.RadialPattern[i];
							if (c.InBounds(CS_0024_003C_003E8__locals40.pawn.Map))
							{
								Plant plant = c.GetPlant(CS_0024_003C_003E8__locals40.pawn.Map);
								if (plant != null && plant.def == ThingDefOf.Plant_TreeAnima)
								{
									plant.TryGetComp<CompSpawnSubplant>()?.AddProgress(AnimaTreeSubplantProgressPerTick);
								}
							}
						}
					}
				});
				_003C_003E2__current = _003Cmeditate_003E5__2;
				_003C_003E1__state = 2;
				return true;
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
			_003CMakeNewToils_003Ed__16 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CMakeNewToils_003Ed__16(0)
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

	protected IntVec3 faceDir;

	private Mote psyfocusMote;

	protected Sustainer sustainer;

	protected const TargetIndex SpotInd = TargetIndex.A;

	protected const TargetIndex BedInd = TargetIndex.B;

	protected const TargetIndex FocusInd = TargetIndex.C;

	public static float AnimaTreeSubplantProgressPerTick = 6.666667E-05f;

	private const int TicksBetweenMotesBase = 100;

	private const int JobCheckIntervalTicks = 4000;

	public LocalTargetInfo Focus => job.GetTarget(TargetIndex.C);

	private bool FromBed => job.GetTarget(TargetIndex.B).IsValid;

	protected string PsyfocusPerDayReport()
	{
		if (!pawn.HasPsylink)
		{
			return "";
		}
		Thing thing = Focus.Thing;
		float f = MeditationUtility.PsyfocusGainPerTick(pawn, thing) * 60000f;
		return "\n" + "PsyfocusPerDayOfMeditation".Translate(f.ToStringPercent()).CapitalizeFirst();
	}

	public override string GetReport()
	{
		if (ModsConfig.RoyaltyActive)
		{
			Thing thing = Focus.Thing;
			if (thing != null && !thing.Destroyed)
			{
				return "MeditatingAt".Translate() + ": " + thing.LabelShort.CapitalizeFirst() + "." + PsyfocusPerDayReport();
			}
			return base.GetReport() + PsyfocusPerDayReport();
		}
		return base.GetReport();
	}

	public override bool TryMakePreToilReservations(bool errorOnFailed)
	{
		return pawn.Reserve(job.GetTarget(TargetIndex.A), job, 1, -1, null, errorOnFailed);
	}

	[IteratorStateMachine(typeof(_003CMakeNewToils_003Ed__16))]
	protected override IEnumerable<Toil> MakeNewToils()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CMakeNewToils_003Ed__16(-2)
		{
			_003C_003E4__this = this
		};
	}

	public override void Notify_Starting()
	{
		base.Notify_Starting();
		if (ModsConfig.RoyaltyActive && pawn.psychicEntropy != null)
		{
			job.psyfocusTargetLast = pawn.psychicEntropy.TargetPsyfocus;
		}
	}

	protected void MeditationTick()
	{
		pawn.skills.Learn(SkillDefOf.Intellectual, 0.018000001f);
		pawn.GainComfortFromCellIfPossible(1);
		if (pawn.needs.joy != null)
		{
			JoyUtility.JoyTickCheckEnd(pawn, 1, JoyTickFullJoyAction.None);
		}
		if (pawn.IsHashIntervalTick(100))
		{
			FleckMaker.ThrowMetaIcon(pawn.Position, pawn.Map, FleckDefOf.Meditating);
		}
		if (!ModsConfig.RoyaltyActive || pawn.psychicEntropy == null)
		{
			return;
		}
		pawn.psychicEntropy.Notify_Meditated();
		if (pawn.HasPsylink && pawn.psychicEntropy.PsychicSensitivity > float.Epsilon)
		{
			float yOffset = (float)(pawn.Position.x % 2 + pawn.Position.z % 2) / 10f;
			if (psyfocusMote == null || psyfocusMote.Destroyed)
			{
				psyfocusMote = MoteMaker.MakeAttachedOverlay(pawn, ThingDefOf.Mote_PsyfocusPulse, Vector3.zero);
				psyfocusMote.yOffset = yOffset;
			}
			psyfocusMote.Maintain();
			if (sustainer == null || sustainer.Ended)
			{
				sustainer = SoundDefOf.MeditationGainPsyfocus.TrySpawnSustainer(SoundInfo.InMap(pawn, MaintenanceType.PerTick));
			}
			sustainer.Maintain();
			pawn.psychicEntropy.GainPsyfocus_NewTemp(1, Focus.Thing);
		}
	}

	public override void ExposeData()
	{
		base.ExposeData();
		Scribe_Values.Look(ref faceDir, "faceDir");
	}
}
