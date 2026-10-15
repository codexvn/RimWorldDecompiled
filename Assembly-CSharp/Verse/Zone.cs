using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using RimWorld;
using UnityEngine;
using Verse.Sound;

namespace Verse;

public abstract class Zone : IExposable, ISelectable, ILoadReferenceable, IRenameable, IHideable
{
	[CompilerGenerated]
	private sealed class _003CGetEnumerator_003Ed__29 : IEnumerator<IntVec3>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private IntVec3 _003C_003E2__current;

		public Zone _003C_003E4__this;

		private int _003Ci_003E5__2;

		IntVec3 IEnumerator<IntVec3>.Current
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
		public _003CGetEnumerator_003Ed__29(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			int num = _003C_003E1__state;
			Zone zone = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				_003Ci_003E5__2 = 0;
				break;
			case 1:
				_003C_003E1__state = -1;
				_003Ci_003E5__2++;
				break;
			}
			if (_003Ci_003E5__2 < zone.cells.Count)
			{
				_003C_003E2__current = zone.cells[_003Ci_003E5__2];
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
	}

	[CompilerGenerated]
	private sealed class _003CGetFloatMenuOptions_003Ed__62 : IEnumerable<FloatMenuOption>, IEnumerable, IEnumerator<FloatMenuOption>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private FloatMenuOption _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		FloatMenuOption IEnumerator<FloatMenuOption>.Current
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
		public _003CGetFloatMenuOptions_003Ed__62(int _003C_003E1__state)
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
			if (_003C_003E1__state != 0)
			{
				return false;
			}
			_003C_003E1__state = -1;
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
		IEnumerator<FloatMenuOption> IEnumerable<FloatMenuOption>.GetEnumerator()
		{
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				return this;
			}
			return new _003CGetFloatMenuOptions_003Ed__62(0);
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<FloatMenuOption>)this).GetEnumerator();
		}
	}

	[CompilerGenerated]
	private sealed class _003CGetGizmos_003Ed__57 : IEnumerable<Gizmo>, IEnumerable, IEnumerator<Gizmo>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Gizmo _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public Zone _003C_003E4__this;

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
		public _003CGetGizmos_003Ed__57(int _003C_003E1__state)
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
				Zone zone = _003C_003E4__this;
				Designator_ZoneDelete_Shrink designator_ZoneDelete_Shrink;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = zone.GetZoneAddGizmos().GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_0078;
				case 1:
					_003C_003E1__state = -3;
					goto IL_0078;
				case 2:
					_003C_003E1__state = -1;
					goto IL_00b7;
				case 3:
					{
						_003C_003E1__state = -1;
						return false;
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
					designator_ZoneDelete_Shrink = DesignatorUtility.FindAllowedDesignator<Designator_ZoneDelete_Shrink>();
					if (designator_ZoneDelete_Shrink != null)
					{
						_003C_003E2__current = designator_ZoneDelete_Shrink;
						_003C_003E1__state = 2;
						return true;
					}
					goto IL_00b7;
					IL_00b7:
					_003C_003E2__current = new Command_Action
					{
						icon = TexButton.Delete,
						defaultLabel = "CommandDeleteZoneLabel".Translate(),
						defaultDesc = "CommandDeleteZoneDesc".Translate(),
						action = zone.Delete,
						hotKey = KeyBindingDefOf.Designator_Deconstruct
					};
					_003C_003E1__state = 3;
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
			_003CGetGizmos_003Ed__57 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CGetGizmos_003Ed__57(0)
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
	private sealed class _003Cget_AllContainedThings_003Ed__37 : IEnumerable<Thing>, IEnumerable, IEnumerator<Thing>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Thing _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public Zone _003C_003E4__this;

		private ThingGrid _003Cgrids_003E5__2;

		private int _003Ci_003E5__3;

		private List<Thing> _003CthingList_003E5__4;

		private int _003Cj_003E5__5;

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
		public _003Cget_AllContainedThings_003Ed__37(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			_003Cgrids_003E5__2 = null;
			_003CthingList_003E5__4 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			int num = _003C_003E1__state;
			Zone zone = _003C_003E4__this;
			if (num != 0)
			{
				if (num != 1)
				{
					return false;
				}
				_003C_003E1__state = -1;
				_003Cj_003E5__5++;
				goto IL_009d;
			}
			_003C_003E1__state = -1;
			_003Cgrids_003E5__2 = zone.Map.thingGrid;
			_003Ci_003E5__3 = 0;
			goto IL_00c7;
			IL_009d:
			if (_003Cj_003E5__5 < _003CthingList_003E5__4.Count)
			{
				_003C_003E2__current = _003CthingList_003E5__4[_003Cj_003E5__5];
				_003C_003E1__state = 1;
				return true;
			}
			_003CthingList_003E5__4 = null;
			_003Ci_003E5__3++;
			goto IL_00c7;
			IL_00c7:
			if (_003Ci_003E5__3 < zone.cells.Count)
			{
				_003CthingList_003E5__4 = _003Cgrids_003E5__2.ThingsListAt(zone.cells[_003Ci_003E5__3]);
				_003Cj_003E5__5 = 0;
				goto IL_009d;
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
		IEnumerator<Thing> IEnumerable<Thing>.GetEnumerator()
		{
			_003Cget_AllContainedThings_003Ed__37 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003Cget_AllContainedThings_003Ed__37(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<Thing>)this).GetEnumerator();
		}
	}

	public ZoneManager zoneManager;

	public int ID = -1;

	public string label;

	private string baseLabel;

	public List<IntVec3> cells = new List<IntVec3>();

	private bool cellsShuffled;

	public Color color = Color.white;

	private Material materialInt;

	private bool hidden;

	private int lastStaticFireCheckTick = -9999;

	private bool lastStaticFireCheckResult;

	private const int StaticFireCheckInterval = 1000;

	private static BoolGrid extantGrid;

	private static BoolGrid foundGrid;

	public string RenamableLabel
	{
		get
		{
			return label ?? baseLabel;
		}
		set
		{
			label = value;
		}
	}

	public string BaseLabel => baseLabel;

	public string InspectLabel => RenamableLabel;

	public Map Map => zoneManager.map;

	public IntVec3 Position
	{
		get
		{
			if (cells.Count == 0)
			{
				return IntVec3.Invalid;
			}
			return cells[0];
		}
	}

	public bool Hidden
	{
		get
		{
			return hidden;
		}
		set
		{
			hidden = value;
			foreach (IntVec3 cell in Cells)
			{
				Map.mapDrawer.MapMeshDirty(cell, MapMeshFlagDefOf.Zone);
			}
		}
	}

	public Material Material
	{
		get
		{
			if ((object)materialInt == null)
			{
				materialInt = CreateMaterial();
			}
			return materialInt;
		}
	}

	public List<IntVec3> Cells
	{
		get
		{
			if (!cellsShuffled)
			{
				cells.Shuffle();
				cellsShuffled = true;
			}
			return cells;
		}
	}

	public int CellCount => cells.Count;

	public int HeldThingsCount
	{
		get
		{
			int num = 0;
			ThingGrid thingGrid = Map.thingGrid;
			for (int i = 0; i < cells.Count; i++)
			{
				List<Thing> list = thingGrid.ThingsListAt(cells[i]);
				num += list.Count;
			}
			return num;
		}
	}

	public IEnumerable<Thing> AllContainedThings
	{
		[IteratorStateMachine(typeof(_003Cget_AllContainedThings_003Ed__37))]
		get
		{
			//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
			return new _003Cget_AllContainedThings_003Ed__37(-2)
			{
				_003C_003E4__this = this
			};
		}
	}

	public bool ContainsStaticFire
	{
		get
		{
			if (Find.TickManager.TicksGame > lastStaticFireCheckTick + 1000)
			{
				lastStaticFireCheckResult = false;
				for (int i = 0; i < cells.Count; i++)
				{
					if (cells[i].ContainsStaticFire(Map))
					{
						lastStaticFireCheckResult = true;
						break;
					}
				}
			}
			return lastStaticFireCheckResult;
		}
	}

	public virtual bool IsMultiselectable => false;

	protected abstract Color NextZoneColor { get; }

	protected virtual Material CreateMaterial()
	{
		Material material = SolidColorMaterials.SimpleSolidColorMaterial(color);
		material.renderQueue = 3600;
		return material;
	}

	[IteratorStateMachine(typeof(_003CGetEnumerator_003Ed__29))]
	public IEnumerator<IntVec3> GetEnumerator()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetEnumerator_003Ed__29(0)
		{
			_003C_003E4__this = this
		};
	}

	public Zone()
	{
	}

	public Zone(string baseName, ZoneManager zoneManager)
	{
		baseLabel = baseName;
		label = zoneManager.NewZoneName(baseName);
		this.zoneManager = zoneManager;
		ID = Find.UniqueIDsManager.GetNextZoneID();
		color = NextZoneColor;
	}

	public virtual void ExposeData()
	{
		Scribe_Values.Look(ref ID, "ID", -1);
		Scribe_Values.Look(ref label, "label");
		Scribe_Values.Look(ref baseLabel, "baseLabel");
		Scribe_Values.Look(ref color, "color");
		Scribe_Values.Look(ref hidden, "hidden", defaultValue: false);
		Scribe_Collections.Look(ref cells, "cells", LookMode.Undefined);
		BackCompatibility.PostExposeData(this);
		if (Scribe.mode == LoadSaveMode.PostLoadInit)
		{
			CheckAddHaulDestination();
		}
	}

	public virtual void AddCell(IntVec3 c)
	{
		if (cells.Contains(c))
		{
			IntVec3 intVec = c;
			Log.Error("Adding cell to zone which already has it. c=" + intVec.ToString() + ", zone=" + this);
			return;
		}
		List<Thing> list = Map.thingGrid.ThingsListAt(c);
		for (int i = 0; i < list.Count; i++)
		{
			Thing thing = list[i];
			if (!thing.def.CanOverlapZones)
			{
				Log.Error("Added zone over zone-incompatible thing " + thing);
				return;
			}
		}
		cells.Add(c);
		zoneManager.AddZoneGridCell(this, c);
		Map.mapDrawer.MapMeshDirty(c, MapMeshFlagDefOf.Zone);
		AutoHomeAreaMaker.Notify_ZoneCellAdded(c, this);
		cellsShuffled = false;
	}

	public virtual void RemoveCell(IntVec3 c)
	{
		if (!cells.Contains(c))
		{
			IntVec3 intVec = c;
			Log.Error("Cannot remove cell from zone which doesn't have it. c=" + intVec.ToString() + ", zone=" + this);
			return;
		}
		cells.Remove(c);
		zoneManager.ClearZoneGridCell(c);
		Map.mapDrawer.MapMeshDirty(c, MapMeshFlagDefOf.Zone);
		cellsShuffled = false;
		if (cells.Count == 0)
		{
			Deregister();
		}
	}

	public void Delete()
	{
		Delete(playSound: true);
	}

	public virtual void Delete(bool playSound)
	{
		if (playSound)
		{
			SoundDefOf.Designate_ZoneDelete.PlayOneShotOnCamera(Map);
		}
		if (cells.Count == 0)
		{
			Deregister();
		}
		else
		{
			while (cells.Count > 0)
			{
				RemoveCell(cells[cells.Count - 1]);
			}
		}
		Find.Selector.Deselect(this);
	}

	public void Deregister()
	{
		zoneManager.DeregisterZone(this);
	}

	public virtual void PostRegister()
	{
		CheckAddHaulDestination();
	}

	public virtual void PostDeregister()
	{
		if (this is IHaulDestination haulDestination)
		{
			Map.haulDestinationManager.RemoveHaulDestination(haulDestination);
		}
	}

	public bool ContainsCell(IntVec3 c)
	{
		for (int i = 0; i < cells.Count; i++)
		{
			if (cells[i] == c)
			{
				return true;
			}
		}
		return false;
	}

	public virtual string GetInspectString()
	{
		return $"{"Size".Translate().CapitalizeFirst()}: {CellCount}";
	}

	public virtual IEnumerable<InspectTabBase> GetInspectTabs()
	{
		return null;
	}

	[IteratorStateMachine(typeof(_003CGetGizmos_003Ed__57))]
	public virtual IEnumerable<Gizmo> GetGizmos()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetGizmos_003Ed__57(-2)
		{
			_003C_003E4__this = this
		};
	}

	public virtual IEnumerable<Gizmo> GetZoneAddGizmos()
	{
		return Enumerable.Empty<Gizmo>();
	}

	public void CheckContiguous()
	{
		if (cells.Count == 0)
		{
			return;
		}
		if (extantGrid == null)
		{
			extantGrid = new BoolGrid(Map);
		}
		else
		{
			extantGrid.ClearAndResizeTo(Map);
		}
		if (foundGrid == null)
		{
			foundGrid = new BoolGrid(Map);
		}
		else
		{
			foundGrid.ClearAndResizeTo(Map);
		}
		for (int i = 0; i < cells.Count; i++)
		{
			extantGrid.Set(cells[i], value: true);
		}
		Predicate<IntVec3> passCheck = delegate(IntVec3 c)
		{
			if (!extantGrid[c])
			{
				return false;
			}
			return !foundGrid[c];
		};
		int numFound = 0;
		Action<IntVec3> processor = delegate(IntVec3 c)
		{
			foundGrid.Set(c, value: true);
			numFound++;
		};
		Map.floodFiller.FloodFill(cells[0], passCheck, processor);
		if (numFound >= cells.Count)
		{
			return;
		}
		foreach (IntVec3 allCell in Map.AllCells)
		{
			if (extantGrid[allCell] && !foundGrid[allCell])
			{
				RemoveCell(allCell);
			}
		}
	}

	[IteratorStateMachine(typeof(_003CGetFloatMenuOptions_003Ed__62))]
	public virtual IEnumerable<FloatMenuOption> GetFloatMenuOptions(Pawn selPawn)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetFloatMenuOptions_003Ed__62(-2);
	}

	private void CheckAddHaulDestination()
	{
		if (this is IHaulDestination haulDestination)
		{
			Map.haulDestinationManager.AddHaulDestination(haulDestination);
		}
	}

	public override string ToString()
	{
		return label;
	}

	public string GetUniqueLoadID()
	{
		return "Zone_" + ID;
	}
}
