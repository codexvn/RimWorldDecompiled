using System;

namespace RimWorld
{
	// Token: 0x020034BE RID: 13502
	public class TransferableComparer_Mass : TransferableComparer
	{
		// Token: 0x06012FA1 RID: 77729 RVA: 0x005A183C File Offset: 0x0059FA3C
		public override int Compare(Transferable lhs, Transferable rhs)
		{
			return lhs.AnyThing.GetStatValue(StatDefOf.Mass, true, -1).CompareTo(rhs.AnyThing.GetStatValue(StatDefOf.Mass, true, -1));
		}
	}
}
