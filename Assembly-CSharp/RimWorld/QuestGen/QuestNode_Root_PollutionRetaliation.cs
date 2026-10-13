using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02003B80 RID: 15232
	public class QuestNode_Root_PollutionRetaliation : QuestNode
	{
		// Token: 0x06014B15 RID: 84757 RVA: 0x0062FB6C File Offset: 0x0062DD6C
		protected override bool TestRunInt(Slate slate)
		{
			Faction faction;
			return QuestGen_Get.GetMap(false, null, true) != null && slate.TryGet<Faction>("enemyFaction", out faction, false) && QuestNode_Root_PollutionRetaliation.IsValidFaction(faction);
		}

		// Token: 0x06014B16 RID: 84758 RVA: 0x0062FBA4 File Offset: 0x0062DDA4
		protected override void RunInt()
		{
			Quest quest = QuestGen.quest;
			Slate slate = QuestGen.slate;
			Map map = slate.Get<Map>("map", null, false);
			float x = slate.Get<float>("points", 0f, false);
			string completeSignal = QuestGen.GenerateNewSignal("DelayCompleted", true);
			Pawn sender = this.GetSender(slate);
			int num = Mathf.RoundToInt(QuestNode_Root_PollutionRetaliation.WastepackCountOverPointsCurve.Evaluate(x));
			quest.Delay(QuestNode_Root_PollutionRetaliation.MaxDelayTicksRange.RandomInRange, delegate
			{
				quest.SignalPass(null, null, completeSignal);
			}, null, null, null, false, null, null, false, null, null, null, false, QuestPart.SignalListenMode.OngoingOnly, true).debugLabel = "Arrival delay";
			int num2 = Rand.Range(1, 4);
			int stackCount = num / num2;
			List<Thing> list = new List<Thing>();
			for (int i = 0; i < num2; i++)
			{
				Thing thing = ThingMaker.MakeThing(ThingDefOf.Wastepack, null);
				thing.stackCount = stackCount;
				list.Add(thing);
			}
			for (int j = 0; j < num2; j++)
			{
				List<Thing> contents = new List<Thing>
				{
					list[j]
				};
				quest.DropPods(map.Parent, contents, null, null, null, null, new bool?(false), false, false, false, completeSignal, null, QuestPart.SignalListenMode.OngoingOnly, null, true, true, false, null);
			}
			quest.Letter(LetterDefOf.NegativeEvent, completeSignal, null, null, null, false, QuestPart.SignalListenMode.OngoingOnly, list, false, "[retaliationLetterText]", null, "[retaliationLetterLabel]", null, null);
			quest.End(QuestEndOutcome.Unknown, 0, null, completeSignal, QuestPart.SignalListenMode.OngoingOnly, false, false);
			slate.Set<Pawn>("sender", sender, false);
			slate.Set<bool>("senderIsNull", sender == null, false);
			slate.Set<int>("wastepackCount", num, false);
		}

		// Token: 0x06014B17 RID: 84759 RVA: 0x0062FD68 File Offset: 0x0062DF68
		private Pawn GetSender(Slate slate)
		{
			Faction faction;
			if (slate.TryGet<Faction>("enemyFaction", out faction, false) && QuestNode_Root_PollutionRetaliation.IsValidFaction(faction))
			{
				return faction.leader;
			}
			if (Find.FactionManager.AllFactionsVisible.Where(new Func<Faction, bool>(QuestNode_Root_PollutionRetaliation.IsValidFaction)).TryRandomElement(out faction))
			{
				return faction.leader;
			}
			return null;
		}

		// Token: 0x06014B18 RID: 84760 RVA: 0x0062FDC0 File Offset: 0x0062DFC0
		private static bool IsValidFaction(Faction faction)
		{
			return !faction.IsPlayer && faction.HostileTo(Faction.OfPlayer) && faction.def.techLevel > TechLevel.Neolithic;
		}

		// Token: 0x0400E5EE RID: 58862
		private static readonly IntRange MaxDelayTicksRange = new IntRange(60000, 180000);

		// Token: 0x0400E5EF RID: 58863
		private static readonly SimpleCurve WastepackCountOverPointsCurve = new SimpleCurve
		{
			{
				new CurvePoint(200f, 40f),
				true
			},
			{
				new CurvePoint(400f, 80f),
				true
			},
			{
				new CurvePoint(800f, 120f),
				true
			},
			{
				new CurvePoint(1600f, 160f),
				true
			},
			{
				new CurvePoint(3200f, 200f),
				true
			},
			{
				new CurvePoint(20000f, 500f),
				true
			}
		};
	}
}
