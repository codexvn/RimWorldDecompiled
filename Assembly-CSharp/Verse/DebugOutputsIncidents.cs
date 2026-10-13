using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using LudeonTK;
using RimWorld;

namespace Verse
{
	// Token: 0x02000AA9 RID: 2729
	public static class DebugOutputsIncidents
	{
		// Token: 0x06004567 RID: 17767 RVA: 0x00164B0C File Offset: 0x00162D0C
		[DebugOutput("Incidents", false)]
		public static void IncidentChances()
		{
			List<StorytellerComp> storytellerComps = Find.Storyteller.storytellerComps;
			for (int i = 0; i < storytellerComps.Count; i++)
			{
				StorytellerComp_CategoryMTB storytellerComp_CategoryMTB = storytellerComps[i] as StorytellerComp_CategoryMTB;
				if (storytellerComp_CategoryMTB != null && ((StorytellerCompProperties_CategoryMTB)storytellerComp_CategoryMTB.props).category == IncidentCategoryDefOf.Misc)
				{
					storytellerComp_CategoryMTB.DebugTablesIncidentChances();
				}
			}
		}

		// Token: 0x06004568 RID: 17768 RVA: 0x00164B64 File Offset: 0x00162D64
		[DebugOutput("Incidents", false)]
		public static void IncidentChancesSampled()
		{
			Dictionary<IncidentDef, int> samples = new Dictionary<IncidentDef, int>();
			int fireCount = 0;
			List<StorytellerComp> storytellerComps = Find.Storyteller.storytellerComps;
			for (int i = 0; i < storytellerComps.Count; i++)
			{
				StorytellerComp storytellerComp = storytellerComps[i];
				if (storytellerComp != null)
				{
					for (int j = 0; j < 50000; j++)
					{
						foreach (FiringIncident firingIncident in storytellerComp.MakeIntervalIncidents(Find.AnyPlayerHomeMap))
						{
							int num;
							if (samples.TryGetValue(firingIncident.def, out num))
							{
								samples[firingIncident.def] = num + 1;
							}
							else
							{
								samples.Add(firingIncident.def, 1);
							}
							int fireCount2 = fireCount;
							fireCount = fireCount2 + 1;
						}
					}
				}
			}
			IEnumerable<IncidentDef> keys = samples.Keys;
			TableDataGetter<IncidentDef>[] array = new TableDataGetter<IncidentDef>[4];
			array[0] = new TableDataGetter<IncidentDef>("defName", (IncidentDef d) => d.defName);
			array[1] = new TableDataGetter<IncidentDef>("category", (IncidentDef d) => d.category);
			array[2] = new TableDataGetter<IncidentDef>("amount fired", (IncidentDef d) => samples[d]);
			array[3] = new TableDataGetter<IncidentDef>("fire chance", (IncidentDef d) => ((float)samples[d] / (float)fireCount).ToString("0.0000"));
			DebugTables.MakeTablesDialog<IncidentDef>(keys, array);
		}

		// Token: 0x06004569 RID: 17769 RVA: 0x00164D0C File Offset: 0x00162F0C
		[DebugOutput("Incidents", true)]
		public static void FutureIncidents()
		{
			StorytellerUtility.ShowFutureIncidentsDebugLogFloatMenu(false);
		}

		// Token: 0x0600456A RID: 17770 RVA: 0x00164D14 File Offset: 0x00162F14
		[DebugOutput("Incidents", true)]
		public static void FutureIncidentsCurrentMap()
		{
			StorytellerUtility.ShowFutureIncidentsDebugLogFloatMenu(true);
		}

		// Token: 0x0600456B RID: 17771 RVA: 0x00164D1C File Offset: 0x00162F1C
		[DebugOutput("Incidents", true)]
		public static void IncidentTargetsList()
		{
			StorytellerUtility.DebugLogTestIncidentTargets();
		}

		// Token: 0x0600456C RID: 17772 RVA: 0x00164D24 File Offset: 0x00162F24
		[DebugOutput("Incidents", false)]
		public static void MinThreatPoints()
		{
			int ticksGame = Find.TickManager.TicksGame;
			IEnumerable<int> dataSources = DebugOutputsIncidents.<MinThreatPoints>g__hourOffsets|5_0();
			TableDataGetter<int>[] array = new TableDataGetter<int>[5];
			array[0] = new TableDataGetter<int>("hours passed", (int h) => h);
			array[1] = new TableDataGetter<int>("days passed", (int h) => h / 24);
			array[2] = new TableDataGetter<int>("points min", (int h) => StorytellerUtility.GlobalPointsMin());
			array[3] = new TableDataGetter<int>("points min ceiling", (int h) => Find.Storyteller.difficulty.MinThreatPointsCeiling);
			array[4] = new TableDataGetter<int>("points min floor", (int h) => 35f);
			DebugTables.MakeTablesDialog<int>(dataSources, array);
			Find.TickManager.DebugSetTicksGame(ticksGame);
		}

		// Token: 0x0600456D RID: 17773 RVA: 0x00164E34 File Offset: 0x00163034
		[DebugOutput("Incidents", false)]
		public static void CurrentThreatPoints()
		{
			Map currentMap = Find.CurrentMap;
			if (currentMap != null)
			{
				currentMap.wealthWatcher.ForceRecount(false);
				Log.Message(StorytellerUtility.DefaultThreatPointsNow(currentMap).ToString("F2"));
			}
		}

		// Token: 0x0600456E RID: 17774 RVA: 0x00164E70 File Offset: 0x00163070
		[DebugOutput("Incidents", false)]
		public static void PawnArrivalCandidates()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine(IncidentDefOf.RaidEnemy.defName);
			stringBuilder.AppendLine(((IncidentWorker_PawnsArrive)IncidentDefOf.RaidEnemy.Worker).DebugListingOfGroupSources());
			stringBuilder.AppendLine(IncidentDefOf.RaidFriendly.defName);
			stringBuilder.AppendLine(((IncidentWorker_PawnsArrive)IncidentDefOf.RaidFriendly.Worker).DebugListingOfGroupSources());
			stringBuilder.AppendLine(IncidentDefOf.VisitorGroup.defName);
			stringBuilder.AppendLine(((IncidentWorker_PawnsArrive)IncidentDefOf.VisitorGroup.Worker).DebugListingOfGroupSources());
			stringBuilder.AppendLine(IncidentDefOf.TravelerGroup.defName);
			stringBuilder.AppendLine(((IncidentWorker_PawnsArrive)IncidentDefOf.TravelerGroup.Worker).DebugListingOfGroupSources());
			stringBuilder.AppendLine(IncidentDefOf.TraderCaravanArrival.defName);
			stringBuilder.AppendLine(((IncidentWorker_PawnsArrive)IncidentDefOf.TraderCaravanArrival.Worker).DebugListingOfGroupSources());
			Log.Message(stringBuilder.ToString());
		}

		// Token: 0x0600456F RID: 17775 RVA: 0x00164F68 File Offset: 0x00163168
		[DebugOutput("Incidents", false)]
		public static void TraderKinds()
		{
			IEnumerable<TraderKindDef> allDefs = DefDatabase<TraderKindDef>.AllDefs;
			TableDataGetter<TraderKindDef>[] array = new TableDataGetter<TraderKindDef>[8];
			array[0] = new TableDataGetter<TraderKindDef>("defName", (TraderKindDef d) => d.defName);
			array[1] = new TableDataGetter<TraderKindDef>("orbital", (TraderKindDef d) => d.orbital.ToStringCheckBlank());
			array[2] = new TableDataGetter<TraderKindDef>("requestable", (TraderKindDef d) => d.requestable.ToStringCheckBlank());
			array[3] = new TableDataGetter<TraderKindDef>("commonality\nbase", (TraderKindDef d) => d.commonality.ToString("F2"));
			array[4] = new TableDataGetter<TraderKindDef>("commonality\nnow", (TraderKindDef d) => d.CalculatedCommonality.ToString("F2"));
			array[5] = new TableDataGetter<TraderKindDef>("faction", delegate(TraderKindDef d)
			{
				if (d.faction == null)
				{
					return "";
				}
				return d.faction.defName;
			});
			array[6] = new TableDataGetter<TraderKindDef>("permit\nrequired", delegate(TraderKindDef d)
			{
				if (d.permitRequiredForTrading == null)
				{
					return "";
				}
				return d.permitRequiredForTrading.defName;
			});
			array[7] = new TableDataGetter<TraderKindDef>("average\nvalue", (TraderKindDef d) => ((ThingSetMaker_TraderStock)ThingSetMakerDefOf.TraderStock.root).DebugAverageTotalStockValue(d).ToString("F0"));
			DebugTables.MakeTablesDialog<TraderKindDef>(allDefs, array);
		}

		// Token: 0x06004570 RID: 17776 RVA: 0x001650E8 File Offset: 0x001632E8
		[DebugOutput("Incidents", false)]
		public static void TraderKindThings()
		{
			List<TableDataGetter<ThingDef>> list = new List<TableDataGetter<ThingDef>>();
			list.Add(new TableDataGetter<ThingDef>("defName", (ThingDef d) => d.defName));
			foreach (TraderKindDef localTk2 in DefDatabase<TraderKindDef>.AllDefs)
			{
				TraderKindDef localTk = localTk2;
				list.Add(new TableDataGetter<ThingDef>(localTk.defName.Replace("_", "\n").Shorten(), (ThingDef td) => DebugOutputsIncidents.<TraderKindThings>g__TraderTradeability|9_1(localTk, td)));
			}
			DebugTables.MakeTablesDialog<ThingDef>((from d in DefDatabase<ThingDef>.AllDefs
			where (d.category == ThingCategory.Item && d.BaseMarketValue > 0.001f && !d.isUnfinishedThing && !d.IsCorpse && d.PlayerAcquirable && d != ThingDefOf.Silver && !d.thingCategories.NullOrEmpty<ThingCategoryDef>()) || (d.category == ThingCategory.Building && d.Minifiable) || d.category == ThingCategory.Pawn
			select d).OrderBy(delegate(ThingDef d)
			{
				if (d.thingCategories.NullOrEmpty<ThingCategoryDef>())
				{
					return "zzzzzzz";
				}
				return d.thingCategories[0].defName;
			}).ThenBy((ThingDef d) => d.BaseMarketValue), list.ToArray());
		}

		// Token: 0x06004571 RID: 17777 RVA: 0x00165224 File Offset: 0x00163424
		[DebugOutput("Incidents", false)]
		public static void TraderStockMarketValues()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (TraderKindDef traderKindDef in DefDatabase<TraderKindDef>.AllDefs)
			{
				stringBuilder.AppendLine(traderKindDef.defName + " : " + ((ThingSetMaker_TraderStock)ThingSetMakerDefOf.TraderStock.root).DebugAverageTotalStockValue(traderKindDef).ToString("F0"));
			}
			Log.Message(stringBuilder.ToString());
		}

		// Token: 0x06004572 RID: 17778 RVA: 0x001652B4 File Offset: 0x001634B4
		[DebugOutput("Incidents", false)]
		public static void TraderStockGeneration()
		{
			List<FloatMenuOption> list = new List<FloatMenuOption>();
			foreach (TraderKindDef localDef2 in DefDatabase<TraderKindDef>.AllDefs)
			{
				TraderKindDef localDef = localDef2;
				FloatMenuOption item = new FloatMenuOption(localDef.defName, delegate()
				{
					Log.Message(((ThingSetMaker_TraderStock)ThingSetMakerDefOf.TraderStock.root).DebugGenerationDataFor(localDef));
				}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
				list.Add(item);
			}
			Find.WindowStack.Add(new FloatMenu(list));
		}

		// Token: 0x06004573 RID: 17779 RVA: 0x00165350 File Offset: 0x00163550
		[DebugOutput("Incidents", false)]
		public static void TraderStockGeneratorsDefs()
		{
			if (Find.CurrentMap == null)
			{
				Log.Error("Requires visible map.");
				return;
			}
			StringBuilder sb = new StringBuilder();
			Action<StockGenerator> action = delegate(StockGenerator gen)
			{
				StockGenerator_Tag stockGenerator_Tag = gen as StockGenerator_Tag;
				StringBuilder sb;
				if (stockGenerator_Tag != null && !stockGenerator_Tag.tradeTag.NullOrEmpty())
				{
					sb = sb;
					Type type = gen.GetType();
					sb.AppendLine(((type != null) ? type.ToString() : null) + " (" + stockGenerator_Tag.tradeTag + ")");
				}
				else
				{
					sb.AppendLine(gen.GetType().ToString());
				}
				sb.AppendLine("ALLOWED DEFS:");
				foreach (ThingDef thingDef in DefDatabase<ThingDef>.AllDefs.Where(new Func<ThingDef, bool>(gen.HandlesThingDef)))
				{
					sb.AppendLine(thingDef.defName + " [" + thingDef.BaseMarketValue.ToString() + "]");
				}
				sb.AppendLine();
				sb.AppendLine("GENERATION TEST:");
				gen.countRange = IntRange.One;
				for (int i = 0; i < 30; i++)
				{
					foreach (Thing thing in gen.GenerateThings(Find.CurrentMap.Tile, null))
					{
						sb.AppendLine(thing.Label + " [" + thing.MarketValue.ToString() + "]");
					}
				}
				sb.AppendLine("---------------------------------------------------------");
			};
			action(new StockGenerator_MarketValue
			{
				tradeTag = "Armor"
			});
			action(new StockGenerator_MarketValue
			{
				tradeTag = "WeaponRanged"
			});
			action(new StockGenerator_MarketValue
			{
				tradeTag = "WeaponMelee"
			});
			action(new StockGenerator_MarketValue
			{
				tradeTag = "BasicClothing"
			});
			action(new StockGenerator_MarketValue
			{
				tradeTag = "Clothing"
			});
			action(new StockGenerator_MarketValue
			{
				tradeTag = "Art"
			});
			Log.Message(sb.ToString());
		}

		// Token: 0x06004574 RID: 17780 RVA: 0x00165420 File Offset: 0x00163620
		[DebugOutput("Incidents", false)]
		public static void TraderAnimalTags()
		{
			TableDataGetter<TraderKindDef>[] getters = (from tag in DefDatabase<TraderKindDef>.AllDefs.SelectMany((TraderKindDef k) => (from g in k.stockGenerators
			where g is StockGenerator_Animals
			select g).Cast<StockGenerator_Animals>().SelectMany((StockGenerator_Animals g) => g.AllRelevantTradeTags)).Distinct<string>()
			select new TableDataGetter<TraderKindDef>(tag, delegate(TraderKindDef def)
			{
				StockGenerator_Animals stockGenerator_Animals = (from g in def.stockGenerators
				where g is StockGenerator_Animals
				select g).Cast<StockGenerator_Animals>().FirstOrDefault<StockGenerator_Animals>();
				return ((stockGenerator_Animals != null) ? stockGenerator_Animals.GetInterestInTradeTag(tag) : null) ?? "-";
			})).Prepend(new TableDataGetter<TraderKindDef>("name", (TraderKindDef def) => def.defName)).ToArray<TableDataGetter<TraderKindDef>>();
			DebugTables.MakeTablesDialog<TraderKindDef>(DefDatabase<TraderKindDef>.AllDefs, getters);
		}

		// Token: 0x06004575 RID: 17781 RVA: 0x001654C0 File Offset: 0x001636C0
		[DebugOutput("Incidents", false)]
		public static void PawnGroupGenSampled()
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			foreach (Faction faction in Find.FactionManager.AllFactions)
			{
				if (faction.def.pawnGroupMakers != null)
				{
					if (faction.def.pawnGroupMakers.Any((PawnGroupMaker x) => x.kindDef == PawnGroupKindDefOf.Combat))
					{
						Faction localFac = faction;
						list.Add(new DebugMenuOption(localFac.Name + " (" + localFac.def.defName + ")", DebugMenuOptionMode.Action, delegate()
						{
							List<DebugMenuOption> list2 = new List<DebugMenuOption>();
							foreach (float localP2 in DebugActionsUtility.PointsOptions(true))
							{
								float localP = localP2;
								float maxPawnCost = PawnGroupMakerUtility.MaxPawnCost(localFac, localP, null, PawnGroupKindDefOf.Combat);
								string defName = (from op in localFac.def.pawnGroupMakers.SelectMany((PawnGroupMaker gm) => gm.options)
								where op.Cost <= maxPawnCost
								select op).MaxBy((PawnGenOption op) => op.Cost).kind.defName;
								string label = string.Concat(new string[]
								{
									localP.ToString(),
									", max ",
									maxPawnCost.ToString("F0"),
									" ",
									defName
								});
								list2.Add(new DebugMenuOption(label, DebugMenuOptionMode.Action, delegate()
								{
									Dictionary<ThingDef, int>[] weaponsCount = new Dictionary<ThingDef, int>[20];
									string[] pawnKinds = new string[20];
									for (int i = 0; i < 20; i++)
									{
										weaponsCount[i] = new Dictionary<ThingDef, int>();
										List<Pawn> list3 = PawnGroupMakerUtility.GeneratePawns(new PawnGroupMakerParms
										{
											groupKind = PawnGroupKindDefOf.Combat,
											tile = Find.CurrentMap.Tile,
											points = localP,
											faction = localFac
										}, false).ToList<Pawn>();
										pawnKinds[i] = PawnUtility.PawnKindsToCommaList(list3, true);
										foreach (Pawn pawn in list3)
										{
											if (pawn.equipment.Primary != null)
											{
												if (!weaponsCount[i].ContainsKey(pawn.equipment.Primary.def))
												{
													weaponsCount[i].Add(pawn.equipment.Primary.def, 0);
												}
												Dictionary<ThingDef, int> dictionary = weaponsCount[i];
												ThingDef def = pawn.equipment.Primary.def;
												int num = dictionary[def];
												dictionary[def] = num + 1;
											}
											pawn.Destroy(DestroyMode.Vanish);
										}
									}
									int totalPawns = weaponsCount.Sum((Dictionary<ThingDef, int> x) => x.Sum((KeyValuePair<ThingDef, int> y) => y.Value));
									List<TableDataGetter<int>> list4 = new List<TableDataGetter<int>>();
									list4.Add(new TableDataGetter<int>("", delegate(int x)
									{
										if (x != 20)
										{
											return (x + 1).ToString();
										}
										return "avg";
									}));
									list4.Add(new TableDataGetter<int>("pawns", delegate(int x)
									{
										string str = " ";
										string str2;
										if (x != 20)
										{
											str2 = weaponsCount[x].Sum((KeyValuePair<ThingDef, int> y) => y.Value).ToString();
										}
										else
										{
											str2 = ((float)totalPawns / 20f).ToString("0.#");
										}
										return str + str2;
									}));
									list4.Add(new TableDataGetter<int>("kinds", delegate(int x)
									{
										if (x != 20)
										{
											return pawnKinds[x];
										}
										return "";
									}));
									list4.AddRange((from x in DefDatabase<ThingDef>.AllDefs
									where x.IsWeapon && !x.weaponTags.NullOrEmpty<string>() && weaponsCount.Any((Dictionary<ThingDef, int> wc) => wc.ContainsKey(x))
									orderby x.IsMeleeWeapon descending, x.techLevel, x.BaseMarketValue
									select x).Select(delegate(ThingDef x)
									{
										Func<Dictionary<ThingDef, int>, int> <>9__19;
										return new TableDataGetter<int>(x.label.Shorten(), delegate(int y)
										{
											IEnumerable<Dictionary<ThingDef, int>> weaponsCount;
											if (y == 20)
											{
												string str = " ";
												weaponsCount = weaponsCount;
												Func<Dictionary<ThingDef, int>, int> selector;
												if ((selector = <>9__19) == null)
												{
													selector = (<>9__19 = delegate(Dictionary<ThingDef, int> z)
													{
														if (!z.ContainsKey(x))
														{
															return 0;
														}
														return z[x];
													});
												}
												return str + ((float)weaponsCount.Sum(selector) / 20f).ToString("0.#");
											}
											if (!weaponsCount[y].ContainsKey(x))
											{
												return "";
											}
											string[] array = new string[5];
											array[0] = " ";
											array[1] = weaponsCount[y][x].ToString();
											array[2] = " (";
											array[3] = ((float)weaponsCount[y][x] / (float)weaponsCount[y].Sum((KeyValuePair<ThingDef, int> z) => z.Value)).ToStringPercent("F0");
											array[4] = ")";
											return string.Concat(array);
										});
									}));
									DebugTables.MakeTablesDialog<int>(Enumerable.Range(0, 21), list4.ToArray());
								}));
							}
							Find.WindowStack.Add(new Dialog_DebugOptionListLister(list2, null));
						}));
					}
				}
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list, null));
		}

		// Token: 0x06004576 RID: 17782 RVA: 0x001655B4 File Offset: 0x001637B4
		[DebugOutput("Incidents", false)]
		public static void RaidFactionSampled()
		{
			((IncidentWorker_Raid)IncidentDefOf.RaidEnemy.Worker).DoTable_RaidFactionSampled();
		}

		// Token: 0x06004577 RID: 17783 RVA: 0x001655CC File Offset: 0x001637CC
		[DebugOutput("Incidents", false)]
		public static void RaidStrategySampled()
		{
			List<FloatMenuOption> list = new List<FloatMenuOption>();
			list.Add(new FloatMenuOption("Choose factions randomly like a real raid", delegate()
			{
				((IncidentWorker_Raid)IncidentDefOf.RaidEnemy.Worker).DoTable_RaidStrategySampled(null);
			}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
			using (IEnumerator<Faction> enumerator = Find.FactionManager.AllFactions.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Faction f = enumerator.Current;
					Faction f2 = f;
					list.Add(new FloatMenuOption(f2.Name + " (" + f2.def.defName + ")", delegate()
					{
						((IncidentWorker_Raid)IncidentDefOf.RaidEnemy.Worker).DoTable_RaidStrategySampled(f);
					}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
				}
			}
			Find.WindowStack.Add(new FloatMenu(list));
		}

		// Token: 0x06004578 RID: 17784 RVA: 0x001656BC File Offset: 0x001638BC
		[DebugOutput("Incidents", false)]
		public static void RaidArrivemodeSampled()
		{
			List<FloatMenuOption> list = new List<FloatMenuOption>();
			list.Add(new FloatMenuOption("Choose factions randomly like a real raid", delegate()
			{
				((IncidentWorker_Raid)IncidentDefOf.RaidEnemy.Worker).DoTable_RaidArrivalModeSampled(null);
			}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
			using (IEnumerator<Faction> enumerator = Find.FactionManager.AllFactions.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Faction f = enumerator.Current;
					Faction f2 = f;
					list.Add(new FloatMenuOption(f2.Name + " (" + f2.def.defName + ")", delegate()
					{
						((IncidentWorker_Raid)IncidentDefOf.RaidEnemy.Worker).DoTable_RaidArrivalModeSampled(f);
					}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
				}
			}
			Find.WindowStack.Add(new FloatMenu(list));
		}

		// Token: 0x06004579 RID: 17785 RVA: 0x001657AC File Offset: 0x001639AC
		[DebugOutput("Incidents", false)]
		public static void ThreatsGenerator()
		{
			StorytellerUtility.DebugLogTestFutureIncidents(new ThreatsGeneratorParams
			{
				allowedThreats = AllowedThreatsGeneratorThreats.All,
				randSeed = Rand.Int,
				onDays = 1f,
				offDays = 0.5f,
				minSpacingDays = 0.04f,
				numIncidentsRange = new FloatRange(1f, 2f)
			});
		}

		// Token: 0x0600457A RID: 17786 RVA: 0x0016580C File Offset: 0x00163A0C
		[DebugOutput("Incidents", false)]
		private static void RaidsInfoSampled()
		{
			if (Find.CurrentMap == null)
			{
				return;
			}
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			foreach (float localP2 in DebugActionsUtility.PointsOptions(true))
			{
				float localP = localP2;
				list.Add(new DebugMenuOption(localP.ToString() + " points", DebugMenuOptionMode.Action, delegate()
				{
					int ticksGame = Find.TickManager.TicksGame;
					Find.TickManager.DebugSetTicksGame(36000000);
					Faction lastRaidFaction = Find.CurrentMap.StoryState.lastRaidFaction;
					List<Tuple<IncidentParms, List<Pawn>>> list2 = new List<Tuple<IncidentParms, List<Pawn>>>();
					for (int i = 0; i < 100; i++)
					{
						IncidentParms incidentParms = new IncidentParms();
						incidentParms.target = Find.CurrentMap;
						incidentParms.points = localP;
						List<Pawn> item;
						if (((IncidentWorker_RaidEnemy)IncidentDefOf.RaidEnemy.Worker).TryGenerateRaidInfo(incidentParms, out item, true))
						{
							list2.Add(new Tuple<IncidentParms, List<Pawn>>(incidentParms, item));
						}
					}
					IEnumerable<Tuple<IncidentParms, List<Pawn>>> dataSources = list2;
					TableDataGetter<Tuple<IncidentParms, List<Pawn>>>[] array = new TableDataGetter<Tuple<IncidentParms, List<Pawn>>>[8];
					array[0] = new TableDataGetter<Tuple<IncidentParms, List<Pawn>>>("faction def", (Tuple<IncidentParms, List<Pawn>> t) => t.Item1.faction.def.defName);
					array[1] = new TableDataGetter<Tuple<IncidentParms, List<Pawn>>>("faction name", (Tuple<IncidentParms, List<Pawn>> t) => t.Item1.faction.Name);
					array[2] = new TableDataGetter<Tuple<IncidentParms, List<Pawn>>>("arrival mode", (Tuple<IncidentParms, List<Pawn>> t) => t.Item1.raidArrivalMode.defName);
					array[3] = new TableDataGetter<Tuple<IncidentParms, List<Pawn>>>("strategy", (Tuple<IncidentParms, List<Pawn>> t) => t.Item1.raidStrategy.defName);
					array[4] = new TableDataGetter<Tuple<IncidentParms, List<Pawn>>>("age restriction", delegate(Tuple<IncidentParms, List<Pawn>> t)
					{
						RaidAgeRestrictionDef raidAgeRestriction = t.Item1.raidAgeRestriction;
						if (raidAgeRestriction == null)
						{
							return null;
						}
						return raidAgeRestriction.defName;
					});
					array[5] = new TableDataGetter<Tuple<IncidentParms, List<Pawn>>>("points", (Tuple<IncidentParms, List<Pawn>> t) => t.Item1.points);
					array[6] = new TableDataGetter<Tuple<IncidentParms, List<Pawn>>>("pawn points\ntotal", (Tuple<IncidentParms, List<Pawn>> t) => t.Item2.Sum((Pawn x) => x.kindDef.combatPower));
					array[7] = new TableDataGetter<Tuple<IncidentParms, List<Pawn>>>("pawns", (Tuple<IncidentParms, List<Pawn>> t) => PawnUtility.PawnKindsToCommaList(t.Item2, false));
					DebugTables.MakeTablesDialog<Tuple<IncidentParms, List<Pawn>>>(dataSources, array);
					foreach (Tuple<IncidentParms, List<Pawn>> tuple in list2)
					{
						List<Pawn> item2 = tuple.Item2;
						for (int j = 0; j < item2.Count; j++)
						{
							item2[j].DestroyOrPassToWorld(DestroyMode.Vanish);
						}
					}
					Find.TickManager.DebugSetTicksGame(ticksGame);
					Find.CurrentMap.storyState.lastRaidFaction = lastRaidFaction;
				}));
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list, null));
		}

		// Token: 0x0600457B RID: 17787 RVA: 0x001658AC File Offset: 0x00163AAC
		[CompilerGenerated]
		internal static IEnumerable<int> <MinThreatPoints>g__hourOffsets|5_0()
		{
			for (int i = 0; i < 1200; i += 6)
			{
				Find.TickManager.DebugSetTicksGame(2500 * i);
				yield return i;
			}
			yield break;
		}

		// Token: 0x0600457C RID: 17788 RVA: 0x001658B8 File Offset: 0x00163AB8
		[CompilerGenerated]
		internal static string <TraderKindThings>g__TraderTradeability|9_1(TraderKindDef tk, ThingDef td)
		{
			Tradeability tradeability = Tradeability.None;
			foreach (StockGenerator stockGenerator in tk.stockGenerators)
			{
				Tradeability tradeability2 = stockGenerator.TradeabilityFor(td);
				if (tradeability == Tradeability.None)
				{
					tradeability = tradeability2;
				}
				else if ((tradeability == Tradeability.Buyable && tradeability2 == Tradeability.Sellable) || (tradeability == Tradeability.Sellable && tradeability2 == Tradeability.Buyable))
				{
					tradeability = Tradeability.All;
					break;
				}
			}
			switch (tradeability)
			{
			case Tradeability.None:
				return "";
			case Tradeability.Sellable:
				return "buy";
			case Tradeability.Buyable:
				return "sell";
			case Tradeability.All:
				return "✓";
			default:
				return td.tradeability.ToString();
			}
		}
	}
}
