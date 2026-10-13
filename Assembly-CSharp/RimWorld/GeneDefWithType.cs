using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02003887 RID: 14471
	public struct GeneDefWithType
	{
		// Token: 0x17003308 RID: 13064
		// (get) Token: 0x06014010 RID: 81936 RVA: 0x005F0AB0 File Offset: 0x005EECB0
		public bool RandomChosen
		{
			get
			{
				return this.geneDef.RandomChosen;
			}
		}

		// Token: 0x06014011 RID: 81937 RVA: 0x005F0ABD File Offset: 0x005EECBD
		public GeneDefWithType(GeneDef geneDef, bool xenogene)
		{
			this.geneDef = geneDef;
			this.isXenogene = xenogene;
		}

		// Token: 0x06014012 RID: 81938 RVA: 0x005F0ACD File Offset: 0x005EECCD
		public bool ConflictsWith(GeneDefWithType other)
		{
			return this.geneDef.ConflictsWith(other.geneDef);
		}

		// Token: 0x06014013 RID: 81939 RVA: 0x005F0AE0 File Offset: 0x005EECE0
		public bool Overrides(GeneDefWithType other)
		{
			return this.geneDef.Overrides(other.geneDef, this.isXenogene, other.isXenogene);
		}

		// Token: 0x0400DD0C RID: 56588
		public GeneDef geneDef;

		// Token: 0x0400DD0D RID: 56589
		public bool isXenogene;
	}
}
