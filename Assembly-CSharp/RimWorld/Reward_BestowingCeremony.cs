using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x0200367D RID: 13949
	[StaticConstructorOnStartup]
	public class Reward_BestowingCeremony : Reward
	{
		// Token: 0x170031DE RID: 12766
		// (get) Token: 0x060135C1 RID: 79297 RVA: 0x005C2D03 File Offset: 0x005C0F03
		public override IEnumerable<GenUI.AnonymousStackElement> StackElements
		{
			get
			{
				if (this.givePsylink)
				{
					yield return QuestPartUtility.GetStandardRewardStackElement("Reward_BestowingCeremony_Label".Translate(), Reward_BestowingCeremony.IconPsylink, () => this.GetDescription(default(RewardsGeneratorParams)).CapitalizeFirst() + ".", delegate()
					{
						Find.WindowStack.Add(new Dialog_InfoCard(HediffDefOf.PsychicAmplifier, null));
					});
				}
				yield return QuestPartUtility.GetStandardRewardStackElement("Reward_Title_Label".Translate(this.titleName.Named("TITLE")), this.awardingFaction.def.FactionIcon, () => "Reward_Title".Translate(this.targetPawnName.Named("PAWNNAME"), this.titleName.Named("TITLE"), this.awardingFaction.Named("FACTION")).Resolve() + ".", delegate()
				{
					if (this.royalTitle != null)
					{
						Find.WindowStack.Add(new Dialog_InfoCard(this.royalTitle, this.awardingFaction, null));
					}
				});
				yield break;
			}
		}

		// Token: 0x060135C2 RID: 79298 RVA: 0x00066B11 File Offset: 0x00064D11
		public override void InitFromValue(float rewardValue, RewardsGeneratorParams parms, out float valueActuallyUsed)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060135C3 RID: 79299 RVA: 0x00066B11 File Offset: 0x00064D11
		public override IEnumerable<QuestPart> GenerateQuestParts(int index, RewardsGeneratorParams parms, string customLetterLabel, string customLetterText, RulePack customLetterLabelRules, RulePack customLetterTextRules)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060135C4 RID: 79300 RVA: 0x005C2D14 File Offset: 0x005C0F14
		public override string GetDescription(RewardsGeneratorParams parms)
		{
			return "Reward_BestowingCeremony".Translate(this.targetPawnName.Named("PAWNNAME")).Resolve();
		}

		// Token: 0x060135C5 RID: 79301 RVA: 0x005C2D44 File Offset: 0x005C0F44
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.targetPawnName, "targetPawnName", null, false);
			Scribe_Values.Look<string>(ref this.titleName, "titleName", null, false);
			Scribe_Values.Look<bool>(ref this.givePsylink, "givePsylink", false, false);
			Scribe_References.Look<Faction>(ref this.awardingFaction, "awardingFaction", false);
			Scribe_Defs.Look<RoyalTitleDef>(ref this.royalTitle, "royalTitle");
		}

		// Token: 0x0400D6C4 RID: 54980
		private static readonly Texture2D IconPsylink = ContentFinder<Texture2D>.Get("Things/Item/Special/PsylinkNeuroformer", true);

		// Token: 0x0400D6C5 RID: 54981
		public string targetPawnName;

		// Token: 0x0400D6C6 RID: 54982
		public string titleName;

		// Token: 0x0400D6C7 RID: 54983
		public RoyalTitleDef royalTitle;

		// Token: 0x0400D6C8 RID: 54984
		public bool givePsylink;

		// Token: 0x0400D6C9 RID: 54985
		public Faction awardingFaction;
	}
}
