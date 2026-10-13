using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02003B43 RID: 15171
	public class QuestNode_Root_DistressCall : QuestNode
	{
		// Token: 0x17003363 RID: 13155
		// (get) Token: 0x06014A00 RID: 84480 RVA: 0x00628A80 File Offset: 0x00626C80
		protected static List<LandmarkDef> AllowedLandmarks
		{
			get
			{
				if (ModsConfig.OdysseyActive && QuestNode_Root_DistressCall.allowedLandmarksCached == null)
				{
					QuestNode_Root_DistressCall.allowedLandmarksCached = new List<LandmarkDef>
					{
						LandmarkDefOf.Oasis,
						LandmarkDefOf.Lake,
						LandmarkDefOf.LakeWithIsland,
						LandmarkDefOf.LakeWithIslands,
						LandmarkDefOf.Pond,
						LandmarkDefOf.DryLake,
						LandmarkDefOf.ToxicLake,
						LandmarkDefOf.Wetland,
						LandmarkDefOf.HotSprings,
						LandmarkDefOf.CoastalIsland,
						LandmarkDefOf.Peninsula,
						LandmarkDefOf.Valley,
						LandmarkDefOf.Cavern,
						LandmarkDefOf.Chasm,
						LandmarkDefOf.Cliffs,
						LandmarkDefOf.Hollow,
						LandmarkDefOf.TerraformingScar,
						LandmarkDefOf.Dunes
					};
				}
				return QuestNode_Root_DistressCall.allowedLandmarksCached;
			}
		}

		// Token: 0x06014A01 RID: 84481 RVA: 0x00628B78 File Offset: 0x00626D78
		protected override bool TestRunInt(Slate slate)
		{
			if (!Find.Storyteller.difficulty.allowViolentQuests)
			{
				return false;
			}
			QuestGenUtility.TestRunAdjustPointsForDistantFight(slate);
			float num = slate.Get<float>("points", 0f, false);
			PlanetTile planetTile;
			Faction faction;
			return num >= 100f && this.TryFindSiteTile(out planetTile, true) && this.TryFindFaction(num, out faction);
		}

		// Token: 0x06014A02 RID: 84482 RVA: 0x00628BD0 File Offset: 0x00626DD0
		protected override void RunInt()
		{
			Quest quest = QuestGen.quest;
			Slate slate = QuestGen.slate;
			float num = slate.Get<float>("points", 0f, false);
			if (num < 100f)
			{
				num = 100f;
			}
			PlanetTile tile;
			this.TryFindSiteTile(out tile, false);
			Faction faction;
			this.TryFindFaction(num, out faction);
			slate.Set<Faction>("faction", faction, false);
			IEnumerable<SitePartDef> source = from def in DefDatabase<SitePartDef>.AllDefs
			where def.tags != null && def.tags.Contains("DistressCall") && typeof(SitePartWorker_DistressCall).IsAssignableFrom(def.workerClass)
			select def;
			SitePartDefWithParams[] array = new SitePartDefWithParams[1];
			array[0] = new SitePartDefWithParams(source.RandomElementByWeight((SitePartDef sp) => sp.selectionWeight), new SitePartParams
			{
				threatPoints = num
			});
			Site site = QuestGen_Sites.GenerateSite(array, tile, faction, false, null, null);
			quest.SpawnWorldObject(site, null, null);
			slate.Set<Site>("site", site, false);
			string inSignalEnable = QuestGenUtility.HardcodedSignalWithQuestID("site.MapGenerated");
			string text = QuestGenUtility.HardcodedSignalWithQuestID("site.AllEnemiesDefeated");
			string inSignal = QuestGenUtility.HardcodedSignalWithQuestID("site.MapRemoved");
			string signalAmbush = QuestGenUtility.HardcodedSignalWithQuestID("ambush");
			Quest quest3 = quest;
			LetterDef neutralEvent = LetterDefOf.NeutralEvent;
			string inSignal2 = null;
			string chosenPawnSignal = null;
			string label = "DistressSignalLabel".Translate();
			string text2 = "DistressSignalText".Translate(site.Faction.Named("FACTION")).Resolve();
			IEnumerable<object> lookTargets = Gen.YieldSingle<Site>(site);
			quest3.Letter(neutralEvent, inSignal2, chosenPawnSignal, site.Faction, null, false, QuestPart.SignalListenMode.OngoingOnly, lookTargets, false, text2, null, label, null, null);
			QuestPart_Choice questPart_Choice = quest.RewardChoice(null, null);
			QuestPart_Choice.Choice choice = new QuestPart_Choice.Choice();
			choice.rewards.Add(new Reward_CampLoot());
			questPart_Choice.choices.Add(choice);
			if (Rand.Chance(0.75f))
			{
				quest.Delay(QuestNode_Root_DistressCall.AmbushDelayTicks.RandomInRange, delegate
				{
					quest.SignalPass(null, null, signalAmbush);
				}, inSignalEnable, null, null, false, null, null, false, null, null, null, false, QuestPart.SignalListenMode.OngoingOnly, false);
				quest.AddPart(new QuestPart_DistressCallAmbush(signalAmbush, site, QuestNode_Root_DistressCall.AmbushPointsCurve.Evaluate(num)));
			}
			quest.WorldObjectTimeout(site, 900000, null, null, false, null, true);
			quest.Delay(900000, delegate
			{
				quest.End(QuestEndOutcome.Fail, 0, null, null, QuestPart.SignalListenMode.OngoingOnly, false, false);
			}, null, null, null, false, null, null, false, null, null, null, false, QuestPart.SignalListenMode.OngoingOnly, false);
			Quest quest2 = quest;
			Map getRaidersFromMap = null;
			text2 = text;
			quest2.Notify_PlayerRaidedSomeone(getRaidersFromMap, site, text2);
			quest.End(QuestEndOutcome.Success, 0, null, inSignal, QuestPart.SignalListenMode.OngoingOnly, false, false);
		}

		// Token: 0x06014A03 RID: 84483 RVA: 0x00628E68 File Offset: 0x00627068
		private bool TryFindFaction(float points, out Faction faction)
		{
			return (from f in Find.FactionManager.AllFactionsListForReading
			where this.FactionUsable(f, points)
			select f).TryRandomElement(out faction);
		}

		// Token: 0x06014A04 RID: 84484 RVA: 0x00628EAC File Offset: 0x006270AC
		private bool FactionUsable(Faction f, float points)
		{
			return (!ModsConfig.RoyaltyActive || points >= 2000f || f != Faction.OfEmpire) && f.def.canGenerateQuestSites && (f.def.humanlikeFaction && !f.def.pawnGroupMakers.NullOrEmpty<PawnGroupMaker>()) && !f.def.permanentEnemy;
		}

		// Token: 0x06014A05 RID: 84485 RVA: 0x00628F10 File Offset: 0x00627110
		private bool TryFindSiteTile(out PlanetTile tile, bool exitOnFirstTileFound = false)
		{
			return TileFinder.TryFindNewSiteTile(out tile, 3, 9, false, QuestNode_Root_DistressCall.AllowedLandmarks, 0.5f, true, TileFinderMode.Near, exitOnFirstTileFound, false, null, null);
		}

		// Token: 0x0400E504 RID: 58628
		private static List<LandmarkDef> allowedLandmarksCached;

		// Token: 0x0400E505 RID: 58629
		private const string SitePartTag = "DistressCall";

		// Token: 0x0400E506 RID: 58630
		private const int MaxDistanceFromColony = 9;

		// Token: 0x0400E507 RID: 58631
		private const int MinDistanceFromColony = 3;

		// Token: 0x0400E508 RID: 58632
		private const float MinPoints = 100f;

		// Token: 0x0400E509 RID: 58633
		private const int TimeoutTicks = 900000;

		// Token: 0x0400E50A RID: 58634
		private const float EmpireSitePointsThreshold = 2000f;

		// Token: 0x0400E50B RID: 58635
		private const float AmbushChance = 0.75f;

		// Token: 0x0400E50C RID: 58636
		private static readonly IntRange AmbushDelayTicks = new IntRange(2400, 4800);

		// Token: 0x0400E50D RID: 58637
		private static readonly SimpleCurve AmbushPointsCurve = new SimpleCurve
		{
			{
				new CurvePoint(100f, 100f),
				true
			},
			{
				new CurvePoint(1000f, 400f),
				true
			},
			{
				new CurvePoint(5000f, 1000f),
				true
			}
		};
	}
}
