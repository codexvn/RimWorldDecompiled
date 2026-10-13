using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using LudeonTK;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000AF3 RID: 2803
	public static class DebugOutputsPawns
	{
		// Token: 0x060047FF RID: 18431 RVA: 0x0016FC90 File Offset: 0x0016DE90
		[DebugOutput("Pawns", false)]
		public static void PawnKindsBasics()
		{
			IEnumerable<PawnKindDef> dataSources = (from d in DefDatabase<PawnKindDef>.AllDefs
			where d.race != null && d.RaceProps.Humanlike
			select d).OrderBy(delegate(PawnKindDef k)
			{
				if (k.defaultFactionDef == null)
				{
					return "";
				}
				return k.defaultFactionDef.label;
			}).ThenBy((PawnKindDef k) => k.combatPower);
			TableDataGetter<PawnKindDef>[] array = new TableDataGetter<PawnKindDef>[19];
			array[0] = new TableDataGetter<PawnKindDef>("defName", (PawnKindDef d) => d.defName);
			array[1] = new TableDataGetter<PawnKindDef>("faction", delegate(PawnKindDef d)
			{
				if (d.defaultFactionDef == null)
				{
					return "";
				}
				return d.defaultFactionDef.defName;
			});
			array[2] = new TableDataGetter<PawnKindDef>("points", (PawnKindDef d) => d.combatPower.ToString("F0"));
			array[3] = new TableDataGetter<PawnKindDef>("min\nage", (PawnKindDef d) => d.minGenerationAge.ToString("F0"));
			array[4] = new TableDataGetter<PawnKindDef>("max\nage", delegate(PawnKindDef d)
			{
				if (d.maxGenerationAge >= 10000)
				{
					return "";
				}
				return d.maxGenerationAge.ToString("F0");
			});
			array[5] = new TableDataGetter<PawnKindDef>("recruit\nresistance", (PawnKindDef d) => d.initialResistanceRange.ToString());
			array[6] = new TableDataGetter<PawnKindDef>("recruit\nwill", (PawnKindDef d) => d.initialWillRange.ToString());
			array[7] = new TableDataGetter<PawnKindDef>("item\nquality", (PawnKindDef d) => d.itemQuality.ToString());
			array[8] = new TableDataGetter<PawnKindDef>("force norm\ngear qual", (PawnKindDef d) => d.forceNormalGearQuality.ToStringCheckBlank());
			array[9] = new TableDataGetter<PawnKindDef>("weapon\n$", (PawnKindDef d) => d.weaponMoney.ToString());
			array[10] = new TableDataGetter<PawnKindDef>("apparel\n$", (PawnKindDef d) => d.apparelMoney.ToString());
			array[11] = new TableDataGetter<PawnKindDef>("tech hediffs\nchance", (PawnKindDef d) => d.techHediffsChance.ToStringPercentEmptyZero("F0"));
			array[12] = new TableDataGetter<PawnKindDef>("tech hediffs\n$", (PawnKindDef d) => d.techHediffsMoney.ToString());
			array[13] = new TableDataGetter<PawnKindDef>("gear\nhealth", (PawnKindDef d) => d.gearHealthRange.ToString());
			array[14] = new TableDataGetter<PawnKindDef>("inv\nnutrition", (PawnKindDef d) => d.invNutrition.ToString());
			array[15] = new TableDataGetter<PawnKindDef>("addiction\nchance", (PawnKindDef d) => d.chemicalAddictionChance.ToStringPercent());
			array[16] = new TableDataGetter<PawnKindDef>("combat drug\nchance", delegate(PawnKindDef d)
			{
				if (d.combatEnhancingDrugsChance <= 0f)
				{
					return "";
				}
				return d.combatEnhancingDrugsChance.ToStringPercent();
			});
			array[17] = new TableDataGetter<PawnKindDef>("combat drug\ncount", delegate(PawnKindDef d)
			{
				if (d.combatEnhancingDrugsCount.max <= 0)
				{
					return "";
				}
				return d.combatEnhancingDrugsCount.ToString();
			});
			array[18] = new TableDataGetter<PawnKindDef>("backstory cryptosleep\ncommonality", (PawnKindDef d) => d.backstoryCryptosleepCommonality.ToStringPercentEmptyZero("F0"));
			DebugTables.MakeTablesDialog<PawnKindDef>(dataSources, array);
		}

		// Token: 0x06004800 RID: 18432 RVA: 0x00170068 File Offset: 0x0016E268
		[DebugOutput("Pawns", false)]
		public static void PawnKindsWeaponUsage()
		{
			List<TableDataGetter<PawnKindDef>> list = new List<TableDataGetter<PawnKindDef>>();
			list.Add(new TableDataGetter<PawnKindDef>("defName", (PawnKindDef x) => x.defName));
			list.Add(new TableDataGetter<PawnKindDef>("avg $", (PawnKindDef x) => x.weaponMoney.Average.ToString()));
			list.Add(new TableDataGetter<PawnKindDef>("min $", (PawnKindDef x) => x.weaponMoney.min.ToString()));
			list.Add(new TableDataGetter<PawnKindDef>("max $", (PawnKindDef x) => x.weaponMoney.max.ToString()));
			list.Add(new TableDataGetter<PawnKindDef>("points", (PawnKindDef x) => x.combatPower.ToString()));
			list.AddRange(from w in DefDatabase<ThingDef>.AllDefs
			where w.IsWeapon && !w.weaponTags.NullOrEmpty<string>()
			orderby w.IsMeleeWeapon descending, w.techLevel, w.BaseMarketValue
			select new TableDataGetter<PawnKindDef>(w.label.Shorten() + "\n$" + w.BaseMarketValue.ToString("F0"), delegate(PawnKindDef k)
			{
				if (k.weaponTags == null || !w.weaponTags.Any((string z) => k.weaponTags.Contains(z)))
				{
					return "";
				}
				float num = PawnWeaponGenerator.CheapestNonDerpPriceFor(w);
				if (k.weaponMoney.max < num)
				{
					return "-";
				}
				if (k.weaponMoney.min > num)
				{
					return "✓";
				}
				return (1f - (num - k.weaponMoney.min) / (k.weaponMoney.max - k.weaponMoney.min)).ToStringPercent("F0");
			}));
			DebugTables.MakeTablesDialog<PawnKindDef>((from x in DefDatabase<PawnKindDef>.AllDefs
			where x.RaceProps.intelligence >= Intelligence.ToolUser
			select x).OrderBy(delegate(PawnKindDef x)
			{
				if (x.defaultFactionDef == null)
				{
					return int.MaxValue;
				}
				return (int)x.defaultFactionDef.techLevel;
			}).ThenBy((PawnKindDef x) => x.combatPower), list.ToArray());
		}

		// Token: 0x06004801 RID: 18433 RVA: 0x001702A4 File Offset: 0x0016E4A4
		[DebugOutput("Pawns", false)]
		public static void PawnKindsApparelUsage()
		{
			List<TableDataGetter<PawnKindDef>> list = new List<TableDataGetter<PawnKindDef>>();
			list.Add(new TableDataGetter<PawnKindDef>("defName", (PawnKindDef x) => x.defName));
			list.Add(new TableDataGetter<PawnKindDef>("avg $", (PawnKindDef x) => x.apparelMoney.Average.ToString()));
			list.Add(new TableDataGetter<PawnKindDef>("min $", (PawnKindDef x) => x.apparelMoney.min.ToString()));
			list.Add(new TableDataGetter<PawnKindDef>("max $", (PawnKindDef x) => x.apparelMoney.max.ToString()));
			list.Add(new TableDataGetter<PawnKindDef>("points", (PawnKindDef x) => x.combatPower.ToString()));
			list.AddRange((from a in DefDatabase<ThingDef>.AllDefs
			where a.IsApparel
			orderby PawnApparelGenerator.IsHeadgear(a), a.techLevel, a.BaseMarketValue
			select a).Select(delegate(ThingDef a)
			{
				Predicate<string> <>9__14;
				return new TableDataGetter<PawnKindDef>(a.label.Shorten() + "\n$" + a.BaseMarketValue.ToString("F0"), delegate(PawnKindDef k)
				{
					if (k.apparelRequired != null && k.apparelRequired.Contains(a))
					{
						return "Rq";
					}
					if (k.apparelDisallowTags != null)
					{
						List<string> apparelDisallowTags = k.apparelDisallowTags;
						Predicate<string> predicate;
						if ((predicate = <>9__14) == null)
						{
							predicate = (<>9__14 = ((string tag) => a.apparel.tags.Contains(tag)));
						}
						if (apparelDisallowTags.Any(predicate))
						{
							return "distag";
						}
					}
					if (k.apparelAllowHeadgearChance <= 0f && PawnApparelGenerator.IsHeadgear(a))
					{
						return "nohat";
					}
					List<SpecificApparelRequirement> specificApparelRequirements = k.specificApparelRequirements;
					if (specificApparelRequirements != null)
					{
						for (int i = 0; i < specificApparelRequirements.Count; i++)
						{
							if (PawnApparelGenerator.ApparelRequirementHandlesThing(specificApparelRequirements[i], a) && PawnApparelGenerator.ApparelRequirementTagsMatch(specificApparelRequirements[i], a))
							{
								return "SpRq";
							}
						}
					}
					if (k.apparelTags == null || !a.apparel.tags.Any((string z) => k.apparelTags.Contains(z)))
					{
						return "";
					}
					float baseMarketValue = a.BaseMarketValue;
					if (k.apparelMoney.max < baseMarketValue)
					{
						return "-";
					}
					if (k.apparelMoney.min > baseMarketValue)
					{
						return "✓";
					}
					return (1f - (baseMarketValue - k.apparelMoney.min) / (k.apparelMoney.max - k.apparelMoney.min)).ToStringPercent("F0");
				});
			}));
			DebugTables.MakeTablesDialog<PawnKindDef>((from x in DefDatabase<PawnKindDef>.AllDefs
			where x.RaceProps.Humanlike
			select x).OrderBy(delegate(PawnKindDef x)
			{
				if (x.defaultFactionDef == null)
				{
					return int.MaxValue;
				}
				return (int)x.defaultFactionDef.techLevel;
			}).ThenBy((PawnKindDef x) => x.combatPower), list.ToArray());
		}

		// Token: 0x06004802 RID: 18434 RVA: 0x001704E0 File Offset: 0x0016E6E0
		[DebugOutput("Pawns", false)]
		public static void PawnKindsTechHediffUsage()
		{
			List<TableDataGetter<PawnKindDef>> list = new List<TableDataGetter<PawnKindDef>>();
			list.Add(new TableDataGetter<PawnKindDef>("defName", (PawnKindDef x) => x.defName));
			list.Add(new TableDataGetter<PawnKindDef>("chance", (PawnKindDef x) => x.techHediffsChance.ToStringPercent()));
			list.Add(new TableDataGetter<PawnKindDef>("$\nmin", (PawnKindDef x) => x.techHediffsMoney.min.ToString()));
			list.Add(new TableDataGetter<PawnKindDef>("$\nmax", (PawnKindDef x) => x.techHediffsMoney.max.ToString()));
			list.Add(new TableDataGetter<PawnKindDef>("points", (PawnKindDef x) => x.combatPower.ToString()));
			list.AddRange(from t in DefDatabase<ThingDef>.AllDefs
			where t.isTechHediff && t.techHediffsTags != null
			orderby t.techLevel descending, t.BaseMarketValue
			select new TableDataGetter<PawnKindDef>(t.label.Shorten().Replace(" ", "\n") + "\n$" + t.BaseMarketValue.ToString("F0"), delegate(PawnKindDef k)
			{
				if (k.techHediffsTags == null || !t.techHediffsTags.Any((string tag) => k.techHediffsTags.Contains(tag)))
				{
					return "";
				}
				if (k.techHediffsMoney.max < t.BaseMarketValue)
				{
					return "-";
				}
				if (k.techHediffsMoney.min >= t.BaseMarketValue)
				{
					return "✓";
				}
				return (1f - (t.BaseMarketValue - k.techHediffsMoney.min) / (k.techHediffsMoney.max - k.techHediffsMoney.min)).ToStringPercent("F0");
			}));
			DebugTables.MakeTablesDialog<PawnKindDef>((from x in DefDatabase<PawnKindDef>.AllDefs
			where x.RaceProps.Humanlike
			select x).OrderBy(delegate(PawnKindDef x)
			{
				if (x.defaultFactionDef == null)
				{
					return int.MaxValue;
				}
				return (int)x.defaultFactionDef.techLevel;
			}).ThenBy((PawnKindDef x) => x.combatPower), list.ToArray());
		}

		// Token: 0x06004803 RID: 18435 RVA: 0x001706F8 File Offset: 0x0016E8F8
		[DebugOutput("Pawns", false)]
		public static void PawnKindGearSampled()
		{
			IEnumerable<PawnKindDef> enumerable = from k in DefDatabase<PawnKindDef>.AllDefs
			where k.RaceProps.ToolUser
			orderby k.combatPower
			select k;
			List<FloatMenuOption> list = new List<FloatMenuOption>();
			foreach (PawnKindDef pawnKindDef in enumerable)
			{
				Faction fac = FactionUtility.DefaultFactionFrom(pawnKindDef.defaultFactionDef);
				PawnKindDef kind = pawnKindDef;
				FloatMenuOption item = new FloatMenuOption(kind.defName + " (" + kind.combatPower.ToString() + ")", delegate()
				{
					DefMap<ThingDef, int> weapons = new DefMap<ThingDef, int>();
					DefMap<ThingDef, int> apparel = new DefMap<ThingDef, int>();
					DefMap<HediffDef, int> hediffs = new DefMap<HediffDef, int>();
					for (int i = 0; i < 400; i++)
					{
						Pawn pawn = PawnGenerator.GeneratePawn(kind, fac, null);
						if (pawn.equipment.Primary != null)
						{
							DefMap<ThingDef, int> weapons2 = weapons;
							ThingDef def = pawn.equipment.Primary.def;
							int num = weapons2[def];
							weapons2[def] = num + 1;
						}
						foreach (Hediff hediff in pawn.health.hediffSet.hediffs)
						{
							DefMap<HediffDef, int> hediffs2 = hediffs;
							HediffDef def2 = hediff.def;
							int num = hediffs2[def2];
							hediffs2[def2] = num + 1;
						}
						foreach (Apparel apparel3 in pawn.apparel.WornApparel)
						{
							DefMap<ThingDef, int> apparel2 = apparel;
							ThingDef def = apparel3.def;
							int num = apparel2[def];
							apparel2[def] = num + 1;
						}
						pawn.Destroy(DestroyMode.Vanish);
					}
					StringBuilder stringBuilder = new StringBuilder();
					stringBuilder.AppendLine(string.Concat(new string[]
					{
						"Sampled ",
						400.ToString(),
						"x ",
						kind.defName,
						":"
					}));
					stringBuilder.AppendLine("Weapons");
					IEnumerable<ThingDef> allDefs = DefDatabase<ThingDef>.AllDefs;
					Func<ThingDef, int> <>9__3;
					Func<ThingDef, int> keySelector;
					if ((keySelector = <>9__3) == null)
					{
						keySelector = (<>9__3 = ((ThingDef t) => weapons[t]));
					}
					foreach (ThingDef thingDef in allDefs.OrderByDescending(keySelector))
					{
						int num2 = weapons[thingDef];
						if (num2 > 0)
						{
							stringBuilder.AppendLine("  " + thingDef.defName + "    " + ((float)num2 / 400f).ToStringPercent());
						}
					}
					stringBuilder.AppendLine();
					stringBuilder.AppendLine("Apparel");
					IEnumerable<ThingDef> allDefs2 = DefDatabase<ThingDef>.AllDefs;
					Func<ThingDef, int> <>9__4;
					Func<ThingDef, int> keySelector2;
					if ((keySelector2 = <>9__4) == null)
					{
						keySelector2 = (<>9__4 = ((ThingDef t) => apparel[t]));
					}
					foreach (ThingDef thingDef2 in allDefs2.OrderByDescending(keySelector2))
					{
						int num3 = apparel[thingDef2];
						if (num3 > 0)
						{
							stringBuilder.AppendLine("  " + thingDef2.defName + "    " + ((float)num3 / 400f).ToStringPercent());
						}
					}
					stringBuilder.AppendLine();
					stringBuilder.AppendLine("Tech hediffs");
					IEnumerable<HediffDef> source = from h in DefDatabase<HediffDef>.AllDefs
					where h.spawnThingOnRemoved != null
					select h;
					Func<HediffDef, int> <>9__6;
					Func<HediffDef, int> keySelector3;
					if ((keySelector3 = <>9__6) == null)
					{
						keySelector3 = (<>9__6 = ((HediffDef h) => hediffs[h]));
					}
					foreach (HediffDef hediffDef in source.OrderByDescending(keySelector3))
					{
						int num4 = hediffs[hediffDef];
						if (num4 > 0)
						{
							stringBuilder.AppendLine("  " + hediffDef.defName + "    " + ((float)num4 / 400f).ToStringPercent());
						}
					}
					stringBuilder.AppendLine();
					stringBuilder.AppendLine("Addiction hediffs");
					IEnumerable<HediffDef> source2 = from h in DefDatabase<HediffDef>.AllDefs
					where h.IsAddiction
					select h;
					Func<HediffDef, int> <>9__8;
					Func<HediffDef, int> keySelector4;
					if ((keySelector4 = <>9__8) == null)
					{
						keySelector4 = (<>9__8 = ((HediffDef h) => hediffs[h]));
					}
					foreach (HediffDef hediffDef2 in source2.OrderByDescending(keySelector4))
					{
						int num5 = hediffs[hediffDef2];
						if (num5 > 0)
						{
							stringBuilder.AppendLine("  " + hediffDef2.defName + "    " + ((float)num5 / 400f).ToStringPercent());
						}
					}
					stringBuilder.AppendLine();
					stringBuilder.AppendLine("Other hediffs");
					IEnumerable<HediffDef> source3 = from h in DefDatabase<HediffDef>.AllDefs
					where h.spawnThingOnRemoved == null && !h.IsAddiction
					select h;
					Func<HediffDef, int> <>9__10;
					Func<HediffDef, int> keySelector5;
					if ((keySelector5 = <>9__10) == null)
					{
						keySelector5 = (<>9__10 = ((HediffDef h) => hediffs[h]));
					}
					foreach (HediffDef hediffDef3 in source3.OrderByDescending(keySelector5))
					{
						int num6 = hediffs[hediffDef3];
						if (num6 > 0)
						{
							stringBuilder.AppendLine("  " + hediffDef3.defName + "    " + ((float)num6 / 400f).ToStringPercent());
						}
					}
					Log.Message(stringBuilder.ToString().TrimEndNewlines());
				}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
				list.Add(item);
			}
			Find.WindowStack.Add(new FloatMenu(list));
		}

		// Token: 0x06004804 RID: 18436 RVA: 0x0017080C File Offset: 0x0016EA0C
		[DebugOutput("Pawns", false)]
		public static void PawnWorkDisablesSampled()
		{
			IEnumerable<PawnKindDef> enumerable = from k in DefDatabase<PawnKindDef>.AllDefs
			where k.RaceProps.Humanlike
			orderby k.combatPower
			select k;
			List<FloatMenuOption> list = new List<FloatMenuOption>();
			foreach (PawnKindDef kind2 in enumerable)
			{
				PawnKindDef kind = kind2;
				Faction fac = FactionUtility.DefaultFactionFrom(kind.defaultFactionDef);
				FloatMenuOption item = new FloatMenuOption(kind.defName + " (" + kind.combatPower.ToString() + ")", delegate()
				{
					Dictionary<WorkTags, int> dictionary = new Dictionary<WorkTags, int>();
					for (int i = 0; i < 1000; i++)
					{
						Pawn pawn = PawnGenerator.GeneratePawn(kind, fac, null);
						WorkTags combinedDisabledWorkTags = pawn.CombinedDisabledWorkTags;
						foreach (object obj in Enum.GetValues(typeof(WorkTags)))
						{
							WorkTags workTags = (WorkTags)obj;
							if (!dictionary.ContainsKey(workTags))
							{
								dictionary.Add(workTags, 0);
							}
							if ((combinedDisabledWorkTags & workTags) != WorkTags.None)
							{
								Dictionary<WorkTags, int> dictionary2 = dictionary;
								WorkTags key = workTags;
								int num = dictionary2[key];
								dictionary2[key] = num + 1;
							}
						}
						pawn.Destroy(DestroyMode.Vanish);
					}
					StringBuilder stringBuilder = new StringBuilder();
					stringBuilder.AppendLine(string.Concat(new string[]
					{
						"Sampled ",
						1000.ToString(),
						"x ",
						kind.defName,
						":"
					}));
					stringBuilder.AppendLine("Worktags disabled");
					foreach (object obj2 in Enum.GetValues(typeof(WorkTags)))
					{
						WorkTags key2 = (WorkTags)obj2;
						int num2 = dictionary[key2];
						stringBuilder.AppendLine(string.Concat(new string[]
						{
							"  ",
							key2.ToString(),
							"    ",
							num2.ToString(),
							" (",
							((float)num2 / 1000f).ToStringPercent(),
							")"
						}));
					}
					Log.Message(stringBuilder.ToString().TrimEndNewlines());
				}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
				list.Add(item);
			}
			Find.WindowStack.Add(new FloatMenu(list));
		}

		// Token: 0x06004805 RID: 18437 RVA: 0x00170928 File Offset: 0x0016EB28
		[DebugOutput("Pawns", false)]
		public static void LivePawnsInspirationChances()
		{
			List<TableDataGetter<Pawn>> list = new List<TableDataGetter<Pawn>>();
			list.Add(new TableDataGetter<Pawn>("name", (Pawn p) => p.Label));
			using (IEnumerator<InspirationDef> enumerator = DefDatabase<InspirationDef>.AllDefs.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					InspirationDef iDef = enumerator.Current;
					list.Add(new TableDataGetter<Pawn>(iDef.defName, delegate(Pawn p)
					{
						if (iDef.Worker.InspirationCanOccur(p))
						{
							return iDef.Worker.CommonalityFor(p).ToString();
						}
						return "-no-";
					}));
				}
			}
			DebugTables.MakeTablesDialog<Pawn>(Find.CurrentMap.mapPawns.FreeColonistsSpawned, list.ToArray());
		}

		// Token: 0x06004806 RID: 18438 RVA: 0x001709EC File Offset: 0x0016EBEC
		[DebugOutput("Pawns", false)]
		public static void RacesFoodConsumption()
		{
			Func<ThingDef, int, string> lsName = delegate(ThingDef d, int lsIndex)
			{
				if (d.race.lifeStageAges.Count <= lsIndex)
				{
					return "";
				}
				return d.race.lifeStageAges[lsIndex].def.defName;
			};
			Func<ThingDef, int, string> maxFood = delegate(ThingDef d, int lsIndex)
			{
				if (d.race.lifeStageAges.Count <= lsIndex)
				{
					return "";
				}
				LifeStageDef def = d.race.lifeStageAges[lsIndex].def;
				return (d.race.baseBodySize * def.bodySizeFactor * def.foodMaxFactor).ToString("F2");
			};
			Func<ThingDef, int, string> hungerRate = delegate(ThingDef d, int lsIndex)
			{
				if (d.race.lifeStageAges.Count <= lsIndex)
				{
					return "";
				}
				LifeStageDef def = d.race.lifeStageAges[lsIndex].def;
				return (d.race.baseHungerRate * def.hungerRateFactor).ToString("F2");
			};
			IEnumerable<ThingDef> dataSources = from d in DefDatabase<ThingDef>.AllDefs
			where d.race != null && d.race.EatsFood
			orderby d.race.baseHungerRate descending
			select d;
			TableDataGetter<ThingDef>[] array = new TableDataGetter<ThingDef>[13];
			array[0] = new TableDataGetter<ThingDef>("defName", (ThingDef d) => d.defName);
			array[1] = new TableDataGetter<ThingDef>("Lifestage 0", (ThingDef d) => lsName(d, 0));
			array[2] = new TableDataGetter<ThingDef>("maxFood", (ThingDef d) => maxFood(d, 0));
			array[3] = new TableDataGetter<ThingDef>("hungerRate", (ThingDef d) => hungerRate(d, 0));
			array[4] = new TableDataGetter<ThingDef>("Lifestage 1", (ThingDef d) => lsName(d, 1));
			array[5] = new TableDataGetter<ThingDef>("maxFood", (ThingDef d) => maxFood(d, 1));
			array[6] = new TableDataGetter<ThingDef>("hungerRate", (ThingDef d) => hungerRate(d, 1));
			array[7] = new TableDataGetter<ThingDef>("Lifestage 2", (ThingDef d) => lsName(d, 2));
			array[8] = new TableDataGetter<ThingDef>("maxFood", (ThingDef d) => maxFood(d, 2));
			array[9] = new TableDataGetter<ThingDef>("hungerRate", (ThingDef d) => hungerRate(d, 2));
			array[10] = new TableDataGetter<ThingDef>("Lifestage 3", (ThingDef d) => lsName(d, 3));
			array[11] = new TableDataGetter<ThingDef>("maxFood", (ThingDef d) => maxFood(d, 3));
			array[12] = new TableDataGetter<ThingDef>("hungerRate", (ThingDef d) => hungerRate(d, 3));
			DebugTables.MakeTablesDialog<ThingDef>(dataSources, array);
		}

		// Token: 0x06004807 RID: 18439 RVA: 0x00170C24 File Offset: 0x0016EE24
		[DebugOutput("Pawns", false)]
		public static void RacesButchery()
		{
			IEnumerable<ThingDef> dataSources = from d in DefDatabase<ThingDef>.AllDefs
			where d.race != null
			orderby d.race.baseBodySize
			select d;
			TableDataGetter<ThingDef>[] array = new TableDataGetter<ThingDef>[8];
			array[0] = new TableDataGetter<ThingDef>("defName", (ThingDef d) => d.defName);
			array[1] = new TableDataGetter<ThingDef>("mktval", (ThingDef d) => d.GetStatValueAbstract(StatDefOf.MarketValue, null).ToString("F0"));
			array[2] = new TableDataGetter<ThingDef>("healthScale", (ThingDef d) => d.race.baseHealthScale.ToString("F2"));
			array[3] = new TableDataGetter<ThingDef>("hunger rate", (ThingDef d) => d.race.baseHungerRate.ToString("F2"));
			array[4] = new TableDataGetter<ThingDef>("wildness", (ThingDef d) => d.GetStatValueAbstract(StatDefOf.Wildness, null).ToStringPercent());
			array[5] = new TableDataGetter<ThingDef>("bodySize", (ThingDef d) => d.race.baseBodySize.ToString("F2"));
			array[6] = new TableDataGetter<ThingDef>("meatAmount", (ThingDef d) => d.GetStatValueAbstract(StatDefOf.MeatAmount, null).ToString("F0"));
			array[7] = new TableDataGetter<ThingDef>("leatherAmount", (ThingDef d) => d.GetStatValueAbstract(StatDefOf.LeatherAmount, null).ToString("F0"));
			DebugTables.MakeTablesDialog<ThingDef>(dataSources, array);
		}

		// Token: 0x06004808 RID: 18440 RVA: 0x00170DEC File Offset: 0x0016EFEC
		[DebugOutput("Pawns", false)]
		public static void AnimalsBasics()
		{
			IEnumerable<PawnKindDef> dataSources = from d in DefDatabase<PawnKindDef>.AllDefs
			where d.race != null && d.RaceProps.Animal
			select d;
			TableDataGetter<PawnKindDef>[] array = new TableDataGetter<PawnKindDef>[18];
			array[0] = new TableDataGetter<PawnKindDef>("defName", (PawnKindDef d) => d.defName);
			array[1] = new TableDataGetter<PawnKindDef>("dps", (PawnKindDef d) => DebugOutputsPawns.MeleeDps(d).ToString("F1"));
			array[2] = new TableDataGetter<PawnKindDef>("healthScale", (PawnKindDef d) => d.RaceProps.baseHealthScale.ToString("F2"));
			array[3] = new TableDataGetter<PawnKindDef>("points", (PawnKindDef d) => d.combatPower.ToString("F0"));
			array[4] = new TableDataGetter<PawnKindDef>("points guess", (PawnKindDef d) => DebugOutputsPawns.<AnimalsBasics>g__pointsGuess|9_0(d).ToString("F0"));
			array[5] = new TableDataGetter<PawnKindDef>("speed", (PawnKindDef d) => d.race.GetStatValueAbstract(StatDefOf.MoveSpeed, null).ToString("F2"));
			array[6] = new TableDataGetter<PawnKindDef>("mktval", (PawnKindDef d) => d.race.GetStatValueAbstract(StatDefOf.MarketValue, null).ToString("F0"));
			array[7] = new TableDataGetter<PawnKindDef>("mktval guess", (PawnKindDef d) => DebugOutputsPawns.<AnimalsBasics>g__mktValGuess|9_1(d).ToString("F0"));
			array[8] = new TableDataGetter<PawnKindDef>("bodySize", (PawnKindDef d) => d.RaceProps.baseBodySize.ToString("F2"));
			array[9] = new TableDataGetter<PawnKindDef>("hunger", (PawnKindDef d) => d.RaceProps.baseHungerRate.ToString("F2"));
			array[10] = new TableDataGetter<PawnKindDef>("wildness", (PawnKindDef d) => d.race.GetStatValueAbstract(StatDefOf.Wildness, null).ToStringPercent());
			array[11] = new TableDataGetter<PawnKindDef>("manhunter\ntame chance", (PawnKindDef d) => d.RaceProps.manhunterOnTameFailChance.ToStringPercent());
			array[12] = new TableDataGetter<PawnKindDef>("manhunter\ndamage chance", (PawnKindDef d) => d.RaceProps.manhunterOnDamageChance.ToStringPercent());
			array[13] = new TableDataGetter<PawnKindDef>("lifespan", (PawnKindDef d) => d.RaceProps.lifeExpectancy.ToString("F1"));
			array[14] = new TableDataGetter<PawnKindDef>("trainability", delegate(PawnKindDef d)
			{
				if (d.RaceProps.trainability == null)
				{
					return "null";
				}
				return d.RaceProps.trainability.label;
			});
			array[15] = new TableDataGetter<PawnKindDef>("tempMin", (PawnKindDef d) => d.race.GetStatValueAbstract(StatDefOf.ComfyTemperatureMin, null).ToString("F0"));
			array[16] = new TableDataGetter<PawnKindDef>("tempMax", (PawnKindDef d) => d.race.GetStatValueAbstract(StatDefOf.ComfyTemperatureMax, null).ToString("F0"));
			array[17] = new TableDataGetter<PawnKindDef>("flammability", (PawnKindDef d) => d.race.GetStatValueAbstract(StatDefOf.Flammability, null).ToStringPercent());
			DebugTables.MakeTablesDialog<PawnKindDef>(dataSources, array);
		}

		// Token: 0x06004809 RID: 18441 RVA: 0x0017114F File Offset: 0x0016F34F
		private static float RaceMeleeDpsEstimate(ThingDef race)
		{
			return race.GetStatValueAbstract(StatDefOf.MeleeDPS, null);
		}

		// Token: 0x0600480A RID: 18442 RVA: 0x00171160 File Offset: 0x0016F360
		[DebugOutput("Pawns", false)]
		public static void AnimalPointsToHuntOrSlaughter()
		{
			IEnumerable<PawnKindDef> dataSources = from a in DefDatabase<PawnKindDef>.AllDefs
			where a.race != null && a.RaceProps.Animal
			select a into d
			orderby d.GetAnimalPointsToHuntOrSlaughter()
			select d;
			TableDataGetter<PawnKindDef>[] array = new TableDataGetter<PawnKindDef>[7];
			array[0] = new TableDataGetter<PawnKindDef>("animal", (PawnKindDef a) => a.LabelCap);
			array[1] = new TableDataGetter<PawnKindDef>("combat power", (PawnKindDef a) => a.combatPower.ToString());
			array[2] = new TableDataGetter<PawnKindDef>("manhunt on dmg", (PawnKindDef a) => a.RaceProps.manhunterOnDamageChance.ToStringPercent());
			array[3] = new TableDataGetter<PawnKindDef>("manhunt on tame", (PawnKindDef a) => a.RaceProps.manhunterOnTameFailChance.ToStringPercent());
			array[4] = new TableDataGetter<PawnKindDef>("wildness", (PawnKindDef a) => a.race.GetStatValueAbstract(StatDefOf.Wildness, null).ToString());
			array[5] = new TableDataGetter<PawnKindDef>("mkt val", (PawnKindDef a) => a.race.statBases.Find((StatModifier x) => x.stat == StatDefOf.MarketValue).value.ToString());
			array[6] = new TableDataGetter<PawnKindDef>("points", (PawnKindDef a) => a.GetAnimalPointsToHuntOrSlaughter().ToString());
			DebugTables.MakeTablesDialog<PawnKindDef>(dataSources, array);
		}

		// Token: 0x0600480B RID: 18443 RVA: 0x001712FC File Offset: 0x0016F4FC
		[DebugOutput("Pawns", false)]
		public static void AnimalCombatBalance()
		{
			IEnumerable<PawnKindDef> dataSources = from d in DefDatabase<PawnKindDef>.AllDefs
			where d.race != null && d.RaceProps.Animal
			orderby d.combatPower
			select d;
			TableDataGetter<PawnKindDef>[] array = new TableDataGetter<PawnKindDef>[8];
			array[0] = new TableDataGetter<PawnKindDef>("animal", (PawnKindDef k) => k.defName);
			array[1] = new TableDataGetter<PawnKindDef>("meleeDps", (PawnKindDef k) => DebugOutputsPawns.MeleeDps(k).ToString("F1"));
			array[2] = new TableDataGetter<PawnKindDef>("baseHealthScale", (PawnKindDef k) => k.RaceProps.baseHealthScale.ToString());
			array[3] = new TableDataGetter<PawnKindDef>("moveSpeed", (PawnKindDef k) => k.race.GetStatValueAbstract(StatDefOf.MoveSpeed, null).ToString());
			array[4] = new TableDataGetter<PawnKindDef>("averageArmor", (PawnKindDef k) => DebugOutputsPawns.AverageArmor(k).ToStringPercent());
			array[5] = new TableDataGetter<PawnKindDef>("combatPowerCalculated", (PawnKindDef k) => DebugOutputsPawns.CombatPowerCalculated(k).ToString("F0"));
			array[6] = new TableDataGetter<PawnKindDef>("combatPower", (PawnKindDef k) => k.combatPower.ToString());
			array[7] = new TableDataGetter<PawnKindDef>("combatPower\ndifference", (PawnKindDef k) => (k.combatPower - DebugOutputsPawns.CombatPowerCalculated(k)).ToString("F0"));
			DebugTables.MakeTablesDialog<PawnKindDef>(dataSources, array);
		}

		// Token: 0x0600480C RID: 18444 RVA: 0x001714C4 File Offset: 0x0016F6C4
		private static float CombatPowerCalculated(PawnKindDef pawnKind)
		{
			float num = 1f + DebugOutputsPawns.MeleeDps(pawnKind) * 2f;
			float num2 = 1f + (pawnKind.RaceProps.baseHealthScale + DebugOutputsPawns.AverageArmor(pawnKind) * 1.8f) * 2f;
			return num * num2 * 2.5f + 10f + pawnKind.race.GetStatValueAbstract(StatDefOf.MoveSpeed, null) * 2f;
		}

		// Token: 0x0600480D RID: 18445 RVA: 0x00171530 File Offset: 0x0016F730
		[DebugOutput("Pawns", false)]
		public static void Mechanoids()
		{
			Func<PawnKindDef, RecipeDef> recipeFor = (PawnKindDef k) => DefDatabase<RecipeDef>.AllDefs.FirstOrDefault((RecipeDef r) => r.ProducedThingDef == k.race);
			Func<PawnKindDef, string> gestationCycles = delegate(PawnKindDef k)
			{
				RecipeDef recipeDef = recipeFor(k);
				if (recipeDef == null)
				{
					return "-";
				}
				return recipeDef.gestationCycles.ToString();
			};
			Func<PawnKindDef, float> formingTime = delegate(PawnKindDef k)
			{
				RecipeDef recipeDef = recipeFor(k);
				if (recipeDef != null)
				{
					return (float)(recipeDef.formingTicks * recipeDef.gestationCycles) / 60000f;
				}
				return 0f;
			};
			Func<PawnKindDef, string> ingredients = delegate(PawnKindDef k)
			{
				RecipeDef recipeDef = recipeFor(k);
				if (recipeDef == null)
				{
					return "-";
				}
				return (from ing in recipeDef.ingredients
				select ing.Summary).ToLineList(null, false);
			};
			Func<PawnKindDef, string> ingredientMarketValue = delegate(PawnKindDef k)
			{
				RecipeDef recipeDef = recipeFor(k);
				if (recipeDef == null)
				{
					return "-";
				}
				float num = 0f;
				foreach (IngredientCount ingredientCount in recipeDef.ingredients)
				{
					Thing thing = ThingMaker.MakeThing(ingredientCount.FixedIngredient, null);
					thing.stackCount = ingredientCount.CountRequiredOfFor(ingredientCount.FixedIngredient, recipeDef, null);
					num += thing.GetStatValue(StatDefOf.MarketValue, true, -1);
					thing.Destroy(DestroyMode.Vanish);
				}
				return num.ToStringMoney(null);
			};
			Func<PawnKindDef, string> producedAt = delegate(PawnKindDef k)
			{
				RecipeDef recipe = DefDatabase<RecipeDef>.AllDefs.FirstOrDefault((RecipeDef r) => r.ProducedThingDef == k.race);
				if (recipe == null)
				{
					return "-";
				}
				return (from t in DefDatabase<ThingDef>.AllDefs
				where !t.recipes.NullOrEmpty<RecipeDef>() && t.recipes.Contains(recipe)
				select t into r
				select r.label).ToCommaList(false, false);
			};
			Func<PawnKindDef, string> researchRequired = delegate(PawnKindDef k)
			{
				RecipeDef recipeDef = DefDatabase<RecipeDef>.AllDefs.FirstOrDefault((RecipeDef r) => r.ProducedThingDef == k.race);
				if (((recipeDef != null) ? recipeDef.researchPrerequisite : null) == null)
				{
					return "-";
				}
				if (recipeDef == null)
				{
					return null;
				}
				ResearchProjectDef researchPrerequisite = recipeDef.researchPrerequisite;
				if (researchPrerequisite == null)
				{
					return null;
				}
				return researchPrerequisite.label;
			};
			Func<PawnKindDef, ThingDef> primaryWeapon = delegate(PawnKindDef k)
			{
				Pawn pawn = PawnGenerator.GeneratePawn(new PawnGenerationRequest(k, null, PawnGenerationContext.NonPlayer, null, false, false, false, true, false, 1f, false, true, false, true, true, false, false, false, false, 0f, 0f, null, 1f, null, null, null, null, null, null, null, null, null, null, null, null, false, false, false, false, null, null, null, null, null, 0f, DevelopmentalStage.Adult, null, null, null, false, false, false, -1, 0, false));
				ThingWithComps primary = pawn.equipment.Primary;
				ThingDef result = null;
				if (primary != null && primary.def.IsWeapon)
				{
					result = primary.def;
				}
				Find.WorldPawns.PassToWorld(pawn, PawnDiscardDecideMode.Discard);
				return result;
			};
			Func<PawnKindDef, string> weaponRange = delegate(PawnKindDef k)
			{
				ThingDef thingDef = primaryWeapon(k);
				if (thingDef == null)
				{
					return "-";
				}
				return thingDef.Verbs.FirstOrDefault((VerbProperties v) => v.isPrimary).range.ToString();
			};
			IEnumerable<PawnKindDef> dataSources = from d in DefDatabase<PawnKindDef>.AllDefs
			where d.race != null && d.RaceProps.IsMechanoid
			orderby d.combatPower
			select d;
			TableDataGetter<PawnKindDef>[] array = new TableDataGetter<PawnKindDef>[29];
			array[0] = new TableDataGetter<PawnKindDef>("defName", (PawnKindDef k) => k.defName);
			array[1] = new TableDataGetter<PawnKindDef>("weightClass", delegate(PawnKindDef k)
			{
				if (k.RaceProps.mechWeightClass != null)
				{
					return k.RaceProps.mechWeightClass.defName;
				}
				return "null";
			});
			array[2] = new TableDataGetter<PawnKindDef>("ingredients", (PawnKindDef k) => ingredients(k));
			array[3] = new TableDataGetter<PawnKindDef>("ingredients\nmarket value", (PawnKindDef k) => ingredientMarketValue(k));
			array[4] = new TableDataGetter<PawnKindDef>("forming\ncycles", (PawnKindDef k) => gestationCycles(k));
			array[5] = new TableDataGetter<PawnKindDef>("total\nforming time\n(game days)", (PawnKindDef k) => formingTime(k));
			array[6] = new TableDataGetter<PawnKindDef>("produced\nat", (PawnKindDef k) => producedAt(k));
			array[7] = new TableDataGetter<PawnKindDef>("research\nrequired", (PawnKindDef k) => researchRequired(k));
			array[8] = new TableDataGetter<PawnKindDef>("bandwidth\ncost", (PawnKindDef k) => k.race.GetStatValueAbstract(StatDefOf.BandwidthCost, null).ToString());
			array[9] = new TableDataGetter<PawnKindDef>("repair\nenergy\ncost", (PawnKindDef k) => k.race.GetStatValueAbstract(StatDefOf.MechEnergyLossPerHP, null).ToString());
			array[10] = new TableDataGetter<PawnKindDef>("base\nhealth\nscale", (PawnKindDef k) => k.RaceProps.baseHealthScale.ToString());
			array[11] = new TableDataGetter<PawnKindDef>("move\nspeed", (PawnKindDef k) => k.race.GetStatValueAbstract(StatDefOf.MoveSpeed, null).ToString());
			array[12] = new TableDataGetter<PawnKindDef>("average\narmor", (PawnKindDef k) => DebugOutputsPawns.AverageArmor(k).ToStringPercent());
			array[13] = new TableDataGetter<PawnKindDef>("melee\nDPS", (PawnKindDef k) => DebugOutputsPawns.MeleeDps(k).ToString("F1"));
			array[14] = new TableDataGetter<PawnKindDef>("weapon", delegate(PawnKindDef k)
			{
				ThingDef thingDef = primaryWeapon(k);
				if (thingDef == null)
				{
					return null;
				}
				return thingDef.label;
			});
			array[15] = new TableDataGetter<PawnKindDef>("weapon\nrange", (PawnKindDef k) => weaponRange(k));
			array[16] = new TableDataGetter<PawnKindDef>("weapon\nDPS", (PawnKindDef k) => DebugOutputsPawns.<Mechanoids>g__weaponDps|14_9(primaryWeapon(k)));
			array[17] = new TableDataGetter<PawnKindDef>("weapon\naccuracy\nshort", (PawnKindDef k) => DebugOutputsPawns.<Mechanoids>g__weaponHitChance|14_10(k, 12f).ToStringPercent());
			array[18] = new TableDataGetter<PawnKindDef>("weapon\naccuracy\nmedium", (PawnKindDef k) => DebugOutputsPawns.<Mechanoids>g__weaponHitChance|14_10(k, 25f).ToStringPercent());
			array[19] = new TableDataGetter<PawnKindDef>("weapon\naccuracy\nlong", (PawnKindDef k) => DebugOutputsPawns.<Mechanoids>g__weaponHitChance|14_10(k, 40f).ToStringPercent());
			array[20] = new TableDataGetter<PawnKindDef>("shooter\naccuracy\nshort", (PawnKindDef k) => DebugOutputsPawns.<Mechanoids>g__shooterHitChance|14_12(k, 12f).ToStringPercent());
			array[21] = new TableDataGetter<PawnKindDef>("shooter\naccuracy\nmedium", (PawnKindDef k) => DebugOutputsPawns.<Mechanoids>g__shooterHitChance|14_12(k, 25f).ToStringPercent());
			array[22] = new TableDataGetter<PawnKindDef>("shooter\naccuracy\nlong", (PawnKindDef k) => DebugOutputsPawns.<Mechanoids>g__shooterHitChance|14_12(k, 40f).ToStringPercent());
			array[23] = new TableDataGetter<PawnKindDef>("total\naccuracy\nshort", (PawnKindDef k) => DebugOutputsPawns.<Mechanoids>g__combinedHitChance|14_13(k, 12f).ToStringPercent());
			array[24] = new TableDataGetter<PawnKindDef>("total\naccuracy\nmedium", (PawnKindDef k) => DebugOutputsPawns.<Mechanoids>g__combinedHitChance|14_13(k, 25f).ToStringPercent());
			array[25] = new TableDataGetter<PawnKindDef>("total\naccuracy\nlong", (PawnKindDef k) => DebugOutputsPawns.<Mechanoids>g__combinedHitChance|14_13(k, 40f).ToStringPercent());
			array[26] = new TableDataGetter<PawnKindDef>("special\nabilities", (PawnKindDef k) => DebugOutputsPawns.<Mechanoids>g__specialAbilities|14_11(k).ToCommaList(false, false));
			array[27] = new TableDataGetter<PawnKindDef>("combat\npower", (PawnKindDef k) => k.combatPower.ToString());
			array[28] = new TableDataGetter<PawnKindDef>("body\nsize", (PawnKindDef k) => k.RaceProps.baseBodySize.ToString("F2"));
			DebugTables.MakeTablesDialog<PawnKindDef>(dataSources, array);
		}

		// Token: 0x0600480E RID: 18446 RVA: 0x00171AF0 File Offset: 0x0016FCF0
		private static float MeleeDps(PawnKindDef pawnKind)
		{
			Pawn pawn = PawnGenerator.GeneratePawn(new PawnGenerationRequest(pawnKind, null, PawnGenerationContext.NonPlayer, null, false, false, false, true, true, 1f, false, true, false, true, true, false, false, false, false, 0f, 0f, null, 1f, null, null, null, null, null, null, null, null, null, null, null, null, false, false, false, false, null, null, null, null, null, 0f, DevelopmentalStage.Adult, null, null, null, false, false, false, -1, 0, false));
			while (pawn.health.hediffSet.hediffs.Count > 0)
			{
				pawn.health.RemoveHediff(pawn.health.hediffSet.hediffs[0]);
			}
			float statValue = pawn.GetStatValue(StatDefOf.MeleeDPS, true, -1);
			Find.WorldPawns.PassToWorld(pawn, PawnDiscardDecideMode.Discard);
			return statValue;
		}

		// Token: 0x0600480F RID: 18447 RVA: 0x00171BE4 File Offset: 0x0016FDE4
		private static float AverageArmor(PawnKindDef pawnKind)
		{
			Pawn pawn = PawnGenerator.GeneratePawn(pawnKind, null, null);
			while (pawn.health.hediffSet.hediffs.Count > 0)
			{
				pawn.health.RemoveHediff(pawn.health.hediffSet.hediffs[0]);
			}
			float result = (pawn.GetStatValue(StatDefOf.ArmorRating_Blunt, true, -1) + pawn.GetStatValue(StatDefOf.ArmorRating_Sharp, true, -1)) / 2f;
			Find.WorldPawns.PassToWorld(pawn, PawnDiscardDecideMode.Discard);
			return result;
		}

		// Token: 0x06004810 RID: 18448 RVA: 0x00171C6C File Offset: 0x0016FE6C
		[DebugOutput("Pawns", false)]
		public static void AnimalTradeTags()
		{
			List<TableDataGetter<PawnKindDef>> list = new List<TableDataGetter<PawnKindDef>>();
			list.Add(new TableDataGetter<PawnKindDef>("animal", (PawnKindDef k) => k.defName));
			using (IEnumerator<string> enumerator = (from k in DefDatabase<PawnKindDef>.AllDefs
			where k.race.tradeTags != null
			select k).SelectMany((PawnKindDef k) => k.race.tradeTags).Distinct<string>().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					string tag = enumerator.Current;
					list.Add(new TableDataGetter<PawnKindDef>(tag, (PawnKindDef k) => (k.race.tradeTags != null && k.race.tradeTags.Contains(tag)).ToStringCheckBlank()));
				}
			}
			DebugTables.MakeTablesDialog<PawnKindDef>(from d in DefDatabase<PawnKindDef>.AllDefs
			where d.race != null && d.RaceProps.Animal
			select d, list.ToArray());
		}

		// Token: 0x06004811 RID: 18449 RVA: 0x00171D90 File Offset: 0x0016FF90
		[DebugOutput("Pawns", false)]
		public static void AnimalBehavior()
		{
			IEnumerable<PawnKindDef> dataSources = from d in DefDatabase<PawnKindDef>.AllDefs
			where d.race != null && d.RaceProps.Animal
			select d;
			TableDataGetter<PawnKindDef>[] array = new TableDataGetter<PawnKindDef>[20];
			array[0] = new TableDataGetter<PawnKindDef>("", (PawnKindDef k) => k.defName);
			array[1] = new TableDataGetter<PawnKindDef>("wildness", (PawnKindDef k) => k.race.GetStatValueAbstract(StatDefOf.Wildness, null).ToStringPercent());
			array[2] = new TableDataGetter<PawnKindDef>("min\nhandling\nskill", (PawnKindDef k) => k.race.GetStatValueAbstract(StatDefOf.MinimumHandlingSkill, null));
			array[3] = new TableDataGetter<PawnKindDef>("trainability", (PawnKindDef k) => k.race.race.trainability.defName);
			array[4] = new TableDataGetter<PawnKindDef>("FenceBlocked", (PawnKindDef k) => k.race.race.FenceBlocked.ToStringCheckBlank());
			array[5] = new TableDataGetter<PawnKindDef>("manhunterOn\nDamage\nChance", (PawnKindDef k) => k.RaceProps.manhunterOnDamageChance.ToStringPercentEmptyZero("F1"));
			array[6] = new TableDataGetter<PawnKindDef>("manhunterOn\nTameFail\nChance", (PawnKindDef k) => k.RaceProps.manhunterOnTameFailChance.ToStringPercentEmptyZero("F1"));
			array[7] = new TableDataGetter<PawnKindDef>("predator", (PawnKindDef k) => k.RaceProps.predator.ToStringCheckBlank());
			array[8] = new TableDataGetter<PawnKindDef>("bodySize", (PawnKindDef k) => k.RaceProps.baseBodySize.ToString("F2"));
			array[9] = new TableDataGetter<PawnKindDef>("max\nPreyBodySize", delegate(PawnKindDef k)
			{
				if (!k.RaceProps.predator)
				{
					return "";
				}
				return k.RaceProps.maxPreyBodySize.ToString("F2");
			});
			array[10] = new TableDataGetter<PawnKindDef>("canBe\nPredatorPrey", (PawnKindDef k) => k.RaceProps.canBePredatorPrey.ToStringCheckBlank());
			array[11] = new TableDataGetter<PawnKindDef>("petness", (PawnKindDef k) => k.RaceProps.petness.ToStringPercent());
			array[12] = new TableDataGetter<PawnKindDef>("nuzzle\nMtbHours", delegate(PawnKindDef k)
			{
				if (k.RaceProps.nuzzleMtbHours <= 0f)
				{
					return "";
				}
				return k.RaceProps.nuzzleMtbHours.ToString();
			});
			array[13] = new TableDataGetter<PawnKindDef>("pack\nAnimal", (PawnKindDef k) => k.RaceProps.packAnimal.ToStringCheckBlank());
			array[14] = new TableDataGetter<PawnKindDef>("herd\nAnimal", (PawnKindDef k) => k.RaceProps.herdAnimal.ToStringCheckBlank());
			array[15] = new TableDataGetter<PawnKindDef>("wildGroupSize\nMin", delegate(PawnKindDef k)
			{
				if (k.wildGroupSize.min == 1)
				{
					return "";
				}
				return k.wildGroupSize.min.ToString();
			});
			array[16] = new TableDataGetter<PawnKindDef>("wildGroupSize\nMax", delegate(PawnKindDef k)
			{
				if (k.wildGroupSize.max == 1)
				{
					return "";
				}
				return k.wildGroupSize.max.ToString();
			});
			array[17] = new TableDataGetter<PawnKindDef>("CanDo\nHerdMigration", (PawnKindDef k) => k.RaceProps.CanDoHerdMigration.ToStringCheckBlank());
			array[18] = new TableDataGetter<PawnKindDef>("herd\nMigration\nAllowed", (PawnKindDef k) => k.RaceProps.herdMigrationAllowed.ToStringCheckBlank());
			array[19] = new TableDataGetter<PawnKindDef>("mateMtb", (PawnKindDef k) => k.RaceProps.mateMtbHours.ToStringEmptyZero("F0"));
			DebugTables.MakeTablesDialog<PawnKindDef>(dataSources, array);
		}

		// Token: 0x06004812 RID: 18450 RVA: 0x00172150 File Offset: 0x00170350
		[DebugOutput("Pawns", false)]
		public static void AnimalsEcosystem()
		{
			Func<PawnKindDef, float> ecosystemWeightGuess = (PawnKindDef k) => k.RaceProps.baseBodySize * 0.2f + k.RaceProps.baseHungerRate * 0.8f;
			IEnumerable<PawnKindDef> dataSources = from d in DefDatabase<PawnKindDef>.AllDefs
			where d.race != null && d.RaceProps.Animal
			orderby d.ecoSystemWeight descending
			select d;
			TableDataGetter<PawnKindDef>[] array = new TableDataGetter<PawnKindDef>[6];
			array[0] = new TableDataGetter<PawnKindDef>("defName", (PawnKindDef d) => d.defName);
			array[1] = new TableDataGetter<PawnKindDef>("bodySize", (PawnKindDef d) => d.RaceProps.baseBodySize.ToString("F2"));
			array[2] = new TableDataGetter<PawnKindDef>("hunger rate", (PawnKindDef d) => d.RaceProps.baseHungerRate.ToString("F2"));
			array[3] = new TableDataGetter<PawnKindDef>("ecosystem weight", (PawnKindDef d) => d.ecoSystemWeight.ToString("F2"));
			array[4] = new TableDataGetter<PawnKindDef>("ecosystem weight guess", (PawnKindDef d) => ecosystemWeightGuess(d).ToString("F2"));
			array[5] = new TableDataGetter<PawnKindDef>("predator", (PawnKindDef d) => d.RaceProps.predator.ToStringCheckBlank());
			DebugTables.MakeTablesDialog<PawnKindDef>(dataSources, array);
		}

		// Token: 0x06004813 RID: 18451 RVA: 0x001722D8 File Offset: 0x001704D8
		[DebugOutput("Pawns", false)]
		public static void MentalBreaks()
		{
			IEnumerable<MentalBreakDef> dataSources = from d in DefDatabase<MentalBreakDef>.AllDefs
			orderby d.intensity, d.defName
			select d;
			TableDataGetter<MentalBreakDef>[] array = new TableDataGetter<MentalBreakDef>[12];
			array[0] = new TableDataGetter<MentalBreakDef>("defName", (MentalBreakDef d) => d.defName);
			array[1] = new TableDataGetter<MentalBreakDef>("intensity", (MentalBreakDef d) => d.intensity.ToString());
			array[2] = new TableDataGetter<MentalBreakDef>("chance in intensity", (MentalBreakDef d) => (d.baseCommonality / (from x in DefDatabase<MentalBreakDef>.AllDefs
			where x.intensity == d.intensity
			select x).Sum((MentalBreakDef x) => x.baseCommonality)).ToStringPercent());
			array[3] = new TableDataGetter<MentalBreakDef>("min duration (days)", delegate(MentalBreakDef d)
			{
				if (d.mentalState != null)
				{
					return ((float)d.mentalState.minTicksBeforeRecovery / 60000f).ToString("0.##");
				}
				return "";
			});
			array[4] = new TableDataGetter<MentalBreakDef>("avg duration (days)", delegate(MentalBreakDef d)
			{
				if (d.mentalState != null)
				{
					return (Mathf.Min((float)d.mentalState.minTicksBeforeRecovery + d.mentalState.recoveryMtbDays * 60000f, (float)d.mentalState.maxTicksBeforeRecovery) / 60000f).ToString("0.##");
				}
				return "";
			});
			array[5] = new TableDataGetter<MentalBreakDef>("max duration (days)", delegate(MentalBreakDef d)
			{
				if (d.mentalState != null)
				{
					return ((float)d.mentalState.maxTicksBeforeRecovery / 60000f).ToString("0.##");
				}
				return "";
			});
			array[6] = new TableDataGetter<MentalBreakDef>("recoverFromSleep", delegate(MentalBreakDef d)
			{
				if (d.mentalState == null || !d.mentalState.recoverFromSleep)
				{
					return "";
				}
				return "recoverFromSleep";
			});
			array[7] = new TableDataGetter<MentalBreakDef>("recoveryThought", delegate(MentalBreakDef d)
			{
				if (d.mentalState != null)
				{
					return d.mentalState.moodRecoveryThought.ToStringSafe<ThoughtDef>();
				}
				return "";
			});
			array[8] = new TableDataGetter<MentalBreakDef>("category", delegate(MentalBreakDef d)
			{
				if (d.mentalState == null)
				{
					return "";
				}
				return d.mentalState.category.ToString();
			});
			array[9] = new TableDataGetter<MentalBreakDef>("blockNormalThoughts", delegate(MentalBreakDef d)
			{
				if (d.mentalState == null || !d.mentalState.blockNormalThoughts)
				{
					return "";
				}
				return "blockNormalThoughts";
			});
			array[10] = new TableDataGetter<MentalBreakDef>("blockRandomInteraction", delegate(MentalBreakDef d)
			{
				if (d.mentalState == null || !d.mentalState.blockRandomInteraction)
				{
					return "";
				}
				return "blockRandomInteraction";
			});
			array[11] = new TableDataGetter<MentalBreakDef>("allowBeatfire", delegate(MentalBreakDef d)
			{
				if (d.mentalState == null || !d.mentalState.allowBeatfire)
				{
					return "";
				}
				return "allowBeatfire";
			});
			DebugTables.MakeTablesDialog<MentalBreakDef>(dataSources, array);
		}

		// Token: 0x06004814 RID: 18452 RVA: 0x00172554 File Offset: 0x00170754
		private static string ThoughtStagesText(ThoughtDef t)
		{
			string text = "";
			if (t.stages == null)
			{
				return null;
			}
			for (int i = 0; i < t.stages.Count; i++)
			{
				ThoughtStage thoughtStage = t.stages[i];
				text = text + "[" + i.ToString() + "] ";
				if (thoughtStage == null)
				{
					text += "null";
				}
				else
				{
					if (thoughtStage.label != null)
					{
						text += thoughtStage.label;
					}
					if (thoughtStage.labelSocial != null)
					{
						if (thoughtStage.label != null)
						{
							text += "/";
						}
						text += thoughtStage.labelSocial;
					}
					text += " ";
					if (thoughtStage.baseMoodEffect != 0f)
					{
						text = text + "[" + thoughtStage.baseMoodEffect.ToStringWithSign("0.##") + " Mo]";
					}
					if (thoughtStage.baseOpinionOffset != 0f)
					{
						text = text + "(" + thoughtStage.baseOpinionOffset.ToStringWithSign("0.##") + " Op)";
					}
				}
				if (i < t.stages.Count - 1)
				{
					text += "\n";
				}
			}
			return text;
		}

		// Token: 0x06004815 RID: 18453 RVA: 0x0017268C File Offset: 0x0017088C
		[DebugOutput("Pawns", false)]
		public static void Thoughts()
		{
			IEnumerable<ThoughtDef> allDefs = DefDatabase<ThoughtDef>.AllDefs;
			TableDataGetter<ThoughtDef>[] array = new TableDataGetter<ThoughtDef>[18];
			array[0] = new TableDataGetter<ThoughtDef>("defName", (ThoughtDef d) => d.defName);
			array[1] = new TableDataGetter<ThoughtDef>("type", delegate(ThoughtDef d)
			{
				if (!d.IsMemory)
				{
					return "situ";
				}
				return "mem";
			});
			array[2] = new TableDataGetter<ThoughtDef>("social", delegate(ThoughtDef d)
			{
				if (!d.IsSocial)
				{
					return "mood";
				}
				return "soc";
			});
			array[3] = new TableDataGetter<ThoughtDef>("stages", (ThoughtDef d) => DebugOutputsPawns.ThoughtStagesText(d));
			array[4] = new TableDataGetter<ThoughtDef>("best\nmood", (ThoughtDef d) => (from st in d.stages
			where st != null
			select st).Max((ThoughtStage st) => st.baseMoodEffect));
			array[5] = new TableDataGetter<ThoughtDef>("worst\nmood", (ThoughtDef d) => (from st in d.stages
			where st != null
			select st).Min((ThoughtStage st) => st.baseMoodEffect));
			array[6] = new TableDataGetter<ThoughtDef>("stack\nlimit", (ThoughtDef d) => d.stackLimit.ToString());
			array[7] = new TableDataGetter<ThoughtDef>("stack\nlimit\nper o. pawn", delegate(ThoughtDef d)
			{
				if (d.stackLimitForSameOtherPawn >= 0)
				{
					return d.stackLimitForSameOtherPawn.ToString();
				}
				return "";
			});
			array[8] = new TableDataGetter<ThoughtDef>("stacked\neffect\nmultiplier", delegate(ThoughtDef d)
			{
				if (d.stackLimit != 1)
				{
					return d.stackedEffectMultiplier.ToStringPercent();
				}
				return "";
			});
			array[9] = new TableDataGetter<ThoughtDef>("duration\n(days)", (ThoughtDef d) => d.durationDays.ToString());
			array[10] = new TableDataGetter<ThoughtDef>("effect\nmultiplying\nstat", delegate(ThoughtDef d)
			{
				if (d.effectMultiplyingStat != null)
				{
					return d.effectMultiplyingStat.defName;
				}
				return "";
			});
			array[11] = new TableDataGetter<ThoughtDef>("game\ncondition", delegate(ThoughtDef d)
			{
				if (d.gameCondition != null)
				{
					return d.gameCondition.defName;
				}
				return "";
			});
			array[12] = new TableDataGetter<ThoughtDef>("hediff", delegate(ThoughtDef d)
			{
				if (d.hediff != null)
				{
					return d.hediff.defName;
				}
				return "";
			});
			array[13] = new TableDataGetter<ThoughtDef>("lerp opinion\nto zero\nafter duration pct", (ThoughtDef d) => d.lerpOpinionToZeroAfterDurationPct.ToStringPercent());
			array[14] = new TableDataGetter<ThoughtDef>("max cumulated\nopinion\noffset", delegate(ThoughtDef d)
			{
				if (d.maxCumulatedOpinionOffset <= 99999f)
				{
					return d.maxCumulatedOpinionOffset.ToString();
				}
				return "";
			});
			array[15] = new TableDataGetter<ThoughtDef>("next\nthought", delegate(ThoughtDef d)
			{
				if (d.nextThought != null)
				{
					return d.nextThought.defName;
				}
				return "";
			});
			array[16] = new TableDataGetter<ThoughtDef>("nullified\nif not colonist", (ThoughtDef d) => d.nullifiedIfNotColonist.ToStringCheckBlank());
			array[17] = new TableDataGetter<ThoughtDef>("show\nbubble", (ThoughtDef d) => d.showBubble.ToStringCheckBlank());
			DebugTables.MakeTablesDialog<ThoughtDef>(allDefs, array);
		}

		// Token: 0x06004816 RID: 18454 RVA: 0x001729CC File Offset: 0x00170BCC
		[DebugOutput("Pawns", false)]
		public static void IdeoThoughts()
		{
			IEnumerable<ThoughtDef> dataSources = DefDatabase<ThoughtDef>.AllDefs.Where(delegate(ThoughtDef td)
			{
				foreach (PreceptDef preceptDef in DefDatabase<PreceptDef>.AllDefsListForReading)
				{
					foreach (PreceptComp preceptComp in preceptDef.comps)
					{
						PreceptComp_Thought preceptComp_Thought = preceptComp as PreceptComp_Thought;
						if (preceptComp_Thought != null && preceptComp_Thought.thought == td)
						{
							return true;
						}
					}
				}
				return false;
			});
			TableDataGetter<ThoughtDef>[] array = new TableDataGetter<ThoughtDef>[18];
			array[0] = new TableDataGetter<ThoughtDef>("defName", (ThoughtDef d) => d.defName);
			array[1] = new TableDataGetter<ThoughtDef>("type", delegate(ThoughtDef d)
			{
				if (!d.IsMemory)
				{
					return "situ";
				}
				return "mem";
			});
			array[2] = new TableDataGetter<ThoughtDef>("social", delegate(ThoughtDef d)
			{
				if (!d.IsSocial)
				{
					return "mood";
				}
				return "soc";
			});
			array[3] = new TableDataGetter<ThoughtDef>("stages", (ThoughtDef d) => DebugOutputsPawns.ThoughtStagesText(d));
			array[4] = new TableDataGetter<ThoughtDef>("best\nmood", (ThoughtDef d) => (from st in d.stages
			where st != null
			select st).Max((ThoughtStage st) => st.baseMoodEffect));
			array[5] = new TableDataGetter<ThoughtDef>("worst\nmood", (ThoughtDef d) => (from st in d.stages
			where st != null
			select st).Min((ThoughtStage st) => st.baseMoodEffect));
			array[6] = new TableDataGetter<ThoughtDef>("stack\nlimit", (ThoughtDef d) => d.stackLimit.ToString());
			array[7] = new TableDataGetter<ThoughtDef>("stack\nlimit\nper o. pawn", delegate(ThoughtDef d)
			{
				if (d.stackLimitForSameOtherPawn >= 0)
				{
					return d.stackLimitForSameOtherPawn.ToString();
				}
				return "";
			});
			array[8] = new TableDataGetter<ThoughtDef>("stacked\neffect\nmultiplier", delegate(ThoughtDef d)
			{
				if (d.stackLimit != 1)
				{
					return d.stackedEffectMultiplier.ToStringPercent();
				}
				return "";
			});
			array[9] = new TableDataGetter<ThoughtDef>("duration\n(days)", (ThoughtDef d) => d.durationDays.ToString());
			array[10] = new TableDataGetter<ThoughtDef>("effect\nmultiplying\nstat", delegate(ThoughtDef d)
			{
				if (d.effectMultiplyingStat != null)
				{
					return d.effectMultiplyingStat.defName;
				}
				return "";
			});
			array[11] = new TableDataGetter<ThoughtDef>("game\ncondition", delegate(ThoughtDef d)
			{
				if (d.gameCondition != null)
				{
					return d.gameCondition.defName;
				}
				return "";
			});
			array[12] = new TableDataGetter<ThoughtDef>("hediff", delegate(ThoughtDef d)
			{
				if (d.hediff != null)
				{
					return d.hediff.defName;
				}
				return "";
			});
			array[13] = new TableDataGetter<ThoughtDef>("lerp opinion\nto zero\nafter duration pct", (ThoughtDef d) => d.lerpOpinionToZeroAfterDurationPct.ToStringPercent());
			array[14] = new TableDataGetter<ThoughtDef>("max cumulated\nopinion\noffset", delegate(ThoughtDef d)
			{
				if (d.maxCumulatedOpinionOffset <= 99999f)
				{
					return d.maxCumulatedOpinionOffset.ToString();
				}
				return "";
			});
			array[15] = new TableDataGetter<ThoughtDef>("next\nthought", delegate(ThoughtDef d)
			{
				if (d.nextThought != null)
				{
					return d.nextThought.defName;
				}
				return "";
			});
			array[16] = new TableDataGetter<ThoughtDef>("nullified\nif not colonist", (ThoughtDef d) => d.nullifiedIfNotColonist.ToStringCheckBlank());
			array[17] = new TableDataGetter<ThoughtDef>("show\nbubble", (ThoughtDef d) => d.showBubble.ToStringCheckBlank());
			DebugTables.MakeTablesDialog<ThoughtDef>(dataSources, array);
		}

		// Token: 0x06004817 RID: 18455 RVA: 0x00172D30 File Offset: 0x00170F30
		[DebugOutput("Pawns", false)]
		public static void TraitsSampled()
		{
			List<Pawn> testColonists = new List<Pawn>();
			for (int i = 0; i < 4000; i++)
			{
				testColonists.Add(PawnGenerator.GeneratePawn(PawnKindDefOf.Colonist, Faction.OfPlayer, null));
			}
			IEnumerable<TraitDegreeData> dataSources = DefDatabase<TraitDef>.AllDefs.SelectMany((TraitDef tr) => tr.degreeDatas);
			TableDataGetter<TraitDegreeData>[] array = new TableDataGetter<TraitDegreeData>[8];
			array[0] = new TableDataGetter<TraitDegreeData>("trait", (TraitDegreeData d) => DebugOutputsPawns.<TraitsSampled>g__getTrait|24_0(d).defName);
			array[1] = new TableDataGetter<TraitDegreeData>("trait commonality", (TraitDegreeData d) => DebugOutputsPawns.<TraitsSampled>g__getTrait|24_0(d).GetGenderSpecificCommonality(Gender.None).ToString("F2"));
			array[2] = new TableDataGetter<TraitDegreeData>("trait commonalityFemale", (TraitDegreeData d) => DebugOutputsPawns.<TraitsSampled>g__getTrait|24_0(d).GetGenderSpecificCommonality(Gender.Female).ToString("F2"));
			array[3] = new TableDataGetter<TraitDegreeData>("degree", (TraitDegreeData d) => d.label);
			array[4] = new TableDataGetter<TraitDegreeData>("degree num", delegate(TraitDegreeData d)
			{
				if (DebugOutputsPawns.<TraitsSampled>g__getTrait|24_0(d).degreeDatas.Count <= 0)
				{
					return "";
				}
				return d.degree.ToString();
			});
			array[5] = new TableDataGetter<TraitDegreeData>("degree commonality", delegate(TraitDegreeData d)
			{
				if (DebugOutputsPawns.<TraitsSampled>g__getTrait|24_0(d).degreeDatas.Count <= 0)
				{
					return "";
				}
				return d.commonality.ToString("F2");
			});
			array[6] = new TableDataGetter<TraitDegreeData>("marketValueFactorOffset", (TraitDegreeData d) => d.marketValueFactorOffset.ToString("F0"));
			array[7] = new TableDataGetter<TraitDegreeData>("prevalence among " + 4000.ToString() + "\ngenerated Colonists", (TraitDegreeData d) => base.<TraitsSampled>g__getPrevalence|1(d).ToStringPercent());
			DebugTables.MakeTablesDialog<TraitDegreeData>(dataSources, array);
		}

		// Token: 0x06004818 RID: 18456 RVA: 0x00172F1C File Offset: 0x0017111C
		[DebugOutput("Pawns", false)]
		public static void BackstoryCountsPerTag()
		{
			IEnumerable<BackstoryDef> allDefs = DefDatabase<BackstoryDef>.AllDefs;
			List<string> dataSources = allDefs.SelectMany((BackstoryDef bs) => bs.spawnCategories).Distinct<string>().ToList<string>();
			Dictionary<string, int> countAdulthoods = new Dictionary<string, int>();
			Dictionary<string, int> countChildhoods = new Dictionary<string, int>();
			foreach (BackstoryDef backstoryDef in allDefs)
			{
				Dictionary<string, int> dictionary = (backstoryDef.slot == BackstorySlot.Adulthood) ? countAdulthoods : countChildhoods;
				foreach (string text in backstoryDef.spawnCategories)
				{
					if (!dictionary.ContainsKey(text))
					{
						dictionary.Add(text, 0);
					}
					Dictionary<string, int> dictionary2 = dictionary;
					string key = text;
					int num = dictionary2[key];
					dictionary2[key] = num + 1;
				}
			}
			List<TableDataGetter<string>> list = new List<TableDataGetter<string>>();
			list.Add(new TableDataGetter<string>("tag", (string t) => t));
			list.Add(new TableDataGetter<string>("adulthoods", delegate(string t)
			{
				if (!countAdulthoods.ContainsKey(t))
				{
					return 0;
				}
				return countAdulthoods[t];
			}));
			list.Add(new TableDataGetter<string>("childhoods", delegate(string t)
			{
				if (!countChildhoods.ContainsKey(t))
				{
					return 0;
				}
				return countChildhoods[t];
			}));
			DebugTables.MakeTablesDialog<string>(dataSources, list.ToArray());
		}

		// Token: 0x06004819 RID: 18457 RVA: 0x001730B0 File Offset: 0x001712B0
		[DebugOutput("Pawns", false)]
		public static void ListSolidBackstories()
		{
			IEnumerable<string> enumerable = SolidBioDatabase.allBios.SelectMany((PawnBio bio) => bio.adulthood.spawnCategories).Distinct<string>();
			List<FloatMenuOption> list = new List<FloatMenuOption>();
			foreach (string catInner2 in enumerable)
			{
				string catInner = catInner2;
				Func<PawnBio, bool> <>9__2;
				FloatMenuOption item = new FloatMenuOption(catInner, delegate()
				{
					IEnumerable<PawnBio> allBios = SolidBioDatabase.allBios;
					Func<PawnBio, bool> predicate;
					if ((predicate = <>9__2) == null)
					{
						predicate = (<>9__2 = ((PawnBio b) => b.adulthood.spawnCategories.Contains(catInner)));
					}
					IEnumerable<PawnBio> enumerable2 = allBios.Where(predicate);
					StringBuilder stringBuilder = new StringBuilder();
					stringBuilder.AppendLine(string.Concat(new string[]
					{
						"Backstories with category: ",
						catInner,
						" (",
						enumerable2.Count<PawnBio>().ToString(),
						")"
					}));
					foreach (PawnBio pawnBio in enumerable2)
					{
						stringBuilder.AppendLine(pawnBio.ToString());
					}
					Log.Message(stringBuilder.ToString());
				}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
				list.Add(item);
			}
			Find.WindowStack.Add(new FloatMenu(list));
		}

		// Token: 0x0600481A RID: 18458 RVA: 0x00173170 File Offset: 0x00171370
		[DebugOutput("Pawns", false)]
		private static void ShowBeardFrequency()
		{
			if (Find.World == null)
			{
				return;
			}
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			using (IEnumerator<Faction> enumerator = Find.FactionManager.AllFactionsVisible.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Faction f = enumerator.Current;
					list.Add(new DebugMenuOption(f.Name, DebugMenuOptionMode.Action, delegate()
					{
						Faction f = f;
						int num = 0;
						for (int i = 0; i < 100; i++)
						{
							PawnKindDef spaceRefugee = PawnKindDefOf.SpaceRefugee;
							Gender? fixedGender = new Gender?(Gender.Male);
							Pawn pawn = PawnGenerator.GeneratePawn(new PawnGenerationRequest(spaceRefugee, f, PawnGenerationContext.NonPlayer, null, false, false, false, true, false, 1f, false, true, false, true, true, false, false, false, false, 0f, 0f, null, 1f, null, null, null, null, null, null, null, fixedGender, null, null, null, null, false, false, false, false, null, null, null, null, null, 0f, DevelopmentalStage.Adult, null, null, null, false, false, false, -1, 0, false));
							if (pawn != null)
							{
								if (pawn.style.beardDef != BeardDefOf.NoBeard)
								{
									num++;
								}
								Find.WorldPawns.PassToWorld(pawn, PawnDiscardDecideMode.Discard);
							}
						}
						Log.Message(string.Concat(new string[]
						{
							"Beard count for ",
							f.Name,
							": ",
							num.ToString(),
							"/100 generated."
						}));
					}));
				}
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list, null));
		}

		// Token: 0x0600481B RID: 18459 RVA: 0x00173208 File Offset: 0x00171408
		[DebugOutput("Pawns", false)]
		public static void Entities()
		{
			IEnumerable<ThingDef> dataSources = DefDatabase<ThingDef>.AllDefs.Where(new Func<ThingDef, bool>(DebugOutputsPawns.ValidateEntity));
			TableDataGetter<ThingDef>[] array = new TableDataGetter<ThingDef>[20];
			array[0] = new TableDataGetter<ThingDef>("defName", (ThingDef d) => d.defName);
			array[1] = new TableDataGetter<ThingDef>("label", (ThingDef d) => d.label);
			array[2] = new TableDataGetter<ThingDef>("knowledge gain", delegate(ThingDef d)
			{
				if (!d.HasComp<CompStudiable>())
				{
					return "-";
				}
				CompProperties_Studiable compProperties = d.GetCompProperties<CompProperties_Studiable>();
				if (compProperties == null)
				{
					return null;
				}
				return compProperties.anomalyKnowledge.ToString();
			});
			array[3] = new TableDataGetter<ThingDef>("knowledge category", delegate(ThingDef d)
			{
				if (!d.HasComp<CompStudiable>())
				{
					return "-";
				}
				CompProperties_Studiable compProperties = d.GetCompProperties<CompProperties_Studiable>();
				if (compProperties == null)
				{
					return null;
				}
				KnowledgeCategoryDef knowledgeCategory = compProperties.knowledgeCategory;
				if (knowledgeCategory == null)
				{
					return null;
				}
				return knowledgeCategory.ToString();
			});
			array[4] = new TableDataGetter<ThingDef>("codex entry", delegate(ThingDef d)
			{
				EntityCodexEntryDef entityCodexEntry = d.entityCodexEntry;
				return ((entityCodexEntry != null) ? entityCodexEntry.label : null) ?? "-";
			});
			array[5] = new TableDataGetter<ThingDef>("combat power", (ThingDef d) => d.race.AnyPawnKind.combatPower);
			array[6] = new TableDataGetter<ThingDef>("min. containment", (ThingDef d) => d.race.AnyPawnKind.race.GetStatValueAbstract(StatDefOf.MinimumContainmentStrength, null).ToString("F0"));
			array[7] = new TableDataGetter<ThingDef>("bioferrite gen.", new Func<ThingDef, string>(DebugOutputsPawns.BioferriteGeneration));
			array[8] = new TableDataGetter<ThingDef>("body size", (ThingDef d) => d.race.baseBodySize);
			array[9] = new TableDataGetter<ThingDef>("health scale", (ThingDef d) => d.race.baseHealthScale);
			array[10] = new TableDataGetter<ThingDef>("speed", (ThingDef d) => d.race.AnyPawnKind.race.GetStatValueAbstract(StatDefOf.MoveSpeed, null));
			array[11] = new TableDataGetter<ThingDef>("lifespan", (ThingDef d) => d.race.lifeExpectancy);
			array[12] = new TableDataGetter<ThingDef>("tempMin", (ThingDef d) => d.race.AnyPawnKind.race.GetStatValueAbstract(StatDefOf.ComfyTemperatureMin, null).ToString("F0"));
			array[13] = new TableDataGetter<ThingDef>("tempMax", (ThingDef d) => d.race.AnyPawnKind.race.GetStatValueAbstract(StatDefOf.ComfyTemperatureMax, null).ToString("F0"));
			array[14] = new TableDataGetter<ThingDef>("flammability", (ThingDef d) => d.race.AnyPawnKind.race.GetStatValueAbstract(StatDefOf.Flammability, null).ToStringPercent());
			array[15] = new TableDataGetter<ThingDef>("meleeDps", (ThingDef d) => DebugOutputsPawns.MeleeDps(d.race.AnyPawnKind).ToString("F1"));
			array[16] = new TableDataGetter<ThingDef>("baseHealthScale", (ThingDef d) => d.race.AnyPawnKind.RaceProps.baseHealthScale.ToString());
			array[17] = new TableDataGetter<ThingDef>("moveSpeed", (ThingDef d) => d.race.AnyPawnKind.race.GetStatValueAbstract(StatDefOf.MoveSpeed, null).ToString());
			array[18] = new TableDataGetter<ThingDef>("averageArmor", (ThingDef d) => DebugOutputsPawns.AverageArmor(d.race.AnyPawnKind).ToStringPercent());
			array[19] = new TableDataGetter<ThingDef>("combatPowerCalculated", (ThingDef d) => DebugOutputsPawns.CombatPowerCalculated(d.race.AnyPawnKind).ToString("F0"));
			DebugTables.MakeTablesDialog<ThingDef>(dataSources, array);
		}

		// Token: 0x0600481C RID: 18460 RVA: 0x001735A0 File Offset: 0x001717A0
		[DebugOutput("Pawns", false)]
		public static void WorldPawnRelations()
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			foreach (WorldPawnSituation worldPawnSituation in (WorldPawnSituation[])Enum.GetValues(typeof(WorldPawnSituation)))
			{
				if (worldPawnSituation != WorldPawnSituation.None)
				{
					foreach (Pawn pawn in Find.WorldPawns.GetPawnsBySituation(worldPawnSituation).OrderBy(delegate(Pawn x)
					{
						Faction faction = x.Faction;
						if (faction == null)
						{
							return -1;
						}
						return faction.loadID;
					}))
					{
						if (pawn.relations != null)
						{
							Pawn local = pawn;
							list.Add(new DebugMenuOption(pawn.LabelShort + " [" + worldPawnSituation.ToString() + "]", DebugMenuOptionMode.Action, delegate()
							{
								StringBuilder stringBuilder = new StringBuilder();
								stringBuilder.AppendLine("Family details for: " + local.Name.ToStringFull);
								stringBuilder.AppendLine("\nDirect relations:");
								foreach (DirectPawnRelation directPawnRelation in local.relations.DirectRelations)
								{
									stringBuilder.AppendLine(string.Format(" - {0} ({1})", directPawnRelation.otherPawn, directPawnRelation.def));
								}
								stringBuilder.AppendLine("\nVirtual relations:");
								foreach (VirtualPawnRelation virtualPawnRelation in local.relations.VirtualRelations)
								{
									stringBuilder.AppendLine(string.Format(" - {0} ({1}, ID: {2}), {3} references", new object[]
									{
										virtualPawnRelation.record.Name,
										virtualPawnRelation.def,
										virtualPawnRelation.record.ID,
										virtualPawnRelation.record.References.Count
									}));
									foreach (Pawn arg in virtualPawnRelation.record.References)
									{
										stringBuilder.AppendLine(string.Format("      - {0}", arg));
									}
								}
								stringBuilder.AppendLine("\nChildren:");
								foreach (Pawn pawn2 in local.relations.Children)
								{
									stringBuilder.AppendLine(" - " + pawn2.LabelShort);
								}
								Log.Message(stringBuilder);
							}));
						}
					}
				}
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list, null));
		}

		// Token: 0x0600481D RID: 18461 RVA: 0x001736B4 File Offset: 0x001718B4
		[DebugOutput("Pawns", false)]
		public static void VirtualRecords()
		{
			StringBuilder stringBuilder = new StringBuilder();
			IReadOnlyDictionary<int, RelationshipRecord> records = Find.RelationshipRecords.Records;
			stringBuilder.AppendLine(string.Format("{0} Records:", records.Count));
			foreach (KeyValuePair<int, RelationshipRecord> keyValuePair in records)
			{
				stringBuilder.AppendLine(string.Format("\n - {0} (ID: {1}), {2} references", keyValuePair.Value.Name, keyValuePair.Value.ID, keyValuePair.Value.References.Count));
				foreach (Pawn arg in keyValuePair.Value.References)
				{
					stringBuilder.AppendLine(string.Format("      - {0}", arg));
				}
			}
			Log.Message(stringBuilder);
		}

		// Token: 0x0600481E RID: 18462 RVA: 0x001737C8 File Offset: 0x001719C8
		private static bool ValidateEntity(ThingDef d)
		{
			if (d.IsCorpse)
			{
				return false;
			}
			RaceProperties race = d.race;
			if (((race != null) ? race.AnyPawnKind : null) == null)
			{
				return false;
			}
			if (d.race.AnyPawnKind is CreepJoinerFormKindDef)
			{
				return false;
			}
			string categoryForPawnKind = DebugToolsSpawning.GetCategoryForPawnKind(d.race.AnyPawnKind);
			return categoryForPawnKind == "Other" || (d.HasComp<CompStudiable>() && d.category == ThingCategory.Pawn && categoryForPawnKind != "Animal" && categoryForPawnKind != "Insect");
		}

		// Token: 0x0600481F RID: 18463 RVA: 0x00173854 File Offset: 0x00171A54
		private static string BioferriteGeneration(ThingDef def)
		{
			float num = 0f;
			if (def.HasComp<CompProducesBioferrite>())
			{
				num = def.GetCompProperties<CompProperties_ProducesBioferrite>().bioferriteDensity;
			}
			else if (def.race.AnyPawnKind.mutant != null)
			{
				num = 1f;
			}
			return (def.race.baseBodySize * num).ToString("F2");
		}

		// Token: 0x06004820 RID: 18464 RVA: 0x001738B0 File Offset: 0x00171AB0
		[DebugOutput]
		public static void AnimalSpecialTrainables()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (TrainableDef trainableDef in from t in DefDatabase<TrainableDef>.AllDefs
			where t.specialTrainable
			select t)
			{
				stringBuilder.AppendLine(trainableDef.label);
				foreach (ThingDef thingDef in DefDatabase<ThingDef>.AllDefs.Where(delegate(ThingDef d)
				{
					if (!d.IsCorpse)
					{
						RaceProperties race = d.race;
						return race != null && race.Animal && race.specialTrainables != null;
					}
					return false;
				}))
				{
					if (thingDef.race.specialTrainables.Contains(trainableDef))
					{
						stringBuilder.AppendLine(" - " + thingDef.label);
					}
				}
			}
			Log.Message(stringBuilder.ToString());
		}

		// Token: 0x06004821 RID: 18465 RVA: 0x001739C4 File Offset: 0x00171BC4
		[CompilerGenerated]
		internal static float <AnimalsBasics>g__pointsGuess|9_0(PawnKindDef d)
		{
			return (15f + DebugOutputsPawns.MeleeDps(d) * 10f) * Mathf.Lerp(1f, d.race.GetStatValueAbstract(StatDefOf.MoveSpeed, null) / 3f, 0.25f) * d.RaceProps.baseHealthScale * GenMath.LerpDouble(0.25f, 1f, 1.65f, 1f, Mathf.Clamp(d.RaceProps.baseBodySize, 0.25f, 1f)) * 0.76f;
		}

		// Token: 0x06004822 RID: 18466 RVA: 0x00173A50 File Offset: 0x00171C50
		[CompilerGenerated]
		internal static float <AnimalsBasics>g__mktValGuess|9_1(PawnKindDef d)
		{
			float num = 18f;
			num += DebugOutputsPawns.<AnimalsBasics>g__pointsGuess|9_0(d) * 2.7f;
			if (d.RaceProps.trainability == TrainabilityDefOf.None)
			{
				num *= 0.5f;
			}
			else if (d.RaceProps.trainability == TrainabilityDefOf.Intermediate)
			{
				num += 0f;
			}
			else
			{
				if (d.RaceProps.trainability != TrainabilityDefOf.Advanced)
				{
					throw new InvalidOperationException();
				}
				num += 250f;
			}
			num += d.RaceProps.baseBodySize * 80f;
			if (d.race.HasComp(typeof(CompMilkable)))
			{
				num += 125f;
			}
			if (d.race.HasComp(typeof(CompShearable)))
			{
				num += 90f;
			}
			if (d.race.HasComp(typeof(CompEggLayer)))
			{
				num += 90f;
			}
			num *= Mathf.Lerp(0.8f, 1.2f, d.race.GetStatValueAbstract(StatDefOf.Wildness, null));
			return num * 0.75f;
		}

		// Token: 0x06004823 RID: 18467 RVA: 0x00173B6C File Offset: 0x00171D6C
		[CompilerGenerated]
		internal static float <Mechanoids>g__weaponDps|14_9(ThingDef d)
		{
			if (d == null || !d.IsRangedWeapon)
			{
				return 0f;
			}
			int burstShotCount = d.Verbs[0].burstShotCount;
			float num = d.Verbs[0].warmupTime;
			num += (float)(burstShotCount - 1) * ((float)d.Verbs[0].ticksBetweenBurstShots / 60f);
			return (float)(((d.Verbs[0].defaultProjectile != null) ? d.Verbs[0].defaultProjectile.projectile.GetDamageAmount(null, null) : 0) * burstShotCount) / num;
		}

		// Token: 0x06004824 RID: 18468 RVA: 0x00173C08 File Offset: 0x00171E08
		[CompilerGenerated]
		internal static float <Mechanoids>g__weaponHitChance|14_10(PawnKindDef k, float distance)
		{
			Pawn pawn = PawnGenerator.GeneratePawn(new PawnGenerationRequest(k, null, PawnGenerationContext.NonPlayer, null, false, false, false, true, false, 1f, false, true, false, true, true, false, false, false, false, 0f, 0f, null, 1f, null, null, null, null, null, null, null, null, null, null, null, null, false, false, false, false, null, null, null, null, null, 0f, DevelopmentalStage.Adult, null, null, null, false, false, false, -1, 0, false));
			float result = 0f;
			ThingWithComps primary = pawn.equipment.Primary;
			if (primary != null && primary.def.IsRangedWeapon)
			{
				result = primary.def.Verbs[0].GetHitChanceFactor(primary, distance);
			}
			Find.WorldPawns.PassToWorld(pawn, PawnDiscardDecideMode.Discard);
			return result;
		}

		// Token: 0x06004825 RID: 18469 RVA: 0x00173CF1 File Offset: 0x00171EF1
		[CompilerGenerated]
		internal static IEnumerable<string> <Mechanoids>g__specialAbilities|14_11(PawnKindDef k)
		{
			if (k.race.HasComp(typeof(CompMechCarrier)))
			{
				yield return "produces war urchins";
			}
			if (k.race.HasComp(typeof(CompShield)))
			{
				yield return "has shield";
			}
			yield break;
		}

		// Token: 0x06004826 RID: 18470 RVA: 0x00173D04 File Offset: 0x00171F04
		[CompilerGenerated]
		internal static float <Mechanoids>g__shooterHitChance|14_12(PawnKindDef k, float distance)
		{
			Pawn pawn = PawnGenerator.GeneratePawn(new PawnGenerationRequest(k, null, PawnGenerationContext.NonPlayer, null, false, false, false, true, false, 1f, false, true, false, true, true, false, false, false, false, 0f, 0f, null, 1f, null, null, null, null, null, null, null, null, null, null, null, null, false, false, false, false, null, null, null, null, null, 0f, DevelopmentalStage.Adult, null, null, null, false, false, false, -1, 0, false));
			float result = ShotReport.HitFactorFromShooter(pawn, distance, null);
			Find.WorldPawns.PassToWorld(pawn, PawnDiscardDecideMode.Discard);
			return result;
		}

		// Token: 0x06004827 RID: 18471 RVA: 0x00173DBD File Offset: 0x00171FBD
		[CompilerGenerated]
		internal static float <Mechanoids>g__combinedHitChance|14_13(PawnKindDef k, float distance)
		{
			return DebugOutputsPawns.<Mechanoids>g__weaponHitChance|14_10(k, distance) * DebugOutputsPawns.<Mechanoids>g__shooterHitChance|14_12(k, distance);
		}

		// Token: 0x06004828 RID: 18472 RVA: 0x00173DD0 File Offset: 0x00171FD0
		[CompilerGenerated]
		internal static TraitDef <TraitsSampled>g__getTrait|24_0(TraitDegreeData d)
		{
			return DefDatabase<TraitDef>.AllDefs.First((TraitDef td) => td.degreeDatas.Contains(d));
		}
	}
}
