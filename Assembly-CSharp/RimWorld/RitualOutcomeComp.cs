using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld;

public abstract class RitualOutcomeComp
{
	[CompilerGenerated]
	private sealed class _003CBlockingIssues_003Ed__15 : IEnumerable<string>, IEnumerable, IEnumerator<string>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private string _003C_003E2__current;

		private int _003C_003El__initialThreadId;

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
		public _003CBlockingIssues_003Ed__15(int _003C_003E1__state)
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
			if (_003C_003E1__state != 0)
			{
				return false;
			}
			_003C_003E1__state = -1;
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
		IEnumerator<string> IEnumerable<string>.GetEnumerator()
		{
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				return this;
			}
			return new _003CBlockingIssues_003Ed__15(0);
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<string>)this).GetEnumerator();
		}
	}

	[MustTranslate]
	protected string label;

	[MustTranslate]
	protected string labelAbstract;

	protected float qualityOffset;

	public virtual bool DataRequired => true;

	public virtual float QualityOffset(LordJob_Ritual ritual, RitualOutcomeComp_Data data)
	{
		return 0f;
	}

	public virtual RitualOutcomeComp_Data MakeData()
	{
		return null;
	}

	public abstract bool Applies(LordJob_Ritual ritual);

	public virtual void Tick(LordJob_Ritual ritual, RitualOutcomeComp_Data data, float progressAmount)
	{
	}

	public virtual string GetDesc(LordJob_Ritual ritual = null, RitualOutcomeComp_Data data = null)
	{
		return label;
	}

	public virtual string GetDescAbstract(bool positive, float quality = -1f)
	{
		return label;
	}

	public virtual string GetBonusDescShort()
	{
		return label;
	}

	public virtual QualityFactor GetQualityFactor(Precept_Ritual ritual, TargetInfo ritualTarget, RitualObligation obligation, RitualRoleAssignments assignments, RitualOutcomeComp_Data data)
	{
		return null;
	}

	protected virtual string ExpectedOffsetDesc(bool positive, float quality = 0f)
	{
		return "";
	}

	public virtual void Notify_AssignmentsChanged(RitualRoleAssignments assignments, RitualOutcomeComp_Data data)
	{
	}

	[IteratorStateMachine(typeof(_003CBlockingIssues_003Ed__15))]
	public virtual IEnumerable<string> BlockingIssues(Precept_Ritual ritual, TargetInfo target, RitualRoleAssignments assignments)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CBlockingIssues_003Ed__15(-2);
	}
}
