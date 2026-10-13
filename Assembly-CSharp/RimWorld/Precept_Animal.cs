using System;

namespace RimWorld
{
	// Token: 0x0200265D RID: 9821
	public class Precept_Animal : Precept_ThingDef
	{
		// Token: 0x170020F7 RID: 8439
		// (get) Token: 0x0600D763 RID: 55139 RVA: 0x003FE40D File Offset: 0x003FC60D
		public override string UIInfoFirstLine
		{
			get
			{
				if (base.ThingDef == null)
				{
					return base.UIInfoFirstLine;
				}
				return base.ThingDef.LabelCap;
			}
		}

		// Token: 0x170020F8 RID: 8440
		// (get) Token: 0x0600D764 RID: 55140 RVA: 0x003FE42E File Offset: 0x003FC62E
		public override string TipLabel
		{
			get
			{
				return this.def.issue.LabelCap + ": " + base.ThingDef.LabelCap;
			}
		}
	}
}
