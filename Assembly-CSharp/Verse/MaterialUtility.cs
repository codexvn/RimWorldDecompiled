using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000D37 RID: 3383
	public static class MaterialUtility
	{
		// Token: 0x0600561D RID: 22045 RVA: 0x001BCCDD File Offset: 0x001BAEDD
		public static Texture2D GetMaskTexture(this Material mat)
		{
			if (!mat.HasProperty(ShaderPropertyIDs.MaskTex))
			{
				return null;
			}
			return (Texture2D)mat.GetTexture(ShaderPropertyIDs.MaskTex);
		}

		// Token: 0x0600561E RID: 22046 RVA: 0x001BCCFE File Offset: 0x001BAEFE
		public static Color GetColorTwo(this Material mat)
		{
			if (!mat.HasProperty(ShaderPropertyIDs.ColorTwo))
			{
				return Color.white;
			}
			return mat.GetColor(ShaderPropertyIDs.ColorTwo);
		}
	}
}
