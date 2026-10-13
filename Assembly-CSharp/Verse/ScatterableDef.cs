using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020002DF RID: 735
	public class ScatterableDef : Def
	{
		// Token: 0x0600121F RID: 4639 RVA: 0x00054634 File Offset: 0x00052834
		public override void PostLoad()
		{
			base.PostLoad();
			if (this.defName == "UnnamedDef")
			{
				this.defName = "Scatterable_" + this.texturePath;
				base.ResolveDefNameHash();
			}
			LongEventHandler.ExecuteWhenFinished(delegate
			{
				GlobalTextureAtlasManager.TryInsertStatic(TextureAtlasGroup.Terrain, ContentFinder<Texture2D>.Get(this.texturePath, true), null);
			});
			LongEventHandler.ExecuteWhenFinished(delegate
			{
				this.mat = MaterialPool.MatFrom(this.texturePath, ShaderDatabase.Transparent);
			});
		}

		// Token: 0x04001053 RID: 4179
		[NoTranslate]
		public string texturePath;

		// Token: 0x04001054 RID: 4180
		public float minSize;

		// Token: 0x04001055 RID: 4181
		public float maxSize;

		// Token: 0x04001056 RID: 4182
		public float scatterChance = 0.5f;

		// Token: 0x04001057 RID: 4183
		[NoTranslate]
		public string scatterType = "";

		// Token: 0x04001058 RID: 4184
		public bool placeUnderNaturalRoofs = true;

		// Token: 0x04001059 RID: 4185
		public Material mat;
	}
}
