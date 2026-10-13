using System;

namespace Verse
{
	// Token: 0x02000471 RID: 1137
	public abstract class SectionLayer : MapDrawLayer
	{
		// Token: 0x06001DE3 RID: 7651 RVA: 0x00098978 File Offset: 0x00096B78
		public SectionLayer(Section section) : base(section.map)
		{
			this.section = section;
		}

		// Token: 0x06001DE4 RID: 7652 RVA: 0x0009898D File Offset: 0x00096B8D
		public override CellRect GetBoundaryRect()
		{
			return this.section.CellRect;
		}

		// Token: 0x040019E9 RID: 6633
		protected Section section;
	}
}
