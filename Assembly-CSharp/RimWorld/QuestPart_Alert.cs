using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using RimWorld.Planet;
using Verse;

namespace RimWorld;

public class QuestPart_Alert : QuestPartActivable
{
	[CompilerGenerated]
	private sealed class _003Cget_QuestLookTargets_003Ed__9 : IEnumerable<GlobalTargetInfo>, IEnumerable, IEnumerator<GlobalTargetInfo>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private GlobalTargetInfo _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public QuestPart_Alert _003C_003E4__this;

		private IEnumerator<GlobalTargetInfo> _003C_003E7__wrap1;

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
		public _003Cget_QuestLookTargets_003Ed__9(int _003C_003E1__state)
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
				QuestPart_Alert questPart_Alert = _003C_003E4__this;
				GlobalTargetInfo globalTargetInfo;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = ((QuestPart)questPart_Alert).QuestLookTargets.GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_0071;
				case 1:
					_003C_003E1__state = -3;
					goto IL_0071;
				case 2:
					{
						_003C_003E1__state = -1;
						break;
					}
					IL_0071:
					if (_003C_003E7__wrap1.MoveNext())
					{
						GlobalTargetInfo current = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = null;
					globalTargetInfo = questPart_Alert.lookTargets.TryGetPrimaryTarget();
					if (globalTargetInfo.IsValid)
					{
						_003C_003E2__current = globalTargetInfo;
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
		IEnumerator<GlobalTargetInfo> IEnumerable<GlobalTargetInfo>.GetEnumerator()
		{
			_003Cget_QuestLookTargets_003Ed__9 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003Cget_QuestLookTargets_003Ed__9(0)
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

	public string label;

	public string explanation;

	public LookTargets lookTargets;

	public bool critical;

	public bool getLookTargetsFromSignal;

	private string resolvedLabel;

	private string resolvedExplanation;

	private LookTargets resolvedLookTargets;

	public override IEnumerable<GlobalTargetInfo> QuestLookTargets
	{
		[IteratorStateMachine(typeof(_003Cget_QuestLookTargets_003Ed__9))]
		get
		{
			//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
			return new _003Cget_QuestLookTargets_003Ed__9(-2)
			{
				_003C_003E4__this = this
			};
		}
	}

	public override string AlertLabel => resolvedLabel;

	public override string AlertExplanation => resolvedExplanation;

	public override bool AlertCritical => critical;

	public override AlertReport AlertReport
	{
		get
		{
			if (base.State != QuestPartState.Enabled)
			{
				return AlertReport.Inactive;
			}
			if (resolvedLookTargets.IsValid())
			{
				return AlertReport.CulpritsAre(resolvedLookTargets.targets);
			}
			return AlertReport.Active;
		}
	}

	protected override void Enable(SignalArgs receivedArgs)
	{
		base.Enable(receivedArgs);
		resolvedLabel = receivedArgs.GetFormattedText(label);
		resolvedExplanation = receivedArgs.GetFormattedText(explanation);
		resolvedLookTargets = lookTargets;
		if (getLookTargetsFromSignal && !resolvedLookTargets.IsValid())
		{
			SignalArgsUtility.TryGetLookTargets(receivedArgs, "SUBJECT", out resolvedLookTargets);
		}
	}

	public override void ExposeData()
	{
		base.ExposeData();
		Scribe_Values.Look(ref label, "label");
		Scribe_Values.Look(ref explanation, "explanation");
		Scribe_Deep.Look(ref lookTargets, "lookTargets");
		Scribe_Values.Look(ref critical, "critical", defaultValue: false);
		Scribe_Values.Look(ref getLookTargetsFromSignal, "getLookTargetsFromSignal", defaultValue: false);
		Scribe_Values.Look(ref resolvedLabel, "resolvedLabel");
		Scribe_Values.Look(ref resolvedExplanation, "resolvedExplanation");
		Scribe_Deep.Look(ref resolvedLookTargets, "resolvedLookTargets");
	}

	public override void AssignDebugData()
	{
		base.AssignDebugData();
		label = "DEV: Test";
		explanation = "Test text";
	}
}
