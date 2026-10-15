using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;
using Verse.Grammar;

namespace RimWorld;

public class Reward_DevelopmentPoints : Reward
{
	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass4_0
	{
		public Ideo fluidIdeo;

		public Reward_DevelopmentPoints _003C_003E4__this;

		internal void _003Cget_StackElements_003Eb__0(Rect rect)
		{
			fluidIdeo.DrawIcon(rect);
		}

		internal string _003Cget_StackElements_003Eb__1()
		{
			return _003C_003E4__this.GetDescription(default(RewardsGeneratorParams)).CapitalizeFirst();
		}

		internal void _003Cget_StackElements_003Eb__2()
		{
			Find.MainTabsRoot.SetCurrentTab(MainButtonDefOf.Ideos);
			IdeoUIUtility.SetSelected(fluidIdeo);
		}
	}

	[CompilerGenerated]
	private sealed class _003Cget_StackElements_003Ed__4 : IEnumerable<GenUI.AnonymousStackElement>, IEnumerable, IEnumerator<GenUI.AnonymousStackElement>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private GenUI.AnonymousStackElement _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public Reward_DevelopmentPoints _003C_003E4__this;

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
			Reward_DevelopmentPoints reward_DevelopmentPoints = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
			{
				_003C_003E1__state = -1;
				_003C_003Ec__DisplayClass4_0 CS_0024_003C_003E8__locals6 = new _003C_003Ec__DisplayClass4_0
				{
					_003C_003E4__this = _003C_003E4__this,
					fluidIdeo = Faction.OfPlayer.ideos.FluidIdeo
				};
				if (CS_0024_003C_003E8__locals6.fluidIdeo == null || !CS_0024_003C_003E8__locals6.fluidIdeo.development.CanBeDevelopedNow)
				{
					return false;
				}
				int num2 = IdeoDevelopmentUtility.DevelopmentPointsForQuestSuccess(CS_0024_003C_003E8__locals6.fluidIdeo, reward_DevelopmentPoints.quest.root);
				if (num2 <= 0)
				{
					return false;
				}
				_003C_003E2__current = QuestPartUtility.GetStandardRewardStackElement("Reward_DevelopmentPointsLabel".Translate() + ": " + num2.ToStringWithSign(), delegate(Rect rect)
				{
					CS_0024_003C_003E8__locals6.fluidIdeo.DrawIcon(rect);
				}, () => CS_0024_003C_003E8__locals6._003C_003E4__this.GetDescription(default(RewardsGeneratorParams)).CapitalizeFirst(), delegate
				{
					Find.MainTabsRoot.SetCurrentTab(MainButtonDefOf.Ideos);
					IdeoUIUtility.SetSelected(CS_0024_003C_003E8__locals6.fluidIdeo);
				});
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

	public Quest quest;

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

	public Reward_DevelopmentPoints()
	{
	}

	public Reward_DevelopmentPoints(Quest quest)
	{
		this.quest = quest;
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
		Ideo fluidIdeo = Faction.OfPlayer.ideos.FluidIdeo;
		int num = IdeoDevelopmentUtility.DevelopmentPointsForQuestSuccess(fluidIdeo, quest.root);
		return "Reward_DevelopmentPoints".Translate(fluidIdeo, num, fluidIdeo.development.points, fluidIdeo.development.NextReformationDevelopmentPoints).Resolve();
	}

	public override void ExposeData()
	{
		base.ExposeData();
		Scribe_References.Look(ref quest, "quest");
	}
}
