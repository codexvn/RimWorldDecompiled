using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020030C3 RID: 12483
	public class Designator_SmoothSurface : Designator_Smooth
	{
		// Token: 0x0601186C RID: 71788 RVA: 0x00508788 File Offset: 0x00506988
		public Designator_SmoothSurface()
		{
			this.defaultLabel = "DesignatorSmoothSurface".Translate();
			this.defaultDesc = "DesignatorSmoothSurfaceDesc".Translate();
			this.icon = ContentFinder<Texture2D>.Get("UI/Designators/SmoothSurface", true);
		}

		// Token: 0x0601186D RID: 71789 RVA: 0x005059B8 File Offset: 0x00503BB8
		public override void DesignateThing(Thing t)
		{
			this.DesignateSingleCell(t.Position);
		}

		// Token: 0x0601186E RID: 71790 RVA: 0x005087D8 File Offset: 0x005069D8
		public override AcceptanceReport CanDesignateCell(IntVec3 c)
		{
			AcceptanceReport result = base.CanDesignateCell(c);
			if (!result.Accepted)
			{
				return result;
			}
			if (base.Map.designationManager.DesignationAt(c, DesignationDefOf.SmoothFloor) != null || base.Map.designationManager.DesignationAt(c, DesignationDefOf.SmoothWall) != null)
			{
				return "SurfaceBeingSmoothed".Translate();
			}
			Building edifice = c.GetEdifice(base.Map);
			if (edifice != null && edifice.def.IsSmoothable)
			{
				return AcceptanceReport.WasAccepted;
			}
			if (edifice != null && !SmoothSurfaceDesignatorUtility.CanSmoothFloorUnder(edifice))
			{
				return "MessageMustDesignateSmoothableSurface".Translate();
			}
			if (!c.GetAffordances(base.Map).Contains(TerrainAffordanceDefOf.SmoothableStone))
			{
				return "MessageMustDesignateSmoothableSurface".Translate();
			}
			return AcceptanceReport.WasAccepted;
		}

		// Token: 0x0601186F RID: 71791 RVA: 0x005088A3 File Offset: 0x00506AA3
		public override void DesignateSingleCell(IntVec3 c)
		{
			if (!SmoothSurfaceDesignatorUtility.DesignateSmoothWall(base.Map, c))
			{
				SmoothSurfaceDesignatorUtility.DesignateSmoothFloor(base.Map, c);
			}
		}
	}
}
