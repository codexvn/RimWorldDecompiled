using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Verse;
using Verse.Grammar;

namespace RimWorld;

public class Reward_DefinedThingDef : Reward
{
	[CompilerGenerated]
	private sealed class _003CGenerateQuestParts_003Ed__6 : IEnumerable<QuestPart>, IEnumerable, IEnumerator<QuestPart>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private QuestPart _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		QuestPart IEnumerator<QuestPart>.Current
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
		public _003CGenerateQuestParts_003Ed__6(int _003C_003E1__state)
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
		IEnumerator<QuestPart> IEnumerable<QuestPart>.GetEnumerator()
		{
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				return this;
			}
			return new _003CGenerateQuestParts_003Ed__6(0);
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<QuestPart>)this).GetEnumerator();
		}
	}

	[CompilerGenerated]
	private sealed class _003Cget_StackElements_003Ed__4 : IEnumerable<GenUI.AnonymousStackElement>, IEnumerable, IEnumerator<GenUI.AnonymousStackElement>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private GenUI.AnonymousStackElement _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public Reward_DefinedThingDef _003C_003E4__this;

		GenUI.AnonymousStackElement IEnumerator<GenUI.AnonymousStackElement>.Current
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
		public _003Cget_StackElements_003Ed__4(int _003C_003E1__state)
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
			Reward_DefinedThingDef CS_0024_003C_003E8__locals3 = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				_003C_003E2__current = QuestPartUtility.GetStandardRewardStackElement(CS_0024_003C_003E8__locals3.thingDef.LabelCap, Widgets.GetIconFor(CS_0024_003C_003E8__locals3.thingDef, null, null, null), () => CS_0024_003C_003E8__locals3.GetDescription(default(RewardsGeneratorParams)).CapitalizeFirst());
				_003C_003E1__state = 1;
				return true;
			case 1:
				_003C_003E1__state = -1;
				return false;
			}
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
		IEnumerator<GenUI.AnonymousStackElement> IEnumerable<GenUI.AnonymousStackElement>.GetEnumerator()
		{
			_003Cget_StackElements_003Ed__4 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003Cget_StackElements_003Ed__4(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<GenUI.AnonymousStackElement>)this).GetEnumerator();
		}
	}

	public ThingDef thingDef;

	public override IEnumerable<GenUI.AnonymousStackElement> StackElements
	{
		[IteratorStateMachine(typeof(_003Cget_StackElements_003Ed__4))]
		get
		{
			//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
			return new _003Cget_StackElements_003Ed__4(-2)
			{
				_003C_003E4__this = this
			};
		}
	}

	public Reward_DefinedThingDef()
	{
	}

	public Reward_DefinedThingDef(ThingDef thingDef)
	{
		this.thingDef = thingDef;
	}

	public override void InitFromValue(float rewardValue, RewardsGeneratorParams parms, out float valueActuallyUsed)
	{
		valueActuallyUsed = rewardValue;
	}

	[IteratorStateMachine(typeof(_003CGenerateQuestParts_003Ed__6))]
	public override IEnumerable<QuestPart> GenerateQuestParts(int index, RewardsGeneratorParams parms, string customLetterLabel, string customLetterText, RulePack customLetterLabelRules, RulePack customLetterTextRules)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGenerateQuestParts_003Ed__6(-2);
	}

	public override string GetDescription(RewardsGeneratorParams parms)
	{
		return thingDef.description;
	}

	public override void ExposeData()
	{
		base.ExposeData();
		Scribe_Defs.Look(ref thingDef, "thingDef");
	}
}
