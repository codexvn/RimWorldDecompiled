using System;
using RimWorld.Planet;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02003B0D RID: 15117
	public class QuestNode_Root_Gravcore_CrashedMechanoidPlatform : QuestNode_Root_Gravcore
	{
		// Token: 0x06014928 RID: 84264 RVA: 0x00622FA5 File Offset: 0x006211A5
		protected override bool TestRunInt(Slate slate)
		{
			return Faction.OfMechanoids != null && base.TestRunInt(slate);
		}

		// Token: 0x06014929 RID: 84265 RVA: 0x00622FB8 File Offset: 0x006211B8
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			Quest quest = QuestGen.quest;
			PlanetTile tile;
			if (!this.TryFindSiteTile(out tile))
			{
				Log.Error("Could not find valid site tile for crashed platform quest.");
				return;
			}
			string text = QuestGenUtility.HardcodedSignalWithQuestID("site.MapGenerated");
			string inSignal = QuestGenUtility.HardcodedSignalWithQuestID("site.MapRemoved");
			string inSignal2 = QuestGenUtility.HardcodedSignalWithQuestID("site.MapSettled");
			Site site = QuestGen_Sites.GenerateSite(new SitePartDefWithParams[]
			{
				new SitePartDefWithParams(SitePartDefOf.CrashedMechanoidPlatform, new SitePartParams
				{
					points = slate.Get<float>("points", 0f, false),
					threatPoints = slate.Get<float>("points", 0f, false)
				})
			}, tile, null, false, null, WorldObjectDefOf.ClaimableSite);
			slate.Set<Site>("site", site, false);
			quest.SpawnWorldObject(site, null, null);
			QuestPart_Choice.Choice choice = new QuestPart_Choice.Choice();
			choice.rewards.Add(new Reward_DefinedThingDef(ThingDefOf.Gravcore));
			choice.rewards.Add(new Reward_DefinedThingDef(ThingDefOf.GravlitePanel));
			quest.RewardChoice(null, null).choices.Add(choice);
			Quest quest2 = quest;
			LetterDef neutralEvent = LetterDefOf.NeutralEvent;
			string inSignal3 = text;
			string chosenPawnSignal = null;
			Faction relatedFaction = null;
			MapParent useColonistsOnMap = null;
			bool useColonistsFromCaravanArg = false;
			QuestPart.SignalListenMode signalListenMode = QuestPart.SignalListenMode.OngoingOnly;
			string label = "CrashedPlatformArrivedLetter".Translate();
			string text2 = "CrashedPlatformArrivedLetterText".Translate();
			quest2.Letter(neutralEvent, inSignal3, chosenPawnSignal, relatedFaction, useColonistsOnMap, useColonistsFromCaravanArg, signalListenMode, Gen.YieldSingle<Map>(site.Map), false, text2, null, label, null, null);
			quest.End(QuestEndOutcome.Success, 0, null, inSignal, QuestPart.SignalListenMode.OngoingOnly, false, false);
			quest.End(QuestEndOutcome.Unknown, 0, null, inSignal2, QuestPart.SignalListenMode.OngoingOnly, false, false);
		}
	}
}
