using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld;

public class CompProperties_CanBeDormant : CompProperties
{
	[CompilerGenerated]
	private sealed class _003CConfigErrors_003Ed__16 : IEnumerable<string>, IEnumerable, IEnumerator<string>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private string _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private ThingDef parentDef;

		public ThingDef _003C_003E3__parentDef;

		public CompProperties_CanBeDormant _003C_003E4__this;

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
		public _003CConfigErrors_003Ed__16(int _003C_003E1__state)
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
			int num = _003C_003E1__state;
			CompProperties_CanBeDormant compProperties_CanBeDormant = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				if (!parentDef.receivesSignals && !compProperties_CanBeDormant.jobDormancy)
				{
					_003C_003E2__current = "ThingDefs with CanBeDormant component must have receivesSignals set to true, otherwise wakeup won't work properly!";
					_003C_003E1__state = 1;
					return true;
				}
				break;
			case 1:
				_003C_003E1__state = -1;
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
		IEnumerator<string> IEnumerable<string>.GetEnumerator()
		{
			_003CConfigErrors_003Ed__16 _003CConfigErrors_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CConfigErrors_003Ed__ = this;
			}
			else
			{
				_003CConfigErrors_003Ed__ = new _003CConfigErrors_003Ed__16(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			_003CConfigErrors_003Ed__.parentDef = _003C_003E3__parentDef;
			return _003CConfigErrors_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<string>)this).GetEnumerator();
		}
	}

	public bool startsDormant;

	[NoTranslate]
	public string wakeUpSignalTag = "CompCanBeDormant.WakeUp";

	public float maxDistAwakenByOther = 40f;

	public bool canWakeUpFogged = true;

	public bool jobDormancy;

	public IntRange wakeUpDelayRange = new IntRange(60, 300);

	[NoTranslate]
	public string awakeStateLabelKey = "AwokeDaysAgo";

	[NoTranslate]
	public string dormantStateLabelKey = "DormantCompInactive";

	[NoTranslate]
	public string wakeUpDelayStateLabelKey;

	public EffecterDef wakeUpEffect;

	public IntRange? wakeUpRepeatSignalDelayRange;

	public bool showSleepingZs = true;

	public bool delayedWakeUpDoesZs = true;

	public bool dontShowDevGizmos;

	public List<NeedDef> freezeNeeds = new List<NeedDef>();

	public CompProperties_CanBeDormant()
	{
		compClass = typeof(CompCanBeDormant);
	}

	[IteratorStateMachine(typeof(_003CConfigErrors_003Ed__16))]
	public override IEnumerable<string> ConfigErrors(ThingDef parentDef)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CConfigErrors_003Ed__16(-2)
		{
			_003C_003E4__this = this,
			_003C_003E3__parentDef = parentDef
		};
	}
}
