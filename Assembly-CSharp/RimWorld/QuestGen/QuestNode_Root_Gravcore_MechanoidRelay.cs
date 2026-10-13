using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02003B10 RID: 15120
	public class QuestNode_Root_Gravcore_MechanoidRelay : QuestNode_Root_Gravcore
	{
		// Token: 0x06014930 RID: 84272 RVA: 0x00622FA5 File Offset: 0x006211A5
		protected override bool TestRunInt(Slate slate)
		{
			return Faction.OfMechanoids != null && base.TestRunInt(slate);
		}

		// Token: 0x06014931 RID: 84273 RVA: 0x006233C4 File Offset: 0x006215C4
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			Quest quest = QuestGen.quest;
			PlanetTile tile;
			if (!this.TryFindSiteTile(out tile))
			{
				Log.Error("Could not find valid site tile for mechanoid relay quest.");
				return;
			}
			string questTagToAdd = QuestGenUtility.HardcodedTargetQuestTagWithQuestID("MechanoidRelay");
			string text = QuestGenUtility.HardcodedSignalWithQuestID("site.MapGenerated");
			string inSignal = QuestGenUtility.HardcodedSignalWithQuestID("site.MapRemoved");
			string inSignal2 = QuestGenUtility.HardcodedSignalWithQuestID("site.MapSettled");
			string text2 = QuestGen.GenerateNewSignal("StabilizerDealtWith", true);
			string text3 = QuestGen.GenerateNewSignal("AllStabilizersDealtWith", true);
			Site site = QuestGen_Sites.GenerateSite(new SitePartDefWithParams[]
			{
				new SitePartDefWithParams(SitePartDefOf.MechanoidRelay, new SitePartParams
				{
					points = slate.Get<float>("points", 0f, false),
					threatPoints = slate.Get<float>("points", 0f, false),
					stabilizerCount = 3
				})
			}, tile, Faction.OfMechanoids, false, null, WorldObjectDefOf.ClaimableSite);
			slate.Set<Site>("site", site, false);
			quest.SpawnWorldObject(site, null, null);
			List<Thing> list = new List<Thing>();
			List<string> list2 = new List<string>();
			for (int i = 0; i < site.parts[0].things.Count; i++)
			{
				Thing thing = site.parts[0].things[i];
				QuestUtility.AddQuestTag(ref thing.questTags, questTagToAdd);
				if (thing.def == ThingDefOf.MechStabilizer)
				{
					this.AddStabilizer(thing, site, quest, slate, text, list, list2);
				}
				else if (thing.def == ThingDefOf.MechRelay)
				{
					slate.Set<Thing>("relay", thing, false);
				}
			}
			quest.SignalPassAny(null, list2, text2);
			QuestPart_Filter_AllThingsHackedOrDestroyed questPart_Filter_AllThingsHackedOrDestroyed = new QuestPart_Filter_AllThingsHackedOrDestroyed();
			questPart_Filter_AllThingsHackedOrDestroyed.things.AddRange(list);
			questPart_Filter_AllThingsHackedOrDestroyed.inSignal = text2;
			questPart_Filter_AllThingsHackedOrDestroyed.outSignal = text3;
			quest.AddPart(questPart_Filter_AllThingsHackedOrDestroyed);
			quest.AddPart(new QuestPart_MechRelay
			{
				inSignal = text3,
				relay = slate.Get<Thing>("relay", null, false)
			});
			quest.Delay(10000, delegate
			{
				quest.RandomRaid(site, FloatRange.One, Faction.OfMechanoids, null, PawnsArrivalModeDefOf.RandomDrop, RaidStrategyDefOf.ImmediateAttack, "GravcoreMechRelayRaidLetterLabel".Translate(), "GravcoreMechRelayRaidLetterText".Translate(), true);
			}, text3, null, null, false, null, null, false, null, null, null, false, QuestPart.SignalListenMode.OngoingOnly, false);
			quest.Letter(LetterDefOf.NeutralEvent, text, null, null, null, false, QuestPart.SignalListenMode.OngoingOnly, null, false, "[letterTextMapGenerated]", null, "[letterLabelMapGenerated]", null, null);
			QuestPart_RelayStabilizersRemainingAlert questPart_RelayStabilizersRemainingAlert = new QuestPart_RelayStabilizersRemainingAlert(site, text, text3, text2, list.Count);
			questPart_RelayStabilizersRemainingAlert.things.AddRange(list);
			quest.AddPart(questPart_RelayStabilizersRemainingAlert);
			QuestPart_Choice.Choice choice = new QuestPart_Choice.Choice();
			choice.rewards.Add(new Reward_DefinedThingDef(ThingDefOf.Gravcore));
			choice.rewards.Add(new Reward_DefinedThingDef(ThingDefOf.GravlitePanel));
			quest.RewardChoice(null, null).choices.Add(choice);
			quest.End(QuestEndOutcome.Success, 0, null, inSignal, QuestPart.SignalListenMode.OngoingOnly, false, false);
			quest.End(QuestEndOutcome.Unknown, 0, null, inSignal2, QuestPart.SignalListenMode.OngoingOnly, false, false);
		}

		// Token: 0x06014932 RID: 84274 RVA: 0x006236EC File Offset: 0x006218EC
		private void AddStabilizer(Thing thing, Site site, Quest quest, Slate slate, string mapGeneratedSignal, List<Thing> stabilizers, List<string> signalsDealtWith)
		{
			string text = string.Format("{0}{1}", "stabilizer", stabilizers.Count);
			slate.Set<Thing>(text, thing, false);
			string text2 = QuestGenUtility.HardcodedSignalWithQuestID(text + ".Destroyed");
			string text3 = QuestGenUtility.HardcodedSignalWithQuestID(text + ".Hacked");
			string inSignalTookDamage = QuestGenUtility.HardcodedSignalWithQuestID(text + ".TookDamage");
			string inSignalLockedOut = QuestGenUtility.HardcodedSignalWithQuestID(text + ".LockedOut");
			signalsDealtWith.Add(text2);
			signalsDealtWith.Add(text3);
			QuestPart_Filter_Hacked questPart_Filter_Hacked = new QuestPart_Filter_Hacked();
			questPart_Filter_Hacked.inSignal = text2;
			questPart_Filter_Hacked.outSignalElse = QuestGen.GenerateNewSignal("SendRaidStabilizerDestroyed", true);
			quest.AddPart(questPart_Filter_Hacked);
			float points = slate.Get<float>("points", 0f, false);
			QuestPart_SleepingMechs part = new QuestPart_SleepingMechs
			{
				inSignal = mapGeneratedSignal,
				inSignalTookDamage = inSignalTookDamage,
				inSignalLockedOut = inSignalLockedOut,
				defendThing = thing,
				mapParent = site,
				points = points
			};
			quest.AddPart(part);
			quest.Message("MessageStabilizerDeactivated".Translate(), MessageTypeDefOf.PositiveEvent, true, null, null, text3);
			quest.RandomRaid(site, FloatRange.One, Faction.OfMechanoids, questPart_Filter_Hacked.outSignalElse, PawnsArrivalModeDefOf.RandomDrop, RaidStrategyDefOf.ImmediateAttack, "GravcoreMechRelayRaidLetterLabel".Translate(), "GravcoreMechRelayRaidLetterText".Translate(), true);
			stabilizers.Add(thing);
		}

		// Token: 0x0400E478 RID: 58488
		private const string StabilizerAlias = "stabilizer";

		// Token: 0x0400E479 RID: 58489
		private const string RelayAlias = "relay";

		// Token: 0x0400E47A RID: 58490
		private const int StablizerCount = 3;
	}
}
