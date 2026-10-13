using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020007F4 RID: 2036
	public class CachedMaterial
	{
		// Token: 0x170009C7 RID: 2503
		// (get) Token: 0x06003402 RID: 13314 RVA: 0x00110FC0 File Offset: 0x0010F1C0
		public Material Material
		{
			get
			{
				if (this.cachedMaterial == null || this.validationIndex != CachedMaterial.curValidationIndex)
				{
					if (this.texPath.NullOrEmpty())
					{
						this.cachedMaterial = BaseContent.BadMat;
					}
					else
					{
						this.cachedMaterial = MaterialPool.MatFrom(this.texPath, this.shader, this.color);
					}
					this.validationIndex = CachedMaterial.curValidationIndex;
				}
				return this.cachedMaterial;
			}
		}

		// Token: 0x170009C8 RID: 2504
		// (get) Token: 0x06003403 RID: 13315 RVA: 0x00111030 File Offset: 0x0010F230
		public string TexturePath
		{
			get
			{
				return this.texPath;
			}
		}

		// Token: 0x170009C9 RID: 2505
		// (get) Token: 0x06003404 RID: 13316 RVA: 0x00111038 File Offset: 0x0010F238
		public Color Color
		{
			get
			{
				return this.color;
			}
		}

		// Token: 0x06003405 RID: 13317 RVA: 0x00111040 File Offset: 0x0010F240
		public CachedMaterial(string texPath, Shader shader, Color color)
		{
			this.texPath = texPath;
			this.shader = shader;
			this.color = color;
			this.cachedMaterial = null;
			this.validationIndex = -1;
		}

		// Token: 0x06003406 RID: 13318 RVA: 0x0011106B File Offset: 0x0010F26B
		public CachedMaterial(string texPath, Shader shader)
		{
			this.texPath = texPath;
			this.shader = shader;
			this.color = Color.white;
			this.cachedMaterial = null;
			this.validationIndex = -1;
		}

		// Token: 0x06003407 RID: 13319 RVA: 0x0011109A File Offset: 0x0010F29A
		public static void ResetStaticData()
		{
			CachedMaterial.curValidationIndex++;
		}

		// Token: 0x0400273D RID: 10045
		private string texPath;

		// Token: 0x0400273E RID: 10046
		private Material cachedMaterial;

		// Token: 0x0400273F RID: 10047
		private Shader shader;

		// Token: 0x04002740 RID: 10048
		private Color color;

		// Token: 0x04002741 RID: 10049
		private int validationIndex;

		// Token: 0x04002742 RID: 10050
		private static int curValidationIndex;
	}
}
