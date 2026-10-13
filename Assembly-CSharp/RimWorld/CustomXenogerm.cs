using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02001B3C RID: 6972
	public class CustomXenogerm : IExposable
	{
		// Token: 0x06009BBF RID: 39871 RVA: 0x002D2910 File Offset: 0x002D0B10
		public CustomXenogerm()
		{
		}

		// Token: 0x06009BC0 RID: 39872 RVA: 0x002D2924 File Offset: 0x002D0B24
		public CustomXenogerm(string name, XenotypeIconDef iconDef, IEnumerable<Genepack> genepacks)
		{
			this.genesets.AddRange(from gp in genepacks
			select gp.GeneSet.Copy());
			this.iconDef = iconDef;
			this.name = name;
		}

		// Token: 0x06009BC1 RID: 39873 RVA: 0x002D2980 File Offset: 0x002D0B80
		public void ExposeData()
		{
			Scribe_Values.Look<string>(ref this.name, "name", null, false);
			Scribe_Collections.Look<GeneSet>(ref this.genesets, "genesets", LookMode.Deep, Array.Empty<object>());
			Scribe_Defs.Look<XenotypeIconDef>(ref this.iconDef, "iconDef");
		}

		// Token: 0x04006C25 RID: 27685
		public string name;

		// Token: 0x04006C26 RID: 27686
		public List<GeneSet> genesets = new List<GeneSet>();

		// Token: 0x04006C27 RID: 27687
		public XenotypeIconDef iconDef;
	}
}
