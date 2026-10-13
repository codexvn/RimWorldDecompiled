using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02003C54 RID: 15444
	public class MoveableArea_Allowed : MoveableArea
	{
		// Token: 0x06014FB7 RID: 85943 RVA: 0x00647F47 File Offset: 0x00646147
		public MoveableArea_Allowed()
		{
		}

		// Token: 0x06014FB8 RID: 85944 RVA: 0x00647F5A File Offset: 0x0064615A
		public MoveableArea_Allowed(Gravship gravship, Area area) : base(gravship, area.Label, area.RenamableLabel, area.Color, area.ID)
		{
		}

		// Token: 0x06014FB9 RID: 85945 RVA: 0x00647F86 File Offset: 0x00646186
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<Pawn>(ref this.assignedPawns, "assignedPawns", LookMode.Reference, Array.Empty<object>());
		}

		// Token: 0x06014FBA RID: 85946 RVA: 0x00647FA4 File Offset: 0x006461A4
		public bool TryCreateArea(AreaManager areaManager, IntVec3 newOrigin)
		{
			if (this.relativeCells.Count == 0)
			{
				return false;
			}
			Area_Allowed area_Allowed = areaManager.GetLabeled(this.label) as Area_Allowed;
			if (area_Allowed == null && !areaManager.TryMakeNewAllowed(out area_Allowed))
			{
				return area_Allowed != null;
			}
			foreach (IntVec3 b in base.RelativeCells)
			{
				area_Allowed[newOrigin + b] = true;
			}
			area_Allowed.SetColor(this.color);
			area_Allowed.SetLabel(this.label);
			area_Allowed.RenamableLabel = this.renamableLabel;
			foreach (Pawn pawn in this.assignedPawns)
			{
				pawn.playerSettings.AreaRestrictionInPawnCurrentMap = area_Allowed;
			}
			return true;
		}

		// Token: 0x0400E9E6 RID: 59878
		public List<Pawn> assignedPawns = new List<Pawn>();
	}
}
