using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;

namespace RimWorld;

public static class ThingDefGenerator_Corpses
{
	[CompilerGenerated]
	private sealed class _003CImpliedCorpseDefs_003Ed__6 : IEnumerable<ThingDef>, IEnumerable, IEnumerator<ThingDef>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private ThingDef _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private bool hotReload;

		public bool _003C_003E3__hotReload;

		private List<ThingDef> _003Cpostprocess_003E5__2;

		private List<ThingDef>.Enumerator _003C_003E7__wrap2;

		private ThingDef _003CpawnDef_003E5__4;

		ThingDef IEnumerator<ThingDef>.Current
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
		public _003CImpliedCorpseDefs_003Ed__6(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			int num = _003C_003E1__state;
			if (num == -3 || (uint)(num - 1) <= 1u)
			{
				try
				{
				}
				finally
				{
					_003C_003Em__Finally1();
				}
			}
			_003Cpostprocess_003E5__2 = null;
			_003C_003E7__wrap2 = default(List<ThingDef>.Enumerator);
			_003CpawnDef_003E5__4 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				switch (_003C_003E1__state)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003Cpostprocess_003E5__2 = new List<ThingDef>();
					_003C_003E7__wrap2 = DefDatabase<ThingDef>.AllDefs.ToList().GetEnumerator();
					_003C_003E1__state = -3;
					break;
				case 1:
					_003C_003E1__state = -3;
					if (ModsConfig.AnomalyActive && _003CpawnDef_003E5__4.race.hasUnnaturalCorpse)
					{
						_003C_003E2__current = GenerateUnnaturalCorpseDef(hotReload, _003CpawnDef_003E5__4);
						_003C_003E1__state = 2;
						return true;
					}
					goto IL_019d;
				case 2:
					{
						_003C_003E1__state = -3;
						goto IL_019d;
					}
					IL_019d:
					_003CpawnDef_003E5__4 = null;
					break;
				}
				while (_003C_003E7__wrap2.MoveNext())
				{
					_003CpawnDef_003E5__4 = _003C_003E7__wrap2.Current;
					if (_003CpawnDef_003E5__4.category == ThingCategory.Pawn && _003CpawnDef_003E5__4.race.hasCorpse)
					{
						if (_003CpawnDef_003E5__4.race.linkedCorpseKind == null || _003CpawnDef_003E5__4.race.linkedCorpseKind == _003CpawnDef_003E5__4)
						{
							ThingDef corpseDef = GenerateCorpseDef(_003CpawnDef_003E5__4, hotReload);
							_003CpawnDef_003E5__4.race.corpseDef = corpseDef;
							_003C_003E2__current = corpseDef;
							_003C_003E1__state = 1;
							return true;
						}
						if (_003CpawnDef_003E5__4.race.linkedCorpseKind.race.corpseDef == null)
						{
							_003Cpostprocess_003E5__2.Add(_003CpawnDef_003E5__4);
						}
						else
						{
							_003CpawnDef_003E5__4.race.corpseDef = _003CpawnDef_003E5__4.race.linkedCorpseKind.race.corpseDef;
						}
					}
				}
				_003C_003Em__Finally1();
				_003C_003E7__wrap2 = default(List<ThingDef>.Enumerator);
				int num = 100;
				while (_003Cpostprocess_003E5__2.Any() && num > 0)
				{
					for (int num2 = _003Cpostprocess_003E5__2.Count - 1; num2 >= 0; num2--)
					{
						ThingDef thingDef = _003Cpostprocess_003E5__2[num2];
						if (thingDef.race.linkedCorpseKind.race.corpseDef != null)
						{
							thingDef.race.corpseDef = thingDef.race.linkedCorpseKind.race.corpseDef;
							_003Cpostprocess_003E5__2.RemoveAt(num2);
						}
					}
					num--;
				}
				if (num == 0)
				{
					Log.ErrorOnce("Failed to load linked corpse defs, check for a circular loop in the defs: " + _003Cpostprocess_003E5__2.Select((ThingDef x) => x.defName).ToCommaList(), 25364571);
				}
				return false;
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
			((IDisposable)_003C_003E7__wrap2/*cast due to .constrained prefix*/).Dispose();
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<ThingDef> IEnumerable<ThingDef>.GetEnumerator()
		{
			_003CImpliedCorpseDefs_003Ed__6 _003CImpliedCorpseDefs_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CImpliedCorpseDefs_003Ed__ = this;
			}
			else
			{
				_003CImpliedCorpseDefs_003Ed__ = new _003CImpliedCorpseDefs_003Ed__6(0);
			}
			_003CImpliedCorpseDefs_003Ed__.hotReload = _003C_003E3__hotReload;
			return _003CImpliedCorpseDefs_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<ThingDef>)this).GetEnumerator();
		}
	}

	private const float DaysToStartRot = 2.5f;

	private const float DaysToDessicate = 5f;

	private const float RotDamagePerDay = 2f;

	private const float DessicatedDamagePerDay = 0.7f;

	private const float ButcherProductsMarketValueFactor = 0.6f;

	private const string DeactivateImagePath = "UI/Commands/DestroyUnnaturalCorpse";

	[IteratorStateMachine(typeof(_003CImpliedCorpseDefs_003Ed__6))]
	public static IEnumerable<ThingDef> ImpliedCorpseDefs(bool hotReload = false)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CImpliedCorpseDefs_003Ed__6(-2)
		{
			_003C_003E3__hotReload = hotReload
		};
	}

	private static ThingDef GenerateUnnaturalCorpseDef(bool hotReload, ThingDef pawnDef)
	{
		ThingDef thingDef = GenerateCorpseDef(pawnDef, hotReload, "UnnaturalCorpse_" + pawnDef.defName);
		thingDef.label = "UnnaturalCorpse".Translate(pawnDef);
		thingDef.thingClass = typeof(UnnaturalCorpse);
		thingDef.SetStatBaseValue(StatDefOf.DeteriorationRate, 0.25f);
		thingDef.tickerType = TickerType.Normal;
		CompProperties_Studiable item = new CompProperties_Studiable
		{
			frequencyTicks = 120000,
			knowledgeCategory = KnowledgeCategoryDefOf.Advanced,
			anomalyKnowledge = 2f,
			minMonolithLevelForStudy = 1,
			studyEnabledByDefault = false,
			showToggleGizmo = true
		};
		thingDef.comps.Add(item);
		thingDef.inspectorTabs.Insert(1, typeof(ITab_StudyNotesUnnaturalCorpse));
		CompProperties_DisableUnnaturalCorpse item2 = new CompProperties_DisableUnnaturalCorpse
		{
			activeTicks = 1,
			ticksToActivate = 180,
			activateTexPath = "UI/Commands/DestroyUnnaturalCorpse",
			activateLabelString = "UnnaturalCorpseDeactivate".Translate(),
			activateDescString = "UnnaturalCorpseDeactivateDesc".Translate(),
			guiLabelString = "UnnaturalCorpseGuiLabel".Translate(),
			jobString = "UnnaturalCorpseJobString".Translate(),
			activatingString = "UnnaturalCorpseActivating".Translate(),
			activatingStringPending = "UnnaturalCorpseActivatingEnroute".Translate(),
			targetingParameters = new TargetingParameters
			{
				canTargetBuildings = false,
				canTargetAnimals = false,
				canTargetMechs = false,
				onlyTargetControlledPawns = true
			}
		};
		thingDef.comps.Add(item2);
		pawnDef.race.unnaturalCorpseDef = thingDef;
		pawnDef.race.corpseDef.virtualDefs.Add(thingDef);
		thingDef.virtualDefParent = pawnDef.race.corpseDef;
		return thingDef;
	}

	private static ThingDef GenerateCorpseDef(ThingDef pawnDef, bool hotReload = false, string customDefName = null)
	{
		string defName = customDefName ?? ("Corpse_" + pawnDef.defName);
		ThingDef thingDef = (hotReload ? (DefDatabase<ThingDef>.GetNamed(defName, errorOnFail: false) ?? new ThingDef()) : new ThingDef());
		thingDef.defName = defName;
		thingDef.label = "CorpseLabel".Translate(pawnDef.label);
		thingDef.description = "CorpseDesc".Translate(pawnDef.label);
		thingDef.soundImpactDefault = pawnDef.soundImpactDefault;
		thingDef.category = ThingCategory.Item;
		thingDef.thingClass = typeof(Corpse);
		thingDef.selectable = true;
		thingDef.tickerType = TickerType.Rare;
		thingDef.altitudeLayer = AltitudeLayer.ItemImportant;
		thingDef.scatterableOnMapGen = false;
		thingDef.alwaysHaulable = true;
		thingDef.soundDrop = SoundDefOf.Corpse_Drop;
		thingDef.pathCost = 14;
		thingDef.socialPropernessMatters = false;
		thingDef.tradeability = Tradeability.None;
		thingDef.messageOnDeteriorateInStorage = false;
		thingDef.modContentPack = pawnDef.modContentPack;
		thingDef.hideStats = pawnDef.hideStats;
		thingDef.hiddenWhileUndiscovered = pawnDef.race.corpseHiddenWhileUndiscovered;
		thingDef.inspectorTabs = new List<Type>
		{
			typeof(ITab_Pawn_Health),
			typeof(ITab_Pawn_Character),
			typeof(ITab_Pawn_Gear),
			typeof(ITab_Pawn_Social),
			typeof(ITab_Pawn_Log)
		};
		thingDef.comps.Add(new CompProperties_Forbiddable());
		if (ModsConfig.BiotechActive)
		{
			thingDef.inspectorTabs.Add(typeof(ITab_Genes));
		}
		thingDef.SetStatBaseValue(StatDefOf.Beauty, -50f);
		thingDef.SetStatBaseValue(StatDefOf.DeteriorationRate, 1f);
		thingDef.SetStatBaseValue(StatDefOf.FoodPoisonChanceFixedHuman, 0.05f);
		thingDef.recipes = new List<RecipeDef>();
		if (!pawnDef.race.IsMechanoid)
		{
			thingDef.recipes.Add(RecipeDefOf.RemoveBodyPart);
		}
		thingDef.SetStatBaseValue(StatDefOf.MarketValue, CalculateMarketValue(pawnDef));
		thingDef.SetStatBaseValue(StatDefOf.Flammability, pawnDef.GetStatValueAbstract(StatDefOf.Flammability));
		thingDef.SetStatBaseValue(StatDefOf.MaxHitPoints, pawnDef.BaseMaxHitPoints);
		thingDef.SetStatBaseValue(StatDefOf.Mass, pawnDef.statBases.GetStatOffsetFromList(StatDefOf.Mass));
		thingDef.SetStatBaseValue(StatDefOf.Nutrition, 5.2f);
		thingDef.ingestible = new IngestibleProperties();
		thingDef.ingestible.parent = thingDef;
		IngestibleProperties ingestible = thingDef.ingestible;
		ingestible.foodType = FoodTypeFlags.Corpse;
		ingestible.sourceDef = pawnDef;
		ingestible.preferability = ((!pawnDef.race.IsFlesh) ? FoodPreferability.NeverForNutrition : FoodPreferability.DesperateOnly);
		DirectXmlCrossRefLoader.RegisterObjectWantsCrossRef(ingestible, "tasteThought", ThoughtDefOf.AteCorpse.defName);
		ingestible.maxNumToIngestAtOnce = 1;
		ingestible.defaultNumToIngestAtOnce = 1;
		ingestible.ingestEffect = EffecterDefOf.EatMeat;
		ingestible.ingestSound = SoundDefOf.RawMeat_Eat;
		ingestible.specialThoughtDirect = pawnDef.race.FleshType.ateDirect;
		if (pawnDef.race.IsFlesh)
		{
			thingDef.comps.Add(new CompProperties_Rottable
			{
				daysToRotStart = 2.5f,
				daysToDessicated = 5f,
				rotDamagePerDay = 2f,
				dessicatedDamagePerDay = 0.7f
			});
			thingDef.comps.Add(new CompProperties_SpawnerFilth
			{
				filthDef = ThingDefOf.Filth_CorpseBile,
				spawnCountOnSpawn = 0,
				spawnMtbHours = 0f,
				spawnRadius = 0.1f,
				spawnEveryDays = 1f,
				requiredRotStage = RotStage.Rotting
			});
		}
		if (ModsConfig.AnomalyActive && pawnDef.race.IsFlesh)
		{
			thingDef.comps.Add(new CompProperties
			{
				compClass = typeof(CompHarbingerTreeConsumable)
			});
		}
		thingDef.race = pawnDef.race;
		if (thingDef.thingCategories == null)
		{
			thingDef.thingCategories = new List<ThingCategoryDef>();
		}
		DirectXmlCrossRefLoader.RegisterListWantsCrossRef(thingDef.thingCategories, pawnDef.race.Humanlike ? ThingCategoryDefOf.CorpsesHumanlike.defName : pawnDef.race.FleshType.corpseCategory.defName, thingDef);
		return thingDef;
	}

	private static float CalculateMarketValue(ThingDef raceDef)
	{
		float num = 0f;
		if (raceDef.race.meatDef != null)
		{
			int num2 = Mathf.RoundToInt(raceDef.GetStatValueAbstract(StatDefOf.MeatAmount));
			num += (float)num2 * raceDef.race.meatDef.GetStatValueAbstract(StatDefOf.MarketValue);
		}
		if (raceDef.race.leatherDef != null)
		{
			int num3 = Mathf.RoundToInt(raceDef.GetStatValueAbstract(StatDefOf.LeatherAmount));
			num += (float)num3 * raceDef.race.leatherDef.GetStatValueAbstract(StatDefOf.MarketValue);
		}
		if (raceDef.butcherProducts != null)
		{
			for (int i = 0; i < raceDef.butcherProducts.Count; i++)
			{
				num += raceDef.butcherProducts[i].thingDef.BaseMarketValue * (float)raceDef.butcherProducts[i].count;
			}
		}
		return num * 0.6f;
	}
}
