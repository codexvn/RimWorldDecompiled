using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Verse;
using Verse.AI;

namespace RimWorld;

public class WorkGiverDef : Def
{
	[CompilerGenerated]
	private sealed class _003CConfigErrors_003Ed__34 : IEnumerable<string>, IEnumerable, IEnumerator<string>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private string _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public WorkGiverDef _003C_003E4__this;

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
		public _003CConfigErrors_003Ed__34(int _003C_003E1__state)
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
				WorkGiverDef workGiverDef = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = ((Def)workGiverDef).ConfigErrors().GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_0076;
				case 1:
					_003C_003E1__state = -3;
					goto IL_0076;
				case 2:
					_003C_003E1__state = -1;
					goto IL_00c5;
				case 3:
					{
						_003C_003E1__state = -1;
						break;
					}
					IL_0076:
					if (_003C_003E7__wrap1.MoveNext())
					{
						string current = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = null;
					if (workGiverDef.verb.NullOrEmpty())
					{
						_003C_003E2__current = workGiverDef.defName + " lacks a verb.";
						_003C_003E1__state = 2;
						return true;
					}
					goto IL_00c5;
					IL_00c5:
					if (workGiverDef.gerund.NullOrEmpty())
					{
						_003C_003E2__current = workGiverDef.defName + " lacks a gerund.";
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
		IEnumerator<string> IEnumerable<string>.GetEnumerator()
		{
			_003CConfigErrors_003Ed__34 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CConfigErrors_003Ed__34(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<string>)this).GetEnumerator();
		}
	}

	public Type giverClass;

	public WorkTypeDef workType;

	public WorkTags workTags;

	public int priorityInType;

	[MustTranslate]
	public string verb;

	[MustTranslate]
	public string gerund;

	public bool scanThings = true;

	public bool scanCells;

	public bool emergency;

	public List<PawnCapacityDef> requiredCapacities = new List<PawnCapacityDef>();

	public bool directOrderable = true;

	public bool prioritizeSustains;

	public bool nonColonistsCanDo;

	public JobTag tagToGive = JobTag.MiscWork;

	public WorkGiverEquivalenceGroupDef equivalenceGroup;

	public bool canBeDoneWhileDrafted;

	public int autoTakeablePriorityDrafted = -1;

	public ThingDef forceMote;

	public FleckDef forceFleck;

	public bool canBeDoneByMechs = true;

	public List<ThingDef> fixedBillGiverDefs;

	public bool billGiversAllHumanlikes;

	public bool billGiversAllHumanlikesCorpses;

	public bool billGiversAllMechanoids;

	public bool billGiversAllMechanoidsCorpses;

	public bool billGiversAllAnimals;

	public bool billGiversAllAnimalsCorpses;

	public bool feedHumanlikesOnly;

	public bool feedAnimalsOnly;

	public bool doesSmoothing;

	public ThingDef scannerDef;

	[Unsaved(false)]
	private WorkGiver workerInt;

	public WorkGiver Worker
	{
		get
		{
			if (workerInt == null)
			{
				workerInt = (WorkGiver)Activator.CreateInstance(giverClass);
				workerInt.def = this;
			}
			return workerInt;
		}
	}

	[IteratorStateMachine(typeof(_003CConfigErrors_003Ed__34))]
	public override IEnumerable<string> ConfigErrors()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CConfigErrors_003Ed__34(-2)
		{
			_003C_003E4__this = this
		};
	}
}
