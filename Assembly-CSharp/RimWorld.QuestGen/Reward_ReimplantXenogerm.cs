using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Verse;
using Verse.Grammar;

namespace RimWorld.QuestGen;

[StaticConstructorOnStartup]
public class Reward_ReimplantXenogerm : Reward
{
	[CompilerGenerated]
	private sealed class _003CGenerateQuestParts_003Ed__5 : IEnumerable<QuestPart>, IEnumerable, IEnumerator<QuestPart>, IEnumerator, IDisposable
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
		public _003CGenerateQuestParts_003Ed__5(int _003C_003E1__state)
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
			switch (_003C_003E1__state)
			{
			default:
				return false;
			case 0:
			{
				_003C_003E1__state = -1;
				if (!ModLister.CheckBiotech("reimplaning reward"))
				{
					return false;
				}
				Slate slate = QuestGen.slate;
				QuestPart_ReimplantXenogerm questPart_ReimplantXenogerm = new QuestPart_ReimplantXenogerm();
				questPart_ReimplantXenogerm.mapParent = slate.Get<Map>("map").Parent;
				questPart_ReimplantXenogerm.faction = slate.Get<Faction>("faction");
				questPart_ReimplantXenogerm.pawns.AddRange(slate.Get<List<Pawn>>("sanguophages"));
				questPart_ReimplantXenogerm.gatherSpot = slate.Get<IntVec3>("gatherSpot");
				questPart_ReimplantXenogerm.inSignal = QuestGen.slate.Get<string>("inSignal");
				questPart_ReimplantXenogerm.inSignalReimplanted = slate.Get<string>("xenogermReimplantedSignal");
				questPart_ReimplantXenogerm.waitDurationTicks = 15000;
				questPart_ReimplantXenogerm.inSignalRemovePawn = QuestGenUtility.HardcodedSignalWithQuestID("sanguophages.ChangedFactionToPlayer");
				_003C_003E2__current = questPart_ReimplantXenogerm;
				_003C_003E1__state = 1;
				return true;
			}
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
		IEnumerator<QuestPart> IEnumerable<QuestPart>.GetEnumerator()
		{
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				return this;
			}
			return new _003CGenerateQuestParts_003Ed__5(0);
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<QuestPart>)this).GetEnumerator();
		}
	}

	[CompilerGenerated]
	private sealed class _003Cget_StackElements_003Ed__3 : IEnumerable<GenUI.AnonymousStackElement>, IEnumerable, IEnumerator<GenUI.AnonymousStackElement>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private GenUI.AnonymousStackElement _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public Reward_ReimplantXenogerm _003C_003E4__this;

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
		public _003Cget_StackElements_003Ed__3(int _003C_003E1__state)
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
			Reward_ReimplantXenogerm CS_0024_003C_003E8__locals1 = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				_003C_003E2__current = QuestPartUtility.GetStandardRewardStackElement("Reward_ReimplantXenogerm_Label".Translate(), Icon.Texture, () => CS_0024_003C_003E8__locals1.GetDescription(default(RewardsGeneratorParams)).CapitalizeFirst());
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
			_003Cget_StackElements_003Ed__3 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003Cget_StackElements_003Ed__3(0)
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

	private static readonly CachedTexture Icon = new CachedTexture("UI/Icons/Genes/Gene_XenogermReimplanter");

	public const int ReimplantWaitDurationTicks = 15000;

	public override IEnumerable<GenUI.AnonymousStackElement> StackElements
	{
		[IteratorStateMachine(typeof(_003Cget_StackElements_003Ed__3))]
		get
		{
			//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
			return new _003Cget_StackElements_003Ed__3(-2)
			{
				_003C_003E4__this = this
			};
		}
	}

	public override void InitFromValue(float rewardValue, RewardsGeneratorParams parms, out float valueActuallyUsed)
	{
		throw new NotImplementedException();
	}

	[IteratorStateMachine(typeof(_003CGenerateQuestParts_003Ed__5))]
	public override IEnumerable<QuestPart> GenerateQuestParts(int index, RewardsGeneratorParams parms, string customLetterLabel, string customLetterText, RulePack customLetterLabelRules, RulePack customLetterTextRules)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGenerateQuestParts_003Ed__5(-2);
	}

	public override string GetDescription(RewardsGeneratorParams parms)
	{
		return "Reward_ReimplantXenogerm_Desc".Translate().Resolve();
	}
}
