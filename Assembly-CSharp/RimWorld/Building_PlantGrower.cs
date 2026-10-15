using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld;

public class Building_PlantGrower : Building, IPlantToGrowSettable
{
	[CompilerGenerated]
	private sealed class _003CGetGizmos_003Ed__6 : IEnumerable<Gizmo>, IEnumerable, IEnumerator<Gizmo>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Gizmo _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public Building_PlantGrower _003C_003E4__this;

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
		public _003CGetGizmos_003Ed__6(int _003C_003E1__state)
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
				Building_PlantGrower building_PlantGrower = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = ((Building)building_PlantGrower).GetGizmos().GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_006f;
				case 1:
					_003C_003E1__state = -3;
					goto IL_006f;
				case 2:
					{
						_003C_003E1__state = -1;
						return false;
					}
					IL_006f:
					if (_003C_003E7__wrap1.MoveNext())
					{
						Gizmo current = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = null;
					_003C_003E2__current = PlantToGrowSettableUtility.SetPlantToGrowCommand(building_PlantGrower);
					_003C_003E1__state = 2;
					return true;
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
		IEnumerator<Gizmo> IEnumerable<Gizmo>.GetEnumerator()
		{
			_003CGetGizmos_003Ed__6 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CGetGizmos_003Ed__6(0)
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
	private sealed class _003Cget_PlantsOnMe_003Ed__3 : IEnumerable<Plant>, IEnumerable, IEnumerator<Plant>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Plant _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public Building_PlantGrower _003C_003E4__this;

		private CellRect.Enumerator _003C_003E7__wrap1;

		private List<Thing> _003CthingList_003E5__3;

		private int _003Ci_003E5__4;

		Plant IEnumerator<Plant>.Current
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
		public _003Cget_PlantsOnMe_003Ed__3(int _003C_003E1__state)
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
			_003CthingList_003E5__3 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				Building_PlantGrower building_PlantGrower = _003C_003E4__this;
				if (num != 0)
				{
					if (num != 1)
					{
						return false;
					}
					_003C_003E1__state = -3;
					goto IL_00bb;
				}
				_003C_003E1__state = -1;
				if (!building_PlantGrower.Spawned)
				{
					return false;
				}
				_003C_003E7__wrap1 = building_PlantGrower.OccupiedRect().GetEnumerator();
				_003C_003E1__state = -3;
				goto IL_00e7;
				IL_00cd:
				if (_003Ci_003E5__4 < _003CthingList_003E5__3.Count)
				{
					if (_003CthingList_003E5__3[_003Ci_003E5__4] is Plant plant)
					{
						_003C_003E2__current = plant;
						_003C_003E1__state = 1;
						return true;
					}
					goto IL_00bb;
				}
				_003CthingList_003E5__3 = null;
				goto IL_00e7;
				IL_00bb:
				_003Ci_003E5__4++;
				goto IL_00cd;
				IL_00e7:
				if (_003C_003E7__wrap1.MoveNext())
				{
					IntVec3 current = _003C_003E7__wrap1.Current;
					_003CthingList_003E5__3 = building_PlantGrower.Map.thingGrid.ThingsListAt(current);
					_003Ci_003E5__4 = 0;
					goto IL_00cd;
				}
				_003C_003Em__Finally1();
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
		IEnumerator<Plant> IEnumerable<Plant>.GetEnumerator()
		{
			_003Cget_PlantsOnMe_003Ed__3 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003Cget_PlantsOnMe_003Ed__3(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<Plant>)this).GetEnumerator();
		}
	}

	private ThingDef plantDefToGrow;

	private CompPowerTrader compPower;

	public IEnumerable<Plant> PlantsOnMe
	{
		[IteratorStateMachine(typeof(_003Cget_PlantsOnMe_003Ed__3))]
		get
		{
			//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
			return new _003Cget_PlantsOnMe_003Ed__3(-2)
			{
				_003C_003E4__this = this
			};
		}
	}

	IEnumerable<IntVec3> IPlantToGrowSettable.Cells => this.OccupiedRect().Cells;

	[IteratorStateMachine(typeof(_003CGetGizmos_003Ed__6))]
	public override IEnumerable<Gizmo> GetGizmos()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetGizmos_003Ed__6(-2)
		{
			_003C_003E4__this = this
		};
	}

	public override void PostMake()
	{
		base.PostMake();
		plantDefToGrow = def.building.defaultPlantToGrow;
	}

	public override void SpawnSetup(Map map, bool respawningAfterLoad)
	{
		base.SpawnSetup(map, respawningAfterLoad);
		compPower = GetComp<CompPowerTrader>();
		PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.GrowingFood, KnowledgeAmount.Total);
	}

	public override void ExposeData()
	{
		base.ExposeData();
		Scribe_Defs.Look(ref plantDefToGrow, "plantDefToGrow");
	}

	public override void TickRare()
	{
		base.TickRare();
		if (compPower == null || compPower.PowerOn)
		{
			return;
		}
		foreach (Plant item in PlantsOnMe)
		{
			DamageInfo dinfo = new DamageInfo(DamageDefOf.Rotting, 1f);
			item.TakeDamage(dinfo);
		}
	}

	public override void DeSpawn(DestroyMode mode = DestroyMode.Vanish)
	{
		if (mode != DestroyMode.WillReplace)
		{
			foreach (Plant item in PlantsOnMe.ToList())
			{
				item.Destroy();
			}
		}
		base.DeSpawn(mode);
	}

	public override string GetInspectString()
	{
		string text = base.GetInspectString();
		if (base.Spawned)
		{
			if (!text.NullOrEmpty())
			{
				text += "\n";
			}
			text = ((!PlantUtility.GrowthSeasonNow(base.Position, base.Map, plantDefToGrow)) ? ((string)(text + "CannotGrowBadSeasonTemperature".Translate())) : ((string)(text + "GrowSeasonHereNow".Translate())));
		}
		return text;
	}

	public ThingDef GetPlantDefToGrow()
	{
		return plantDefToGrow;
	}

	public void SetPlantDefToGrow(ThingDef plantDef)
	{
		plantDefToGrow = plantDef;
	}

	public bool CanAcceptSowNow()
	{
		if (compPower != null && !compPower.PowerOn)
		{
			return false;
		}
		return true;
	}
}
