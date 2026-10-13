using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02002D5D RID: 11613
	public class FocusStrengthOffset_BuildingDefsLit : FocusStrengthOffset_BuildingDefs
	{
		// Token: 0x0601050F RID: 66831 RVA: 0x004B6192 File Offset: 0x004B4392
		public override bool CanApply(Thing parent, Pawn user = null)
		{
			return this.BuildingLit(parent) && base.CanApply(parent, user);
		}

		// Token: 0x06010510 RID: 66832 RVA: 0x004B61A7 File Offset: 0x004B43A7
		protected override float OffsetForBuilding(Thing b)
		{
			if (this.BuildingLit(b))
			{
				return base.OffsetFor(b.def);
			}
			return 0f;
		}

		// Token: 0x06010511 RID: 66833 RVA: 0x004B61C4 File Offset: 0x004B43C4
		private bool BuildingLit(Thing b)
		{
			CompGlower compGlower = b.TryGetComp<CompGlower>();
			return compGlower != null && compGlower.Glows;
		}

		// Token: 0x06010512 RID: 66834 RVA: 0x004B61E4 File Offset: 0x004B43E4
		protected override int BuildingCount(Thing parent)
		{
			if (parent == null || !parent.Spawned)
			{
				return 0;
			}
			int num = 0;
			List<Thing> forCell = parent.Map.listerBuldingOfDefInProximity.GetForCell(parent.Position, this.radius, this.defs, parent);
			for (int i = 0; i < forCell.Count; i++)
			{
				Thing b = forCell[i];
				if (this.BuildingLit(b))
				{
					num++;
				}
			}
			return Math.Min(num, this.maxBuildings);
		}
	}
}
