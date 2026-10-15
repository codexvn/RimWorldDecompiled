using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using RimWorld.Planet;
using Verse;

namespace RimWorld;

public class QuestPart_RequirementsToAcceptThingStudied_ArchotechStructures : QuestPart_RequirementsToAcceptThingStudied
{
	[CompilerGenerated]
	private sealed class _003Cget_QuestLookTargets_003Ed__1 : IEnumerable<GlobalTargetInfo>, IEnumerable, IEnumerator<GlobalTargetInfo>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private GlobalTargetInfo _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public QuestPart_RequirementsToAcceptThingStudied_ArchotechStructures _003C_003E4__this;

		private IEnumerator<GlobalTargetInfo> _003C_003E7__wrap1;

		private List<WorldObject>.Enumerator _003C_003E7__wrap2;

		GlobalTargetInfo IEnumerator<GlobalTargetInfo>.Current
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
		public _003Cget_QuestLookTargets_003Ed__1(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			switch (_003C_003E1__state)
			{
			case -3:
			case 1:
				try
				{
				}
				finally
				{
					_003C_003Em__Finally1();
				}
				break;
			case -4:
			case 2:
				try
				{
				}
				finally
				{
					_003C_003Em__Finally2();
				}
				break;
			}
			_003C_003E7__wrap1 = null;
			_003C_003E7__wrap2 = default(List<WorldObject>.Enumerator);
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			bool result;
			try
			{
				int num = _003C_003E1__state;
				QuestPart_RequirementsToAcceptThingStudied_ArchotechStructures questPart_RequirementsToAcceptThingStudied_ArchotechStructures = _003C_003E4__this;
				switch (num)
				{
				default:
					result = false;
					break;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = ((QuestPart_RequirementsToAcceptThingStudied)questPart_RequirementsToAcceptThingStudied_ArchotechStructures).QuestLookTargets.GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_0072;
				case 1:
					_003C_003E1__state = -3;
					goto IL_0072;
				case 2:
					{
						_003C_003E1__state = -4;
						result = false;
						_003C_003Em__Finally2();
						break;
					}
					IL_0072:
					if (_003C_003E7__wrap1.MoveNext())
					{
						GlobalTargetInfo current = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						result = true;
						break;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = null;
					if (questPart_RequirementsToAcceptThingStudied_ArchotechStructures.thing != null && !questPart_RequirementsToAcceptThingStudied_ArchotechStructures.thing.Spawned)
					{
						if (questPart_RequirementsToAcceptThingStudied_ArchotechStructures.thing.TryGetComp<CompStudiable>().Completed)
						{
							result = false;
							break;
						}
						_003C_003E7__wrap2 = Find.WorldObjects.AllWorldObjects.GetEnumerator();
						_003C_003E1__state = -4;
						while (true)
						{
							if (_003C_003E7__wrap2.MoveNext())
							{
								WorldObject current2 = _003C_003E7__wrap2.Current;
								if (current2 is AbandonedArchotechStructures abandonedArchotechStructures && !abandonedArchotechStructures.archotechStructures.NullOrEmpty() && abandonedArchotechStructures.archotechStructures.Contains(questPart_RequirementsToAcceptThingStudied_ArchotechStructures.thing))
								{
									_003C_003E2__current = current2;
									_003C_003E1__state = 2;
									result = true;
									goto end_IL_000f;
								}
								continue;
							}
							_003C_003Em__Finally2();
							_003C_003E7__wrap2 = default(List<WorldObject>.Enumerator);
							break;
						}
					}
					result = false;
					break;
					end_IL_000f:
					break;
				}
			}
			catch
			{
				//try-fault
				((IDisposable)this).Dispose();
				throw;
			}
			return result;
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

		private void _003C_003Em__Finally2()
		{
			_003C_003E1__state = -1;
			((IDisposable)_003C_003E7__wrap2/*cast due to .constrained prefix*/).Dispose();
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<GlobalTargetInfo> IEnumerable<GlobalTargetInfo>.GetEnumerator()
		{
			_003Cget_QuestLookTargets_003Ed__1 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003Cget_QuestLookTargets_003Ed__1(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<GlobalTargetInfo>)this).GetEnumerator();
		}
	}

	public override IEnumerable<GlobalTargetInfo> QuestLookTargets
	{
		[IteratorStateMachine(typeof(_003Cget_QuestLookTargets_003Ed__1))]
		get
		{
			//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
			return new _003Cget_QuestLookTargets_003Ed__1(-2)
			{
				_003C_003E4__this = this
			};
		}
	}
}
