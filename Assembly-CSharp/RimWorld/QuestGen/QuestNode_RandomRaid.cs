using System;
using Verse;
using Verse.Grammar;

namespace RimWorld.QuestGen
{
	// Token: 0x02003BF9 RID: 15353
	public class QuestNode_RandomRaid : QuestNode
	{
		// Token: 0x06014D32 RID: 85298 RVA: 0x0063D210 File Offset: 0x0063B410
		protected override bool TestRunInt(Slate slate)
		{
			return Find.Storyteller.difficulty.allowViolentQuests && slate.Exists("map", false) && (slate.Exists("enemyFaction", false) || this.faction.GetValue(slate) != null);
		}

		// Token: 0x06014D33 RID: 85299 RVA: 0x0063D260 File Offset: 0x0063B460
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			Map map = QuestGen.slate.Get<Map>("map", null, false);
			float val = QuestGen.slate.Get<float>("points", 0f, false);
			Faction faction = this.faction.GetValue(slate) ?? QuestGen.slate.Get<Faction>("enemyFaction", null, false);
			QuestPart_RandomRaid randomRaid = new QuestPart_RandomRaid();
			randomRaid.inSignal = (QuestGenUtility.HardcodedSignalWithQuestID(this.inSignal.GetValue(slate)) ?? QuestGen.slate.Get<string>("inSignal", null, false));
			randomRaid.mapParent = map.Parent;
			randomRaid.faction = faction;
			randomRaid.pointsRange = QuestNode_RandomRaid.RaidPointsRandomFactor * val;
			randomRaid.useCurrentThreatPoints = this.useCurrentThreatPoints.GetValue(slate);
			randomRaid.currentThreatPointsFactor = (this.currentThreatPointsFactor.GetValue(slate) ?? 1f);
			if (this.arrivalMode.GetValue(slate) != null)
			{
				randomRaid.arrivalMode = this.arrivalMode.GetValue(slate);
			}
			if (!this.customLetterLabel.GetValue(slate).NullOrEmpty() || this.customLetterLabelRules.GetValue(slate) != null)
			{
				QuestGen.AddTextRequest("root", delegate(string x)
				{
					randomRaid.customLetterLabel = x;
				}, QuestGenUtility.MergeRules(this.customLetterLabelRules.GetValue(slate), this.customLetterLabel.GetValue(slate), "root"));
			}
			if (!this.customLetterText.GetValue(slate).NullOrEmpty() || this.customLetterTextRules.GetValue(slate) != null)
			{
				QuestGen.AddTextRequest("root", delegate(string x)
				{
					randomRaid.customLetterText = x;
				}, QuestGenUtility.MergeRules(this.customLetterTextRules.GetValue(slate), this.customLetterText.GetValue(slate), "root"));
			}
			QuestGen.quest.AddPart(randomRaid);
		}

		// Token: 0x0400E7FA RID: 59386
		[NoTranslate]
		public SlateRef<string> inSignal;

		// Token: 0x0400E7FB RID: 59387
		public SlateRef<Faction> faction;

		// Token: 0x0400E7FC RID: 59388
		public SlateRef<bool> useCurrentThreatPoints;

		// Token: 0x0400E7FD RID: 59389
		public SlateRef<float?> currentThreatPointsFactor;

		// Token: 0x0400E7FE RID: 59390
		public SlateRef<PawnsArrivalModeDef> arrivalMode;

		// Token: 0x0400E7FF RID: 59391
		public SlateRef<string> customLetterLabel;

		// Token: 0x0400E800 RID: 59392
		public SlateRef<string> customLetterText;

		// Token: 0x0400E801 RID: 59393
		public SlateRef<RulePack> customLetterLabelRules;

		// Token: 0x0400E802 RID: 59394
		public SlateRef<RulePack> customLetterTextRules;

		// Token: 0x0400E803 RID: 59395
		private const string RootSymbol = "root";

		// Token: 0x0400E804 RID: 59396
		private static readonly FloatRange RaidPointsRandomFactor = new FloatRange(0.9f, 1.1f);
	}
}
