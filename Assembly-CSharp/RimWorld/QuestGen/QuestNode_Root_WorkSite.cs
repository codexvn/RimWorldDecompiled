using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using UnityEngine;
using Verse;
using Verse.Grammar;

namespace RimWorld.QuestGen
{
	// Token: 0x02003BC8 RID: 15304
	public class QuestNode_Root_WorkSite : QuestNode
	{
		// Token: 0x06014C87 RID: 85127 RVA: 0x00639C20 File Offset: 0x00637E20
		private static bool AnySpawnCandidate(PlanetTile aroundTile)
		{
			foreach (QuestNode_Root_WorkSite.SiteSpawnCandidate siteSpawnCandidate in QuestNode_Root_WorkSite.GetCandidates(aroundTile))
			{
				if (Find.WorldGrid[siteSpawnCandidate.tile].PrimaryBiome.campSelectionWeight > 0f)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06014C88 RID: 85128 RVA: 0x00639C90 File Offset: 0x00637E90
		private static QuestNode_Root_WorkSite.SiteSpawnCandidate GetSpawnCandidate(PlanetTile aroundTile)
		{
			List<QuestNode_Root_WorkSite.SiteSpawnCandidate> list = QuestNode_Root_WorkSite.GetCandidates(aroundTile).ToList<QuestNode_Root_WorkSite.SiteSpawnCandidate>();
			if (list.Count == 0)
			{
				return default(QuestNode_Root_WorkSite.SiteSpawnCandidate);
			}
			SitePartDef campTypeToGenerate = (from tct in list
			select tct.sitePart).Distinct<SitePartDef>().RandomElement<SitePartDef>();
			return (from tct in list
			where tct.sitePart == campTypeToGenerate
			select tct).RandomElementByWeightWithFallback((QuestNode_Root_WorkSite.SiteSpawnCandidate t) => Find.WorldGrid[t.tile].PrimaryBiome.campSelectionWeight, default(QuestNode_Root_WorkSite.SiteSpawnCandidate));
		}

		// Token: 0x06014C89 RID: 85129 RVA: 0x00639D38 File Offset: 0x00637F38
		private static IEnumerable<QuestNode_Root_WorkSite.SiteSpawnCandidate> GetCandidates(PlanetTile aroundTile)
		{
			IEnumerable<SitePartDef> source = from def in DefDatabase<SitePartDef>.AllDefs
			where def.tags != null && def.tags.Contains("WorkSite") && typeof(SitePartWorker_WorkSite).IsAssignableFrom(def.workerClass)
			select def;
			List<PlanetTile> potentialTiles = QuestNode_Root_WorkSite.PotentialSiteTiles(aroundTile);
			return source.SelectMany(delegate(SitePartDef sitePart)
			{
				SitePartWorker_WorkSite worker = (SitePartWorker_WorkSite)sitePart.Worker;
				return from tile in potentialTiles
				where worker.CanSpawnOn(tile)
				select tile into t
				select new QuestNode_Root_WorkSite.SiteSpawnCandidate
				{
					tile = t,
					sitePart = sitePart
				};
			});
		}

		// Token: 0x06014C8A RID: 85130 RVA: 0x00639D94 File Offset: 0x00637F94
		public static Site GenerateSite(float points, PlanetTile aroundTile)
		{
			QuestNode_Root_WorkSite.<>c__DisplayClass16_0 CS$<>8__locals1 = new QuestNode_Root_WorkSite.<>c__DisplayClass16_0();
			QuestNode_Root_WorkSite.SiteSpawnCandidate spawnCandidate = QuestNode_Root_WorkSite.GetSpawnCandidate(aroundTile);
			CS$<>8__locals1.sitePartWorker = (SitePartWorker_WorkSite)spawnCandidate.sitePart.Worker;
			Faction faction = null;
			QuestNode_Root_WorkSite.FactionType item = QuestNode_Root_WorkSite.FactionChances.RandomElementByWeight((Tuple<float, QuestNode_Root_WorkSite.FactionType> e) => e.Item1).Item2;
			QuestPart_Hyperlinks questPart_Hyperlinks = new QuestPart_Hyperlinks();
			QuestGen.quest.AddPart(questPart_Hyperlinks);
			CS$<>8__locals1.factionValidator = null;
			if (item != QuestNode_Root_WorkSite.FactionType.Enemy)
			{
				if (item == QuestNode_Root_WorkSite.FactionType.AllyOrNeutral)
				{
					CS$<>8__locals1.factionValidator = ((Faction f) => f.AllyOrNeutralTo(Faction.OfPlayer));
				}
			}
			else
			{
				CS$<>8__locals1.factionValidator = ((Faction f) => f.HostileTo(Faction.OfPlayer));
			}
			if (CS$<>8__locals1.factionValidator != null)
			{
				faction = (from f in Find.FactionManager.AllFactionsListForReading
				where base.<GenerateSite>g__FactionUseable|2(f) && CS$<>8__locals1.factionValidator(f)
				select f).RandomElementWithFallback(null);
			}
			if (faction != null)
			{
				questPart_Hyperlinks.factions.Add(faction);
			}
			else
			{
				FactionDef factionDef = DefDatabase<FactionDef>.AllDefsListForReading.Where(new Func<FactionDef, bool>(CS$<>8__locals1.<GenerateSite>g__FactionDefUseable|1)).RandomElement<FactionDef>();
				bool? hidden = new bool?(true);
				FactionGeneratorParms factionGeneratorParms = new FactionGeneratorParms(factionDef, default(IdeoGenerationParms), hidden);
				factionGeneratorParms.ideoGenerationParms = new IdeoGenerationParms(factionGeneratorParms.factionDef, false, CS$<>8__locals1.sitePartWorker.DisallowedPrecepts.ToList<PreceptDef>(), null, null, false, false, false, false, "", null, null, false, "", false);
				List<FactionRelation> list = new List<FactionRelation>();
				foreach (Faction faction2 in Find.FactionManager.AllFactionsListForReading)
				{
					if (!faction2.def.PermanentlyHostileTo(factionGeneratorParms.factionDef))
					{
						if (faction2 == Faction.OfPlayer)
						{
							list.Add(new FactionRelation
							{
								other = faction2,
								kind = FactionRelationKind.Hostile
							});
						}
						else
						{
							list.Add(new FactionRelation
							{
								other = faction2,
								kind = FactionRelationKind.Neutral
							});
						}
					}
				}
				faction = FactionGenerator.NewGeneratedFactionWithRelations(factionGeneratorParms, list);
				faction.temporary = true;
				Find.FactionManager.Add(faction);
			}
			Site site = QuestGen_Sites.GenerateSite(new SitePartDefWithParams[]
			{
				new SitePartDefWithParams(spawnCandidate.sitePart, new SitePartParams
				{
					threatPoints = points
				})
			}, spawnCandidate.tile, faction, false, null, null);
			if (site.parts.Any<SitePart>() && site.parts[0].lootThings.Any<ThingDefCount>())
			{
				questPart_Hyperlinks.thingDefs.Add(site.parts[0].lootThings[0].ThingDef);
			}
			site.desiredThreatPoints = site.ActualThreatPoints;
			return site;
		}

		// Token: 0x06014C8B RID: 85131 RVA: 0x0063A068 File Offset: 0x00638268
		public static List<PlanetTile> PotentialSiteTiles(PlanetTile root)
		{
			List<PlanetTile> tiles = new List<PlanetTile>();
			root.Layer.Filler.FloodFill(root, (PlanetTile p) => !Find.World.Impassable(p) && Find.WorldGrid.ApproxDistanceInTiles(p, root) <= 9f, delegate(PlanetTile p)
			{
				if (Find.WorldGrid.ApproxDistanceInTiles(p, root) >= 3f)
				{
					WorldLandmarks landmarks = Find.World.landmarks;
					if (((landmarks != null) ? landmarks[p] : null) == null)
					{
						tiles.Add(p);
					}
				}
			}, int.MaxValue, null);
			return tiles;
		}

		// Token: 0x06014C8C RID: 85132 RVA: 0x0063A0CC File Offset: 0x006382CC
		public static float AppearanceFrequency(Map map)
		{
			float num = 1f;
			float num2 = 0f;
			List<PlanetTile> list = QuestNode_Root_WorkSite.PotentialSiteTiles(map.Tile);
			if (list.Count == 0)
			{
				return 0f;
			}
			if (!QuestNode_Root_WorkSite.AnySpawnCandidate(map.Tile))
			{
				return 0f;
			}
			foreach (PlanetTile tile in list)
			{
				num2 += Find.WorldGrid[tile].PrimaryBiome.campSelectionWeight;
			}
			num2 /= (float)list.Count;
			num *= num2;
			int num3 = 0;
			foreach (Site site in Find.WorldObjects.Sites)
			{
				if (site.MainSitePartDef.tags != null && site.MainSitePartDef.tags.Contains("WorkSite"))
				{
					num3++;
				}
			}
			num *= QuestNode_Root_WorkSite.ExistingCampsAppearanceFrequencyMultiplier.Evaluate((float)num3);
			int num4 = map.mapPawns.FreeColonists.Count<Pawn>();
			if (num4 <= 1)
			{
				return 0f;
			}
			if (num4 == 2)
			{
				return num / 2f;
			}
			return num;
		}

		// Token: 0x06014C8D RID: 85133 RVA: 0x0063A21C File Offset: 0x0063841C
		public static float BestAppearanceFrequency()
		{
			float num = 0f;
			foreach (Map map in Find.Maps)
			{
				if (map.IsPlayerHome)
				{
					num = Mathf.Max(num, QuestNode_Root_WorkSite.AppearanceFrequency(map));
				}
			}
			return num;
		}

		// Token: 0x06014C8E RID: 85134 RVA: 0x0063A284 File Offset: 0x00638484
		protected override void RunInt()
		{
			if (!ModLister.CheckIdeology("Work site"))
			{
				return;
			}
			Quest quest = QuestGen.quest;
			Slate slate = QuestGen.slate;
			QuestGenUtility.RunAdjustPointsForDistantFight();
			float num = slate.Get<float>("points", 0f, false);
			if (num < 40f)
			{
				num = 40f;
			}
			Map map = (from m in Find.Maps
			where m.IsPlayerHome && QuestNode_Root_WorkSite.GetCandidates(m.Tile).Any<QuestNode_Root_WorkSite.SiteSpawnCandidate>()
			select m).RandomElementByWeight((Map m) => QuestNode_Root_WorkSite.AppearanceFrequency(m));
			slate.Set<Map>("map", map, false);
			Site site = QuestNode_Root_WorkSite.GenerateSite(num, map.Tile);
			quest.SpawnWorldObject(site, null, null);
			quest.ReserveFaction(site.Faction);
			int num2 = 1800000;
			quest.WorldObjectTimeout(site, num2, null, null, false, null, true);
			quest.Delay(num2, delegate
			{
				quest.End(QuestEndOutcome.Fail, 0, null, null, QuestPart.SignalListenMode.OngoingOnly, false, false);
			}, null, null, null, false, null, null, false, null, null, null, false, QuestPart.SignalListenMode.OngoingOnly, false);
			quest.Message("MessageCampDetected".Translate(site.Named("CAMP"), site.Faction.Named("FACTION")), MessageTypeDefOf.NeutralEvent, false, null, new LookTargets(site), null);
			SitePart sitePart = site.parts[0];
			if (!sitePart.things.NullOrEmpty<Thing>())
			{
				ThingDef def = sitePart.things.First<Thing>().def;
				int num3 = 0;
				foreach (Thing thing in ((IEnumerable<Thing>)sitePart.things))
				{
					if (thing.def == def)
					{
						num3 += thing.stackCount;
					}
				}
				QuestGen.AddQuestDescriptionRules(new List<Rule>
				{
					new Rule_String("loot", def.label + " x" + num3.ToString())
				});
			}
			slate.Set<Site>("campSite", site, false);
			slate.Set<Faction>("faction", site.Faction, false);
			slate.Set<int>("timeout", num2, false);
			string inSignal = QuestGenUtility.HardcodedSignalWithQuestID("campSite.AllEnemiesDefeated");
			string inSignalEnabled = QuestGenUtility.HardcodedSignalWithQuestID("campSite.MapGenerated");
			string inSignal2 = QuestGenUtility.HardcodedSignalWithQuestID("campSite.MapRemoved");
			QuestPart_Choice questPart_Choice = quest.RewardChoice(null, null);
			QuestPart_Choice.Choice choice = new QuestPart_Choice.Choice();
			choice.rewards.Add(new Reward_CampLoot());
			questPart_Choice.choices.Add(choice);
			if (num >= 400f)
			{
				quest.SurpriseReinforcements(inSignalEnabled, site, site.Faction, 0.35f);
			}
			quest.Notify_PlayerRaidedSomeone(null, site, inSignal);
			quest.End(QuestEndOutcome.Success, 0, null, inSignal2, QuestPart.SignalListenMode.OngoingOnly, false, false);
			QuestGen.AddQuestDescriptionRules(new List<Rule>
			{
				new Rule_String("siteLabel", site.Label)
			});
		}

		// Token: 0x06014C8F RID: 85135 RVA: 0x0063A5A0 File Offset: 0x006387A0
		protected override bool TestRunInt(Slate slate)
		{
			if (!Find.Storyteller.difficulty.allowViolentQuests)
			{
				return false;
			}
			QuestGenUtility.TestRunAdjustPointsForDistantFight(slate);
			if (slate.Get<float>("points", 0f, false) < 40f)
			{
				return false;
			}
			foreach (Map map in Find.Maps)
			{
				if (map.IsPlayerHome && QuestNode_Root_WorkSite.AppearanceFrequency(map) > 0f && QuestNode_Root_WorkSite.GetCandidates(map.Tile).Any<QuestNode_Root_WorkSite.SiteSpawnCandidate>())
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0400E74F RID: 59215
		private const int SpawnRange = 9;

		// Token: 0x0400E750 RID: 59216
		private const int MinSpawnDist = 3;

		// Token: 0x0400E751 RID: 59217
		private const float MinPointsForSurpriseReinforcements = 400f;

		// Token: 0x0400E752 RID: 59218
		private const float SurpriseReinforcementChance = 0.35f;

		// Token: 0x0400E753 RID: 59219
		private static readonly SimpleCurve ExistingCampsAppearanceFrequencyMultiplier = new SimpleCurve
		{
			{
				new CurvePoint(0f, 1f),
				true
			},
			{
				new CurvePoint(2f, 1f),
				true
			},
			{
				new CurvePoint(3f, 0.3f),
				true
			},
			{
				new CurvePoint(4f, 0.1f),
				true
			}
		};

		// Token: 0x0400E754 RID: 59220
		private const float MinPoints = 40f;

		// Token: 0x0400E755 RID: 59221
		private const string SitePartTag = "WorkSite";

		// Token: 0x0400E756 RID: 59222
		private const float TemporaryFactionChance = 0.6f;

		// Token: 0x0400E757 RID: 59223
		private const float EnemyFactionChance = 0.25f;

		// Token: 0x0400E758 RID: 59224
		private const float AllyFactionChance = 0.15f;

		// Token: 0x0400E759 RID: 59225
		private static readonly Tuple<float, QuestNode_Root_WorkSite.FactionType>[] FactionChances = new Tuple<float, QuestNode_Root_WorkSite.FactionType>[]
		{
			new Tuple<float, QuestNode_Root_WorkSite.FactionType>(0.6f, QuestNode_Root_WorkSite.FactionType.Temporary),
			new Tuple<float, QuestNode_Root_WorkSite.FactionType>(0.25f, QuestNode_Root_WorkSite.FactionType.Enemy),
			new Tuple<float, QuestNode_Root_WorkSite.FactionType>(0.15f, QuestNode_Root_WorkSite.FactionType.AllyOrNeutral)
		};

		// Token: 0x02003BC9 RID: 15305
		private struct SiteSpawnCandidate
		{
			// Token: 0x0400E75A RID: 59226
			public PlanetTile tile;

			// Token: 0x0400E75B RID: 59227
			public SitePartDef sitePart;
		}

		// Token: 0x02003BCA RID: 15306
		private enum FactionType
		{
			// Token: 0x0400E75D RID: 59229
			Temporary,
			// Token: 0x0400E75E RID: 59230
			Enemy,
			// Token: 0x0400E75F RID: 59231
			AllyOrNeutral
		}
	}
}
