using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using LudeonTK;
using RimWorld;
using Verse.Grammar;

namespace Verse
{
	// Token: 0x02000B1C RID: 2844
	public static class DebugOutputsTextGen
	{
		// Token: 0x060049E3 RID: 18915 RVA: 0x00178204 File Offset: 0x00176404
		[DebugOutput("Text generation", false)]
		public static void FlavorfulCombatTest()
		{
			DebugOutputsTextGen.<>c__DisplayClass0_0 CS$<>8__locals1 = new DebugOutputsTextGen.<>c__DisplayClass0_0();
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			CS$<>8__locals1.maneuvers = DefDatabase<ManeuverDef>.AllDefsListForReading;
			DebugOutputsTextGen.<>c__DisplayClass0_0 CS$<>8__locals2 = CS$<>8__locals1;
			Func<ManeuverDef, RulePackDef>[] array = new Func<ManeuverDef, RulePackDef>[5];
			array[0] = ((ManeuverDef m) => new RulePackDef[]
			{
				m.combatLogRulesHit,
				m.combatLogRulesDeflect,
				m.combatLogRulesMiss,
				m.combatLogRulesDodge
			}.RandomElement<RulePackDef>());
			array[1] = ((ManeuverDef m) => m.combatLogRulesHit);
			array[2] = ((ManeuverDef m) => m.combatLogRulesDeflect);
			array[3] = ((ManeuverDef m) => m.combatLogRulesMiss);
			array[4] = ((ManeuverDef m) => m.combatLogRulesDodge);
			CS$<>8__locals2.results = array;
			string[] array2 = new string[]
			{
				"(random)",
				"Hit",
				"Deflect",
				"Miss",
				"Dodge"
			};
			using (IEnumerator<Pair<ManeuverDef, int>> enumerator = CS$<>8__locals1.maneuvers.Concat(null).Cross(Enumerable.Range(0, array2.Length)).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Pair<ManeuverDef, int> maneuverresult = enumerator.Current;
					Action<Action<List<BodyPartRecord>, List<bool>>> <>9__9;
					DebugMenuOption item = new DebugMenuOption(string.Format("{0}/{1}", (maneuverresult.First == null) ? "(random)" : maneuverresult.First.defName, array2[maneuverresult.Second]), DebugMenuOptionMode.Action, delegate()
					{
						Action<Action<List<BodyPartRecord>, List<bool>>> callback;
						if ((callback = <>9__9) == null)
						{
							callback = (<>9__9 = delegate(Action<List<BodyPartRecord>, List<bool>> bodyPartCreator)
							{
								StringBuilder stringBuilder = new StringBuilder();
								for (int i = 0; i < 100; i++)
								{
									ManeuverDef maneuver = maneuverresult.First;
									if (maneuver == null)
									{
										maneuver = CS$<>8__locals1.maneuvers.RandomElement<ManeuverDef>();
									}
									RulePackDef rulePackDef = CS$<>8__locals1.results[maneuverresult.Second](maneuver);
									List<BodyPartRecord> list2 = null;
									List<bool> list3 = null;
									if (rulePackDef == maneuver.combatLogRulesHit)
									{
										list2 = new List<BodyPartRecord>();
										list3 = new List<bool>();
										bodyPartCreator(list2, list3);
									}
									Pair<ThingDef, Tool> pair;
									ImplementOwnerTypeDef implementOwnerTypeDef;
									string toolLabel;
									if (!(from ttp in (from td in DefDatabase<ThingDef>.AllDefsListForReading
									where td.IsMeleeWeapon && !td.tools.NullOrEmpty<Tool>()
									select td).SelectMany((ThingDef td) => from tool in td.tools
									select new Pair<ThingDef, Tool>(td, tool))
									where ttp.Second.capacities.Contains(maneuver.requiredCapacity)
									select ttp).TryRandomElement(out pair))
									{
										string str = "Melee weapon with tool with capacity ";
										ToolCapacityDef requiredCapacity = maneuver.requiredCapacity;
										Log.Warning(str + ((requiredCapacity != null) ? requiredCapacity.ToString() : null) + " not found.");
										implementOwnerTypeDef = ImplementOwnerTypeDefOf.Bodypart;
										toolLabel = "(" + implementOwnerTypeDef.defName + ")";
									}
									else
									{
										implementOwnerTypeDef = ((pair.Second == null) ? ImplementOwnerTypeDefOf.Bodypart : ImplementOwnerTypeDefOf.Weapon);
										toolLabel = ((pair.Second != null) ? pair.Second.label : ("(" + implementOwnerTypeDef.defName + ")"));
									}
									BattleLogEntry_MeleeCombat battleLogEntry_MeleeCombat = new BattleLogEntry_MeleeCombat(rulePackDef, false, DebugOutputsTextGen.RandomPawnForCombat(), DebugOutputsTextGen.RandomPawnForCombat(), implementOwnerTypeDef, toolLabel, pair.First, null, null);
									battleLogEntry_MeleeCombat.FillTargets(list2, list3, battleLogEntry_MeleeCombat.RuleDef.defName.Contains("Deflect"));
									battleLogEntry_MeleeCombat.Debug_OverrideTicks(Rand.Int);
									stringBuilder.AppendLine(battleLogEntry_MeleeCombat.ToGameStringFromPOV(null, false));
								}
								Log.Message(stringBuilder.ToString());
							});
						}
						DebugOutputsTextGen.CreateDamagedDestroyedMenu(callback);
					});
					list.Add(item);
				}
			}
			int rf;
			int rf2;
			for (rf = 0; rf < 2; rf = rf2)
			{
				list.Add(new DebugMenuOption((rf == 0) ? "Ranged fire singleshot" : "Ranged fire burst", DebugMenuOptionMode.Action, delegate()
				{
					StringBuilder stringBuilder = new StringBuilder();
					for (int i = 0; i < 100; i++)
					{
						ThingDef thingDef = (from td in DefDatabase<ThingDef>.AllDefsListForReading
						where td.IsRangedWeapon && td.IsWeaponUsingProjectiles && td.PlayerAcquirable
						select td).RandomElement<ThingDef>();
						bool flag = Rand.Value < 0.2f;
						bool flag2 = !flag && Rand.Value < 0.95f;
						BattleLogEntry_RangedFire battleLogEntry_RangedFire = new BattleLogEntry_RangedFire(DebugOutputsTextGen.RandomPawnForCombat(), flag ? null : DebugOutputsTextGen.RandomPawnForCombat(), flag2 ? null : thingDef, null, rf != 0);
						battleLogEntry_RangedFire.Debug_OverrideTicks(Rand.Int);
						stringBuilder.AppendLine(battleLogEntry_RangedFire.ToGameStringFromPOV(null, false));
					}
					Log.Message(stringBuilder.ToString());
				}));
				rf2 = rf + 1;
			}
			list.Add(new DebugMenuOption("Ranged impact hit", DebugMenuOptionMode.Action, delegate()
			{
				DebugOutputsTextGen.CreateDamagedDestroyedMenu(delegate(Action<List<BodyPartRecord>, List<bool>> bodyPartCreator)
				{
					StringBuilder stringBuilder = new StringBuilder();
					for (int i = 0; i < 100; i++)
					{
						ThingDef weaponDef = (from td in DefDatabase<ThingDef>.AllDefsListForReading
						where td.IsRangedWeapon && td.IsWeaponUsingProjectiles && td.PlayerAcquirable
						select td).RandomElement<ThingDef>();
						List<BodyPartRecord> list2 = new List<BodyPartRecord>();
						List<bool> list3 = new List<bool>();
						bodyPartCreator(list2, list3);
						Pawn pawn = DebugOutputsTextGen.RandomPawnForCombat();
						BattleLogEntry_RangedImpact battleLogEntry_RangedImpact = new BattleLogEntry_RangedImpact(DebugOutputsTextGen.RandomPawnForCombat(), pawn, pawn, weaponDef, null, ThingDefOf.Wall);
						battleLogEntry_RangedImpact.FillTargets(list2, list3, Rand.Chance(0.5f));
						battleLogEntry_RangedImpact.Debug_OverrideTicks(Rand.Int);
						stringBuilder.AppendLine(battleLogEntry_RangedImpact.ToGameStringFromPOV(null, false));
					}
					Log.Message(stringBuilder.ToString());
				});
			}));
			list.Add(new DebugMenuOption("Ranged impact miss", DebugMenuOptionMode.Action, delegate()
			{
				StringBuilder stringBuilder = new StringBuilder();
				for (int i = 0; i < 100; i++)
				{
					ThingDef weaponDef = (from td in DefDatabase<ThingDef>.AllDefsListForReading
					where td.IsRangedWeapon && td.IsWeaponUsingProjectiles && td.PlayerAcquirable
					select td).RandomElement<ThingDef>();
					BattleLogEntry_RangedImpact battleLogEntry_RangedImpact = new BattleLogEntry_RangedImpact(DebugOutputsTextGen.RandomPawnForCombat(), null, DebugOutputsTextGen.RandomPawnForCombat(), weaponDef, null, ThingDefOf.Wall);
					battleLogEntry_RangedImpact.Debug_OverrideTicks(Rand.Int);
					stringBuilder.AppendLine(battleLogEntry_RangedImpact.ToGameStringFromPOV(null, false));
				}
				Log.Message(stringBuilder.ToString());
			}));
			list.Add(new DebugMenuOption("Ranged impact hit incorrect", DebugMenuOptionMode.Action, delegate()
			{
				DebugOutputsTextGen.CreateDamagedDestroyedMenu(delegate(Action<List<BodyPartRecord>, List<bool>> bodyPartCreator)
				{
					StringBuilder stringBuilder = new StringBuilder();
					for (int i = 0; i < 100; i++)
					{
						ThingDef weaponDef = (from td in DefDatabase<ThingDef>.AllDefsListForReading
						where td.IsRangedWeapon && td.IsWeaponUsingProjectiles && td.PlayerAcquirable
						select td).RandomElement<ThingDef>();
						List<BodyPartRecord> list2 = new List<BodyPartRecord>();
						List<bool> list3 = new List<bool>();
						bodyPartCreator(list2, list3);
						BattleLogEntry_RangedImpact battleLogEntry_RangedImpact = new BattleLogEntry_RangedImpact(DebugOutputsTextGen.RandomPawnForCombat(), DebugOutputsTextGen.RandomPawnForCombat(), DebugOutputsTextGen.RandomPawnForCombat(), weaponDef, null, ThingDefOf.Wall);
						battleLogEntry_RangedImpact.FillTargets(list2, list3, Rand.Chance(0.5f));
						battleLogEntry_RangedImpact.Debug_OverrideTicks(Rand.Int);
						stringBuilder.AppendLine(battleLogEntry_RangedImpact.ToGameStringFromPOV(null, false));
					}
					Log.Message(stringBuilder.ToString());
				});
			}));
			using (IEnumerator<RulePackDef> enumerator2 = (from def in DefDatabase<RulePackDef>.AllDefsListForReading
			where def.defName.Contains("Transition") && !def.defName.Contains("Include")
			select def).GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					RulePackDef transition = enumerator2.Current;
					list.Add(new DebugMenuOption(transition.defName, DebugMenuOptionMode.Action, delegate()
					{
						StringBuilder stringBuilder = new StringBuilder();
						for (int i = 0; i < 100; i++)
						{
							Pawn pawn = DebugOutputsTextGen.RandomPawnForCombat();
							Pawn initiator = DebugOutputsTextGen.RandomPawnForCombat();
							BodyPartRecord partRecord = pawn.health.hediffSet.GetNotMissingParts(BodyPartHeight.Undefined, BodyPartDepth.Undefined, null, null).RandomElement<BodyPartRecord>();
							BattleLogEntry_StateTransition battleLogEntry_StateTransition = new BattleLogEntry_StateTransition(pawn, transition, initiator, HediffMaker.MakeHediff(DefDatabase<HediffDef>.AllDefsListForReading.RandomElement<HediffDef>(), pawn, partRecord), pawn.RaceProps.body.AllParts.RandomElement<BodyPartRecord>());
							battleLogEntry_StateTransition.Debug_OverrideTicks(Rand.Int);
							stringBuilder.AppendLine(battleLogEntry_StateTransition.ToGameStringFromPOV(null, false));
						}
						Log.Message(stringBuilder.ToString());
					}));
				}
			}
			using (IEnumerator<RulePackDef> enumerator2 = (from def in DefDatabase<RulePackDef>.AllDefsListForReading
			where def.defName.Contains("DamageEvent") && !def.defName.Contains("Include")
			select def).GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					RulePackDef damageEvent = enumerator2.Current;
					Action<Action<List<BodyPartRecord>, List<bool>>> <>9__25;
					list.Add(new DebugMenuOption(damageEvent.defName, DebugMenuOptionMode.Action, delegate()
					{
						Action<Action<List<BodyPartRecord>, List<bool>>> callback;
						if ((callback = <>9__25) == null)
						{
							callback = (<>9__25 = delegate(Action<List<BodyPartRecord>, List<bool>> bodyPartCreator)
							{
								StringBuilder stringBuilder = new StringBuilder();
								for (int i = 0; i < 100; i++)
								{
									List<BodyPartRecord> list2 = new List<BodyPartRecord>();
									List<bool> list3 = new List<bool>();
									bodyPartCreator(list2, list3);
									BattleLogEntry_DamageTaken battleLogEntry_DamageTaken = new BattleLogEntry_DamageTaken(DebugOutputsTextGen.RandomPawnForCombat(), damageEvent, null);
									battleLogEntry_DamageTaken.FillTargets(list2, list3, false);
									battleLogEntry_DamageTaken.Debug_OverrideTicks(Rand.Int);
									stringBuilder.AppendLine(battleLogEntry_DamageTaken.ToGameStringFromPOV(null, false));
								}
								Log.Message(stringBuilder.ToString());
							});
						}
						DebugOutputsTextGen.CreateDamagedDestroyedMenu(callback);
					}));
				}
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list, null));
		}

		// Token: 0x060049E4 RID: 18916 RVA: 0x001785E8 File Offset: 0x001767E8
		[DebugOutput("Text generation", false)]
		public static void CubeDescriptions()
		{
			if (!ModsConfig.AnomalyActive)
			{
				return;
			}
			List<Tale> list = new List<Tale>();
			for (int i = 0; i < 10; i++)
			{
				list.Add(TaleFactory.MakeRandomTestTale(null));
			}
			List<ValueTuple<string, string>> list2 = new List<ValueTuple<string, string>>();
			for (int j = 0; j < 40; j++)
			{
				TaleReference taleReference = new TaleReference(list[j % 10]);
				TaggedString taggedString = taleReference.GenerateText(TextGenerationPurpose.ArtName, RulePackDefOf.NamerArtCubeSculpture, null, null);
				TaggedString taggedString2 = taleReference.GenerateText(TextGenerationPurpose.ArtDescription, RulePackDefOf.ArtDescription_CubeSculpture, null, null);
				list2.Add(new ValueTuple<string, string>(taggedString, taggedString2));
			}
			IEnumerable<ValueTuple<string, string>> dataSources = list2;
			TableDataGetter<ValueTuple<string, string>>[] array = new TableDataGetter<ValueTuple<string, string>>[2];
			array[0] = new TableDataGetter<ValueTuple<string, string>>("title", ([TupleElementNames(new string[]
			{
				"title",
				"desc"
			})] ValueTuple<string, string> g) => g.Item1);
			array[1] = new TableDataGetter<ValueTuple<string, string>>("description", ([TupleElementNames(new string[]
			{
				"title",
				"desc"
			})] ValueTuple<string, string> g) => g.Item2);
			DebugTables.MakeTablesDialog<ValueTuple<string, string>>(dataSources, array);
		}

		// Token: 0x060049E5 RID: 18917 RVA: 0x001786DC File Offset: 0x001768DC
		[DebugOutput("Text generation", false)]
		public static void VoidSculptureDescriptions()
		{
			if (!ModsConfig.AnomalyActive)
			{
				return;
			}
			List<Tale> list = new List<Tale>();
			for (int i = 0; i < 10; i++)
			{
				list.Add(TaleFactory.MakeRandomTestTale(null));
			}
			List<ValueTuple<string, string>> list2 = new List<ValueTuple<string, string>>();
			for (int j = 0; j < 40; j++)
			{
				TaleReference taleReference = new TaleReference(list[j % 10]);
				TaggedString taggedString = taleReference.GenerateText(TextGenerationPurpose.ArtName, RulePackDefOf.NamerArtVoidSculpture, null, null);
				TaggedString taggedString2 = taleReference.GenerateText(TextGenerationPurpose.ArtDescription, RulePackDefOf.ArtDescription_VoidSculpture, null, null);
				list2.Add(new ValueTuple<string, string>(taggedString, taggedString2));
			}
			IEnumerable<ValueTuple<string, string>> dataSources = list2;
			TableDataGetter<ValueTuple<string, string>>[] array = new TableDataGetter<ValueTuple<string, string>>[2];
			array[0] = new TableDataGetter<ValueTuple<string, string>>("title", ([TupleElementNames(new string[]
			{
				"title",
				"desc"
			})] ValueTuple<string, string> g) => g.Item1);
			array[1] = new TableDataGetter<ValueTuple<string, string>>("description", ([TupleElementNames(new string[]
			{
				"title",
				"desc"
			})] ValueTuple<string, string> g) => g.Item2);
			DebugTables.MakeTablesDialog<ValueTuple<string, string>>(dataSources, array);
		}

		// Token: 0x060049E6 RID: 18918 RVA: 0x001787D0 File Offset: 0x001769D0
		[DebugOutput("Text generation", false)]
		public static void Books()
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			using (List<ThingDef>.Enumerator enumerator = (from x in DefDatabase<ThingDef>.AllDefsListForReading
			where x.HasComp<CompBook>()
			select x).ToList<ThingDef>().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					ThingDef def = enumerator.Current;
					list.Add(new DebugMenuOption(def.defName, DebugMenuOptionMode.Action, delegate()
					{
						DebugOutputsTextGen.BookDefGenerated(def);
					}));
				}
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list, null));
		}

		// Token: 0x060049E7 RID: 18919 RVA: 0x0017888C File Offset: 0x00176A8C
		private static void BookDefGenerated(ThingDef def)
		{
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < 30; i++)
			{
				Book book = BookUtility.MakeBook(def, ArtGenerationContext.Outsider, null);
				stringBuilder.AppendLine(book.Label);
				stringBuilder.AppendLine(book.FlavorUI);
				stringBuilder.AppendLine();
			}
			Log.Message(stringBuilder.ToString());
		}

		// Token: 0x060049E8 RID: 18920 RVA: 0x001788EC File Offset: 0x00176AEC
		public static Pawn RandomPawnForCombat()
		{
			PawnKindDef pawnKindDef = DefDatabase<PawnKindDef>.AllDefsListForReading.RandomElementByWeight(delegate(PawnKindDef pawnkind)
			{
				if (pawnkind.RaceProps.Humanlike)
				{
					return 8f;
				}
				if (pawnkind.RaceProps.IsMechanoid)
				{
					return 8f;
				}
				return 1f;
			});
			Faction faction = FactionUtility.DefaultFactionFrom(pawnKindDef.defaultFactionDef);
			return PawnGenerator.GeneratePawn(pawnKindDef, faction, null);
		}

		// Token: 0x060049E9 RID: 18921 RVA: 0x00178940 File Offset: 0x00176B40
		private static void CreateDamagedDestroyedMenu(Action<Action<List<BodyPartRecord>, List<bool>>> callback)
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			IEnumerable<int> damagedes = Enumerable.Range(0, 5);
			IEnumerable<int> destroyedes = Enumerable.Range(0, 5);
			using (IEnumerator<Pair<int, int>> enumerator = damagedes.Concat(-1).Cross(destroyedes.Concat(-1)).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Pair<int, int> damageddestroyed = enumerator.Current;
					Action<List<BodyPartRecord>, List<bool>> <>9__1;
					DebugMenuOption item = new DebugMenuOption(string.Format("{0} damaged/{1} destroyed", (damageddestroyed.First == -1) ? "(random)" : damageddestroyed.First.ToString(), (damageddestroyed.Second == -1) ? "(random)" : damageddestroyed.Second.ToString()), DebugMenuOptionMode.Action, delegate()
					{
						Action<Action<List<BodyPartRecord>, List<bool>>> callback2 = callback;
						Action<List<BodyPartRecord>, List<bool>> obj;
						if ((obj = <>9__1) == null)
						{
							obj = (<>9__1 = delegate(List<BodyPartRecord> bodyparts, List<bool> flags)
							{
								int num = damageddestroyed.First;
								int destroyed = damageddestroyed.Second;
								if (num == -1)
								{
									num = damagedes.RandomElement<int>();
								}
								if (destroyed == -1)
								{
									destroyed = destroyedes.RandomElement<int>();
								}
								Pair<BodyPartRecord, bool>[] source = (from idx in Enumerable.Range(0, num + destroyed)
								select new Pair<BodyPartRecord, bool>(BodyDefOf.Human.AllParts.RandomElement<BodyPartRecord>(), idx < destroyed)).InRandomOrder(null).ToArray<Pair<BodyPartRecord, bool>>();
								bodyparts.Clear();
								flags.Clear();
								bodyparts.AddRange(from part in source
								select part.First);
								flags.AddRange(from part in source
								select part.Second);
							});
						}
						callback2(obj);
					});
					list.Add(item);
				}
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list, null));
		}

		// Token: 0x060049EA RID: 18922 RVA: 0x00178A6C File Offset: 0x00176C6C
		[DebugOutput("Text generation", false)]
		public static void ArtDescsSpecificTale()
		{
			List<FloatMenuOption> list = new List<FloatMenuOption>();
			foreach (TaleDef localDef2 in from def in DefDatabase<TaleDef>.AllDefs
			orderby def.defName
			select def)
			{
				TaleDef localDef = localDef2;
				FloatMenuOption item = new FloatMenuOption(localDef.defName, delegate()
				{
					DebugOutputsTextGen.LogSpecificTale(localDef, 40);
				}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
				list.Add(item);
			}
			Find.WindowStack.Add(new FloatMenu(list));
		}

		// Token: 0x060049EB RID: 18923 RVA: 0x00178B2C File Offset: 0x00176D2C
		[DebugOutput("Text generation", false)]
		public static void NamesFromRulepack()
		{
			IEnumerable<RulePackDef> enumerable = from d in DefDatabase<RulePackDef>.AllDefsListForReading
			where d.directTestable || d.defName.StartsWith("Namer")
			orderby d.defName
			select d;
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			foreach (RulePackDef localNamer2 in enumerable)
			{
				RulePackDef localNamer = localNamer2;
				list.Add(new DebugMenuOption(localNamer.defName, DebugMenuOptionMode.Action, delegate()
				{
					StringBuilder stringBuilder = new StringBuilder();
					stringBuilder.AppendLine("Testing RulePack " + localNamer.defName + " as a name generator:");
					for (int i = 0; i < 200; i++)
					{
						string testPawnNameSymbol = (i % 2 == 0) ? "Smithee" : null;
						stringBuilder.AppendLine(NameGenerator.GenerateName(localNamer, null, false, localNamer.FirstRuleKeyword, testPawnNameSymbol, null));
					}
					Log.Message(stringBuilder.ToString());
				}));
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list, null));
		}

		// Token: 0x060049EC RID: 18924 RVA: 0x00178C00 File Offset: 0x00176E00
		[DebugOutput("Text generation", false)]
		public static void LandmarkNames()
		{
			List<LandmarkDef> allDefsListForReading = DefDatabase<LandmarkDef>.AllDefsListForReading;
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			using (List<LandmarkDef>.Enumerator enumerator = allDefsListForReading.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					LandmarkDef landmark = enumerator.Current;
					RulePackDef localNamer = landmark.nameMaker;
					list.Add(new DebugMenuOption(landmark.defName, DebugMenuOptionMode.Action, delegate()
					{
						StringBuilder stringBuilder = new StringBuilder();
						stringBuilder.AppendLine("Testing Landmark Namer " + landmark.defName + " using namer " + localNamer.defName);
						for (int i = 0; i < 200; i++)
						{
							string testPawnNameSymbol = (i % 2 == 0) ? "Smithee" : null;
							stringBuilder.AppendLine(NameGenerator.GenerateName(localNamer, null, false, "r_name", testPawnNameSymbol, null));
						}
						Log.Message(stringBuilder.ToString());
					}));
				}
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list, null));
		}

		// Token: 0x060049ED RID: 18925 RVA: 0x00178CA4 File Offset: 0x00176EA4
		[DebugOutput("Text generation", true)]
		public static void DatabaseTalesList()
		{
			Find.TaleManager.LogTales();
		}

		// Token: 0x060049EE RID: 18926 RVA: 0x00178CB0 File Offset: 0x00176EB0
		[DebugOutput("Text generation", true)]
		public static void DatabaseTalesInterest()
		{
			Find.TaleManager.LogTaleInterestSummary();
		}

		// Token: 0x060049EF RID: 18927 RVA: 0x00178CBC File Offset: 0x00176EBC
		[DebugOutput("Text generation", true)]
		public static void ArtDescsDatabaseTales()
		{
			DebugOutputsTextGen.LogTales(from t in Find.TaleManager.AllTalesListForReading
			where t.def.usableForArt
			select t);
		}

		// Token: 0x060049F0 RID: 18928 RVA: 0x00178CF4 File Offset: 0x00176EF4
		[DebugOutput("Text generation", true)]
		public static void ArtDescsRandomTales()
		{
			int num = 40;
			List<Tale> list = new List<Tale>();
			for (int i = 0; i < num; i++)
			{
				list.Add(TaleFactory.MakeRandomTestTale(null));
			}
			DebugOutputsTextGen.LogTales(list);
		}

		// Token: 0x060049F1 RID: 18929 RVA: 0x00178D28 File Offset: 0x00176F28
		[DebugOutput("Text generation", true)]
		public static void ArtDescsTaleless()
		{
			List<Tale> list = new List<Tale>();
			for (int i = 0; i < 20; i++)
			{
				list.Add(null);
			}
			DebugOutputsTextGen.LogTales(list);
		}

		// Token: 0x060049F2 RID: 18930 RVA: 0x00178D58 File Offset: 0x00176F58
		[DebugOutput("Text generation", false)]
		public static void InteractionLogs()
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			using (List<InteractionDef>.Enumerator enumerator = DefDatabase<InteractionDef>.AllDefsListForReading.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					InteractionDef def = enumerator.Current;
					list.Add(new DebugMenuOption(def.defName, DebugMenuOptionMode.Action, delegate()
					{
						StringBuilder stringBuilder = new StringBuilder();
						Pawn pawn = PawnGenerator.GeneratePawn(PawnKindDefOf.Colonist, null, null);
						Pawn recipient = PawnGenerator.GeneratePawn(PawnKindDefOf.Colonist, null, null);
						for (int i = 0; i < 100; i++)
						{
							PlayLogEntry_Interaction playLogEntry_Interaction = new PlayLogEntry_Interaction(def, pawn, recipient, null);
							stringBuilder.AppendLine(playLogEntry_Interaction.ToGameStringFromPOV(pawn, false));
						}
						Log.Message(stringBuilder.ToString());
					}));
				}
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list, null));
		}

		// Token: 0x060049F3 RID: 18931 RVA: 0x00178DE8 File Offset: 0x00176FE8
		private static void LogSpecificTale(TaleDef def, int count)
		{
			List<Tale> list = new List<Tale>();
			for (int i = 0; i < count; i++)
			{
				list.Add(TaleFactory.MakeRandomTestTale(def));
			}
			DebugOutputsTextGen.LogTales(list);
		}

		// Token: 0x060049F4 RID: 18932 RVA: 0x00178E1C File Offset: 0x0017701C
		private static void LogTales(IEnumerable<Tale> tales)
		{
			StringBuilder stringBuilder = new StringBuilder();
			int num = 0;
			foreach (Tale tale in tales)
			{
				TaleReference tr = new TaleReference(tale);
				stringBuilder.AppendLine(DebugOutputsTextGen.RandomArtworkName(tr));
				stringBuilder.AppendLine(DebugOutputsTextGen.RandomArtworkDescription(tr));
				stringBuilder.AppendLine();
				num++;
				if (num % 20 == 0)
				{
					Log.Message(stringBuilder.ToString());
					stringBuilder = new StringBuilder();
				}
			}
			if (!stringBuilder.ToString().NullOrEmpty())
			{
				Log.Message(stringBuilder.ToString());
			}
		}

		// Token: 0x060049F5 RID: 18933 RVA: 0x00178EC0 File Offset: 0x001770C0
		private static string RandomArtworkName(TaleReference tr)
		{
			RulePackDef extraInclude = null;
			switch (Rand.RangeInclusive(0, 4))
			{
			case 0:
				extraInclude = RulePackDefOf.NamerArtSculpture;
				break;
			case 1:
				extraInclude = RulePackDefOf.NamerArtWeaponMelee;
				break;
			case 2:
				extraInclude = RulePackDefOf.NamerArtWeaponGun;
				break;
			case 3:
				extraInclude = RulePackDefOf.NamerArtFurniture;
				break;
			case 4:
				extraInclude = RulePackDefOf.NamerArtSarcophagusPlate;
				break;
			}
			return tr.GenerateText(TextGenerationPurpose.ArtName, extraInclude, null, null);
		}

		// Token: 0x060049F6 RID: 18934 RVA: 0x00178F28 File Offset: 0x00177128
		private static string RandomArtworkDescription(TaleReference tr)
		{
			RulePackDef extraInclude = null;
			switch (Rand.RangeInclusive(0, 4))
			{
			case 0:
				extraInclude = RulePackDefOf.ArtDescription_Sculpture;
				break;
			case 1:
				extraInclude = RulePackDefOf.ArtDescription_WeaponMelee;
				break;
			case 2:
				extraInclude = RulePackDefOf.ArtDescription_WeaponGun;
				break;
			case 3:
				extraInclude = RulePackDefOf.ArtDescription_Furniture;
				break;
			case 4:
				extraInclude = RulePackDefOf.ArtDescription_SarcophagusPlate;
				break;
			}
			return tr.GenerateText(TextGenerationPurpose.ArtDescription, extraInclude, null, null);
		}

		// Token: 0x060049F7 RID: 18935 RVA: 0x00178F90 File Offset: 0x00177190
		[DebugOutput("Text generation", false)]
		private static void WordCount()
		{
			DebugOutputsTextGen.<>c__DisplayClass20_0 CS$<>8__locals1;
			CS$<>8__locals1.words = 0;
			CS$<>8__locals1.wordRegex = new Regex("\\w(?![^{]*})(?![^[]*])[^ \\W]*");
			CS$<>8__locals1.results = new HashSet<string>();
			foreach (KeyValuePair<string, LoadedLanguage.KeyedReplacement> keyValuePair in LanguageDatabase.activeLanguage.keyedReplacements)
			{
				DebugOutputsTextGen.<WordCount>g__TryAdd|20_1(keyValuePair.Value.value, ref CS$<>8__locals1);
			}
			foreach (Type type in GenDefDatabase.AllDefTypesWithDatabases())
			{
				foreach (Def def in GenDefDatabase.GetAllDefsInDatabaseForDef(type))
				{
					if (!def.generated && !(def is RulePackDef))
					{
						foreach (FieldInfo fieldInfo in type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
						{
							if (fieldInfo.HasAttribute<MustTranslateAttribute>() || fieldInfo.HasAttribute<MayTranslateAttribute>())
							{
								DebugOutputsTextGen.<WordCount>g__ProcessField|20_2(fieldInfo.GetValue(def), ref CS$<>8__locals1);
							}
						}
					}
				}
			}
			foreach (RulePackDef rulePackDef in DefDatabase<RulePackDef>.AllDefsListForReading)
			{
				List<Rule> rulesImmediate = rulePackDef.RulesImmediate;
				if (!rulesImmediate.NullOrEmpty<Rule>())
				{
					foreach (Rule rule in rulesImmediate)
					{
						Rule_String rule_String = rule as Rule_String;
						if (rule_String != null)
						{
							string text = DebugOutputsTextGen.<WordCount>g__ProcessString|20_0(rule_String.Generate());
							if (CS$<>8__locals1.results.Add(text))
							{
								CS$<>8__locals1.words += CS$<>8__locals1.wordRegex.Matches(text).Count;
							}
						}
					}
				}
			}
			Log.Message(string.Format("Word count: {0}", CS$<>8__locals1.words));
		}

		// Token: 0x060049F8 RID: 18936 RVA: 0x001791D4 File Offset: 0x001773D4
		[CompilerGenerated]
		internal static string <WordCount>g__ProcessString|20_0(string str)
		{
			return str.Flatten().Replace("-", "");
		}

		// Token: 0x060049F9 RID: 18937 RVA: 0x001791EC File Offset: 0x001773EC
		[CompilerGenerated]
		internal static void <WordCount>g__TryAdd|20_1(string str, ref DebugOutputsTextGen.<>c__DisplayClass20_0 A_1)
		{
			if (str.NullOrEmpty())
			{
				return;
			}
			string text = DebugOutputsTextGen.<WordCount>g__ProcessString|20_0(str);
			if (A_1.results.Add(text))
			{
				A_1.words += A_1.wordRegex.Matches(text).Count;
			}
		}

		// Token: 0x060049FA RID: 18938 RVA: 0x00179238 File Offset: 0x00177438
		[CompilerGenerated]
		internal static void <WordCount>g__ProcessField|20_2(object obj, ref DebugOutputsTextGen.<>c__DisplayClass20_0 A_1)
		{
			if (obj == null)
			{
				return;
			}
			string text = obj as string;
			if (text != null)
			{
				DebugOutputsTextGen.<WordCount>g__TryAdd|20_1(text, ref A_1);
				return;
			}
			if (obj is TaggedString)
			{
				DebugOutputsTextGen.<WordCount>g__TryAdd|20_1(((TaggedString)obj).RawText, ref A_1);
				return;
			}
			IEnumerable<string> enumerable = obj as IEnumerable<string>;
			if (enumerable != null)
			{
				using (IEnumerator<string> enumerator = enumerable.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						string str = enumerator.Current;
						DebugOutputsTextGen.<WordCount>g__TryAdd|20_1(str, ref A_1);
					}
					return;
				}
			}
			IEnumerable<TaggedString> enumerable2 = obj as IEnumerable<TaggedString>;
			if (enumerable2 != null)
			{
				foreach (TaggedString taggedString in enumerable2)
				{
					DebugOutputsTextGen.<WordCount>g__TryAdd|20_1(taggedString, ref A_1);
				}
			}
		}
	}
}
