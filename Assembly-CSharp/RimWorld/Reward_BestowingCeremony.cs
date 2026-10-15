using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;
using Verse.Grammar;

namespace RimWorld;

[StaticConstructorOnStartup]
public class Reward_BestowingCeremony : Reward
{
	[CompilerGenerated]
	private sealed class _003Cget_StackElements_003Ed__7 : IEnumerable<GenUI.AnonymousStackElement>, IEnumerable, IEnumerator<GenUI.AnonymousStackElement>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private GenUI.AnonymousStackElement _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public Reward_BestowingCeremony _003C_003E4__this;

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
		public _003Cget_StackElements_003Ed__7(int _003C_003E1__state)
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
			Reward_BestowingCeremony CS_0024_003C_003E8__locals10 = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				if (CS_0024_003C_003E8__locals10.givePsylink)
				{
					_003C_003E2__current = QuestPartUtility.GetStandardRewardStackElement("Reward_BestowingCeremony_Label".Translate(), IconPsylink, () => CS_0024_003C_003E8__locals10.GetDescription(default(RewardsGeneratorParams)).CapitalizeFirst() + ".", delegate
					{
						Find.WindowStack.Add(new Dialog_InfoCard(HediffDefOf.PsychicAmplifier));
					});
					_003C_003E1__state = 1;
					return true;
				}
				goto IL_008b;
			case 1:
				_003C_003E1__state = -1;
				goto IL_008b;
			case 2:
				{
					_003C_003E1__state = -1;
					return false;
				}
				IL_008b:
				_003C_003E2__current = QuestPartUtility.GetStandardRewardStackElement("Reward_Title_Label".Translate(CS_0024_003C_003E8__locals10.titleName.Named("TITLE")), CS_0024_003C_003E8__locals10.awardingFaction.def.FactionIcon, () => "Reward_Title".Translate(CS_0024_003C_003E8__locals10.targetPawnName.Named("PAWNNAME"), CS_0024_003C_003E8__locals10.titleName.Named("TITLE"), CS_0024_003C_003E8__locals10.awardingFaction.Named("FACTION")).Resolve() + ".", delegate
				{
					if (CS_0024_003C_003E8__locals10.royalTitle != null)
					{
						Find.WindowStack.Add(new Dialog_InfoCard(CS_0024_003C_003E8__locals10.royalTitle, CS_0024_003C_003E8__locals10.awardingFaction));
					}
				});
				_003C_003E1__state = 2;
				return true;
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
			_003Cget_StackElements_003Ed__7 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003Cget_StackElements_003Ed__7(0)
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

	private static readonly Texture2D IconPsylink = ContentFinder<Texture2D>.Get("Things/Item/Special/PsylinkNeuroformer");

	public string targetPawnName;

	public string titleName;

	public RoyalTitleDef royalTitle;

	public bool givePsylink;

	public Faction awardingFaction;

	public override IEnumerable<GenUI.AnonymousStackElement> StackElements
	{
		[IteratorStateMachine(typeof(_003Cget_StackElements_003Ed__7))]
		get
		{
			//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
			return new _003Cget_StackElements_003Ed__7(-2)
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
		return "Reward_BestowingCeremony".Translate(targetPawnName.Named("PAWNNAME")).Resolve();
	}

	public override void ExposeData()
	{
		base.ExposeData();
		Scribe_Values.Look(ref targetPawnName, "targetPawnName");
		Scribe_Values.Look(ref titleName, "titleName");
		Scribe_Values.Look(ref givePsylink, "givePsylink", defaultValue: false);
		Scribe_References.Look(ref awardingFaction, "awardingFaction");
		Scribe_Defs.Look(ref royalTitle, "royalTitle");
	}
}
