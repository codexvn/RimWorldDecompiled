using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;

namespace RimWorld;

public class Designator_Plan_Copy : Designator_Plan_Add
{
	[CompilerGenerated]
	private sealed class _003CGetCurrentCells_003Ed__21 : IEnumerable<IntVec3>, IEnumerable, IEnumerator<IntVec3>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private IntVec3 _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private IntVec3 c;

		public IntVec3 _003C_003E3__c;

		public Designator_Plan_Copy _003C_003E4__this;

		private IntVec3 _003Croot_003E5__2;

		private int _003Cx_003E5__3;

		private int _003Cz_003E5__4;

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
		public _003CGetCurrentCells_003Ed__21(int _003C_003E1__state)
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
			Designator_Plan_Copy designator_Plan_Copy = _003C_003E4__this;
			if (num != 0)
			{
				if (num != 1)
				{
					return false;
				}
				_003C_003E1__state = -1;
				goto IL_00c1;
			}
			_003C_003E1__state = -1;
			_003Croot_003E5__2 = PrefabUtility.GetRoot(c, new IntVec2(designator_Plan_Copy.grid.Width, designator_Plan_Copy.grid.Height), designator_Plan_Copy.rotation);
			_003Cx_003E5__3 = 0;
			goto IL_00f4;
			IL_00d1:
			if (_003Cz_003E5__4 < designator_Plan_Copy.grid.Height)
			{
				if (designator_Plan_Copy.grid[_003Cx_003E5__3, _003Cz_003E5__4])
				{
					IntVec3 adjustedLocalPosition = PrefabUtility.GetAdjustedLocalPosition(new IntVec3(_003Cx_003E5__3, 0, _003Cz_003E5__4), designator_Plan_Copy.rotation);
					_003C_003E2__current = _003Croot_003E5__2 + adjustedLocalPosition;
					_003C_003E1__state = 1;
					return true;
				}
				goto IL_00c1;
			}
			_003Cx_003E5__3++;
			goto IL_00f4;
			IL_00c1:
			_003Cz_003E5__4++;
			goto IL_00d1;
			IL_00f4:
			if (_003Cx_003E5__3 < designator_Plan_Copy.grid.Width)
			{
				_003Cz_003E5__4 = 0;
				goto IL_00d1;
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
			_003CGetCurrentCells_003Ed__21 _003CGetCurrentCells_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CGetCurrentCells_003Ed__ = this;
			}
			else
			{
				_003CGetCurrentCells_003Ed__ = new _003CGetCurrentCells_003Ed__21(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			_003CGetCurrentCells_003Ed__.c = _003C_003E3__c;
			return _003CGetCurrentCells_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<IntVec3>)this).GetEnumerator();
		}
	}

	private BoolGrid grid;

	private Rot4 rotation;

	private readonly List<IntVec3> cells = new List<IntVec3>();

	private static float middleMouseDownTime;

	public override bool DragDrawMeasurements => false;

	protected override bool CanSelectColor => false;

	public override DrawStyleCategoryDef DrawStyleCategory => null;

	public override bool AlwaysDoGuiControls => true;

	public override bool CanRightClickToggleVisibility => false;

	public float PaneTopY => UI.screenHeight - 35;

	public override Color IconDrawColor => Color.white;

	public Designator_Plan_Copy()
	{
		useMouseIcon = false;
		icon = ContentFinder<Texture2D>.Get("UI/Designators/PlanCopy");
		defaultLabel = "CommandCopyPlanLabel".Translate();
		defaultDesc = "CommandCopyPlanDesc".Translate();
		hotKey = KeyBindingDefOf.Designator_Cancel;
	}

	public override void SelectedUpdate()
	{
		base.SelectedUpdate();
		cells.Clear();
		foreach (IntVec3 currentCell in GetCurrentCells(UI.MouseCell()))
		{
			if (currentCell.InBounds(base.Map) && !currentCell.InNoBuildEdgeArea(base.Map))
			{
				cells.Add(currentCell);
			}
		}
		GenDraw.DrawFieldEdges(cells, colorDef.color, null, null, 3600);
		GenDraw.DrawNoZoneEdgeLines();
	}

	public override AcceptanceReport CanDesignateCell(IntVec3 loc)
	{
		foreach (IntVec3 currentCell in GetCurrentCells(loc))
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
		return true;
	}

	public override void DesignateSingleCell(IntVec3 c)
	{
		PlanCells(cells);
		base.SelectedPlan = null;
	}

	[IteratorStateMachine(typeof(_003CGetCurrentCells_003Ed__21))]
	protected IEnumerable<IntVec3> GetCurrentCells(IntVec3 c)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetCurrentCells_003Ed__21(-2)
		{
			_003C_003E4__this = this,
			_003C_003E3__c = c
		};
	}

	public override void Selected()
	{
		Plan selectedPlan = base.SelectedPlan;
		CellRect cellRect = CellRect.FromCellList(selectedPlan.Cells);
		colorDef = selectedPlan.Color;
		rotation = Rot4.North;
		grid = new BoolGrid(cellRect.Width, cellRect.Height);
		foreach (IntVec3 cell in selectedPlan.Cells)
		{
			IntVec3 c = cell - cellRect.Min;
			grid.Set(c, value: true);
		}
		base.SelectedPlan = null;
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
