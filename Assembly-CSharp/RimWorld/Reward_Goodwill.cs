using System;
using System.Collections.Generic;
using RimWorld.QuestGen;
using UnityEngine;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x02003688 RID: 13960
	public class Reward_Goodwill : Reward
	{
		// Token: 0x170031EC RID: 12780
		// (get) Token: 0x06013610 RID: 79376 RVA: 0x005C358B File Offset: 0x005C178B
		public override IEnumerable<GenUI.AnonymousStackElement> StackElements
		{
			get
			{
				yield return QuestPartUtility.GetStandardRewardStackElement("Goodwill".Translate() + " " + this.amount.ToStringWithSign(), delegate(Rect r)
				{
					GUI.color = this.faction.Color;
					GUI.DrawTexture(r, this.faction.def.FactionIcon);
					GUI.color = Color.white;
				}, () => "GoodwillTip".Translate(this.faction, this.amount, -75, 75, this.faction.PlayerGoodwill, this.faction.PlayerRelationKind.GetLabelCap()).Resolve(), delegate()
				{
					Find.WindowStack.Add(new Dialog_InfoCard(this.faction));
				});
				yield break;
			}
		}

		// Token: 0x06013611 RID: 79377 RVA: 0x005C359C File Offset: 0x005C179C
		public override void InitFromValue(float rewardValue, RewardsGeneratorParams parms, out float valueActuallyUsed)
		{
			this.amount = GenMath.RoundRandom(RewardsGenerator.RewardValueToGoodwillCurve.Evaluate(rewardValue));
			this.amount = Mathf.Min(this.amount, 100 - parms.giverFaction.PlayerGoodwill);
			this.amount = Mathf.Max(this.amount, 1);
			valueActuallyUsed = RewardsGenerator.RewardValueToGoodwillCurve.EvaluateInverted((float)this.amount);
			if (parms.giverFaction.HostileTo(Faction.OfPlayer))
			{
				this.amount += Mathf.Clamp(-parms.giverFaction.PlayerGoodwill / 2, 0, this.amount);
				this.amount = Mathf.Min(this.amount, 100 - parms.giverFaction.PlayerGoodwill);
				if (this.amount < 1)
				{
					Log.Warning("Tried to use " + this.amount.ToString() + " goodwill in Reward_Goodwill. A different reward type should have been chosen in this case.");
					this.amount = 1;
				}
			}
			this.faction = parms.giverFaction;
		}

		// Token: 0x06013612 RID: 79378 RVA: 0x005C3696 File Offset: 0x005C1896
		public override IEnumerable<QuestPart> GenerateQuestParts(int index, RewardsGeneratorParams parms, string customLetterLabel, string customLetterText, RulePack customLetterLabelRules, RulePack customLetterTextRules)
		{
			yield return new QuestPart_FactionGoodwillChange
			{
				change = this.amount,
				faction = this.faction,
				inSignal = QuestGen.slate.Get<string>("inSignal", null, false)
			};
			yield break;
		}

		// Token: 0x06013613 RID: 79379 RVA: 0x005C36A8 File Offset: 0x005C18A8
		public override string GetDescription(RewardsGeneratorParams parms)
		{
			return "Reward_Goodwill".Translate(this.faction, this.amount).Resolve();
		}

		// Token: 0x06013614 RID: 79380 RVA: 0x005C36E0 File Offset: 0x005C18E0
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

		// Token: 0x06013615 RID: 79381 RVA: 0x005C373A File Offset: 0x005C193A
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.amount, "amount", 0, false);
			Scribe_References.Look<Faction>(ref this.faction, "faction", false);
		}

		// Token: 0x0400D6E4 RID: 55012
		public int amount;

		// Token: 0x0400D6E5 RID: 55013
		public Faction faction;
	}
}
