using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000D36 RID: 3382
	[StaticConstructorOnStartup]
	public static class MatBases
	{
		// Token: 0x04003E42 RID: 15938
		public static readonly Material LightOverlay = MatLoader.LoadMat("Lighting/LightOverlay", -1);

		// Token: 0x04003E43 RID: 15939
		public static readonly Material SunShadow = MatLoader.LoadMat("Lighting/SunShadow", -1);

		// Token: 0x04003E44 RID: 15940
		public static readonly Material SunShadowFade = MatLoader.LoadMat("Lighting/SunShadowFade", -1);

		// Token: 0x04003E45 RID: 15941
		public static readonly Material EdgeShadow = MatLoader.LoadMat("Lighting/EdgeShadow", -1);

		// Token: 0x04003E46 RID: 15942
		public static readonly Material IndoorMask = MatLoader.LoadMat("Misc/IndoorMask", -1);

		// Token: 0x04003E47 RID: 15943
		public static readonly Material FilledMask = MatLoader.LoadMat("Misc/FilledMask", -1);

		// Token: 0x04003E48 RID: 15944
		public static readonly Material GravshipMask = MatLoader.LoadMat("Map/Gravship/GravshipMask", -1);

		// Token: 0x04003E49 RID: 15945
		public static readonly Material LightOverlayGravship = MatLoader.LoadMat("Lighting/LightOverlayGravship", -1);

		// Token: 0x04003E4A RID: 15946
		public static readonly Material ShadowMask = MatLoader.LoadMat("Misc/ShadowMask", -1);

		// Token: 0x04003E4B RID: 15947
		public static readonly Material RoofedOutdoorMask = MatLoader.LoadMat("Misc/RoofedOutdoorMask", -1);

		// Token: 0x04003E4C RID: 15948
		public static readonly Material FogOfWar = MatLoader.LoadMat("Misc/FogOfWar", -1);

		// Token: 0x04003E4D RID: 15949
		public static readonly Material Darkness = MatLoader.LoadMat("Misc/Darkness", -1);

		// Token: 0x04003E4E RID: 15950
		public static readonly Material Snow = MatLoader.LoadMat("Misc/Snow", -1);

		// Token: 0x04003E4F RID: 15951
		public static readonly Material DebugOverlay = MatLoader.LoadMat("Misc/DebugOverlay", -1);

		// Token: 0x04003E50 RID: 15952
		public static readonly Material Sand = MatLoader.LoadMat("Misc/Sand", -1);
	}
}
