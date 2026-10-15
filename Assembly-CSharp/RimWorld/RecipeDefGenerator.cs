using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld;

public static class RecipeDefGenerator
{
	[CompilerGenerated]
	private sealed class _003CDefsFromRecipeMakers_003Ed__1 : IEnumerable<RecipeDef>, IEnumerable, IEnumerator<RecipeDef>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private RecipeDef _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private bool hotReload;

		public bool _003C_003E3__hotReload;

		private IEnumerator<ThingDef> _003C_003E7__wrap1;

		private ThingDef _003Cdef_003E5__3;

		RecipeDef IEnumerator<RecipeDef>.Current
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
		public _003CDefsFromRecipeMakers_003Ed__1(int _003C_003E1__state)
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
			_003C_003E7__wrap1 = null;
			_003Cdef_003E5__3 = null;
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
					_003C_003E7__wrap1 = DefDatabase<ThingDef>.AllDefs.Where((ThingDef d) => d.recipeMaker != null).GetEnumerator();
					_003C_003E1__state = -3;
					break;
				case 1:
					_003C_003E1__state = -3;
					if (_003Cdef_003E5__3.recipeMaker.bulkRecipeCount > 0)
					{
						_003C_003E2__current = CreateRecipeDefFromMaker(_003Cdef_003E5__3, _003Cdef_003E5__3.recipeMaker.bulkRecipeCount, hotReload);
						_003C_003E1__state = 2;
						return true;
					}
					goto IL_00f4;
				case 2:
					{
						_003C_003E1__state = -3;
						goto IL_00f4;
					}
					IL_00f4:
					_003Cdef_003E5__3 = null;
					break;
				}
				if (_003C_003E7__wrap1.MoveNext())
				{
					_003Cdef_003E5__3 = _003C_003E7__wrap1.Current;
					_003C_003E2__current = CreateRecipeDefFromMaker(_003Cdef_003E5__3, 1, hotReload);
					_003C_003E1__state = 1;
					return true;
				}
				_003C_003Em__Finally1();
				_003C_003E7__wrap1 = null;
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
		IEnumerator<RecipeDef> IEnumerable<RecipeDef>.GetEnumerator()
		{
			_003CDefsFromRecipeMakers_003Ed__1 _003CDefsFromRecipeMakers_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CDefsFromRecipeMakers_003Ed__ = this;
			}
			else
			{
				_003CDefsFromRecipeMakers_003Ed__ = new _003CDefsFromRecipeMakers_003Ed__1(0);
			}
			_003CDefsFromRecipeMakers_003Ed__.hotReload = _003C_003E3__hotReload;
			return _003CDefsFromRecipeMakers_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<RecipeDef>)this).GetEnumerator();
		}
	}

	[CompilerGenerated]
	private sealed class _003CDrugAdministerDefs_003Ed__5 : IEnumerable<RecipeDef>, IEnumerable, IEnumerator<RecipeDef>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private RecipeDef _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private bool hotReload;

		public bool _003C_003E3__hotReload;

		private IEnumerator<ThingDef> _003C_003E7__wrap1;

		RecipeDef IEnumerator<RecipeDef>.Current
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
		public _003CDrugAdministerDefs_003Ed__5(int _003C_003E1__state)
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
			_003C_003E7__wrap1 = null;
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
					_003C_003E7__wrap1 = DefDatabase<ThingDef>.AllDefs.Where((ThingDef d) => d.IsDrug).GetEnumerator();
					_003C_003E1__state = -3;
					break;
				case 1:
					_003C_003E1__state = -3;
					break;
				}
				if (_003C_003E7__wrap1.MoveNext())
				{
					ThingDef current = _003C_003E7__wrap1.Current;
					string defName = "Administer_" + current.defName;
					RecipeDef recipeDef = (hotReload ? (DefDatabase<RecipeDef>.GetNamed(defName, errorOnFail: false) ?? new RecipeDef()) : new RecipeDef());
					recipeDef.defName = defName;
					recipeDef.label = "RecipeAdminister".Translate(current.label);
					recipeDef.jobString = "RecipeAdministerJobString".Translate(current.label);
					recipeDef.workerClass = typeof(Recipe_AdministerIngestible);
					recipeDef.targetsBodyPart = false;
					recipeDef.anesthetize = false;
					recipeDef.surgerySuccessChanceFactor = 99999f;
					recipeDef.modContentPack = current.modContentPack;
					recipeDef.workAmount = current.ingestible.baseIngestTicks;
					recipeDef.humanlikeOnly = current.ingestible.humanlikeOnly;
					IngredientCount ingredientCount = new IngredientCount();
					ingredientCount.SetBaseCount(1f);
					ingredientCount.filter.SetAllow(current, allow: true);
					recipeDef.ingredients.Add(ingredientCount);
					recipeDef.fixedIngredientFilter.SetAllow(current, allow: true);
					recipeDef.recipeUsers = new List<ThingDef>();
					foreach (ThingDef item in DefDatabase<ThingDef>.AllDefs.Where((ThingDef d) => d.category == ThingCategory.Pawn && d.race.IsFlesh))
					{
						recipeDef.recipeUsers.Add(item);
					}
					_003C_003E2__current = recipeDef;
					_003C_003E1__state = 1;
					return true;
				}
				_003C_003Em__Finally1();
				_003C_003E7__wrap1 = null;
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
		IEnumerator<RecipeDef> IEnumerable<RecipeDef>.GetEnumerator()
		{
			_003CDrugAdministerDefs_003Ed__5 _003CDrugAdministerDefs_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CDrugAdministerDefs_003Ed__ = this;
			}
			else
			{
				_003CDrugAdministerDefs_003Ed__ = new _003CDrugAdministerDefs_003Ed__5(0);
			}
			_003CDrugAdministerDefs_003Ed__.hotReload = _003C_003E3__hotReload;
			return _003CDrugAdministerDefs_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<RecipeDef>)this).GetEnumerator();
		}
	}

	[CompilerGenerated]
	private sealed class _003CImpliedRecipeDefs_003Ed__0 : IEnumerable<RecipeDef>, IEnumerable, IEnumerator<RecipeDef>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private RecipeDef _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private bool hotReload;

		public bool _003C_003E3__hotReload;

		private IEnumerator<RecipeDef> _003C_003E7__wrap1;

		RecipeDef IEnumerator<RecipeDef>.Current
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
		public _003CImpliedRecipeDefs_003Ed__0(int _003C_003E1__state)
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
			_003C_003E7__wrap1 = null;
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
					_003C_003E7__wrap1 = DefsFromRecipeMakers(hotReload).Concat(DrugAdministerDefs(hotReload)).GetEnumerator();
					_003C_003E1__state = -3;
					break;
				case 1:
					_003C_003E1__state = -3;
					break;
				}
				if (_003C_003E7__wrap1.MoveNext())
				{
					RecipeDef current = _003C_003E7__wrap1.Current;
					_003C_003E2__current = current;
					_003C_003E1__state = 1;
					return true;
				}
				_003C_003Em__Finally1();
				_003C_003E7__wrap1 = null;
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
		IEnumerator<RecipeDef> IEnumerable<RecipeDef>.GetEnumerator()
		{
			_003CImpliedRecipeDefs_003Ed__0 _003CImpliedRecipeDefs_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CImpliedRecipeDefs_003Ed__ = this;
			}
			else
			{
				_003CImpliedRecipeDefs_003Ed__ = new _003CImpliedRecipeDefs_003Ed__0(0);
			}
			_003CImpliedRecipeDefs_003Ed__.hotReload = _003C_003E3__hotReload;
			return _003CImpliedRecipeDefs_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<RecipeDef>)this).GetEnumerator();
		}
	}

	[IteratorStateMachine(typeof(_003CImpliedRecipeDefs_003Ed__0))]
	public static IEnumerable<RecipeDef> ImpliedRecipeDefs(bool hotReload = false)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CImpliedRecipeDefs_003Ed__0(-2)
		{
			_003C_003E3__hotReload = hotReload
		};
	}

	[IteratorStateMachine(typeof(_003CDefsFromRecipeMakers_003Ed__1))]
	private static IEnumerable<RecipeDef> DefsFromRecipeMakers(bool hotReload = false)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CDefsFromRecipeMakers_003Ed__1(-2)
		{
			_003C_003E3__hotReload = hotReload
		};
	}

	private static RecipeDef CreateRecipeDefFromMaker(ThingDef def, int adjustedCount = 1, bool hotReload = false)
	{
		RecipeMakerProperties recipeMaker = def.recipeMaker;
		string text = "Make_" + def.defName;
		if (adjustedCount != 1)
		{
			text += "Bulk";
		}
		RecipeDef recipeDef = (hotReload ? (DefDatabase<RecipeDef>.GetNamed(text, errorOnFail: false) ?? new RecipeDef()) : new RecipeDef());
		recipeDef.defName = text;
		string text2 = def.label;
		if (adjustedCount != 1)
		{
			text2 = text2 + " x" + adjustedCount;
		}
		if (string.IsNullOrEmpty(recipeMaker.label))
		{
			recipeDef.label = "RecipeMake".Translate(text2);
		}
		else
		{
			recipeDef.label = recipeMaker.label;
		}
		recipeDef.jobString = "RecipeMakeJobString".Translate(text2);
		recipeDef.modContentPack = def.modContentPack;
		recipeDef.displayPriority = recipeMaker.displayPriority + adjustedCount - 1;
		recipeDef.workAmount = recipeMaker.workAmount * adjustedCount;
		recipeDef.workSpeedStat = recipeMaker.workSpeedStat;
		recipeDef.efficiencyStat = recipeMaker.efficiencyStat;
		SetIngredients(recipeDef, def, adjustedCount);
		recipeDef.useIngredientsForColor = recipeMaker.useIngredientsForColor;
		if (def.costListForDifficulty != null)
		{
			recipeDef.regenerateOnDifficultyChange = true;
		}
		recipeDef.defaultIngredientFilter = recipeMaker.defaultIngredientFilter;
		recipeDef.products.Add(new ThingDefCountClass(def, recipeMaker.productCount * adjustedCount));
		recipeDef.targetCountAdjustment = recipeMaker.targetCountAdjustment * adjustedCount;
		recipeDef.skillRequirements = recipeMaker.skillRequirements.ListFullCopyOrNull();
		recipeDef.workSkill = recipeMaker.workSkill;
		recipeDef.workSkillLearnFactor = recipeMaker.workSkillLearnPerTick;
		recipeDef.requiredGiverWorkType = recipeMaker.requiredGiverWorkType;
		recipeDef.unfinishedThingDef = recipeMaker.unfinishedThingDef;
		recipeDef.recipeUsers = recipeMaker.recipeUsers.ListFullCopyOrNull();
		recipeDef.mechanitorOnlyRecipe = recipeMaker.mechanitorOnlyRecipe;
		recipeDef.effectWorking = recipeMaker.effectWorking;
		recipeDef.soundWorking = recipeMaker.soundWorking;
		recipeDef.researchPrerequisite = recipeMaker.researchPrerequisite;
		recipeDef.memePrerequisitesAny = recipeMaker.memePrerequisitesAny;
		recipeDef.researchPrerequisites = recipeMaker.researchPrerequisites;
		recipeDef.factionPrerequisiteTags = recipeMaker.factionPrerequisiteTags;
		recipeDef.fromIdeoBuildingPreceptOnly = recipeMaker.fromIdeoBuildingPreceptOnly;
		string[] items = recipeDef.products.Select((ThingDefCountClass p) => (p.count != 1) ? p.Label : Find.ActiveLanguageWorker.WithIndefiniteArticle(p.thingDef.label)).ToArray();
		recipeDef.description = "RecipeMakeDescription".Translate(items.ToCommaList(useAnd: true));
		recipeDef.descriptionHyperlinks = recipeDef.products.Select((ThingDefCountClass p) => new DefHyperlink(p.thingDef)).ToList();
		if (adjustedCount != 1 && recipeDef.workAmount < 0f)
		{
			recipeDef.workAmount = recipeDef.WorkAmountTotal(null) * (float)adjustedCount;
		}
		return recipeDef;
	}

	public static void SetIngredients(RecipeDef r, ThingDef def, int adjustedCount = 1)
	{
		r.ingredients.Clear();
		r.adjustedCount = adjustedCount;
		if (def.MadeFromStuff)
		{
			IngredientCount ingredientCount = new IngredientCount();
			ingredientCount.SetBaseCount(def.CostStuffCount * adjustedCount);
			ingredientCount.filter.SetAllowAllWhoCanMake(def);
			ingredientCount.filter.customSummary = def.stuffCategorySummary ?? def.stuffCategories.Select((StuffCategoryDef category) => category.noun).ToCommaListOr();
			r.ingredients.Add(ingredientCount);
			r.fixedIngredientFilter.SetAllowAllWhoCanMake(def);
			r.productHasIngredientStuff = true;
		}
		if (def.CostList == null)
		{
			return;
		}
		foreach (ThingDefCountClass cost in def.CostList)
		{
			IngredientCount ingredientCount2 = new IngredientCount();
			ingredientCount2.SetBaseCount(cost.count * adjustedCount);
			ingredientCount2.filter.SetAllow(cost.thingDef, allow: true);
			r.ingredients.Add(ingredientCount2);
		}
	}

	public static void ResetRecipeIngredientsForDifficulty()
	{
		foreach (RecipeDef item in DefDatabase<RecipeDef>.AllDefs.Where((RecipeDef x) => x.regenerateOnDifficultyChange))
		{
			SetIngredients(item, item.products[0].thingDef, item.adjustedCount);
		}
	}

	[IteratorStateMachine(typeof(_003CDrugAdministerDefs_003Ed__5))]
	private static IEnumerable<RecipeDef> DrugAdministerDefs(bool hotReload = false)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CDrugAdministerDefs_003Ed__5(-2)
		{
			_003C_003E3__hotReload = hotReload
		};
	}
}
