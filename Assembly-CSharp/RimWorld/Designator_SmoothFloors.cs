using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020030C2 RID: 12482
	public class Designator_SmoothFloors : Designator_Smooth
	{
		// Token: 0x06011869 RID: 71785 RVA: 0x0050868C File Offset: 0x0050688C
		public Designator_SmoothFloors()
		{
			this.defaultLabel = "DesignatorSmoothFloors".Translate();
			this.defaultDesc = "DesignatorSmoothFloorsDesc".Translate();
			this.icon = ContentFinder<Texture2D>.Get("UI/Designators/SmoothSurface", true);
		}

		// Token: 0x0601186A RID: 71786 RVA: 0x005086DA File Offset: 0x005068DA
		public override void DesignateSingleCell(IntVec3 c)
		{
			SmoothSurfaceDesignatorUtility.DesignateSmoothFloor(base.Map, c);
		}

		// Token: 0x0601186B RID: 71787 RVA: 0x005086E8 File Offset: 0x005068E8
		public override AcceptanceReport CanDesignateCell(IntVec3 c)
		{
			AcceptanceReport result = base.CanDesignateCell(c);
			if (!result.Accepted)
			{
				return result;
			}
			if (base.Map.designationManager.DesignationAt(c, DesignationDefOf.SmoothFloor) != null)
			{
				return "SurfaceBeingSmoothed".Translate();
			}
			Building edifice = c.GetEdifice(base.Map);
			if (edifice != null && !SmoothSurfaceDesignatorUtility.CanSmoothFloorUnder(edifice))
			{
				return "MessageMustDesignateSmoothableFloor".Translate();
			}
			if (!c.GetAffordances(base.Map).Contains(TerrainAffordanceDefOf.SmoothableStone))
			{
				return "MessageMustDesignateSmoothableFloor".Translate();
			}
			return AcceptanceReport.WasAccepted;
		}
	}
}
