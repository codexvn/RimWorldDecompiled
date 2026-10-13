using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02003836 RID: 14390
	public static class GeneMaker
	{
		// Token: 0x06013E28 RID: 81448 RVA: 0x005E73AE File Offset: 0x005E55AE
		public static Gene MakeGene(GeneDef def, Pawn pawn)
		{
			Gene gene = (Gene)Activator.CreateInstance(def.geneClass);
			gene.def = def;
			gene.pawn = pawn;
			gene.loadID = Find.UniqueIDsManager.GetNextGeneID();
			gene.PostMake();
			return gene;
		}
	}
}
