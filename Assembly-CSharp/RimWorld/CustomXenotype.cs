using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02003152 RID: 12626
	public class CustomXenotype : IExposable
	{
		// Token: 0x17002E2D RID: 11821
		// (get) Token: 0x06011C34 RID: 72756 RVA: 0x005213D7 File Offset: 0x0051F5D7
		public XenotypeIconDef IconDef
		{
			get
			{
				return this.iconDef ?? XenotypeIconDefOf.Basic;
			}
		}

		// Token: 0x06011C35 RID: 72757 RVA: 0x005213E8 File Offset: 0x0051F5E8
		public void ExposeData()
		{
			Scribe_Values.Look<string>(ref this.name, "name", null, false);
			Scribe_Values.Look<bool>(ref this.inheritable, "inheritable", false, false);
			Scribe_Collections.Look<GeneDef>(ref this.genes, "genes", LookMode.Def, Array.Empty<object>());
			Scribe_Defs.Look<XenotypeIconDef>(ref this.iconDef, "iconDef");
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				if (this.genes.RemoveAll((GeneDef x) => x == null) > 0)
				{
					Log.Error("Removed null genes");
				}
			}
		}

		// Token: 0x0400B742 RID: 46914
		public string fileName;

		// Token: 0x0400B743 RID: 46915
		public string name;

		// Token: 0x0400B744 RID: 46916
		public List<GeneDef> genes = new List<GeneDef>();

		// Token: 0x0400B745 RID: 46917
		public XenotypeIconDef iconDef;

		// Token: 0x0400B746 RID: 46918
		public bool inheritable;
	}
}
