using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.Grammar;

namespace RimWorld.QuestGen
{
	// Token: 0x02003B72 RID: 15218
	public abstract class QuestNode_Root_MysteriousCargo : QuestNode
	{
		// Token: 0x1700336D RID: 13165
		// (get) Token: 0x06014AE3 RID: 84707 RVA: 0x0062EF48 File Offset: 0x0062D148
		protected virtual bool RequiresPawn { get; } = 1;

		// Token: 0x06014AE4 RID: 84708 RVA: 0x0062EF50 File Offset: 0x0062D150
		protected override bool TestRunInt(Slate slate)
		{
			Map map = QuestGen_Get.GetMap(false, null, true);
			Pawn pawn;
			return map != null && (!this.RequiresPawn || QuestUtility.TryGetIdealColonist(out pawn, map, new Func<Pawn, bool>(this.ValidatePawn)));
		}

		// Token: 0x06014AE5 RID: 84709 RVA: 0x0062EF94 File Offset: 0x0062D194
		protected override void RunInt()
		{
			Quest quest = QuestGen.quest;
			Slate slate = QuestGen.slate;
			Map map = QuestGen_Get.GetMap(false, null, true);
			float points = slate.Get<float>("points", 0f, false);
			slate.Set<Map>("map", map, false);
			Pawn asker = this.FindAsker();
			Pawn pawn = null;
			if (this.RequiresPawn && !QuestUtility.TryGetIdealColonist(out pawn, map, new Func<Pawn, bool>(this.ValidatePawn)))
			{
				Log.ErrorOnce("Attempted to create a mysterious cargo quest but no valid pawns or world pawns could be found", 94657346);
				quest.End(QuestEndOutcome.InvalidPreAcceptance, true, true);
			}
			Thing thing = this.GenerateThing(pawn);
			quest.Delay(120, delegate
			{
				quest.DropPods(map.Parent, new List<Thing>
				{
					thing
				}, "[deliveredLetterLabel]", null, "[deliveredLetterText]", null, new bool?(true), false, false, false, null, null, QuestPart.SignalListenMode.OngoingOnly, null, true, true, false, null);
				this.AddPostDroppedQuestParts(pawn, thing, quest);
			}, null, null, null, false, null, null, false, null, null, null, false, QuestPart.SignalListenMode.OngoingOnly, false);
			quest.Delay(Mathf.RoundToInt(QuestNode_Root_MysteriousCargo.DelayHours.RandomInRange * 2500f), delegate
			{
				Quest quest = quest;
				RewardsGeneratorParams parms = new RewardsGeneratorParams
				{
					rewardValue = QuestNode_Root_MysteriousCargo.RewardValueCurve.Evaluate(points),
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
			}, null, null, null, false, null, null, false, null, null, null, false, QuestPart.SignalListenMode.OngoingOnly, false);
			slate.Set<Pawn>("asker", asker, false);
			slate.Set<bool>("askerIsNull", asker == null, false);
			slate.Set<Pawn>("pawn", pawn, false);
			Slate slate2 = slate;
			string name = "pawnOnMap";
			Pawn pawn2 = pawn;
			slate2.Set<bool>(name, ((pawn2 != null) ? pawn2.MapHeld : null) == map, false);
		}

		// Token: 0x06014AE6 RID: 84710
		protected abstract Thing GenerateThing(Pawn pawn);

		// Token: 0x06014AE7 RID: 84711 RVA: 0x000026EA File Offset: 0x000008EA
		protected virtual void AddPostDroppedQuestParts(Pawn pawn, Thing thing, Quest quest)
		{
		}

		// Token: 0x06014AE8 RID: 84712 RVA: 0x0062F12B File Offset: 0x0062D32B
		protected virtual bool ValidatePawn(Pawn pawn)
		{
			return pawn.IsColonist || pawn.IsSlaveOfColony;
		}

		// Token: 0x06014AE9 RID: 84713 RVA: 0x0062F140 File Offset: 0x0062D340
		private Pawn FindAsker()
		{
			Faction faction;
			if ((from f in Find.FactionManager.AllFactionsVisible
			where f.def.humanlikeFaction && !f.IsPlayer && !f.HostileTo(Faction.OfPlayer) && f.def.techLevel > TechLevel.Neolithic && f.leader != null && !f.temporary && !f.Hidden
			select f).TryRandomElement(out faction))
			{
				return faction.leader;
			}
			return null;
		}

		// Token: 0x0400E5CF RID: 58831
		private static readonly FloatRange DelayHours = new FloatRange(1f, 3f);

		// Token: 0x0400E5D0 RID: 58832
		private static readonly SimpleCurve RewardValueCurve = new SimpleCurve
		{
			{
				new CurvePoint(500f, 800f),
				true
			},
			{
				new CurvePoint(3000f, 1200f),
				true
			}
		};
	}
}
