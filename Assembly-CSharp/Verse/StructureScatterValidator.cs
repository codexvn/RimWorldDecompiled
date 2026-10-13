using System;

namespace Verse
{
	// Token: 0x020004AC RID: 1196
	public class StructureScatterValidator : ScattererValidator
	{
		// Token: 0x06001F2F RID: 7983 RVA: 0x000A3BEB File Offset: 0x000A1DEB
		public override bool Allows(IntVec3 c, Map map)
		{
			return c.InHorDistOf(this.structureRect.CenterCell, this.maxDistFromStructure);
		}

		// Token: 0x04001ADD RID: 6877
		public CellRect structureRect;

		// Token: 0x04001ADE RID: 6878
		public float maxDistFromStructure;
	}
}
