using System;
using RimWorld.Planet;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02003B16 RID: 15126
	public class QuestNode_Root_Gravcore_Mechhive : QuestNode_Root_Gravcore
	{
		// Token: 0x0601494A RID: 84298 RVA: 0x00622FA5 File Offset: 0x006211A5
		protected override bool TestRunInt(Slate slate)
		{
			return Faction.OfMechanoids != null && base.TestRunInt(slate);
		}

		// Token: 0x0601494B RID: 84299 RVA: 0x00623E18 File Offset: 0x00622018
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			Quest quest = QuestGen.quest;
			PlanetTile tile;
			if (!this.TryFindSiteTile(out tile))
			{
				Log.Error("Could not find valid site tile for mechhive quest.");
				return;
			}
			string text = QuestGenUtility.HardcodedTargetQuestTagWithQuestID("core");
			string stabilizerTag = QuestGenUtility.HardcodedTargetQuestTagWithQuestID("stabilizer");
			string text2 = QuestGenUtility.HardcodedSignalWithQuestID("site.MapGenerated");
			string inSignal = QuestGenUtility.HardcodedSignalWithQuestID("site.MapRemoved");
			string text3 = QuestGenUtility.HardcodedSignalWithQuestID(text + ".CoreDefeated");
			Site site = QuestGen_Sites.GenerateSite(new SitePartDefWithParams[]
			{
				new SitePartDefWithParams(SitePartDefOf.OrbitalMechhive, new SitePartParams
				{
					points = slate.Get<float>("points", 0f, false),
					threatPoints = slate.Get<float>("points", 0f, false)
				})
			}, tile, null, false, null, WorldObjectDefOf.Mechhive);
			slate.Set<Site>("site", site, false);
			quest.SpawnWorldObject(site, null, null);
			QuestPart_Choice.Choice choice = new QuestPart_Choice.Choice();
			choice.rewards.Add(new Reward_Unknown());
			quest.RewardChoice(null, null).choices.Add(choice);
			Quest quest2 = quest;
			LetterDef neutralEvent = LetterDefOf.NeutralEvent;
			string inSignal2 = text2;
			string chosenPawnSignal = null;
			Faction relatedFaction = null;
			MapParent useColonistsOnMap = null;
			bool useColonistsFromCaravanArg = false;
			QuestPart.SignalListenMode signalListenMode = QuestPart.SignalListenMode.OngoingOnly;
			string label = "OrbitalMechhiveLetterArrived".Translate();
			string text4 = "OrbitalMechhiveLetterArrivedText".Translate();
			quest2.Letter(neutralEvent, inSignal2, chosenPawnSignal, relatedFaction, useColonistsOnMap, useColonistsFromCaravanArg, signalListenMode, Gen.YieldSingle<Map>(site.Map), false, text4, null, label, null, null);
			quest.AddPart(new QuestPart_CerebrexCore(site, text2, text, stabilizerTag));
			quest.AddPart(new QuestPart_StabilizersRemainingAlert(site, text2, text3));
			quest.End(QuestEndOutcome.Success, 0, null, text3, QuestPart.SignalListenMode.OngoingOnly, false, false);
			quest.End(QuestEndOutcome.Fail, 0, null, inSignal, QuestPart.SignalListenMode.OngoingOnly, false, false);
			quest.AddPart(new QuestPart_QuestEndParent
			{
				inSignal = text3,
				outcome = new QuestEndOutcome?(QuestEndOutcome.Success),
				sendLetter = true,
				signalListenMode = QuestPart.SignalListenMode.Always
			});
		}
	}
}
