using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.Grammar;

namespace RimWorld.QuestGen
{
	// Token: 0x02003AFD RID: 15101
	public class QuestNode_SpawnMechCluster : QuestNode
	{
		// Token: 0x060148F7 RID: 84215 RVA: 0x00621E44 File Offset: 0x00620044
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			QuestPart_MechCluster questPart_MechCluster = new QuestPart_MechCluster();
			questPart_MechCluster.inSignal = (QuestGenUtility.HardcodedSignalWithQuestID(this.inSignal.GetValue(slate)) ?? QuestGen.slate.Get<string>("inSignal", null, false));
			questPart_MechCluster.tag = QuestGenUtility.HardcodedTargetQuestTagWithQuestID(this.tag.GetValue(slate));
			questPart_MechCluster.mapParent = slate.Get<Map>("map", null, false).Parent;
			questPart_MechCluster.sketch = this.GenerateSketch(slate);
			questPart_MechCluster.dropSpot = (this.dropSpot.GetValue(slate) ?? IntVec3.Invalid);
			QuestGen.quest.AddPart(questPart_MechCluster);
			string text = "";
			if (questPart_MechCluster.sketch.pawns != null)
			{
				text += PawnUtility.PawnKindsToLineList(from m in questPart_MechCluster.sketch.pawns
				select m.kindDef, "  - ", ColoredText.ThreatColor);
			}
			string[] array = (from t in questPart_MechCluster.sketch.buildingsSketch.Things
			where GenHostility.IsDefMechClusterThreat(t.def)
			group t by t.def.label).Select(delegate(IGrouping<string, SketchThing> grp)
			{
				int num = grp.Count<SketchThing>();
				return num.ToString() + " " + ((num > 1) ? Find.ActiveLanguageWorker.Pluralize(grp.Key, num) : grp.Key);
			}).ToArray<string>();
			if (array.Any<string>())
			{
				if (text != "")
				{
					text += "\n";
				}
				text += array.ToLineList(ColoredText.ThreatColor, "  - ");
			}
			if (text != "")
			{
				QuestGen.AddQuestDescriptionRules(new List<Rule>
				{
					new Rule_String("allThreats", text)
				});
			}
		}

		// Token: 0x060148F8 RID: 84216 RVA: 0x00622038 File Offset: 0x00620238
		private MechClusterSketch GenerateSketch(Slate slate)
		{
			return MechClusterGenerator.GenerateClusterSketch(this.points.GetValue(slate) ?? slate.Get<float>("points", 0f, false), slate.Get<Map>("map", null, false), true, false);
		}

		// Token: 0x060148F9 RID: 84217 RVA: 0x00622089 File Offset: 0x00620289
		protected override bool TestRunInt(Slate slate)
		{
			return Find.Storyteller.difficulty.allowViolentQuests && Faction.OfMechanoids != null && slate.Get<Map>("map", null, false) != null;
		}

		// Token: 0x0400E43B RID: 58427
		[NoTranslate]
		public SlateRef<string> inSignal;

		// Token: 0x0400E43C RID: 58428
		[NoTranslate]
		public SlateRef<string> tag;

		// Token: 0x0400E43D RID: 58429
		public SlateRef<float?> points;

		// Token: 0x0400E43E RID: 58430
		public SlateRef<IntVec3?> dropSpot;
	}
}
