using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Verse;
using Verse.AI;

namespace RimWorld;

public static class HaulSourceUtility
{
	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass0_0
	{
		public IHaulSource source;

		public Pawn pawn;

		public Action _003C_003E9__0;

		internal void _003CGetFloatMenuOptions_003Eb__0()
		{
			bool flag = false;
			foreach (Thing item in (IEnumerable<Thing>)source.GetDirectlyHeldThings())
			{
				Job job = HaulAIUtility.HaulToStorageJob(pawn, item, forced: true);
				if (job != null)
				{
					job.playerForced = true;
					if (!flag)
					{
						flag = true;
						pawn.jobs.TryTakeOrderedJob(job, JobTag.Misc);
					}
					else
					{
						pawn.jobs.TryTakeOrderedJob(job, JobTag.Misc, requestQueueing: true);
					}
				}
			}
		}
	}

	[CompilerGenerated]
	private sealed class _003CGetFloatMenuOptions_003Ed__0 : IEnumerable<FloatMenuOption>, IEnumerable, IEnumerator<FloatMenuOption>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private FloatMenuOption _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private IHaulSource source;

		public IHaulSource _003C_003E3__source;

		private Pawn pawn;

		public Pawn _003C_003E3__pawn;

		private IEnumerator<Thing> _003C_003E7__wrap1;

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
			_003C_003E7__wrap1 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				switch (_003C_003E1__state)
				{
				default:
					return false;
				case 0:
				{
					_003C_003E1__state = -1;
					_003C_003Ec__DisplayClass0_0 CS_0024_003C_003E8__locals8 = new _003C_003Ec__DisplayClass0_0
					{
						source = source,
						pawn = pawn
					};
					_003C_003E7__wrap1 = ((IEnumerable<Thing>)CS_0024_003C_003E8__locals8.source.GetDirectlyHeldThings()).GetEnumerator();
					_003C_003E1__state = -3;
					while (_003C_003E7__wrap1.MoveNext())
					{
						Thing current = _003C_003E7__wrap1.Current;
						StoragePriority currentPriority = StoreUtility.CurrentStoragePriorityOf(current, forced: true);
						if (!current.MapHeld.listerHaulables.ThingsPotentiallyNeedingHauling().Contains(current) || !StoreUtility.TryFindBestBetterStorageFor(current, CS_0024_003C_003E8__locals8.pawn, CS_0024_003C_003E8__locals8.pawn.MapHeld, currentPriority, CS_0024_003C_003E8__locals8.pawn.Faction, out var _, out var _))
						{
							continue;
						}
						_003C_003E2__current = new FloatMenuOption("HaulFromSource".Translate(), delegate
						{
							bool flag = false;
							foreach (Thing item in (IEnumerable<Thing>)CS_0024_003C_003E8__locals8.source.GetDirectlyHeldThings())
							{
								Job job = HaulAIUtility.HaulToStorageJob(CS_0024_003C_003E8__locals8.pawn, item, forced: true);
								if (job != null)
								{
									job.playerForced = true;
									if (!flag)
									{
										flag = true;
										CS_0024_003C_003E8__locals8.pawn.jobs.TryTakeOrderedJob(job, JobTag.Misc);
									}
									else
									{
										CS_0024_003C_003E8__locals8.pawn.jobs.TryTakeOrderedJob(job, JobTag.Misc, requestQueueing: true);
									}
								}
							}
						});
						_003C_003E1__state = 1;
						return true;
					}
					break;
				}
				case 1:
					_003C_003E1__state = -3;
					break;
				}
				_003C_003Em__Finally1();
				_003C_003E7__wrap1 = null;
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
				_003CGetFloatMenuOptions_003Ed__ = new _003CGetFloatMenuOptions_003Ed__0(0);
			}
			_003CGetFloatMenuOptions_003Ed__.source = _003C_003E3__source;
			_003CGetFloatMenuOptions_003Ed__.pawn = _003C_003E3__pawn;
			return _003CGetFloatMenuOptions_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<FloatMenuOption>)this).GetEnumerator();
		}
	}

	[IteratorStateMachine(typeof(_003CGetFloatMenuOptions_003Ed__0))]
	public static IEnumerable<FloatMenuOption> GetFloatMenuOptions(IHaulSource source, Pawn pawn)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetFloatMenuOptions_003Ed__0(-2)
		{
			_003C_003E3__source = source,
			_003C_003E3__pawn = pawn
		};
	}
}
