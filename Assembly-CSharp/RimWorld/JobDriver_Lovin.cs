using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld;

public class JobDriver_Lovin : JobDriver
{
	[CompilerGenerated]
	private sealed class _003CMakeNewToils_003Ed__12 : IEnumerable<Toil>, IEnumerable, IEnumerator<Toil>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Toil _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public JobDriver_Lovin _003C_003E4__this;

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
		public _003CMakeNewToils_003Ed__12(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			int num = _003C_003E1__state;
			JobDriver_Lovin CS_0024_003C_003E8__locals61 = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				CS_0024_003C_003E8__locals61.FailOnDespawnedOrNull(CS_0024_003C_003E8__locals61.BedInd);
				CS_0024_003C_003E8__locals61.FailOnDespawnedOrNull(CS_0024_003C_003E8__locals61.PartnerInd);
				CS_0024_003C_003E8__locals61.FailOn(() => !CS_0024_003C_003E8__locals61.Partner.health.capacities.CanBeAwake);
				CS_0024_003C_003E8__locals61.KeepLyingDown(CS_0024_003C_003E8__locals61.BedInd);
				_003C_003E2__current = Toils_Bed.ClaimBedIfNonMedical(CS_0024_003C_003E8__locals61.BedInd);
				_003C_003E1__state = 1;
				return true;
			case 1:
				_003C_003E1__state = -1;
				_003C_003E2__current = Toils_Bed.GotoBed(CS_0024_003C_003E8__locals61.BedInd);
				_003C_003E1__state = 2;
				return true;
			case 2:
			{
				_003C_003E1__state = -1;
				Toil toil2 = ToilMaker.MakeToil("MakeNewToils");
				toil2.initAction = delegate
				{
					if (CS_0024_003C_003E8__locals61.Partner.CurJob == null || CS_0024_003C_003E8__locals61.Partner.CurJob.def != JobDefOf.Lovin)
					{
						Job newJob = JobMaker.MakeJob(JobDefOf.Lovin, CS_0024_003C_003E8__locals61.pawn, CS_0024_003C_003E8__locals61.Bed);
						CS_0024_003C_003E8__locals61.Partner.jobs.StartJob(newJob, JobCondition.InterruptForced, null, resumeCurJobAfterwards: false, cancelBusyStances: true, null, null, fromQueue: false, canReturnCurJobToPool: false, null);
						CS_0024_003C_003E8__locals61.ticksLeft = (int)(2500f * Mathf.Clamp(Rand.Range(0.1f, 1.1f), 0.1f, 2f));
						Find.HistoryEventsManager.RecordEvent(new HistoryEvent(HistoryEventDefOf.InitiatedLovin, CS_0024_003C_003E8__locals61.pawn.Named(HistoryEventArgsNames.Doer)));
						if (InteractionWorker_RomanceAttempt.CanCreatePsychicBondBetween(CS_0024_003C_003E8__locals61.pawn, CS_0024_003C_003E8__locals61.Partner) && InteractionWorker_RomanceAttempt.TryCreatePsychicBondBetween(CS_0024_003C_003E8__locals61.pawn, CS_0024_003C_003E8__locals61.Partner) && (PawnUtility.ShouldSendNotificationAbout(CS_0024_003C_003E8__locals61.pawn) || PawnUtility.ShouldSendNotificationAbout(CS_0024_003C_003E8__locals61.Partner)))
						{
							Find.LetterStack.ReceiveLetter("LetterPsychicBondCreatedLovinLabel".Translate(), "LetterPsychicBondCreatedLovinText".Translate(CS_0024_003C_003E8__locals61.pawn.Named("BONDPAWN"), CS_0024_003C_003E8__locals61.Partner.Named("OTHERPAWN")), LetterDefOf.PositiveEvent, new LookTargets(CS_0024_003C_003E8__locals61.pawn, CS_0024_003C_003E8__locals61.Partner));
						}
					}
					else
					{
						CS_0024_003C_003E8__locals61.ticksLeft = 9999999;
					}
				};
				toil2.defaultCompleteMode = ToilCompleteMode.Instant;
				_003C_003E2__current = toil2;
				_003C_003E1__state = 3;
				return true;
			}
			case 3:
			{
				_003C_003E1__state = -1;
				Toil toil = Toils_LayDown.LayDown(CS_0024_003C_003E8__locals61.BedInd, hasBed: true, lookForOtherJobs: false, canSleep: false, gainRestAndHealth: false);
				toil.FailOn(() => CS_0024_003C_003E8__locals61.Partner.CurJob == null || CS_0024_003C_003E8__locals61.Partner.CurJob.def != JobDefOf.Lovin);
				toil.AddPreTickIntervalAction(delegate(int delta)
				{
					CS_0024_003C_003E8__locals61.ticksLeft -= delta;
					if (CS_0024_003C_003E8__locals61.ticksLeft <= 0)
					{
						CS_0024_003C_003E8__locals61.ReadyForNextToil();
					}
					else if (CS_0024_003C_003E8__locals61.pawn.IsHashIntervalTick(100, delta))
					{
						FleckMaker.ThrowMetaIcon(CS_0024_003C_003E8__locals61.pawn.Position, CS_0024_003C_003E8__locals61.pawn.Map, FleckDefOf.Heart);
					}
				});
				toil.AddFinishAction(delegate
				{
					Thought_Memory thought_Memory = (Thought_Memory)ThoughtMaker.MakeThought(ThoughtDefOf.GotSomeLovin);
					if ((CS_0024_003C_003E8__locals61.pawn.health != null && CS_0024_003C_003E8__locals61.pawn.health.hediffSet != null && CS_0024_003C_003E8__locals61.pawn.health.hediffSet.hediffs.Any((Hediff h) => h.def == HediffDefOf.LoveEnhancer)) || (CS_0024_003C_003E8__locals61.Partner.health != null && CS_0024_003C_003E8__locals61.Partner.health.hediffSet != null && CS_0024_003C_003E8__locals61.Partner.health.hediffSet.hediffs.Any((Hediff h) => h.def == HediffDefOf.LoveEnhancer)))
					{
						thought_Memory.moodPowerFactor = 1.5f;
					}
					if (CS_0024_003C_003E8__locals61.pawn.needs.mood != null)
					{
						CS_0024_003C_003E8__locals61.pawn.needs.mood.thoughts.memories.TryGainMemory(thought_Memory, CS_0024_003C_003E8__locals61.Partner);
					}
					Find.HistoryEventsManager.RecordEvent(new HistoryEvent(HistoryEventDefOf.GotLovin, CS_0024_003C_003E8__locals61.pawn.Named(HistoryEventArgsNames.Doer)));
					HistoryEventDef def = (CS_0024_003C_003E8__locals61.pawn.relations.DirectRelationExists(PawnRelationDefOf.Spouse, CS_0024_003C_003E8__locals61.Partner) ? HistoryEventDefOf.GotLovin_Spouse : HistoryEventDefOf.GotLovin_NonSpouse);
					Find.HistoryEventsManager.RecordEvent(new HistoryEvent(def, CS_0024_003C_003E8__locals61.pawn.Named(HistoryEventArgsNames.Doer)));
					CS_0024_003C_003E8__locals61.pawn.mindState.canLovinTick = Find.TickManager.TicksGame + CS_0024_003C_003E8__locals61.GenerateRandomMinTicksToNextLovin(CS_0024_003C_003E8__locals61.pawn);
					if (ModsConfig.BiotechActive)
					{
						Pawn pawn = ((CS_0024_003C_003E8__locals61.pawn.gender == Gender.Male) ? CS_0024_003C_003E8__locals61.pawn : ((CS_0024_003C_003E8__locals61.Partner.gender == Gender.Male) ? CS_0024_003C_003E8__locals61.Partner : null));
						Pawn pawn2 = ((CS_0024_003C_003E8__locals61.pawn.gender == Gender.Female) ? CS_0024_003C_003E8__locals61.pawn : ((CS_0024_003C_003E8__locals61.Partner.gender == Gender.Female) ? CS_0024_003C_003E8__locals61.Partner : null));
						if (pawn != null && pawn2 != null && Rand.Chance(PregnancyChance * PregnancyUtility.PregnancyChanceForPartners(pawn2, pawn)))
						{
							bool success;
							GeneSet inheritedGeneSet = PregnancyUtility.GetInheritedGeneSet(pawn, pawn2, out success);
							if (success)
							{
								Hediff_Pregnant hediff_Pregnant = (Hediff_Pregnant)HediffMaker.MakeHediff(HediffDefOf.PregnantHuman, pawn2);
								hediff_Pregnant.SetParents(null, pawn, inheritedGeneSet);
								pawn2.health.AddHediff(hediff_Pregnant, null, null);
							}
							else if (PawnUtility.ShouldSendNotificationAbout(pawn) || PawnUtility.ShouldSendNotificationAbout(pawn2))
							{
								Messages.Message("MessagePregnancyFailed".Translate(pawn.Named("FATHER"), pawn2.Named("MOTHER")) + ": " + "CombinedGenesExceedMetabolismLimits".Translate(), new LookTargets(pawn, pawn2), MessageTypeDefOf.NegativeEvent);
							}
						}
					}
				});
				toil.socialMode = RandomSocialMode.Off;
				_003C_003E2__current = toil;
				_003C_003E1__state = 4;
				return true;
			}
			case 4:
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
			_003CMakeNewToils_003Ed__12 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CMakeNewToils_003Ed__12(0)
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

	private int ticksLeft;

	private TargetIndex PartnerInd = TargetIndex.A;

	private TargetIndex BedInd = TargetIndex.B;

	private const int TicksBetweenHeartMotes = 100;

	private static float PregnancyChance = 0.05f;

	private static readonly SimpleCurve LovinIntervalHoursFromAgeCurve = new SimpleCurve
	{
		new CurvePoint(16f, 1.5f),
		new CurvePoint(22f, 1.5f),
		new CurvePoint(30f, 4f),
		new CurvePoint(50f, 12f),
		new CurvePoint(75f, 36f)
	};

	private Pawn Partner => (Pawn)(Thing)job.GetTarget(PartnerInd);

	private Building_Bed Bed => (Building_Bed)(Thing)job.GetTarget(BedInd);

	public override void ExposeData()
	{
		base.ExposeData();
		Scribe_Values.Look(ref ticksLeft, "ticksLeft", 0);
	}

	public override bool TryMakePreToilReservations(bool errorOnFailed)
	{
		if (pawn.Reserve(Partner, job, 1, -1, null, errorOnFailed))
		{
			return pawn.Reserve(Bed, job, Bed.SleepingSlotsCount, 0, null, errorOnFailed);
		}
		return false;
	}

	public override bool CanBeginNowWhileLyingDown()
	{
		return JobInBedUtility.InBedOrRestSpotNow(pawn, job.GetTarget(BedInd));
	}

	[IteratorStateMachine(typeof(_003CMakeNewToils_003Ed__12))]
	protected override IEnumerable<Toil> MakeNewToils()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CMakeNewToils_003Ed__12(-2)
		{
			_003C_003E4__this = this
		};
	}

	private int GenerateRandomMinTicksToNextLovin(Pawn pawn)
	{
		if (DebugSettings.alwaysDoLovin)
		{
			return 100;
		}
		float num = LovinIntervalHoursFromAgeCurve.Evaluate(pawn.ageTracker.AgeBiologicalYearsFloat);
		if (ModsConfig.BiotechActive && pawn.genes != null)
		{
			foreach (Gene item in pawn.genes.GenesListForReading)
			{
				num *= item.def.lovinMTBFactor;
			}
		}
		foreach (Hediff hediff in pawn.health.hediffSet.hediffs)
		{
			HediffComp_GiveLovinMTBFactor hediffComp_GiveLovinMTBFactor = hediff.TryGetComp<HediffComp_GiveLovinMTBFactor>();
			if (hediffComp_GiveLovinMTBFactor != null)
			{
				num *= hediffComp_GiveLovinMTBFactor.Props.lovinMTBFactor;
			}
		}
		num = Rand.Gaussian(num, 0.3f);
		if (num < 0.5f)
		{
			num = 0.5f;
		}
		return (int)(num * 2500f);
	}
}
