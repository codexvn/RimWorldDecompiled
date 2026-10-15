using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Verse;
using Verse.AI;

namespace RimWorld;

public class Building_StylingStation : Building
{
	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass0_0
	{
		public Pawn selPawn;

		public Building_StylingStation _003C_003E4__this;

		internal void _003CGetFloatMenuOptions_003Eb__0()
		{
			selPawn.jobs.TryTakeOrderedJob(JobMaker.MakeJob(JobDefOf.OpenStylingStationDialog, _003C_003E4__this), JobTag.Misc);
		}

		internal void _003CGetFloatMenuOptions_003Eb__1()
		{
			JobGiver_OptimizeApparel.TryCreateRecolorJob(selPawn, out var job);
			selPawn.jobs.TryTakeOrderedJob(job, JobTag.Misc);
		}
	}

	[CompilerGenerated]
	private sealed class _003CGetFloatMenuOptions_003Ed__0 : IEnumerable<FloatMenuOption>, IEnumerable, IEnumerator<FloatMenuOption>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private FloatMenuOption _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private Pawn selPawn;

		public Pawn _003C_003E3__selPawn;

		public Building_StylingStation _003C_003E4__this;

		private _003C_003Ec__DisplayClass0_0 _003C_003E8__1;

		private IEnumerator<FloatMenuOption> _003C_003E7__wrap1;

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
		public _003CGetFloatMenuOptions_003Ed__0(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			int num = _003C_003E1__state;
			if (num == -3 || num == 1)
			{
				try
				{
				}
				finally
				{
					_003C_003Em__Finally1();
				}
			}
			_003C_003E8__1 = null;
			_003C_003E7__wrap1 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				Building_StylingStation building_StylingStation = _003C_003E4__this;
				Job job;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E8__1 = new _003C_003Ec__DisplayClass0_0();
					_003C_003E8__1.selPawn = selPawn;
					_003C_003E8__1._003C_003E4__this = _003C_003E4__this;
					_003C_003E7__wrap1 = ((ThingWithComps)building_StylingStation).GetFloatMenuOptions(_003C_003E8__1.selPawn).GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_00b2;
				case 1:
					_003C_003E1__state = -3;
					goto IL_00b2;
				case 2:
					_003C_003E1__state = -1;
					goto IL_01ba;
				case 3:
					_003C_003E1__state = -1;
					goto IL_01ba;
				case 4:
					{
						_003C_003E1__state = -1;
						break;
					}
					IL_01ba:
					if (JobGiver_OptimizeApparel.TryCreateRecolorJob(_003C_003E8__1.selPawn, out job, dryRun: true))
					{
						_003C_003E2__current = FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption("RecolorApparel".Translate().CapitalizeFirst(), delegate
						{
							JobGiver_OptimizeApparel.TryCreateRecolorJob(_003C_003E8__1.selPawn, out var job2);
							_003C_003E8__1.selPawn.jobs.TryTakeOrderedJob(job2, JobTag.Misc);
						}), _003C_003E8__1.selPawn, building_StylingStation);
						_003C_003E1__state = 4;
						return true;
					}
					break;
					IL_00b2:
					if (_003C_003E7__wrap1.MoveNext())
					{
						FloatMenuOption current = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = null;
					if (ModLister.IdeologyInstalled)
					{
						if (!_003C_003E8__1.selPawn.CanReach(building_StylingStation, PathEndMode.OnCell, Danger.Deadly))
						{
							_003C_003E2__current = new FloatMenuOption("CannotUseReason".Translate("NoPath".Translate().CapitalizeFirst()), null);
							_003C_003E1__state = 2;
							return true;
						}
						_003C_003E2__current = FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption("ChangeStyle".Translate().CapitalizeFirst(), delegate
						{
							_003C_003E8__1.selPawn.jobs.TryTakeOrderedJob(JobMaker.MakeJob(JobDefOf.OpenStylingStationDialog, _003C_003E8__1._003C_003E4__this), JobTag.Misc);
						}), _003C_003E8__1.selPawn, building_StylingStation);
						_003C_003E1__state = 3;
						return true;
					}
					break;
				}
				return false;
			}
			catch
			{
				//try-fault
				((IDisposable)this).Dispose();
				throw;
			}
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		private void _003C_003Em__Finally1()
		{
			_003C_003E1__state = -1;
			if (_003C_003E7__wrap1 != null)
			{
				_003C_003E7__wrap1.Dispose();
			}
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<FloatMenuOption> IEnumerable<FloatMenuOption>.GetEnumerator()
		{
			_003CGetFloatMenuOptions_003Ed__0 _003CGetFloatMenuOptions_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CGetFloatMenuOptions_003Ed__ = this;
			}
			else
			{
				_003CGetFloatMenuOptions_003Ed__ = new _003CGetFloatMenuOptions_003Ed__0(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			_003CGetFloatMenuOptions_003Ed__.selPawn = _003C_003E3__selPawn;
			return _003CGetFloatMenuOptions_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<FloatMenuOption>)this).GetEnumerator();
		}
	}

	[IteratorStateMachine(typeof(_003CGetFloatMenuOptions_003Ed__0))]
	public override IEnumerable<FloatMenuOption> GetFloatMenuOptions(Pawn selPawn)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetFloatMenuOptions_003Ed__0(-2)
		{
			_003C_003E4__this = this,
			_003C_003E3__selPawn = selPawn
		};
	}
}
