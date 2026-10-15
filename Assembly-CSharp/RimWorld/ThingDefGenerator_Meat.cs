using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld;

public static class ThingDefGenerator_Meat
{
	[CompilerGenerated]
	private sealed class _003CImpliedMeatDefs_003Ed__0 : IEnumerable<ThingDef>, IEnumerable, IEnumerator<ThingDef>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private ThingDef _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private bool hotReload;

		public bool _003C_003E3__hotReload;

		private List<ThingDef>.Enumerator _003C_003E7__wrap1;

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
		public _003CImpliedMeatDefs_003Ed__0(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			int num = _003C_003E1__state;
			if (num == -3 || num == 1)
			{
				try
				{
				}
				finally
				{
					_003C_003Em__Finally1();
				}
			}
			_003C_003E7__wrap1 = default(List<ThingDef>.Enumerator);
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
					_003C_003E7__wrap1 = DefDatabase<ThingDef>.AllDefs.ToList().GetEnumerator();
					_003C_003E1__state = -3;
					break;
				case 1:
					_003C_003E1__state = -3;
					break;
				}
				while (_003C_003E7__wrap1.MoveNext())
				{
					ThingDef current = _003C_003E7__wrap1.Current;
					if (current.category != ThingCategory.Pawn || !current.race.hasMeat || !current.race.hasCorpse || current.race.useMeatFrom != null || current.race.specificMeatDef != null)
					{
						continue;
					}
					if (!current.race.IsFlesh)
					{
						DirectXmlCrossRefLoader.RegisterObjectWantsCrossRef(current.race, "meatDef", "Steel");
						continue;
					}
					string defName = "Meat_" + current.defName;
					ThingDef thingDef = (hotReload ? (DefDatabase<ThingDef>.GetNamed(defName, errorOnFail: false) ?? new ThingDef()) : new ThingDef());
					thingDef.drawerType = DrawerType.MapMeshOnly;
					thingDef.resourceReadoutPriority = ResourceCountPriority.Middle;
					thingDef.category = ThingCategory.Item;
					thingDef.thingClass = typeof(ThingWithComps);
					thingDef.graphicData = new GraphicData();
					thingDef.graphicData.graphicClass = typeof(Graphic_StackCount);
					thingDef.useHitPoints = true;
					thingDef.selectable = true;
					thingDef.SetStatBaseValue(StatDefOf.MaxHitPoints, 100f);
					thingDef.altitudeLayer = AltitudeLayer.Item;
					thingDef.stackLimit = 75;
					thingDef.comps.Add(new CompProperties_Forbiddable());
					CompProperties_Rottable item = new CompProperties_Rottable
					{
						daysToRotStart = 2f,
						rotDestroys = true
					};
					thingDef.comps.Add(item);
					thingDef.tickerType = TickerType.Rare;
					thingDef.SetStatBaseValue(StatDefOf.Beauty, -4f);
					thingDef.alwaysHaulable = true;
					thingDef.rotatable = false;
					thingDef.pathCost = 14;
					thingDef.drawGUIOverlay = true;
					thingDef.socialPropernessMatters = true;
					thingDef.modContentPack = current.modContentPack;
					thingDef.category = ThingCategory.Item;
					if (current.race.Humanlike)
					{
						thingDef.description = "MeatHumanDesc".Translate(current.label);
					}
					else if (current.race.FleshType == FleshTypeDefOf.Insectoid)
					{
						thingDef.description = "MeatInsectDesc".Translate(current.label);
					}
					else
					{
						thingDef.description = "MeatDesc".Translate(current.label);
					}
					thingDef.useHitPoints = true;
					thingDef.healthAffectsPrice = false;
					thingDef.SetStatBaseValue(StatDefOf.MaxHitPoints, 60f);
					thingDef.SetStatBaseValue(StatDefOf.DeteriorationRate, 6f);
					thingDef.SetStatBaseValue(StatDefOf.Mass, 0.03f);
					thingDef.SetStatBaseValue(StatDefOf.Flammability, 0.5f);
					thingDef.SetStatBaseValue(StatDefOf.Nutrition, 0.05f);
					thingDef.SetStatBaseValue(StatDefOf.FoodPoisonChanceFixedHuman, 0.02f);
					thingDef.BaseMarketValue = current.race.meatMarketValue;
					if (thingDef.thingCategories == null)
					{
						thingDef.thingCategories = new List<ThingCategoryDef>();
					}
					DirectXmlCrossRefLoader.RegisterListWantsCrossRef(thingDef.thingCategories, "MeatRaw", thingDef);
					thingDef.ingestible = new IngestibleProperties();
					thingDef.ingestible.parent = thingDef;
					thingDef.ingestible.foodType = FoodTypeFlags.Meat;
					thingDef.ingestible.preferability = FoodPreferability.RawBad;
					DirectXmlCrossRefLoader.RegisterObjectWantsCrossRef(thingDef.ingestible, "tasteThought", ThoughtDefOf.AteRawFood.defName);
					thingDef.ingestible.ingestEffect = EffecterDefOf.EatMeat;
					thingDef.ingestible.ingestSound = SoundDefOf.RawMeat_Eat;
					thingDef.ingestible.specialThoughtDirect = current.race.FleshType.ateDirect;
					thingDef.ingestible.specialThoughtAsIngredient = current.race.FleshType.ateAsIngredient;
					if (ModsConfig.AnomalyActive)
					{
						CompProperties item2 = new CompProperties
						{
							compClass = typeof(CompHarbingerTreeConsumable)
						};
						thingDef.comps.Add(item2);
					}
					if (current.ingredient != null)
					{
						thingDef.ingredient = new IngredientProperties();
						thingDef.ingredient.mergeCompatibilityTags.AddRange(current.ingredient.mergeCompatibilityTags);
					}
					thingDef.graphicData.color = current.race.meatColor;
					if (current.race.Humanlike)
					{
						thingDef.graphicData.texPath = "Things/Item/Resource/MeatFoodRaw/Meat_Human";
						thingDef.possessionCount = 5;
					}
					else if (current.race.FleshType == FleshTypeDefOf.Insectoid)
					{
						thingDef.graphicData.texPath = "Things/Item/Resource/MeatFoodRaw/Meat_Insect";
					}
					else if (current.race.baseBodySize < 0.7f)
					{
						thingDef.graphicData.texPath = "Things/Item/Resource/MeatFoodRaw/Meat_Small";
					}
					else
					{
						thingDef.graphicData.texPath = "Things/Item/Resource/MeatFoodRaw/Meat_Big";
					}
					thingDef.defName = defName;
					if (current.race.meatLabel.NullOrEmpty())
					{
						thingDef.label = "MeatLabel".Translate(current.label);
					}
					else
					{
						thingDef.label = current.race.meatLabel;
					}
					thingDef.ingestible.sourceDef = current;
					current.race.meatDef = thingDef;
					_003C_003E2__current = thingDef;
					_003C_003E1__state = 1;
					return true;
				}
				_003C_003Em__Finally1();
				_003C_003E7__wrap1 = default(List<ThingDef>.Enumerator);
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
			((IDisposable)_003C_003E7__wrap1/*cast due to .constrained prefix*/).Dispose();
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<ThingDef> IEnumerable<ThingDef>.GetEnumerator()
		{
			_003CImpliedMeatDefs_003Ed__0 _003CImpliedMeatDefs_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CImpliedMeatDefs_003Ed__ = this;
			}
			else
			{
				_003CImpliedMeatDefs_003Ed__ = new _003CImpliedMeatDefs_003Ed__0(0);
			}
			_003CImpliedMeatDefs_003Ed__.hotReload = _003C_003E3__hotReload;
			return _003CImpliedMeatDefs_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<ThingDef>)this).GetEnumerator();
		}
	}

	[IteratorStateMachine(typeof(_003CImpliedMeatDefs_003Ed__0))]
	public static IEnumerable<ThingDef> ImpliedMeatDefs(bool hotReload = false)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CImpliedMeatDefs_003Ed__0(-2)
		{
			_003C_003E3__hotReload = hotReload
		};
	}
}
