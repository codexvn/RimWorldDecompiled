using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020029C7 RID: 10695
	public class Building_AncientCommsConsole : Building
	{
		// Token: 0x0600EBC5 RID: 60357 RVA: 0x00447A34 File Offset: 0x00445C34
		public override string GetInspectString()
		{
			return base.GetInspectString() + ("\n" + "LinkedTo".Translate() + ": " + "SupplySatellite".Translate());
		}
	}
}
