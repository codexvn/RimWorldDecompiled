using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using RimWorld.Planet;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld;

public class FloatMenuOptionProvider_LoadOntoPackAnimal : FloatMenuOptionProvider
{
	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass7_0
	{
		public Thing clickedThing;

		public FloatMenuContext context;

		public Pawn bestPackAnimal;

		public Action<int> _003C_003E9__3;

		internal void _003CGetOptionsFor_003Eb__0()
		{
			clickedThing.SetForbidden(value: false, warnOnFail: false);
			Job job = JobMaker.MakeJob(JobDefOf.GiveToPackAnimal, clickedThing);
			job.count = 1;
			context.FirstSelectedPawn.jobs.TryTakeOrderedJob(job, JobTag.Misc);
		}

		internal void _003CGetOptionsFor_003Eb__1()
		{
			clickedThing.SetForbidden(value: false, warnOnFail: false);
			Job job = JobMaker.MakeJob(JobDefOf.GiveToPackAnimal, clickedThing);
			job.count = clickedThing.stackCount;
			context.FirstSelectedPawn.jobs.TryTakeOrderedJob(job, JobTag.Misc);
		}

		internal void _003CGetOptionsFor_003Eb__2()
		{
			int to = Mathf.Min(MassUtility.CountToPickUpUntilOverEncumbered(bestPackAnimal, clickedThing), clickedThing.stackCount);
			Dialog_Slider window = new Dialog_Slider("GiveToPackAnimalCount".Translate(clickedThing.LabelNoCount, clickedThing), 1, to, delegate(int count)
			{
				clickedThing.SetForbidden(value: false, warnOnFail: false);
				Job job = JobMaker.MakeJob(JobDefOf.GiveToPackAnimal, clickedThing);
				job.count = count;
				context.FirstSelectedPawn.jobs.TryTakeOrderedJob(job, JobTag.Misc);
			});
			Find.WindowStack.Add(window);
		}

		internal void _003CGetOptionsFor_003Eb__3(int count)
		{
			clickedThing.SetForbidden(value: false, warnOnFail: false);
			Job job = JobMaker.MakeJob(JobDefOf.GiveToPackAnimal, clickedThing);
			job.count = count;
			context.FirstSelectedPawn.jobs.TryTakeOrderedJob(job, JobTag.Misc);
		}
	}

	[CompilerGenerated]
	private sealed class _003CGetOptionsFor_003Ed__7 : IEnumerable<FloatMenuOption>, IEnumerable, IEnumerator<FloatMenuOption>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private FloatMenuOption _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private Thing clickedThing;

		public Thing _003C_003E3__clickedThing;

		private FloatMenuContext context;

		public FloatMenuContext _003C_003E3__context;

		private _003C_003Ec__DisplayClass7_0 _003C_003E8__1;

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
		public _003CGetOptionsFor_003Ed__7(int _003C_003E1__state)
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
			switch (_003C_003E1__state)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				_003C_003E8__1 = new _003C_003Ec__DisplayClass7_0();
				_003C_003E8__1.clickedThing = clickedThing;
				_003C_003E8__1.context = context;
				if (_003C_003E8__1.clickedThing.def.category != ThingCategory.Item)
				{
					return false;
				}
				if (!_003C_003E8__1.clickedThing.def.EverHaulable)
				{
					return false;
				}
				_003C_003E8__1.bestPackAnimal = GiveToPackAnimalUtility.UsablePackAnimalWithTheMostFreeSpace(_003C_003E8__1.context.FirstSelectedPawn);
				if (_003C_003E8__1.bestPackAnimal == null)
				{
					return false;
				}
				if (!_003C_003E8__1.context.FirstSelectedPawn.CanReach(_003C_003E8__1.clickedThing, PathEndMode.ClosestTouch, Danger.Deadly))
				{
					_003C_003E2__current = new FloatMenuOption("CannotGiveToPackAnimal".Translate(_003C_003E8__1.clickedThing.Label, _003C_003E8__1.clickedThing) + ": " + "NoPath".Translate().CapitalizeFirst(), null);
					_003C_003E1__state = 1;
					return true;
				}
				if (MassUtility.WillBeOverEncumberedAfterPickingUp(_003C_003E8__1.bestPackAnimal, _003C_003E8__1.clickedThing, 1))
				{
					_003C_003E2__current = new FloatMenuOption("CannotGiveToPackAnimal".Translate(_003C_003E8__1.clickedThing.Label, _003C_003E8__1.clickedThing) + ": " + "TooHeavy".Translate(), null);
					_003C_003E1__state = 2;
					return true;
				}
				if (_003C_003E8__1.clickedThing.stackCount == 1)
				{
					_003C_003E2__current = FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption("GiveToPackAnimal".Translate(_003C_003E8__1.clickedThing.Label, _003C_003E8__1.clickedThing), delegate
					{
						_003C_003E8__1.clickedThing.SetForbidden(value: false, warnOnFail: false);
						Job job2 = JobMaker.MakeJob(JobDefOf.GiveToPackAnimal, _003C_003E8__1.clickedThing);
						job2.count = 1;
						_003C_003E8__1.context.FirstSelectedPawn.jobs.TryTakeOrderedJob(job2, JobTag.Misc);
					}, MenuOptionPriority.High), _003C_003E8__1.context.FirstSelectedPawn, _003C_003E8__1.clickedThing);
					_003C_003E1__state = 3;
					return true;
				}
				if (MassUtility.WillBeOverEncumberedAfterPickingUp(_003C_003E8__1.bestPackAnimal, _003C_003E8__1.clickedThing, _003C_003E8__1.clickedThing.stackCount))
				{
					_003C_003E2__current = new FloatMenuOption("CannotGiveToPackAnimalAll".Translate(_003C_003E8__1.clickedThing.Label, _003C_003E8__1.clickedThing) + ": " + "TooHeavy".Translate(), null);
					_003C_003E1__state = 4;
					return true;
				}
				_003C_003E2__current = FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption("GiveToPackAnimalAll".Translate(_003C_003E8__1.clickedThing.Label, _003C_003E8__1.clickedThing), delegate
				{
					_003C_003E8__1.clickedThing.SetForbidden(value: false, warnOnFail: false);
					Job job3 = JobMaker.MakeJob(JobDefOf.GiveToPackAnimal, _003C_003E8__1.clickedThing);
					job3.count = _003C_003E8__1.clickedThing.stackCount;
					_003C_003E8__1.context.FirstSelectedPawn.jobs.TryTakeOrderedJob(job3, JobTag.Misc);
				}, MenuOptionPriority.High), _003C_003E8__1.context.FirstSelectedPawn, _003C_003E8__1.clickedThing);
				_003C_003E1__state = 5;
				return true;
			case 1:
				_003C_003E1__state = -1;
				break;
			case 2:
				_003C_003E1__state = -1;
				break;
			case 3:
				_003C_003E1__state = -1;
				break;
			case 4:
				_003C_003E1__state = -1;
				goto IL_03fa;
			case 5:
				_003C_003E1__state = -1;
				goto IL_03fa;
			case 6:
				{
					_003C_003E1__state = -1;
					break;
				}
				IL_03fa:
				_003C_003E2__current = FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption("GiveToPackAnimalSome".Translate(_003C_003E8__1.clickedThing.LabelNoCount, _003C_003E8__1.clickedThing), delegate
				{
					int to = Mathf.Min(MassUtility.CountToPickUpUntilOverEncumbered(_003C_003E8__1.bestPackAnimal, _003C_003E8__1.clickedThing), _003C_003E8__1.clickedThing.stackCount);
					Dialog_Slider window = new Dialog_Slider("GiveToPackAnimalCount".Translate(_003C_003E8__1.clickedThing.LabelNoCount, _003C_003E8__1.clickedThing), 1, to, delegate(int count)
					{
						_003C_003E8__1.clickedThing.SetForbidden(value: false, warnOnFail: false);
						Job job = JobMaker.MakeJob(JobDefOf.GiveToPackAnimal, _003C_003E8__1.clickedThing);
						job.count = count;
						_003C_003E8__1.context.FirstSelectedPawn.jobs.TryTakeOrderedJob(job, JobTag.Misc);
					});
					Find.WindowStack.Add(window);
				}, MenuOptionPriority.High), _003C_003E8__1.context.FirstSelectedPawn, _003C_003E8__1.clickedThing);
				_003C_003E1__state = 6;
				return true;
			}
			return false;
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
			_003CGetOptionsFor_003Ed__7 _003CGetOptionsFor_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CGetOptionsFor_003Ed__ = this;
			}
			else
			{
				_003CGetOptionsFor_003Ed__ = new _003CGetOptionsFor_003Ed__7(0);
			}
			_003CGetOptionsFor_003Ed__.clickedThing = _003C_003E3__clickedThing;
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

	protected override bool Undrafted => true;

	protected override bool Multiselect => false;

	protected override bool AppliesInt(FloatMenuContext context)
	{
		if (!context.FirstSelectedPawn.IsFormingCaravan())
		{
			return !context.map.IsPlayerHome;
		}
		return false;
	}

	[IteratorStateMachine(typeof(_003CGetOptionsFor_003Ed__7))]
	public override IEnumerable<FloatMenuOption> GetOptionsFor(Thing clickedThing, FloatMenuContext context)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetOptionsFor_003Ed__7(-2)
		{
			_003C_003E3__clickedThing = clickedThing,
			_003C_003E3__context = context
		};
	}
}
