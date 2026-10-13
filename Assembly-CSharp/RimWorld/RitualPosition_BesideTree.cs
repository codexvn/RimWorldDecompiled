using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200279C RID: 10140
	public class RitualPosition_BesideTree : RitualPosition_ThingDef
	{
		// Token: 0x170021F0 RID: 8688
		// (get) Token: 0x0600DE1D RID: 56861 RVA: 0x0041597A File Offset: 0x00413B7A
		protected override ThingDef ThingDef
		{
			get
			{
				return ThingDefOf.Plant_TreeGauranlen;
			}
		}

		// Token: 0x0600DE1E RID: 56862 RVA: 0x00415981 File Offset: 0x00413B81
		public override IntVec3 PositionForThing(Thing t)
		{
			return t.Position - new IntVec3(1, 0, 0);
		}

		// Token: 0x0600DE1F RID: 56863 RVA: 0x00415996 File Offset: 0x00413B96
		protected override Rot4 FacingDir(Thing t)
		{
			return Rot4.East;
		}
	}
}
