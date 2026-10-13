using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x020007F5 RID: 2037
	public class CachedTexture
	{
		// Token: 0x170009CA RID: 2506
		// (get) Token: 0x06003408 RID: 13320 RVA: 0x001110A8 File Offset: 0x0010F2A8
		public Texture2D Texture
		{
			get
			{
				if (this.cachedTexture == null || this.validationIndex != CachedTexture.curValidationIndex)
				{
					if (this.texPath.NullOrEmpty())
					{
						this.cachedTexture = BaseContent.BadTex;
					}
					else
					{
						this.cachedTexture = (ContentFinder<Texture2D>.Get(this.texPath, true) ?? BaseContent.BadTex);
					}
					this.validationIndex = CachedTexture.curValidationIndex;
				}
				return this.cachedTexture;
			}
		}

		// Token: 0x06003409 RID: 13321 RVA: 0x00111116 File Offset: 0x0010F316
		public CachedTexture(string texPath)
		{
			this.texPath = texPath;
			this.cachedTexture = null;
			this.validationIndex = -1;
		}

		// Token: 0x0600340A RID: 13322 RVA: 0x00111133 File Offset: 0x0010F333
		public static void ResetStaticData()
		{
			CachedTexture.curValidationIndex++;
		}

		// Token: 0x0600340B RID: 13323 RVA: 0x00111144 File Offset: 0x0010F344
		public static Texture2D Get(string path)
		{
			CachedTexture cachedTexture;
			if (!CachedTexture.cachedTextures.TryGetValue(path, out cachedTexture))
			{
				cachedTexture = (CachedTexture.cachedTextures[path] = new CachedTexture(path));
			}
			return cachedTexture.Texture;
		}

		// Token: 0x04002743 RID: 10051
		private string texPath;

		// Token: 0x04002744 RID: 10052
		private Texture2D cachedTexture;

		// Token: 0x04002745 RID: 10053
		private int validationIndex;

		// Token: 0x04002746 RID: 10054
		private static int curValidationIndex = 0;

		// Token: 0x04002747 RID: 10055
		private static readonly Dictionary<string, CachedTexture> cachedTextures = new Dictionary<string, CachedTexture>(32);
	}
}
