using System;

namespace Verse
{
	// Token: 0x02000478 RID: 1144
	public abstract class SectionLayer_Dynamic : SectionLayer
	{
		// Token: 0x06001E08 RID: 7688 RVA: 0x000028E7 File Offset: 0x00000AE7
		public virtual bool ShouldDrawDynamic(CellRect view)
		{
			return true;
		}

		// Token: 0x06001E09 RID: 7689 RVA: 0x0009AD92 File Offset: 0x00098F92
		public SectionLayer_Dynamic(Section section) : base(section)
		{
			this.section = section;
		}
	}
}
