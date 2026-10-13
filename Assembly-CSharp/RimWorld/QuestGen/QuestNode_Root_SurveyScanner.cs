using System;
using RimWorld.Planet;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02003B9D RID: 15261
	public class QuestNode_Root_SurveyScanner : QuestNode
	{
		// Token: 0x06014B97 RID: 84887 RVA: 0x00634E18 File Offset: 0x00633018
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			Quest quest = QuestGen.quest;
			float num = QuestGen.slate.Get<float>("points", 0f, false);
			string text = QuestGenUtility.HardcodedTargetQuestTagWithQuestID("scanner");
			string inSignalEnable = QuestGenUtility.HardcodedSignalWithQuestID("site.MapGenerated");
			string inSignal = QuestGenUtility.HardcodedSignalWithQuestID("site.MapRemoved");
			string text2 = QuestGenUtility.HardcodedSignalWithQuestID(text + ".Destroyed");
			string completedSignal = QuestGenUtility.HardcodedSignalWithQuestID("site.SurveyCompleted");
			string text3 = QuestGenUtility.HardcodedSignalWithQuestID("site.Raid");
			int value = this.duration.GetValue(slate);
			Site site = (Site)this.site.GetValue(slate);
			int fireRaidRemainingTicks = (int)(this.raidAttackRemainingHoursRange.GetValue(slate).RandomInRange * 2500f);
			Faction faction;
			if (Rand.Chance(this.raidChance.GetValue(slate)) && Find.FactionManager.TryGetRandomNonColonyHumanlikeFaction(out faction, true, false, TechLevel.Undefined, TechLevel.Undefined, false, true))
			{
				slate.Set<Faction>("enemyFaction", faction, false);
				Quest quest2 = quest;
				MapParent parent = site;
				float points = QuestNode_Root_SurveyScanner.RaidPointsRandomFactor.RandomInRange * num;
				Faction faction2 = faction;
				string inSignalLeave = null;
				string inSignal2 = text3;
				RaidStrategyDef immediateAttack = RaidStrategyDefOf.ImmediateAttack;
				quest2.Raid(parent, points, faction2, inSignalLeave, this.raidLetterLabel, this.raidLetterText, null, null, null, null, inSignal2, "root", null, immediateAttack, null, false, true, true, true);
			}
			quest.AddPart(new QuestPart_SurveyScanner(site, inSignalEnable, text, value));
			quest.AddPart(new QuestPart_ScannerDurationRemainingAlert(site, inSignalEnable, text2, completedSignal, text3, value, fireRaidRemainingTicks));
			quest.End(QuestEndOutcome.Fail, 0, null, text2, QuestPart.SignalListenMode.OngoingOnly, true, false);
			quest.End(QuestEndOutcome.Fail, 0, null, inSignal, QuestPart.SignalListenMode.OngoingOnly, true, false);
		}

		// Token: 0x06014B98 RID: 84888 RVA: 0x00624C29 File Offset: 0x00622E29
		protected override bool TestRunInt(Slate slate)
		{
			return ModsConfig.OdysseyActive;
		}

		// Token: 0x0400E697 RID: 59031
		private SlateRef<WorldObject> site;

		// Token: 0x0400E698 RID: 59032
		private SlateRef<int> duration;

		// Token: 0x0400E699 RID: 59033
		private SlateRef<float> raidChance;

		// Token: 0x0400E69A RID: 59034
		private SlateRef<FloatRange> raidAttackRemainingHoursRange;

		// Token: 0x0400E69B RID: 59035
		private string raidLetterLabel;

		// Token: 0x0400E69C RID: 59036
		private string raidLetterText;

		// Token: 0x0400E69D RID: 59037
		private const string ReasonSymbol = "attackReason";

		// Token: 0x0400E69E RID: 59038
		private const string SiteAlias = "site";

		// Token: 0x0400E69F RID: 59039
		private const string SurveyCompleted = "SurveyCompleted";

		// Token: 0x0400E6A0 RID: 59040
		private const string SiteRaid = "Raid";

		// Token: 0x0400E6A1 RID: 59041
		private static readonly FloatRange RaidPointsRandomFactor = new FloatRange(0.9f, 1.1f);
	}
}
