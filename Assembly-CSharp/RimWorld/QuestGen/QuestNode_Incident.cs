using System;
using Verse;
using Verse.Grammar;

namespace RimWorld.QuestGen
{
	// Token: 0x02003BF1 RID: 15345
	public class QuestNode_Incident : QuestNode
	{
		// Token: 0x06014D19 RID: 85273 RVA: 0x00620959 File Offset: 0x0061EB59
		protected override bool TestRunInt(Slate slate)
		{
			return slate.Exists("map", false);
		}

		// Token: 0x06014D1A RID: 85274 RVA: 0x0063C634 File Offset: 0x0063A834
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			Map target = QuestGen.slate.Get<Map>("map", null, false);
			float points = QuestGen.slate.Get<float>("points", 0f, false);
			QuestPart_Incident questPart_Incident = new QuestPart_Incident();
			questPart_Incident.incident = this.incidentDef.GetValue(slate);
			IncidentParms incidentParms = new IncidentParms();
			incidentParms.forced = true;
			incidentParms.target = target;
			incidentParms.points = points;
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
		}

		// Token: 0x0400E7D1 RID: 59345
		[NoTranslate]
		public SlateRef<string> inSignal;

		// Token: 0x0400E7D2 RID: 59346
		public SlateRef<IncidentDef> incidentDef;

		// Token: 0x0400E7D3 RID: 59347
		public SlateRef<string> customLetterLabel;

		// Token: 0x0400E7D4 RID: 59348
		public SlateRef<string> customLetterText;

		// Token: 0x0400E7D5 RID: 59349
		public SlateRef<RulePack> customLetterLabelRules;

		// Token: 0x0400E7D6 RID: 59350
		public SlateRef<RulePack> customLetterTextRules;

		// Token: 0x0400E7D7 RID: 59351
		private const string RootSymbol = "root";
	}
}
