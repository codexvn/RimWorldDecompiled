using System;
using System.Collections.Generic;
using Verse;
using Verse.Grammar;

namespace RimWorld.QuestGen
{
	// Token: 0x02003C0C RID: 15372
	[StaticConstructorOnStartup]
	public class Reward_ReimplantXenogerm : Reward
	{
		// Token: 0x1700338E RID: 13198
		// (get) Token: 0x06014D91 RID: 85393 RVA: 0x0063EAC0 File Offset: 0x0063CCC0
		public override IEnumerable<GenUI.AnonymousStackElement> StackElements
		{
			get
			{
				yield return QuestPartUtility.GetStandardRewardStackElement("Reward_ReimplantXenogerm_Label".Translate(), Reward_ReimplantXenogerm.Icon.Texture, () => this.GetDescription(default(RewardsGeneratorParams)).CapitalizeFirst(), null);
				yield break;
			}
		}

		// Token: 0x06014D92 RID: 85394 RVA: 0x00066B11 File Offset: 0x00064D11
		public override void InitFromValue(float rewardValue, RewardsGeneratorParams parms, out float valueActuallyUsed)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06014D93 RID: 85395 RVA: 0x0063EAD0 File Offset: 0x0063CCD0
		public override IEnumerable<QuestPart> GenerateQuestParts(int index, RewardsGeneratorParams parms, string customLetterLabel, string customLetterText, RulePack customLetterLabelRules, RulePack customLetterTextRules)
		{
			if (!ModLister.CheckBiotech("reimplaning reward"))
			{
				yield break;
			}
			Slate slate = QuestGen.slate;
			QuestPart_ReimplantXenogerm questPart_ReimplantXenogerm = new QuestPart_ReimplantXenogerm();
			questPart_ReimplantXenogerm.mapParent = slate.Get<Map>("map", null, false).Parent;
			questPart_ReimplantXenogerm.faction = slate.Get<Faction>("faction", null, false);
			questPart_ReimplantXenogerm.pawns.AddRange(slate.Get<List<Pawn>>("sanguophages", null, false));
			questPart_ReimplantXenogerm.gatherSpot = slate.Get<IntVec3>("gatherSpot", default(IntVec3), false);
			questPart_ReimplantXenogerm.inSignal = QuestGen.slate.Get<string>("inSignal", null, false);
			questPart_ReimplantXenogerm.inSignalReimplanted = slate.Get<string>("xenogermReimplantedSignal", null, false);
			questPart_ReimplantXenogerm.waitDurationTicks = 15000;
			questPart_ReimplantXenogerm.inSignalRemovePawn = QuestGenUtility.HardcodedSignalWithQuestID("sanguophages.ChangedFactionToPlayer");
			yield return questPart_ReimplantXenogerm;
			yield break;
		}

		// Token: 0x06014D94 RID: 85396 RVA: 0x0063EADC File Offset: 0x0063CCDC
		public override string GetDescription(RewardsGeneratorParams parms)
		{
			return "Reward_ReimplantXenogerm_Desc".Translate().Resolve();
		}

		// Token: 0x0400E876 RID: 59510
		private static readonly CachedTexture Icon = new CachedTexture("UI/Icons/Genes/Gene_XenogermReimplanter");

		// Token: 0x0400E877 RID: 59511
		public const int ReimplantWaitDurationTicks = 15000;
	}
}
