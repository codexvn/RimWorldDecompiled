using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02002BEB RID: 11243
	public class CompProperties_AssignableToPawn : CompProperties
	{
		// Token: 0x0600FBC8 RID: 64456 RVA: 0x00491534 File Offset: 0x0048F734
		public CompProperties_AssignableToPawn()
		{
			this.compClass = typeof(CompAssignableToPawn);
		}

		// Token: 0x0600FBC9 RID: 64457 RVA: 0x002B5875 File Offset: 0x002B3A75
		public override IEnumerable<string> ConfigErrors(ThingDef parentDef)
		{
			return base.ConfigErrors(parentDef);
		}

		// Token: 0x0600FBCA RID: 64458 RVA: 0x00491561 File Offset: 0x0048F761
		public override void PostLoadSpecial(ThingDef parent)
		{
			if (parent.thingClass == typeof(Building_Bed))
			{
				this.maxAssignedPawnsCount = BedUtility.GetSleepingSlotsCount(parent.size);
			}
		}

		// Token: 0x0400A522 RID: 42274
		public int maxAssignedPawnsCount = 1;

		// Token: 0x0400A523 RID: 42275
		public bool drawAssignmentOverlay = true;

		// Token: 0x0400A524 RID: 42276
		public bool drawUnownedAssignmentOverlay = true;

		// Token: 0x0400A525 RID: 42277
		[MustTranslate]
		public string noAssignablePawnsDesc;
	}
}
