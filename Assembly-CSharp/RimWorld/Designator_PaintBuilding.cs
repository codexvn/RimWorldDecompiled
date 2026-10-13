using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020030B3 RID: 12467
	[StaticConstructorOnStartup]
	public class Designator_PaintBuilding : Designator_Paint
	{
		// Token: 0x17002D59 RID: 11609
		// (get) Token: 0x06011815 RID: 71701 RVA: 0x00506F9C File Offset: 0x0050519C
		protected override Texture2D IconTopTex
		{
			get
			{
				return ContentFinder<Texture2D>.Get("UI/Designators/Paint_Top", true);
			}
		}

		// Token: 0x17002D5A RID: 11610
		// (get) Token: 0x06011816 RID: 71702 RVA: 0x00506FA9 File Offset: 0x005051A9
		protected override DesignationDef Designation
		{
			get
			{
				return DesignationDefOf.PaintBuilding;
			}
		}

		// Token: 0x06011817 RID: 71703 RVA: 0x00506FB0 File Offset: 0x005051B0
		public Designator_PaintBuilding()
		{
			this.defaultLabel = "DesignatorPaintBuilding".Translate();
			this.defaultDesc = "DesignatorPaintBuildingDesc".Translate();
			this.icon = ContentFinder<Texture2D>.Get("UI/Designators/Paint_Bottom", true);
			this.tutorTag = "PaintBuilding";
		}

		// Token: 0x06011818 RID: 71704 RVA: 0x0050700C File Offset: 0x0050520C
		public override AcceptanceReport CanDesignateCell(IntVec3 c)
		{
			if (this.eyedropMode)
			{
				return this.eyedropper.CanDesignateCell(c);
			}
			if (!c.InBounds(base.Map) || c.Fogged(base.Map))
			{
				return false;
			}
			List<Thing> thingList = c.GetThingList(base.Map);
			for (int i = 0; i < thingList.Count; i++)
			{
				if (this.CanDesignateThing(thingList[i]))
				{
					return true;
				}
			}
			return "MessageMustDesignatePaintableBuildings".Translate(this.colorDef);
		}

		// Token: 0x06011819 RID: 71705 RVA: 0x005070A4 File Offset: 0x005052A4
		public override void DesignateSingleCell(IntVec3 c)
		{
			if (this.eyedropMode)
			{
				this.eyedropper.DesignateSingleCell(c);
				return;
			}
			List<Thing> thingList = c.GetThingList(base.Map);
			for (int i = 0; i < thingList.Count; i++)
			{
				if (this.CanDesignateThing(thingList[i]).Accepted)
				{
					this.DesignateThing(thingList[i]);
				}
			}
		}

		// Token: 0x0601181A RID: 71706 RVA: 0x00507108 File Offset: 0x00505308
		public override AcceptanceReport CanDesignateThing(Thing t)
		{
			if (t.def.building == null || !t.def.building.paintable)
			{
				return false;
			}
			if (t.Faction != Faction.OfPlayer)
			{
				return false;
			}
			Building building = t as Building;
			if (building != null && building.PaintColorDef == this.colorDef)
			{
				return false;
			}
			if (base.Map.designationManager.DesignationOn(t, DesignationDefOf.Uninstall) != null)
			{
				return false;
			}
			return true;
		}

		// Token: 0x0601181B RID: 71707 RVA: 0x00507194 File Offset: 0x00505394
		public override void DesignateThing(Thing t)
		{
			base.Map.designationManager.TryRemoveDesignationOn(t, this.Designation);
			base.Map.designationManager.TryRemoveDesignationOn(t, DesignationDefOf.RemovePaintBuilding);
			if (DebugSettings.godMode)
			{
				((Building)t).ChangePaint(this.colorDef);
				return;
			}
			base.Map.designationManager.AddDesignation(new Designation(t, this.Designation, this.colorDef));
		}

		// Token: 0x0601181C RID: 71708 RVA: 0x00507210 File Offset: 0x00505410
		protected override int NumHighlightedCells()
		{
			Designator_PaintBuilding.tmpPaintThings.Clear();
			Find.DesignatorManager.Dragger.UpdateCellBuffer();
			foreach (IntVec3 c in Find.DesignatorManager.Dragger.CellBuffer)
			{
				if (c.InBounds(base.Map) && !c.Fogged(base.Map))
				{
					List<Thing> thingList = c.GetThingList(base.Map);
					for (int i = 0; i < thingList.Count; i++)
					{
						if (!Designator_PaintBuilding.tmpPaintThings.Contains(thingList[i]) && this.CanDesignateThing(thingList[i]))
						{
							Designator_PaintBuilding.tmpPaintThings.Add(thingList[i]);
						}
					}
				}
			}
			return Designator_PaintBuilding.tmpPaintThings.Count;
		}

		// Token: 0x0400B4E7 RID: 46311
		private static readonly HashSet<Thing> tmpPaintThings = new HashSet<Thing>();
	}
}
