using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using RimWorld.QuestGen;
using UnityEngine;
using Verse;
using Verse.Grammar;

namespace RimWorld;

public class Reward_RoyalFavor : Reward
{
	[CompilerGenerated]
	private sealed class _003CGenerateQuestParts_003Ed__7 : IEnumerable<QuestPart>, IEnumerable, IEnumerator<QuestPart>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private QuestPart _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public Reward_RoyalFavor _003C_003E4__this;

		private RewardsGeneratorParams parms;

		public RewardsGeneratorParams _003C_003E3__parms;

		private Slate _003Cslate_003E5__2;

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
		public _003CGenerateQuestParts_003Ed__7(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			_003Cslate_003E5__2 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			int num = _003C_003E1__state;
			Reward_RoyalFavor reward_RoyalFavor = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
			{
				_003C_003E1__state = -1;
				_003Cslate_003E5__2 = RimWorld.QuestGen.QuestGen.slate;
				QuestPart_GiveRoyalFavor questPart_GiveRoyalFavor = new QuestPart_GiveRoyalFavor
				{
					faction = reward_RoyalFavor.faction,
					amount = reward_RoyalFavor.amount
				};
				if (!parms.chosenPawnSignal.NullOrEmpty())
				{
					questPart_GiveRoyalFavor.inSignal = QuestGenUtility.HardcodedSignalWithQuestID(parms.chosenPawnSignal);
					questPart_GiveRoyalFavor.signalListenMode = QuestPart.SignalListenMode.Always;
				}
				else
				{
					questPart_GiveRoyalFavor.inSignal = _003Cslate_003E5__2.Get<string>("inSignal");
					questPart_GiveRoyalFavor.giveToAccepter = true;
				}
				_003C_003E2__current = questPart_GiveRoyalFavor;
				_003C_003E1__state = 1;
				return true;
			}
			case 1:
				_003C_003E1__state = -1;
				_003Cslate_003E5__2.Set("royalFavorReward_amount", reward_RoyalFavor.amount);
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
			_003CGenerateQuestParts_003Ed__7 _003CGenerateQuestParts_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CGenerateQuestParts_003Ed__ = this;
			}
			else
			{
				_003CGenerateQuestParts_003Ed__ = new _003CGenerateQuestParts_003Ed__7(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			_003CGenerateQuestParts_003Ed__.parms = _003C_003E3__parms;
			return _003CGenerateQuestParts_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<QuestPart>)this).GetEnumerator();
		}
	}

	[CompilerGenerated]
	private sealed class _003Cget_StackElements_003Ed__5 : IEnumerable<GenUI.AnonymousStackElement>, IEnumerable, IEnumerator<GenUI.AnonymousStackElement>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private GenUI.AnonymousStackElement _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public Reward_RoyalFavor _003C_003E4__this;

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
		public _003Cget_StackElements_003Ed__5(int _003C_003E1__state)
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
			Reward_RoyalFavor CS_0024_003C_003E8__locals7 = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				_003C_003E2__current = QuestPartUtility.GetStandardRewardStackElement(CS_0024_003C_003E8__locals7.faction.def.royalFavorLabel.CapitalizeFirst() + " " + CS_0024_003C_003E8__locals7.amount.ToStringWithSign(), CS_0024_003C_003E8__locals7.faction.def.RoyalFavorIcon, () => ("RoyalFavorTip".Translate(Faction.OfPlayer.def.pawnsPlural, CS_0024_003C_003E8__locals7.amount, CS_0024_003C_003E8__locals7.faction.def.royalFavorLabel, CS_0024_003C_003E8__locals7.faction) + "\n\n" + "ClickForMoreInfo".Translate().Colorize(ColoredText.SubtleGrayColor)).Resolve(), delegate
				{
					Find.WindowStack.Add(new Dialog_InfoCard(CS_0024_003C_003E8__locals7.faction));
				});
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
			_003Cget_StackElements_003Ed__5 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003Cget_StackElements_003Ed__5(0)
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

	public int amount;

	public Faction faction;

	public override bool MakesUseOfChosenPawnSignal => true;

	public override IEnumerable<GenUI.AnonymousStackElement> StackElements
	{
		[IteratorStateMachine(typeof(_003Cget_StackElements_003Ed__5))]
		get
		{
			//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
			return new _003Cget_StackElements_003Ed__5(-2)
			{
				_003C_003E4__this = this
			};
		}
	}

	public override void InitFromValue(float rewardValue, RewardsGeneratorParams parms, out float valueActuallyUsed)
	{
		amount = GenMath.RoundRandom(RewardsGenerator.RewardValueToRoyalFavorCurve.Evaluate(rewardValue));
		amount = Mathf.Clamp(amount, 1, 12);
		valueActuallyUsed = RewardsGenerator.RewardValueToRoyalFavorCurve.EvaluateInverted(amount);
		faction = parms.giverFaction;
	}

	[IteratorStateMachine(typeof(_003CGenerateQuestParts_003Ed__7))]
	public override IEnumerable<QuestPart> GenerateQuestParts(int index, RewardsGeneratorParams parms, string customLetterLabel, string customLetterText, RulePack customLetterLabelRules, RulePack customLetterTextRules)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGenerateQuestParts_003Ed__7(-2)
		{
			_003C_003E4__this = this,
			_003C_003E3__parms = parms
		};
	}

	public override string GetDescription(RewardsGeneratorParams parms)
	{
		if (!parms.chosenPawnSignal.NullOrEmpty())
		{
			return "Reward_RoyalFavor_ChoosePawn".Translate(faction, amount, Faction.OfPlayer.def.pawnsPlural).Resolve();
		}
		return "Reward_RoyalFavor".Translate(faction, amount).Resolve();
	}

	public override string ToString()
	{
		return GetType().Name + " (faction=" + faction.ToStringSafe() + ", amount=" + amount + ")";
	}

	public override void ExposeData()
	{
		base.ExposeData();
		Scribe_Values.Look(ref amount, "amount", 0);
		Scribe_References.Look(ref faction, "faction");
	}
}
