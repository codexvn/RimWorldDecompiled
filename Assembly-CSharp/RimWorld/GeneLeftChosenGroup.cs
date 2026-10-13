using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020031E1 RID: 12769
	public class GeneLeftChosenGroup
	{
		// Token: 0x06011F3D RID: 73533 RVA: 0x00539D99 File Offset: 0x00537F99
		public GeneLeftChosenGroup(GeneDef left)
		{
			this.leftChosen = left;
		}

		// Token: 0x0400B9E4 RID: 47588
		public GeneDef leftChosen;

		// Token: 0x0400B9E5 RID: 47589
		public List<GeneDef> overriddenGenes = new List<GeneDef>();
	}
}
