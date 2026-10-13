using System;

namespace RimWorld.Planet
{
	// Token: 0x02003DDE RID: 15838
	public class SitePartDefWithParams
	{
		// Token: 0x060159E0 RID: 88544 RVA: 0x0067897F File Offset: 0x00676B7F
		public SitePartDefWithParams(SitePartDef def, SitePartParams parms)
		{
			this.def = def;
			this.parms = parms;
		}

		// Token: 0x0400F02D RID: 61485
		public SitePartDef def;

		// Token: 0x0400F02E RID: 61486
		public SitePartParams parms;
	}
}
