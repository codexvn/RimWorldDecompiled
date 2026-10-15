using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Verse;
using Verse.AI;

namespace RimWorld;

public class FloatMenuOptionProvider_DraftedTend : FloatMenuOptionProvider
{
	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass13_0
	{
		public Pawn clickedPawn;

		public Thing medicine;

		public FloatMenuContext context;

		internal void _003CGetOptionsFor_003Eb__1()
		{
			Job job = JobMaker.MakeJob(JobDefOf.TendPatient, clickedPawn, null);
			job.count = 1;
			job.draftedTend = true;
			context.FirstSelectedPawn.jobs.TryTakeOrderedJob(job, JobTag.Misc);
		}
	}

	[CompilerGenerated]
	private sealed class _003CGetOptionsFor_003Ed__13 : IEnumerable<FloatMenuOption>, IEnumerable, IEnumerator<FloatMenuOption>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private FloatMenuOption _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private Pawn clickedPawn;

		public Pawn _003C_003E3__clickedPawn;

		private FloatMenuContext context;

		public FloatMenuContext _003C_003E3__context;

		public FloatMenuOptionProvider_DraftedTend _003C_003E4__this;

		private _003C_003Ec__DisplayClass13_0 _003C_003E8__1;

		FloatMenuOption IEnumerator<FloatMenuOption>.Current
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
		public _003CGetOptionsFor_003Ed__13(int _003C_003E1__state)
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
			FloatMenuOptionProvider_DraftedTend floatMenuOptionProvider_DraftedTend = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
			{
				_003C_003E1__state = -1;
				_003C_003E8__1 = new _003C_003Ec__DisplayClass13_0();
				_003C_003E8__1.clickedPawn = clickedPawn;
				_003C_003E8__1.context = context;
				if (!floatMenuOptionProvider_DraftedTend.IsValidTendTarget(_003C_003E8__1.context.FirstSelectedPawn, _003C_003E8__1.clickedPawn))
				{
					return false;
				}
				if (!_003C_003E8__1.clickedPawn.health.HasHediffsNeedingTend())
				{
					if (_003C_003E8__1.context.FirstSelectedPawn != _003C_003E8__1.clickedPawn)
					{
						_003C_003E2__current = new FloatMenuOption("CannotTend".Translate(_003C_003E8__1.clickedPawn) + ": " + "TendingNotRequired".Translate(_003C_003E8__1.clickedPawn), null);
						_003C_003E1__state = 1;
						return true;
					}
					goto IL_0137;
				}
				if (_003C_003E8__1.context.FirstSelectedPawn.WorkTypeIsDisabled(WorkTypeDefOf.Doctor))
				{
					_003C_003E2__current = new FloatMenuOption("CannotTend".Translate(_003C_003E8__1.clickedPawn) + ": " + "CannotPrioritizeWorkTypeDisabled".Translate(WorkTypeDefOf.Doctor.gerundLabel), null);
					_003C_003E1__state = 2;
					return true;
				}
				if (!_003C_003E8__1.context.FirstSelectedPawn.CanReach(_003C_003E8__1.clickedPawn, PathEndMode.ClosestTouch, Danger.Deadly))
				{
					_003C_003E2__current = new FloatMenuOption("CannotTend".Translate(_003C_003E8__1.clickedPawn) + ": " + "NoPath".Translate().CapitalizeFirst(), null);
					_003C_003E1__state = 3;
					return true;
				}
				if (_003C_003E8__1.clickedPawn == _003C_003E8__1.context.FirstSelectedPawn && _003C_003E8__1.context.FirstSelectedPawn.playerSettings != null && !_003C_003E8__1.context.FirstSelectedPawn.playerSettings.selfTend)
				{
					_003C_003E2__current = new FloatMenuOption("CannotTend".Translate(_003C_003E8__1.clickedPawn) + ": " + "SelfTendDisabled".Translate().CapitalizeFirst(), null);
					_003C_003E1__state = 4;
					return true;
				}
				if (_003C_003E8__1.clickedPawn.InAggroMentalState && !_003C_003E8__1.clickedPawn.health.hediffSet.HasHediff(HediffDefOf.Scaria))
				{
					_003C_003E2__current = new FloatMenuOption("CannotTend".Translate(_003C_003E8__1.clickedPawn) + ": " + "PawnIsInAggroMentalState".Translate(_003C_003E8__1.clickedPawn).CapitalizeFirst(), null);
					_003C_003E1__state = 5;
					return true;
				}
				_003C_003E8__1.medicine = HealthAIUtility.FindBestMedicine(_003C_003E8__1.context.FirstSelectedPawn, _003C_003E8__1.clickedPawn, onlyUseInventory: true);
				TaggedString taggedString = "Tend".Translate(_003C_003E8__1.clickedPawn);
				if (_003C_003E8__1.medicine == null)
				{
					taggedString += " (" + "WithoutMedicine".Translate() + ")";
				}
				_003C_003E2__current = FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption(taggedString, Action), _003C_003E8__1.context.FirstSelectedPawn, _003C_003E8__1.clickedPawn);
				_003C_003E1__state = 6;
				return true;
			}
			case 1:
				_003C_003E1__state = -1;
				goto IL_0137;
			case 2:
				_003C_003E1__state = -1;
				return false;
			case 3:
				_003C_003E1__state = -1;
				return false;
			case 4:
				_003C_003E1__state = -1;
				return false;
			case 5:
				_003C_003E1__state = -1;
				return false;
			case 6:
				_003C_003E1__state = -1;
				if (_003C_003E8__1.medicine != null && _003C_003E8__1.context.FirstSelectedPawn.CanReserve(_003C_003E8__1.clickedPawn) && _003C_003E8__1.clickedPawn.Spawned)
				{
					_003C_003E2__current = new FloatMenuOption("Tend".Translate(_003C_003E8__1.clickedPawn) + " (" + "WithoutMedicine".Translate() + ")", delegate
					{
						Job job = JobMaker.MakeJob(JobDefOf.TendPatient, _003C_003E8__1.clickedPawn, null);
						job.count = 1;
						job.draftedTend = true;
						_003C_003E8__1.context.FirstSelectedPawn.jobs.TryTakeOrderedJob(job, JobTag.Misc);
					});
					_003C_003E1__state = 7;
					return true;
				}
				break;
			case 7:
				{
					_003C_003E1__state = -1;
					break;
				}
				IL_0137:
				return false;
			}
			return false;
			void Action()
			{
				Job job2 = JobMaker.MakeJob(JobDefOf.TendPatient, ((_003C_003Ec__DisplayClass13_0)this).clickedPawn, ((_003C_003Ec__DisplayClass13_0)this).medicine);
				job2.count = 1;
				job2.draftedTend = true;
				((_003C_003Ec__DisplayClass13_0)this).context.FirstSelectedPawn.jobs.TryTakeOrderedJob(job2, JobTag.Misc);
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
		IEnumerator<FloatMenuOption> IEnumerable<FloatMenuOption>.GetEnumerator()
		{
			_003CGetOptionsFor_003Ed__13 _003CGetOptionsFor_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CGetOptionsFor_003Ed__ = this;
			}
			else
			{
				_003CGetOptionsFor_003Ed__ = new _003CGetOptionsFor_003Ed__13(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			_003CGetOptionsFor_003Ed__.clickedPawn = _003C_003E3__clickedPawn;
			_003CGetOptionsFor_003Ed__.context = _003C_003E3__context;
			return _003CGetOptionsFor_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<FloatMenuOption>)this).GetEnumerator();
		}
	}

	protected override bool Drafted => true;

	protected override bool Undrafted => false;

	protected override bool Multiselect => false;

	protected override bool MechanoidCanDo => true;

	protected override bool RequiresManipulation => true;

	protected override bool CanSelfTarget => true;

	protected override bool AppliesInt(FloatMenuContext context)
	{
		if (context.FirstSelectedPawn.IsMutant)
		{
			return context.FirstSelectedPawn.mutant.Def.canTend;
		}
		return true;
	}

	[IteratorStateMachine(typeof(_003CGetOptionsFor_003Ed__13))]
	public override IEnumerable<FloatMenuOption> GetOptionsFor(Pawn clickedPawn, FloatMenuContext context)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetOptionsFor_003Ed__13(-2)
		{
			_003C_003E4__this = this,
			_003C_003E3__clickedPawn = clickedPawn,
			_003C_003E3__context = context
		};
	}

	private bool IsValidTendTarget(Pawn doctor, Pawn patient)
	{
		if (!doctor.Drafted && patient != doctor)
		{
			return false;
		}
		if (patient.Downed)
		{
			return true;
		}
		if (patient.HostileTo(doctor.Faction))
		{
			return false;
		}
		if (patient.IsColonist || patient.IsQuestLodger() || patient.IsPrisonerOfColony || patient.IsSlaveOfColony || (patient.Faction == Faction.OfPlayer && patient.IsAnimal))
		{
			return true;
		}
		if (patient.IsColonySubhuman && patient.mutant.Def.entitledToMedicalCare)
		{
			return true;
		}
		return false;
	}

	protected override FloatMenuOption GetSingleOptionFor(Thing clickedThing, FloatMenuContext context)
	{
		Building_HoldingPlatform holdingPlatform = clickedThing as Building_HoldingPlatform;
		if (holdingPlatform == null)
		{
			return null;
		}
		Pawn heldPawn = holdingPlatform.HeldPawn;
		if (heldPawn == null)
		{
			return null;
		}
		if (!HealthAIUtility.ShouldBeTendedNowByPlayer(heldPawn))
		{
			return null;
		}
		if (context.FirstSelectedPawn.WorkTypeIsDisabled(WorkTypeDefOf.Doctor))
		{
			return new FloatMenuOption("CannotTend".Translate(heldPawn) + ": " + "CannotPrioritizeWorkTypeDisabled".Translate(WorkTypeDefOf.Doctor.gerundLabel), null);
		}
		if (!context.FirstSelectedPawn.CanReach(heldPawn, PathEndMode.ClosestTouch, Danger.Deadly))
		{
			return new FloatMenuOption("CannotTend".Translate(heldPawn) + ": " + "NoPath".Translate().CapitalizeFirst(), null);
		}
		Thing medicine = HealthAIUtility.FindBestMedicine(context.FirstSelectedPawn, heldPawn);
		return FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption("Tend".Translate(heldPawn.LabelShort), delegate
		{
			JobDef tendEntity = JobDefOf.TendEntity;
			LocalTargetInfo targetA = holdingPlatform;
			Thing thing = medicine;
			Job job = JobMaker.MakeJob(tendEntity, targetA, (thing != null) ? ((LocalTargetInfo)thing) : LocalTargetInfo.Invalid);
			job.count = 1;
			job.draftedTend = true;
			context.FirstSelectedPawn.jobs.TryTakeOrderedJob(job, JobTag.Misc);
		}), context.FirstSelectedPawn, holdingPlatform);
	}
}
