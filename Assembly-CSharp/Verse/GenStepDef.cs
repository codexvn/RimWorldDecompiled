using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x02000280 RID: 640
	public class GenStepDef : Def
	{
		// Token: 0x06001022 RID: 4130 RVA: 0x0004BF23 File Offset: 0x0004A123
		public override void PostLoad()
		{
			base.PostLoad();
			this.genStep.def = this;
		}

		// Token: 0x04000CB6 RID: 3254
		public SitePartDef linkWithSite;

		// Token: 0x04000CB7 RID: 3255
		public float order;

		// Token: 0x04000CB8 RID: 3256
		public GenStep genStep;

		// Token: 0x04000CB9 RID: 3257
		public List<GenStepDef> preventsGenSteps;
	}
}
