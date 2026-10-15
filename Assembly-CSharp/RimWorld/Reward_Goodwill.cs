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

public class Reward_Goodwill : Reward
{
	[CompilerGenerated]
	private sealed class _003CGenerateQuestParts_003Ed__5 : IEnumerable<QuestPart>, IEnumerable, IEnumerator<QuestPart>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private QuestPart _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public Reward_Goodwill _003C_003E4__this;

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
			int num = _003C_003E1__state;
			Reward_Goodwill reward_Goodwill = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				_003C_003E2__current = new QuestPart_FactionGoodwillChange
				{
					change = reward_Goodwill.amount,
					faction = reward_Goodwill.faction,
					inSignal = RimWorld.QuestGen.QuestGen.slate.Get<string>("inSignal")
				};
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
		IEnumerator<QuestPart> IEnumerable<QuestPart>.GetEnumerator()
		{
			_003CGenerateQuestParts_003Ed__5 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CGenerateQuestParts_003Ed__5(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
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

		public Reward_Goodwill _003C_003E4__this;

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
			Reward_Goodwill CS_0024_003C_003E8__locals8 = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				_003C_003E2__current = QuestPartUtility.GetStandardRewardStackElement("Goodwill".Translate() + " " + CS_0024_003C_003E8__locals8.amount.ToStringWithSign(), delegate(Rect r)
				{
					GUI.color = CS_0024_003C_003E8__locals8.faction.Color;
					GUI.DrawTexture(r, CS_0024_003C_003E8__locals8.faction.def.FactionIcon);
					GUI.color = Color.white;
				}, () => "GoodwillTip".Translate(CS_0024_003C_003E8__locals8.faction, CS_0024_003C_003E8__locals8.amount, -75, 75, CS_0024_003C_003E8__locals8.faction.PlayerGoodwill, CS_0024_003C_003E8__locals8.faction.PlayerRelationKind.GetLabelCap()).Resolve(), delegate
				{
					Find.WindowStack.Add(new Dialog_InfoCard(CS_0024_003C_003E8__locals8.faction));
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

	public int amount;

	public Faction faction;

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
		amount = GenMath.RoundRandom(RewardsGenerator.RewardValueToGoodwillCurve.Evaluate(rewardValue));
		amount = Mathf.Min(amount, 100 - parms.giverFaction.PlayerGoodwill);
		amount = Mathf.Max(amount, 1);
		valueActuallyUsed = RewardsGenerator.RewardValueToGoodwillCurve.EvaluateInverted(amount);
		if (parms.giverFaction.HostileTo(Faction.OfPlayer))
		{
			amount += Mathf.Clamp(-parms.giverFaction.PlayerGoodwill / 2, 0, amount);
			amount = Mathf.Min(amount, 100 - parms.giverFaction.PlayerGoodwill);
			if (amount < 1)
			{
				Log.Warning("Tried to use " + amount + " goodwill in Reward_Goodwill. A different reward type should have been chosen in this case.");
				amount = 1;
			}
		}
		faction = parms.giverFaction;
	}

	[IteratorStateMachine(typeof(_003CGenerateQuestParts_003Ed__5))]
	public override IEnumerable<QuestPart> GenerateQuestParts(int index, RewardsGeneratorParams parms, string customLetterLabel, string customLetterText, RulePack customLetterLabelRules, RulePack customLetterTextRules)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGenerateQuestParts_003Ed__5(-2)
		{
			_003C_003E4__this = this
		};
	}

	public override string GetDescription(RewardsGeneratorParams parms)
	{
		return "Reward_Goodwill".Translate(faction, amount).Resolve();
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
