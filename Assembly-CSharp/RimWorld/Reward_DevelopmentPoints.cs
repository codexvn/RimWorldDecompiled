using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x02003685 RID: 13957
	public class Reward_DevelopmentPoints : Reward
	{
		// Token: 0x060135FD RID: 79357 RVA: 0x005C2DAE File Offset: 0x005C0FAE
		public Reward_DevelopmentPoints()
		{
		}

		// Token: 0x060135FE RID: 79358 RVA: 0x005C332F File Offset: 0x005C152F
		public Reward_DevelopmentPoints(Quest quest)
		{
			this.quest = quest;
		}

		// Token: 0x170031E9 RID: 12777
		// (get) Token: 0x060135FF RID: 79359 RVA: 0x005C333E File Offset: 0x005C153E
		public override IEnumerable<GenUI.AnonymousStackElement> StackElements
		{
			get
			{
				Ideo fluidIdeo = Faction.OfPlayer.ideos.FluidIdeo;
				if (fluidIdeo == null || !fluidIdeo.development.CanBeDevelopedNow)
				{
					yield break;
				}
				int num = IdeoDevelopmentUtility.DevelopmentPointsForQuestSuccess(fluidIdeo, this.quest.root);
				if (num <= 0)
				{
					yield break;
				}
				yield return QuestPartUtility.GetStandardRewardStackElement("Reward_DevelopmentPointsLabel".Translate() + ": " + num.ToStringWithSign(), delegate(Rect rect)
				{
					fluidIdeo.DrawIcon(rect);
				}, () => this.GetDescription(default(RewardsGeneratorParams)).CapitalizeFirst(), delegate()
				{
					Find.MainTabsRoot.SetCurrentTab(MainButtonDefOf.Ideos, true);
					IdeoUIUtility.SetSelected(fluidIdeo);
				});
				yield break;
			}
		}

		// Token: 0x06013600 RID: 79360 RVA: 0x00066B11 File Offset: 0x00064D11
		public override void InitFromValue(float rewardValue, RewardsGeneratorParams parms, out float valueActuallyUsed)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06013601 RID: 79361 RVA: 0x00066B11 File Offset: 0x00064D11
		public override IEnumerable<QuestPart> GenerateQuestParts(int index, RewardsGeneratorParams parms, string customLetterLabel, string customLetterText, RulePack customLetterLabelRules, RulePack customLetterTextRules)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06013602 RID: 79362 RVA: 0x005C3350 File Offset: 0x005C1550
		public override string GetDescription(RewardsGeneratorParams parms)
		{
			Ideo fluidIdeo = Faction.OfPlayer.ideos.FluidIdeo;
			int value = IdeoDevelopmentUtility.DevelopmentPointsForQuestSuccess(fluidIdeo, this.quest.root);
			return "Reward_DevelopmentPoints".Translate(fluidIdeo, value, fluidIdeo.development.points, fluidIdeo.development.NextReformationDevelopmentPoints).Resolve();
		}

		// Token: 0x06013603 RID: 79363 RVA: 0x005C33BD File Offset: 0x005C15BD
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Quest>(ref this.quest, "quest", false);
		}

		// Token: 0x0400D6DD RID: 55005
		public Quest quest;
	}
}
