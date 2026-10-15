using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Verse;
using Verse.AI;

namespace RimWorld;

public abstract class RitualObligationTargetFilter : IExposable
{
	[CompilerGenerated]
	private sealed class _003CGetBlockingIssues_003Ed__10 : IEnumerable<string>, IEnumerable, IEnumerator<string>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private string _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private RitualRoleAssignments assignments;

		public RitualRoleAssignments _003C_003E3__assignments;

		private TargetInfo target;

		public TargetInfo _003C_003E3__target;

		private List<RitualRole>.Enumerator _003C_003E7__wrap1;

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
		public _003CGetBlockingIssues_003Ed__10(int _003C_003E1__state)
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
			_003C_003E7__wrap1 = default(List<RitualRole>.Enumerator);
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
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = assignments.AllRolesForReading.GetEnumerator();
					_003C_003E1__state = -3;
					break;
				case 1:
					_003C_003E1__state = -3;
					break;
				}
				while (_003C_003E7__wrap1.MoveNext())
				{
					RitualRole current = _003C_003E7__wrap1.Current;
					if (current.mustBeAbleToReachTarget)
					{
						Pawn pawn = assignments.FirstAssignedPawn(current);
						if (pawn != null && !pawn.CanReach((LocalTargetInfo)target, PathEndMode.Touch, pawn.NormalMaxDanger()))
						{
							_003C_003E2__current = "RitualTargetUnreachable".Translate(current.LabelCap);
							_003C_003E1__state = 1;
							return true;
						}
					}
				}
				_003C_003Em__Finally1();
				_003C_003E7__wrap1 = default(List<RitualRole>.Enumerator);
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
			((IDisposable)_003C_003E7__wrap1/*cast due to .constrained prefix*/).Dispose();
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<string> IEnumerable<string>.GetEnumerator()
		{
			_003CGetBlockingIssues_003Ed__10 _003CGetBlockingIssues_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CGetBlockingIssues_003Ed__ = this;
			}
			else
			{
				_003CGetBlockingIssues_003Ed__ = new _003CGetBlockingIssues_003Ed__10(0);
			}
			_003CGetBlockingIssues_003Ed__.target = _003C_003E3__target;
			_003CGetBlockingIssues_003Ed__.assignments = _003C_003E3__assignments;
			return _003CGetBlockingIssues_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<string>)this).GetEnumerator();
		}
	}

	public Precept_Ritual parent;

	public RitualObligationTargetFilterDef def;

	public RitualObligationTargetFilter()
	{
	}

	public RitualObligationTargetFilter(RitualObligationTargetFilterDef def)
	{
		this.def = def;
	}

	public RitualTargetUseReport CanUseTarget(TargetInfo target, RitualObligation obligation)
	{
		if ((target.Thing != null && !target.Thing.Spawned) || Find.IdeoManager.GetActiveRitualOn(target.Thing) != null)
		{
			return false;
		}
		return CanUseTargetInternal(target, obligation);
	}

	public abstract IEnumerable<TargetInfo> GetTargets(RitualObligation obligation, Map map);

	protected abstract RitualTargetUseReport CanUseTargetInternal(TargetInfo target, RitualObligation obligation);

	public virtual List<string> MissingTargetBuilding(Ideo ideo)
	{
		return null;
	}

	public virtual bool ObligationTargetsValid(RitualObligation obligation)
	{
		return true;
	}

	public abstract IEnumerable<string> GetTargetInfos(RitualObligation obligation);

	[IteratorStateMachine(typeof(_003CGetBlockingIssues_003Ed__10))]
	public virtual IEnumerable<string> GetBlockingIssues(TargetInfo target, RitualRoleAssignments assignments)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetBlockingIssues_003Ed__10(-2)
		{
			_003C_003E3__target = target,
			_003C_003E3__assignments = assignments
		};
	}

	public virtual string LabelExtraPart(RitualObligation obligation)
	{
		return "";
	}

	public virtual bool ShouldGrayOut(Pawn pawn, ILordJobAssignmentsManager<RitualRole> assignments, out TaggedString reason)
	{
		reason = TaggedString.Empty;
		return false;
	}

	public virtual void CopyTo(RitualObligationTargetFilter other)
	{
		other.parent = parent;
		other.def = def;
	}

	public virtual void ExposeData()
	{
		Scribe_Defs.Look(ref def, "def");
		Scribe_References.Look(ref parent, "parent");
	}
}
