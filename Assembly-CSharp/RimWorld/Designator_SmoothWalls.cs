using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020030C1 RID: 12481
	public class Designator_SmoothWalls : Designator_Smooth
	{
		// Token: 0x06011864 RID: 71780 RVA: 0x00508570 File Offset: 0x00506770
		public Designator_SmoothWalls()
		{
			this.defaultLabel = "DesignatorSmoothWalls".Translate();
			this.defaultDesc = "DesignatorSmoothWallsDesc".Translate();
			this.icon = ContentFinder<Texture2D>.Get("UI/Designators/SmoothSurface", true);
		}

		// Token: 0x06011865 RID: 71781 RVA: 0x005085C0 File Offset: 0x005067C0
		public override AcceptanceReport CanDesignateThing(Thing t)
		{
			if (t != null && t.def.IsSmoothable && this.CanDesignateCell(t.Position).Accepted)
			{
				return AcceptanceReport.WasAccepted;
			}
			return false;
		}

		// Token: 0x06011866 RID: 71782 RVA: 0x005059B8 File Offset: 0x00503BB8
		public override void DesignateThing(Thing t)
		{
			this.DesignateSingleCell(t.Position);
		}

		// Token: 0x06011867 RID: 71783 RVA: 0x005085FF File Offset: 0x005067FF
		public override void DesignateSingleCell(IntVec3 c)
		{
			SmoothSurfaceDesignatorUtility.DesignateSmoothWall(base.Map, c);
		}

		// Token: 0x06011868 RID: 71784 RVA: 0x00508610 File Offset: 0x00506810
		public override AcceptanceReport CanDesignateCell(IntVec3 c)
		{
			AcceptanceReport result = base.CanDesignateCell(c);
			if (!result.Accepted)
			{
				return result;
			}
			if (base.Map.designationManager.DesignationAt(c, DesignationDefOf.SmoothWall) != null)
			{
				return "SurfaceBeingSmoothed".Translate();
			}
			Building edifice = c.GetEdifice(base.Map);
			if (edifice == null || !edifice.def.IsSmoothable)
			{
				return "MessageMustDesignateSmoothableWall".Translate();
			}
			return AcceptanceReport.WasAccepted;
		}
	}
}
