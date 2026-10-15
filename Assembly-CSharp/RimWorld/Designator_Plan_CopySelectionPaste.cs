using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;

namespace RimWorld;

public class Designator_Plan_CopySelectionPaste : Designator_Plan_Add
{
	[CompilerGenerated]
	private sealed class _003CGetCurrentCells_003Ed__19 : IEnumerable<IntVec3>, IEnumerable, IEnumerator<IntVec3>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private IntVec3 _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private IntVec3 c;

		public IntVec3 _003C_003E3__c;

		public Designator_Plan_CopySelectionPaste _003C_003E4__this;

		private ColorDef color;

		public ColorDef _003C_003E3__color;

		private IntVec3 _003Croot_003E5__2;

		private int _003Ci_003E5__3;

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
		public _003CGetCurrentCells_003Ed__19(int _003C_003E1__state)
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
			Designator_Plan_CopySelectionPaste designator_Plan_CopySelectionPaste = _003C_003E4__this;
			if (num != 0)
			{
				if (num != 1)
				{
					return false;
				}
				_003C_003E1__state = -1;
				goto IL_00b2;
			}
			_003C_003E1__state = -1;
			_003Croot_003E5__2 = PrefabUtility.GetRoot(c, new IntVec2(designator_Plan_CopySelectionPaste.indices.SizeX, designator_Plan_CopySelectionPaste.indices.SizeZ), designator_Plan_CopySelectionPaste.rotation);
			_003Ci_003E5__3 = 0;
			goto IL_00c2;
			IL_00b2:
			_003Ci_003E5__3++;
			goto IL_00c2;
			IL_00c2:
			if (_003Ci_003E5__3 < designator_Plan_CopySelectionPaste.indices.NumGridCells)
			{
				if (designator_Plan_CopySelectionPaste.grid[_003Ci_003E5__3] == color)
				{
					_003C_003E2__current = _003Croot_003E5__2 + PrefabUtility.GetAdjustedLocalPosition(designator_Plan_CopySelectionPaste.indices[_003Ci_003E5__3], designator_Plan_CopySelectionPaste.rotation);
					_003C_003E1__state = 1;
					return true;
				}
				goto IL_00b2;
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
		IEnumerator<IntVec3> IEnumerable<IntVec3>.GetEnumerator()
		{
			_003CGetCurrentCells_003Ed__19 _003CGetCurrentCells_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CGetCurrentCells_003Ed__ = this;
			}
			else
			{
				_003CGetCurrentCells_003Ed__ = new _003CGetCurrentCells_003Ed__19(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			_003CGetCurrentCells_003Ed__.c = _003C_003E3__c;
			_003CGetCurrentCells_003Ed__.color = _003C_003E3__color;
			return _003CGetCurrentCells_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<IntVec3>)this).GetEnumerator();
		}
	}

	private readonly List<ColorDef> grid = new List<ColorDef>();

	private readonly HashSet<ColorDef> colors = new HashSet<ColorDef>();

	private CellIndices indices;

	private Rot4 rotation;

	private readonly List<IntVec3> cells = new List<IntVec3>();

	private static readonly List<IntVec3> tmpSelected = new List<IntVec3>();

	private static float middleMouseDownTime;

	public override bool DragDrawMeasurements => true;

	public override DrawStyleCategoryDef DrawStyleCategory => null;

	public override bool AlwaysDoGuiControls => true;

	public float PaneTopY => UI.screenHeight - 35;

	public override bool CanRightClickToggleVisibility => false;

	public Designator_Plan_CopySelectionPaste()
	{
		useMouseIcon = true;
		hideMouseIcon = true;
		mouseText = "CommandCopyPlanSelectionPasteMouse".Translate();
	}

	public override void SelectedUpdate()
	{
		base.SelectedUpdate();
		foreach (ColorDef color in colors)
		{
			foreach (IntVec3 currentCell in GetCurrentCells(UI.MouseCell(), color))
			{
				if (currentCell.InBounds(base.Map) && !currentCell.InNoBuildEdgeArea(base.Map))
				{
					cells.Add(currentCell);
				}
			}
			GenDraw.DrawFieldEdges(cells, color.color, null, null, 3600);
			cells.Clear();
		}
		GenDraw.DrawNoZoneEdgeLines();
	}

	public override AcceptanceReport CanDesignateCell(IntVec3 loc)
	{
		foreach (ColorDef color in colors)
		{
			foreach (IntVec3 currentCell in GetCurrentCells(loc, color))
			{
				if (!currentCell.InBounds(base.Map))
				{
					return false;
				}
				if (currentCell.InNoBuildEdgeArea(base.Map))
				{
					return "TooCloseToMapEdge".Translate();
				}
			}
		}
		return true;
	}

	public override void DesignateSingleCell(IntVec3 c)
	{
		foreach (ColorDef color in colors)
		{
			base.SelectedPlan = null;
			colorDef = color;
			PlanCells(GetCurrentCells(c, color));
		}
		base.SelectedPlan = null;
	}

	[IteratorStateMachine(typeof(_003CGetCurrentCells_003Ed__19))]
	protected IEnumerable<IntVec3> GetCurrentCells(IntVec3 c, ColorDef color)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetCurrentCells_003Ed__19(-2)
		{
			_003C_003E4__this = this,
			_003C_003E3__c = c,
			_003C_003E3__color = color
		};
	}

	public void Initialize(IEnumerable<IntVec3> selected)
	{
		rotation = Rot4.North;
		tmpSelected.Clear();
		tmpSelected.AddRange(selected);
		CellRect cellRect = CellRect.FromCellList(tmpSelected);
		indices = new CellIndices(cellRect.Size.x, cellRect.Size.z);
		grid.Clear();
		colors.Clear();
		grid.Capacity = Mathf.Max(grid.Capacity, cellRect.Area);
		for (int i = 0; i < indices.NumGridCells; i++)
		{
			IntVec3 c = cellRect.Min + indices[i];
			ColorDef colorDef = base.Map.planManager.PlanAt(c)?.Color;
			grid.Add(colorDef);
			if (colorDef != null)
			{
				colors.Add(colorDef);
			}
		}
	}

	public override void DoExtraGuiControls(float leftX, float bottomY)
	{
		DesignatorUtility.GUIDoRotationControls(leftX, bottomY, rotation, delegate(Rot4 rot)
		{
			rotation = rot;
		});
	}

	public override void SelectedProcessInput(Event ev)
	{
		RotationDirection rotationDirection = RotationDirection.None;
		if (Event.current.button == 2)
		{
			if (Event.current.type == EventType.mouseDown)
			{
				Event.current.Use();
				middleMouseDownTime = Time.realtimeSinceStartup;
			}
			if (Event.current.type == EventType.mouseUp && Time.realtimeSinceStartup - middleMouseDownTime < 0.15f)
			{
				rotationDirection = RotationDirection.Clockwise;
			}
		}
		if (KeyBindingDefOf.Designator_RotateRight.KeyDownEvent)
		{
			rotationDirection = RotationDirection.Clockwise;
		}
		else if (KeyBindingDefOf.Designator_RotateLeft.KeyDownEvent)
		{
			rotationDirection = RotationDirection.Counterclockwise;
		}
		if (rotationDirection != 0)
		{
			rotation.Rotate(rotationDirection);
		}
		DesignatorUtility.GUIDoRotationControls(0f, PaneTopY, rotation, delegate(Rot4 rot)
		{
			rotation = rot;
		});
	}
}
