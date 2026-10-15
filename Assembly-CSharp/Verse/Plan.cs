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

public class Plan : IExposable, ISelectable, ILoadReferenceable, IRenameable, IHideable
{
	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass45_0
	{
		public ColorDef newCol;

		public Plan _003C_003E4__this;

		internal void _003CGetGizmos_003Eb__1()
		{
			_003C_003E4__this.Color = newCol;
			_003C_003E4__this.CheckContiguous();
		}
	}

	[CompilerGenerated]
	private sealed class _003CGetFloatMenuOptions_003Ed__50 : IEnumerable<FloatMenuOption>, IEnumerable, IEnumerator<FloatMenuOption>, IEnumerator, IDisposable
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
		public _003CGetFloatMenuOptions_003Ed__50(int _003C_003E1__state)
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
			return new _003CGetFloatMenuOptions_003Ed__50(0);
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<FloatMenuOption>)this).GetEnumerator();
		}
	}

	[CompilerGenerated]
	private sealed class _003CGetGizmos_003Ed__45 : IEnumerable<Gizmo>, IEnumerable, IEnumerator<Gizmo>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Gizmo _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public Plan _003C_003E4__this;

		private Designator_Plan_Expand _003Cexpand_003E5__2;

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
		public _003CGetGizmos_003Ed__45(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			_003Cexpand_003E5__2 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			int num = _003C_003E1__state;
			Plan CS_0024_003C_003E8__locals10 = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				_003Cexpand_003E5__2 = DesignatorUtility.FindAllowedDesignator<Designator_Plan_Expand>();
				_003Cexpand_003E5__2.Initialize(CS_0024_003C_003E8__locals10.color);
				_003C_003E2__current = new Command_Hide_Plans(CS_0024_003C_003E8__locals10)
				{
					icon = ContentFinder<Texture2D>.Get("UI/Designators/PlanHide"),
					defaultDesc = "CommandHidePlanDesc".Translate(),
					hotKey = KeyBindingDefOf.Misc2
				};
				_003C_003E1__state = 1;
				return true;
			case 1:
				_003C_003E1__state = -1;
				_003C_003E2__current = _003Cexpand_003E5__2;
				_003C_003E1__state = 2;
				return true;
			case 2:
				_003C_003E1__state = -1;
				_003C_003E2__current = DesignatorUtility.FindAllowedDesignator<Designator_Plan_Shrink>();
				_003C_003E1__state = 3;
				return true;
			case 3:
				_003C_003E1__state = -1;
				_003C_003E2__current = new Command_Action
				{
					icon = ContentFinder<Texture2D>.Get("UI/Designators/PlanChangeColor"),
					defaultLabel = "CommandChangeColorPlanLabel".Translate(),
					defaultDesc = "CommandChangeColorPlanDesc".Translate(),
					action = delegate
					{
						List<FloatMenuGridOption> list = new List<FloatMenuGridOption>();
						foreach (ColorDef color2 in Designator_Plan_Add.Colors)
						{
							_003C_003Ec__DisplayClass45_0 CS_0024_003C_003E8__locals8 = new _003C_003Ec__DisplayClass45_0
							{
								_003C_003E4__this = CS_0024_003C_003E8__locals10,
								newCol = color2
							};
							Color color = CS_0024_003C_003E8__locals8.newCol.color;
							color.a = 0.8f;
							list.Add(new FloatMenuGridOption(BaseContent.WhiteTex, delegate
							{
								CS_0024_003C_003E8__locals8._003C_003E4__this.Color = CS_0024_003C_003E8__locals8.newCol;
								CS_0024_003C_003E8__locals8._003C_003E4__this.CheckContiguous();
							}, color, CS_0024_003C_003E8__locals8.newCol.LabelCap));
						}
						Find.WindowStack.Add(new FloatMenuGrid(list));
					}
				};
				_003C_003E1__state = 4;
				return true;
			case 4:
				_003C_003E1__state = -1;
				_003C_003E2__current = new Command_Action
				{
					icon = TexButton.Delete,
					defaultLabel = "CommandDeletePlanLabel".Translate(),
					defaultDesc = "CommandDeletePlanDesc".Translate(),
					action = CS_0024_003C_003E8__locals10.Delete,
					hotKey = KeyBindingDefOf.Designator_Deconstruct
				};
				_003C_003E1__state = 5;
				return true;
			case 5:
				_003C_003E1__state = -1;
				_003C_003E2__current = DesignatorUtility.FindAllowedDesignator<Designator_Plan_Copy>();
				_003C_003E1__state = 6;
				return true;
			case 6:
				_003C_003E1__state = -1;
				_003C_003E2__current = DesignatorUtility.FindAllowedDesignator<Designator_Plan_CopySelection>();
				_003C_003E1__state = 7;
				return true;
			case 7:
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
		IEnumerator<Gizmo> IEnumerable<Gizmo>.GetEnumerator()
		{
			_003CGetGizmos_003Ed__45 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CGetGizmos_003Ed__45(0)
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

	public PlanManager planManager;

	private ColorDef color;

	private int ID = -1;

	public string label;

	private string baseLabel;

	private List<IntVec3> cells = new List<IntVec3>();

	private bool hidden;

	private Material materialInt;

	private bool cellsShuffled;

	private static BoolGrid extantGrid;

	private static BoolGrid foundGrid;

	private static readonly HashSet<IntVec3> toSplit = new HashSet<IntVec3>();

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

	public Map Map => planManager.map;

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
				Map.mapDrawer.MapMeshDirty(cell, MapMeshFlagDefOf.Plan);
			}
		}
	}

	public ColorDef Color
	{
		get
		{
			return color;
		}
		private set
		{
			if (color != value)
			{
				color = value;
				materialInt = CreateMaterial();
				Dirty();
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

	public void MergeIn(Plan plan)
	{
		for (int num = plan.Cells.Count - 1; num >= 0; num--)
		{
			IntVec3 c = plan.Cells[num];
			plan.RemoveCell(c);
			AddCell(c);
		}
	}

	protected virtual Material CreateMaterial()
	{
		MaterialRequest req = default(MaterialRequest);
		req.BaseTexPath = "Designations/Plan";
		req.shader = ShaderDatabase.MetaOverlay;
		req.color = color.color;
		return MaterialPool.MatFrom(req);
	}

	public IEnumerator<IntVec3> GetEnumerator()
	{
		return cells.GetEnumerator();
	}

	public Plan()
	{
	}

	public Plan(ColorDef color, PlanManager planManager)
	{
		baseLabel = "NewPlan".Translate();
		label = planManager.NewPlanName("NewPlan".Translate());
		this.planManager = planManager;
		ID = Find.UniqueIDsManager.GetNextPlanID();
		Map.planManager.RegisterPlan(this);
		this.color = color;
	}

	public virtual void ExposeData()
	{
		Scribe_Values.Look(ref ID, "ID", -1);
		Scribe_Values.Look(ref label, "label");
		Scribe_Values.Look(ref baseLabel, "baseLabel");
		Scribe_Values.Look(ref hidden, "hidden", defaultValue: false);
		Scribe_Defs.Look(ref color, "color");
		Scribe_Collections.Look(ref cells, "cells", LookMode.Undefined);
		BackCompatibility.PostExposeData(this);
	}

	public virtual void AddCell(IntVec3 c)
	{
		if (cells.Contains(c))
		{
			Log.Error($"Adding cell to zone which already has it. c={c}, zone={this}");
		}
		else
		{
			cells.Add(c);
			planManager.AddPlanGridCell(this, c);
			Map.mapDrawer.MapMeshDirty(c, MapMeshFlagDefOf.Plan);
			cellsShuffled = false;
		}
	}

	public virtual void RemoveCell(IntVec3 c)
	{
		if (!cells.Contains(c))
		{
			Log.Error($"Cannot remove cell from zone which doesn't have it. c={c}, zone={this}");
			return;
		}
		cells.Remove(c);
		planManager.ClearPlanGridCell(c);
		Map.mapDrawer.MapMeshDirty(c, MapMeshFlagDefOf.Plan);
		cellsShuffled = false;
		if (cells.Count == 0)
		{
			Deregister();
		}
	}

	public void Dirty()
	{
		foreach (IntVec3 cell in cells)
		{
			Map.mapDrawer.MapMeshDirty(cell, MapMeshFlagDefOf.Plan);
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
				RemoveCell(cells.First());
			}
		}
		Find.Selector.Deselect(this);
	}

	public void Deregister()
	{
		planManager.DeregisterPlan(this);
	}

	public bool ContainsCell(IntVec3 c)
	{
		return cells.Contains(c);
	}

	public virtual string GetInspectString()
	{
		int num = 0;
		for (int i = 0; i < planManager.AllPlans.Count; i++)
		{
			Plan plan = planManager.AllPlans[i];
			num += plan.CellCount;
		}
		string text = $"{"Size".Translate().CapitalizeFirst()}: {CellCount}";
		if (planManager.AllPlans.Count > 1)
		{
			text += $"\n{"TotalPlanningArea".Translate().CapitalizeFirst()}: {num}";
		}
		return text;
	}

	public virtual IEnumerable<InspectTabBase> GetInspectTabs()
	{
		return null;
	}

	[IteratorStateMachine(typeof(_003CGetGizmos_003Ed__45))]
	public virtual IEnumerable<Gizmo> GetGizmos()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetGizmos_003Ed__45(-2)
		{
			_003C_003E4__this = this
		};
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
		foreach (IntVec3 allCell in Map.AllCells)
		{
			Plan plan = allCell.GetPlan(Map);
			if (plan != null && plan == this)
			{
				extantGrid.Set(allCell, value: true);
			}
		}
		int numFound = 0;
		Map.floodFiller.FloodFill(cells.First(), (Predicate<IntVec3>)PassCheck, (Action<IntVec3>)Processor, int.MaxValue, rememberParents: false, (IEnumerable<IntVec3>)null);
		if (numFound >= cells.Count)
		{
			return;
		}
		foreach (IntVec3 allCell2 in Map.AllCells)
		{
			if (!extantGrid[allCell2] || foundGrid[allCell2])
			{
				continue;
			}
			Map.floodFiller.FloodFill(allCell2, (IntVec3 cell) => extantGrid[cell] && !foundGrid[cell], delegate(IntVec3 cell)
			{
				if (Map.planManager.PlanAt(cell) == this)
				{
					toSplit.Add(cell);
				}
				extantGrid[cell] = false;
			});
			Plan plan2 = null;
			foreach (IntVec3 item in toSplit)
			{
				if (plan2 == null)
				{
					plan2 = new Plan(color, planManager);
				}
				plan2.RenamableLabel = RenamableLabel;
				RemoveCell(item);
				plan2.AddCell(item);
			}
			toSplit.Clear();
		}
		static bool PassCheck(IntVec3 c)
		{
			if (!extantGrid[c])
			{
				return false;
			}
			if (foundGrid[c])
			{
				return false;
			}
			return true;
		}
		void Processor(IntVec3 c)
		{
			foundGrid.Set(c, value: true);
			numFound++;
		}
	}

	[IteratorStateMachine(typeof(_003CGetFloatMenuOptions_003Ed__50))]
	public virtual IEnumerable<FloatMenuOption> GetFloatMenuOptions(Pawn selPawn)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetFloatMenuOptions_003Ed__50(-2);
	}

	public override string ToString()
	{
		return label;
	}

	public string GetUniqueLoadID()
	{
		return $"Plan_{ID}";
	}
}
