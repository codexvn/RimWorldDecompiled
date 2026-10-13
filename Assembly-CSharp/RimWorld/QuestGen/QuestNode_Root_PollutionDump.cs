using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.Grammar;

namespace RimWorld.QuestGen
{
	// Token: 0x02003B7B RID: 15227
	public class QuestNode_Root_PollutionDump : QuestNode
	{
		// Token: 0x06014B04 RID: 84740 RVA: 0x0062F640 File Offset: 0x0062D840
		protected override bool TestRunInt(Slate slate)
		{
			return QuestGen_Get.GetMap(false, null, true) != null && this.FindAsker() != null;
		}

		// Token: 0x06014B05 RID: 84741 RVA: 0x0062F66C File Offset: 0x0062D86C
		protected override void RunInt()
		{
			Quest quest = QuestGen.quest;
			Slate slate = QuestGen.slate;
			Map map = QuestGen_Get.GetMap(false, null, true);
			slate.Set<Map>("map", map, false);
			float x = slate.Get<float>("points", 0f, false);
			Pawn asker = this.FindAsker();
			int wastepackCount = Mathf.RoundToInt(QuestNode_Root_PollutionDump.WastepackCountOverPointsCurve.Evaluate(x));
			quest.Delay(2500, delegate
			{
				List<Thing> list = new List<Thing>();
				Thing thing;
				for (int i = wastepackCount; i > 0; i -= thing.stackCount)
				{
					thing = ThingMaker.MakeThing(ThingDefOf.Wastepack, null);
					thing.stackCount = Mathf.Min(i, thing.def.stackLimit);
					list.Add(thing);
				}
				Quest quest;
				quest.DropPods(map.Parent, list, "[wastepacksLetterLabel]", null, "[wastepacksLetterText]", null, new bool?(true), false, false, false, null, null, QuestPart.SignalListenMode.OngoingOnly, null, true, true, false, null);
				QuestScriptDefOf.Util_GetDefaultRewardValueFromPoints.Run();
				float rewardValue = slate.Get<float>("rewardValue", 0f, false);
				quest = quest;
				RewardsGeneratorParams parms = new RewardsGeneratorParams
				{
					rewardValue = rewardValue,
					thingRewardItemsOnly = true
				};
				string inSignal = null;
				string customLetterLabel = null;
				string customLetterText = null;
				RulePack customLetterLabelRules = null;
				RulePack customLetterTextRules = null;
				Pawn asker = asker;
				quest.GiveRewards(parms, inSignal, customLetterLabel, customLetterText, customLetterLabelRules, customLetterTextRules, null, null, null, false, asker, false, false, null);
				quest.End(QuestEndOutcome.Success, 0, null, null, QuestPart.SignalListenMode.OngoingOnly, true, false);
			}, null, null, null, false, null, null, false, null, null, null, false, QuestPart.SignalListenMode.OngoingOnly, true);
			slate.Set<Pawn>("asker", asker, false);
			slate.Set<bool>("askerIsNull", asker == null, false);
			slate.Set<int>("wastepackCount", wastepackCount, false);
		}

		// Token: 0x06014B06 RID: 84742 RVA: 0x0062F770 File Offset: 0x0062D970
		private Pawn FindAsker()
		{
			Faction faction;
			if ((from f in Find.FactionManager.AllFactionsVisible
			where f.def.humanlikeFaction && !f.IsPlayer && !f.HostileTo(Faction.OfPlayer) && f.def.techLevel > TechLevel.Neolithic && f.leader != null && !f.temporary && !f.Hidden && f.leader.Faction == f
			select f).TryRandomElement(out faction))
			{
				return faction.leader;
			}
			return null;
		}

		// Token: 0x0400E5E1 RID: 58849
		private const int DropPodsDelayTicks = 2500;

		// Token: 0x0400E5E2 RID: 58850
		private static readonly SimpleCurve WastepackCountOverPointsCurve = new SimpleCurve
		{
			{
				new CurvePoint(200f, 90f),
				true
			},
			{
				new CurvePoint(400f, 150f),
				true
			},
			{
				new CurvePoint(800f, 225f),
				true
			},
			{
				new CurvePoint(1600f, 325f),
				true
			},
			{
				new CurvePoint(3200f, 400f),
				true
			},
			{
				new CurvePoint(20000f, 1000f),
				true
			}
		};
	}
}
