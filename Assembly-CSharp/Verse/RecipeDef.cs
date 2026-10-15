using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using RimWorld;
using UnityEngine;

namespace Verse;

public class RecipeDef : Def
{
	[CompilerGenerated]
	private sealed class _003CConfigErrors_003Ed__107 : IEnumerable<string>, IEnumerable, IEnumerator<string>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private string _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public RecipeDef _003C_003E4__this;

		private IEnumerator<string> _003C_003E7__wrap1;

		string IEnumerator<string>.Current
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
		public _003CConfigErrors_003Ed__107(int _003C_003E1__state)
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
				int num = _003C_003E1__state;
				RecipeDef recipeDef = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = ((Def)recipeDef).ConfigErrors().GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_006f;
				case 1:
					_003C_003E1__state = -3;
					goto IL_006f;
				case 2:
					{
						_003C_003E1__state = -1;
						break;
					}
					IL_006f:
					if (_003C_003E7__wrap1.MoveNext())
					{
						string current = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = null;
					if (recipeDef.workerClass == null)
					{
						_003C_003E2__current = "workerClass is null.";
						_003C_003E1__state = 2;
						return true;
					}
					break;
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
		IEnumerator<string> IEnumerable<string>.GetEnumerator()
		{
			_003CConfigErrors_003Ed__107 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CConfigErrors_003Ed__107(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<string>)this).GetEnumerator();
		}
	}

	[CompilerGenerated]
	private sealed class _003CPotentiallyMissingIngredients_003Ed__105 : IEnumerable<ThingDef>, IEnumerable, IEnumerator<ThingDef>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private ThingDef _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public RecipeDef _003C_003E4__this;

		private Map map;

		public Map _003C_003E3__map;

		private Pawn billDoer;

		public Pawn _003C_003E3__billDoer;

		private int _003Ci_003E5__2;

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
		public _003CPotentiallyMissingIngredients_003Ed__105(int _003C_003E1__state)
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
			int num = _003C_003E1__state;
			RecipeDef CS_0024_003C_003E8__locals4 = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				_003Ci_003E5__2 = 0;
				goto IL_0178;
			case 1:
				_003C_003E1__state = -1;
				goto IL_0166;
			case 2:
				{
					_003C_003E1__state = -1;
					goto IL_0166;
				}
				IL_0178:
				if (_003Ci_003E5__2 < CS_0024_003C_003E8__locals4.ingredients.Count)
				{
					IngredientCount ingredientCount = CS_0024_003C_003E8__locals4.ingredients[_003Ci_003E5__2];
					bool flag = false;
					List<Thing> list = map.listerThings.ThingsInGroup(ThingRequestGroup.HaulableEver);
					for (int i = 0; i < list.Count; i++)
					{
						Thing thing = list[i];
						if ((billDoer == null || !thing.IsForbidden(billDoer)) && !thing.Position.Fogged(map) && (ingredientCount.IsFixedIngredient || CS_0024_003C_003E8__locals4.fixedIngredientFilter.Allows(thing)) && ingredientCount.filter.Allows(thing))
						{
							flag = true;
							break;
						}
					}
					if (!flag)
					{
						if (ingredientCount.IsFixedIngredient)
						{
							_003C_003E2__current = ingredientCount.filter.AllowedThingDefs.First();
							_003C_003E1__state = 1;
							return true;
						}
						ThingDef thingDef = ingredientCount.filter.AllowedThingDefs.OrderBy((ThingDef x) => x.BaseMarketValue).FirstOrDefault((ThingDef x) => CS_0024_003C_003E8__locals4.fixedIngredientFilter.Allows(x));
						if (thingDef != null)
						{
							_003C_003E2__current = thingDef;
							_003C_003E1__state = 2;
							return true;
						}
					}
					goto IL_0166;
				}
				return false;
				IL_0166:
				_003Ci_003E5__2++;
				goto IL_0178;
			}
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
		IEnumerator<ThingDef> IEnumerable<ThingDef>.GetEnumerator()
		{
			_003CPotentiallyMissingIngredients_003Ed__105 _003CPotentiallyMissingIngredients_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CPotentiallyMissingIngredients_003Ed__ = this;
			}
			else
			{
				_003CPotentiallyMissingIngredients_003Ed__ = new _003CPotentiallyMissingIngredients_003Ed__105(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			_003CPotentiallyMissingIngredients_003Ed__.billDoer = _003C_003E3__billDoer;
			_003CPotentiallyMissingIngredients_003Ed__.map = _003C_003E3__map;
			return _003CPotentiallyMissingIngredients_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<ThingDef>)this).GetEnumerator();
		}
	}

	[CompilerGenerated]
	private sealed class _003CSpecialDisplayStats_003Ed__115 : IEnumerable<StatDrawEntry>, IEnumerable, IEnumerator<StatDrawEntry>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private StatDrawEntry _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public RecipeDef _003C_003E4__this;

		private IEnumerator<StatDrawEntry> _003C_003E7__wrap1;

		StatDrawEntry IEnumerator<StatDrawEntry>.Current
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
		public _003CSpecialDisplayStats_003Ed__115(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			int num = _003C_003E1__state;
			if (num == -3 || num == 12)
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
				RecipeDef CS_0024_003C_003E8__locals24 = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					if (CS_0024_003C_003E8__locals24.workSkill != null)
					{
						_003C_003E2__current = new StatDrawEntry(StatCategoryDefOf.Basics, "Skill".Translate(), CS_0024_003C_003E8__locals24.workSkill.LabelCap, "Stat_Recipe_Skill_Desc".Translate(), 4404);
						_003C_003E1__state = 1;
						return true;
					}
					goto IL_00ba;
				case 1:
					_003C_003E1__state = -1;
					goto IL_00ba;
				case 2:
					_003C_003E1__state = -1;
					goto IL_0136;
				case 3:
					_003C_003E1__state = -1;
					goto IL_01c0;
				case 4:
				{
					_003C_003E1__state = -1;
					float num2 = CS_0024_003C_003E8__locals24.WorkAmountTotal(null);
					if (num2 > 0f)
					{
						_003C_003E2__current = new StatDrawEntry(StatCategoryDefOf.Basics, StatDefOf.WorkToMake.LabelCap, num2.ToStringWorkAmount(), StatDefOf.WorkToMake.description, StatDefOf.WorkToMake.displayPriorityInCategory);
						_003C_003E1__state = 5;
						return true;
					}
					goto IL_02c0;
				}
				case 5:
					_003C_003E1__state = -1;
					goto IL_02c0;
				case 6:
					_003C_003E1__state = -1;
					goto IL_0324;
				case 7:
					_003C_003E1__state = -1;
					goto IL_0388;
				case 8:
					_003C_003E1__state = -1;
					goto IL_050e;
				case 9:
					_003C_003E1__state = -1;
					if (CS_0024_003C_003E8__locals24.deathOnFailedSurgeryChance >= 99999f)
					{
						_003C_003E2__current = new StatDrawEntry(StatCategoryDefOf.Surgery, "SurgeryDeathOnFailChance".Translate(), "100%", "Stat_Thing_Surgery_DeathOnFailChance_Desc".Translate(), 4101);
						_003C_003E1__state = 10;
						return true;
					}
					_003C_003E2__current = new StatDrawEntry(StatCategoryDefOf.Surgery, "SurgeryDeathOnFailChance".Translate(), CS_0024_003C_003E8__locals24.deathOnFailedSurgeryChance.ToStringPercent(), "Stat_Thing_Surgery_DeathOnFailChance_Desc".Translate(), 4101);
					_003C_003E1__state = 11;
					return true;
				case 10:
					_003C_003E1__state = -1;
					goto IL_050e;
				case 11:
					_003C_003E1__state = -1;
					goto IL_050e;
				case 12:
					{
						_003C_003E1__state = -3;
						goto IL_055f;
					}
					IL_02c0:
					if (CS_0024_003C_003E8__locals24.workSpeedStat != null)
					{
						_003C_003E2__current = new StatDrawEntry(StatCategoryDefOf.Basics, "WorkSpeedStat".Translate(), CS_0024_003C_003E8__locals24.workSpeedStat.LabelCap, "Stat_Recipe_WorkSpeedStat_Desc".Translate(), 4402);
						_003C_003E1__state = 6;
						return true;
					}
					goto IL_0324;
					IL_01c0:
					if (CS_0024_003C_003E8__locals24.products != null && CS_0024_003C_003E8__locals24.products.Count > 0)
					{
						_003C_003E2__current = new StatDrawEntry(StatCategoryDefOf.Basics, "Products".Translate(), CS_0024_003C_003E8__locals24.products.Select((ThingDefCountClass pr) => pr.Summary).ToCommaList(), "Stat_Recipe_Products_Desc".Translate(), 4405, null, CS_0024_003C_003E8__locals24.GetProductsHyperlinks());
						_003C_003E1__state = 4;
						return true;
					}
					goto IL_02c0;
					IL_0324:
					if (CS_0024_003C_003E8__locals24.efficiencyStat != null)
					{
						_003C_003E2__current = new StatDrawEntry(StatCategoryDefOf.Basics, "EfficiencyStat".Translate(), CS_0024_003C_003E8__locals24.efficiencyStat.LabelCap, "Stat_Recipe_EfficiencyStat_Desc".Translate(), 4401);
						_003C_003E1__state = 7;
						return true;
					}
					goto IL_0388;
					IL_055f:
					if (_003C_003E7__wrap1.MoveNext())
					{
						StatDrawEntry current = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 12;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = null;
					break;
					IL_00ba:
					if (!CS_0024_003C_003E8__locals24.ingredients.NullOrEmpty())
					{
						_003C_003E2__current = new StatDrawEntry(StatCategoryDefOf.Basics, "Ingredients".Translate(), CS_0024_003C_003E8__locals24.ingredients.Select((IngredientCount ic) => ic.SummaryFor(CS_0024_003C_003E8__locals24)).ToCommaList(), "Stat_Recipe_Ingredients_Desc".Translate(), 4405, null, CS_0024_003C_003E8__locals24.GetIngredientsHyperlinks());
						_003C_003E1__state = 2;
						return true;
					}
					goto IL_0136;
					IL_050e:
					if (CS_0024_003C_003E8__locals24.addsHediff == null)
					{
						break;
					}
					_003C_003E7__wrap1 = MedicalRecipesUtility.GetMedicalStatsFromRecipeDefs(Gen.YieldSingle(CS_0024_003C_003E8__locals24)).GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_055f;
					IL_0136:
					if (!CS_0024_003C_003E8__locals24.skillRequirements.NullOrEmpty())
					{
						_003C_003E2__current = new StatDrawEntry(StatCategoryDefOf.Basics, "SkillRequirements".Translate(), CS_0024_003C_003E8__locals24.skillRequirements.Select((SkillRequirement sr) => sr.Summary).ToCommaList(), "Stat_Recipe_SkillRequirements_Desc".Translate(), 4403);
						_003C_003E1__state = 3;
						return true;
					}
					goto IL_01c0;
					IL_0388:
					if (CS_0024_003C_003E8__locals24.IsSurgery)
					{
						if (CS_0024_003C_003E8__locals24.surgerySuccessChanceFactor >= 99999f)
						{
							_003C_003E2__current = new StatDrawEntry(StatCategoryDefOf.Surgery, "SurgerySuccessChanceFactor".Translate(), "Stat_Thing_Surgery_SuccessChanceFactor_CantFail".Translate(), "Stat_Thing_Surgery_SuccessChanceFactor_CantFail_Desc".Translate(), 4102);
							_003C_003E1__state = 8;
							return true;
						}
						_003C_003E2__current = new StatDrawEntry(StatCategoryDefOf.Surgery, "SurgerySuccessChanceFactor".Translate(), CS_0024_003C_003E8__locals24.surgerySuccessChanceFactor.ToStringPercent(), "Stat_Thing_Surgery_SuccessChanceFactor_Desc".Translate(), 4102);
						_003C_003E1__state = 9;
						return true;
					}
					goto IL_050e;
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
		IEnumerator<StatDrawEntry> IEnumerable<StatDrawEntry>.GetEnumerator()
		{
			_003CSpecialDisplayStats_003Ed__115 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CSpecialDisplayStats_003Ed__115(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<StatDrawEntry>)this).GetEnumerator();
		}
	}

	[CompilerGenerated]
	private sealed class _003Cget_AllRecipeUsers_003Ed__95 : IEnumerable<ThingDef>, IEnumerable, IEnumerator<ThingDef>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private ThingDef _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public RecipeDef _003C_003E4__this;

		private List<ThingDef> _003CthingDefs_003E5__2;

		private int _003Ci_003E5__3;

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
		public _003Cget_AllRecipeUsers_003Ed__95(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			_003CthingDefs_003E5__2 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			int num = _003C_003E1__state;
			RecipeDef recipeDef = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				if (recipeDef.recipeUsers != null)
				{
					_003Ci_003E5__3 = 0;
					goto IL_0071;
				}
				goto IL_0084;
			case 1:
				_003C_003E1__state = -1;
				_003Ci_003E5__3++;
				goto IL_0071;
			case 2:
				{
					_003C_003E1__state = -1;
					goto IL_00f5;
				}
				IL_00f5:
				_003Ci_003E5__3++;
				goto IL_0105;
				IL_0071:
				if (_003Ci_003E5__3 < recipeDef.recipeUsers.Count)
				{
					_003C_003E2__current = recipeDef.recipeUsers[_003Ci_003E5__3];
					_003C_003E1__state = 1;
					return true;
				}
				goto IL_0084;
				IL_0084:
				_003CthingDefs_003E5__2 = DefDatabase<ThingDef>.AllDefsListForReading;
				_003Ci_003E5__3 = 0;
				goto IL_0105;
				IL_0105:
				if (_003Ci_003E5__3 < _003CthingDefs_003E5__2.Count)
				{
					if (_003CthingDefs_003E5__2[_003Ci_003E5__3].recipes != null && _003CthingDefs_003E5__2[_003Ci_003E5__3].recipes.Contains(recipeDef))
					{
						_003C_003E2__current = _003CthingDefs_003E5__2[_003Ci_003E5__3];
						_003C_003E1__state = 2;
						return true;
					}
					goto IL_00f5;
				}
				return false;
			}
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
		IEnumerator<ThingDef> IEnumerable<ThingDef>.GetEnumerator()
		{
			_003Cget_AllRecipeUsers_003Ed__95 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003Cget_AllRecipeUsers_003Ed__95(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<ThingDef>)this).GetEnumerator();
		}
	}

	public Type workerClass = typeof(RecipeWorker);

	public Type workerCounterClass = typeof(RecipeWorkerCounter);

	[MustTranslate]
	public string jobString = "Doing an unknown recipe.";

	public WorkTypeDef requiredGiverWorkType;

	public float workAmount = -1f;

	public StatDef workSpeedStat;

	public StatDef efficiencyStat;

	public StatDef workTableEfficiencyStat;

	public StatDef workTableSpeedStat;

	public List<IngredientCount> ingredients = new List<IngredientCount>();

	public ThingFilter fixedIngredientFilter = new ThingFilter();

	public ThingFilter defaultIngredientFilter;

	public bool allowMixingIngredients;

	public bool ignoreIngredientCountTakeEntireStacks;

	private Type ingredientValueGetterClass = typeof(IngredientValueGetter_Volume);

	public List<SpecialThingFilterDef> forceHiddenSpecialFilters;

	public bool autoStripCorpses = true;

	public bool interruptIfIngredientIsRotting;

	public List<ThingDefCountClass> products = new List<ThingDefCountClass>();

	public List<SpecialProductType> specialProducts;

	public bool productHasIngredientStuff;

	public bool useIngredientsForColor = true;

	public int targetCountAdjustment = 1;

	public ThingDef unfinishedThingDef;

	public List<SkillRequirement> skillRequirements;

	public SkillDef workSkill;

	public float workSkillLearnFactor = 1f;

	public EffecterDef effectWorking;

	public SoundDef soundWorking;

	private ThingDef uiIconThing;

	[NoTranslate]
	private string uiIconPath;

	public List<ThingDef> recipeUsers;

	public List<BodyPartDef> appliedOnFixedBodyParts = new List<BodyPartDef>();

	public List<BodyPartGroupDef> appliedOnFixedBodyPartGroups = new List<BodyPartGroupDef>();

	public HediffDef addsHediff;

	public HediffDef removesHediff;

	public HediffDef addsHediffOnFailure;

	public HediffDef changesHediffLevel;

	public List<string> incompatibleWithHediffTags;

	public int hediffLevelOffset;

	public bool hideBodyPartNames;

	public bool isViolation;

	[MustTranslate]
	public string successfullyRemovedHediffMessage;

	public float surgerySuccessChanceFactor = 1f;

	public float deathOnFailedSurgeryChance;

	public bool targetsBodyPart = true;

	public bool anesthetize = true;

	public int minPartCount = -1;

	public bool surgeryIgnoreEnvironment;

	public SurgeryOutcomeEffectDef surgeryOutcomeEffect;

	public Gender? genderPrerequisite;

	public bool mustBeFertile;

	public bool allowedForQuestLodgers = true;

	public int minAllowedAge = -1;

	public DevelopmentalStage? developmentalStageFilter;

	public bool humanlikeOnly;

	public List<MutantDef> mutantPrerequisite;

	public List<MutantDef> mutantBlacklist;

	public float smeltingWorkAmount = -1f;

	public ResearchProjectDef researchPrerequisite;

	public List<MemeDef> memePrerequisitesAny;

	public List<ResearchProjectDef> researchPrerequisites;

	[NoTranslate]
	public List<string> factionPrerequisiteTags;

	public bool fromIdeoBuildingPreceptOnly;

	public ConceptDef conceptLearned;

	public bool dontShowIfAnyIngredientMissing;

	public int displayPriority = 99999;

	public int formingTicks;

	public bool mechanitorOnlyRecipe;

	public int gestationCycles;

	public bool mechResurrection;

	[Unsaved(false)]
	private RecipeWorker workerInt;

	[Unsaved(false)]
	private RecipeWorkerCounter workerCounterInt;

	[Unsaved(false)]
	private IngredientValueGetter ingredientValueGetterInt;

	[Unsaved(false)]
	private List<ThingDef> premultipliedSmallIngredients;

	[Unsaved(false)]
	public bool regenerateOnDifficultyChange;

	[Unsaved(false)]
	public int adjustedCount = 1;

	[Unsaved(false)]
	private Texture2D cachedIcon;

	[Unsaved(false)]
	private bool resolvedCachedIcon;

	[Unsaved(false)]
	private bool? isSurgeryCached;

	public ThingDef UIIconThing => uiIconThing ?? ProducedThingDef;

	public Texture2D UIIcon
	{
		get
		{
			if (!resolvedCachedIcon)
			{
				resolvedCachedIcon = true;
				if (!uiIconPath.NullOrEmpty())
				{
					cachedIcon = ContentFinder<Texture2D>.Get(uiIconPath);
				}
			}
			return cachedIcon;
		}
	}

	public RecipeWorker Worker
	{
		get
		{
			if (workerInt == null)
			{
				workerInt = (RecipeWorker)Activator.CreateInstance(workerClass);
				workerInt.recipe = this;
			}
			return workerInt;
		}
	}

	public RecipeWorkerCounter WorkerCounter
	{
		get
		{
			if (workerCounterInt == null)
			{
				workerCounterInt = (RecipeWorkerCounter)Activator.CreateInstance(workerCounterClass);
				workerCounterInt.recipe = this;
			}
			return workerCounterInt;
		}
	}

	public IngredientValueGetter IngredientValueGetter
	{
		get
		{
			if (ingredientValueGetterInt == null)
			{
				ingredientValueGetterInt = (IngredientValueGetter)Activator.CreateInstance(ingredientValueGetterClass);
			}
			return ingredientValueGetterInt;
		}
	}

	public bool AvailableNow
	{
		get
		{
			if (researchPrerequisite != null && !researchPrerequisite.IsFinished)
			{
				return false;
			}
			if (memePrerequisitesAny != null)
			{
				bool flag = false;
				foreach (MemeDef item in memePrerequisitesAny)
				{
					if (Faction.OfPlayer.ideos.HasAnyIdeoWithMeme(item))
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					return false;
				}
			}
			if (researchPrerequisites != null && researchPrerequisites.Any((ResearchProjectDef r) => !r.IsFinished))
			{
				return false;
			}
			bool unlockedByIdeo;
			if (factionPrerequisiteTags != null && factionPrerequisiteTags.Any((string tag) => Faction.OfPlayer.def.recipePrerequisiteTags == null || !Faction.OfPlayer.def.recipePrerequisiteTags.Contains(tag)))
			{
				unlockedByIdeo = false;
				Check();
				if (!unlockedByIdeo)
				{
					return false;
				}
			}
			if (fromIdeoBuildingPreceptOnly && (!ModsConfig.IdeologyActive || !IdeoUtility.PlayerHasPreceptForBuilding(ProducedThingDef)))
			{
				return false;
			}
			return true;
			void Check()
			{
				foreach (Ideo allIdeo in Faction.OfPlayer.ideos.AllIdeos)
				{
					foreach (Precept_Role item2 in allIdeo.RolesListForReading)
					{
						if (item2.apparelRequirements != null)
						{
							foreach (PreceptApparelRequirement apparelRequirement in item2.apparelRequirements)
							{
								ThingDef thingDef = apparelRequirement.requirement.AllRequiredApparel().FirstOrDefault();
								if (thingDef == null)
								{
									Log.Error("Apparel requirement for role " + item2.Label + " is null");
								}
								foreach (ThingDefCountClass product in products)
								{
									if (product.thingDef == thingDef)
									{
										unlockedByIdeo = true;
										return;
									}
								}
							}
						}
					}
				}
			}
		}
	}

	public string MinSkillString
	{
		get
		{
			StringBuilder stringBuilder = new StringBuilder();
			bool flag = false;
			if (skillRequirements != null)
			{
				for (int i = 0; i < skillRequirements.Count; i++)
				{
					SkillRequirement skillRequirement = skillRequirements[i];
					stringBuilder.AppendLine("   " + skillRequirement.skill.skillLabel.CapitalizeFirst() + ": " + skillRequirement.minLevel);
					flag = true;
				}
			}
			if (!flag)
			{
				stringBuilder.AppendLine("   (" + "NoneLower".Translate() + ")");
			}
			return stringBuilder.ToString();
		}
	}

	public IEnumerable<ThingDef> AllRecipeUsers
	{
		[IteratorStateMachine(typeof(_003Cget_AllRecipeUsers_003Ed__95))]
		get
		{
			//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
			return new _003Cget_AllRecipeUsers_003Ed__95(-2)
			{
				_003C_003E4__this = this
			};
		}
	}

	public bool UsesUnfinishedThing => unfinishedThingDef != null;

	public bool IsSurgery
	{
		get
		{
			if (!isSurgeryCached.HasValue)
			{
				isSurgeryCached = false;
				foreach (ThingDef allRecipeUser in AllRecipeUsers)
				{
					if (allRecipeUser.category == ThingCategory.Pawn)
					{
						isSurgeryCached = true;
						break;
					}
				}
			}
			return isSurgeryCached.Value;
		}
	}

	public ThingDef ProducedThingDef
	{
		get
		{
			if (specialProducts != null)
			{
				return null;
			}
			if (products == null || products.Count != 1)
			{
				return null;
			}
			return products[0].thingDef;
		}
	}

	public bool AvailableOnNow(Thing thing, BodyPartRecord part = null)
	{
		return Worker.AvailableOnNow(thing, part);
	}

	public float WorkAmountTotal(Thing thing)
	{
		if (thing == null)
		{
			return WorkAmountForStuff(null);
		}
		if (this == RecipeDefOf.SmeltOrDestroyThing && thing.Smeltable)
		{
			return smeltingWorkAmount;
		}
		return WorkAmountForStuff(thing.Stuff);
	}

	public float WorkAmountForStuff(ThingDef stuff)
	{
		if (workAmount >= 0f)
		{
			return workAmount;
		}
		return products[0].thingDef.GetStatValueAbstract(StatDefOf.WorkToMake, stuff);
	}

	[IteratorStateMachine(typeof(_003CPotentiallyMissingIngredients_003Ed__105))]
	public IEnumerable<ThingDef> PotentiallyMissingIngredients(Pawn billDoer, Map map)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CPotentiallyMissingIngredients_003Ed__105(-2)
		{
			_003C_003E4__this = this,
			_003C_003E3__billDoer = billDoer,
			_003C_003E3__map = map
		};
	}

	public bool IsIngredient(ThingDef th)
	{
		for (int i = 0; i < ingredients.Count; i++)
		{
			if (ingredients[i].filter.Allows(th) && (ingredients[i].IsFixedIngredient || fixedIngredientFilter.Allows(th)))
			{
				return true;
			}
		}
		return false;
	}

	[IteratorStateMachine(typeof(_003CConfigErrors_003Ed__107))]
	public override IEnumerable<string> ConfigErrors()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CConfigErrors_003Ed__107(-2)
		{
			_003C_003E4__this = this
		};
	}

	public override void ResolveReferences()
	{
		base.ResolveReferences();
		DeepProfiler.Start("Stat refs");
		try
		{
			if (workTableSpeedStat == null)
			{
				workTableSpeedStat = StatDefOf.WorkTableWorkSpeedFactor;
			}
			if (workTableEfficiencyStat == null)
			{
				workTableEfficiencyStat = StatDefOf.WorkTableEfficiencyFactor;
			}
		}
		finally
		{
			DeepProfiler.End();
		}
		DeepProfiler.Start("ingredients reference resolve");
		try
		{
			for (int i = 0; i < ingredients.Count; i++)
			{
				ingredients[i].ResolveReferences();
			}
		}
		finally
		{
			DeepProfiler.End();
		}
		DeepProfiler.Start("fixedIngredientFilter.ResolveReferences()");
		try
		{
			fixedIngredientFilter?.ResolveReferences();
		}
		finally
		{
			DeepProfiler.End();
		}
		DeepProfiler.Start("defaultIngredientFilter setup");
		try
		{
			if (defaultIngredientFilter == null)
			{
				defaultIngredientFilter = new ThingFilter();
				if (fixedIngredientFilter != null)
				{
					defaultIngredientFilter.CopyAllowancesFrom(fixedIngredientFilter);
				}
			}
		}
		finally
		{
			DeepProfiler.End();
		}
		DeepProfiler.Start("defaultIngredientFilter.ResolveReferences()");
		try
		{
			defaultIngredientFilter.ResolveReferences();
		}
		finally
		{
			DeepProfiler.End();
		}
	}

	public bool CompatibleWithHediff(HediffDef hediffDef)
	{
		if (incompatibleWithHediffTags.NullOrEmpty() || hediffDef.tags.NullOrEmpty())
		{
			return true;
		}
		for (int i = 0; i < incompatibleWithHediffTags.Count; i++)
		{
			for (int j = 0; j < hediffDef.tags.Count; j++)
			{
				if (incompatibleWithHediffTags[i].Equals(hediffDef.tags[j], StringComparison.InvariantCultureIgnoreCase))
				{
					return false;
				}
			}
		}
		return true;
	}

	public bool PawnSatisfiesSkillRequirements(Pawn pawn)
	{
		return FirstSkillRequirementPawnDoesntSatisfy(pawn) == null;
	}

	public SkillRequirement FirstSkillRequirementPawnDoesntSatisfy(Pawn pawn)
	{
		if (skillRequirements == null)
		{
			return null;
		}
		for (int i = 0; i < skillRequirements.Count; i++)
		{
			if (!skillRequirements[i].PawnSatisfies(pawn))
			{
				return skillRequirements[i];
			}
		}
		return null;
	}

	public List<ThingDef> GetPremultipliedSmallIngredients()
	{
		if (premultipliedSmallIngredients != null)
		{
			return premultipliedSmallIngredients;
		}
		premultipliedSmallIngredients = (from td in ingredients.SelectMany((IngredientCount ingredient) => ingredient.filter.AllowedThingDefs)
			where td.smallVolume
			select td).Distinct().ToList();
		bool flag = true;
		while (flag)
		{
			flag = false;
			for (int i = 0; i < ingredients.Count; i++)
			{
				if (!ingredients[i].filter.AllowedThingDefs.Any((ThingDef td) => !premultipliedSmallIngredients.Contains(td)))
				{
					continue;
				}
				foreach (ThingDef allowedThingDef in ingredients[i].filter.AllowedThingDefs)
				{
					flag |= premultipliedSmallIngredients.Remove(allowedThingDef);
				}
			}
		}
		return premultipliedSmallIngredients;
	}

	private IEnumerable<Dialog_InfoCard.Hyperlink> GetIngredientsHyperlinks()
	{
		return Dialog_InfoCard.DefsToHyperlinks(from i in ingredients
			where i.IsFixedIngredient
			select i.FixedIngredient into i
			where i != null
			select i);
	}

	private IEnumerable<Dialog_InfoCard.Hyperlink> GetProductsHyperlinks()
	{
		return Dialog_InfoCard.DefsToHyperlinks(products.Select((ThingDefCountClass i) => i.thingDef));
	}

	[IteratorStateMachine(typeof(_003CSpecialDisplayStats_003Ed__115))]
	public override IEnumerable<StatDrawEntry> SpecialDisplayStats(StatRequest req)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CSpecialDisplayStats_003Ed__115(-2)
		{
			_003C_003E4__this = this
		};
	}
}
