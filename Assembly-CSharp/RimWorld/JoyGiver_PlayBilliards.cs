using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020014DC RID: 5340
	public class JoyGiver_PlayBilliards : JoyGiver_InteractBuilding
	{
		// Token: 0x170013A9 RID: 5033
		// (get) Token: 0x06007F91 RID: 32657 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected override bool CanDoDuringGathering
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06007F92 RID: 32658 RVA: 0x002663C1 File Offset: 0x002645C1
		protected override Job TryGivePlayJob(Pawn pawn, Thing t)
		{
			if (!JoyGiver_PlayBilliards.ThingHasStandableSpaceOnAllSides(t))
			{
				return null;
			}
			return JobMaker.MakeJob(this.def.jobDef, t);
		}

		// Token: 0x06007F93 RID: 32659 RVA: 0x002663E4 File Offset: 0x002645E4
		public static bool ThingHasStandableSpaceOnAllSides(Thing t)
		{
			CellRect cellRect = t.OccupiedRect();
			foreach (IntVec3 c in cellRect.ExpandedBy(1))
			{
				if (!cellRect.Contains(c) && !c.Standable(t.Map))
				{
					return false;
				}
			}
			return true;
		}
	}
}
