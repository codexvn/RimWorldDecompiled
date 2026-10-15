using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Verse;
using Verse.AI.Group;

namespace RimWorld;

public class PsychicRitualDef_SummonAnimals : PsychicRitualDef_InvocationCircle
{
	[CompilerGenerated]
	private sealed class _003CBlockingIssues_003Ed__4 : IEnumerable<string>, IEnumerable, IEnumerator<string>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private string _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public PsychicRitualDef_SummonAnimals _003C_003E4__this;

		private PsychicRitualRoleAssignments assignments;

		public PsychicRitualRoleAssignments _003C_003E3__assignments;

		private Map map;

		public Map _003C_003E3__map;

		private IEnumerator<string> _003C_003E7__wrap1;

		string IEnumerator<string>.Current
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
		public _003CBlockingIssues_003Ed__4(int _003C_003E1__state)
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
				int num = _003C_003E1__state;
				PsychicRitualDef_SummonAnimals psychicRitualDef_SummonAnimals = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = ((PsychicRitualDef_InvocationCircle)psychicRitualDef_SummonAnimals).BlockingIssues(assignments, map).GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_007b;
				case 1:
					_003C_003E1__state = -3;
					goto IL_007b;
				case 2:
					{
						_003C_003E1__state = -1;
						break;
					}
					IL_007b:
					if (_003C_003E7__wrap1.MoveNext())
					{
						string current = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = null;
					if (!psychicRitualDef_SummonAnimals.animalsAllowed)
					{
						_003C_003E2__current = "NotEnoughAnimals".Translate();
						_003C_003E1__state = 2;
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
		IEnumerator<string> IEnumerable<string>.GetEnumerator()
		{
			_003CBlockingIssues_003Ed__4 _003CBlockingIssues_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CBlockingIssues_003Ed__ = this;
			}
			else
			{
				_003CBlockingIssues_003Ed__ = new _003CBlockingIssues_003Ed__4(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			_003CBlockingIssues_003Ed__.assignments = _003C_003E3__assignments;
			_003CBlockingIssues_003Ed__.map = _003C_003E3__map;
			return _003CBlockingIssues_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<string>)this).GetEnumerator();
		}
	}

	public SimpleCurve manhunterSpawnChanceFromQualityCurve;

	private bool animalsAllowed;

	public override List<PsychicRitualToil> CreateToils(PsychicRitual psychicRitual, PsychicRitualGraph graph)
	{
		List<PsychicRitualToil> list = base.CreateToils(psychicRitual, graph);
		list.Add(new PsychicRitualToil_SummonAnimals(InvokerRole));
		return list;
	}

	public override TaggedString OutcomeDescription(FloatRange qualityRange, string qualityNumber, PsychicRitualRoleAssignments assignments)
	{
		return outcomeDescription.Formatted(manhunterSpawnChanceFromQualityCurve.Evaluate(qualityRange.min).ToStringPercent());
	}

	[IteratorStateMachine(typeof(_003CBlockingIssues_003Ed__4))]
	public override IEnumerable<string> BlockingIssues(PsychicRitualRoleAssignments assignments, Map map)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CBlockingIssues_003Ed__4(-2)
		{
			_003C_003E4__this = this,
			_003C_003E3__assignments = assignments,
			_003C_003E3__map = map
		};
	}

	public override void InitializeCast(Map map)
	{
		base.InitializeCast(map);
		animalsAllowed = map.Biome.AllWildAnimals.EnumerableCount() > 0 && map.Biome.wildAnimalsCanWanderInto && RCellFinder.TryFindRandomPawnEntryCell(out var _, map, CellFinder.EdgeRoadChance_Animal);
	}
}
