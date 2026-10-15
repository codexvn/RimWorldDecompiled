using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using RimWorld;
using UnityEngine;

namespace Verse;

public static class GenRecipe
{
	[CompilerGenerated]
	private sealed class _003CFinalizeGestatedPawns_003Ed__1 : IEnumerable<Thing>, IEnumerable, IEnumerator<Thing>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Thing _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private Bill_Mech bill;

		public Bill_Mech _003C_003E3__bill;

		private Pawn worker;

		public Pawn _003C_003E3__worker;

		private ThingStyleDef style;

		public ThingStyleDef _003C_003E3__style;

		private int? overrideGraphicIndex;

		public int? _003C_003E3__overrideGraphicIndex;

		Thing IEnumerator<Thing>.Current
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
		public _003CFinalizeGestatedPawns_003Ed__1(int _003C_003E1__state)
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
				_003C_003E2__current = PostProcessProduct(bill.Gestator.GestatingMech, bill.recipe, worker, bill.precept, style, overrideGraphicIndex);
				_003C_003E1__state = 1;
				return true;
			case 1:
				_003C_003E1__state = -1;
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
		IEnumerator<Thing> IEnumerable<Thing>.GetEnumerator()
		{
			_003CFinalizeGestatedPawns_003Ed__1 _003CFinalizeGestatedPawns_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CFinalizeGestatedPawns_003Ed__ = this;
			}
			else
			{
				_003CFinalizeGestatedPawns_003Ed__ = new _003CFinalizeGestatedPawns_003Ed__1(0);
			}
			_003CFinalizeGestatedPawns_003Ed__.bill = _003C_003E3__bill;
			_003CFinalizeGestatedPawns_003Ed__.worker = _003C_003E3__worker;
			_003CFinalizeGestatedPawns_003Ed__.style = _003C_003E3__style;
			_003CFinalizeGestatedPawns_003Ed__.overrideGraphicIndex = _003C_003E3__overrideGraphicIndex;
			return _003CFinalizeGestatedPawns_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<Thing>)this).GetEnumerator();
		}
	}

	[CompilerGenerated]
	private sealed class _003CMakeRecipeProducts_003Ed__0 : IEnumerable<Thing>, IEnumerable, IEnumerator<Thing>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Thing _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private RecipeDef recipeDef;

		public RecipeDef _003C_003E3__recipeDef;

		private Pawn worker;

		public Pawn _003C_003E3__worker;

		private IBillGiver billGiver;

		public IBillGiver _003C_003E3__billGiver;

		private Thing dominantIngredient;

		public Thing _003C_003E3__dominantIngredient;

		private List<Thing> ingredients;

		public List<Thing> _003C_003E3__ingredients;

		private Precept_ThingStyle precept;

		public Precept_ThingStyle _003C_003E3__precept;

		private ThingStyleDef style;

		public ThingStyleDef _003C_003E3__style;

		private int? overrideGraphicIndex;

		public int? _003C_003E3__overrideGraphicIndex;

		private float _003Cefficiency_003E5__2;

		private int _003Ci_003E5__3;

		private int _003Cj_003E5__4;

		private IEnumerator<Thing> _003C_003E7__wrap4;

		Thing IEnumerator<Thing>.Current
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
		public _003CMakeRecipeProducts_003Ed__0(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			switch (_003C_003E1__state)
			{
			case -3:
			case 2:
				try
				{
				}
				finally
				{
					_003C_003Em__Finally1();
				}
				break;
			case -4:
			case 3:
				try
				{
				}
				finally
				{
					_003C_003Em__Finally2();
				}
				break;
			case -5:
			case 4:
				try
				{
				}
				finally
				{
					_003C_003Em__Finally3();
				}
				break;
			}
			_003C_003E7__wrap4 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				Thing thing2;
				switch (_003C_003E1__state)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					if (recipeDef.efficiencyStat == null)
					{
						_003Cefficiency_003E5__2 = 1f;
					}
					else
					{
						_003Cefficiency_003E5__2 = worker.GetStatValue(recipeDef.efficiencyStat);
					}
					if (recipeDef.workTableEfficiencyStat != null && billGiver is Building_WorkTable thing)
					{
						_003Cefficiency_003E5__2 *= thing.GetStatValue(recipeDef.workTableEfficiencyStat);
					}
					if (recipeDef.products != null)
					{
						_003Ci_003E5__3 = 0;
						goto IL_01e3;
					}
					goto IL_01fe;
				case 1:
					_003C_003E1__state = -1;
					_003Ci_003E5__3++;
					goto IL_01e3;
				case 2:
					_003C_003E1__state = -3;
					goto IL_02df;
				case 3:
					_003C_003E1__state = -4;
					goto IL_036e;
				case 4:
					{
						_003C_003E1__state = -5;
						goto IL_0403;
					}
					IL_036e:
					if (_003C_003E7__wrap4.MoveNext())
					{
						Thing current = _003C_003E7__wrap4.Current;
						_003C_003E2__current = PostProcessProduct(current, recipeDef, worker, precept, style, overrideGraphicIndex);
						_003C_003E1__state = 3;
						return true;
					}
					_003C_003Em__Finally2();
					_003C_003E7__wrap4 = null;
					goto IL_041d;
					IL_02df:
					if (_003C_003E7__wrap4.MoveNext())
					{
						Thing current2 = _003C_003E7__wrap4.Current;
						_003C_003E2__current = PostProcessProduct(current2, recipeDef, worker, precept, style, overrideGraphicIndex);
						_003C_003E1__state = 2;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap4 = null;
					goto IL_041d;
					IL_041d:
					_003Cj_003E5__4++;
					goto IL_042f;
					IL_0403:
					if (_003C_003E7__wrap4.MoveNext())
					{
						Thing current3 = _003C_003E7__wrap4.Current;
						_003C_003E2__current = PostProcessProduct(current3, recipeDef, worker, precept, style, overrideGraphicIndex);
						_003C_003E1__state = 4;
						return true;
					}
					_003C_003Em__Finally3();
					_003C_003E7__wrap4 = null;
					goto IL_041d;
					IL_042f:
					if (_003Cj_003E5__4 < ingredients.Count)
					{
						thing2 = ingredients[_003Cj_003E5__4];
						switch (recipeDef.specialProducts[_003Ci_003E5__3])
						{
						case SpecialProductType.Butchery:
							break;
						case SpecialProductType.Smelted:
							goto IL_02fe;
						case SpecialProductType.StoneBlocks:
							goto IL_038d;
						default:
							goto IL_041d;
						}
						_003C_003E7__wrap4 = thing2.ButcherProducts(worker, _003Cefficiency_003E5__2).GetEnumerator();
						_003C_003E1__state = -3;
						goto IL_02df;
					}
					_003Ci_003E5__3++;
					goto IL_0457;
					IL_038d:
					_003C_003E7__wrap4 = thing2.StoneBlockProducts(worker, _003Cefficiency_003E5__2).GetEnumerator();
					_003C_003E1__state = -5;
					goto IL_0403;
					IL_01e3:
					if (_003Ci_003E5__3 < recipeDef.products.Count)
					{
						ThingDefCountClass thingDefCountClass = recipeDef.products[_003Ci_003E5__3];
						ThingDef stuff = (thingDefCountClass.thingDef.MadeFromStuff ? dominantIngredient.def : null);
						Thing thing3 = ThingMaker.MakeThing(thingDefCountClass.thingDef, stuff);
						thing3.stackCount = Mathf.CeilToInt((float)thingDefCountClass.count * _003Cefficiency_003E5__2);
						if (dominantIngredient != null && recipeDef.useIngredientsForColor)
						{
							thing3.SetColor(dominantIngredient.DrawColor, reportFailure: false);
						}
						CompIngredients compIngredients = thing3.TryGetComp<CompIngredients>();
						if (compIngredients != null)
						{
							for (int i = 0; i < ingredients.Count; i++)
							{
								compIngredients.RegisterIngredient(ingredients[i].def);
							}
						}
						thing3.Notify_RecipeProduced(worker);
						_003C_003E2__current = PostProcessProduct(thing3, recipeDef, worker, precept, style, overrideGraphicIndex);
						_003C_003E1__state = 1;
						return true;
					}
					goto IL_01fe;
					IL_01fe:
					if (recipeDef.specialProducts == null)
					{
						break;
					}
					_003Ci_003E5__3 = 0;
					goto IL_0457;
					IL_0457:
					if (_003Ci_003E5__3 >= recipeDef.specialProducts.Count)
					{
						break;
					}
					_003Cj_003E5__4 = 0;
					goto IL_042f;
					IL_02fe:
					_003C_003E7__wrap4 = thing2.SmeltProducts(_003Cefficiency_003E5__2).GetEnumerator();
					_003C_003E1__state = -4;
					goto IL_036e;
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
			if (_003C_003E7__wrap4 != null)
			{
				_003C_003E7__wrap4.Dispose();
			}
		}

		private void _003C_003Em__Finally2()
		{
			_003C_003E1__state = -1;
			if (_003C_003E7__wrap4 != null)
			{
				_003C_003E7__wrap4.Dispose();
			}
		}

		private void _003C_003Em__Finally3()
		{
			_003C_003E1__state = -1;
			if (_003C_003E7__wrap4 != null)
			{
				_003C_003E7__wrap4.Dispose();
			}
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<Thing> IEnumerable<Thing>.GetEnumerator()
		{
			_003CMakeRecipeProducts_003Ed__0 _003CMakeRecipeProducts_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CMakeRecipeProducts_003Ed__ = this;
			}
			else
			{
				_003CMakeRecipeProducts_003Ed__ = new _003CMakeRecipeProducts_003Ed__0(0);
			}
			_003CMakeRecipeProducts_003Ed__.recipeDef = _003C_003E3__recipeDef;
			_003CMakeRecipeProducts_003Ed__.worker = _003C_003E3__worker;
			_003CMakeRecipeProducts_003Ed__.ingredients = _003C_003E3__ingredients;
			_003CMakeRecipeProducts_003Ed__.dominantIngredient = _003C_003E3__dominantIngredient;
			_003CMakeRecipeProducts_003Ed__.billGiver = _003C_003E3__billGiver;
			_003CMakeRecipeProducts_003Ed__.precept = _003C_003E3__precept;
			_003CMakeRecipeProducts_003Ed__.style = _003C_003E3__style;
			_003CMakeRecipeProducts_003Ed__.overrideGraphicIndex = _003C_003E3__overrideGraphicIndex;
			return _003CMakeRecipeProducts_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<Thing>)this).GetEnumerator();
		}
	}

	[IteratorStateMachine(typeof(_003CMakeRecipeProducts_003Ed__0))]
	public static IEnumerable<Thing> MakeRecipeProducts(RecipeDef recipeDef, Pawn worker, List<Thing> ingredients, Thing dominantIngredient, IBillGiver billGiver, Precept_ThingStyle precept = null, ThingStyleDef style = null, int? overrideGraphicIndex = null)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CMakeRecipeProducts_003Ed__0(-2)
		{
			_003C_003E3__recipeDef = recipeDef,
			_003C_003E3__worker = worker,
			_003C_003E3__ingredients = ingredients,
			_003C_003E3__dominantIngredient = dominantIngredient,
			_003C_003E3__billGiver = billGiver,
			_003C_003E3__precept = precept,
			_003C_003E3__style = style,
			_003C_003E3__overrideGraphicIndex = overrideGraphicIndex
		};
	}

	[IteratorStateMachine(typeof(_003CFinalizeGestatedPawns_003Ed__1))]
	public static IEnumerable<Thing> FinalizeGestatedPawns(Bill_Mech bill, Pawn worker, ThingStyleDef style = null, int? overrideGraphicIndex = null)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CFinalizeGestatedPawns_003Ed__1(-2)
		{
			_003C_003E3__bill = bill,
			_003C_003E3__worker = worker,
			_003C_003E3__style = style,
			_003C_003E3__overrideGraphicIndex = overrideGraphicIndex
		};
	}

	private static Thing PostProcessProduct(Thing product, RecipeDef recipeDef, Pawn worker, Precept_ThingStyle precept = null, ThingStyleDef style = null, int? overrideGraphicIndex = null)
	{
		CompQuality compQuality = product.TryGetComp<CompQuality>();
		if (compQuality != null)
		{
			if (recipeDef.workSkill == null)
			{
				Log.Error(recipeDef?.ToString() + " needs workSkill because it creates a product with a quality.");
			}
			QualityCategory q = QualityUtility.GenerateQualityCreatedByPawn(worker, recipeDef.workSkill);
			compQuality.SetQuality(q, ArtGenerationContext.Colony);
		}
		CompArt compArt = product.TryGetComp<CompArt>();
		if (compArt != null)
		{
			compArt.JustCreatedBy(worker);
			if (compQuality != null && (int)compQuality.Quality >= 4)
			{
				TaleRecorder.RecordTale(TaleDefOf.CraftedArt, worker, product);
			}
		}
		if (compQuality != null)
		{
			QualityUtility.SendCraftNotification(product, worker);
		}
		if (worker.Ideo != null)
		{
			product.StyleDef = worker.Ideo.GetStyleFor(product.def);
		}
		if (precept != null)
		{
			product.StyleSourcePrecept = precept;
		}
		else if (style != null)
		{
			product.StyleDef = style;
		}
		else if (!product.def.randomStyle.NullOrEmpty() && Rand.Chance(product.def.randomStyleChance))
		{
			product.SetStyleDef(product.def.randomStyle.RandomElementByWeight((ThingStyleChance x) => x.Chance).StyleDef);
		}
		product.overrideGraphicIndex = overrideGraphicIndex;
		if (product.def.Minifiable)
		{
			product = product.MakeMinified();
		}
		return product;
	}
}
