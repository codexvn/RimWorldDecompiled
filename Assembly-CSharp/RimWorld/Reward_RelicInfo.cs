using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;
using Verse.Grammar;

namespace RimWorld;

public class Reward_RelicInfo : Reward
{
	[CompilerGenerated]
	private sealed class _003Cget_StackElements_003Ed__3 : IEnumerable<GenUI.AnonymousStackElement>, IEnumerable, IEnumerator<GenUI.AnonymousStackElement>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private GenUI.AnonymousStackElement _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public Reward_RelicInfo _003C_003E4__this;

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
			Reward_RelicInfo CS_0024_003C_003E8__locals5 = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				_003C_003E2__current = QuestPartUtility.GetStandardRewardStackElement("Reward_RelicInfoLabel".Translate(CS_0024_003C_003E8__locals5.relic.Label), delegate(Rect rect)
				{
					CS_0024_003C_003E8__locals5.relic.DrawIcon(rect);
				}, () => CS_0024_003C_003E8__locals5.GetDescription(default(RewardsGeneratorParams)).CapitalizeFirst() + ".", delegate
				{
					if (CS_0024_003C_003E8__locals5.quest?.parent != null)
					{
						Find.MainTabsRoot.SetCurrentTab(MainButtonDefOf.Quests);
						((MainTabWindow_Quests)MainButtonDefOf.Quests.TabWindow).Select(CS_0024_003C_003E8__locals5.quest.parent);
					}
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

	public Precept_Relic relic;

	public Quest quest;

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

	public override IEnumerable<QuestPart> GenerateQuestParts(int index, RewardsGeneratorParams parms, string customLetterLabel, string customLetterText, RulePack customLetterLabelRules, RulePack customLetterTextRules)
	{
		throw new NotImplementedException();
	}

	public override string GetDescription(RewardsGeneratorParams parms)
	{
		return "Reward_RelicInfo".Translate(relic.Label);
	}

	public override void ExposeData()
	{
		base.ExposeData();
		Scribe_References.Look(ref relic, "relic");
		Scribe_References.Look(ref quest, "quest");
	}
}
