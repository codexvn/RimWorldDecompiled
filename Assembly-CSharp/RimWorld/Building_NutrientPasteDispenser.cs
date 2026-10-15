using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.Sound;

namespace RimWorld;

public class Building_NutrientPasteDispenser : Building
{
	[CompilerGenerated]
	private sealed class _003CGetGizmos_003Ed__17 : IEnumerable<Gizmo>, IEnumerable, IEnumerator<Gizmo>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Gizmo _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public Building_NutrientPasteDispenser _003C_003E4__this;

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
		public _003CGetGizmos_003Ed__17(int _003C_003E1__state)
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
				Building_NutrientPasteDispenser building_NutrientPasteDispenser = _003C_003E4__this;
				Designator_Build designator_Build;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = ((Building)building_NutrientPasteDispenser).GetGizmos().GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_0071;
				case 1:
					_003C_003E1__state = -3;
					goto IL_0071;
				case 2:
					{
						_003C_003E1__state = -1;
						break;
					}
					IL_0071:
					if (_003C_003E7__wrap1.MoveNext())
					{
						Gizmo current = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = null;
					designator_Build = BuildCopyCommandUtility.FindAllowedDesignator(ThingDefOf.Hopper);
					if (designator_Build != null)
					{
						_003C_003E2__current = designator_Build;
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
		IEnumerator<Gizmo> IEnumerable<Gizmo>.GetEnumerator()
		{
			_003CGetGizmos_003Ed__17 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CGetGizmos_003Ed__17(0)
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

	public CompPowerTrader powerComp;

	private List<IntVec3> cachedAdjCellsCardinal;

	public static int CollectDuration = 50;

	public bool CanDispenseNow
	{
		get
		{
			if (powerComp.PowerOn)
			{
				return HasEnoughFeedstockInHoppers();
			}
			return false;
		}
	}

	public List<IntVec3> AdjCellsCardinalInBounds
	{
		get
		{
			if (cachedAdjCellsCardinal == null)
			{
				cachedAdjCellsCardinal = (from c in GenAdj.CellsAdjacentCardinal(this)
					where c.InBounds(base.Map)
					select c).ToList();
			}
			return cachedAdjCellsCardinal;
		}
	}

	public virtual ThingDef DispensableDef => ThingDefOf.MealNutrientPaste;

	public override Color DrawColor
	{
		get
		{
			if (!this.IsSociallyProper(null, forPrisoner: false))
			{
				return Building_Bed.SheetColorForPrisoner;
			}
			return base.DrawColor;
		}
	}

	public override void SpawnSetup(Map map, bool respawningAfterLoad)
	{
		base.SpawnSetup(map, respawningAfterLoad);
		powerComp = GetComp<CompPowerTrader>();
		cachedAdjCellsCardinal = null;
	}

	public virtual Building AdjacentReachableHopper(Pawn reacher)
	{
		for (int i = 0; i < AdjCellsCardinalInBounds.Count; i++)
		{
			Building edifice = AdjCellsCardinalInBounds[i].GetEdifice(base.Map);
			if (edifice != null && edifice.IsHopper() && reacher.CanReach(edifice, PathEndMode.Touch, Danger.Deadly) && !edifice.IsBurning())
			{
				return edifice;
			}
		}
		return null;
	}

	public virtual Thing TryDispenseFood()
	{
		if (!CanDispenseNow)
		{
			return null;
		}
		float num = def.building.nutritionCostPerDispense - 0.0001f;
		List<ThingDef> list = new List<ThingDef>();
		do
		{
			Thing thing = FindFeedInAnyHopper();
			if (thing == null)
			{
				Log.Error("Did not find enough food in hoppers while trying to dispense.");
				return null;
			}
			int num2 = Mathf.Min(thing.stackCount, Mathf.CeilToInt(num / thing.GetStatValue(StatDefOf.Nutrition)));
			num -= (float)num2 * thing.GetStatValue(StatDefOf.Nutrition);
			list.Add(thing.def);
			if (thing.stackCount == num2)
			{
				thing.Destroy();
			}
			else
			{
				thing.SplitOff(num2);
			}
		}
		while (!(num <= 0f));
		def.building.soundDispense.PlayOneShot(new TargetInfo(base.Position, base.Map));
		Thing thing2 = ThingMaker.MakeThing(ThingDefOf.MealNutrientPaste);
		CompIngredients compIngredients = thing2.TryGetComp<CompIngredients>();
		for (int i = 0; i < list.Count; i++)
		{
			compIngredients.RegisterIngredient(list[i]);
		}
		return thing2;
	}

	public virtual Thing FindFeedInAnyHopper()
	{
		for (int i = 0; i < AdjCellsCardinalInBounds.Count; i++)
		{
			Thing thing = null;
			Thing thing2 = null;
			List<Thing> thingList = AdjCellsCardinalInBounds[i].GetThingList(base.Map);
			for (int j = 0; j < thingList.Count; j++)
			{
				Thing thing3 = thingList[j];
				if (IsAcceptableFeedstock(thing3.def))
				{
					thing = thing3;
				}
				if (thing3.IsHopper())
				{
					thing2 = thing3;
				}
			}
			if (thing != null && thing2 != null)
			{
				return thing;
			}
		}
		return null;
	}

	public virtual bool HasEnoughFeedstockInHoppers()
	{
		float num = 0f;
		for (int i = 0; i < AdjCellsCardinalInBounds.Count; i++)
		{
			IntVec3 c = AdjCellsCardinalInBounds[i];
			Thing thing = null;
			Thing thing2 = null;
			List<Thing> thingList = c.GetThingList(base.Map);
			for (int j = 0; j < thingList.Count; j++)
			{
				Thing thing3 = thingList[j];
				if (IsAcceptableFeedstock(thing3.def))
				{
					thing = thing3;
				}
				if (thing3.IsHopper())
				{
					thing2 = thing3;
				}
			}
			if (thing != null && thing2 != null)
			{
				num += (float)thing.stackCount * thing.GetStatValue(StatDefOf.Nutrition);
			}
			if (num >= def.building.nutritionCostPerDispense)
			{
				return true;
			}
		}
		return false;
	}

	public static bool IsAcceptableFeedstock(ThingDef def)
	{
		if (def.IsNutritionGivingIngestible && def.ingestible.preferability != 0 && (def.ingestible.foodType & FoodTypeFlags.Plant) != FoodTypeFlags.Plant)
		{
			return (def.ingestible.foodType & FoodTypeFlags.Tree) != FoodTypeFlags.Tree;
		}
		return false;
	}

	[IteratorStateMachine(typeof(_003CGetGizmos_003Ed__17))]
	public override IEnumerable<Gizmo> GetGizmos()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetGizmos_003Ed__17(-2)
		{
			_003C_003E4__this = this
		};
	}

	public override string GetInspectString()
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.AppendLine(base.GetInspectString());
		if (!this.IsSociallyProper(null, forPrisoner: false))
		{
			stringBuilder.AppendLine("InPrisonCell".Translate());
		}
		return stringBuilder.ToString().Trim();
	}
}
