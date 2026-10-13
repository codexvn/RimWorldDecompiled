using System;
using System.Collections.Generic;
using RimWorld.QuestGen;
using UnityEngine;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x0200369D RID: 13981
	public class Reward_RoyalFavor : Reward
	{
		// Token: 0x17003206 RID: 12806
		// (get) Token: 0x0601369C RID: 79516 RVA: 0x000028E7 File Offset: 0x00000AE7
		public override bool MakesUseOfChosenPawnSignal
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17003207 RID: 12807
		// (get) Token: 0x0601369D RID: 79517 RVA: 0x005C4F77 File Offset: 0x005C3177
		public override IEnumerable<GenUI.AnonymousStackElement> StackElements
		{
			get
			{
				yield return QuestPartUtility.GetStandardRewardStackElement(this.faction.def.royalFavorLabel.CapitalizeFirst() + " " + this.amount.ToStringWithSign(), this.faction.def.RoyalFavorIcon, () => ("RoyalFavorTip".Translate(Faction.OfPlayer.def.pawnsPlural, this.amount, this.faction.def.royalFavorLabel, this.faction) + "\n\n" + "ClickForMoreInfo".Translate().Colorize(ColoredText.SubtleGrayColor)).Resolve(), delegate()
				{
					Find.WindowStack.Add(new Dialog_InfoCard(this.faction));
				});
				yield break;
			}
		}

		// Token: 0x0601369E RID: 79518 RVA: 0x005C4F88 File Offset: 0x005C3188
		public override void InitFromValue(float rewardValue, RewardsGeneratorParams parms, out float valueActuallyUsed)
		{
			this.amount = GenMath.RoundRandom(RewardsGenerator.RewardValueToRoyalFavorCurve.Evaluate(rewardValue));
			this.amount = Mathf.Clamp(this.amount, 1, 12);
			valueActuallyUsed = RewardsGenerator.RewardValueToRoyalFavorCurve.EvaluateInverted((float)this.amount);
			this.faction = parms.giverFaction;
		}

		// Token: 0x0601369F RID: 79519 RVA: 0x005C4FDE File Offset: 0x005C31DE
		public override IEnumerable<QuestPart> GenerateQuestParts(int index, RewardsGeneratorParams parms, string customLetterLabel, string customLetterText, RulePack customLetterLabelRules, RulePack customLetterTextRules)
		{
			Slate slate = QuestGen.slate;
			QuestPart_GiveRoyalFavor questPart_GiveRoyalFavor = new QuestPart_GiveRoyalFavor();
			questPart_GiveRoyalFavor.faction = this.faction;
			questPart_GiveRoyalFavor.amount = this.amount;
			if (!parms.chosenPawnSignal.NullOrEmpty())
			{
				questPart_GiveRoyalFavor.inSignal = QuestGenUtility.HardcodedSignalWithQuestID(parms.chosenPawnSignal);
				questPart_GiveRoyalFavor.signalListenMode = QuestPart.SignalListenMode.Always;
			}
			else
			{
				questPart_GiveRoyalFavor.inSignal = slate.Get<string>("inSignal", null, false);
				questPart_GiveRoyalFavor.giveToAccepter = true;
			}
			yield return questPart_GiveRoyalFavor;
			slate.Set<int>("royalFavorReward_amount", this.amount, false);
			yield break;
		}

		// Token: 0x060136A0 RID: 79520 RVA: 0x005C4FF8 File Offset: 0x005C31F8
		public override string GetDescription(RewardsGeneratorParams parms)
		{
			if (!parms.chosenPawnSignal.NullOrEmpty())
			{
				return "Reward_RoyalFavor_ChoosePawn".Translate(this.faction, this.amount, Faction.OfPlayer.def.pawnsPlural).Resolve();
			}
			return "Reward_RoyalFavor".Translate(this.faction, this.amount).Resolve();
		}

		// Token: 0x060136A1 RID: 79521 RVA: 0x005C5078 File Offset: 0x005C3278
		public override string ToString()
		{
			return string.Concat(new string[]
			{
				base.GetType().Name,
				" (faction=",
				this.faction.ToStringSafe<Faction>(),
				", amount=",
				this.amount.ToString(),
				")"
			});
		}

		// Token: 0x060136A2 RID: 79522 RVA: 0x005C50D2 File Offset: 0x005C32D2
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.amount, "amount", 0, false);
			Scribe_References.Look<Faction>(ref this.faction, "faction", false);
		}

		// Token: 0x0400D738 RID: 55096
		public int amount;

		// Token: 0x0400D739 RID: 55097
		public Faction faction;
	}
}
