using System;

namespace Verse
{
	// Token: 0x02000469 RID: 1129
	[Flags]
	public enum MeshParts : byte
	{
		// Token: 0x040019BC RID: 6588
		None = 0,
		// Token: 0x040019BD RID: 6589
		Verts = 1,
		// Token: 0x040019BE RID: 6590
		Tris = 2,
		// Token: 0x040019BF RID: 6591
		Colors = 4,
		// Token: 0x040019C0 RID: 6592
		UVs1 = 8,
		// Token: 0x040019C1 RID: 6593
		UVs2 = 16,
		// Token: 0x040019C2 RID: 6594
		Normals = 32,
		// Token: 0x040019C3 RID: 6595
		All = 63
	}
}
