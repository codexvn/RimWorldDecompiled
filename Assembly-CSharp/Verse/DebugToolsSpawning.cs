using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using LudeonTK;
using RimWorld;
using RimWorld.Planet;
using RimWorld.QuestGen;
using Verse.AI.Group;

namespace Verse;

public static class DebugToolsSpawning
{
	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass24_0
	{
		private sealed class _003C_003CCreateMealWithSpecifics_003Eg__GetIngredientOptions_007C2_003Ed : IEnumerable<DebugMenuOption>, IEnumerable, IEnumerator<DebugMenuOption>, IEnumerator, IDisposable
		{
			private int _003C_003E1__state;

			private DebugMenuOption _003C_003E2__current;

			private int _003C_003El__initialThreadId;

			public _003C_003Ec__DisplayClass24_0 _003C_003E4__this;

			private IEnumerator<ThingDef> _003C_003E7__wrap1;

			DebugMenuOption IEnumerator<DebugMenuOption>.Current
			{
				[DebuggerHidden]
				get
				{
					return _003C_003E2__current;
				}
			}

			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return _003C_003E2__current;
				}
			}

			[DebuggerHidden]
			public _003C_003CCreateMealWithSpecifics_003Eg__GetIngredientOptions_007C2_003Ed(int _003C_003E1__state)
			{
				this._003C_003E1__state = _003C_003E1__state;
				_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
			}

			[DebuggerHidden]
			void IDisposable.Dispose()
			{
				int num = _003C_003E1__state;
				if (num == -3 || num == 3)
				{
					try
					{
					}
					finally
					{
						_003C_003Em__Finally1();
					}
				}
				_003C_003E7__wrap1 = null;
				_003C_003E1__state = -2;
			}

			private bool MoveNext()
			{
				try
				{
					int num = _003C_003E1__state;
					_003C_003Ec__DisplayClass24_0 CS_0024_003C_003E8__locals22 = _003C_003E4__this;
					switch (num)
					{
					default:
						return false;
					case 0:
						_003C_003E1__state = -1;
						_003C_003E2__current = new DebugMenuOption("[Finish and place " + CS_0024_003C_003E8__locals22.ingredients.Count + " / " + 3 + "]", DebugMenuOptionMode.Tool, delegate
						{
							Thing thing = ThingMaker.MakeThing(CS_0024_003C_003E8__locals22.mealDef);
							CompIngredients compIngredients = thing.TryGetComp<CompIngredients>();
							for (int i = 0; i < CS_0024_003C_003E8__locals22.ingredients.Count; i++)
							{
								compIngredients.RegisterIngredient(CS_0024_003C_003E8__locals22.ingredients[i]);
							}
							GenPlace.TryPlaceThing(thing, UI.MouseCell(), Find.CurrentMap, ThingPlaceMode.Near, null, null, null);
						});
						_003C_003E1__state = 1;
						return true;
					case 1:
						_003C_003E1__state = -1;
						_003C_003E2__current = new DebugMenuOption("[Clear selections]", DebugMenuOptionMode.Action, delegate
						{
							CS_0024_003C_003E8__locals22.ingredients.Clear();
							Find.WindowStack.Add(new Dialog_DebugOptionListLister(GetIngredientOptions()));
						});
						_003C_003E1__state = 2;
						return true;
					case 2:
						_003C_003E1__state = -1;
						_003C_003E7__wrap1 = CS_0024_003C_003E8__locals22.ingredientDefs.GetEnumerator();
						_003C_003E1__state = -3;
						break;
					case 3:
						_003C_003E1__state = -3;
						break;
					}
					while (_003C_003E7__wrap1.MoveNext())
					{
						ThingDef current = _003C_003E7__wrap1.Current;
						_003C_003Ec__DisplayClass24_1 CS_0024_003C_003E8__locals21 = new _003C_003Ec__DisplayClass24_1
						{
							CS_0024_003C_003E8__locals1 = CS_0024_003C_003E8__locals22,
							ingredient = current
						};
						if (CS_0024_003C_003E8__locals22.ingredients.Count >= 3 || !FoodUtility.MealCanBeMadeFrom(CS_0024_003C_003E8__locals22.mealDef, CS_0024_003C_003E8__locals21.ingredient))
						{
							continue;
						}
						string label = (CS_0024_003C_003E8__locals22.ingredients.Contains(CS_0024_003C_003E8__locals21.ingredient) ? (CS_0024_003C_003E8__locals21.ingredient.defName + " ✓") : CS_0024_003C_003E8__locals21.ingredient.defName);
						_003C_003E2__current = new DebugMenuOption(label, DebugMenuOptionMode.Action, delegate
						{
							if (CS_0024_003C_003E8__locals21.CS_0024_003C_003E8__locals1.ingredients.Contains(CS_0024_003C_003E8__locals21.ingredient))
							{
								CS_0024_003C_003E8__locals21.CS_0024_003C_003E8__locals1.ingredients.Remove(CS_0024_003C_003E8__locals21.ingredient);
							}
							else
							{
								CS_0024_003C_003E8__locals21.CS_0024_003C_003E8__locals1.ingredients.Add(CS_0024_003C_003E8__locals21.ingredient);
							}
							Find.WindowStack.Add(new Dialog_DebugOptionListLister(GetIngredientOptions()));
						});
						_003C_003E1__state = 3;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = null;
					return false;
					[IteratorStateMachine(typeof(_003C_003CCreateMealWithSpecifics_003Eg__GetIngredientOptions_007C2_003Ed))]
					IEnumerable<DebugMenuOption> GetIngredientOptions()
					{
						//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
						return new _003C_003CCreateMealWithSpecifics_003Eg__GetIngredientOptions_007C2_003Ed(-2)
						{
							_003C_003E4__this = CS_0024_003C_003E8__locals22
						};
					}
				}
				catch
				{
					//try-fault
					((IDisposable)this).Dispose();
					throw;
				}
			}

			bool IEnumerator.MoveNext()
			{
				//ILSpy generated this explicit interface implementation from .override directive in MoveNext
				return this.MoveNext();
			}

			private void _003C_003Em__Finally1()
			{
				_003C_003E1__state = -1;
				if (_003C_003E7__wrap1 != null)
				{
					_003C_003E7__wrap1.Dispose();
				}
			}

			[DebuggerHidden]
			void IEnumerator.Reset()
			{
				throw new NotSupportedException();
			}

			[DebuggerHidden]
			IEnumerator<DebugMenuOption> IEnumerable<DebugMenuOption>.GetEnumerator()
			{
				_003C_003CCreateMealWithSpecifics_003Eg__GetIngredientOptions_007C2_003Ed result;
				if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
				{
					_003C_003E1__state = 0;
					result = this;
				}
				else
				{
					result = new _003C_003CCreateMealWithSpecifics_003Eg__GetIngredientOptions_007C2_003Ed(0)
					{
						_003C_003E4__this = _003C_003E4__this
					};
				}
				return result;
			}

			[DebuggerHidden]
			IEnumerator IEnumerable.GetEnumerator()
			{
				return ((IEnumerable<DebugMenuOption>)this).GetEnumerator();
			}
		}

		public List<ThingDef> ingredients;

		public ThingDef mealDef;

		public IEnumerable<ThingDef> ingredientDefs;

		internal void _003CCreateMealWithSpecifics_003Eb__3()
		{
			Thing thing = ThingMaker.MakeThing(mealDef);
			CompIngredients compIngredients = thing.TryGetComp<CompIngredients>();
			for (int i = 0; i < ingredients.Count; i++)
			{
				compIngredients.RegisterIngredient(ingredients[i]);
			}
			GenPlace.TryPlaceThing(thing, UI.MouseCell(), Find.CurrentMap, ThingPlaceMode.Near, null, null, null);
		}

		internal void _003CCreateMealWithSpecifics_003Eb__4()
		{
			ingredients.Clear();
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(GetIngredientOptions()));
			[IteratorStateMachine(typeof(_003C_003CCreateMealWithSpecifics_003Eg__GetIngredientOptions_007C2_003Ed))]
			IEnumerable<DebugMenuOption> GetIngredientOptions()
			{
				//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
				return new _003C_003CCreateMealWithSpecifics_003Eg__GetIngredientOptions_007C2_003Ed(-2)
				{
					_003C_003E4__this = this
				};
			}
		}
	}

	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass24_1
	{
		public ThingDef ingredient;

		public _003C_003Ec__DisplayClass24_0 CS_0024_003C_003E8__locals1;

		internal void _003CCreateMealWithSpecifics_003Eb__5()
		{
			if (CS_0024_003C_003E8__locals1.ingredients.Contains(ingredient))
			{
				CS_0024_003C_003E8__locals1.ingredients.Remove(ingredient);
			}
			else
			{
				CS_0024_003C_003E8__locals1.ingredients.Add(ingredient);
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(GetIngredientOptions()));
			[IteratorStateMachine(typeof(_003C_003Ec__DisplayClass24_0._003C_003CCreateMealWithSpecifics_003Eg__GetIngredientOptions_007C2_003Ed))]
			IEnumerable<DebugMenuOption> GetIngredientOptions()
			{
				//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
				return new _003C_003Ec__DisplayClass24_0._003C_003CCreateMealWithSpecifics_003Eg__GetIngredientOptions_007C2_003Ed(-2)
				{
					_003C_003E4__this = this
				};
			}
		}
	}

	[CompilerGenerated]
	private sealed class _003CPointsMechCluster_003Ed__26 : IEnumerable<float>, IEnumerable, IEnumerator<float>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private float _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private float _003Cpoints_003E5__2;

		float IEnumerator<float>.Current
		{
			[DebuggerHidden]
			get
			{
				return _003C_003E2__current;
			}
		}

		object IEnumerator.Current
		{
			[DebuggerHidden]
			get
			{
				return _003C_003E2__current;
			}
		}

		[DebuggerHidden]
		public _003CPointsMechCluster_003Ed__26(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			switch (_003C_003E1__state)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				_003Cpoints_003E5__2 = 50f;
				break;
			case 1:
				_003C_003E1__state = -1;
				_003Cpoints_003E5__2 += 50f;
				break;
			}
			if (_003Cpoints_003E5__2 <= 10000f)
			{
				_003C_003E2__current = _003Cpoints_003E5__2;
				_003C_003E1__state = 1;
				return true;
			}
			return false;
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<float> IEnumerable<float>.GetEnumerator()
		{
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				return this;
			}
			return new _003CPointsMechCluster_003Ed__26(0);
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<float>)this).GetEnumerator();
		}
	}

	private static readonly List<Def> excluded = new List<Def>();

	private static readonly List<Def> requires = new List<Def>();

	private static readonly float[] MarketValues = new float[3] { 1000f, 10000f, 100000f };

	[DebugAction("Spawning", null, false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap, displayPriority = 1000)]
	private static List<DebugActionNode> SpawnPawn()
	{
		List<DebugActionNode> list = new List<DebugActionNode>();
		foreach (PawnKindDef item in from x in DefDatabase<PawnKindDef>.AllDefs
			where x.showInDebugSpawner
			select x into kd
			orderby kd.defName
			select kd)
		{
			PawnKindDef localKindDef = item;
			list.Add(new DebugActionNode(localKindDef.defName, DebugActionType.ToolMap)
			{
				category = GetCategoryForPawnKind(localKindDef),
				action = delegate
				{
					Faction faction = FactionUtility.DefaultFactionFrom(localKindDef.defaultFactionDef);
					Pawn pawn = PawnGenerator.GeneratePawn(localKindDef, faction, Find.CurrentMap.Tile);
					GenSpawn.Spawn(pawn, UI.MouseCell(), Find.CurrentMap);
					PostPawnSpawn(pawn);
				}
			});
		}
		return list;
	}

	[DebugAction("Spawning", null, false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap, displayPriority = 1000)]
	private static List<DebugActionNode> SpawnPawnWithLifestage()
	{
		List<DebugActionNode> list = new List<DebugActionNode>();
		foreach (PawnKindDef item in from x in DefDatabase<PawnKindDef>.AllDefs
			where x.showInDebugSpawner
			select x into kd
			orderby kd.defName
			select kd)
		{
			PawnKindDef localKindDef = item;
			DebugActionNode debugActionNode = new DebugActionNode(localKindDef.defName, DebugActionType.ToolMap)
			{
				category = GetCategoryForPawnKind(localKindDef)
			};
			for (int i = 0; i < localKindDef.RaceProps.lifeStageAges.Count; i++)
			{
				LifeStageAge raceStage = localKindDef.RaceProps.lifeStageAges[i];
				debugActionNode.AddChild(new DebugActionNode(raceStage.def.defName, DebugActionType.ToolMap)
				{
					action = delegate
					{
						Pawn pawn = PawnGenerator.GeneratePawn(localKindDef, FactionUtility.DefaultFactionFrom(localKindDef.defaultFactionDef), null);
						pawn.ageTracker.AgeBiologicalTicks = (long)(raceStage.minAge * 3600000f);
						GenSpawn.Spawn(pawn, UI.MouseCell(), Find.CurrentMap);
						PostPawnSpawn(pawn);
					}
				});
			}
			list.Add(debugActionNode);
		}
		return list;
	}

	[DebugAction("Anomaly", "Generate CreepJoiner...", false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap, requiresAnomaly = true)]
	private static void GenerateCreeperJoiner()
	{
		requires.Clear();
		excluded.Clear();
		Slate slate = new Slate();
		ShowOptions(slate, "form", DefDatabase<CreepJoinerFormKindDef>.AllDefs, requires, excluded, delegate
		{
			ShowOptions(slate, "benefit", DefDatabase<CreepJoinerBenefitDef>.AllDefs, requires, excluded, delegate
			{
				ShowOptions(slate, "downside", DefDatabase<CreepJoinerDownsideDef>.AllDefs, requires, excluded, delegate
				{
					ShowOptions(slate, "aggressive", DefDatabase<CreepJoinerAggressiveDef>.AllDefs, requires, excluded, delegate
					{
						ShowOptions(slate, "rejection", DefDatabase<CreepJoinerRejectionDef>.AllDefs, requires, excluded, delegate
						{
							Quest quest = QuestGen.Generate(QuestScriptDefOf.CreepJoinerArrival, slate);
							Find.QuestManager.Add(quest);
							requires.Clear();
							excluded.Clear();
						});
					});
				});
			});
		});
	}

	private static void ShowOptions<T>(Slate slate, string key, IEnumerable<T> defs, List<Def> requires, List<Def> excluded, Action selected) where T : Def, ICreepJoinerDef
	{
		List<DebugMenuOption> list = new List<DebugMenuOption>();
		float num = StorytellerUtility.DefaultThreatPointsNow(Find.CurrentMap);
		foreach (T def in defs)
		{
			DebugMenuOption item = new DebugMenuOption(def.label, DebugMenuOptionMode.Action, delegate
			{
				if (!excluded.Contains(def))
				{
					slate.Set(key, def);
					excluded.AddRange(def.Excludes);
					requires.AddRange(def.Requires);
					selected?.Invoke();
				}
			});
			if (excluded.Contains(def))
			{
				item.label += " [NO]";
			}
			else if (num < def.MinCombatPoints)
			{
				item.label += " [PNTS LOW]";
			}
			else if (requires.Contains(def))
			{
				item.label += " [REQ]";
			}
			list.Add(item);
		}
		Find.WindowStack.Add(new Dialog_DebugOptionListLister(list, "Choose " + key));
	}

	[DebugAction("Spawning", null, false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap, displayPriority = 1000, requiresBiotech = true)]
	private static List<DebugActionNode> SpawnNewborn()
	{
		return SpawnAtDevelopmentalStages(DevelopmentalStage.Newborn);
	}

	[DebugAction("Spawning", null, false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap, displayPriority = 1000, requiresBiotech = true)]
	private static List<DebugActionNode> SpawnChild()
	{
		return SpawnAtDevelopmentalStages(DevelopmentalStage.Child);
	}

	private static List<DebugActionNode> SpawnAtDevelopmentalStages(DevelopmentalStage stages)
	{
		List<DebugActionNode> list = new List<DebugActionNode>();
		foreach (PawnKindDef item in DefDatabase<PawnKindDef>.AllDefs.OrderBy((PawnKindDef kd) => kd.defName))
		{
			PawnKindDef localKindDef = item;
			list.Add(new DebugActionNode(localKindDef.defName, DebugActionType.ToolMap)
			{
				category = GetCategoryForPawnKind(localKindDef),
				action = delegate
				{
					Faction faction = FactionUtility.DefaultFactionFrom(localKindDef.defaultFactionDef);
					PawnKindDef kind = localKindDef;
					DevelopmentalStage developmentalStages = stages;
					Pawn pawn = PawnGenerator.GeneratePawn(new PawnGenerationRequest(kind, faction, PawnGenerationContext.NonPlayer, null, forceGenerateNewPawn: false, allowDead: false, allowDowned: true, canGeneratePawnRelations: true, mustBeCapableOfViolence: false, 1f, forceAddFreeWarmLayerIfNeeded: false, allowGay: true, allowPregnant: false, allowFood: true, allowAddictions: true, inhabitant: false, certainlyBeenInCryptosleep: false, forceRedressWorldPawnIfFormerColonist: false, worldPawnFactionDoesntMatter: false, 0f, 0f, null, 1f, null, null, null, null, null, null, null, null, null, null, null, null, forceNoIdeo: false, forceNoBackstory: false, forbidAnyTitle: false, forceDead: false, null, null, null, null, null, 0f, developmentalStages, null, null, null));
					GenSpawn.Spawn(pawn, UI.MouseCell(), Find.CurrentMap);
					PostPawnSpawn(pawn);
				}
			});
		}
		return list;
	}

	public static string GetCategoryForPawnKind(PawnKindDef kindDef)
	{
		if (!kindDef.overrideDebugActionCategory.NullOrEmpty())
		{
			return kindDef.overrideDebugActionCategory;
		}
		if (kindDef.RaceProps.Humanlike)
		{
			return "Humanlike";
		}
		if (kindDef.RaceProps.Insect)
		{
			return "Insect";
		}
		if (kindDef.RaceProps.IsMechanoid)
		{
			return "Mechanoid";
		}
		if (kindDef.RaceProps.Animal)
		{
			return "Animal";
		}
		return "Other";
	}

	private static void PostPawnSpawn(Pawn pawn)
	{
		if (pawn.Spawned && pawn.Faction != null && pawn.Faction != Faction.OfPlayer)
		{
			Lord lord = null;
			if (pawn.Map.mapPawns.SpawnedPawnsInFaction(pawn.Faction).Any((Pawn p) => p != pawn))
			{
				lord = ((Pawn)GenClosest.ClosestThing_Global(pawn.Position, pawn.Map.mapPawns.SpawnedPawnsInFaction(pawn.Faction), 99999f, (Thing p) => p != pawn && ((Pawn)p).GetLord() != null))?.GetLord();
			}
			if (lord == null || !lord.CanAddPawn(pawn))
			{
				lord = LordMaker.MakeNewLord(pawn.Faction, new LordJob_DefendPoint(pawn.Position, null, null), Find.CurrentMap);
			}
			if (lord != null && lord.LordJob.CanAutoAddPawns)
			{
				lord.AddPawn(pawn);
			}
		}
		pawn.Rotation = Rot4.South;
	}

	[DebugAction("Spawning", "Spawn thing", false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap, displayPriority = 1000)]
	private static List<DebugActionNode> TryPlaceNearThing()
	{
		return (from x in DebugThingPlaceHelper.TryPlaceOptionsForStackCount(1, direct: false)
			orderby x.label
			select x).ToList();
	}

	[DebugAction("Spawning", "Spawn thing with style", false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap)]
	private static List<DebugActionNode> TryPlaceNearThingWithStyle()
	{
		List<DebugActionNode> list = new List<DebugActionNode>();
		foreach (ThingDef item in DefDatabase<ThingDef>.AllDefs.OrderBy((ThingDef d) => d.defName))
		{
			ThingDef localDef = item;
			if (localDef.randomStyle.NullOrEmpty() && !DefDatabase<StyleCategoryDef>.AllDefs.Any((StyleCategoryDef s) => s.GetStyleForThingDef(localDef) != null))
			{
				continue;
			}
			DebugActionNode debugActionNode = new DebugActionNode(localDef.LabelCap);
			debugActionNode.AddChild(new DebugActionNode("Standard", DebugActionType.ToolMap, delegate
			{
				DebugThingPlaceHelper.DebugSpawn(localDef, UI.MouseCell(), -1, direct: false, null, canBeMinified: true, null);
			}));
			foreach (StyleCategoryDef cat in DefDatabase<StyleCategoryDef>.AllDefs.Where((StyleCategoryDef x) => x.thingDefStyles.Any((ThingDefStyle y) => y.ThingDef == localDef)))
			{
				StyleCategoryDef styleCategoryDef = cat;
				debugActionNode.AddChild(new DebugActionNode(styleCategoryDef.defName, DebugActionType.ToolMap, delegate
				{
					DebugThingPlaceHelper.DebugSpawn(localDef, UI.MouseCell(), -1, direct: false, cat.GetStyleForThingDef(localDef), canBeMinified: true, null);
				}));
			}
			if (localDef.randomStyle != null)
			{
				foreach (ThingStyleChance item2 in localDef.randomStyle)
				{
					ThingStyleChance style = item2;
					debugActionNode.AddChild(new DebugActionNode(style.StyleDef.defName, DebugActionType.ToolMap, delegate
					{
						DebugThingPlaceHelper.DebugSpawn(localDef, UI.MouseCell(), -1, direct: false, style.StyleDef, canBeMinified: true, null);
					}));
				}
			}
			list.Add(debugActionNode);
		}
		if (list.Count == 0)
		{
			list.Add(new DebugActionNode("No styleable things", DebugActionType.Action, delegate
			{
			}));
		}
		return list;
	}

	[DebugAction("Spawning", "Spawn unminified thing", false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap)]
	private static List<DebugActionNode> TryPlaceMinifiedThing()
	{
		return (from x in DebugThingPlaceHelper.TryPlaceOptionsUnminified()
			orderby x.label
			select x).ToList();
	}

	[DebugAction("Spawning", "Spawn full thing stack", false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap, displayPriority = 1000)]
	private static List<DebugActionNode> TryPlaceNearFullStack()
	{
		return (from x in DebugThingPlaceHelper.TryPlaceOptionsForStackCount(-1, direct: false)
			orderby x.label
			select x).ToList();
	}

	[DebugAction("Spawning", "Spawn stack of 25", false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap, hideInSubMenu = true)]
	private static List<DebugActionNode> TryPlaceNearStacksOf25()
	{
		return (from x in DebugThingPlaceHelper.TryPlaceOptionsForStackCount(25, direct: false)
			orderby x.label
			select x).ToList();
	}

	[DebugAction("Spawning", "Spawn stack of 75", false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap, hideInSubMenu = true)]
	private static List<DebugActionNode> TryPlaceNearStacksOf75()
	{
		return (from x in DebugThingPlaceHelper.TryPlaceOptionsForStackCount(75, direct: false)
			orderby x.label
			select x).ToList();
	}

	[DebugAction("Spawning", "Try place direct thing", false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap, hideInSubMenu = true)]
	private static List<DebugActionNode> TryPlaceDirectThing()
	{
		return (from x in DebugThingPlaceHelper.TryPlaceOptionsForStackCount(1, direct: true)
			orderby x.label
			select x).ToList();
	}

	[DebugAction("Spawning", "Try place direct full stack", false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap, hideInSubMenu = true)]
	private static List<DebugActionNode> TryPlaceDirectFullStack()
	{
		return (from x in DebugThingPlaceHelper.TryPlaceOptionsForStackCount(-1, direct: true)
			orderby x.label
			select x).ToList();
	}

	[DebugAction("Spawning", "Try place direct stack of 25", false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap, hideInSubMenu = true)]
	private static List<DebugActionNode> TryPlaceDirectStackOf25()
	{
		return (from x in DebugThingPlaceHelper.TryPlaceOptionsForStackCount(25, direct: true)
			orderby x.label
			select x).ToList();
	}

	[DebugAction("Spawning", null, false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap)]
	private static List<DebugActionNode> SpawnWeapon()
	{
		List<DebugActionNode> list = new List<DebugActionNode>();
		foreach (ThingDef item in from def in DefDatabase<ThingDef>.AllDefs
			where def.equipmentType == EquipmentType.Primary
			select def into d
			orderby d.defName
			select d)
		{
			ThingDef localDef = item;
			list.Add(new DebugActionNode(localDef.defName, DebugActionType.ToolMap, delegate
			{
				DebugThingPlaceHelper.DebugSpawn(localDef, UI.MouseCell(), -1, direct: false, null, canBeMinified: true, null);
			}));
		}
		return list;
	}

	[DebugAction("Spawning", "Spawn apparel", false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap)]
	private static List<DebugActionNode> SpawnApparel()
	{
		List<DebugActionNode> list = new List<DebugActionNode>();
		foreach (ThingDef item in from def in DefDatabase<ThingDef>.AllDefs
			where def.IsApparel
			select def into d
			orderby d.defName
			select d)
		{
			ThingDef localDef = item;
			list.Add(new DebugActionNode(localDef.defName, DebugActionType.ToolMap, delegate
			{
				DebugThingPlaceHelper.DebugSpawn(localDef, UI.MouseCell(), -1, direct: false, null, canBeMinified: true, null);
			}));
		}
		return list;
	}

	[DebugAction("Spawning", "Try spawn stack of market value...", false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap)]
	private static List<DebugActionNode> TryPlaceNearMarketValue()
	{
		List<DebugActionNode> list = new List<DebugActionNode>();
		float[] marketValues = MarketValues;
		foreach (float num in marketValues)
		{
			DebugActionNode debugActionNode = new DebugActionNode(num.ToStringMoney());
			foreach (DebugActionNode item in DebugThingPlaceHelper.TryPlaceOptionsForBaseMarketValue(num, direct: false))
			{
				debugActionNode.AddChild(item);
			}
			list.Add(debugActionNode);
		}
		return list;
	}

	[DebugAction("Spawning", "Spawn meal with specifics...", false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap)]
	private static void CreateMealWithSpecifics()
	{
		_003C_003Ec__DisplayClass24_0 CS_0024_003C_003E8__locals5 = new _003C_003Ec__DisplayClass24_0();
		IEnumerable<ThingDef> enumerable = DefDatabase<ThingDef>.AllDefs.Where((ThingDef x) => x.IsNutritionGivingIngestible && x.ingestible.IsMeal);
		CS_0024_003C_003E8__locals5.ingredientDefs = DefDatabase<ThingDef>.AllDefs.Where((ThingDef x) => x.IsNutritionGivingIngestible && x.ingestible.HumanEdible && !x.ingestible.IsMeal && !x.IsCorpse);
		CS_0024_003C_003E8__locals5.mealDef = null;
		CS_0024_003C_003E8__locals5.ingredients = new List<ThingDef>();
		List<DebugMenuOption> list = new List<DebugMenuOption>();
		using (IEnumerator<ThingDef> enumerator = enumerable.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				_003C_003Ec__DisplayClass24_0 _003C_003Ec__DisplayClass24_ = CS_0024_003C_003E8__locals5;
				ThingDef d = enumerator.Current;
				list.Add(new DebugMenuOption(d.defName, DebugMenuOptionMode.Action, delegate
				{
					_003C_003Ec__DisplayClass24_.mealDef = d;
					Find.WindowStack.Add(new Dialog_DebugOptionListLister(GetIngredientOptions()));
				}));
			}
		}
		Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
		[IteratorStateMachine(typeof(_003C_003Ec__DisplayClass24_0._003C_003CCreateMealWithSpecifics_003Eg__GetIngredientOptions_007C2_003Ed))]
		IEnumerable<DebugMenuOption> GetIngredientOptions()
		{
			//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
			return new _003C_003Ec__DisplayClass24_0._003C_003CCreateMealWithSpecifics_003Eg__GetIngredientOptions_007C2_003Ed(-2)
			{
				_003C_003E4__this = CS_0024_003C_003E8__locals5
			};
		}
	}

	[DebugAction("Spawning", "Spawn thing with wipe mode", false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap, hideInSubMenu = true)]
	private static DebugActionNode SpawnThingWithWipeMode()
	{
		DebugActionNode debugActionNode = new DebugActionNode();
		WipeMode[] array = (WipeMode[])Enum.GetValues(typeof(WipeMode));
		for (int i = 0; i < array.Length; i++)
		{
			WipeMode wipeMode = array[i];
			WipeMode localWipeMode = wipeMode;
			DebugActionNode debugActionNode2 = new DebugActionNode(wipeMode.ToString());
			debugActionNode2.childGetter = () => DebugThingPlaceHelper.SpawnOptions(localWipeMode);
			debugActionNode.AddChild(debugActionNode2);
		}
		return debugActionNode;
	}

	[IteratorStateMachine(typeof(_003CPointsMechCluster_003Ed__26))]
	private static IEnumerable<float> PointsMechCluster()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CPointsMechCluster_003Ed__26(-2);
	}

	[DebugAction("Spawning", null, false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap, requiresRoyalty = true)]
	private static DebugActionNode SpawnMechCluster()
	{
		DebugActionNode debugActionNode = new DebugActionNode();
		foreach (float item in PointsMechCluster())
		{
			float localPoints = item;
			DebugActionNode debugActionNode2 = new DebugActionNode(localPoints + " points");
			debugActionNode2.AddChild(new DebugActionNode("In pods, click place", DebugActionType.ToolMap, delegate
			{
				MechClusterSketch sketch = MechClusterGenerator.GenerateClusterSketch(localPoints, Find.CurrentMap);
				MechClusterUtility.SpawnCluster(UI.MouseCell(), Find.CurrentMap, sketch);
			}));
			debugActionNode2.AddChild(new DebugActionNode("In pods, autoplace", DebugActionType.ToolMap, delegate
			{
				MechClusterSketch sketch2 = MechClusterGenerator.GenerateClusterSketch(localPoints, Find.CurrentMap);
				MechClusterUtility.SpawnCluster(MechClusterUtility.FindClusterPosition(Find.CurrentMap, sketch2), Find.CurrentMap, sketch2);
			}));
			debugActionNode2.AddChild(new DebugActionNode("Direct spawn, click place", DebugActionType.ToolMap, delegate
			{
				MechClusterSketch sketch3 = MechClusterGenerator.GenerateClusterSketch(localPoints, Find.CurrentMap);
				MechClusterUtility.SpawnCluster(UI.MouseCell(), Find.CurrentMap, sketch3, dropInPods: false);
			}));
			debugActionNode2.AddChild(new DebugActionNode("Direct spawn, autoplace", DebugActionType.Action, delegate
			{
				MechClusterSketch sketch4 = MechClusterGenerator.GenerateClusterSketch(localPoints, Find.CurrentMap);
				MechClusterUtility.SpawnCluster(MechClusterUtility.FindClusterPosition(Find.CurrentMap, sketch4), Find.CurrentMap, sketch4, dropInPods: false);
			}));
			debugActionNode.AddChild(debugActionNode2);
		}
		debugActionNode.visibilityGetter = () => Faction.OfMechanoids != null;
		return debugActionNode;
	}

	[DebugAction("Spawning", "Make filth x100", false, false, false, false, false, 0, false, actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap, hideInSubMenu = true)]
	private static void MakeFilthx100()
	{
		for (int i = 0; i < 100; i++)
		{
			IntVec3 c = UI.MouseCell() + GenRadial.RadialPattern[i];
			if (c.InBounds(Find.CurrentMap) && c.WalkableByAny(Find.CurrentMap))
			{
				FilthMaker.TryMakeFilth(c, Find.CurrentMap, ThingDefOf.Filth_Dirt, 2);
				FleckMaker.ThrowMetaPuff(c.ToVector3Shifted(), Find.CurrentMap);
			}
		}
	}

	[DebugAction("Spawning", null, false, false, false, false, false, 0, false, actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap)]
	private static void SpawnFactionLeader()
	{
		List<FloatMenuOption> list = new List<FloatMenuOption>();
		foreach (Faction allFaction in Find.FactionManager.AllFactions)
		{
			Faction localFac = allFaction;
			if (localFac.leader != null)
			{
				list.Add(new FloatMenuOption(localFac.Name + " - " + localFac.leader.Name.ToStringFull, delegate
				{
					GenSpawn.Spawn(localFac.leader, UI.MouseCell(), Find.CurrentMap);
				}));
			}
		}
		Find.WindowStack.Add(new FloatMenu(list));
	}

	[DebugAction("Spawning", "Spawn world pawn...", false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap)]
	private static void SpawnWorldPawn()
	{
		List<DebugMenuOption> list = new List<DebugMenuOption>();
		Action<Pawn> act = delegate(Pawn p)
		{
			List<DebugMenuOption> list2 = new List<DebugMenuOption>();
			foreach (PawnKindDef item in DefDatabase<PawnKindDef>.AllDefs.Where((PawnKindDef x) => x.race == p.def))
			{
				PawnKindDef kLocal = item;
				list2.Add(new DebugMenuOption(kLocal.defName, DebugMenuOptionMode.Tool, delegate
				{
					PawnGenerationRequest request = new PawnGenerationRequest(kLocal, p.Faction, PawnGenerationContext.NonPlayer, null, forceGenerateNewPawn: false, allowDead: false, allowDowned: false, canGeneratePawnRelations: true, mustBeCapableOfViolence: false, 1f, forceAddFreeWarmLayerIfNeeded: false, allowGay: true, allowPregnant: false, allowFood: true, allowAddictions: true, inhabitant: false, certainlyBeenInCryptosleep: false, forceRedressWorldPawnIfFormerColonist: false, worldPawnFactionDoesntMatter: false, 0f, 0f, null, 1f, null, null, null, null, null, null, null, null, null, null, null, null, forceNoIdeo: false, forceNoBackstory: false, forbidAnyTitle: false, forceDead: false, null, null, null, null, null, 0f, DevelopmentalStage.Adult, null, null, null);
					PawnGenerator.RedressPawn(p, request);
					GenSpawn.Spawn(p, UI.MouseCell(), Find.CurrentMap);
					DebugTools.curTool = null;
				}));
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list2));
		};
		foreach (Pawn item2 in Find.WorldPawns.AllPawnsAlive)
		{
			Pawn pLocal = item2;
			list.Add(new DebugMenuOption(item2.LabelShort, DebugMenuOptionMode.Action, delegate
			{
				act(pLocal);
			}));
		}
		Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
	}

	[DebugAction("Spawning", "Spawn thing set", false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap)]
	private static List<DebugActionNode> SpawnThingSet()
	{
		List<DebugActionNode> list = new List<DebugActionNode>();
		foreach (ThingSetMakerDef allDef in DefDatabase<ThingSetMakerDef>.AllDefs)
		{
			ThingSetMakerDef localGen = allDef;
			list.Add(new DebugActionNode(localGen.defName, DebugActionType.ToolMap, delegate
			{
				if (UI.MouseCell().InBounds(Find.CurrentMap))
				{
					StringBuilder stringBuilder = new StringBuilder();
					string nonNullFieldsDebugInfo = Gen.GetNonNullFieldsDebugInfo(localGen.debugParams);
					List<Thing> list2 = localGen.root.Generate(localGen.debugParams);
					stringBuilder.Append(localGen.defName + " generated " + list2.Count + " things");
					if (!nonNullFieldsDebugInfo.NullOrEmpty())
					{
						stringBuilder.Append(" (used custom debug params: " + nonNullFieldsDebugInfo + ")");
					}
					stringBuilder.AppendLine(":");
					float num = 0f;
					float num2 = 0f;
					for (int i = 0; i < list2.Count; i++)
					{
						stringBuilder.AppendLine("   - " + list2[i].LabelCap);
						num += list2[i].MarketValue * (float)list2[i].stackCount;
						if (!(list2[i] is Pawn))
						{
							num2 += list2[i].GetStatValue(StatDefOf.Mass) * (float)list2[i].stackCount;
						}
						if (!GenPlace.TryPlaceThing(list2[i], UI.MouseCell(), Find.CurrentMap, ThingPlaceMode.Near, null, null, null))
						{
							list2[i].Destroy();
						}
					}
					stringBuilder.AppendLine("Total market value: " + num.ToString("0.##"));
					stringBuilder.AppendLine("Total mass: " + num2.ToStringMass());
					Log.Message(stringBuilder.ToString());
				}
			}));
		}
		return list;
	}

	[DebugAction("Spawning", "Trigger effecter...", false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap)]
	private static List<DebugActionNode> TriggerEffecter()
	{
		List<DebugActionNode> list = new List<DebugActionNode>();
		foreach (EffecterDef allDef in DefDatabase<EffecterDef>.AllDefs)
		{
			EffecterDef localDef = allDef;
			list.Add(new DebugActionNode(localDef.defName, DebugActionType.ToolMap, delegate
			{
				Effecter effecter = localDef.Spawn();
				effecter.Trigger(new TargetInfo(UI.MouseCell(), Find.CurrentMap), new TargetInfo(UI.MouseCell(), Find.CurrentMap));
				effecter.Cleanup();
			}));
		}
		return list;
	}

	[DebugAction("Spawning", "Trigger Maintained Effecter...", false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap)]
	private static List<DebugActionNode> TriggerMaintainedEffecter()
	{
		List<DebugActionNode> list = new List<DebugActionNode>();
		foreach (EffecterDef allDef in DefDatabase<EffecterDef>.AllDefs)
		{
			EffecterDef localDef = allDef;
			list.Add(new DebugActionNode(localDef.defName, DebugActionType.ToolMap, delegate
			{
				TargetInfo targetInfo = new TargetInfo(UI.MouseCell(), Find.CurrentMap);
				TargetInfo b = targetInfo;
				localDef.SpawnMaintained(targetInfo, b);
			}));
		}
		return list;
	}

	[DebugAction("Spawning", "Trigger Maintained Effecter (5s)...", false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap)]
	private static List<DebugActionNode> TriggerMaintainedEffecter5S()
	{
		List<DebugActionNode> list = new List<DebugActionNode>();
		foreach (EffecterDef allDef in DefDatabase<EffecterDef>.AllDefs)
		{
			EffecterDef localDef = allDef;
			list.Add(new DebugActionNode(localDef.defName, DebugActionType.ToolMap, delegate
			{
				Effecter effecter = localDef.Spawn();
				IntVec3 cell = UI.MouseCell();
				TargetInfo targetInfo = new TargetInfo(cell, Find.CurrentMap);
				TargetInfo b = targetInfo;
				effecter.Trigger(targetInfo, b);
				Find.CurrentMap.effecterMaintainer.AddEffecterToMaintain(effecter, targetInfo, b, 300);
			}));
		}
		return list;
	}

	[DebugAction("Spawning", "Trigger Maintained Effecter (12s)...", false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap)]
	private static List<DebugActionNode> TriggerMaintainedEffecter12S()
	{
		List<DebugActionNode> list = new List<DebugActionNode>();
		foreach (EffecterDef allDef in DefDatabase<EffecterDef>.AllDefs)
		{
			EffecterDef localDef = allDef;
			list.Add(new DebugActionNode(localDef.defName, DebugActionType.ToolMap, delegate
			{
				Effecter effecter = localDef.Spawn();
				IntVec3 cell = UI.MouseCell();
				TargetInfo targetInfo = new TargetInfo(cell, Find.CurrentMap);
				TargetInfo b = targetInfo;
				effecter.Trigger(targetInfo, b);
				Find.CurrentMap.effecterMaintainer.AddEffecterToMaintain(effecter, targetInfo, b, 720);
			}));
		}
		return list;
	}

	[DebugAction("Spawning", null, false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap)]
	private static DebugActionNode SpawnShuttle()
	{
		DebugActionNode debugActionNode = new DebugActionNode();
		debugActionNode.AddChild(new DebugActionNode("Incoming", DebugActionType.ToolMap, delegate
		{
			GenPlace.TryPlaceThing(SkyfallerMaker.MakeSkyfaller(ThingDefOf.ShuttleIncoming, ThingMaker.MakeThing(ThingDefOf.Shuttle)), UI.MouseCell(), Find.CurrentMap, ThingPlaceMode.Near, null, null, null);
		}));
		debugActionNode.AddChild(new DebugActionNode("Crashing", DebugActionType.ToolMap, delegate
		{
			GenPlace.TryPlaceThing(SkyfallerMaker.MakeSkyfaller(ThingDefOf.ShuttleCrashing, ThingMaker.MakeThing(ThingDefOf.ShuttleCrashed)), UI.MouseCell(), Find.CurrentMap, ThingPlaceMode.Near, null, null, null);
		}));
		debugActionNode.AddChild(new DebugActionNode("Stationary", DebugActionType.ToolMap, delegate
		{
			GenPlace.TryPlaceThing(ThingMaker.MakeThing(ThingDefOf.Shuttle), UI.MouseCell(), Find.CurrentMap, ThingPlaceMode.Near, null, null, null);
		}));
		return debugActionNode;
	}

	[DebugAction("Spawning", null, false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap)]
	private static List<DebugActionNode> SpawnAnimalGroup()
	{
		List<DebugActionNode> list = new List<DebugActionNode>();
		foreach (PawnKindDef item in from x in DefDatabase<PawnKindDef>.AllDefs
			where x.RaceProps.Animal
			select x into kd
			orderby kd.defName
			select kd)
		{
			PawnKindDef localKindDef = item;
			list.Add(new DebugActionNode(localKindDef.defName)
			{
				category = GetCategoryForPawnKind(localKindDef),
				action = delegate
				{
					Find.CurrentMap.wildAnimalSpawner.SpawnRandomWildAnimalAt(RCellFinder.RandomAnimalSpawnCell_MapGen(Find.CurrentMap), canFlyIn: true, localKindDef);
				}
			});
		}
		list.Add(new DebugActionNode("[random suitable]")
		{
			action = delegate
			{
				Find.CurrentMap.wildAnimalSpawner.SpawnRandomWildAnimalAt(RCellFinder.RandomAnimalSpawnCell_MapGen(Find.CurrentMap), canFlyIn: true);
			}
		});
		return list;
	}

	[DebugAction("Spawning", null, false, false, false, false, false, 0, false, actionType = DebugActionType.ToolWorld, allowedGameStates = AllowedGameStates.PlayingOnWorld)]
	private static void SpawnRandomCaravan()
	{
		PlanetTile planetTile = GenWorld.MouseTile();
		if (Find.WorldGrid[planetTile].PrimaryBiome.impassable)
		{
			return;
		}
		List<Pawn> list = new List<Pawn>();
		int num = Rand.RangeInclusive(1, 10);
		for (int i = 0; i < num; i++)
		{
			Pawn pawn = PawnGenerator.GeneratePawn(Faction.OfPlayer.def.basicMemberKind, Faction.OfPlayer, null);
			list.Add(pawn);
			if (!pawn.WorkTagIsDisabled(WorkTags.Violent))
			{
				ThingDef thingDef = DefDatabase<ThingDef>.AllDefs.Where((ThingDef def) => def.IsWeapon && !def.weaponTags.NullOrEmpty() && (def.weaponTags.Contains("SimpleGun") || def.weaponTags.Contains("IndustrialGunAdvanced") || def.weaponTags.Contains("SpacerGun") || def.weaponTags.Contains("MedievalMeleeAdvanced") || def.weaponTags.Contains("NeolithicRangedBasic") || def.weaponTags.Contains("NeolithicRangedDecent") || def.weaponTags.Contains("NeolithicRangedHeavy"))).RandomElementWithFallback();
				pawn.equipment.AddEquipment((ThingWithComps)ThingMaker.MakeThing(thingDef, GenStuff.RandomStuffFor(thingDef)));
			}
		}
		int num2 = Rand.RangeInclusive(-4, 10);
		for (int j = 0; j < num2; j++)
		{
			Pawn item = PawnGenerator.GeneratePawn(DefDatabase<PawnKindDef>.AllDefs.Where((PawnKindDef d) => d.RaceProps.Animal && d.race.GetStatValueAbstract(StatDefOf.Wildness) < 1f).RandomElement(), Faction.OfPlayer, null);
			list.Add(item);
		}
		Caravan caravan = CaravanMaker.MakeCaravan(list, Faction.OfPlayer, planetTile, addToWorldPawnsIfNotAlready: true);
		List<Thing> list2 = ThingSetMakerDefOf.DebugCaravanInventory.root.Generate();
		for (int k = 0; k < list2.Count; k++)
		{
			Thing thing = list2[k];
			if (!(thing.GetStatValue(StatDefOf.Mass) * (float)thing.stackCount > caravan.MassCapacity - caravan.MassUsage))
			{
				CaravanInventoryUtility.GiveThing(caravan, thing);
				continue;
			}
			break;
		}
	}

	[DebugAction("Spawning", null, false, false, false, false, false, 0, false, actionType = DebugActionType.ToolWorld, allowedGameStates = AllowedGameStates.PlayingOnWorld)]
	private static void SpawnRandomFactionBase()
	{
		if (Find.FactionManager.AllFactions.Where((Faction x) => !x.IsPlayer && !x.Hidden).TryRandomElement(out var result))
		{
			PlanetTile tile = GenWorld.MouseTile();
			if (!Find.WorldGrid[tile].PrimaryBiome.impassable)
			{
				Settlement settlement = (Settlement)WorldObjectMaker.MakeWorldObject(tile.LayerDef.SettlementWorldObjectDef);
				settlement.SetFaction(result);
				settlement.Tile = tile;
				settlement.Name = SettlementNameGenerator.GenerateSettlementName(settlement);
				Find.WorldObjects.Add(settlement);
			}
		}
	}

	[DebugAction("Spawning", null, false, false, false, false, false, 0, false, actionType = DebugActionType.ToolWorld, allowedGameStates = AllowedGameStates.PlayingOnWorld)]
	private static void SpawnSite()
	{
		PlanetTile tile = GenWorld.MouseTile();
		if (!tile.Valid || Find.World.Impassable(tile))
		{
			Messages.Message("Impassable", MessageTypeDefOf.RejectInput, historical: false);
			return;
		}
		List<SitePartDef> parts = new List<SitePartDef>();
		Action addPart = null;
		addPart = delegate
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>
			{
				new DebugMenuOption("-Done (" + parts.Count + " parts)-", DebugMenuOptionMode.Action, delegate
				{
					Site site = SiteMaker.TryMakeSite(parts, tile, disallowNonHostileFactions: true, null, ifHostileThenMustRemainHostile: true, null);
					if (site == null)
					{
						Messages.Message("Could not find any valid faction for this site.", MessageTypeDefOf.RejectInput, historical: false);
					}
					else
					{
						Find.WorldObjects.Add(site);
					}
				})
			};
			foreach (SitePartDef allDef in DefDatabase<SitePartDef>.AllDefs)
			{
				SitePartDef localPart = allDef;
				list.Add(new DebugMenuOption(allDef.defName, DebugMenuOptionMode.Action, delegate
				{
					parts.Add(localPart);
					addPart();
				}));
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
		};
		addPart();
	}

	[DebugAction("Spawning", null, false, false, false, false, false, 0, false, actionType = DebugActionType.ToolWorld, allowedGameStates = AllowedGameStates.PlayingOnWorld)]
	private static void DestroySite()
	{
		PlanetTile tileID = GenWorld.MouseTile();
		foreach (WorldObject item in Find.WorldObjects.ObjectsAt(tileID).ToList())
		{
			item.Destroy();
		}
	}

	[DebugAction("Spawning", null, false, false, false, false, false, 0, false, actionType = DebugActionType.ToolWorld, allowedGameStates = AllowedGameStates.PlayingOnWorld)]
	private static void SpawnSiteWithPoints()
	{
		PlanetTile tile = GenWorld.MouseTile();
		if (!tile.Valid || Find.World.Impassable(tile))
		{
			Messages.Message("Impassable", MessageTypeDefOf.RejectInput, historical: false);
			return;
		}
		List<SitePartDef> parts = new List<SitePartDef>();
		Action addPart = null;
		addPart = delegate
		{
			List<DebugMenuOption> list2 = new List<DebugMenuOption>
			{
				new DebugMenuOption("-Done (" + parts.Count + " parts)-", DebugMenuOptionMode.Action, delegate
				{
					List<DebugMenuOption> list = new List<DebugMenuOption>();
					foreach (float item in DebugActionsUtility.PointsOptions(extended: true))
					{
						float localPoints = item;
						list.Add(new DebugMenuOption(item.ToString("F0"), DebugMenuOptionMode.Action, delegate
						{
							Site site = SiteMaker.TryMakeSite(parts, tile, disallowNonHostileFactions: true, null, ifHostileThenMustRemainHostile: true, localPoints);
							if (site == null)
							{
								Messages.Message("Could not find any valid faction for this site.", MessageTypeDefOf.RejectInput, historical: false);
							}
							else
							{
								Find.WorldObjects.Add(site);
							}
						}));
					}
					Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
				})
			};
			foreach (SitePartDef allDef in DefDatabase<SitePartDef>.AllDefs)
			{
				SitePartDef localPart = allDef;
				list2.Add(new DebugMenuOption(allDef.defName, DebugMenuOptionMode.Action, delegate
				{
					parts.Add(localPart);
					addPart();
				}));
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list2));
		};
		addPart();
	}

	[DebugAction("Spawning", null, false, false, false, false, false, 0, false, actionType = DebugActionType.ToolWorld, allowedGameStates = AllowedGameStates.PlayingOnWorld)]
	private static List<DebugActionNode> SpawnWorldObject()
	{
		List<DebugActionNode> list = new List<DebugActionNode>();
		foreach (WorldObjectDef allDef in DefDatabase<WorldObjectDef>.AllDefs)
		{
			WorldObjectDef localDef = allDef;
			list.Add(new DebugActionNode(localDef.defName, DebugActionType.ToolWorld, delegate
			{
				PlanetTile planetTile = GenWorld.MouseTile();
				if (!planetTile.Valid || Find.World.Impassable(planetTile))
				{
					Messages.Message("Impassable", MessageTypeDefOf.RejectInput, historical: false);
				}
				else
				{
					WorldObject worldObject = WorldObjectMaker.MakeWorldObject(localDef);
					worldObject.Tile = planetTile;
					Find.WorldObjects.Add(worldObject);
				}
			}));
		}
		return list;
	}

	[DebugAction("General", "Change camera config", false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnWorld)]
	private static List<DebugActionNode> ChangeCameraConfigWorld()
	{
		List<DebugActionNode> list = new List<DebugActionNode>();
		foreach (Type item in typeof(WorldCameraConfig).AllSubclasses())
		{
			Type localType = item;
			string text = localType.Name;
			if (text.StartsWith("WorldCameraConfig_"))
			{
				text = text.Substring("WorldCameraConfig_".Length);
			}
			list.Add(new DebugActionNode(text, DebugActionType.Action, delegate
			{
				Find.WorldCameraDriver.config = (WorldCameraConfig)Activator.CreateInstance(localType);
			}));
		}
		return list;
	}

	[DebugAction("Spawning", null, false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap, requiresBiotech = true)]
	private static List<DebugActionNode> SpawnBossgroup()
	{
		List<DebugActionNode> list = new List<DebugActionNode>();
		foreach (BossgroupDef def in DefDatabase<BossgroupDef>.AllDefs)
		{
			BossgroupDef localDef = def;
			string text = localDef.defName;
			if (!PawnsFinder.AllMaps_FreeColonists.TryRandomElement(out var caller) || !localDef.Worker.CanResolve(caller))
			{
				text += " [NO]";
			}
			DebugActionNode debugActionNode = new DebugActionNode(text);
			debugActionNode.childGetter = delegate
			{
				List<DebugActionNode> list2 = new List<DebugActionNode>();
				int currentWave = 0;
				GameComponent_Bossgroup component = Current.Game.GetComponent<GameComponent_Bossgroup>();
				if (component != null)
				{
					currentWave = component.NumTimesCalledBossgroup(def);
				}
				list2.Add(new DebugActionNode("*Current (times called: " + currentWave + ")", DebugActionType.Action, delegate
				{
					if (caller == null)
					{
						Messages.Message("No colonist found to call bossgroup.", MessageTypeDefOf.RejectInput, historical: false);
					}
					else
					{
						localDef.Worker.Resolve(caller.Map, currentWave);
					}
				}));
				for (int i = 0; i < def.waves.Count; i++)
				{
					int index = i;
					list2.Add(new DebugActionNode("Wave " + index, DebugActionType.Action, delegate
					{
						if (caller == null)
						{
							Messages.Message("No colonist found to call bossgroup.", MessageTypeDefOf.RejectInput, historical: false);
						}
						else
						{
							localDef.Worker.Resolve(caller.Map, index);
						}
					}));
				}
				return list2;
			};
			list.Add(debugActionNode);
		}
		return list;
	}

	[DebugAction("Spawning", null, false, false, false, false, false, 0, false, actionType = DebugActionType.ToolWorld, allowedGameStates = AllowedGameStates.PlayingOnWorld)]
	public static List<DebugActionNode> AbandonThing()
	{
		return (from t in DebugThingPlaceHelper.TryAbandonOptionsForStackCount()
			orderby t.label
			select t).ToList();
	}

	[DebugAction("Spawning", null, false, false, false, false, false, 0, false, actionType = DebugActionType.Action, allowedGameStates = AllowedGameStates.PlayingOnMap)]
	private static void TestDeferredSpawner()
	{
		IncidentParms incidentParms = StorytellerUtility.DefaultParmsNow(IncidentCategoryDefOf.ThreatBig, Find.CurrentMap);
		incidentParms.forced = true;
		incidentParms.points = 200f;
		incidentParms.faction = Faction.OfHoraxCult;
		List<Pawn> list = new List<Pawn>(PawnGroupMakerUtility.GeneratePawns(IncidentParmsUtility.GetDefaultPawnGroupMakerParms(PawnGroupKindDefOf.Combat, incidentParms)));
		List<IntVec3> list2 = new List<IntVec3>();
		for (int i = 0; i < list.Count; i++)
		{
			list2.Add(CellFinder.RandomSpawnCellForPawnNear(Find.CurrentMap.Center, Find.CurrentMap));
		}
		SpawnRequest spawnRequest = new SpawnRequest(list.Cast<Thing>().ToList(), list2, 1, 2f);
		spawnRequest.initialDelay = 500;
		spawnRequest.preSpawnEffecterOffsetTicks = -120;
		spawnRequest.preSpawnEffect = EffecterDefOf.WaterMist;
		Find.CurrentMap.deferredSpawner.AddRequest(spawnRequest);
	}

	[DebugAction("Spawning", null, false, false, false, false, false, 0, false, actionType = DebugActionType.Action, allowedGameStates = AllowedGameStates.PlayingOnMap, requiresAnomaly = true)]
	private static void CreateColonistDuplicate()
	{
		Map currentMap = Find.CurrentMap;
		GenSpawn.Spawn(Find.PawnDuplicator.Duplicate(currentMap.mapPawns.FreeColonistsSpawned[0]), CellFinder.RandomSpawnCellForPawnNear(currentMap.Center, currentMap), currentMap);
	}

	[DebugAction("Spawning", null, false, false, false, false, false, 0, false, actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap, requiresAnomaly = true)]
	private static void Duplicate()
	{
		Map currentMap = Find.CurrentMap;
		foreach (Thing item in currentMap.thingGrid.ThingsAt(UI.MouseCell()).ToList())
		{
			if (item is Pawn pawn && pawn.RaceProps.Humanlike)
			{
				GenSpawn.Spawn(Find.PawnDuplicator.Duplicate(pawn), CellFinder.RandomSpawnCellForPawnNear(pawn.PositionHeld, currentMap), currentMap);
			}
		}
	}

	[DebugAction("Spawning", null, false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap, displayPriority = 1000)]
	private static List<DebugActionNode> WaterEmergePawn()
	{
		List<DebugActionNode> list = new List<DebugActionNode>();
		foreach (PawnKindDef item in from x in DefDatabase<PawnKindDef>.AllDefs
			where x.showInDebugSpawner
			select x into kd
			orderby kd.defName
			select kd)
		{
			PawnKindDef localKindDef = item;
			DebugActionNode debugActionNode = new DebugActionNode(localKindDef.defName, DebugActionType.ToolMap)
			{
				category = GetCategoryForPawnKind(localKindDef)
			};
			for (int i = 1; i <= 3; i++)
			{
				int delaySeconds = i;
				debugActionNode.AddChild(new DebugActionNode($"{i}s delay", DebugActionType.ToolMap)
				{
					action = delegate
					{
						Faction faction = FactionUtility.DefaultFactionFrom(localKindDef.defaultFactionDef);
						DelayedEffecterSpawner.Spawn(PawnGenerator.GeneratePawn(localKindDef, faction, null), delayTicks: delaySeconds * 60, pos: UI.MouseCell(), map: Find.CurrentMap, emergeEffect: EffecterDefOf.PawnEmergeFromWaterLarge, preEmergeEffect: EffecterDefOf.WaterMist, emergeSound: SoundDefOf.EmergeFromWater);
					}
				});
			}
			list.Add(debugActionNode);
		}
		return list;
	}

	[DebugAction("Spawning", null, false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap, displayPriority = 1000, requiresAnomaly = true)]
	private static List<DebugActionNode> SpawnFlare()
	{
		List<DebugActionNode> list = new List<DebugActionNode>();
		int[] array = new int[7] { 1, 2, 3, 4, 5, 15, 30 };
		foreach (int delaySeconds in array)
		{
			list.Add(new DebugActionNode($"{delaySeconds}s duration", DebugActionType.ToolMap)
			{
				action = delegate
				{
					int num = delaySeconds * 60;
					ThingWithComps obj = (ThingWithComps)ThingMaker.MakeThing(ThingDefOf.DisruptorFlare);
					GenSpawn.Spawn(obj, UI.MouseCell(), Find.CurrentMap);
					CompDestroyAfterDelay comp = obj.GetComp<CompDestroyAfterDelay>();
					comp.spawnTick = GenTicks.TicksGame + num - comp.Props.delayTicks;
					EffecterDefOf.DisruptorFlareLanded.Spawn(UI.MouseCell(), Find.CurrentMap).Cleanup();
				}
			});
		}
		return list;
	}

	[DebugAction("Spawning", null, false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap, displayPriority = 1000, actionType = DebugActionType.ToolMapForPawns, requiresOdyssey = true)]
	private static void SpawnStatueCraftedBy(Pawn sculptor)
	{
		List<Pawn> allPossiblePawnsToSelectFrom = CompStatue.GetAllPossiblePawnsToSelectFrom(sculptor);
		if (allPossiblePawnsToSelectFrom.Count == 0)
		{
			Messages.Message("No suitable statue subjects found", MessageTypeDefOf.RejectInput, historical: false);
			return;
		}
		Find.WindowStack.Add(new Dialog_DebugOptionListLister(allPossiblePawnsToSelectFrom.Select((Pawn subject) => new DebugMenuOption(subject.Label, DebugMenuOptionMode.Tool, delegate
		{
			IntVec3 center = UI.MouseCell();
			GenPlace.TryPlaceThing(CompStatue.GenerateSpecificStatue(sculptor, subject), center, Find.CurrentMap, ThingPlaceMode.Near, null, null, null);
		}))));
	}

	[DebugAction("Spawning", null, false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap)]
	private static void SpawnFilth()
	{
		List<ThingDef> source = DefDatabase<ThingDef>.AllDefs.Where((ThingDef d) => d.category == ThingCategory.Filth).ToList();
		Find.WindowStack.Add(new Dialog_DebugOptionListLister(source.Select((ThingDef filthDef) => new DebugMenuOption(filthDef.defName, DebugMenuOptionMode.Tool, delegate
		{
			FilthMaker.TryMakeFilth(UI.MouseCell(), Find.CurrentMap, filthDef);
		}))));
	}
}
