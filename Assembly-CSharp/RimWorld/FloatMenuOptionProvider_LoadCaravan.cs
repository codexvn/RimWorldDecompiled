using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using RimWorld.Planet;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld;

public class FloatMenuOptionProvider_LoadCaravan : FloatMenuOptionProvider
{
	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass7_0
	{
		public Thing clickedThing;

		public JobDef jobDef;

		public Pawn packTarget;

		public FloatMenuContext context;

		public Action<int> _003C_003E9__4;

		internal void _003CGetOptionsFor_003Eb__1()
		{
			clickedThing.SetForbidden(value: false, warnOnFail: false);
			Job job = JobMaker.MakeJob(jobDef, clickedThing);
			job.count = 1;
			job.checkEncumbrance = packTarget == context.FirstSelectedPawn;
			context.FirstSelectedPawn.jobs.TryTakeOrderedJob(job, JobTag.Misc);
		}

		internal void _003CGetOptionsFor_003Eb__2()
		{
			clickedThing.SetForbidden(value: false, warnOnFail: false);
			Job job = JobMaker.MakeJob(jobDef, clickedThing);
			job.count = clickedThing.stackCount;
			job.checkEncumbrance = packTarget == context.FirstSelectedPawn;
			context.FirstSelectedPawn.jobs.TryTakeOrderedJob(job, JobTag.Misc);
		}

		internal void _003CGetOptionsFor_003Eb__4(int count)
		{
			clickedThing.SetForbidden(value: false, warnOnFail: false);
			Job job = JobMaker.MakeJob(jobDef, clickedThing);
			job.count = count;
			job.checkEncumbrance = packTarget == context.FirstSelectedPawn;
			context.FirstSelectedPawn.jobs.TryTakeOrderedJob(job, JobTag.Misc);
		}
	}

	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass7_1
	{
		public float capacityLeft;

		public _003C_003Ec__DisplayClass7_0 CS_0024_003C_003E8__locals1;

		public Func<int, string> _003C_003E9__3;

		internal void _003CGetOptionsFor_003Eb__0()
		{
			int to = Mathf.Min(MassUtility.CountToPickUpUntilOverEncumbered(CS_0024_003C_003E8__locals1.packTarget, CS_0024_003C_003E8__locals1.clickedThing), CS_0024_003C_003E8__locals1.clickedThing.stackCount);
			Dialog_Slider window = new Dialog_Slider(delegate(int val)
			{
				float num = capacityLeft - (float)val * CS_0024_003C_003E8__locals1.clickedThing.GetStatValue(StatDefOf.Mass);
				return CaravanFormingUtility.AppendOverweightInfo("LoadIntoCaravanCount".Translate(CS_0024_003C_003E8__locals1.clickedThing.LabelNoCount, CS_0024_003C_003E8__locals1.clickedThing).Formatted(val), num);
			}, 1, to, delegate(int count)
			{
				CS_0024_003C_003E8__locals1.clickedThing.SetForbidden(value: false, warnOnFail: false);
				Job job = JobMaker.MakeJob(CS_0024_003C_003E8__locals1.jobDef, CS_0024_003C_003E8__locals1.clickedThing);
				job.count = count;
				job.checkEncumbrance = CS_0024_003C_003E8__locals1.packTarget == CS_0024_003C_003E8__locals1.context.FirstSelectedPawn;
				CS_0024_003C_003E8__locals1.context.FirstSelectedPawn.jobs.TryTakeOrderedJob(job, JobTag.Misc);
			});
			Find.WindowStack.Add(window);
		}

		internal string _003CGetOptionsFor_003Eb__3(int val)
		{
			float num = capacityLeft - (float)val * CS_0024_003C_003E8__locals1.clickedThing.GetStatValue(StatDefOf.Mass);
			return CaravanFormingUtility.AppendOverweightInfo("LoadIntoCaravanCount".Translate(CS_0024_003C_003E8__locals1.clickedThing.LabelNoCount, CS_0024_003C_003E8__locals1.clickedThing).Formatted(val), num);
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

		private _003C_003Ec__DisplayClass7_1 _003C_003E8__1;

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
			{
				_003C_003E1__state = -1;
				_003C_003Ec__DisplayClass7_0 _003C_003Ec__DisplayClass7_ = new _003C_003Ec__DisplayClass7_0
				{
					clickedThing = clickedThing,
					context = context
				};
				if (_003C_003Ec__DisplayClass7_.clickedThing.def.category != ThingCategory.Item)
				{
					return false;
				}
				if (!_003C_003Ec__DisplayClass7_.clickedThing.def.EverHaulable || !_003C_003Ec__DisplayClass7_.clickedThing.def.canLoadIntoCaravan)
				{
					return false;
				}
				_003C_003Ec__DisplayClass7_.packTarget = GiveToPackAnimalUtility.UsablePackAnimalWithTheMostFreeSpace(_003C_003Ec__DisplayClass7_.context.FirstSelectedPawn) ?? _003C_003Ec__DisplayClass7_.context.FirstSelectedPawn;
				_003C_003Ec__DisplayClass7_.jobDef = ((_003C_003Ec__DisplayClass7_.packTarget == _003C_003Ec__DisplayClass7_.context.FirstSelectedPawn) ? JobDefOf.TakeInventory : JobDefOf.GiveToPackAnimal);
				if (!_003C_003Ec__DisplayClass7_.context.FirstSelectedPawn.CanReach(_003C_003Ec__DisplayClass7_.clickedThing, PathEndMode.ClosestTouch, Danger.Deadly))
				{
					_003C_003E2__current = new FloatMenuOption("CannotLoadIntoCaravan".Translate(_003C_003Ec__DisplayClass7_.clickedThing.Label, _003C_003Ec__DisplayClass7_.clickedThing) + ": " + "NoPath".Translate().CapitalizeFirst(), null);
					_003C_003E1__state = 1;
					return true;
				}
				if (MassUtility.WillBeOverEncumberedAfterPickingUp(_003C_003Ec__DisplayClass7_.packTarget, _003C_003Ec__DisplayClass7_.clickedThing, 1))
				{
					_003C_003E2__current = new FloatMenuOption("CannotLoadIntoCaravan".Translate(_003C_003Ec__DisplayClass7_.clickedThing.Label, _003C_003Ec__DisplayClass7_.clickedThing) + ": " + "TooHeavy".Translate(), null);
					_003C_003E1__state = 2;
					return true;
				}
				_003C_003E8__1 = new _003C_003Ec__DisplayClass7_1();
				_003C_003E8__1.CS_0024_003C_003E8__locals1 = _003C_003Ec__DisplayClass7_;
				LordJob_FormAndSendCaravan lordJob = (LordJob_FormAndSendCaravan)_003C_003E8__1.CS_0024_003C_003E8__locals1.context.FirstSelectedPawn.GetLord().LordJob;
				_003C_003E8__1.capacityLeft = CaravanFormingUtility.CapacityLeft(lordJob);
				if (_003C_003E8__1.CS_0024_003C_003E8__locals1.clickedThing.stackCount == 1)
				{
					float capacityLeft2 = _003C_003E8__1.capacityLeft - _003C_003E8__1.CS_0024_003C_003E8__locals1.clickedThing.GetStatValue(StatDefOf.Mass);
					_003C_003E2__current = FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption(CaravanFormingUtility.AppendOverweightInfo("LoadIntoCaravan".Translate(_003C_003E8__1.CS_0024_003C_003E8__locals1.clickedThing.Label, _003C_003E8__1.CS_0024_003C_003E8__locals1.clickedThing), capacityLeft2), delegate
					{
						_003C_003E8__1.CS_0024_003C_003E8__locals1.clickedThing.SetForbidden(value: false, warnOnFail: false);
						Job job2 = JobMaker.MakeJob(_003C_003E8__1.CS_0024_003C_003E8__locals1.jobDef, _003C_003E8__1.CS_0024_003C_003E8__locals1.clickedThing);
						job2.count = 1;
						job2.checkEncumbrance = _003C_003E8__1.CS_0024_003C_003E8__locals1.packTarget == _003C_003E8__1.CS_0024_003C_003E8__locals1.context.FirstSelectedPawn;
						_003C_003E8__1.CS_0024_003C_003E8__locals1.context.FirstSelectedPawn.jobs.TryTakeOrderedJob(job2, JobTag.Misc);
					}, MenuOptionPriority.High), _003C_003E8__1.CS_0024_003C_003E8__locals1.context.FirstSelectedPawn, _003C_003E8__1.CS_0024_003C_003E8__locals1.clickedThing);
					_003C_003E1__state = 3;
					return true;
				}
				if (MassUtility.WillBeOverEncumberedAfterPickingUp(_003C_003E8__1.CS_0024_003C_003E8__locals1.packTarget, _003C_003E8__1.CS_0024_003C_003E8__locals1.clickedThing, _003C_003E8__1.CS_0024_003C_003E8__locals1.clickedThing.stackCount))
				{
					_003C_003E2__current = new FloatMenuOption("CannotLoadIntoCaravanAll".Translate(_003C_003E8__1.CS_0024_003C_003E8__locals1.clickedThing.Label, _003C_003E8__1.CS_0024_003C_003E8__locals1.clickedThing) + ": " + "TooHeavy".Translate(), null);
					_003C_003E1__state = 4;
					return true;
				}
				float capacityLeft3 = _003C_003E8__1.capacityLeft - (float)_003C_003E8__1.CS_0024_003C_003E8__locals1.clickedThing.stackCount * _003C_003E8__1.CS_0024_003C_003E8__locals1.clickedThing.GetStatValue(StatDefOf.Mass);
				_003C_003E2__current = FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption(CaravanFormingUtility.AppendOverweightInfo("LoadIntoCaravanAll".Translate(_003C_003E8__1.CS_0024_003C_003E8__locals1.clickedThing.Label, _003C_003E8__1.CS_0024_003C_003E8__locals1.clickedThing), capacityLeft3), delegate
				{
					_003C_003E8__1.CS_0024_003C_003E8__locals1.clickedThing.SetForbidden(value: false, warnOnFail: false);
					Job job3 = JobMaker.MakeJob(_003C_003E8__1.CS_0024_003C_003E8__locals1.jobDef, _003C_003E8__1.CS_0024_003C_003E8__locals1.clickedThing);
					job3.count = _003C_003E8__1.CS_0024_003C_003E8__locals1.clickedThing.stackCount;
					job3.checkEncumbrance = _003C_003E8__1.CS_0024_003C_003E8__locals1.packTarget == _003C_003E8__1.CS_0024_003C_003E8__locals1.context.FirstSelectedPawn;
					_003C_003E8__1.CS_0024_003C_003E8__locals1.context.FirstSelectedPawn.jobs.TryTakeOrderedJob(job3, JobTag.Misc);
				}, MenuOptionPriority.High), _003C_003E8__1.CS_0024_003C_003E8__locals1.context.FirstSelectedPawn, _003C_003E8__1.CS_0024_003C_003E8__locals1.clickedThing);
				_003C_003E1__state = 5;
				return true;
			}
			case 1:
				_003C_003E1__state = -1;
				break;
			case 2:
				_003C_003E1__state = -1;
				break;
			case 3:
				_003C_003E1__state = -1;
				goto IL_05a7;
			case 4:
				_003C_003E1__state = -1;
				goto IL_04fc;
			case 5:
				_003C_003E1__state = -1;
				goto IL_04fc;
			case 6:
				{
					_003C_003E1__state = -1;
					goto IL_05a7;
				}
				IL_04fc:
				_003C_003E2__current = FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption("LoadIntoCaravanSome".Translate(_003C_003E8__1.CS_0024_003C_003E8__locals1.clickedThing.LabelNoCount, _003C_003E8__1.CS_0024_003C_003E8__locals1.clickedThing), delegate
				{
					int to = Mathf.Min(MassUtility.CountToPickUpUntilOverEncumbered(_003C_003E8__1.CS_0024_003C_003E8__locals1.packTarget, _003C_003E8__1.CS_0024_003C_003E8__locals1.clickedThing), _003C_003E8__1.CS_0024_003C_003E8__locals1.clickedThing.stackCount);
					Dialog_Slider window = new Dialog_Slider(delegate(int val)
					{
						float capacityLeft = _003C_003E8__1.capacityLeft - (float)val * _003C_003E8__1.CS_0024_003C_003E8__locals1.clickedThing.GetStatValue(StatDefOf.Mass);
						return CaravanFormingUtility.AppendOverweightInfo("LoadIntoCaravanCount".Translate(_003C_003E8__1.CS_0024_003C_003E8__locals1.clickedThing.LabelNoCount, _003C_003E8__1.CS_0024_003C_003E8__locals1.clickedThing).Formatted(val), capacityLeft);
					}, 1, to, delegate(int count)
					{
						_003C_003E8__1.CS_0024_003C_003E8__locals1.clickedThing.SetForbidden(value: false, warnOnFail: false);
						Job job = JobMaker.MakeJob(_003C_003E8__1.CS_0024_003C_003E8__locals1.jobDef, _003C_003E8__1.CS_0024_003C_003E8__locals1.clickedThing);
						job.count = count;
						job.checkEncumbrance = _003C_003E8__1.CS_0024_003C_003E8__locals1.packTarget == _003C_003E8__1.CS_0024_003C_003E8__locals1.context.FirstSelectedPawn;
						_003C_003E8__1.CS_0024_003C_003E8__locals1.context.FirstSelectedPawn.jobs.TryTakeOrderedJob(job, JobTag.Misc);
					});
					Find.WindowStack.Add(window);
				}, MenuOptionPriority.High), _003C_003E8__1.CS_0024_003C_003E8__locals1.context.FirstSelectedPawn, _003C_003E8__1.CS_0024_003C_003E8__locals1.clickedThing);
				_003C_003E1__state = 6;
				return true;
				IL_05a7:
				_003C_003E8__1 = null;
				break;
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
		return context.FirstSelectedPawn.IsFormingCaravan();
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
