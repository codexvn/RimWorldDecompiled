using System;
using System.Collections.Generic;
using Verse;
using Verse.Grammar;

namespace RimWorld.QuestGen
{
	// Token: 0x02003BF5 RID: 15349
	public class QuestNode_ManhunterPack : QuestNode
	{
		// Token: 0x06014D25 RID: 85285 RVA: 0x0063C9B8 File Offset: 0x0063ABB8
		protected override bool TestRunInt(Slate slate)
		{
			PawnKindDef pawnKindDef;
			return Find.Storyteller.difficulty.allowViolentQuests && slate.Exists("map", false) && AggressiveAnimalIncidentUtility.TryFindAggressiveAnimalKind(slate.Get<float>("points", 0f, false), slate.Get<Map>("map", null, false), out pawnKindDef);
		}

		// Token: 0x06014D26 RID: 85286 RVA: 0x0063CA14 File Offset: 0x0063AC14
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			Map map = QuestGen.slate.Get<Map>("map", null, false);
			float points = QuestGen.slate.Get<float>("points", 0f, false);
			QuestPart_Incident questPart_Incident = new QuestPart_Incident();
			questPart_Incident.incident = IncidentDefOf.ManhunterPack;
			IncidentParms incidentParms = new IncidentParms();
			incidentParms.forced = true;
			incidentParms.target = map;
			incidentParms.points = points;
			incidentParms.questTag = QuestGenUtility.HardcodedTargetQuestTagWithQuestID(this.tag.GetValue(slate));
			incidentParms.spawnCenter = (this.walkInSpot.GetValue(slate) ?? (QuestGen.slate.Get<IntVec3?>("walkInSpot", null, false) ?? IntVec3.Invalid));
			incidentParms.pawnCount = this.animalCount.GetValue(slate);
			PawnKindDef pawnKindDef;
			if (AggressiveAnimalIncidentUtility.TryFindAggressiveAnimalKind(points, map, out pawnKindDef))
			{
				incidentParms.pawnKind = pawnKindDef;
			}
			slate.Set<PawnKindDef>("animalKindDef", pawnKindDef, false);
			int num = (incidentParms.pawnCount > 0) ? incidentParms.pawnCount : AggressiveAnimalIncidentUtility.GetAnimalsCount(pawnKindDef, points);
			QuestGen.slate.Set<int>("animalCount", num, false);
			if (!this.customLetterLabel.GetValue(slate).NullOrEmpty() || this.customLetterLabelRules.GetValue(slate) != null)
			{
				QuestGen.AddTextRequest("root", delegate(string x)
				{
					incidentParms.customLetterLabel = x;
				}, QuestGenUtility.MergeRules(this.customLetterLabelRules.GetValue(slate), this.customLetterLabel.GetValue(slate), "root"));
			}
			if (!this.customLetterText.GetValue(slate).NullOrEmpty() || this.customLetterTextRules.GetValue(slate) != null)
			{
				QuestGen.AddTextRequest("root", delegate(string x)
				{
					incidentParms.customLetterText = x;
				}, QuestGenUtility.MergeRules(this.customLetterTextRules.GetValue(slate), this.customLetterText.GetValue(slate), "root"));
			}
			questPart_Incident.SetIncidentParmsAndRemoveTarget(incidentParms);
			questPart_Incident.inSignal = (QuestGenUtility.HardcodedSignalWithQuestID(this.inSignal.GetValue(slate)) ?? QuestGen.slate.Get<string>("inSignal", null, false));
			QuestGen.quest.AddPart(questPart_Incident);
			List<Rule> list = new List<Rule>();
			list.Add(new Rule_String("animalKind_label", pawnKindDef.label));
			list.Add(new Rule_String("animalKind_labelPlural", pawnKindDef.GetLabelPlural(num)));
			QuestGen.AddQuestDescriptionRules(list);
			QuestGen.AddQuestNameRules(list);
		}

		// Token: 0x0400E7E2 RID: 59362
		[NoTranslate]
		public SlateRef<string> inSignal;

		// Token: 0x0400E7E3 RID: 59363
		public SlateRef<string> customLetterLabel;

		// Token: 0x0400E7E4 RID: 59364
		public SlateRef<string> customLetterText;

		// Token: 0x0400E7E5 RID: 59365
		public SlateRef<RulePack> customLetterLabelRules;

		// Token: 0x0400E7E6 RID: 59366
		public SlateRef<RulePack> customLetterTextRules;

		// Token: 0x0400E7E7 RID: 59367
		public SlateRef<IntVec3?> walkInSpot;

		// Token: 0x0400E7E8 RID: 59368
		public SlateRef<int> animalCount;

		// Token: 0x0400E7E9 RID: 59369
		[NoTranslate]
		public SlateRef<string> tag;

		// Token: 0x0400E7EA RID: 59370
		private const string RootSymbol = "root";
	}
}
