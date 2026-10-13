using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002C0A RID: 11274
	public class CompProperties_Biocodable : CompProperties
	{
		// Token: 0x0600FCA0 RID: 64672 RVA: 0x004943EF File Offset: 0x004925EF
		public CompProperties_Biocodable()
		{
			this.compClass = typeof(CompBiocodable);
		}

		// Token: 0x0400A596 RID: 42390
		public bool biocodeOnEquip;
	}
}
