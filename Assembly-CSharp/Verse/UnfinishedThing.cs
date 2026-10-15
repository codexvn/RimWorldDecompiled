using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using RimWorld;
using UnityEngine;

namespace Verse;

public class UnfinishedThing : ThingWithComps
{
	[CompilerGenerated]
	private sealed class _003CGetGizmos_003Ed__28 : IEnumerable<Gizmo>, IEnumerable, IEnumerator<Gizmo>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Gizmo _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public UnfinishedThing _003C_003E4__this;

		private IEnumerator<Gizmo> _003C_003E7__wrap1;

		Gizmo IEnumerator<Gizmo>.Current
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
		public _003CGetGizmos_003Ed__28(int _003C_003E1__state)
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
				UnfinishedThing CS_0024_003C_003E8__locals5 = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = ((ThingWithComps)CS_0024_003C_003E8__locals5).GetGizmos().GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_0078;
				case 1:
					_003C_003E1__state = -3;
					goto IL_0078;
				case 2:
					_003C_003E1__state = -1;
					if (CS_0024_003C_003E8__locals5.Initialized && DebugSettings.ShowDevGizmos && !CS_0024_003C_003E8__locals5.debugCompleted)
					{
						_003C_003E2__current = new Command_Action
						{
							defaultLabel = "DEV: Complete",
							action = delegate
							{
								CS_0024_003C_003E8__locals5.debugCompleted = true;
							}
						};
						_003C_003E1__state = 3;
						return true;
					}
					break;
				case 3:
					{
						_003C_003E1__state = -1;
						break;
					}
					IL_0078:
					if (_003C_003E7__wrap1.MoveNext())
					{
						Gizmo current = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = null;
					_003C_003E2__current = new Command_Action
					{
						defaultLabel = "CommandCancelConstructionLabel".Translate(),
						defaultDesc = "CommandCancelConstructionDesc".Translate(),
						icon = ContentFinder<Texture2D>.Get("UI/Designators/Cancel"),
						hotKey = KeyBindingDefOf.Designator_Cancel,
						action = delegate
						{
							CS_0024_003C_003E8__locals5.Destroy(DestroyMode.Cancel);
						}
					};
					_003C_003E1__state = 2;
					return true;
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
		IEnumerator<Gizmo> IEnumerable<Gizmo>.GetEnumerator()
		{
			_003CGetGizmos_003Ed__28 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CGetGizmos_003Ed__28(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<Gizmo>)this).GetEnumerator();
		}
	}

	[CompilerGenerated]
	private sealed class _003CSpecialDisplayStats_003Ed__32 : IEnumerable<StatDrawEntry>, IEnumerable, IEnumerator<StatDrawEntry>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private StatDrawEntry _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public UnfinishedThing _003C_003E4__this;

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
		public _003CSpecialDisplayStats_003Ed__32(int _003C_003E1__state)
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
				UnfinishedThing unfinishedThing = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = ((ThingWithComps)unfinishedThing).SpecialDisplayStats().GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_0072;
				case 1:
					_003C_003E1__state = -3;
					goto IL_0072;
				case 2:
					{
						_003C_003E1__state = -1;
						break;
					}
					IL_0072:
					if (_003C_003E7__wrap1.MoveNext())
					{
						StatDrawEntry current = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = null;
					if (unfinishedThing.BoundBill?.style?.Category != null && unfinishedThing.StyleSourcePrecept == null)
					{
						_003C_003E2__current = new StatDrawEntry(StatCategoryDefOf.BasicsNonPawn, "Stat_Thing_StyleLabel".Translate(), unfinishedThing.BoundBill.style.Category.LabelCap, "Stat_Thing_StyleDesc".Translate(), 1108);
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
		IEnumerator<StatDrawEntry> IEnumerable<StatDrawEntry>.GetEnumerator()
		{
			_003CSpecialDisplayStats_003Ed__32 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CSpecialDisplayStats_003Ed__32(0)
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

	private Pawn creatorInt;

	private string creatorName = "ErrorCreatorName";

	private RecipeDef recipeInt;

	public List<Thing> ingredients = new List<Thing>();

	private Bill_ProductionWithUft boundBillInt;

	public float workLeft = -10000f;

	public bool debugCompleted;

	private const float CancelIngredientRecoveryFraction = 0.75f;

	public Pawn Creator
	{
		get
		{
			return creatorInt;
		}
		set
		{
			if (value == null)
			{
				Log.Error("Cannot set creator to null.");
				return;
			}
			creatorInt = value;
			creatorName = value.LabelShort;
		}
	}

	public RecipeDef Recipe => recipeInt;

	public Bill_ProductionWithUft BoundBill
	{
		get
		{
			if (boundBillInt != null && (boundBillInt.DeletedOrDereferenced || boundBillInt.BoundUft != this))
			{
				boundBillInt = null;
			}
			return boundBillInt;
		}
		set
		{
			if (value == boundBillInt)
			{
				return;
			}
			Bill_ProductionWithUft bill_ProductionWithUft = boundBillInt;
			boundBillInt = value;
			if (bill_ProductionWithUft != null && bill_ProductionWithUft.BoundUft == this)
			{
				bill_ProductionWithUft.SetBoundUft(null, setOtherLink: false);
			}
			if (value != null)
			{
				recipeInt = value.recipe;
				if (value.BoundUft != this)
				{
					value.SetBoundUft(this, setOtherLink: false);
				}
			}
		}
	}

	public Thing BoundWorkTable
	{
		get
		{
			if (BoundBill == null)
			{
				return null;
			}
			Thing thing = BoundBill.billStack.billGiver as Thing;
			if (thing.Destroyed)
			{
				return null;
			}
			return thing;
		}
	}

	public override string LabelNoCount
	{
		get
		{
			if (Recipe == null)
			{
				return base.LabelNoCount;
			}
			if (base.Stuff == null)
			{
				return "UnfinishedItem".Translate(Recipe.products[0].thingDef.label);
			}
			return "UnfinishedItemWithStuff".Translate(base.Stuff.LabelAsStuff, Recipe.products[0].thingDef.label);
		}
	}

	public override string DescriptionDetailed
	{
		get
		{
			if (Recipe == null)
			{
				return base.LabelNoCount;
			}
			return Recipe.ProducedThingDef.DescriptionDetailed;
		}
	}

	public override string DescriptionFlavor
	{
		get
		{
			if (Recipe == null)
			{
				return base.LabelNoCount;
			}
			return Recipe.ProducedThingDef.description;
		}
	}

	public bool Initialized => workLeft > -5000f;

	public override void ExposeData()
	{
		base.ExposeData();
		if (Scribe.mode == LoadSaveMode.Saving && boundBillInt != null && boundBillInt.DeletedOrDereferenced)
		{
			boundBillInt = null;
		}
		Scribe_References.Look(ref creatorInt, "creator");
		Scribe_Values.Look(ref creatorName, "creatorName");
		Scribe_References.Look(ref boundBillInt, "bill");
		Scribe_Defs.Look(ref recipeInt, "recipe");
		Scribe_Values.Look(ref workLeft, "workLeft", 0f);
		Scribe_Collections.Look(ref ingredients, "ingredients", LookMode.Deep);
	}

	public override void Destroy(DestroyMode mode = DestroyMode.Vanish)
	{
		if (mode == DestroyMode.Cancel)
		{
			for (int i = 0; i < ingredients.Count; i++)
			{
				int num = GenMath.RoundRandom((float)ingredients[i].stackCount * 0.75f);
				if (num > 0)
				{
					ingredients[i].stackCount = num;
					GenPlace.TryPlaceThing(ingredients[i], base.Position, base.Map, ThingPlaceMode.Near, null, null, null);
				}
			}
			ingredients.Clear();
		}
		base.Destroy(mode);
		BoundBill = null;
	}

	[IteratorStateMachine(typeof(_003CGetGizmos_003Ed__28))]
	public override IEnumerable<Gizmo> GetGizmos()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetGizmos_003Ed__28(-2)
		{
			_003C_003E4__this = this
		};
	}

	public Bill_ProductionWithUft BillOnTableForMe(Thing workTable)
	{
		if (Recipe.AllRecipeUsers.Contains(workTable.def))
		{
			IBillGiver billGiver = (IBillGiver)workTable;
			for (int i = 0; i < billGiver.BillStack.Count; i++)
			{
				if (billGiver.BillStack[i] is Bill_ProductionWithUft bill_ProductionWithUft && bill_ProductionWithUft.ShouldDoNow() && bill_ProductionWithUft != null && bill_ProductionWithUft.recipe == Recipe)
				{
					return bill_ProductionWithUft;
				}
			}
		}
		return null;
	}

	public override void DrawExtraSelectionOverlays()
	{
		base.DrawExtraSelectionOverlays();
		if (BoundWorkTable != null)
		{
			GenDraw.DrawLineBetween(this.TrueCenter(), BoundWorkTable.TrueCenter());
		}
	}

	public override string GetInspectString()
	{
		string text = base.GetInspectString();
		if (!text.NullOrEmpty())
		{
			text += "\n";
		}
		text += "Author".Translate() + ": " + creatorName;
		text += "\n" + "WorkLeft".Translate() + ": " + workLeft.ToStringWorkAmount();
		if (BoundBill?.style?.Category != null && base.StyleSourcePrecept == null)
		{
			text += "\n" + "Style".Translate() + ": " + BoundBill.style.Category.LabelCap;
		}
		return text;
	}

	[IteratorStateMachine(typeof(_003CSpecialDisplayStats_003Ed__32))]
	public override IEnumerable<StatDrawEntry> SpecialDisplayStats()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CSpecialDisplayStats_003Ed__32(-2)
		{
			_003C_003E4__this = this
		};
	}
}
