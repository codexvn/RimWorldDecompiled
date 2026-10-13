using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020030F5 RID: 12533
	public class Designator_Plan_CopySelectionPaste : Designator_Plan_Add
	{
		// Token: 0x17002DB9 RID: 11705
		// (get) Token: 0x060119C0 RID: 72128 RVA: 0x000028E7 File Offset: 0x00000AE7
		public override bool DragDrawMeasurements
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17002DBA RID: 11706
		// (get) Token: 0x060119C1 RID: 72129 RVA: 0x00002C42 File Offset: 0x00000E42
		public override DrawStyleCategoryDef DrawStyleCategory
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17002DBB RID: 11707
		// (get) Token: 0x060119C2 RID: 72130 RVA: 0x000028E7 File Offset: 0x00000AE7
		public override bool AlwaysDoGuiControls
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17002DBC RID: 11708
		// (get) Token: 0x060119C3 RID: 72131 RVA: 0x00505E0A File Offset: 0x0050400A
		public float PaneTopY
		{
			get
			{
				return (float)(UI.screenHeight - 35);
			}
		}

		// Token: 0x17002DBD RID: 11709
		// (get) Token: 0x060119C4 RID: 72132 RVA: 0x00002501 File Offset: 0x00000701
		public override bool CanRightClickToggleVisibility
		{
			get
			{
				return false;
			}
		}

		// Token: 0x060119C5 RID: 72133 RVA: 0x0050DFD4 File Offset: 0x0050C1D4
		public Designator_Plan_CopySelectionPaste()
		{
			this.useMouseIcon = true;
			this.hideMouseIcon = true;
			this.mouseText = "CommandCopyPlanSelectionPasteMouse".Translate();
		}

		// Token: 0x060119C6 RID: 72134 RVA: 0x0050E02C File Offset: 0x0050C22C
		public override void SelectedUpdate()
		{
			base.SelectedUpdate();
			foreach (ColorDef colorDef in this.colors)
			{
				foreach (IntVec3 intVec in this.GetCurrentCells(UI.MouseCell(), colorDef))
				{
					if (intVec.InBounds(base.Map) && !intVec.InNoBuildEdgeArea(base.Map))
					{
						this.cells.Add(intVec);
					}
				}
				GenDraw.DrawFieldEdges(this.cells, colorDef.color, null, null, 3600);
				this.cells.Clear();
			}
			GenDraw.DrawNoZoneEdgeLines();
		}

		// Token: 0x060119C7 RID: 72135 RVA: 0x0050E118 File Offset: 0x0050C318
		public override AcceptanceReport CanDesignateCell(IntVec3 loc)
		{
			foreach (ColorDef color in this.colors)
			{
				foreach (IntVec3 c in this.GetCurrentCells(loc, color))
				{
					if (!c.InBounds(base.Map))
					{
						return false;
					}
					if (c.InNoBuildEdgeArea(base.Map))
					{
						return "TooCloseToMapEdge".Translate();
					}
				}
			}
			return true;
		}

		// Token: 0x060119C8 RID: 72136 RVA: 0x0050E1E0 File Offset: 0x0050C3E0
		public override void DesignateSingleCell(IntVec3 c)
		{
			foreach (ColorDef colorDef in this.colors)
			{
				base.SelectedPlan = null;
				this.colorDef = colorDef;
				base.PlanCells(this.GetCurrentCells(c, colorDef));
			}
			base.SelectedPlan = null;
		}

		// Token: 0x060119C9 RID: 72137 RVA: 0x0050E250 File Offset: 0x0050C450
		protected IEnumerable<IntVec3> GetCurrentCells(IntVec3 c, ColorDef color)
		{
			IntVec3 root = PrefabUtility.GetRoot(c, new IntVec2(this.indices.SizeX, this.indices.SizeZ), this.rotation);
			int num;
			for (int i = 0; i < this.indices.NumGridCells; i = num + 1)
			{
				if (this.grid[i] == color)
				{
					yield return root + PrefabUtility.GetAdjustedLocalPosition(this.indices[i], this.rotation);
				}
				num = i;
			}
			yield break;
		}

		// Token: 0x060119CA RID: 72138 RVA: 0x0050E270 File Offset: 0x0050C470
		public void Initialize(IEnumerable<IntVec3> selected)
		{
			this.rotation = Rot4.North;
			Designator_Plan_CopySelectionPaste.tmpSelected.Clear();
			Designator_Plan_CopySelectionPaste.tmpSelected.AddRange(selected);
			CellRect cellRect = CellRect.FromCellList(Designator_Plan_CopySelectionPaste.tmpSelected);
			this.indices = new CellIndices(cellRect.Size.x, cellRect.Size.z);
			this.grid.Clear();
			this.colors.Clear();
			this.grid.Capacity = Mathf.Max(this.grid.Capacity, cellRect.Area);
			for (int i = 0; i < this.indices.NumGridCells; i++)
			{
				IntVec3 c = cellRect.Min + this.indices[i];
				Plan plan = base.Map.planManager.PlanAt(c);
				ColorDef colorDef = (plan != null) ? plan.Color : null;
				this.grid.Add(colorDef);
				if (colorDef != null)
				{
					this.colors.Add(colorDef);
				}
			}
		}

		// Token: 0x060119CB RID: 72139 RVA: 0x0050E36C File Offset: 0x0050C56C
		public override void DoExtraGuiControls(float leftX, float bottomY)
		{
			DesignatorUtility.GUIDoRotationControls(leftX, bottomY, this.rotation, delegate(Rot4 rot)
			{
				this.rotation = rot;
			});
		}

		// Token: 0x060119CC RID: 72140 RVA: 0x0050E388 File Offset: 0x0050C588
		public override void SelectedProcessInput(Event ev)
		{
			RotationDirection rotationDirection = RotationDirection.None;
			if (Event.current.button == 2)
			{
				if (Event.current.type == EventType.MouseDown)
				{
					Event.current.Use();
					Designator_Plan_CopySelectionPaste.middleMouseDownTime = Time.realtimeSinceStartup;
				}
				if (Event.current.type == EventType.MouseUp && Time.realtimeSinceStartup - Designator_Plan_CopySelectionPaste.middleMouseDownTime < 0.15f)
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
			if (rotationDirection != RotationDirection.None)
			{
				this.rotation.Rotate(rotationDirection);
			}
			DesignatorUtility.GUIDoRotationControls(0f, this.PaneTopY, this.rotation, delegate(Rot4 rot)
			{
				this.rotation = rot;
			});
		}

		// Token: 0x0400B57D RID: 46461
		private readonly List<ColorDef> grid = new List<ColorDef>();

		// Token: 0x0400B57E RID: 46462
		private readonly HashSet<ColorDef> colors = new HashSet<ColorDef>();

		// Token: 0x0400B57F RID: 46463
		private CellIndices indices;

		// Token: 0x0400B580 RID: 46464
		private Rot4 rotation;

		// Token: 0x0400B581 RID: 46465
		private readonly List<IntVec3> cells = new List<IntVec3>();

		// Token: 0x0400B582 RID: 46466
		private static readonly List<IntVec3> tmpSelected = new List<IntVec3>();

		// Token: 0x0400B583 RID: 46467
		private static float middleMouseDownTime;
	}
}
