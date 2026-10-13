using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020030F2 RID: 12530
	public class Designator_Plan_Copy : Designator_Plan_Add
	{
		// Token: 0x17002DAE RID: 11694
		// (get) Token: 0x060119A1 RID: 72097 RVA: 0x00002501 File Offset: 0x00000701
		public override bool DragDrawMeasurements
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17002DAF RID: 11695
		// (get) Token: 0x060119A2 RID: 72098 RVA: 0x00002501 File Offset: 0x00000701
		protected override bool CanSelectColor
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17002DB0 RID: 11696
		// (get) Token: 0x060119A3 RID: 72099 RVA: 0x00002C42 File Offset: 0x00000E42
		public override DrawStyleCategoryDef DrawStyleCategory
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17002DB1 RID: 11697
		// (get) Token: 0x060119A4 RID: 72100 RVA: 0x000028E7 File Offset: 0x00000AE7
		public override bool AlwaysDoGuiControls
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17002DB2 RID: 11698
		// (get) Token: 0x060119A5 RID: 72101 RVA: 0x00002501 File Offset: 0x00000701
		public override bool CanRightClickToggleVisibility
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17002DB3 RID: 11699
		// (get) Token: 0x060119A6 RID: 72102 RVA: 0x00505E0A File Offset: 0x0050400A
		public float PaneTopY
		{
			get
			{
				return (float)(UI.screenHeight - 35);
			}
		}

		// Token: 0x17002DB4 RID: 11700
		// (get) Token: 0x060119A7 RID: 72103 RVA: 0x000440FD File Offset: 0x000422FD
		public override Color IconDrawColor
		{
			get
			{
				return Color.white;
			}
		}

		// Token: 0x060119A8 RID: 72104 RVA: 0x0050DA08 File Offset: 0x0050BC08
		public Designator_Plan_Copy()
		{
			this.useMouseIcon = false;
			this.icon = ContentFinder<Texture2D>.Get("UI/Designators/PlanCopy", true);
			this.defaultLabel = "CommandCopyPlanLabel".Translate();
			this.defaultDesc = "CommandCopyPlanDesc".Translate();
			this.hotKey = KeyBindingDefOf.Designator_Cancel;
		}

		// Token: 0x060119A9 RID: 72105 RVA: 0x0050DA74 File Offset: 0x0050BC74
		public override void SelectedUpdate()
		{
			base.SelectedUpdate();
			this.cells.Clear();
			foreach (IntVec3 intVec in this.GetCurrentCells(UI.MouseCell()))
			{
				if (intVec.InBounds(base.Map) && !intVec.InNoBuildEdgeArea(base.Map))
				{
					this.cells.Add(intVec);
				}
			}
			GenDraw.DrawFieldEdges(this.cells, this.colorDef.color, null, null, 3600);
			GenDraw.DrawNoZoneEdgeLines();
		}

		// Token: 0x060119AA RID: 72106 RVA: 0x0050DB24 File Offset: 0x0050BD24
		public override AcceptanceReport CanDesignateCell(IntVec3 loc)
		{
			foreach (IntVec3 c in this.GetCurrentCells(loc))
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
			return true;
		}

		// Token: 0x060119AB RID: 72107 RVA: 0x0050DBAC File Offset: 0x0050BDAC
		public override void DesignateSingleCell(IntVec3 c)
		{
			base.PlanCells(this.cells);
			base.SelectedPlan = null;
		}

		// Token: 0x060119AC RID: 72108 RVA: 0x0050DBC1 File Offset: 0x0050BDC1
		protected IEnumerable<IntVec3> GetCurrentCells(IntVec3 c)
		{
			IntVec3 root = PrefabUtility.GetRoot(c, new IntVec2(this.grid.Width, this.grid.Height), this.rotation);
			int num;
			for (int x = 0; x < this.grid.Width; x = num + 1)
			{
				for (int z = 0; z < this.grid.Height; z = num + 1)
				{
					if (this.grid[x, z])
					{
						IntVec3 adjustedLocalPosition = PrefabUtility.GetAdjustedLocalPosition(new IntVec3(x, 0, z), this.rotation);
						yield return root + adjustedLocalPosition;
					}
					num = z;
				}
				num = x;
			}
			yield break;
		}

		// Token: 0x060119AD RID: 72109 RVA: 0x0050DBD8 File Offset: 0x0050BDD8
		public override void Selected()
		{
			Plan selectedPlan = base.SelectedPlan;
			CellRect cellRect = CellRect.FromCellList(selectedPlan.Cells);
			this.colorDef = selectedPlan.Color;
			this.rotation = Rot4.North;
			this.grid = new BoolGrid(cellRect.Width, cellRect.Height);
			foreach (IntVec3 a in selectedPlan.Cells)
			{
				IntVec3 c = a - cellRect.Min;
				this.grid.Set(c, true);
			}
			base.SelectedPlan = null;
		}

		// Token: 0x060119AE RID: 72110 RVA: 0x0050DC88 File Offset: 0x0050BE88
		public override void DoExtraGuiControls(float leftX, float bottomY)
		{
			DesignatorUtility.GUIDoRotationControls(leftX, bottomY, this.rotation, delegate(Rot4 rot)
			{
				this.rotation = rot;
			});
		}

		// Token: 0x060119AF RID: 72111 RVA: 0x0050DCA4 File Offset: 0x0050BEA4
		public override void SelectedProcessInput(Event ev)
		{
			RotationDirection rotationDirection = RotationDirection.None;
			if (Event.current.button == 2)
			{
				if (Event.current.type == EventType.MouseDown)
				{
					Event.current.Use();
					Designator_Plan_Copy.middleMouseDownTime = Time.realtimeSinceStartup;
				}
				if (Event.current.type == EventType.MouseUp && Time.realtimeSinceStartup - Designator_Plan_Copy.middleMouseDownTime < 0.15f)
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

		// Token: 0x0400B570 RID: 46448
		private BoolGrid grid;

		// Token: 0x0400B571 RID: 46449
		private Rot4 rotation;

		// Token: 0x0400B572 RID: 46450
		private readonly List<IntVec3> cells = new List<IntVec3>();

		// Token: 0x0400B573 RID: 46451
		private static float middleMouseDownTime;
	}
}
