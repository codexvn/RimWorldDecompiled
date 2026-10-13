using System;

namespace RimWorld.Planet
{
	// Token: 0x02003CE6 RID: 15590
	public struct TriangleIndices
	{
		// Token: 0x0601538A RID: 86922 RVA: 0x00659135 File Offset: 0x00657335
		public TriangleIndices(int v1, int v2, int v3)
		{
			this.v1 = v1;
			this.v2 = v2;
			this.v3 = v3;
		}

		// Token: 0x0601538B RID: 86923 RVA: 0x0065914C File Offset: 0x0065734C
		public bool SharesAnyVertexWith(TriangleIndices t, int otherThan)
		{
			return (this.v1 != otherThan && (this.v1 == t.v1 || this.v1 == t.v2 || this.v1 == t.v3)) || (this.v2 != otherThan && (this.v2 == t.v1 || this.v2 == t.v2 || this.v2 == t.v3)) || (this.v3 != otherThan && (this.v3 == t.v1 || this.v3 == t.v2 || this.v3 == t.v3));
		}

		// Token: 0x0601538C RID: 86924 RVA: 0x006591FB File Offset: 0x006573FB
		public int GetNextOrderedVertex(int root)
		{
			if (this.v1 == root)
			{
				return this.v2;
			}
			if (this.v2 == root)
			{
				return this.v3;
			}
			return this.v1;
		}

		// Token: 0x0400EC96 RID: 60566
		public int v1;

		// Token: 0x0400EC97 RID: 60567
		public int v2;

		// Token: 0x0400EC98 RID: 60568
		public int v3;
	}
}
