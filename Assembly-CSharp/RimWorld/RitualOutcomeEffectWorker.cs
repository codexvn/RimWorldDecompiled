using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld;

public abstract class RitualOutcomeEffectWorker : IExposable
{
	[CompilerGenerated]
	private sealed class _003CBlockingIssues_003Ed__22 : IEnumerable<string>, IEnumerable, IEnumerator<string>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private string _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public RitualOutcomeEffectWorker _003C_003E4__this;

		private Precept_Ritual ritual;

		public Precept_Ritual _003C_003E3__ritual;

		private TargetInfo target;

		public TargetInfo _003C_003E3__target;

		private RitualRoleAssignments assignments;

		public RitualRoleAssignments _003C_003E3__assignments;

		private List<RitualOutcomeComp>.Enumerator _003C_003E7__wrap1;

		private IEnumerator<string> _003C_003E7__wrap2;

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
		public _003CBlockingIssues_003Ed__22(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			int num = _003C_003E1__state;
			if ((uint)(num - -4) <= 1u || num == 1)
			{
				try
				{
					if (num == -4 || num == 1)
					{
						try
						{
						}
						finally
						{
							_003C_003Em__Finally2();
						}
					}
				}
				finally
				{
					_003C_003Em__Finally1();
				}
			}
			_003C_003E7__wrap1 = default(List<RitualOutcomeComp>.Enumerator);
			_003C_003E7__wrap2 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				RitualOutcomeEffectWorker ritualOutcomeEffectWorker = _003C_003E4__this;
				if (num != 0)
				{
					if (num != 1)
					{
						return false;
					}
					_003C_003E1__state = -4;
					goto IL_00bc;
				}
				_003C_003E1__state = -1;
				if (!ritualOutcomeEffectWorker.def.comps.NullOrEmpty())
				{
					_003C_003E7__wrap1 = ritualOutcomeEffectWorker.def.comps.GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_00d6;
				}
				goto IL_00f8;
				IL_00bc:
				if (_003C_003E7__wrap2.MoveNext())
				{
					string current = _003C_003E7__wrap2.Current;
					_003C_003E2__current = current;
					_003C_003E1__state = 1;
					return true;
				}
				_003C_003Em__Finally2();
				_003C_003E7__wrap2 = null;
				goto IL_00d6;
				IL_00d6:
				if (_003C_003E7__wrap1.MoveNext())
				{
					RitualOutcomeComp current2 = _003C_003E7__wrap1.Current;
					_003C_003E7__wrap2 = current2.BlockingIssues(ritual, target, assignments).GetEnumerator();
					_003C_003E1__state = -4;
					goto IL_00bc;
				}
				_003C_003Em__Finally1();
				_003C_003E7__wrap1 = default(List<RitualOutcomeComp>.Enumerator);
				goto IL_00f8;
				IL_00f8:
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

		private void _003C_003Em__Finally2()
		{
			_003C_003E1__state = -3;
			if (_003C_003E7__wrap2 != null)
			{
				_003C_003E7__wrap2.Dispose();
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
			_003CBlockingIssues_003Ed__22 _003CBlockingIssues_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CBlockingIssues_003Ed__ = this;
			}
			else
			{
				_003CBlockingIssues_003Ed__ = new _003CBlockingIssues_003Ed__22(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			_003CBlockingIssues_003Ed__.ritual = _003C_003E3__ritual;
			_003CBlockingIssues_003Ed__.target = _003C_003E3__target;
			_003CBlockingIssues_003Ed__.assignments = _003C_003E3__assignments;
			return _003CBlockingIssues_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<string>)this).GetEnumerator();
		}
	}

	public RitualOutcomeEffectDef def;

	public List<RitualOutcomeComp_Data> compDatas;

	public virtual bool ApplyOnFailure => false;

	public virtual bool SupportsAttachableOutcomeEffect => false;

	public virtual bool ShowQuality => true;

	public virtual string Description => def.Description;

	public RitualOutcomeEffectWorker()
	{
	}

	public RitualOutcomeEffectWorker(RitualOutcomeEffectDef def)
	{
		this.def = def;
		FillCompData();
	}

	public void ResetCompDatas()
	{
		foreach (RitualOutcomeComp_Data compData in compDatas)
		{
			compData?.Reset();
		}
	}

	public void FillCompData()
	{
		if ((compDatas != null && compDatas.Count == def.comps.Count) || def.comps.NullOrEmpty())
		{
			return;
		}
		compDatas = new List<RitualOutcomeComp_Data>();
		foreach (RitualOutcomeComp comp in def.comps)
		{
			compDatas.Add(comp.MakeData());
		}
	}

	public RitualOutcomeComp_Data DataForComp(RitualOutcomeComp comp)
	{
		int num = def.comps.IndexOf(comp);
		if (num != -1)
		{
			return compDatas[num];
		}
		Log.Error("Can't find index for " + comp.GetType().Name);
		return null;
	}

	public abstract void Apply(float progress, Dictionary<Pawn, int> totalPresence, LordJob_Ritual jobRitual);

	public virtual void ExposeData()
	{
		Scribe_Defs.Look(ref def, "def");
		Scribe_Collections.Look(ref compDatas, "compDatas", LookMode.Deep);
		if (Scribe.mode == LoadSaveMode.PostLoadInit && (compDatas.NullOrEmpty() || compDatas.Count != def.comps.Count))
		{
			FillCompData();
		}
	}

	public Thought_Memory MakeMemory(Pawn p, LordJob_Ritual ritual, ThoughtDef overrideDef = null)
	{
		Thought_Memory obj = (Thought_Memory)ThoughtMaker.MakeThought(overrideDef ?? def.memoryDef);
		obj.sourcePrecept = ritual.Ritual;
		return obj;
	}

	public virtual void Tick(LordJob_Ritual ritual, float progressAmount = 1f)
	{
		if (def.comps.NullOrEmpty())
		{
			return;
		}
		foreach (RitualOutcomeComp comp in def.comps)
		{
			comp.Tick(ritual, DataForComp(comp), progressAmount);
		}
	}

	public virtual string ExtraAlertParagraph(Precept_Ritual ritual)
	{
		return null;
	}

	public virtual string ExpectedQualityLabel()
	{
		return null;
	}

	public virtual RitualOutcomePossibility GetForcedOutcome(Precept_Ritual ritual, TargetInfo ritualTarget, RitualObligation obligation, RitualRoleAssignments assignments)
	{
		return null;
	}

	[IteratorStateMachine(typeof(_003CBlockingIssues_003Ed__22))]
	public virtual IEnumerable<string> BlockingIssues(Precept_Ritual ritual, TargetInfo target, RitualRoleAssignments assignments)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CBlockingIssues_003Ed__22(-2)
		{
			_003C_003E4__this = this,
			_003C_003E3__ritual = ritual,
			_003C_003E3__target = target,
			_003C_003E3__assignments = assignments
		};
	}
}
