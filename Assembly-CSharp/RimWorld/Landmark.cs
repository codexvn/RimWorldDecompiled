using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002828 RID: 10280
	public class Landmark : IExposable
	{
		// Token: 0x0600E0C6 RID: 57542 RVA: 0x000024DF File Offset: 0x000006DF
		public Landmark()
		{
		}

		// Token: 0x0600E0C7 RID: 57543 RVA: 0x0041F7A4 File Offset: 0x0041D9A4
		public Landmark(LandmarkDef def)
		{
			this.def = def;
		}

		// Token: 0x0600E0C8 RID: 57544 RVA: 0x0041F7B3 File Offset: 0x0041D9B3
		public void ExposeData()
		{
			Scribe_Defs.Look<LandmarkDef>(ref this.def, "def");
			Scribe_Values.Look<string>(ref this.name, "name", null, false);
			Scribe_Values.Look<bool>(ref this.isComboLandmark, "isComboLandmark", false, false);
		}

		// Token: 0x04009708 RID: 38664
		public LandmarkDef def;

		// Token: 0x04009709 RID: 38665
		public string name;

		// Token: 0x0400970A RID: 38666
		public bool isComboLandmark;
	}
}
