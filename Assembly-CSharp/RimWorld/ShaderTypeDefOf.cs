using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020035E6 RID: 13798
	[DefOf]
	public static class ShaderTypeDefOf
	{
		// Token: 0x060133D4 RID: 78804 RVA: 0x005B5F9F File Offset: 0x005B419F
		static ShaderTypeDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(ShaderTypeDefOf));
		}

		// Token: 0x0400D3A9 RID: 54185
		public static ShaderTypeDef Cutout;

		// Token: 0x0400D3AA RID: 54186
		public static ShaderTypeDef CutoutHair;

		// Token: 0x0400D3AB RID: 54187
		public static ShaderTypeDef CutoutTiling;

		// Token: 0x0400D3AC RID: 54188
		public static ShaderTypeDef CutoutComplex;

		// Token: 0x0400D3AD RID: 54189
		public static ShaderTypeDef Transparent;

		// Token: 0x0400D3AE RID: 54190
		public static ShaderTypeDef MetaOverlay;

		// Token: 0x0400D3AF RID: 54191
		public static ShaderTypeDef EdgeDetect;

		// Token: 0x0400D3B0 RID: 54192
		public static ShaderTypeDef TerrainFadeRoughLinearAdd;

		// Token: 0x0400D3B1 RID: 54193
		public static ShaderTypeDef MoteGlow;

		// Token: 0x0400D3B2 RID: 54194
		[MayRequireBiotech]
		public static ShaderTypeDef TerrainFadeRoughLinearBurn;
	}
}
